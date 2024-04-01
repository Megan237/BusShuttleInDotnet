using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;

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

        return View(this.entryService.GetEntries().Select(e => EntryViewModel.FromEntry(e)));

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
    public async Task<IActionResult> EntryCreate([Bind("TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry)
    {
        if (ModelState.IsValid)
        {
            this.entryService.CreateEntry(entry.TimeStamp, entry.Boarded, entry.LeftBehind);
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

        return View(this.routeService.GetRoutes().Select(r => RouteViewModel.FromRoute(r)));

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
    public async Task<IActionResult> RouteEdit(int id, [Bind("Order")] RouteEditModel route)
    {
        if (ModelState.IsValid)
        {
            this.routeService.UpdateRouteByID(id, route.Order);
            return RedirectToAction("RouteView");
        }
        else
        {
            return View(route);
        }
    }

    public IActionResult RouteCreate()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RouteCreate([Bind("Order")] RouteCreateModel route)
    {
        if (ModelState.IsValid)
        {
            this.routeService.CreateRoute(route.Order);
            return RedirectToAction("RouteView");
        }
        else
        {
            return View();
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

    // public IActionResult Index()
    // {
    //     return View();
    // }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("UserName, Password")] UserModel user)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("model is valid");
            if (this.userService.VerifyUserAsManager(user.UserName, user.Password))
            {

                return RedirectToAction("HomeView");
            }
            else
            {
                return RedirectToAction("Index");
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
