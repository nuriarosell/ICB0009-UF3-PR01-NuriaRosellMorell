using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
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

            // Leer el ID asignado por el servidor (sería un ID del vehículo o alguna identificación generada por el servidor)
            string idRecibido = NetworkStreamClass.LeerMensajeNetworkStream(stream);
            Console.WriteLine($"ID recibido del servidor: {idRecibido}");

            // Enviar una confirmación al servidor de que hemos recibido el ID
            NetworkStreamClass.EscribirMensajeNetworkStream(stream, idRecibido); // Confirmación de ID

            // Crear un nuevo vehículo
            Vehiculo nuevoVehiculo = new Vehiculo();
            nuevoVehiculo.Id = int.Parse(idRecibido); // Asignar el ID recibido al vehículo
            Console.WriteLine($"Nuevo vehículo creado con ID: {nuevoVehiculo.Id}");

            // Enviar el vehículo al servidor
            NetworkStreamClass.EscribirVehiculoNetworkStream(stream, nuevoVehiculo);
            Console.WriteLine("Vehículo enviado al servidor.");

            // Confirmar al servidor que el vehículo fue enviado
            string confirmacion = NetworkStreamClass.LeerMensajeNetworkStream(stream);
            Console.WriteLine($"Confirmación del servidor: {confirmacion}");

            // Cerrar la conexión con el servidor
            cliente.Close();
        }
    }
}
