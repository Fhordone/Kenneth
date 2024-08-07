# EjercicioTecXYZ
Pasos para ejecutar el código según el problema
PASO 1: Ejecutar el script de la BD "XYZBD.SQL" en MySQL
PASO 2: Modificar la cadena de conexión en el archivo AppSettings.JSON según las credenciales de su BD
PASO 3: Ejecutar el proyecto
PASO 4: Realizar las pruebas en Postman
Paso 5: Para autenticar el usuario se debe ingresar el URL https://localhost:7283/api/Authenticate/Validar (Cambiar el Puerto según su equipo) en método POST y este arrojará un token único si el email y password es correcto que estará activa la sesión por 5 minutos si es que no hay uso.
Adjunto el Body:
{
    "email": "maria.lopez@botica.com",
	"password": "123"
}
Paso 6: Para modificar el estado de un pedido, se debe ingresar el Bearer Token como parámetro anteriormente dado por la autenticación, solo el usuario con rol de "Repartidor" estará autorizado a realizar una actualización del pedido.
Paso 7: Se debe ingresar el URL https://localhost:7283/api/Pedido/UpdatePedido/1 (El número 1 es el Número de Pedido) 
Paso 8: Se debe ingresar en el Body:
{
    "Estado": 2
}
El algoritmo no permitirá retroceder de estado y se actualizará la fecha según el día que se realice el cambio de estado
De haber alguna duda existente, escribirme a kenneth.motta27@gmail.com
Saludos!