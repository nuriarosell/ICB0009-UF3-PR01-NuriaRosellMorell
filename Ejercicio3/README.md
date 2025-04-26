# ¿Dónde controlar el acceso? ¿Cliente o Servidor?

## Control en el Cliente

Cada cliente (vehículo) decide si puede entrar al puente basándose en el estado que recibe.

### Ventajas:
- Menor carga de procesamiento para el servidor.
- Código más simple en pequeños proyectos de prueba.

### Inconvenientes:
- No se puede garantizar que solo un vehículo cruce a la vez.
- Dificultad para sincronizar correctamente en redes reales (latencia, desconexiones).
- Riesgo de que un cliente modificado incumpla las reglas (problemas de seguridad).

---

## Control en el Servidor

El servidor es quien decide quién puede entrar y quién debe esperar.

### Ventajas:
- **Control total**: garantiza que solo un vehículo esté en el puente a la vez.
- **Manejo correcto** de situaciones simultáneas (colas, prioridades).
- **Seguridad**: los clientes no pueden saltarse las reglas.
- **Escalabilidad**: se puede adaptar fácilmente a más usuarios o situaciones más complejas.

### Inconvenientes:
- Mayor carga de procesamiento para el servidor.
- Ligeramente más complejo de programar.

---

## Conclusión

Es recomendable controlar el acceso al túnel desde el **servidor** para asegurar la coherencia, la seguridad y la fiabilidad del sistema.

----------------------------------------------------------------------------------------------------------

# Gestión de Colas en el Servidor

## ¿Qué estructura de datos usar?

Utilizaría dos `Queue` (cola FIFO), una para cada dirección (norte y sur):

```csharp
Queue<Vehiculo> colaNorte;
Queue<Vehiculo> colaSur;
```

### Justificación:

- `Queue` es una estructura **FIFO (First In, First Out)**:
  El primer vehículo en llegar es el primero en cruzar, respetando el orden de llegada.
- Separar las colas por dirección **facilita el control de turnos** y evita conflictos.
- Permite implementar **políticas de prioridad o alternancia** fácilmente (por ejemplo, dejar pasar 2 vehículos del norte, luego 1 del sur).
- Es una estructura **muy eficiente** y de **bajo coste computacional**.

---

## Lógica Básica del Servidor

### Cuando un vehículo solicita acceso al puente:

- El servidor:
  - **Si el puente está libre**:
    - Permite al vehículo cruzar.
    - Actualiza la variable `VehiculoEnPuente`.
  - **Si el puente está ocupado**:
    - Añade el vehículo a la cola correspondiente (`colaNorte` o `colaSur`).
    - Informa al cliente que debe esperar.

### Cuando un vehículo termina de cruzar:

- El servidor:
  - Marca `VehiculoEnPuente = null`.
  - Revisa las colas:
    - Si hay vehículos esperando, se elige el siguiente según la política de prioridad (por ejemplo, el que más tiempo lleve esperando).
    - Se notifica al siguiente vehículo que puede cruzar.

