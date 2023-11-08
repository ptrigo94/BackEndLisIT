Prueba Backend para LISIT

Montado: 

-El sistema corre en .netCore 6.0, por lo tanto se requiere la instalacion de .netCore 6.0 como dependencia.


-Para poder montar el sistema y que corra adecuadamente se deben ejecutar las migraciones (En la consola de paquetes NuGet escribir "Add-Migration "NOMBRE MIGRACION" y luego "Update-Database", o solamente el segundo comando").


-Se requiere una base de datos en SQL SERVER con el nombre testLisit

-La API está Autodocumentada con Swagger lo que permite un mejor entendimiento del uso de esta.

Para poder acceder a ciertas funciones que requieren de autorizacion se debe crear un usuario en los endpoints de user y un rol llamado "Admin", este usuario que se creará debe ser referenciado  al momento de ser creado con la id del rol de administrador.

Es necesario iniciar sesion como administrador para poder acceder a las funciones, esto se logra de la siguiente manera:

En el endpoint de "AUTH" se debe iniciar sesion con el correo y password. Esto devolvera un token bearer, el cual se debe usar en el boton "Authorize" de swagger con la siguiente nomenclatura: 

"bearer {token}". 

Sin comillas y token hace referencia al string devuelto despues del login.

Ante cualquier duda, por favor contactarme a mi correo.
