using System;
using System.Collections.Generic;
using Essalud_SJM;

namespace Essalud_SJM
{
    class Program
    {
        private const string NOMBRE_ARCHIVO = "atenciones_salud.txt";

        static void Main(string[] args)
        {
            List<Atencion> listaAtenciones = ArchivoManager.CargarArchivo(NOMBRE_ARCHIVO);
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("\n==============================================");
                Console.WriteLine("  SISTEMA DE ATENCIONES - CENTRO DE SALUD (C#)");
                Console.WriteLine("==============================================");
                Console.WriteLine("1. Registrar nueva atención médica");
                Console.WriteLine("2. Buscar atención por DNI o Historia Clínica");
                Console.WriteLine("3. Ordenar por Método Burbuja (retorna lista)");
                Console.WriteLine("4. Guardar datos en el archivo (.txt)");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==============================================");
                Console.Write("Seleccione una opción (1-5): ");

                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        RegistrarAtencion(listaAtenciones);
                        break;
                    case "2":
                        BuscarAtencion(listaAtenciones);
                        break;
                    case "3":
                        MenuOrdenarBurbuja(listaAtenciones);
                        break;
                    case "4":
                        ArchivoManager.GuardarArchivo(listaAtenciones, NOMBRE_ARCHIVO);
                        break;
                    case "5":
                        Console.Write("¿Desea guardar los cambios antes de salir? (s/n): ");
                        string resp = Console.ReadLine()?.Trim().ToLower();
                        if (resp == "s")
                        {
                            ArchivoManager.GuardarArchivo(listaAtenciones, NOMBRE_ARCHIVO);
                        }
                        Console.WriteLine("\n¡Gracias por utilizar el sistema! Hasta luego.");
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("\n[!] Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        private static void RegistrarAtencion(List<Atencion> lista)
        {
            Console.WriteLine("\n--- REGISTRO DE NUEVA ATENCIÓN MÉDICA ---");
            string dni, hc;

            while (true)
            {
                Console.Write("Ingrese DNI del paciente (8 dígitos): ");
                dni = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Ingrese Código HC (ej. HC-2026-001): ");
                hc = Console.ReadLine()?.Trim() ?? "";

                var (esValido, mensaje) = LogicaAtenciones.ValidarDatos(dni, hc);
                if (esValido) break;

                Console.WriteLine($"[!] {mensaje}");
            }

            if (LogicaAtenciones.ExisteDuplicado(lista, hc))
            {
                Console.WriteLine("\n[!] ERROR: El código de Historia Clínica ya se encuentra registrado.");
                return;
            }

            Console.Write("Ingrese Nombres y Apellidos: ");
            string nombres = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrEmpty(nombres)) nombres = "NO REGISTRADO";

            Console.Write("Ingrese Especialidad Médica: ");
            string especialidad = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(especialidad)) especialidad = "General";

            Console.Write("Ingrese Fecha (AAAA-MM-DD): ");
            string fecha = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(fecha)) fecha = "2026-01-01";

            Atencion nuevaAtencion = new Atencion(dni, hc.ToUpper(), nombres, especialidad, fecha);
            lista.Add(nuevaAtencion);

            Console.WriteLine("\n¡Registro guardado correctamente en memoria!");
        }

        private static void BuscarAtencion(List<Atencion> lista)
        {
            Console.WriteLine("\n--- BÚSQUEDA DE ATENCIÓN MÉDICA ---");
            if (lista.Count == 0)
            {
                Console.WriteLine("La lista está vacía.");
                return;
            }

            Console.Write("Ingrese Código HC o DNI a consultar: ");
            string criterio = Console.ReadLine()?.Trim().ToUpper() ?? "";
            bool encontrado = false;

            foreach (var reg in lista)
            {
                if (reg.Hc.ToUpper() == criterio || reg.Dni == criterio)
                {
                    Console.WriteLine("\n========================================");
                    Console.WriteLine("        FICHA CLINICA ENCONTRADA");
                    Console.WriteLine("========================================");
                    Console.WriteLine($"DNI:                 {reg.Dni}");
                    Console.WriteLine($"Historia Clínica:    {reg.Hc}");
                    Console.WriteLine($"Paciente:            {reg.Nombres}");
                    Console.WriteLine($"Especialidad:        {reg.Especialidad}");
                    Console.WriteLine($"Fecha de Atención:   {reg.Fecha}");
                    Console.WriteLine("========================================");
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                Console.WriteLine("\n[!] No se encontró ningún registro con el criterio ingresado.");
            }
        }

        private static void MenuOrdenarBurbuja(List<Atencion> lista)
        {
            Console.WriteLine("\n--- ORDENAR ATENCIONES (MÉTODO BURBUJA) ---");
            if (lista.Count == 0)
            {
                Console.WriteLine("La lista de atenciones está vacía.");
                return;
            }

            Console.WriteLine("1. Ordenar por Fecha de Atención");
            Console.WriteLine("2. Ordenar por Código de Historia Clínica (HC)");
            Console.Write("Seleccione la opción: ");
            string op = Console.ReadLine()?.Trim();

            string criterio = null;
            switch (op)
            {
                case "1":
                    criterio = "fecha";
                    break;
                case "2":
                    criterio = "hc";
                    break;
                default:
                    criterio = null;
                    break;
            }

            if (criterio == null)
            {
                Console.WriteLine("Opción no válida.");
                return;
            }

            List<Atencion> resultadoOrdenado = LogicaAtenciones.OrdenarPorBurbuja(lista, criterio);

            Console.WriteLine($"\n¡Lista ordenada por '{criterio.ToUpper()}' mediante Burbuja exitosamente!");
            MostrarTabla(resultadoOrdenado);
        }

        private static void MostrarTabla(List<Atencion> lista)
        {
            Console.WriteLine("\n" + new string('-', 85));
            Console.WriteLine($"{"DNI",-10} | {"HISTORIA C.",-14} | {"PACIENTE",-25} | {"ESPECIALIDAD",-15} | {"FECHA",-10}");
            Console.WriteLine(new string('-', 85));
            foreach (var reg in lista)
            {
                string nom = reg.Nombres.Length > 24 ? reg.Nombres.Substring(0, 24) : reg.Nombres;
                string esp = reg.Especialidad.Length > 14 ? reg.Especialidad.Substring(0, 14) : reg.Especialidad;
                Console.WriteLine($"{reg.Dni,-10} | {reg.Hc,-14} | {nom,-25} | {esp,-15} | {reg.Fecha,-10}");
            }
            Console.WriteLine(new string('-', 85));
        }
    }
}