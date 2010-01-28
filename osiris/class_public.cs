//////////////////////////////////////////////////////////
// project created on 04/01/2010 at 10:20 a   
// Monterrey - Mexico
// 
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;
using System.Security.Cryptography;

namespace osiris
{
	public class class_public
	{		
		public string LoginUsuario = "";
		public string NombrUsuario = "";
		public string idUsuario = "";
		
		// Informacion de la Empresa
		public string nombre_empresa = "";
		public string direccion_empresa = "";
		public string telefonofax_empresa = "";
		public string version_sistema = "0.1";
		
		public string ivaparaaplicar = "16.00";
		
		// variable para la conexion---> los valores estan en facturador.cs
		string connectionString = "";
		string nombrebd = "";
		
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		// Funcion de Encriptacion en MD5 para las contraseñas de usuarios
		public string CreatePasswordMD5(string password)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(password);
			bs = md5.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs){
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();			
		}
		
		public string lee_ultimonumero_registrado(string name_table,string name_field,string condition_table)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "1";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char("+name_field+",'9999999999') AS field_last_number FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){	
					tomavalor = (int.Parse((string) lector["field_last_number"])+1).ToString();
					conexion.Close();
					return tomavalor;					
				}else{
					conexion.Close();
					return tomavalor;					
				}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
				conexion.Close();
				return tomavalor;
			}
		}
	}
}