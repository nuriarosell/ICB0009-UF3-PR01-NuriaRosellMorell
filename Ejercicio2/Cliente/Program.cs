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

            // Crear un nuevo vehículo
            Vehiculo nuevoVehiculo = new Vehiculo();
            Console.WriteLine($"Nuevo vehículo creado con ID: {nuevoVehiculo.Id}");

            // Enviar el vehículo al servidor
            NetworkStreamClass.EscribirDatosVehiculoNS(stream, nuevoVehiculo);
            Console.WriteLine("Vehículo enviado al servidor.");

            // Confirmar al servidor que el vehículo fue enviado (se puede enviar algún tipo de mensaje de confirmación si es necesario)
            string confirmacion = NetworkStreamClass.LeerMensajeNetworkStream(stream);
            Console.WriteLine($"Confirmación del servidor: {confirmacion}");

            // Cerrar la conexión con el servidor
            cliente.Close();
        }
    }
}
