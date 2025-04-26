# Comunicación Cliente-Servidor con TCP  
**Proyecto DAM – UF3 – Nuria Rosell Morell**  
# Ejercicio 2: Intercambio de Información entre Vehículos

Este ejercicio simula el intercambio de información entre vehículos en una carretera, utilizando una arquitectura cliente-servidor.

## Flujo de Datos

1. El cliente mueve un vehículo y envía los datos al servidor.
2. El servidor actualiza la simulación y envía el estado completo de la carretera a todos los clientes.
3. Los clientes muestran el estado actualizado de la carretera y los vehículos.

### Ejemplo de salida en el cliente:


## Clases

- **Vehiculo**: Representa un vehículo con `Id`, `Pos` (posición), `Velocidad`, `Acabado`, `Direccion` y `Parado`.
- **Carretera**: Contiene una lista de vehículos y el número total de vehículos en la carretera.

## Métodos de `NetworkStreamClass`

- **EscribirDatosCarreteraNS**: Envía los datos de la carretera al servidor.
- **LeerDatosCarreteraNS**: Recibe los datos de la carretera desde el servidor.
- **EscribirDatosVehiculoNS**: Envía los datos de un vehículo al servidor.
- **LeerDatosVehiculoNS**: Recibe los datos de un vehículo desde el servidor.

## Etapas

1. **Etapa 1**: Implementación de los métodos para leer y escribir en el `NetworkStream`.
2. **Etapa 2**: El cliente crea un vehículo y lo envía al servidor.
3. **Etapa 3**: El cliente mueve el vehículo y envía la actualización al servidor.
4. **Etapa 4**: El servidor envía el estado de la carretera a todos los clientes.
5. **Etapa 5**: El cliente recibe y muestra la información del servidor.

## Recomendaciones

- Usa `try-catch` para manejar errores de lectura y escritura.
- Asegúrate de gestionar correctamente la concurrencia en las operaciones.

## Ejecución

1. **Inicia el servidor** para escuchar conexiones.
2. **Inicia los clientes** para simular el movimiento de los vehículos.
3. **Verifica la simulación** en la consola de los clientes.

