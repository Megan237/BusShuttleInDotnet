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

    //Entry

    public IActionResult EntryView()
    {

        return View(this.entryService.GetEntries().Select(e => EntryViewModel.FromEntry(e)));

    }

    //Loop
    public IActionResult LoopView()
    {

        return View(this.loopService.GetLoops().Select(l => LoopViewModel.FromLoop(l)));

    }

    //Route

    public IActionResult RouteView()
    {

        return View(this.routeService.GetRoutes().Select(r => RouteViewModel.FromRoute(r)));

    }

    //Stop

    public IActionResult StopView()
    {

        return View(this.stopService.GetStops().Select(s => StopViewModel.FromStop(s)));

    }
}
