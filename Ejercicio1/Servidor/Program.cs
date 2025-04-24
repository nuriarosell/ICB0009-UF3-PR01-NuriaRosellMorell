using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Servidor
{

    class Program
    {

        static void Main(string[] args)
        {      
            // Definir el puerto y la dirección IP
            int puerto = 5000;
            string direccionIP = "127.0.0.1"; // localhost

            // Crear un objeto de TcpListener que escuche en la dirección IP y el puerto
            TcpListener servidor = new TcpListener(IPAddress.Parse(direccionIP), puerto);
            servidor.Start();

            Console.WriteLine("Servidor esperando conexiones...");

            // Aceptar conexiones de clientes
            while (true)
            {
                // Aceptar la conexión de un cliente
                TcpClient cliente = servidor.AcceptTcpClient();
                Console.WriteLine("Cliente conectado.");

                // Obtener el flujo de datos del cliente
                NetworkStream stream = cliente.GetStream();
                
                // Enviar un mensaje al cliente
                byte[] mensaje = Encoding.ASCII.GetBytes("Bienvenido al servidor.");
                stream.Write(mensaje, 0, mensaje.Length);
                
                // Cerrar la conexión con el cliente
                cliente.Close();
            }      


        }
    }
}

