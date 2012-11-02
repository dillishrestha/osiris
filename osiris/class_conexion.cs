using System;

namespace osiris
{
	/// <summary>
	/// Classe para la conexion de la base de datos
	/// </summary>
	class class_conexion
	{
		public string _url_servidor = "Server=localhost;";
		public string _port_DB = "Port=5432;";
		public string _usuario_DB = "User ID=admin;";
		public string _passwrd_user_DB = "Password=pwd;";
		public string _nombrebd = "Database=osiris_produccion;";
	}
}