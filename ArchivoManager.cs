using System;
using System.Collections.Generic;
using System.IO;
using Essalud_SJM;

namespace Essalud_SJM
{
    public static class ArchivoManager
    {
        public static List<Atencion> CargarArchivo(string ruta)
        {
            List<Atencion> lista = new List<Atencion>();
            if (!File.Exists(ruta))
            {
                Console.WriteLine("-> No se encontró archivo previo. Se iniciará con una lista vacía.");
                return lista;
            }

            try
            {
                using (StreamReader sr = new StreamReader(ruta))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        linea = linea.Trim();
                        if (!string.IsNullOrEmpty(linea))
                        {
                            string[] datos = linea.Split(';');
                            if (datos.Length == 5)
                            {
                                lista.Add(new Atencion(datos[0], datos[1], datos[2], datos[3], datos[4]));
                            }
                        }
                    }
                }
                Console.WriteLine($"-> Datos cargados exitosamente ({lista.Count} registros).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar archivo: {ex.Message}");
            }

            return lista;
        }

        public static void GuardarArchivo(List<Atencion> lista, string ruta)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ruta, false))
                {
                    foreach (var reg in lista)
                    {
                        sw.WriteLine(reg.ALineaArchivo());
                    }
                }
                Console.WriteLine("\n¡Datos guardados exitosamente en el archivo de texto!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar archivo: {ex.Message}");
            }
        }
    }
}