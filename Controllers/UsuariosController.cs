using Microsoft.AspNetCore.Mvc;
using Atica.Models.ViewModels;
using Atica.Services;
using Atica.Models;

namespace Atica.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;
        private const string RolActual = "Administrador";

        public UsuariosController(
            IUsuarioService usuarioService,
            ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerUsuariosAsync(RolActual);

                ViewBag.RolActual = RolActual;
                ViewBag.EsAdministrador = RolActual.EsAdministrador();

                return View(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el listado de usuarios");
                TempData["Error"] = "Error al cargar los usuarios. Por favor, intente nuevamente.";
                return View(new List<Models.Usuario>());
            }
        }

        public IActionResult Create()
        {
            ViewBag.EsAdministrador = RolActual.EsAdministrador();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }

            try
            {
                var (exito, mensaje, id) = await _usuarioService.CrearUsuarioAsync(model);

                if (exito)
                {
                    _logger.LogInformation("Usuario creado con ID: {Id}", id);
                    TempData["Success"] = mensaje;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, mensaje);
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                ModelState.AddModelError(string.Empty, "Error inesperado al crear el usuario.");
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id.Value);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Documento = usuario.Documento,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    Activo = usuario.Activo
                };

                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar usuario para editar");
                TempData["Error"] = "Error al cargar el usuario.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }

            try
            {
                var (exito, mensaje) = await _usuarioService.ActualizarUsuarioAsync(model);

                if (exito)
                {
                    _logger.LogInformation("Usuario actualizado con ID: {Id}", id);
                    TempData["Success"] = mensaje;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, mensaje);
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario");
                ModelState.AddModelError(string.Empty, "Error inesperado al actualizar el usuario.");
                ViewBag.EsAdministrador = RolActual.EsAdministrador();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (exito, mensaje) = await _usuarioService.EliminarUsuarioAsync(id);

                if (exito)
                {
                    _logger.LogInformation("Usuario eliminado con ID: {Id}", id);
                    TempData["Success"] = mensaje;
                }
                else
                {
                    TempData["Error"] = mensaje;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                TempData["Error"] = "Error inesperado al eliminar el usuario.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}