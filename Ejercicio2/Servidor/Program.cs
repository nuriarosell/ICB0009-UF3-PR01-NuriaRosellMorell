using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using VehiculoClass;
using CarreteraClass;
using System.Collections.Generic;

namespace Servidor
{
    class Program
    {
        // Contador para asignar ID único a cada vehículo
        private static int IDCounter = 0;
        private static readonly object lockObj = new object(); // Objeto para sincronización
        private static List<Cliente> clientesConectados = new List<Cliente>();


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

            // Leer mensaje de inicio
            string mensajeInicio = NetworkStreamClass.LeerMensajeNetworkStream(stream);

            // Mostrar el mensaje recibido en la consola del servidor
            Console.WriteLine($"Mensaje recibido del cliente: {mensajeInicio}");
            if (mensajeInicio == "INICIO")
            {
                // Asignar un ID único y una dirección aleatoria
                Vehiculo vehiculo = AsignarIDYDireccion();

                // Enviar el ID al cliente
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, vehiculo.Id.ToString());

                // Leer la confirmación del ID por parte del cliente
                string confirmacion = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                if (confirmacion == vehiculo.Id.ToString())
                {
                    Console.WriteLine("Cliente ha confirmado el ID correctamente.");

                    // Añadir cliente a la lista de clientes conectados
                    Cliente nuevoCliente = new Cliente(vehiculo.Id, stream);
                    lock (lockObj)
                    {
                        clientesConectados.Add(nuevoCliente);
                        Console.WriteLine($"Clientes conectados: {clientesConectados.Count}");
                    }
                }
                else
                {
                    Console.WriteLine("Error: El cliente no ha confirmado correctamente el ID.");
                }
            }

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
