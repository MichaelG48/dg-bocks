using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DgBooksDetails.DgBServices;
using System.Web.Mvc;
using DgBooksDB;
using DgBooks.Models;

namespace DgBooks.Controllers
{
    public class LibrosController : Controller
    {
        
        libroServices services = new libroServices();
        usuarioServices usuarioServices = new usuarioServices();
        autorServices autorServices = new autorServices();
        personaServices personaServices = new personaServices();
        generoServises generoServis = new generoServises();

        public LibrosController()
        {
            if (!ValidateSession())
            {
                RedirectToAction("ErrorPage", "Home");
            }
        }

        // GET: Libros
        public ActionResult Index()
        {
            return RedirectToAction("index","Home");
        }
        
        public bool ValidateSession()
        {
            try
            {
                int loginId = MySession.Current.IdUsuario;

                if (loginId == 0)
                {
                    MySession.Current.MensageGeneral = "Es necesario que vuelva ingresar al sistema.";
                    MySession.Current.TipoMensage = "error";
                    //return RedirectToAction("ErrorPage", "Home");
                    return false;
                }
            }
            catch (NullReferenceException Ex)
            {
                MySession.Current.MensageGeneral = "Es necesario que vuelva ingresar al sistema.";
                MySession.Current.TipoMensage = "error";
                //return RedirectToAction("ErrorPage", "Home");
                return false;
            }
            return true;
        }

        [HttpGet]
        public ActionResult PantallaP()
        {
            MySession.Current.MensageGeneral = "";
            MySession.Current.TipoMensage = "";

            try
            {
                if (MySession.Current.Usuario == null) { 
                    if ((int)MySession.Current.IdUsuario > 0)
                    {
                        MySession.Current.Usuario = usuarioServices.GetUsuarioById((int)MySession.Current.IdUsuario);
                    }
                }

                List<Libro> List = new List<Libro>();

                if (Session["IdGenero"] == null)
                {
                    Session["IdGenero"] = 0;
                    List = services.GetLibros();
                }
                else
                {
                    if ((int)Session["IdGenero"] == 0)
                    {
                        List = services.GetLibros();
                    }
                    else
                    {
                        List = services.LibrosGenero((int)Session["IdGenero"]);
                        Session["IdGenero"] = 0;
                    }
                }

                ViewBag.User = MySession.Current.Usuario;
                return View(List);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                MySession.Current.MensageGeneral = "Error al cargar la ventana principal, intenta nuevamente.";
                MySession.Current.TipoMensage = "error";
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        public ActionResult PantallaP(string intIdLibor)
        {
            MySession.Current.MensageGeneral = "";
            MySession.Current.TipoMensage = "";

            MySession.Current.intIdLibor = Convert.ToInt32(intIdLibor);
            return RedirectToAction("PantallaDetalles", "Libros");
        }

        [HttpPost]
        public ActionResult Busqueda(string inpBusqueda)
        {

            var ListBusqueda = services.GetLibrosBusqueda(inpBusqueda);
            if (ListBusqueda.Count == 0)
            {
                MySession.Current.MensageGeneral = "Sin resultados";
                MySession.Current.TipoMensage = "info";
            }
            else
            {
                MySession.Current.MensageGeneral = "";
                MySession.Current.TipoMensage = "";
            }

            return View(ListBusqueda);
        }
       



        [HttpGet]
        public ActionResult PantallaDetalles()
        {
            try
            {
                int idLibro = (int)MySession.Current.intIdLibor;
                var libro = services.GetLibroById((int)MySession.Current.intIdLibor);
                var autor = autorServices.GetAutor(int.Parse(libro.intIdAutor.ToString()));
                var persona = personaServices.GetPersona(int.Parse(autor.idPersona.ToString()));

                ViewBag.Autor = persona;
                ViewBag.User = MySession.Current.Usuario;
                ViewBag.Year = int.Parse(libro.dtFechaPublicacion.Value.Year.ToString());

                return View(libro);
            }
            catch (Exception ex)
            {
                MySession.Current.MensageGeneral = "Error al obtener los detalles del libro, intenta nuevamente.";
                MySession.Current.TipoMensage = "error";
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public ActionResult PantallaLectura(int id, int idU)
        {
            MySession.Current.MensageGeneral = "";
            MySession.Current.TipoMensage = "";

            try
            {
                var libro=services.GetLibroById(id);
                var user = usuarioServices.GetUsuarioById(idU);
                ViewBag.User = user;
                return View(libro);
            }
            catch
            {
                MySession.Current.MensageGeneral = "Error al visualizar el libro, intenta nuevamente.";
                MySession.Current.TipoMensage = "error";
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        #region Favoritos

        [HttpGet]
        public ActionResult PantallaFavoritos()
        {
            try
            {
                var librosFavs = services.LibrosFavs((int)MySession.Current.IdUsuario);
                
                if (librosFavs.Count == 0)
                {
                    MySession.Current.MensageGeneral = "Sin registros";
                    MySession.Current.TipoMensage = "info";
                }
                return View(librosFavs);
            }
            catch (Exception ex)
            {
                MySession.Current.MensageGeneral = "Error al visualizar sus favoritos, intenta nuevamente.";
                MySession.Current.TipoMensage = "error";
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        public ActionResult PantallaFavoritos(string intIdLibor)
        {
            MySession.Current.intIdLibor = Convert.ToInt32(intIdLibor);
            return RedirectToAction("PantallaDetalles", "Libros");
        }

        public ActionResult AgregarFavorito(int idLibro)
        {
            try
            {
                var user = usuarioServices.GetUsuarioById((int)MySession.Current.IdUsuario);
                var libro = services.GetLibroById(idLibro);
                var List = services.GetLibros();
                var registro = services.LibroUsuarioAgregar(libro, user);

                MySession.Current.MensageGeneral = "Favorito agregado a su lista.";
                MySession.Current.TipoMensage = "exito";
                return RedirectToAction("PantallaDetalles");
            }
            catch
            {
                MySession.Current.MensageGeneral = "Error al agregar su favorito, intenta nuevamente.";
                MySession.Current.TipoMensage = "error";
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarFavorito(int intIdLibor)
        {
            try
            {
                var user = usuarioServices.GetUsuarioById((int)MySession.Current.IdUsuario);
                var libro = services.GetLibroById(intIdLibor);
                var List = services.GetLibros();
                var registro = services.LibroUsuarioEliminar(libro, user);

                if (registro)
                {
                    MySession.Current.MensageGeneral = "El libro ha sido eliminado de sus favoritos.";
                    MySession.Current.TipoMensage = "exito";
                } else
                {
                    MySession.Current.MensageGeneral = "No se pudo eliminar de su lista de favoritos, intente nuevamente.";
                    MySession.Current.TipoMensage = "error";
                }

                return RedirectToAction("PantallaFavoritos");
            }
            catch
            {
                MySession.Current.MensageGeneral = "Ocurrió un error al intentar eliminar.";
                MySession.Current.TipoMensage = "error";

                return RedirectToAction("PantallaFavoritos");
            }
        }

        #endregion
    }
}