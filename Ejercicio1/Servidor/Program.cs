using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Servidor
{
    class Program
    {
        // Contador para asignar ID único a cada vehículo
        private static int IDCounter = 0;
        private static readonly object lockObj = new object(); // Objeto para sincronización

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

                // Crear un nuevo hilo para gestionar el cliente
                Thread clienteThread = new Thread(() => GestionarCliente(cliente));
                clienteThread.Start();
            }
        }

        // Método para gestionar cada cliente
        private static void GestionarCliente(TcpClient cliente)
        {
            // Obtener el flujo de datos del cliente
            NetworkStream stream = cliente.GetStream();
                
            // Asignar un ID único y una dirección aleatoria
            Vehiculo vehiculo = AsignarIDYDireccion();

            // Enviar el mensaje con la información del vehículo
            string mensaje = $"Vehículo ID: {vehiculo.Id}, Dirección: {vehiculo.Direccion}";
            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            stream.Write(datos, 0, datos.Length);
            
            // Mostrar en consola el ID asignado
            Console.WriteLine($"Gestionando nuevo vehículo... ID: {vehiculo.Id}, Dirección: {vehiculo.Direccion}");
                
            // Cerrar la conexión con el cliente
            cliente.Close();
        }

        // Método para asignar un ID único y una dirección aleatoria
        private static Vehiculo AsignarIDYDireccion()
        {
            Vehiculo nuevoVehiculo = new Vehiculo();

            // Bloquear la sección crítica para asegurar que el ID se asigna correctamente
            lock (lockObj)
            {
                nuevoVehiculo.Id = IDCounter++;
            }

            // Asignar dirección aleatoria
            Random rand = new Random();
            nuevoVehiculo.Direccion = rand.Next(2) == 0 ? "Norte" : "Sur";

            return nuevoVehiculo;
        }
    }
}
