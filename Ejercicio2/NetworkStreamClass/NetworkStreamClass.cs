using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Text.Json;
using VehiculoClass;
using CarreteraClass;

namespace NetworkStreamNS
{
    public class NetworkStreamClass
    {
        //Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void EscribirDatosCarreteraNS(NetworkStream NS, Carretera C)
        {
            string json = JsonSerializer.Serialize(C);
            byte[] data = Encoding.UTF8.GetBytes(json);
            NS.Write(data, 0, data.Length);
        }

        //Método para leer de un NetworkStream los datos que de un objeto Carretera
        public static Carretera LeerDatosCarreteraNS(NetworkStream NS)
        {
            byte[] bufferLectura = new byte[1024];
            int bytesLeidos = 0;
            var tmpStream = new MemoryStream();

            do
            {
                int bytesLectura = NS.Read(bufferLectura, 0, bufferLectura.Length);
                tmpStream.Write(bufferLectura, 0, bytesLectura);
                bytesLeidos += bytesLectura;
            } while (NS.DataAvailable);

            string json = Encoding.UTF8.GetString(tmpStream.ToArray(), 0, bytesLeidos);
            return JsonSerializer.Deserialize<Carretera>(json);
        }

        //Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void EscribirVehiculoNetworkStream(NetworkStream NS, Vehiculo V)
        {
            string json = JsonSerializer.Serialize(V);
            byte[] data = Encoding.UTF8.GetBytes(json);
            NS.Write(data, 0, data.Length);
        }

        //Método para leer un objeto Vehiculo desde NetworkStream
        public static Vehiculo LeerVehiculoNetworkStream(NetworkStream NS)
        {
            byte[] bufferLectura = new byte[1024];
            int bytesLeidos = 0;
            var tmpStream = new MemoryStream();

            do
            {
                int bytesLectura = NS.Read(bufferLectura, 0, bufferLectura.Length);
                tmpStream.Write(bufferLectura, 0, bytesLectura);
                bytesLeidos += bytesLectura;
            } while (NS.DataAvailable);

            string json = Encoding.UTF8.GetString(tmpStream.ToArray(), 0, bytesLeidos);
            return JsonSerializer.Deserialize<Vehiculo>(json);
        }

        //Método que permite leer un mensaje de tipo texto (string) de un NetworkStream
        public static string LeerMensajeNetworkStream(NetworkStream NS)
        {
            byte[] bufferLectura = new byte[1024];
            int bytesLeidos = 0;
            var tmpStream = new MemoryStream();
            byte[] bytesTotales;

            do
            {
                int bytesLectura = NS.Read(bufferLectura, 0, bufferLectura.Length);
                tmpStream.Write(bufferLectura, 0, bytesLectura);
                bytesLeidos += bytesLectura;
            } while (NS.DataAvailable);

            bytesTotales = tmpStream.ToArray();
            return Encoding.Unicode.GetString(bytesTotales, 0, bytesLeidos);
        }

        //Método que permite escribir un mensaje de tipo texto (string) al NetworkStream
        public static void EscribirMensajeNetworkStream(NetworkStream NS, string Str)
        {
            byte[] MensajeBytes = Encoding.Unicode.GetBytes(Str);
            NS.Write(MensajeBytes, 0, MensajeBytes.Length);
        }
    }
}
