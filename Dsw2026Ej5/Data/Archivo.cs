using Dsw2026Ej5.Domain;
using System.Xml;

namespace Dsw2026Ej5.Data;

public class Archivo
{
    private static readonly string ruta = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "vehiculos.xml"
);


    public static void Guardar(List<Vehiculo> vehiculos)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement raiz = doc.CreateElement("Vehiculos");
        doc.AppendChild(raiz);

        foreach (Vehiculo v in vehiculos)
        {
            XmlElement nodo = doc.CreateElement("Vehiculo");
            nodo.SetAttribute("patente", v.GetPatente());
            nodo.SetAttribute("marca", v.GetMarca());
            nodo.SetAttribute("modelo", v.GetModelo());
            nodo.SetAttribute("anio", v.GetAnio().ToString());
            nodo.SetAttribute("capacidadCarga", v.GetCapacidadCarga().ToString());
            nodo.SetAttribute("sucursal", v.GetSucursal().GetCodigo());

            if (v is VehiculoElectrico electrico)
            {
                nodo.SetAttribute("tipo", "Electrico");
                nodo.SetAttribute("kwhBase", electrico.GetKwhBase().ToString());
            }
            else if (v is VehiculoCombustible combustible)
            {
                nodo.SetAttribute("tipo", "Combustible");
                nodo.SetAttribute("kilometrosPorLitro", combustible.GetKilometrosPorLitro().ToString());
                nodo.SetAttribute("litrosExtra", combustible.GetLitrosExtra().ToString());
            }

            raiz.AppendChild(nodo);
        }

        doc.Save(ruta);
    }

    public static List<Vehiculo> Leer(List<Sucursal> sucursales)
    {
        List<Vehiculo> vehiculos = new List<Vehiculo>();
        if (!File.Exists(ruta)) return vehiculos;

        XmlDocument doc = new XmlDocument();
        doc.Load(ruta);

        XmlNodeList? nodos = doc.SelectNodes("//Vehiculo");
        if (nodos == null) return vehiculos;

        foreach (XmlNode nodo in nodos)
        {
            string patente = nodo.Attributes?["patente"]?.Value ?? "";
            string marca = nodo.Attributes?["marca"]?.Value ?? "";
            string modelo = nodo.Attributes?["modelo"]?.Value ?? "";
            int anio = int.Parse(nodo.Attributes?["anio"]?.Value ?? "0");
            double capacidadCarga = double.Parse(nodo.Attributes?["capacidadCarga"]?.Value ?? "0");
            string codigoSucursal = nodo.Attributes?["sucursal"]?.Value ?? "";
            string tipo = nodo.Attributes?["tipo"]?.Value ?? "";

            Sucursal? sucursal = sucursales.Find(s => s.GetCodigo() == codigoSucursal);
            if (sucursal == null) continue;

            if (tipo == "Electrico")
            {
                double kwhBase = double.Parse(nodo.Attributes?["kwhBase"]?.Value ?? "0");
                vehiculos.Add(new VehiculoElectrico(patente, marca, modelo, anio, capacidadCarga, sucursal, kwhBase));
            }
            else if (tipo == "Combustible")
            {
                double kmPorLitro = double.Parse(nodo.Attributes?["kilometrosPorLitro"]?.Value ?? "0");
                double litrosExtra = double.Parse(nodo.Attributes?["litrosExtra"]?.Value ?? "0");
                vehiculos.Add(new VehiculoCombustible(patente, marca, modelo, anio, capacidadCarga, sucursal, kmPorLitro, litrosExtra));
            }
        }

        return vehiculos;
    }
}
