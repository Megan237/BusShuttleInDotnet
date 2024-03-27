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

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService)
    {
        _logger = logger;
        this.busService = busService;
    }

    public IActionResult Index()
    {

        return View(this.busService.GetBusses().Select(b => BusViewModel.FromBus(b)));

    }
    //This name needs to be ame as View
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
            return RedirectToAction("Index");
        }
        else
        {
            return View(bus);
        }
    }
}
