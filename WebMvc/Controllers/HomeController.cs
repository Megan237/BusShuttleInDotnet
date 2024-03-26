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


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
