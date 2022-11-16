using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DgBooksDB;

namespace DgBooksDetails.DgBServices
{
    public class libroServices
    {
        public List<Libro> GetLibros() 
        { 
            List<Libro> libros = new List<Libro>();

            try
            {
                using(var context=new DgBooksEntities())
                {
                    libros = context.Libro.ToList();
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
            }

            return libros;
        }

        public List<Libro> LibrosGenero(int id)
        {
            List<Libro> LibrosPG = new List<Libro>();
            try
            {
                using (var context = new DgBooksEntities())
                {
                    var genero = context.Genero.FirstOrDefault(p => p.intIdGenero == id);
               
                    var ListLibrosEn = context.Libro.ToList().Where(p => p.Genero.Contains(genero));

                    foreach (Libro item in ListLibrosEn)
                    {
                        LibrosPG.Add(item);

                    }

                    return LibrosPG;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Libro> GetLibrosBusqueda(string busqueda)
        {
            List<Libro> libros = new List<Libro>();
            List<Libro> librosBusqueda = new List<Libro>();

            try
            {
                using (var context = new DgBooksEntities())
                {
                    libros = context.Libro.ToList();

                    foreach (var libro in libros)
                    {
                        if (libro.NombreLibro.Contains(busqueda) || libro.strSinopsis.Contains(busqueda))
                        {
                            librosBusqueda.Add(libro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return librosBusqueda;
        }

        public Libro GetLibroById(int id)
        {
            Libro libro = new Libro();
            try
            {
                using(var context= new DgBooksEntities())
                {
                    libro = context.Libro.FirstOrDefault(p => p.intIdLibor == id);
                }
                if(libro == null)
                {
                    libro.intIdLibor = 0;
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
            }
            return libro;
        }

        public bool LibroUsuarioAgregar(Libro libro, Usuario usuario)
        {
            bool insertado = false;
            Libro libroEn = new Libro();
            Usuario usuarioEn = new Usuario();
            try
            {
                using (var context = new DgBooksEntities())
                {
                    libroEn = context.Libro.FirstOrDefault(p => p.intIdLibor == libro.intIdLibor);
                    usuarioEn = context.Usuario.FirstOrDefault(p => p.intIdUsuario == usuario.intIdUsuario);

                    if (usuarioEn.Libro.Contains(libroEn))
                    {
                        return insertado;
                    }

                    usuarioEn.Libro.Add(libroEn);
                    context.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        public bool LibroUsuarioEliminar(Libro libro, Usuario usuario)
        {
            bool Eliminado = false;
            Libro libroEn = new Libro();
            Usuario usuarioEn = new Usuario();
            try
            {
                using (var context = new DgBooksEntities())
                {
                    libroEn = context.Libro.FirstOrDefault(p => p.intIdLibor == libro.intIdLibor);
                    usuarioEn = context.Usuario.FirstOrDefault(p => p.intIdUsuario == usuario.intIdUsuario);

                    if (usuarioEn.Libro.FirstOrDefault(p => p.intIdLibor == libroEn.intIdLibor) == null)
                    {
                        return Eliminado;
                    }

                    usuarioEn.Libro.Remove(libroEn);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public List<Libro> LibrosFavs(int idU)
        {

            List<Libro> libros = new List<Libro>();
            try
            {
                using (var context = new DgBooksEntities())
                {
                    var UsuarioEn = context.Usuario.FirstOrDefault(p => p.intIdUsuario == idU);
                    libros = UsuarioEn.Libro.ToList();
                    return libros;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;

            }
        }

        public List<Libro> LibrosShop(int idU)
        {

            List<Libro> libros = new List<Libro>();
            try
            {
                using (var context = new DgBooksEntities())
                {
                    var UsuarioEn = context.Usuario.FirstOrDefault(p => p.intIdUsuario == idU);
                    libros = UsuarioEn.Libro.ToList();
                    return libros;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;

            }
        }

    }
}
