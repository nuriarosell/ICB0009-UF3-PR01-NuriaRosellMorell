using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using VehiculoClass;
using System.Collections.Generic;

namespace Servidor
{
    class Program
    {
        private static readonly object lockObj = new object();
        private static List<Vehiculo> vehiculosEnCarretera = new List<Vehiculo>();

        static void Main(string[] args)
        {
            int puerto = 5000;
            string direccionIP = "127.0.0.1"; // localhost

            // Crear un TcpListener para aceptar conexiones de los clientes
            TcpListener servidor = new TcpListener(IPAddress.Parse(direccionIP), puerto);
            servidor.Start();

            Console.WriteLine("Servidor esperando conexiones...");

            // Aceptar conexiones de clientes
            while (true)
            {
                TcpClient cliente = servidor.AcceptTcpClient();
                Console.WriteLine("Cliente conectado.");

                // Crear un hilo para gestionar al cliente
                Thread clienteThread = new Thread(() => GestionarCliente(cliente));
                clienteThread.Start();
            }
        }

        private static void GestionarCliente(TcpClient cliente)
        {
            NetworkStream stream = cliente.GetStream();

            // Leer el mensaje inicial de "INICIO"
            string mensajeInicio = NetworkStreamClass.LeerMensajeNetworkStream(stream);
            Console.WriteLine($"Mensaje recibido del cliente: {mensajeInicio}");

            if (mensajeInicio == "INICIO")
            {
                // Asignar un ID y una dirección aleatoria
                Vehiculo vehiculo = new Vehiculo
                {
                    Id = new Random().Next(1, 1000), // ID aleatorio para el ejemplo
                    Direccion = "Norte" // Dirección aleatoria
                };

                // Enviar el ID al cliente
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, vehiculo.Id.ToString());

                // Leer la confirmación del cliente
                string confirmacion = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                if (confirmacion == "ID recibido")
                {
                    Console.WriteLine("Cliente ha confirmado el ID correctamente.");

                    // Agregar el vehículo a la carretera
                    lock (lockObj)
                    {
                        vehiculosEnCarretera.Add(vehiculo);
                    }

                    // Bucle para actualizar el vehículo en la carretera
                    while (true)
                    {
                        // Leer el vehículo actualizado desde el cliente
                        Vehiculo vehiculoActualizado = NetworkStreamClass.LeerVehiculoNetworkStream(stream);
                        if (vehiculoActualizado.Acabado)
                        {
                            Console.WriteLine($"Vehículo {vehiculoActualizado.Id} ha terminado su recorrido.");
                            break;
                        }

                        // Actualizar el vehículo en la carretera
                        lock (lockObj)
                        {
                            var vehiculoExistente = vehiculosEnCarretera.Find(v => v.Id == vehiculoActualizado.Id);
                            if (vehiculoExistente != null)
                            {
                                vehiculoExistente.Pos = vehiculoActualizado.Pos;
                            }
                        }

                        // Mostrar la lista de vehículos en la carretera
                        MostrarVehiculosEnCarretera();
                    }
                }
            }

            // Cerrar la conexión con el cliente
            cliente.Close();
        }

        private static void MostrarVehiculosEnCarretera()
        {
            Console.WriteLine("Vehículos en la carretera:");
            foreach (var vehiculo in vehiculosEnCarretera)
            {
                Console.WriteLine($"ID: {vehiculo.Id}, Dirección: {vehiculo.Direccion}, Posición: {vehiculo.Pos}");
            }
        }
    }
}
