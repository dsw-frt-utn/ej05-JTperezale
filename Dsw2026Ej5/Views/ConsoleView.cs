namespace Dsw2026Ej5.Views;

public class ConsoleView
{
    private static List<VehiculoViewModel> _vehiculos = Controlador.GetVehiculos();
    public static void DibujarMenu()
    {
        string? opcion = null;
        do
        {
            LimpiarPantalla();
            DibujarLinea();
            CentrarTexto("Menú Principal - Empresa de Transporte", out int _);
            DibujarLinea();
            Console.WriteLine("Elija una opción: \n");
            Console.WriteLine("1. Listar vehículos");
            Console.WriteLine("2. Agregar vehículo");
            Console.WriteLine("3. Salir");
            Console.WriteLine("\n");
            Console.WriteLine("Ingrese su opción: ");
            opcion = Console.ReadLine();
            if (opcion == "1")
            {
                Console.WriteLine("Listando vehículos...");
                ListarVehiculos();
            }
            else if (opcion == "2")
            {
                Console.WriteLine("Agregando vehículo...");
                    AgregarVehiculo();
                    _vehiculos = Controlador.GetVehiculos(); // Refresca la lista
                
            }
        }
        while (opcion != "3");
    }
    public static void CentrarTexto(string? texto, out int usado, int? ancho = null, bool salto = true)
    {
        texto ??= string.Empty;
        ancho ??= Console.WindowWidth;
        int largo = texto.Length;
        if (largo > ancho)
        {
            largo = ancho.Value;
            texto = texto.Substring(0, ancho.Value);
        }
        int espacios = (ancho.Value - largo) / 2;
        espacios = espacios % 2 == 0 ? espacios : espacios + 1;
        string fin = salto ? "\n" : string.Empty;
        string final = new string(' ', espacios) + texto + fin;
        Console.Write(final);
        usado = final.Length;
    }
    public static void LimpiarPantalla()
    {
        Console.Clear();
    }

    public static void DibujarLinea()
    {
        var with = Console.WindowWidth;
        for (int i = 0; i < with; i++)
        {
            Console.Write("-");
        }
    }

    private static void ListarVehiculos()
    {
        LimpiarPantalla();
        string[] columnas = { "Patente", "Vehículo", "Tipo", "Cap. Carga", "Km/l", "Año", "L.Extra", "Kms a recorrer" };
        DibujarEncabezado(columnas);
        DibjuarDatos(columnas.Length);
        DibujarLinea();
        Console.Write("\n");
        Console.Write("\n");
        Console.WriteLine("Presione una tecla para calcular el total de consumos...");
        Console.ReadLine();
        Dictionary<string, double> vehiculos = new Dictionary<string, double>();
        foreach (VehiculoViewModel vehiculo in _vehiculos)
        {
            vehiculos.Add(vehiculo.GetPatente(), vehiculo.GetKmARecorrer());
        }
        (double, double) totalConsumos = Controlador.CalcularConsumos(vehiculos);
        DibujarLinea();
        Console.WriteLine($"Total consumo Vehículos Eléctricos: {totalConsumos.Item1} kWh");
        Console.WriteLine($"Total consumo Vehículos Combustible: {totalConsumos.Item2} Litros");
        DibujarLinea();
        Console.Write("\n");
        Console.Write("\n");
        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadLine();
    }
    private static void AgregarVehiculo()
    {
        LimpiarPantalla();
        DibujarLinea();
        CentrarTexto("Agregar Vehículo", out int _);
        DibujarLinea();
        Console.WriteLine("\n0. Volver atrás\n");

        // Tipo
        Console.WriteLine("Tipo de vehículo:");
        Console.WriteLine("1. Eléctrico");
        Console.WriteLine("2. Combustible");
        Console.Write("Opción: ");
        string? tipo = Console.ReadLine();
        if (tipo == "0") return;
        if (tipo != "1" && tipo != "2")
        {
            Console.WriteLine("Opción inválida. Presione Enter para volver...");
            Console.ReadLine();
            return;
        }

        // Datos comunes
        Console.Write("Patente: ");
        string? patente = Console.ReadLine();
        if (patente == "0") return;

        Console.Write("Marca: ");
        string? marca = Console.ReadLine();
        if (marca == "0") return;

        Console.Write("Modelo: ");
        string? modelo = Console.ReadLine();
        if (modelo == "0") return;

        Console.Write("Año: ");
        string? anioStr = Console.ReadLine();
        if (anioStr == "0") return;
        if (!int.TryParse(anioStr, out int anio))
        {
            Console.WriteLine("Año inválido. Presione Enter para volver...");
            Console.ReadLine();
            return;
        }

        Console.Write("Capacidad de carga (kg): ");
        string? capacidadStr = Console.ReadLine();
        if (capacidadStr == "0") return;
        if (!double.TryParse(capacidadStr, out double capacidad))
        {
            Console.WriteLine("Capacidad inválida. Presione Enter para volver...");
            Console.ReadLine();
            return;
        }

        // Sucursal
        List<string> sucursales = Controlador.GetCodigosSucursales();
        Console.WriteLine("Sucursales disponibles: " + string.Join(", ", sucursales));
        Console.Write("Código de sucursal: ");
        string? sucursal = Console.ReadLine();
        if (sucursal == "0") return;

        // Datos específicos por tipo
        double parametro1 = 0, parametro2 = 0;
        if (tipo == "1")
        {
            Console.Write("kWh base: ");
            string? kwhStr = Console.ReadLine();
            if (kwhStr == "0") return;
            if (!double.TryParse(kwhStr, out parametro1))
            {
                Console.WriteLine("Valor inválido. Presione Enter para volver...");
                Console.ReadLine();
                return;
            }
        }
        else
        {
            Console.Write("Kilómetros por litro: ");
            string? kmLStr = Console.ReadLine();
            if (kmLStr == "0") return;
            if (!double.TryParse(kmLStr, out parametro1))
            {
                Console.WriteLine("Valor inválido. Presione Enter para volver...");
                Console.ReadLine();
                return;
            }

            Console.Write("Litros extra: ");
            string? litrosStr = Console.ReadLine();
            if (litrosStr == "0") return;
            if (!double.TryParse(litrosStr, out parametro2))
            {
                Console.WriteLine("Valor inválido. Presione Enter para volver...");
                Console.ReadLine();
                return;
            }
        }

        bool ok = Controlador.AgregarVehiculo(patente!, marca!, modelo!, anio, capacidad, sucursal!, tipo, parametro1, parametro2);

        DibujarLinea();
        Console.WriteLine(ok ? "✓ Vehículo agregado correctamente." : "✗ Error: sucursal no encontrada.");
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }
    private static void DibujarEncabezado(params string[] columnas)
    {
        DibujarLinea();
        int ancho = Console.WindowWidth / columnas.Length;

        foreach (var columna in columnas)
        {
            Console.Write("|");
            CentrarTexto(columna, out int l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
        }
        Console.Write("\n");
        DibujarLinea();
    }
    private static void DibjuarDatos(int columnas)
    {
        int ancho = Console.WindowWidth / columnas;
        foreach (var vehiculo in _vehiculos)
        {
            Console.Write("|");
            CentrarTexto(vehiculo.GetPatente(), out int l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetVehiculo(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetTipo(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetCapacidadCarga().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetKmPorLitro().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetAnio().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetLitrosExtra().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetKmARecorrer().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
        }
    }
}
