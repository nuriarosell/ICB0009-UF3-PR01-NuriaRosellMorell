using System.Xml.Serialization;
using VehiculoClass;

namespace CarreteraClass;

[Serializable]
public class Carretera
{
    public List<Vehiculo> VehiculosEnCarretera = new List<Vehiculo>();
    public int NumVehiculosEnCarrera = 0;

    public Carretera() {}

    // Crea y añade un nuevo vehículo a la carretera
    public void CrearVehiculo()
    {
        Vehiculo nuevo = new Vehiculo();
        AñadirVehiculo(nuevo);
    }

    // Añade un vehículo ya creado a la lista de la carretera
    public void AñadirVehiculo(Vehiculo V)
    {
        VehiculosEnCarretera.Add(V);
        NumVehiculosEnCarrera++;
    }

    // Actualiza un vehículo existente en la carretera con nuevos datos
    public void ActualizarVehiculo(Vehiculo V)
    {
        Vehiculo veh = VehiculosEnCarretera.FirstOrDefault(x => x.Id == V.Id);
        if (veh != null)
        {
            veh.Pos = V.Pos;
            veh.Velocidad = V.Velocidad;
            veh.Acabado = V.Acabado;
            veh.Direccion = V.Direccion;
            veh.Parado = V.Parado;
        }
    }

    // Muestra todos los vehículos con su estado de forma visual
    public void MostrarVehiculos()
    {
        foreach (Vehiculo v in VehiculosEnCarretera)
        {
            string barra = GenerarBarraPosicion(v.Pos);
            string estado = v.Acabado ? "Finalizado" : (v.Parado ? "Esperando" : "Cruzando");
            Console.WriteLine($"[{v.Direccion}] Vehículo #{v.Id}: {barra} (km {v.Pos} - {estado})");
        }
    }

    // Genera la barra gráfica de posición en pantalla
    private string GenerarBarraPosicion(int pos)
{
    int total = 50;
    int llenos = pos * total / 100;
    int vacíos = total - llenos;
    return new string('#', llenos) + new string('.', vacíos);
    }

    // Serializa el objeto Carretera a array de bytes usando XML
    public byte[] CarreteraABytes()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));
        using MemoryStream MS = new MemoryStream();
        serializer.Serialize(MS, this);
        return MS.ToArray();
    }

    // Deserializa una cadena de bytes a un objeto Carretera
    public static Carretera BytesACarretera(byte[] datos)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));
        using MemoryStream MS = new MemoryStream(datos);
        return (Carretera)serializer.Deserialize(MS);
    }
}
