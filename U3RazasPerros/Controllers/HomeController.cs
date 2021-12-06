using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using U3RazasPerros.Models;
using U3RazasPerros.Models.ViewModels;

namespace U3RazasPerros.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(perrosContext context)
        {
            Context = context;
        }

        public perrosContext Context { get; }

        [Route("/")]
        [Route("Razas/{id?}")]
        public IActionResult Index(char? id)
        {
            IndexViewModel vm = new();

            vm.Perros = id == null
                ? Context.Razas.OrderBy(x => x.Nombre)
                : Context.Razas.Where(x=> EF.Functions.Like(x.Nombre, id+"%")).OrderBy(x => x.Nombre);
            vm.Iniciales = Context.Razas.Select(x => x.Nombre[0]).ToList().Distinct().OrderBy(x => x);

            return View(vm);
        }

        [Route("{id?}/Datos")]
        public IActionResult Raza(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Replace("-", " ");
                var raza = Context.Razas
                    .Include(x=>x.IdPaisNavigation)
                    .Include(x => x.Caracteristicasfisicas)
                    .FirstOrDefault(x => x.Nombre == id);
                if (raza == null)
                {
                    return RedirectToAction("Index");
                }

                return View(raza);
            }
            else{
                return RedirectToAction("Index");
            }
        }
    }
}
