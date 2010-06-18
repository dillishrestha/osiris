// created on 24/05/2007 at 06:08 p// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@gmail.com (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)
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
// Programa		: hscmty.cs
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using Gtk;
using Glade;

using System.Data;
using System.Data.Odbc;

namespace osiris
{
	public class herramientas_del_sistemas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_herramientas;
		[Widget] Gtk.Button button_catalogos;
		[Widget] Gtk.Button button_cambios_tabla;
		[Widget] Gtk.Button button_corrige_san_nicolas;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string connectionString;
		string nombrebd;
		int secuencia = 0;
		int admision = 0;
		class_conexion conexion_a_DB = new class_conexion();
		
		public herramientas_del_sistemas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd; 
			
			Glade.XML gxml = new Glade.XML (null, "herramientas.glade", "menu_herramientas", null);
			gxml.Autoconnect (this);        
			////// Muestra ventana de Glade
			menu_herramientas.Show();
			button_cambios_tabla.Clicked += new EventHandler(on_button_cmb_tab_clicked);
			button_catalogos.Clicked += new EventHandler(on_button_catalogos_clicked);
			button_corrige_san_nicolas.Clicked += new EventHandler(on_button_corrige_san_nicolas_clicked);
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
		}
		
		// Asignando numero de factura a los procedimientos
		void on_button_catalogos_clicked(object sender, EventArgs args)
		{
			string tipo_catalogo = verifica_acceso();
			if(tipo_catalogo == "SISTEMAS" ) {new osiris.catalogos_generales("menu",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd); }
			if(tipo_catalogo == "MEDICOS" ) {new osiris.catalogos_generales("medicos",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);}
			if(tipo_catalogo == "NO" ) {MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"NO esta autorizado para accesar");
				  						msgBoxError.Run ();			msgBoxError.Destroy(); }
			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_cmb_tab_clicked(object sender, EventArgs args)
		{
			/*string connectionString = "DNS=huelladigital;"+"UID=usuario;"+"PWD=nac3000";
			
			IDbConnection dbcon;
			dbcon = new OdbcConnection(connectionString);
			dbcon.Open();
			IDbCommand dbcmd = dbcon.CreateCommand();
			
			string sql ="SELECT * FROM NGAC_DEPARTAMENTO;";
			dbcmd.CommandText = sql;
			IDataReader reader = dbcmd.ExecuteReader();
			
			while(reader.Read()){
				Console.WriteLine((string) reader["descripcion"]);
			}
			
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
			dbcon.Close();
			dbcon = null;*/
			
			/*NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+"Database=osiris;");
        	// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char() "+
               						  "FROM inventario_sub_almacenes;";	               						
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string codigodeproducto = "";
				  
				while(lector.Read()){
					codigodeproducto = (int) lector["numero_factura"];
					//Console.WriteLine(numerofactura.ToString());
					
					NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+"Database=osiris;");
		        	// Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
		               	comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,1)+"',"+
												"historial_ajustes = historial_ajustes || 'DOLIVARES";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
												Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)).Trim()+";"+
												Convert.ToString((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
												"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
												"AND eliminado = 'false' " +
												"AND id_producto = '"+(string)this.lista_almacenes.Model.GetValue(iterSelected2,3)+"' ;";	               						
						//Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						if(lector1.Read()){
							
						}else{
							Console.WriteLine("NO ENCONTRE LA FACTURA; "+numerofactura.ToString().Trim());
						}
						
					}catch(NpgsqlException ex){
						Console.WriteLine("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
					}
					conexion1.Close();
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();*/
		}
				
		void on_button_corrige_san_nicolas_clicked(object sender, EventArgs args)
		{
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN"){
				
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	        	// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	               	comando.CommandText = "SELECT to_char(folio_de_servicio,'9999999999') AS foliodeservicio,"+
	               						  "to_char(id_producto,'999999999999') AS idproducto,"+
	               						  "to_char(precio_producto,'99999999.999') AS precioproducto "+
	               						  "FROM modificaciones_san_nicolas "+
	               						  "WHERE actualizado = false;";	               						
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					string folioservicio_ = "";
					string id_producto_ = "";
					string preciopruducto_ = "";  
					while(lector.Read())
					{
						folioservicio_ = (string) lector["foliodeservicio"];
						id_producto_ = (string) lector["idproducto"];
						preciopruducto_ = (string) lector["precioproducto"];
						
						Console.WriteLine("folioservicio_ = "+folioservicio_ +" id_producto_ = "+id_producto_);
						NpgsqlConnection conexion2; 
						conexion2 = new NpgsqlConnection (connectionString+"Database=osiris;");
			            try{
							conexion2.Open ();
							NpgsqlCommand comando2; 
							comando2 = conexion2.CreateCommand ();
							comando2.CommandText ="UPDATE osiris_erp_cobros_deta SET precio_producto = '"+preciopruducto_+"',"+
												"precio_por_cantidad = '"+preciopruducto_+"' * cantidad_aplicada "+
												" WHERE folio_de_servicio = '"+folioservicio_+"' AND id_producto = '"+id_producto_+"'; ";		           
							
							//Console.WriteLine(comando2.CommandText);
							
							comando2.ExecuteNonQuery();
							comando2.Dispose();
							
							
						
						}catch(NpgsqlException ex){
							Console.WriteLine("subquery"+"PostgresSQL error: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
						conexion2.Close();
					
					}
											
					NpgsqlConnection conexion3; 
					conexion3 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion3.Open ();
						NpgsqlCommand comando3; 
						comando3 = conexion.CreateCommand ();
			            comando3.CommandText = "UPDATE modificaciones_san_nicolas SET fecha_actualizacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
			               							"actualizado = true WHERE actualizado = false;";
			            comando3.ExecuteNonQuery();
						comando3.Dispose();							
						conexion3.Close();
						
					}catch(NpgsqlException ex){
						Console.WriteLine("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
					}
				}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close();
			}else{ MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				  MessageType.Error,ButtonsType.Close,"NO esta autorizado para accesar");
				  msgBoxError.Run ();			msgBoxError.Destroy(); 
			}			
		}
		
		public string verifica_acceso()
		{
			string varpaso = "NO";
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" ){
				varpaso = "SISTEMAS";
				return varpaso;
			}
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" || LoginEmpleado == "RIOSGARCIA"){
			  varpaso = "MEDICOS";
			  return varpaso;
			}
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" || LoginEmpleado == "SZALETAGONZALEZ"){
				varpaso = "RH"; 
				return varpaso;
			}else{
				return varpaso;
			} 	
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}