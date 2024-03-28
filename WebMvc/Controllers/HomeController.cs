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

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService, LoopServiceInterface loopService, DriverServiceInterface driverService, EntryServiceInterface entryService, RouteServiceInterface routeService, StopServiceInterface stopService)
    {
        _logger = logger;
        this.busService = busService;
        this.loopService = loopService;
        this.driverService = driverService;
        this.entryService = entryService;
        this.routeService = routeService;
        this.stopService = stopService;
    }

    public IActionResult Index()
    {

        return View();

    }

    //Bus

    public IActionResult BusView()
    {

        return View(this.busService.GetBusses().Select(b => BusViewModel.FromBus(b)));

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
}
