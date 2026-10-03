using System;
using System.Collections.Generic;
using System.IO;

namespace RegistroGastos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string archivo = "gastos.csv";
            string opcion;
            List<Gasto> listaGastos = Cargar(archivo);
            int idActual = 1;

            foreach (Gasto gasto in listaGastos)
            {
                if (gasto.Id >= idActual)
                {
                    idActual = gasto.Id + 1;
                }
            }

            do
            {
                opcion = LeerMenu();

                switch (opcion)
                {
                    case "1":
                        RegistrarGasto(listaGastos, ref idActual);
                        break;

                    case "2":
                        MostrarGastos(listaGastos);
                        break;

                    case "3":
                        BuscarCategoria(listaGastos);
                        break;

                    case "4":
                        Guardar(listaGastos, archivo);
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            while (opcion != "4");
        }

        private static string LeerMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== REGISTRO DE GASTOS ===");
            Console.WriteLine("1. Agregar gasto");
            Console.WriteLine("2. Listar gastos");
            Console.WriteLine("3. Buscar por categoría");
            Console.WriteLine("4. Salir");
            Console.Write("Elige una opción: ");

            return Console.ReadLine() ?? "";
        }

        private static void MostrarGastos(List<Gasto> lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados.");
                return;
            }

            decimal total = 0;

            Console.WriteLine("Id  Descripción          Monto      Categoría");
            Console.WriteLine("----------------------------------------------");

            foreach (Gasto gasto in lista)
            {
                Console.WriteLine(gasto);
                total += gasto.Monto;
            }

            Console.WriteLine();
            Console.WriteLine($"TOTAL GASTADO: Q {total:N2}");
        }

        private static void RegistrarGasto(List<Gasto> lista, ref int id)
        {
            Console.Write("Descripción: ");
            string detalle = Console.ReadLine() ?? "";
            detalle = detalle.Trim();

            Console.Write("Monto: ");
            bool montoValido = decimal.TryParse(
                Console.ReadLine(),
                out decimal cantidad);

            Console.Write("Categoría: ");
            string tipo = Console.ReadLine() ?? "";
            tipo = tipo.Trim();

            if (!montoValido
                || cantidad <= 0
                || string.IsNullOrWhiteSpace(detalle)
                || string.IsNullOrWhiteSpace(tipo))
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            lista.Add(new Gasto
            {
                Id = id,
                Descripcion = detalle,
                Monto = cantidad,
                Categoria = tipo
            });

            id++;

            Console.WriteLine("Gasto agregado.");
        }

        private static void BuscarCategoria(List<Gasto> lista)
        {
            Console.Write("Categoría a buscar: ");
            string categoriaBuscada = Console.ReadLine() ?? "";
            categoriaBuscada = categoriaBuscada.Trim().ToUpper();

            bool existe = false;

            foreach (Gasto gasto in lista)
            {
                if (gasto.Categoria.ToUpper().Contains(categoriaBuscada))
                {
                    Console.WriteLine(gasto);
                    existe = true;
                }
            }

            if (!existe)
            {
                Console.WriteLine("Sin coincidencias.");
            }
        }

        private static void CargarDatos(List<Gasto> lista, string archivo)
        {
            foreach (Gasto gasto in lista)
            {
                Console.WriteLine(gasto);
            }
        }

        private static void Guardar(List<Gasto> lista, string archivo)
        {
            var registros = new List<string>();

            foreach (Gasto gasto in lista)
            {
                registros.Add(
                    $"{gasto.Id},{gasto.Descripcion},{gasto.Monto},{gasto.Categoria}");
            }

            File.WriteAllLines(archivo, registros);

            Console.WriteLine(
                $"Gastos guardados en {archivo} ({lista.Count} registros).");
        }

        private static List<Gasto> Cargar(string archivo)
        {
            var lista = new List<Gasto>();

            if (!File.Exists(archivo))
            {
                return lista;
            }

            foreach (string registro in File.ReadAllLines(archivo))
            {
                var datos = registro.Split(',');

                if (datos.Length == 4)
                {
                    lista.Add(new Gasto
                    {
                        Id = int.Parse(datos[0]),
                        Descripcion = datos[1],
                        Monto = decimal.Parse(datos[2]),
                        Categoria = datos[3]
                    });
                }
            }

            Console.WriteLine(
                $"Cargados {lista.Count} gastos desde {archivo}.");

            return lista;
        }
    }
}