// created on 17/07/2010
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	:  Ing. Daniel Olivares C. (Programacion Base y Ajustes)
//
// Licencia		: GLP
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osiris is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: 
// Proposito	: 
// Objeto		: 
/////////////////////////////////////////////////////////

using System;
using Npgsql;
using Gtk;
using Glade;
using Gdk;

namespace osiris
{
	public class analisis_devoluciones
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana principal
		[Widget] Gtk.Window resumen_devoluciones = null;
		[Widget] Gtk.Statusbar statusbar_analisisdevol = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_selecciona_folio = null;
		[Widget] Gtk.Button button_buscar_paciente = null;
		[Widget] Gtk.TextView textview1 = null;
		
		TextBuffer buffer = new TextBuffer (null);
		
		string LoginEmpleado;
		string NomEmpleado; 
		string AppEmpleado; 
		string ApmEmpleado;
			
		string connectionString;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public analisis_devoluciones(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_; 
			AppEmpleado = AppEmpleado_; 
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			//Console.WriteLine("antes de abrir venmtana");
			Glade.XML gxml = new Glade.XML (null,"almacen_costos_compras.glade","resumen_devoluciones",null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			resumen_devoluciones.Show();
			
			statusbar_analisisdevol.Pop(0);
			statusbar_analisisdevol.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_analisisdevol.HasResizeGrip = false;
			
			entry_folio_servicio.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			// Validando que sen solo numeros
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			button_selecciona_folio.Clicked += new EventHandler(on_button_selecciona_folio_clicked);
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
		}
		
		void on_button_selecciona_folio_clicked(object obj, EventArgs args)
		{
			llenado_de_devoluciones();	
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    {
			object[] parametros_objetos = {entry_folio_servicio,entry_pid_paciente,entry_nombre_paciente};
			string[] parametros_sql = {"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' ",															
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND apellido_paterno_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND nombre1_paciente LIKE '%",
									"SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente,osiris_erp_cobros_enca WHERE activo = 'true' "+
										"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+
										"AND osiris_erp_cobros_enca.alta_paciente = false "+
										"AND osiris_erp_cobros_enca.cancelado = false "+
										"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cerrado = 'false' "+
										"AND osiris_erp_cobros_enca.reservacion = 'false' "+
										"AND osiris_his_paciente.pid_paciente = '"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_paciente"," ORDER BY osiris_his_paciente.pid_paciente","%' ",1);
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenado_de_devoluciones();		
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
		
		void llenado_de_devoluciones()
		{
			buffer.Clear();
			buffer = textview1.Buffer;			
			TextIter insertIter = buffer.StartIter;
			classpublic.CreateTags(buffer);
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT osiris_erp_cobros_deta.id_producto AS idproducto,SUM(osiris_erp_cobros_deta.cantidad_aplicada)AS cantidad_aplicada,osiris_his_solicitudes_deta.id_producto,"+
									"SUM(osiris_his_solicitudes_deta.cantidad_autorizada) AS cantidad_autorizada,descripcion_producto "+
									"FROM osiris_erp_cobros_deta,osiris_his_solicitudes_deta,osiris_productos "+
									"WHERE osiris_erp_cobros_deta.folio_de_servicio = osiris_his_solicitudes_deta.folio_de_servicio "+ 
									"AND osiris_erp_cobros_deta.id_producto = osiris_his_solicitudes_deta.id_producto AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
									"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) entry_folio_servicio.Text.ToString().Trim()+"' "+
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_his_solicitudes_deta.id_producto,descripcion_producto ORDER BY osiris_erp_cobros_deta.id_producto;";
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if(lector.Read()){
					while(lector.Read()){
						buffer.InsertWithTagsByName (ref insertIter, (string) lector["idproducto"].ToString()+" "+(string) lector["descripcion_producto"].ToString()+"\n","not_editable");											
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Error,
			 							ButtonsType.Close, "Este numero de atencion no tiene cargo verifique");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			//buffer.InsertWithTagsByName (ref insertIter, "\nThis line has center justification.\n", "center");
		}
		
		void on_cierraventanas_clicked (object obj, EventArgs args)
		{
			Widget win = (Widget) obj;
			win.Toplevel.Destroy();
		}
	}
}
