using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using sgc.ml.Dto;
using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Services.Interfaces;
using sgc.ml.Util;


namespace sgc.ml.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IUserService userservice;
        private readonly IAdmUsuarioRepository _admUsuarioRepository;
        private readonly IAdmRolRepository _admRolRepository;
        private readonly IAdmPreguntaRepository admPreguntaRepository;

        public SecurityController(IUserService userservice,
            IAdmUsuarioRepository _admUsuarioRepository,
            IAdmRolRepository _admRolRepository,
            IAdmPreguntaRepository admPreguntaRepository
        )
        {
            this.userservice = userservice;
            this._admUsuarioRepository = _admUsuarioRepository;
            this._admRolRepository = _admRolRepository;
            this.admPreguntaRepository = admPreguntaRepository;
        }


        public IActionResult Index()
        {
            return View("Login");
        }


        [HttpPost]
        public ActionResult Validate(DtoLogin model)
        {
            ViewBag.error_login = "Usuario no existe";

            if (!ModelState.IsValid)
            {
                ViewBag.error_login = ModelState.Errors();
                return RedirectToAction("Login", "Security", new { error = ViewBag.error_login });
            }

            AdmUsuario? usuario = _admUsuarioRepository.checkUsuarioAndPwd(model.username, model.pwd);

            if (!SgcFunctions.Isnull(usuario))
            {
                HttpContext.Session.SetString("Access", "true");
                HttpContext.Session.SetString("username", model.username);
                HttpContext.Session.SetString("nombres", usuario.nombres);
                HttpContext.Session.SetString("apellidos", usuario.apellidos);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Security", new { error = ViewBag.error_login });
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.roles = _admRolRepository.GetRol();
            ViewBag.preguntas = admPreguntaRepository.GetPreguntas();

            return View();
        }


        [HttpGet]
        public ActionResult Recover()
        {
            ViewBag.roles = _admRolRepository.GetRol();
            ViewBag.preguntas = admPreguntaRepository.GetPreguntas();

            return View();
        }

        [HttpPost]
        public IActionResult RegisterPost(UsuariosDTO model)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { result = false, msg = ModelState.Errors(), @event = "error" });

            if (!SgcFunctions.Isnull(_admUsuarioRepository.checkIfUsuarioExist(model.usuario)))
                return new JsonResult(new
                    { result = false, msg = "Usuario ingresado ya existe en el sistema", @event = "error" });

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _admUsuarioRepository.Persist(new AdmUsuario()
                    {
                        usuario_id = 0,
                        nombres = model.nombres,
                        apellidos = model.apellidos,
                        email = model.email,
                        pwd = model.pwd,
                        username = model.usuario,
                        rol = model.rol,
                        pregunta = model.pregunta,
                        respuesta = model.respuesta
                    });
                    scope.Complete();

                    return new JsonResult(new { result = true, msg = "Usuario creado con exito", @event = "success" });
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    return new JsonResult(new
                        { result = false, msg = "Ocurrio un error al crear el usuario", @event = "error" });
                }
            }
        }


        [HttpPost]
        public IActionResult RecoverPost(UsuarioRecoverDto model)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { result = false, msg = ModelState.Errors(), @event = "error" });

            AdmUsuario? usuario = _admUsuarioRepository.checkIfUsuarioExist(model.usuario);

            ;

            if (SgcFunctions.Isnull(usuario))
                return new JsonResult(new
                    { result = false, msg = "El usuario no existe en el sistema", @event = "error" });


            if (usuario.pregunta != model.pregunta)
                return new JsonResult(new
                    { result = false, msg = "La pregunta no coincide con la registrada", @event = "error" });

            if (!usuario.respuesta.Equals(model.respuesta))
                return new JsonResult(new
                    { result = false, msg = "La respuesta no coincide con la registrada", @event = "error" });


            var newpassword = Guid.NewGuid().ToString("d").Replace("-", "").Substring(1, 10);

            usuario.pwd = newpassword;
            _admUsuarioRepository.update(usuario);
            
            return new JsonResult(new
            {
                result = true, msg = "Password Validados Correctamente", value = usuario.pwd, @event = "success"
            });
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Access");
            return RedirectToAction("Login", "Security");
        }


        public IActionResult Login(string? error)
        {
            ViewBag.error_login = error;

            return View();
        }
    }
}