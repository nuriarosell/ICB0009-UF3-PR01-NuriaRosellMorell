using System;
using System.Net.Sockets;
using NetworkStreamNS;
using VehiculoClass;
using System.Threading;

namespace Cliente
{
    class Program
    {
        static void Main(string[] args)
        {
            // Definir la IP y el puerto del servidor
            string direccionIP = "127.0.0.1"; // localhost
            int puerto = 5000;

            try
            {
                // Crear un TcpClient para conectarse al servidor
                TcpClient cliente = new TcpClient(direccionIP, puerto);
                NetworkStream stream = cliente.GetStream();

                // Iniciar el handshake enviando el mensaje "INICIO"
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, "INICIO");

                // Leer el ID asignado por el servidor (ID del vehículo)
                string idRecibido = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                Console.WriteLine($"ID recibido del servidor: {idRecibido}");

                // Enviar una confirmación al servidor de que hemos recibido el ID
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, "ID recibido");

                // Crear un nuevo vehículo con el ID recibido
                Vehiculo nuevoVehiculo = new Vehiculo
                {
                    Id = int.Parse(idRecibido), // Asignar el ID recibido al vehículo
                    Direccion = "Norte", // Asignar una dirección específica
                    Pos = 0, // Iniciar la posición en 0
                    Velocidad = 10, // Velocidad de avance
                    Acabado = false // El vehículo no ha terminado aún
                };
                Console.WriteLine($"Nuevo vehículo creado con ID: {nuevoVehiculo.Id}, Dirección: {nuevoVehiculo.Direccion}");

                // Crear un hilo para escuchar los datos del servidor
                Thread escuchaHilo = new Thread(() => EscucharDatosDelServidor(stream));
                escuchaHilo.Start();

                // Bucle de avance del vehículo
                for (int i = 0; i <= 100; i++)
                {
                    // Actualizar la posición del vehículo
                    nuevoVehiculo.Pos = i;

                    // Enviar el vehículo actualizado al servidor
                    NetworkStreamClass.EscribirVehiculoNetworkStream(stream, nuevoVehiculo);
                    Console.WriteLine($"Vehículo en la posición: {nuevoVehiculo.Pos}");

                    // Pausa para simular la velocidad del vehículo
                    Thread.Sleep(nuevoVehiculo.Velocidad);

                    // Si el vehículo ha llegado al final
                    if (nuevoVehiculo.Pos == 100)
                    {
                        nuevoVehiculo.Acabado = true;
                        break;
                    }
                }

                // Enviar mensaje final de que el vehículo ha acabado
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, "Vehículo terminado");

                // Cerrar la conexión con el servidor
                cliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la comunicación con el servidor: {ex.Message}");
            }
        }

        // Método para escuchar los datos del servidor
        private static void EscucharDatosDelServidor(NetworkStream stream)
        {
            try
            {
                while (true)
                {
                    // Leer datos de la carretera enviados por el servidor
                    string datosRecibidos = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                    Console.WriteLine($"Datos de la carretera recibidos: {datosRecibidos}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recibir datos del servidor: {ex.Message}");
            }
        }
    }
}
