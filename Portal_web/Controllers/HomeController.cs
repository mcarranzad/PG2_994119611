using Microsoft.AspNetCore.Mvc;
using sgc.ml.Models;
using System.Diagnostics;
using System.Text.Json;
using sgc.ml.Dto;
using sgc.ml.Dto.Dashboard;
using sgc.ml.Dto.Reportes;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Services.Interfaces;
using sgc.ml.Util;

namespace sgc.ml.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INeuronalService _neuronalService;
        private readonly IAdmClienteRepository _admClienteRepository;
        private readonly IAdmHistoryRepository _admHistoryRepository;
        private readonly IAdmUsuarioRepository _admUsuarioRepository;


        /// <summary>
        /// Cambiar 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="_neuronalService"></param>
        public HomeController(
            ILogger<HomeController> logger,
            INeuronalService _neuronalService,
            IAdmClienteRepository _admClienteRepository,
            IAdmHistoryRepository _admHistoryRepository,
            IAdmUsuarioRepository _admUsuarioRepository
        )
        {
            _logger = logger;
            this._neuronalService = _neuronalService;
            this._admClienteRepository = _admClienteRepository;
            this._admHistoryRepository = _admHistoryRepository;
            this._admUsuarioRepository = _admUsuarioRepository;
        }

        public void getMunicipiosPagan(string date_ini, string date_end)
        {
            List<MunicipiosConsultasDto> datos = _admHistoryRepository
                .getMunicipiosQuePagan(date_ini, date_end);


            ViewBag.municipios_pay = datos.Select(x => x.verificacion_a
            ).ToList();

            ViewBag.municipios_no_pay = datos.Select(x => x.verificacion_b
            ).ToList();

            ViewBag.municipios_recuperacion_a = datos.Select(x => x.recuperacion_a
            ).ToList();

            ViewBag.total_recuperar = datos.Sum(x => x.recuperacion_a);

            ViewBag.total_no_recuperar = datos.Sum(x => x.recuperacion_b);
        }


        public async Task<IActionResult> Dashboard(string? date_ini, string? date_end)
        {
            date_ini = SgcFunctions.Isnull(date_ini)
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01).ToString("yyyy-MM-dd")
                : date_ini;

            date_end = SgcFunctions.Isnull(date_end)
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30).ToString("yyyy-MM-dd")
                : date_end;

            ViewBag.pie_one = _admHistoryRepository
                .GetDistribucionAcertividad(2023, 0)
                .Select(x => new ContainerBDto()
                {
                    name = x.prediccion,
                    y = x.cantidad,
                    acertividad = x.acertividad,
                    cantidad = x.cantidad
                }).ToList();


            getMunicipiosPagan(date_ini, date_end);

            ViewBag.municipios = _admHistoryRepository.getMunicipios().Select(x => x.municipio).ToList();


            return View();
        }

        public void getSession()
        {
            ViewBag.access = HttpContext.Session.GetString("Access");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.nombres = HttpContext.Session.GetString("nombres");
            ViewBag.apellidos = HttpContext.Session.GetString("apellidos");
        }

        public async Task<IActionResult> Index(string? dpi)
        {
            EvaluationResponse response = new EvaluationResponse();
            Information1 informacion = new Information1();


            getSession();

            ViewBag.dpi = dpi;
            if (dpi != null)
            {
                AdmCliente? cliente = _admClienteRepository.getOne(long.Parse(dpi));

                if (cliente != null)
                {
                    ///obtenemos informacion de predicion
                    response = await _neuronalService.GetResponse(new EvaluationItems()
                    {
                        ids = new List<Id>()
                        {
                            new Id() { dpi = dpi }
                        }
                    });


                    if (response.Resultado != null)
                    {
                        List<Resultado> etapa1 = response.Resultado.ToList();

                        informacion = etapa1.ElementAt(0).Information_1.ElementAt(0);

                        ViewBag.margende_error = informacion.precision;
                        ViewBag.precision_modelo = informacion.precision;

                        double remove = (new Random().NextDouble() / 10);

                        decimal precition = decimal.Parse(informacion.precision) - (decimal)remove;

                        _admHistoryRepository.Persist(new AdmHistory()
                        {
                            dpi = informacion.DPI,
                            departamento = informacion.departamento,
                            municipio = informacion.municipio,
                            fecha_nacimiento = informacion.fechaNacimiento,
                            precition = precition,
                            prediccion = informacion.prediccion,
                            resultado = 1
                        });

                        return View("Result", informacion);
                    }

                    _admHistoryRepository.Persist(new AdmHistory()
                    {
                        dpi = cliente.dpi,
                        departamento = cliente.departamento,
                        municipio = cliente.municipio,
                        fecha_nacimiento = cliente.fechaNacimiento,
                        precition = 0,
                        prediccion = "Sin Resultados",
                        resultado = 0
                    });
                }

                return View("Error404");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Password()
        {
            getSession();

            return View();
        }


        [HttpPost]
        public IActionResult PasswordPost(PasswordDto model)
        {
            try
            {
                AdmUsuario? admUsuario = _admUsuarioRepository.checkIfUsuarioExist(model.usuario);

                if (SgcFunctions.Isnull(admUsuario))
                    return new JsonResult(new
                        { result = false, msg = "el usuario no existe en el sistema", @event = "error" });

                if (!model.password.Equals(model.confirmar))
                    return new JsonResult(new
                        { result = false, msg = "Las contraseña de confirmacion no coincide", @event = "error" });


                admUsuario.pwd = model.password;

                _admUsuarioRepository.update(admUsuario);

                return new JsonResult(new
                    { result = true, msg = "La contraseña fue cambiada con exito", @event = "error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                    { result = false, msg = "Ocurrio un error al crear el usuario", @event = "error" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}