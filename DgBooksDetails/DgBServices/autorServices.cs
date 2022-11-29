using DgBooksDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DgBooksDetails.DgBServices
{
    public class autorServices
    {
        public Autor GetAutor(int id)
        {
            Autor autor = new Autor();

            try
            {
                using(var context=new DgBooksEntities())
                {
                    autor = context.Autor.FirstOrDefault(p=>p.intIdAutor==id);
                }
            }
            catch
            {

            }

            return autor;
        }

        public List<Persona> GetAutorBusqueda(string busqueda)
        {
            List<Persona> autores = new List<Persona>();
            List<Persona> autorBusqueda = new List<Persona>();

            try
            {
                using (var context = new DgBooksEntities())
                {
                    autores = context.Persona.ToList();

                    foreach (var autor in autores)
                    {
                        if (autor.strNombrePersona.Contains(busqueda) || autor.strApellidoP.Contains(busqueda))
                        {
                            autorBusqueda.Add(autor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return autorBusqueda;
        }

    }
}
