using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;
using Microsoft.AspNetCore.Mvc.Rendering;

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


    public IActionResult Index()
    {

        return View();

    }

    public IActionResult HomeView()
    {

        return View();

    }
    public IActionResult DriverWaiting()
    {

        return View();

    }

    // Driver screens
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

    //Bus

    public IActionResult BusView()
    {

        return View(this.busService.GetBusses().Select(b => BusViewModel.FromBus(b)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BusDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.busService.DeleteBus(id);
            return RedirectToAction("BusView");
        }
        else
        {
            return View();
        }
    }

    //This name needs to be same as View
    public IActionResult BusEdit([FromRoute] int id)
    {
        var bus = this.busService.FindBusByID(id);
        var busEditModel = BusEditModel.FromBus(bus);
        return View(busEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusEdit(int id, [Bind("BusNumber")] BusEditModel bus)
    {
        if (ModelState.IsValid)
        {
            this.busService.UpdateBusByID(id, bus.BusNumber);
            return RedirectToAction("BusView");
        }
        else
        {
            return View(bus);
        }
    }

    public IActionResult BusCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusCreate([Bind("BusNumber")] BusCreateModel bus)
    {
        if (ModelState.IsValid)
        {
            this.busService.CreateBus(bus.BusNumber);
            return RedirectToAction("BusView");
        }
        else
        {
            return View();
        }
    }


    //Driver
    public IActionResult DriverView()
    {

        return View(this.driverService.GetDrivers().Select(d => DriverViewModel.FromDriver(d)));

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DriverDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.driverService.DeleteDriver(id);
            return RedirectToAction("DriverView");
        }
        else
        {
            return View();
        }
    }
    public IActionResult DriverEdit([FromRoute] int id)
    {
        var driver = this.driverService.FindDriverByID(id);
        var driverEditModel = DriverEditModel.FromDriver(driver);
        return View(driverEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverEdit(int id, [Bind("FirstName, LastName")] DriverEditModel driver)
    {
        if (ModelState.IsValid)
        {
            this.driverService.UpdateDriverByID(id, driver.FirstName, driver.LastName);
            return RedirectToAction("DriverView");
        }
        else
        {
            return View(driver);
        }
    }

    public IActionResult DriverCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverCreate([Bind("FirstName, LastName")] DriverCreateModel driver)
    {
        if (ModelState.IsValid)
        {
            this.driverService.CreateDriver(driver.FirstName, driver.LastName);
            return RedirectToAction("DriverView");
        }
        else
        {
            return View();
        }
    }


    //Entry

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
    public IActionResult EntryDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.entryService.DeleteEntry(id);
            return RedirectToAction("EntryView");
        }
        else
        {
            return View();
        }
    }

    public IActionResult EntryEdit([FromRoute] int id)
    {
        var entry = this.entryService.FindEntryByID(id);
        var entryEditModel = EntryEditModel.FromEntry(entry);
        return View(entryEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EntryEdit(int id, [Bind("TimeStamp, Boarded, LeftBehind")] EntryEditModel entry)
    {
        if (ModelState.IsValid)
        {
            this.entryService.UpdateEntryByID(id, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
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
    public async Task<IActionResult> EntryCreate([Bind("TimeStamp, Boarded, LeftBehind, StopId, LoopId, DriverId, BusId")] EntryCreateModel entry)
    {
        if (ModelState.IsValid)
        {
            this.entryService.CreateEntry(entry.TimeStamp, entry.Boarded, entry.LeftBehind, entry.BusId, entry.StopId, entry.DriverId, entry.LoopId);
            return RedirectToAction("EntryView");
        }
        else
        {
            return View();
        }
    }

    //Loop
    public IActionResult LoopView()
    {

        return View(this.loopService.GetLoops().Select(l => LoopViewModel.FromLoop(l)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LoopDelete(int id)
    {
        if (ModelState.IsValid)
        {
            this.loopService.DeleteLoop(id);
            return RedirectToAction("LoopView");
        }
        else
        {
            return View();
        }
    }
    public IActionResult LoopEdit([FromRoute] int id)
    {
        var loop = this.loopService.FindLoopByID(id);
        var loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoopEdit(int id, [Bind("Name")] LoopEditModel loop)
    {
        if (ModelState.IsValid)
        {
            this.loopService.UpdateLoopByID(id, loop.Name);
            return RedirectToAction("LoopView");
        }
        else
        {
            return View(loop);
        }
    }

    public IActionResult LoopCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoopCreate([Bind("Name")] LoopCreateModel loop)
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



    public IActionResult RouteEdit([FromRoute] int id)
    {
        var route = this.routeService.FindRouteByID(id);
        var routeEditModel = RouteEditModel.FromRoute(route);
        return View(routeEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RouteEdit(int id, [Bind("Order, StopId, LoopId")] RouteEditModel route)
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
    public async Task<IActionResult> RouteCreate([Bind("Order, StopId, LoopId")] RouteCreateModel route)
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

    public IActionResult StopView()
    {

        return View(this.stopService.GetStops().Select(s => StopViewModel.FromStop(s)));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
    public IActionResult StopEdit([FromRoute] int id)
    {
        var stop = this.stopService.FindStopByID(id);
        var stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StopEdit(int id, [Bind("Name, Latitude, Longitude")] StopEditModel stop)
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

    public IActionResult StopCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StopCreate([Bind("Name, Latitude, Longitude")] StopCreateModel stop)
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
            Console.WriteLine("model is valid");
            if (this.userService.VerifyUser(user.UserName, user.Password))
            {


                if (this.userService.VerifyUserAsManager(user.UserName, user.Password))
                {

                    return RedirectToAction("HomeView");
                }
                else
                {
                    if (this.userService.VerifyUserAsDriver(user.UserName, user.Password))
                    {
                        return RedirectToAction("DriverSignOn");
                    }
                    else
                    {
                        return RedirectToAction("DriverWaiting");
                    }
                }

            }

            else
            {
                return View();
            }

        }
        else
        {
            return View();
        }
    }

    //Register

    public IActionResult RegisterView()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterView([Bind("FirstName, LastName, UserName, Password")] UserModel user)
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
