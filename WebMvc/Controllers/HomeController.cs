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
    }

    [Authorize(Roles = "Manager")]
    public IActionResult Report(string loopId, string busId, string stopId, string driverId, string day)
    {
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


        return View(entryViewModels);
    }

    public IActionResult Index()
    {

        return View();

    }
    [Authorize(Roles = "Manager")]
    public IActionResult HomeView()
    {

        return View();

    }
    public IActionResult DriverWaiting()
    {

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
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Driver")]
    public IActionResult DriverSignOn(DriverSignOnModel driverSignOn)
    {
        if (ModelState.IsValid)
        {
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
        if (ModelState.IsValid)
        {
            // Check if user is authenticated and retrieve the name
            if (User.Identity.IsAuthenticated)
            {
                var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (fullName != null)
                {
                    // Using the full name to get driver details
                    var driver = driverService.GetDriverByName(fullName);
                    _logger.LogInformation(driver.ToString());

                    entryService.CreateEntry(DateTime.Now, driverSignOn.Boarded, driverSignOn.LeftBehind, driverSignOn.BusId, driverSignOn.StopId, driver, driverSignOn.LoopId);
                    _logger.LogInformation("Entry Created at " + driverSignOn.TimeStamp);
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve driver's name from claims.");
                    // Handle error: the name claim is not found or the user might not be a driver
                }
            }
            else
            {
                _logger.LogWarning("User is not authenticated.");
                // Handle error: the user is not authenticated
            }
        }

        return View(driverSignOn);
    }
    //Bus
    [Authorize(Roles = "Manager")]
    public IActionResult BusView()
    {

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
            _logger.LogInformation("Bus with Id " + id + " removed");
            return RedirectToAction("BusView");
        }
        else
        {
            return View();
        }
    }

    [Authorize(Roles = "Manager")]
    public IActionResult BusEdit([FromRoute] int id)
    {
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
            _logger.LogInformation("Bus with id" + id + "was updated to " + bus);
            return RedirectToAction("BusView");
        }
        else
        {
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
            _logger.LogInformation("Bus was created with this bus number" + bus.BusNumber);
            return RedirectToAction("BusView");
        }
        else
        {
            return View();
        }
    }


    //Driver
    [Authorize(Roles = "Manager")]
    public IActionResult DriverView()
    {

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
            _logger.LogInformation("Driver with id " + id + "was deleted");
            return RedirectToAction("DriverView");
        }
        else
        {
            return View();
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult DriverEdit([FromRoute] int id)
    {
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
            _logger.LogInformation("Driver updated to " + driver);
            return RedirectToAction("DriverView");
        }
        else
        {
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
            _logger.LogInformation("New driver added as " + driver);

            return RedirectToAction("DriverView");
        }
        else
        {
            return View();
        }
    }


    //Entry
    [Authorize(Roles = "Manager")]
    public IActionResult EntryView()
    {
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
            // Assign other properties as necessary
        }).ToList();

        // Pass the RouteViewModel list to the view
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
            _logger.LogInformation("Entry with Id " + id + " was removed");
            return RedirectToAction("EntryView");
        }
        else
        {
            return View();
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult EntryEdit([FromRoute] int id)
    {
        var entry = this.entryService.FindEntryByID(id);
        var entryEditModel = EntryEditModel.FromEntry(entry);
        return View(entryEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult EntryEdit(int id, [Bind("TimeStamp, Boarded, LeftBehind")] EntryEditModel entry)
    {
        if (ModelState.IsValid)
        {
            this.entryService.UpdateEntryByID(id, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            _logger.LogInformation("Entry updated to " + entry);
            return RedirectToAction("EntryView");
        }
        else
        {
            return View(entry);
        }
    }

    public IActionResult EntryCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EntryCreate([Bind("TimeStamp, Boarded, LeftBehind, StopId, LoopId, DriverId, BusId")] EntryCreateModel entry)
    {
        if (ModelState.IsValid)
        {
            this.entryService.CreateEntry(entry.TimeStamp, entry.Boarded, entry.LeftBehind, entry.BusId, entry.StopId, entry.DriverId, entry.LoopId);
            _logger.LogInformation("Entry created with information " + entry);
            return RedirectToAction("EntryView");
        }
        else
        {
            return View();
        }
    }

    //Loop
    [Authorize(Roles = "Manager")]
    public IActionResult LoopView()
    {

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
            _logger.LogInformation("Loop with id " + id + "was removed");
            return RedirectToAction("LoopView");
        }
        else
        {
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
            _logger.LogInformation("Loop updated with this information " + loop.ToString());
            return RedirectToAction("LoopView");
        }
        else
        {
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
            return RedirectToAction("LoopView");
        }
        else
        {
            return View();
        }
    }



    //Route
    [Authorize(Roles = "Manager")]
    public IActionResult RouteView()
    {
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
            return RedirectToAction("RouteView");
        }
        else
        {
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
            return RedirectToAction("RouteView");
        }
        else
        {
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
            return RedirectToAction("StopView");
        }
        else
        {
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
            return RedirectToAction("StopView");
        }
        else
        {
            return View(stop);
        }
    }
    [Authorize(Roles = "Manager")]
    public IActionResult StopCreate()
    {
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
            return RedirectToAction("StopView");
        }
        else
        {
            return View();
        }
    }

    //Login


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index([Bind("UserName, Password")] UserModel user)
    {
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
                }
                else
                {
                    var driver = userService.VerifyUserAsDriver(user.UserName, user.Password);
                    if (driver != null)
                    {
                        fullName = driver.FirstName + " " + driver.LastName;
                        userRole = "Driver";
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
                return View(user);
            }
        }
        else
        {
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
        if (ModelState.IsValid)
        {
            this.userService.CreateUser(user.FirstName, user.LastName, user.UserName, user.Password);
            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }
}
