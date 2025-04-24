using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

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

                // Crear un nuevo hilo para manejar este cliente
                Thread hiloCliente = new Thread(() => GestionarCliente(cliente));
                hiloCliente.Start();
            }
        }

        // Método para gestionar a cada cliente
        private static void GestionarCliente(TcpClient cliente)
        {
            try
            {
                // Obtener el flujo de datos del cliente
                NetworkStream stream = cliente.GetStream();

                // Enviar un mensaje al cliente
                byte[] mensaje = Encoding.ASCII.GetBytes("Bienvenido al servidor.");
                stream.Write(mensaje, 0, mensaje.Length);

                // Mostrar el mensaje de que el vehículo está siendo gestionado
                Console.WriteLine("Gestionando nuevo vehículo...");

                // Aquí puedes agregar más lógica para la interacción con el cliente (leer datos, responder, etc.)

                // Esperar para simular algún proceso, luego cerrar la conexión
                Thread.Sleep(1000); // Simulando que se está gestionando algo

                // Cerrar la conexión con el cliente
                cliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error gestionando el cliente: {ex.Message}");
            }
        }
    }
}
