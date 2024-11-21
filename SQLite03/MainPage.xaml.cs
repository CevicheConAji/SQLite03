using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SQLite03
{
    
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Trabajador> _ocTrabajadores;
        private String _nombre;
        private String _apellidos;
        private Trabajador _selectedTrabajador;
        public Trabajador SelectedTrabajador
        {
            get { return _selectedTrabajador; }
            set
            {
                _selectedTrabajador = value;
                OnPropertyChanged();


            }
        }
        public ObservableCollection<Trabajador> OcTrabajadores


        {
            get { return _ocTrabajadores;}
            set
            {
                _ocTrabajadores = value;
                OnPropertyChanged();
            }
        }
        public MainPage()
        {
            InitializeComponent();
            // Lista de los trabajadores
            OcTrabajadores = new ObservableCollection<Trabajador>();


            // Creamos la ruta de la base de datos en el directorio de nuestro proyecto
            // No lo haríamos en producción pero en pruebas es más cómodo
            // tener localizada la base de datos y poder examinarla
            // con otros programas para ver los cambios producidos

            // En mi caso, devuelve:
            // "D:\\Datos\\proyectos_DI2425\\ud04part01ExempleSQLite\\SQLite03\\bin\\Debug\\net8.0-windows10.0.19041.0\\win10-x64\\AppX\\"
            string rutaDirectorioApp = System.AppContext.BaseDirectory;

            DirectoryInfo directorioApp = new DirectoryInfo(rutaDirectorioApp);

            // El objeto directorio será ahora:
            // D:\Datos\proyectos_DI2425\ud04part01ExempleSQLite\SQLite03
            directorioApp = directorioApp.Parent.Parent.Parent.Parent.Parent.Parent;

            // Creamos la ruta completa del fichero de la base de datos
            // En mi ejemplo:
            // D:\Datos\proyectos_DI2425\ud04part01ExempleSQLite\SQLite03\empresa.db
            string databasePath = Path.Combine(directorioApp.FullName, "empresa.db");

            // Creamos la cadena de conexión 
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Creamos la tabla de trabajador
                CrearTablaTrabajador(connection);

                // Creamos la consulta y la ejecutamos
                string sql = "SELECT * FROM Trabajador";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                
                // Recorremos los registros devueltos del SELECT
                while (reader.Read())
                {
                    int idTrabajador = reader.GetInt32(0);
                    string nombreTrabajador = reader.GetString(1);
                    string apellidosTrabajador = reader.GetString(2);

                    // Creamos un objeto Trabajador y lo añadimos al Observable Collection
                    Trabajador trabajador = new Trabajador
                    {
                        Id = idTrabajador,
                        Nombre = nombreTrabajador,
                        Apellidos = apellidosTrabajador,
                    };

                    // Añadimos el trabajador al Observable Collection
                    OcTrabajadores.Add(trabajador);
                }

                reader.Close();
                connection.Close();
            }
            
            BindingContext = this;
        }

        private void CrearTablaTrabajador(SQLiteConnection connection)
        {
            // Creamos la tabla Trabajador en caso de que no exista
            // Su clave principal es un autonumérico
            string queryCrearTablaTrabajador = "CREATE TABLE IF NOT EXISTS Trabajador (" +
                                     "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                     "nombre TEXT, " +
                                     "apellidos TEXT)";
            EjecutarNonQuery(connection, queryCrearTablaTrabajador);
        }

       

        private void EjecutarNonQuery(SQLiteConnection connection, string query)
        {
            // Este método ejecuta órdenes SQL que no devuelven consultas (Non-query command)

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void EntryNombre_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
        {
            _nombre = e.NewTextValue;
        }

        private void EntryApellidos_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
        {
            _apellidos = e.NewTextValue;
        }

        private void ButtonInsertar_Clicked(object sender, EventArgs e)
        {
            // Creamos la ruta completa del fichero de la base de datos
            string rutaDirectorioApp = System.AppContext.BaseDirectory;
            DirectoryInfo directorioApp = new DirectoryInfo(rutaDirectorioApp);
            directorioApp = directorioApp.Parent.Parent.Parent.Parent.Parent.Parent;
            string databasePath = Path.Combine(directorioApp.FullName, "empresa.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Ejecutamos la consulta de inserción
                EjecutarNonQuery(connection, $"INSERT INTO Trabajador (nombre, apellidos) VALUES ('{_nombre}', '{_apellidos}')");

                // Vaciamos la colección actual
                OcTrabajadores.Clear();

                // Volvemos a cargar los trabajadores
                string sql = "SELECT * FROM Trabajador";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idTrabajador = reader.GetInt32(0);
                    string nombreTrabajador = reader.GetString(1);
                    string apellidosTrabajador = reader.GetString(2);

                    Trabajador trabajador = new Trabajador
                    {
                        Id = idTrabajador,
                        Nombre = nombreTrabajador,
                        Apellidos = apellidosTrabajador,
                    };

                    OcTrabajadores.Add(trabajador);
                }

                reader.Close();
                connection.Close();
            }
        }

        private void ButtonEliminar_Clicked(object sender, EventArgs e)
        {
            if (SelectedTrabajador == null)
                return;

            // Creamos la ruta completa del fichero de la base de datos
            string rutaDirectorioApp = System.AppContext.BaseDirectory;
            DirectoryInfo directorioApp = new DirectoryInfo(rutaDirectorioApp);
            directorioApp = directorioApp.Parent.Parent.Parent.Parent.Parent.Parent;
            string databasePath = Path.Combine(directorioApp.FullName, "empresa.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Eliminamos el trabajador de la base de datos
                string query = "DELETE FROM Trabajador WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", SelectedTrabajador.Id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Eliminamos el trabajador de la interfaz
            OcTrabajadores.Remove(SelectedTrabajador);

            // Limpia la selección
            SelectedTrabajador = null;
        }

        private void ListViewTrabajadores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            SelectedTrabajador = e.SelectedItem as Trabajador;

            // Asignar los valores del trabajador seleccionado a los entries
            _nombre = SelectedTrabajador.Nombre;
            _apellidos = SelectedTrabajador.Apellidos;
        }

        private void ButtonActualizar_Clicked(object sender, EventArgs e)
        {
            if (SelectedTrabajador == null)
                return;

            // Actualizar los valores del trabajador seleccionado con los valores de los entries
            SelectedTrabajador.Nombre = _nombre;
            SelectedTrabajador.Apellidos = _apellidos;

            // Creamos la ruta completa del fichero de la base de datos
            string rutaDirectorioApp = System.AppContext.BaseDirectory;
            DirectoryInfo directorioApp = new DirectoryInfo(rutaDirectorioApp);
            directorioApp = directorioApp.Parent.Parent.Parent.Parent.Parent.Parent;
            string databasePath = Path.Combine(directorioApp.FullName, "empresa.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Actualizamos el registro en la base de datos
                string query = "UPDATE Trabajador SET nombre = @nombre, apellidos = @apellidos WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", SelectedTrabajador.Nombre);
                    command.Parameters.AddWithValue("@apellidos", SelectedTrabajador.Apellidos);
                    command.Parameters.AddWithValue("@id", SelectedTrabajador.Id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Actualiza la lista en la interfaz
            var trabajador = OcTrabajadores.FirstOrDefault(t => t.Id == SelectedTrabajador.Id);
            if (trabajador != null)
            {
                trabajador.Nombre = SelectedTrabajador.Nombre;
                trabajador.Apellidos = SelectedTrabajador.Apellidos;

                // Notificar cambios en la colección
                OnPropertyChanged(nameof(OcTrabajadores));
            }
        }

    }
}