using System.Xml.Serialization;

namespace VehiculoClass;

[Serializable]
public class Vehiculo
{
    public int Id { get; set; }
    public int Pos { get; set; }
    public int Velocidad { get; set; }
    public string Direccion { get; set; } // "Norte" o "Sur"
    public bool Acabado { get; set; }
    public bool Parado { get; set; }

    public Vehiculo()
    {
        Random rand = new Random();

        this.Id = rand.Next(1000, 9999); // ID aleatorio para identificar el vehículo
        this.Velocidad = rand.Next(100, 500); // Velocidad entre 100 y 500
        this.Pos = 0;
        this.Acabado = false;
        this.Parado = false;
        this.Direccion = "Norte"; // Dirección por defecto
    }

    // Serializa el objeto Vehiculo a array de bytes (XML)
    public byte[] VehiculoABytes()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Vehiculo));
        using MemoryStream MS = new MemoryStream();
        serializer.Serialize(MS, this);
        return MS.ToArray();
    }

    // Deserializa un array de bytes a un objeto Vehiculo
    public static Vehiculo BytesAVehiculo(byte[] datos)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Vehiculo));
        using MemoryStream MS = new MemoryStream(datos);
        return (Vehiculo)serializer.Deserialize(MS);
    }
}
