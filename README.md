MAUI CRUD Application

Descripción

Este proyecto es una aplicación desarrollada en .NET MAUI que implementa un CRUD (Crear, Leer, Actualizar y Eliminar) sobre una base de datos SQLite (empresa.db). Permite gestionar una lista de trabajadores de manera sencilla y eficiente.

Características

Interfaz gráfica moderna con .NET MAUI.

CRUD completo sobre la base de datos SQLite.

Validación de datos para evitar registros incorrectos.

Uso de MVVM para una mejor separación de responsabilidades.

Compatibilidad multiplataforma (Android, iOS, Windows, macOS).

Tecnologías Utilizadas

.NET MAUI

SQLite

C#

XAML (para la UI)

Instalación y Configuración

Requisitos Previos

Tener instalado .NET SDK

Tener instalado Visual Studio con la carga de trabajo de .NET MAUI

Pasos de Instalación

Clonar el repositorio:

git clone https://github.com/CevicheConAji/SQLite03.git
cd maui-crud

Restaurar paquetes:

dotnet restore

Ejecutar la aplicación:

dotnet build
dotnet run

Uso de la Aplicación

Pantalla Principal

Se muestra una lista de trabajadores almacenados en la base de datos.

Se pueden agregar nuevos trabajadores mediante dos campos de entrada (Entry).

Se pueden seleccionar trabajadores de la lista para actualizar o eliminar.

Funcionalidades CRUD

Crear: Ingresar nombre y apellidos en los Entry y presionar "Agregar".

Leer: Los trabajadores se listan en un CollectionView.

Actualizar: Seleccionar un trabajador, editar los Entry y presionar "Actualizar".

Eliminar: Seleccionar un trabajador y presionar "Eliminar".

Capturas de Pantalla


Contribución

Si deseas contribuir a este proyecto, sigue estos pasos:

Haz un fork del repositorio.

Crea una rama (git checkout -b feature-nueva-funcionalidad).

Realiza tus cambios y haz commit (git commit -m "Descripción del cambio").

Haz push a la rama (git push origin feature-nueva-funcionalidad).

Crea un Pull Request.

Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo LICENSE para más detalles.

Contacto

Si tienes preguntas o sugerencias, contáctame en: zavalachirapiero@gmail.com.
