using Microsoft.AspNetCore.Mvc;
using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Services.Interfaces;
using sgc.ml.Util;

namespace sgc.ml.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INeuronalService _neuronalService;
    private readonly IAdmClienteRepository _admClienteRepository;
    private readonly IAdmHistoryRepository _admHistoryRepository;


    // GET
    /// <summary>
    /// Cambiar 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="_neuronalService"></param>
    public ReportController(
        ILogger<HomeController> logger,
        INeuronalService _neuronalService,
        IAdmClienteRepository _admClienteRepository,
        IAdmHistoryRepository _admHistoryRepository
    )
    {
        _logger = logger;
        this._neuronalService = _neuronalService;
        this._admClienteRepository = _admClienteRepository;
        this._admHistoryRepository = _admHistoryRepository;
    }

    
    public void getSession()
    {
        ViewBag.access = HttpContext.Session.GetString("Access");
        ViewBag.username = HttpContext.Session.GetString("username");
        ViewBag.nombres = HttpContext.Session.GetString("nombres");
        ViewBag.apellidos = HttpContext.Session.GetString("apellidos");
    }

    public IActionResult Index(string? date_ini, string? date_end)
    {
        getSession();
        
        @ViewBag.date_ini = SgcFunctions.Isnull(date_ini) ? DateTime.Now.ToString("yyyy-MM-dd") : date_ini;
        @ViewBag.date_end = SgcFunctions.Isnull(date_end) ? DateTime.Now.ToString("yyyy-MM-dd") : date_end;

        List<AdmHistory> histories = _admHistoryRepository.GetAll(@ViewBag.date_ini, @ViewBag.date_end);

        return View(histories);
    }
}