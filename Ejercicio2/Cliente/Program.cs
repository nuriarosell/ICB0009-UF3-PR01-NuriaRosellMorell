using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using VehiculoClass;
using CarreteraClass;

namespace Cliente
{
    class Program
    {
        static void Main(string[] args)
        {
            // Definir la IP y el puerto del servidor
            string direccionIP = "127.0.0.1"; // localhost
            int puerto = 5000;

            // Crear un TcpClient para conectarse al servidor
            TcpClient cliente = new TcpClient(direccionIP, puerto);
            NetworkStream stream = cliente.GetStream();

            // Iniciar el handshake enviando el mensaje "INICIO"
            NetworkStreamClass.EscribirMensajeNetworkStream(stream, "INICIO");

            // Leer el ID asignado por el servidor
            string idRecibido = NetworkStreamClass.LeerMensajeNetworkStream(stream);
            Console.WriteLine($"ID recibido del servidor: {idRecibido}");

            // Confirmar el ID enviado al servidor
            NetworkStreamClass.EscribirMensajeNetworkStream(stream, idRecibido);

            // Cerrar la conexión con el servidor
            cliente.Close();
        }
    }
}
