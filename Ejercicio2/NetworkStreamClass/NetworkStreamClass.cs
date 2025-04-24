using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using VehiculoClass;
using CarreteraClass;


namespace NetworkStreamNS
{
    public class NetworkStreamClass
    {
        
        //Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void  EscribirDatosCarreteraNS(NetworkStream NS, Carretera C)
        {            
            // Serializar el objeto Carretera a un array de bytes
            byte[] datosSerializados = SerializarCarretera(C);

            // Escribir los bytes en el NetworkStream
            NS.Write(datosSerializados, 0, datosSerializados.Length);        
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Carretera
        public static Carretera LeerDatosCarreteraNS (NetworkStream NS)
        {
            
            // Leer la longitud de los datos recibidos
            byte[] buffer = new byte[1024]; // Un tamaño arbitrario del buffer
            int bytesLeidos = NS.Read(buffer, 0, buffer.Length);

            // Deserializar los datos leídos a un objeto Carretera
            byte[] datosRecibidos = new byte[bytesLeidos];
            Array.Copy(buffer, 0, datosRecibidos, 0, bytesLeidos);

            // Deserializar el array de bytes a un objeto Carretera
            return DeserializarCarretera(datosRecibidos);

        }

        //Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void  EscribirDatosVehiculoNS(NetworkStream NS, Vehiculo V)
        {      
            // Serializar el objeto Vehiculo a un array de bytes
            byte[] datosSerializados = SerializarVehiculo(V);

            // Escribir los bytes en el NetworkStream
            NS.Write(datosSerializados, 0, datosSerializados.Length);      
                    
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Vehiculo
        public static Vehiculo LeerDatosVehiculoNS (NetworkStream NS)
        {
            // Leer la longitud de los datos recibidos
            byte[] buffer = new byte[1024]; // Tamaño arbitrario del buffer
            int bytesLeidos = NS.Read(buffer, 0, buffer.Length);

            // Deserializar los datos leídos a un objeto Vehiculo
            byte[] datosRecibidos = new byte[bytesLeidos];
            Array.Copy(buffer, 0, datosRecibidos, 0, bytesLeidos);

            // Deserializar el array de bytes a un objeto Vehiculo
            return DeserializarVehiculo(datosRecibidos);

        }

        //Método que permite leer un mensaje de tipo texto (string) de un NetworkStream
        public static string LeerMensajeNetworkStream (NetworkStream NS)
        {
            byte[] bufferLectura = new byte[1024];

            //Lectura del mensaje
            int bytesLeidos = 0;
            var tmpStream = new MemoryStream();
            byte[] bytesTotales; 
            do
            {
                int bytesLectura = NS.Read(bufferLectura,0,bufferLectura.Length);
                tmpStream.Write(bufferLectura, 0, bytesLectura);
                bytesLeidos = bytesLeidos + bytesLectura;
            }while (NS.DataAvailable);

            bytesTotales = tmpStream.ToArray();            

            return Encoding.Unicode.GetString(bytesTotales, 0, bytesLeidos);                 
        }

        //Método que permite escribir un mensaje de tipo texto (string) al NetworkStream
        public static void  EscribirMensajeNetworkStream(NetworkStream NS, string Str)
        {            
            byte[] MensajeBytes = Encoding.Unicode.GetBytes(Str);
            NS.Write(MensajeBytes,0,MensajeBytes.Length);                        
        }                          

    }
}
