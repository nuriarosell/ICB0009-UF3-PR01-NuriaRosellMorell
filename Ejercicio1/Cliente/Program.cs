using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            // Definir la dirección IP del servidor y el puerto
            string direccionIP = "127.0.0.1"; // localhost
            int puerto = 5000;

            // Crear un objeto TcpClient y conectarse al servidor
            TcpClient cliente = new TcpClient(direccionIP, puerto);
            Console.WriteLine("Conectado al servidor.");

            // Obtener el flujo de datos del servidor
            NetworkStream stream = cliente.GetStream();

            // Leer los datos enviados por el servidor
            byte[] datos = new byte[256];
            int bytesLeidos = stream.Read(datos, 0, datos.Length);
            string mensaje = Encoding.ASCII.GetString(datos, 0, bytesLeidos);

            // Mostrar el mensaje recibido
            Console.WriteLine($"Mensaje del servidor: {mensaje}");

            // Cerrar la conexión
            cliente.Close();

        }

    }
}