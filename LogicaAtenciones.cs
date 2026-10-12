using System;
using System.Collections.Generic;
using Essalud_SJM;

namespace Essalud_SJM
{
    public static class LogicaAtenciones
    {
        public static (bool esValido, string mensaje) ValidarDatos(string dni, string hc)
        {
            if (dni.Length != 8 || !EsNumero(dni))
            {
                return (false, "Error: El DNI debe contener exactamente 8 dígitos numéricos.");
            }
            if (string.IsNullOrWhiteSpace(hc))
            {
                return (false, "Error: El Código de Historia Clínica no puede estar vacío.");
            }
            return (true, "OK");
        }

        public static bool EsNumero(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }

        public static bool ExisteDuplicado(List<Atencion> lista, string hc)
        {
            foreach (var reg in lista)
            {
                if (reg.Hc.Equals(hc, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Atencion> OrdenarPorBurbuja(List<Atencion> listaOriginal, string criterio)
        {
            List<Atencion> listaOrdenada = new List<Atencion>(listaOriginal);
            int n = listaOrdenada.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    bool intercambiar = false;

                    if (criterio.ToLower() == "fecha")
                    {
                        if (string.Compare(listaOrdenada[j].Fecha, listaOrdenada[j + 1].Fecha, StringComparison.Ordinal) > 0)
                        {
                            intercambiar = true;
                        }
                    }
                    else if (criterio.ToLower() == "hc")
                    {
                        if (string.Compare(listaOrdenada[j].Hc, listaOrdenada[j + 1].Hc, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            intercambiar = true;
                        }
                    }

                    if (intercambiar)
                    {
                        Atencion aux = listaOrdenada[j];
                        listaOrdenada[j] = listaOrdenada[j + 1];
                        listaOrdenada[j + 1] = aux;
                    }
                }
            }
            return listaOrdenada;
        }
    }
}