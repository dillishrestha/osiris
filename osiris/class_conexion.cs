using System;

namespace osiris
{
	/// <summary>
	/// Classe para la conexion de la base de datos
	/// </summary>
	class class_conexion
	{
		public string _url_servidor = "Server=192.168.1.10;";
        public string _port_DB = "Port=5432;";
        public string _usuario_DB = "User ID=admin;";
        public string _passwrd_user_DB = "Password=1qaz2wsx123;";
		public string _nombrebd = "Database=osiris_produccion";
	}
}