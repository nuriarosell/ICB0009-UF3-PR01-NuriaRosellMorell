# Comunicación Cliente-Servidor con TCP  
**Proyecto DAM – UF3 – Nuria Rosell Morell**  
Este proyecto simula la comunicación entre un servidor y múltiples clientes (vehículos), utilizando sockets TCP en C#. El flujo de conexión está basado en un *handshake* y el almacenamiento de información de los clientes conectados.

---

## Tecnologías utilizadas
- Lenguaje: C#
- Librerías:
  - `System.Net.Sockets`
  - `System.Threading`
  - Clases auxiliares personalizadas (`Vehiculo`, `NetworkStreamClass`, etc.)

---

## Estructura del Proyecto
- `Servidor`: Código principal del servidor TCP.
- `Cliente`: Código principal del cliente que se conecta al servidor.
- `Vehiculo.cs`: Clase que representa un vehículo con ID y dirección.
- `Cliente.cs`: Clase que representa un cliente conectado (con ID y `NetworkStream`).
- `NetworkStreamClass.cs`: Clase estática con métodos para enviar y recibir mensajes de forma sencilla por `NetworkStream`.

---

## Etapas implementadas

### Etapa 1: Conexión servidor - cliente
Creamos un servidor y un cliente básicos que se conectan usando `TcpListener` y `TcpClient`.  
Mensajes de consola informan del estado de la conexión para facilitar la depuración.

---

### Etapa 2: Aceptación de múltiples clientes
El servidor acepta múltiples clientes usando **programación concurrente**.  
Cada nuevo cliente es gestionado en un hilo separado, permitiendo que el hilo principal siga escuchando nuevas conexiones.

---

### Etapa 3: Asignación de ID único y dirección
Cada vez que un cliente se conecta, el servidor le asigna un **ID único** y una **dirección aleatoria** (`Norte` o `Sur`).  
El acceso a variables compartidas (como el contador de ID) se protege con `lock`.

---

### Etapa 4: Obtención del `NetworkStream`
Tanto el servidor como el cliente recuperan el `NetworkStream` desde el `TcpClient`, que se utilizará para la transmisión de datos.

---

### Etapa 5: Métodos para enviar y recibir mensajes
Implementamos los métodos `EscribirMensajeNetworkStream` y `LeerMensajeNetworkStream` para facilitar la comunicación entre servidor y cliente mediante `NetworkStream`.  
Estos métodos se encapsulan en una clase estática para su reutilización.

---

### Etapa 6: Handshake entre cliente y servidor
Proceso de sincronización inicial:
1. El cliente envía `"INICIO"` al servidor.
2. El servidor responde con el ID asignado.
3. El cliente responde con el mismo ID para confirmar que lo ha recibido correctamente.

---

### Etapa 7: Almacenamiento de clientes conectados
Creamos una clase `Cliente` que almacena:
- El ID del cliente.
- Su `NetworkStream`.

El servidor guarda todos los objetos `Cliente` en una **lista de clientes conectados**, lo que permite enviar mensajes a todos o realizar otras operaciones.  
También mostramos el total de clientes conectados usando `.Count`.
---

## Flujo de Ejecución

**Servidor:**
1. Inicia el `TcpListener` y queda esperando conexiones.
2. Acepta nuevos clientes en un bucle infinito.
3. Por cada cliente, lanza un nuevo hilo para gestionarlo.
4. Asigna un ID único y una dirección aleatoria (norte/sur).
5. Realiza el proceso de *handshake*:
   - Espera recibir `"INICIO"`.
   - Responde con el ID asignado.
   - Recibe confirmación del ID.
6. Añade al cliente a la lista de clientes conectados.
7. Muestra por consola los eventos relevantes (conexión, ID, total de clientes...).

**Cliente (Vehículo):**
1. Se conecta al servidor usando `TcpClient`.
2. Obtiene el `NetworkStream`.
3. Inicia el *handshake* enviando `"INICIO"`.
4. Recibe su ID asignado.
5. Envía el mismo ID de vuelta al servidor como confirmación.
6. Muestra el ID recibido en consola.