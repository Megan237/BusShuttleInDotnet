using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    BusServiceInterface busService;
    LoopServiceInterface loopService;
    DriverServiceInterface driverService;
    EntryServiceInterface entryService;
    RouteServiceInterface routeService;
    StopServiceInterface stopService;
    UserServiceInterface userService;

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService, LoopServiceInterface loopService, DriverServiceInterface driverService, EntryServiceInterface entryService, RouteServiceInterface routeService, StopServiceInterface stopService, UserServiceInterface userService)
    {
        _logger = logger;
        this.busService = busService;
        this.loopService = loopService;
        this.driverService = driverService;
        this.entryService = entryService;
        this.routeService = routeService;
        this.stopService = stopService;
        this.userService = userService;
        _logger.LogInformation("HomeController initialized with all dependencies.");
    }

    [Authorize(Roles = "Manager")]
    public IActionResult Report(string loopId, string busId, string stopId, string driverId, string day)
    {
        _logger.LogDebug("Starting Report generation.");
        var loops = loopService.GetLoops().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.Name,
        }).ToList();
        loops.Insert(0, new SelectListItem
        {
            Text = "",
            Value = ""
        });
        ViewBag.AvailableLoops = loops;

        var busses = busService.GetBusses().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.BusNumber.ToString()
        }).ToList();
        busses.Insert(0, new SelectListItem
        {
            Text = "",
            Value = ""
        });
        ViewBag.AvailableBusses = busses;

        var stops = stopService.GetStops().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.Name
        }).ToList();
        stops.Insert(0, new SelectListItem
        {
            Text = "",
            Value = ""
        });
        ViewBag.AvailableStops = stops;

        var drivers = driverService.GetDrivers().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.FirstName + " " + l.LastName,
        }).ToList();
        drivers.Insert(0, new SelectListItem
        {
            Text = "",
            Value = ""
        });
        ViewBag.AvailableDrivers = drivers;

        var entries = entryService.GetEntryDetails();

        if (!string.IsNullOrEmpty(loopId))
        {
            entries = entries.Where(e => e.LoopId == int.Parse(loopId)).ToList();
        }

        if (!string.IsNullOrEmpty(busId))
        {
            entries = entries.Where(e => e.BusId == int.Parse(busId)).ToList();
        }

        if (!string.IsNullOrEmpty(stopId))
        {
            entries = entries.Where(e => e.StopId == int.Parse(stopId)).ToList();
        }

        if (!string.IsNullOrEmpty(driverId))
        {
            entries = entries.Where(e => e.DriverId == int.Parse(driverId)).ToList();
        }

        if (!string.IsNullOrEmpty(day))
        {
            entries = entries.Where(e => e.TimeStamp.Date.Equals(DateTime.Parse(day).Date)).ToList();
        }

        var entryViewModels = entries.Select(dto => new EntryViewModel
        {
            Id = dto.Id,
            TimeStamp = dto.TimeStamp,
            Boarded = dto.Boarded,
            LeftBehind = dto.LeftBehind,
            StopName = dto.StopName,
            LoopName = dto.LoopName,
            DriverName = dto.DriverName,
            BusNumber = dto.BusNumber
            // Assign other properties as necessary
        }).ToList();
        _logger.LogInformation("Report generated successfully.");

        return View(entryViewModels);
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Accessed Index page.");
        return View();

    }
    [Authorize(Roles = "Manager")]
    public IActionResult HomeView()
    {
        _logger.LogInformation("Accessed HomeView for Managers.");
        return View();

    }
    public IActionResult DriverWaiting()
    {
        _logger.LogInformation("Accessed DriverWaiting page.");
        return View();

    }

    // Driver screens
    [Authorize(Roles = "Driver")]
    public IActionResult DriverSignOn()
    {
        var loops = loopService.GetLoops().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(), // Assuming 'Id' is the loop identifier in your loop entity
            Text = l.Name // And 'Name' is the property you want to display in the dropdown
        }).ToList();
        ViewBag.AvailableLoops = loops;

        var busses = busService.GetBusses().Select(b => new SelectListItem
        {
            Value = b.Id.ToString(), // Assuming 'Id' is the loop identifier in your loop entity
            Text = b.BusNumber.ToString() // And 'Name' is the property you want to display in the dropdown
        }).ToList();

        ViewBag.AvailableBusses = busses;
        _logger.LogInformation("Accessed DriverSignOn page.");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Driver")]
    public IActionResult DriverSignOn(DriverSignOnModel driverSignOn)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Driver signed on successfully. Redirecting to DriverScreen.");
            return RedirectToAction("DriverScreen", new { busId = driverSignOn.BusId, loopId = driverSignOn.LoopId });
        }
        _logger.LogError("Failed entry start validation at {time}.", DateTime.Now);
        return View(driverSignOn);
    }
    [Authorize(Roles = "Driver")]
    public IActionResult DriverScreen(int busId, int loopId)
    {
        var stops = entryService.GetAvailableStops(loopId).Select(l => new SelectListItem
        {
            Value = l.Id.ToString(), // Assuming 'Id' is the loop identifier in your loop entity
            Text = l.StopName // And 'Name' is the property you want to display in the dropdown
        }).ToList();
        ViewBag.AvailableStops = stops;
        _logger.LogInformation("DriverScreen displayed with bus ID {BusId} and loop ID {LoopId}.", busId, loopId);
        return View(new DriverScreenModel
        {
            BusId = busId,
            LoopId = loopId
        });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Driver")]
    public IActionResult DriverScreen(DriverScreenModel driverSignOn)
    {
        _logger.LogInformation("Processing DriverScreen with model data.");
        if (ModelState.IsValid)
        {
            _logger.LogDebug("Model state is valid. Proceeding with user authentication check.");
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogDebug("User is authenticated. Retrieving user's full name from claims.");
                var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (fullName != null)
                {
                    _logger.LogInformation("Successfully retrieved full name: {FullName}. Fetching driver details.", fullName);
                    var driver = driverService.GetDriverByName(fullName);
                    _logger.LogInformation(driver.ToString());

                    entryService.CreateEntry(DateTime.Now, driverSignOn.Boarded, driverSignOn.LeftBehind, driverSignOn.BusId, driverSignOn.StopId, driver, driverSignOn.LoopId);
                    _logger.LogInformation("Entry created successfully at {TimeStamp}.", DateTime.Now);
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve driver's name from claims.");
                }
            }
            else
            {
                _logger.LogWarning("User is not authenticated.");
            }
        }

        return View(driverSignOn);
    }
    //Bus
    [Authorize(Roles = "Manager")]
    public IActionResult BusView()
    {
        _logger.LogDebug("Fetching bus list for view.");

        return View(this.busService.GetBusses().Select(b => BusViewModel.FromBus(b)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult BusDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.busService.DeleteBus(id);
            _logger.LogInformation("Bus with Id {BusId} removed.", id);
            return RedirectToAction("BusView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to delete bus with Id {BusId}.", id);
            return View();
        }
    }

    [Authorize(Roles = "Manager")]
    public IActionResult BusEdit([FromRoute] int id)
    {
        _logger.LogDebug("Editing bus with Id {BusId}.", id);
        var bus = this.busService.FindBusByID(id);
        var busEditModel = BusEditModel.FromBus(bus);

        return View(busEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult BusEdit(int id, [Bind("BusNumber")] BusEditModel bus)
    {
        if (ModelState.IsValid)
        {
            this.busService.UpdateBusByID(id, bus.BusNumber);
            _logger.LogInformation("Bus with id {BusId} was updated to {BusDetails}.", id, bus);
            return RedirectToAction("BusView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to update bus with Id {BusId}.", id);
            return View(bus);
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult BusCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult BusCreate([Bind("BusNumber")] BusCreateModel bus)
    {
        if (ModelState.IsValid)
        {
            this.busService.CreateBus(bus.BusNumber);
            _logger.LogInformation("New bus was created with Bus Number {BusNumber}.", bus.BusNumber);
            return RedirectToAction("BusView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to create a new bus.");
            return View();
        }
    }


    //Driver
    [Authorize(Roles = "Manager")]
    public IActionResult DriverView()
    {
        _logger.LogDebug("Fetching driver list for view.");

        return View(this.driverService.GetDrivers().Select(d => DriverViewModel.FromDriver(d)));

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult DriverDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.driverService.DeleteDriver(id);
            _logger.LogInformation("Driver with id {DriverId} was deleted.", id);
            return RedirectToAction("DriverView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to delete driver with id {DriverId}.", id);
            return View();
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult DriverEdit([FromRoute] int id)
    {
        _logger.LogDebug("Editing driver with Id {DriverId}.", id);
        var driver = this.driverService.FindDriverByID(id);
        var driverEditModel = DriverEditModel.FromDriver(driver);
        return View(driverEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult DriverEdit(int id, [Bind("FirstName, LastName")] DriverEditModel driver)
    {
        if (ModelState.IsValid)
        {
            this.driverService.UpdateDriverByID(id, driver.FirstName, driver.LastName);
            _logger.LogInformation("Driver with id {DriverId} updated to FirstName: {FirstName}, LastName: {LastName}", id, driver.FirstName, driver.LastName);
            return RedirectToAction("DriverView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to update driver with Id {DriverId}.", id);
            return View(driver);
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult DriverCreate()
    {
        var drivers = driverService.GetDrivers().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.FirstName + " " + l.LastName
        }).ToList();

        ViewBag.AvailableDrivers = drivers;

        // Create a set of driver names for fast lookup
        var driverNames = new HashSet<string>(drivers.Select(d => d.Text));

        var users = userService.GetUsers().Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.FirstName + " " + u.LastName
        })
        .Where(u => u.Value != "1" && !driverNames.Contains(u.Text)) // Exclude users with ID 0 and all matching drivers
        .ToList();

        ViewBag.AvailableUsers = users;

        return View();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult DriverCreate([Bind("UserId")] DriverCreateModel driver)
    {
        if (ModelState.IsValid)
        {
            var FirstName = userService.FindUserByID(driver.UserId).FirstName;
            var LastName = userService.FindUserByID(driver.UserId).LastName;

            this.driverService.CreateDriver(FirstName, LastName);
            _logger.LogInformation("New driver added with FirstName: {FirstName}, LastName: {LastName}", FirstName, LastName);

            return RedirectToAction("DriverView");
        }
        else
        {
            _logger.LogWarning("Model state is invalid when attempting to create a new driver.");
            return View();
        }
    }


    //Entry
    [Authorize(Roles = "Manager")]
    public IActionResult EntryView()
    {
        _logger.LogDebug("Fetching entry details for view.");
        var entryDetailsDto = entryService.GetEntryDetails();

        // Convert RouteDetailDTO list to RouteViewModel list
        var entryViewModels = entryDetailsDto.Select(dto => new EntryViewModel
        {
            Id = dto.Id,
            TimeStamp = dto.TimeStamp,
            Boarded = dto.Boarded,
            LeftBehind = dto.LeftBehind,
            StopName = dto.StopName,
            LoopName = dto.LoopName,
            DriverName = dto.DriverName,
            BusNumber = dto.BusNumber
        }).ToList();
        _logger.LogInformation("Fetched {Count} entries for display.", entryViewModels.Count);
        return View(entryViewModels);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult EntryDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.entryService.DeleteEntry(id);
            _logger.LogInformation("Entry with Id {EntryId} was removed", id);
            return RedirectToAction("EntryView");
        }
        else
        {
            _logger.LogWarning("Attempted to delete entry with Id {EntryId}, but model state is invalid.", id);
            return View();
        }
    }

    //Loop
    [Authorize(Roles = "Manager")]
    public IActionResult LoopView()
    {
        _logger.LogDebug("Fetching loop details for view.");
        return View(this.loopService.GetLoops().Select(l => LoopViewModel.FromLoop(l)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult LoopDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.loopService.DeleteLoop(id);
            _logger.LogInformation("Loop with id {LoopId} was removed", id);
            return RedirectToAction("LoopView");
        }
        else
        {
            _logger.LogWarning("Attempted to delete loop with id {LoopId}, but model state is invalid.", id);
            return View();
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult LoopEdit([FromRoute] int id)
    {
        var loop = this.loopService.FindLoopByID(id);
        var loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult LoopEdit(int id, [Bind("Name")] LoopEditModel loop)
    {
        if (ModelState.IsValid)
        {
            this.loopService.UpdateLoopByID(id, loop.Name);
            _logger.LogInformation("Loop with id {LoopId} was updated with new name: {Name}", id, loop.Name);
            return RedirectToAction("LoopView");
        }
        else
        {
            _logger.LogWarning("Attempted to update loop with id {LoopId}, but model state is invalid.", id);
            return View(loop);
        }
    }

    [Authorize(Roles = "Manager")]
    public IActionResult LoopCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult LoopCreate([Bind("Name")] LoopCreateModel loop)
    {
        if (ModelState.IsValid)
        {
            this.loopService.CreateLoop(loop.Name);
            _logger.LogInformation("New loop was created with name: {Name}", loop.Name);
            return RedirectToAction("LoopView");
        }
        else
        {
            _logger.LogWarning("Failed to create new loop because model state is invalid.");
            return View();
        }
    }



    //Route
    [Authorize(Roles = "Manager")]
    public IActionResult RouteView()
    {
        _logger.LogDebug("Fetching route details for view.");
        var routeDetailsDto = routeService.GetRouteDetails();

        // Convert RouteDetailDTO list to RouteViewModel list
        var routeViewModels = routeDetailsDto.Select(dto => new RouteViewModel
        {
            Id = dto.Id,
            Order = dto.Order,
            StopName = dto.StopName,
            LoopName = dto.LoopName,
            // Assign other properties as necessary
        }).ToList();
        _logger.LogInformation("Fetched {Count} routes for display.", routeViewModels.Count);
        // Pass the RouteViewModel list to the view
        return View(routeViewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult RouteDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.routeService.DeleteRoute(id);
            _logger.LogInformation("Route with id {RouteId} was deleted.", id);
            return RedirectToAction("RouteView");
        }
        else
        {
            _logger.LogWarning("Failed to delete route with id {RouteId}; model state invalid.", id);
            return View();
        }
    }


    [Authorize(Roles = "Manager")]
    public IActionResult RouteEdit([FromRoute] int id)
    {
        var route = this.routeService.FindRouteByID(id);
        var routeEditModel = RouteEditModel.FromRoute(route);
        return View(routeEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult RouteEdit(int id, [Bind("Order, StopId, LoopId")] RouteEditModel route)
    {
        if (ModelState.IsValid)
        {
            this.routeService.UpdateRouteByID(id, route.Order, route.StopId, route.LoopId);
            _logger.LogInformation("Route with id {RouteId} was updated.", id);
            return RedirectToAction("RouteView");
        }
        else
        {
            _logger.LogWarning("Failed to update route with id {RouteId}; model state invalid.", id);
            return View(route);
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult RouteCreate()
    {
        var loops = loopService.GetLoops().Select(l => new SelectListItem
        {
            Value = l.Id.ToString(), // Assuming 'Id' is the loop identifier in your loop entity
            Text = l.Name // And 'Name' is the property you want to display in the dropdown
        }).ToList();

        ViewBag.AvailableLoops = loops;

        var stops = stopService.GetStops().Select(s => new SelectListItem
        {
            Value = s.Id.ToString(), // Assuming 'Id' is the loop identifier in your loop entity
            Text = s.Name // And 'Name' is the property you want to display in the dropdown
        }).ToList();

        ViewBag.AvailableStops = stops;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult RouteCreate([Bind("Order, StopId, LoopId")] RouteCreateModel route)
    {
        if (ModelState.IsValid)
        {
            var routeCount = routeService.GetRoutes().Count;
            this.routeService.CreateRoute(routeCount + 1, route.StopId, route.LoopId);

            return RedirectToAction("RouteView");
        }
        else
        {
            _logger.LogWarning("Failed to create route; model state invalid. Route details: {Details}", route);
            // Repopulate the dropdown list in case of validation failure to ensure the dropdown is still populated when the view is returned
            var loops = loopService.GetLoops().Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            }).ToList();

            ViewBag.AvailableLoops = loops;

            var stops = stopService.GetStops().Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            ViewBag.AvailableStops = stops;
            return View(route);
        }
    }



    //Stop
    [Authorize(Roles = "Manager")]
    public IActionResult StopView()
    {
        _logger.LogDebug("Fetching all stops for viewing.");
        return View(this.stopService.GetStops().Select(s => StopViewModel.FromStop(s)));

    }

    [Authorize(Roles = "Manager")]
    public IActionResult MapView()
    {
        _logger.LogDebug("Fetching all stops for Map viewing.");
        return View(this.stopService.GetStops().Select(s => StopViewModel.FromStop(s)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult StopDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.stopService.DeleteStop(id);
            _logger.LogInformation("Stop with id {StopId} deleted successfully.", id);
            return RedirectToAction("StopView");
        }
        else
        {
            _logger.LogWarning("Failed to delete stop with id {StopId} due to invalid model state.", id);
            return View();
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult StopEdit([FromRoute] int id)
    {
        var stop = this.stopService.FindStopByID(id);
        var stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult StopEdit(int id, [Bind("Name, Latitude, Longitude")] StopEditModel stop)
    {
        if (ModelState.IsValid)
        {
            this.stopService.UpdateStopByID(id, stop.Name, stop.Latitude, stop.Longitude);
            _logger.LogInformation("Stop with id {StopId} updated successfully.", id);
            return RedirectToAction("StopView");
        }
        else
        {
            _logger.LogWarning("Failed to update stop with id {StopId} due to invalid model state.", id);
            return View(stop);
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult StopCreate()
    {
        _logger.LogDebug("Navigated to Stop creation page.");
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult StopCreate([Bind("Name, Latitude, Longitude")] StopCreateModel stop)
    {
        if (ModelState.IsValid)
        {
            this.stopService.CreateStop(stop.Name, stop.Latitude, stop.Longitude);
            _logger.LogInformation("New stop created: {Name}", stop.Name);
            return RedirectToAction("StopView");
        }
        else
        {
            _logger.LogWarning("Failed to create new stop due to invalid model state.");
            return View();
        }
    }

    //Login


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index([Bind("UserName, Password")] UserModel user)
    {
        _logger.LogDebug("Attempting to log in user with username: {Username}", user.UserName);
        if (ModelState.IsValid)
        {
            if (userService.VerifyUser(user.UserName, user.Password))
            {
                var userRole = string.Empty;
                var fullName = string.Empty;
                var claims = new List<Claim>();

                if (userService.VerifyUserAsManager(user.UserName, user.Password))
                {
                    var manager = userService.FindUserByID(1); // Ensure this is the correct user
                    fullName = manager.FirstName + " " + manager.LastName;
                    userRole = "Manager";
                    _logger.LogInformation("Manager {FullName} logged in successfully.", fullName);
                }
                else
                {
                    var driver = userService.VerifyUserAsDriver(user.UserName, user.Password);
                    if (driver != null)
                    {
                        fullName = driver.FirstName + " " + driver.LastName;
                        userRole = "Driver";
                        _logger.LogInformation("Driver {FullName} logged in successfully.", fullName);
                    }
                }

                claims.Add(new Claim(ClaimTypes.Name, fullName));
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect based on role
                switch (userRole)
                {
                    case "Manager":
                        return RedirectToAction("HomeView");
                    case "Driver":
                        return RedirectToAction("DriverSignOn");
                    default:
                        return RedirectToAction("DriverWaiting");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                _logger.LogWarning("Login attempt failed for user {Username}.", user.UserName);
                return View(user);
            }
        }
        else
        {
            _logger.LogWarning("Login attempt with invalid model state for user {Username}.", user.UserName);
            return View(user);
        }
    }


    //Register

    public IActionResult RegisterView()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RegisterView([Bind("FirstName, LastName, UserName, Password")] UserModel user)
    {
        _logger.LogDebug("Attempting to register a new user with username: {Username}", user.UserName);
        if (ModelState.IsValid)
        {
            this.userService.CreateUser(user.FirstName, user.LastName, user.UserName, user.Password);
            _logger.LogInformation("New user {FullName} registered successfully.", user.FirstName + " " + user.LastName);
            return RedirectToAction("Index");
        }
        else
        {
            _logger.LogWarning("Registration attempt failed due to invalid model state for user {Username}.", user.UserName);
            return View();
        }
    }
}
