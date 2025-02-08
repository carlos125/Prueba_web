# Prueba_web
Prueba de practica con Vue.js
--------------Descargar proyecto GitHub----------------
1.-Abre la terminal o línea de comandos.
2.-Navega a la carpeta donde deseas descargar el repositorio:
	*Usa el comando cd para cambiar a la carpeta en la que quieres almacenar el repositorio. Ejemplo:
	*cd /ruta/a/tu/carpeta
3.-Ejecuta el siguiente comando en la terminal:
	*git clone https://github.com/carlos125/Prueba_web.git

4.-Accede a la carpeta del repositorio:
	*Una vez que el repositorio se haya clonado, navega a la carpeta:
	*cd PropuestaLaboral

--------------Abrir Proyecto en Visual Studio 2022----------------
1.-Abrir Visual Studio 2022:
	*Inicia Visual Studio 2022 en tu computadora.
2.-Seleccionar "Abrir un proyecto o una solución":
	*En la pantalla de inicio de Visual Studio, selecciona la opción "Abrir un proyecto o una solución".
3.-Buscar la carpeta donde se clonó el repositorio:
	*Navega hasta la carpeta en la que clonaste el repositorio, que en este caso sería la carpeta Prueba.
4.-Seleccionar el archivo de solución:
	*Dentro de la carpeta Prueba, busca y selecciona el archivo con el nombre Prueba.sln (este archivo es la solución que contiene el proyecto).
5.-Una vez que la solución se abra, podrás ver los archivos del proyecto en el Explorador de soluciones en el lado derecho de Visual Studio.
	*Si todo ha ido bien, verás la estructura del proyecto y podrás comenzar a trabajar en él.
5.-Haz clic en el botón verde de "Ejecutar" en la barra superior de Visual Studio.
    *Asegúrate de tener seleccionado un navegador en las configuraciones de depuración (por ejemplo, Chrome o Edge).
    *La aplicación se iniciará y abrirá automáticamente el navegador con una URL como:
    https://localhost:7112/.
------------------IMPORTANT----------------
En caso de que la url sea distinta a https://localhost:7112/ favor de editarla en appsettings.json en la linea 18 y 19 ("Issuer": "https://localhost:7112/", "Audience": "https://localhost:7112/")

--------------Instalación o Verificación de Dependencias NuGet (Versiones Sugeridas)----------------
Microsoft.AspNetCore.Authentication - Versión 8.0.0
Microsoft.EntityFrameworkCore.Tools - Versión 9.0.1
Newtonsoft.Json - Versión 13.0.3
Swashbuckle.AspNetCore.Swagger - Versión 7.2.0
Microsoft.EntityFrameworkCore.SqlServer - Versión 9.0.1
RestSharp - Versión 112.1.0
Microsoft.IdentityModel.Tokens - Versión 7.0.3
System.IdentityModel.Tokens.Jwt - Versión 7.0.3


--------------Configuración de la Base de Datos----------------
1. Acceso a SQL Server Management Studio
	Ingrese a SQL Server Management Studio.
	Ejecute el siguiente comando en el gestor de base de datos:
		CREATE DATABASE igrtec;
	Guarde el usuario y la contraseña con los que se ejecutó este comando.

2. Configuración del Servidor de Base de Datos (Recomendaciones)
	Si se utiliza Windows Authentication, se recomienda configurar el servidor para que soporte tanto Windows Authentication como SQL Server Authentication.
	Pasos:
	* Ingrese al servidor y haga clic derecho sobre él.
	* Seleccione Properties.
	* En el apartado de Security, en Server Authentication, seleccione la opción: SQL Server and Windows Authentication mode.
	* Precaución: Este cambio requiere reiniciar el servidor, por lo que es recomendable hacerlo fuera del horario de uso.

2.1. Creación de un Usuario en SQL Server
	* Inicie sesión en el servidor y navegue hasta la carpeta Security.
	* Haga clic derecho sobre Logins y seleccione New Login.
	* Configuración del nuevo usuario:

2.1.1.Seleccione SQL Server Authentication.
	* En Login Name, ingrese el nombre de usuario: carlosbd.
	* Ingrese la contraseña 12345678 en los campos Password y Confirm Password.
	* En Server Roles, seleccione sysadmin para asegurar que el usuario tenga los permisos necesarios para la administración de la base de datos.
	* Asignación de permisos a la base de datos:

2.1.2 En el apartado User Mapping
	* seleccione la base de datos igrtec.
	* En la sección de Database role membership for: igrtec, seleccione todos los roles y permisos disponibles para el usuario.
2.1.3 En el apartado Status
	* seleccione en "Permission to connect to database engine: Grant y en Login: Enabled.
2.2.Configuración Final
	Seleccionar boton "Ok" y si todo esta bien se creará el usuario
------------------IMPORTANT----------------
Con esta configuración, podrá utilizar el proyecto sin inconvenientes.
Si ya tiene un usuario con estos permisos, simplemente actualice el archivo appsettings.json del proyecto con las credenciales correspondientes.



--------------Consola del Administrador de paqutes ----------------
1.-Revisar permisos en appsettings.json
	*Asegúrate de que el usuario tenga los permisos necesarios para ejecutar el proceso de migración en la base de datos.
	*En el archivo appsettings.json, revisa la cadena de conexión:
	"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=igrtec;User Id=carlosbd;Password=12345678;TrustServerCertificate=True;"
  	}
2.-Abrir la Consola del Administrador de Paquetes:
	* Después de abrir el proyecto, verifica que la ventana Consola del Administrador de Paquetes esté visible en Visual Studio.
	*Si no la ves en la parte inferior izquierda, abre la ventana desde el menú:
		*Haz clic en Ver en la barra superior.
		*Luego selecciona Otras ventanas y elige Consola del Administrador de Paquetes.
3.-Ejecutar la migración para la base de datos:
	*En la Consola del Administrador de Paquetes, escribe el siguiente comando para crear una migración:
	add-migration Users
	*Esto generará un archivo de migración que contendrá las instrucciones para crear o actualizar las tablas en la base de datos.
4.-Aplicar la migración a la base de datos:
	*Para aplicar la migración y crear o actualizar la base de datos, ejecuta el siguiente comando en la consola:
	update-database
	*Este comando actualizará la base de datos con los cambios definidos en la migración.
4.1.-Si es necesario modificar la base de datos:
	*Si realizas cambios adicionales que requieran modificar la estructura de la base de datos, necesitarás crear una nueva migración.
	*add-migration contextoopconalCapturePendingChanges
4.2.-Actualizar la base de datos nuevamente:
	*Una vez que hayas creado la nueva migración, ejecuta el siguiente comando para aplicar los cambios:
	update-database
------------------IMPORTANT----------------
Asegúrate de que cualquier cambio en la base de datos que se realice sea revisado en el archivo appsettings.json y que los permisos del usuario estén correctamente configurados para evitar problemas con la ejecución de migraciones.

------------Generar el Token desde el Endpoint ------
1 Usar el Navegador
1.1 En el navegador donde se ejecuta la aplicación, navega al endpoint:
    https://localhost:7203/Login
1.2.-Si tienes habilitado Swagger:
    *Despliega el apartado /Login.
    *Haz clic en Try it out.
    *Haz agrega los parametros de email y password.
    *Selecciona Execute.
1.2.1.-Verifica el resultado:
    *Si todo está configurado correctamente, en la sección Responses > Response body verás un token en formato JSON:
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MzcxNTE4MzMsImV4cCI6MTczNzE1MjQzMywiaWF0IjoxNzM3MTUxODMzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTEyLyJ9.mTqQvrXmfBq9S0p5hf_Kgeuotc8tt1V8wNPypv61g6Y"
    }
2.-Usar Postman
    *Copiar la URL base:
    *Al iniciar la aplicación, copia la URL generada en el navegador (por ejemplo, https://localhost:7203/).
    *Abrir Postman:
    *Abre la aplicación Postman en tu computadora.
    *Configurar una nueva consulta:
    *Crea una nueva solicitud en Postman.
    *Selecciona el método POST.
    *En la URL, combina la ruta base con el endpoint /Login, por ejemplo
        *https://localhost:7112/Login
        *Haz clic en body y agrega los parametros que te da Swagger .
        *Haz clic en Send.
        *Si la configuración es correcta, en el área de respuesta de Postman verás un JSON con el token:
        {
            "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MzcxNTE1NzQsImV4cCI6MTczNzE1MjE3NCwiaWF0IjoxNzM3MTUxNTc0LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTEyLyJ9.1uqacAHwKQjL70Tb-JxdegXI_H43fJ697IzuJnQfOF8"
        }
        

------------Generar el Token para utilizar en los Endpoint------
Nota: Estos apartados solo podran ser probados con postman
*Al iniciar la aplicación, copia la URL generada en el navegador (por ejemplo, https://localhost:7112/).
    *Abrir Postman:
    *Abre la aplicación Postman en tu computadora.
    *Configurar una nueva consulta:
    *Crea una nueva solicitud en Postman.
