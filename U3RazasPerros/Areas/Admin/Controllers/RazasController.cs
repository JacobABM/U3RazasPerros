using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using U3RazasPerros.Areas.Admin.Models;
using U3RazasPerros.Models;
using System.Threading.Tasks;

namespace U3RazasPerros.Areas.Controllers
{
    [Area("Admin")]
    public class RazasController : Controller
    {
        public perrosContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public RazasController(perrosContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }
        public IActionResult Index()
        {
            IEnumerable<Razas> razas = Context.Razas.OrderBy(x => x.Nombre);
            return View(razas);
        }
        public IActionResult Agregar()
        {
            return View(new RPerrosViewModel { Paises = Context.Paises.OrderBy(x => x.Nombre) });
        }

        [HttpPost]
        public IActionResult Agregar(RPerrosViewModel vm, IFormFile imagen)
        {
            if (!Verificacion(vm, out string m))
            {
                ModelState.AddModelError("", m);
                vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                return View(vm);
            }
            if (imagen != null)
            {
                if (imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite la carga de archivos JPG");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
                if (imagen.Length > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "No se permite la carga de archivos mayores a 5MB");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
            }
            Context.Add(vm.Raza);
            Context.SaveChanges();

            if (imagen != null)
            {
                var path = Host.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg";
                FileStream fs = new FileStream(path, FileMode.Create);
                imagen.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            Razas raza = Context.Razas.FirstOrDefault(x => x.Id == id);
            if (raza == null)
            {
                return RedirectToAction("Index");
            }
            return View(raza);
        }

        [HttpPost]
        public IActionResult Eliminar(Razas vm)
        {
            Razas raza = Context.Razas.FirstOrDefault(x => x.Id == vm.Id);
            Caracteristicasfisicas caracteristicas = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == vm.Id);
            if (raza == null || caracteristicas == null)
            {
                ModelState.AddModelError("", "No ha sido posible realizar está acción");
                return View(vm);
            }

            Context.Remove(caracteristicas);
            Context.Remove(raza);
            Context.SaveChanges();

            string path = Host.WebRootPath + "/imgs_perros/" + raza.Id + "_0.jpg";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Razas raza = Context.Razas.FirstOrDefault(x => x.Id == id);
            Caracteristicasfisicas c = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == id);
            if (raza == null)
            {
                return RedirectToAction("Index");
            }
            RPerrosViewModel vm = new RPerrosViewModel
            {
                Raza = raza,
                Paises = Context.Paises.OrderBy(x => x.Nombre)
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(RPerrosViewModel vm, IFormFile imagen)
        {
            var raza = Context.Razas.Include(x => x.Caracteristicasfisicas).FirstOrDefault(x => x.Id == vm.Raza.Id);
            if (raza == null)
            {
                return RedirectToAction("Index");
            }
            if (!Verificacion(vm, out string m))
            {
                ModelState.AddModelError("", m);
                vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                return View(vm);
            }
            if (imagen != null)
            {
                if (imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "formato no permitido, utilice imágenes JPG");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
                if (imagen.Length > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "Su archivo debe pesar menos de 5MB");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
            }


            raza.Nombre = vm.Raza.Nombre;
            raza.OtrosNombres = vm.Raza.OtrosNombres;
            raza.IdPais = vm.Raza.IdPais;
            raza.AlturaMin = vm.Raza.AlturaMin;
            raza.AlturaMax = vm.Raza.AlturaMax;
            raza.PesoMin = vm.Raza.PesoMin;
            raza.PesoMax = vm.Raza.PesoMax;
            raza.EsperanzaVida = vm.Raza.EsperanzaVida;

            raza.Caracteristicasfisicas.Cola = vm.Raza.Caracteristicasfisicas.Cola;
            raza.Caracteristicasfisicas.Color = vm.Raza.Caracteristicasfisicas.Color;
            raza.Caracteristicasfisicas.Hocico = vm.Raza.Caracteristicasfisicas.Hocico;
            raza.Caracteristicasfisicas.Patas = vm.Raza.Caracteristicasfisicas.Patas;
            raza.Caracteristicasfisicas.Pelo = vm.Raza.Caracteristicasfisicas.Pelo;

            Context.Update(raza);
            Context.SaveChanges();
            if (imagen != null)
            {
                var path = Host.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg";
                FileStream fs = new FileStream(path, FileMode.Create);
                imagen.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Index");
        }

       

        private bool Verificacion(RPerrosViewModel vm, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(vm.Raza.Nombre))
            {
                mensaje = "Agregar un nombre";
                return false;
            }
            if (string.IsNullOrWhiteSpace(vm.Raza.OtrosNombres))
            {
                vm.Raza.OtrosNombres = "No tiene";
            }
            if (string.IsNullOrWhiteSpace(vm.Raza.Descripcion))
            {
                mensaje = "Agregar una Descrpción";
                return false;
            }
            if (vm.Raza.PesoMin == 0)
            {
                mensaje = "Cual es el peso mínimo";
                return false;
            }
            if (vm.Raza.PesoMax == 0)
            {
                mensaje = "Cual es el peso máximo";
                return false;
            }
            if (vm.Raza.PesoMin >= vm.Raza.PesoMax)
            {
                mensaje = "El peso mínimo tiene que ser menor al peso máximo";
                return false;
            }
            if (vm.Raza.AlturaMin == 0)
            {
                mensaje = "Cual es la altura mínima";
                return false;
            }
            if (vm.Raza.AlturaMax == 0)
            {
                mensaje = "Cual es la altura máxima";
                return false;
            }
            if (vm.Raza.AlturaMin >= vm.Raza.AlturaMax)
            {
                mensaje = "La altura máxima debe ser mayor a la altura mínima";
                return false;
            }
            if (vm.Raza.EsperanzaVida == 0)
            {
                mensaje = "Agregar una esperanza de vida";
                return false;
            }


            mensaje = "";
            return true;
        }
    }
}


