using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essalud_SJM
{
    public class Atencion
    {
        public string Dni { get; set; }
        public string Hc { get; set; }
        public string Nombres { get; set; }
        public string Especialidad { get; set; }
        public string Fecha { get; set; }

        public Atencion(string dni, string hc, string nombres, string especialidad, string fecha)
        {
            Dni = dni;
            Hc = hc;
            Nombres = nombres;
            Especialidad = especialidad;
            Fecha = fecha;
        }

        public string ALineaArchivo()
        {
            return $"{Dni};{Hc};{Nombres};{Especialidad};{Fecha}";
        }
    }
}