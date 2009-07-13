///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
// Colaboration:  Ing
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
// Programa		: hscmty.cs
// Proposito	: Pagos en Caja 
// Objeto		: caja.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class seleccion_folio
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de pago
		[Widget] Gtk.Window busqueda_folio;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_fecha_admision;
		[Widget] Gtk.Entry entry_hora_registro;
		[Widget] Gtk.Entry entry_fechahora_alta;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_telefono_paciente;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Entry entry_especialidad;
		[Widget] Gtk.Entry entry_tipo_paciente;
		[Widget] Gtk.Entry entry_aseguradora;
		[Widget] Gtk.Entry entry_poliza;
		[Widget] Gtk.Button button_buscar_paciente;
		[Widget] Gtk.Button button_selec_folio;
		[Widget] Gtk.Button button_imprimir_protocolo;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_caja;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto;
								
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineServicio;
		private ListStore treeViewEngineExtras;
		private ArrayList arraycargosextras;
		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		// Declaracion de variables publicas
		public int folioservicio = 0;	    // Toma el valor de numero de atencion de paciente
		public int PidPaciente = 0;		    // Toma la actualizacion del pid del paciente
		public int id_tipopaciente;         // Toma el valor del tipo de paciente
		public int idtipointernamiento = 10;// Toma el valor del tipo de internamiento
		
		public string nombrebd;
		public string LoginEmpleado;
			
		public string connectionString = "Server=192.168.1.4;" +
						"Port=5432;" +
						"User ID=admin;" +
						"Password=1qaz2wsx;";
						
		protocolo_admision proadmi;
		
		public seleccion_folio(string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string _nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			nombrebd = _nombrebd_; 
			
			Glade.XML gxml = new Glade.XML (null, "imp_prot.glade", "busqueda_folio", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			busqueda_folio.Show();

			entry_fecha_admision.Sensitive = false;
			entry_hora_registro.Sensitive = false;
			entry_fechahora_alta.Sensitive = false;
			entry_nombre_paciente.Sensitive = false;
			entry_pid_paciente.Sensitive = false;
			entry_telefono_paciente.Sensitive = false;
			entry_cirugia.Sensitive = false;
			entry_doctor.Sensitive = false;
			entry_especialidad.Sensitive = false;
			entry_tipo_paciente.Sensitive = false;
			entry_aseguradora.Sensitive = false;
			entry_poliza.Sensitive = false;
				    	
			// Activacion de boton de busqueda
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			// Voy a buscar el folio que capturo
			button_selec_folio.Clicked += new EventHandler(on_selec_folio_clicked);
			// Imprimir
			button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
	    	
			statusbar_caja.Pop(0);
			statusbar_caja.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_caja.HasResizeGrip = false;
	    	
	    
			// convierte un string a numero
			//string  prueba;
			//prueba=this.folioservicio.ToString();
			//prueba=int.Parse("787878");
		}
		// Imprime protocolo de admision
		void on_button_imprimir_protocolo_clicked (object sender, EventArgs args)
		{
			
			if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" )
		    {	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
							"existente para que el protocolo se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				int result = msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				proadmi = new protocolo_admision(PidPaciente,int.Parse(entry_folio_servicio.Text),nombrebd);   // rpt_prot_admision.cs
			}
		}
		void on_selec_folio_clicked (object sender, EventArgs args)
		{
			llenado_de_productos_aplicados( (string) entry_folio_servicio.Text );
		}
		
		void llenado_de_productos_aplicados(string foliodeservicio)
		{
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio AS foliodeatencion,to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
            				"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
            				"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,"+
            				"to_char(hscmty_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy') AS fecha_registro,"+
            				"to_char(hscmty_erp_cobros_enca.fechahora_creacion,'HH:mm:ss') AS hora_registro,"+
            				"to_char(hscmty_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH:mm:ss') AS fechahora_alta,"+
            				"hscmty_erp_movcargos.id_tipo_paciente AS idtipopaciente,descripcion_tipo_paciente,"+
            				"hscmty_erp_movcargos.id_tipo_admisiones AS idtipoadmision,descripcion_admisiones,"+
            				"hscmty_erp_cobros_enca.id_aseguradora,descripcion_aseguradora, "+
            				"hscmty_erp_cobros_enca.id_medico,nombre_medico "+
            				"FROM hscmty_erp_cobros_enca,hscmty_his_paciente,hscmty_erp_movcargos,hscmty_his_tipo_admisiones,hscmty_his_tipo_pacientes, "+
            				"hscmty_aseguradoras, hscmty_his_medicos "+
            				"WHERE hscmty_erp_cobros_enca.pid_paciente = hscmty_his_paciente.pid_paciente "+
            				"AND hscmty_erp_movcargos.folio_de_servicio = hscmty_erp_cobros_enca.folio_de_servicio "+
            				"AND pagado = false "+
            				"AND hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+ 
            				"AND hscmty_erp_cobros_enca.id_aseguradora = hscmty_aseguradoras.id_aseguradora "+ 
            				"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
            				"AND hscmty_erp_movcargos.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente "+
            				"AND hscmty_erp_cobros_enca.folio_de_servicio = '"+(string) foliodeservicio+"';";
				NpgsqlDataReader lector = comando.ExecuteReader ();
            	
				while (lector.Read())
				{
					entry_fecha_admision.Text = (string) lector["fecha_registro"];
					entry_hora_registro.Text = (string) lector["hora_registro"];
					entry_fechahora_alta.Text = (string) lector["fechahora_alta"];
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"]+" "+(string)+" "+lector["nombre2_paciente"]+" "+lector["apellido_paterno_paciente"]+" "+lector["apellido_materno_paciente"];
					entry_pid_paciente.Text = (string) lector["pidpaciente"];
					entry_telefono_paciente.Text = (string) lector["telefono_particular1_paciente"];
					entry_doctor.Text = (string) lector["nombre_medico"];
					entry_tipo_paciente.Text = (string) lector["descripcion_tipo_paciente"];
					entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
					entry_poliza.Text =  (string) lector["numero_poliza"];
					id_tipopaciente = (int) lector["idtipopaciente"];
            		
					int foliointernodep = (int) lector["folio_de_servicio_dep"];
					
					PidPaciente = int.Parse(entry_pid_paciente.Text);		    // Toma la actualizacion del pid del paciente
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	       	}
       		conexion.Close ();
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    	{
			Glade.XML gxml = new Glade.XML (null, "imp_prot.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("paciente");
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_paciente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			button_nuevo_paciente.Sensitive = false;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
	    
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "paciente")
			{
				treeViewEngineBusca = new TreeStore(typeof(int),typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string) );
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_foliodeatencion = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
				col_foliodeatencion.PackStart(cellr0, true);
				col_foliodeatencion.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
			
				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr1, true);
				col_PidPaciente.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt2, true);
				col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt3, true);
				col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt4, true);
				col_app_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt5, true);
				col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
				col_fechanacimiento_Paciente.PackStart(cellrt6, true);
				col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
				TreeViewColumn col_edad_Paciente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_edad_Paciente.Title = "Edad";
				col_edad_Paciente.PackStart(cellrt7, true);
				col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
				TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_sexo_Paciente.Title = "Sexo";
				col_sexo_Paciente.PackStart(cellrt8, true);
				col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt9, true);
				col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;

				lista_de_Pacientes.AppendColumn(col_foliodeatencion);
				lista_de_Pacientes.AppendColumn(col_PidPaciente);
				lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
				lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
				lista_de_Pacientes.AppendColumn(col_app_Paciente);
				lista_de_Pacientes.AppendColumn(col_apm_Paciente);
				lista_de_Pacientes.AppendColumn(col_fechanacimiento_Paciente);
				lista_de_Pacientes.AppendColumn(col_edad_Paciente);
				lista_de_Pacientes.AppendColumn(col_sexo_Paciente);
				lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
			}
		}
			
		enum Column
		{
			col_foliodeatencion,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente
		}
				
		// activa busqueda con boton busqueda de paciente
		// y llena la lista con los pacientes
		
		void on_buscar_paciente_clicked (object sender, EventArgs args)
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	               	
				if (radiobutton_busca_apellido.Active == true)
				{
					comando.CommandText = "SELECT hscmty_erp_cobros_enca.folio_de_servicio,hscmty_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion "+
										"FROM hscmty_his_paciente,hscmty_erp_cobros_enca "+
										"WHERE hscmty_his_paciente.pid_paciente = hscmty_erp_cobros_enca.pid_paciente "+ 
										"AND hscmty_erp_cobros_enca.pagado = 'false' "+
										"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY pid_paciente;";
								
				}
				if (radiobutton_busca_nombre.Active == true)
				{
					comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion "+
										"FROM hscmty_his_paciente WHERE nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY pid_paciente;";
				}
				if (radiobutton_busca_expediente.Active == true)
				{
					comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion "+
										"FROM hscmty_his_paciente WHERE pid_paciente = '"+entry_expresion.Text+"' ORDER BY pid_paciente;";					
				}
				if ((string) entry_expresion.Text.ToString() == "*")
				{
					comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mm:ss') AS fech_creacion "+
										"FROM hscmty_his_paciente ORDER BY pid_paciente;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					TreeIter iter = treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],
										(int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
				}
				
				
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
		
		void on_selecciona_paciente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				 folioservicio = (int) model.GetValue(iterSelected, 0);
 				 entry_folio_servicio.Text = folioservicio.ToString();
 				 
 				 llenado_de_productos_aplicados(folioservicio.ToString());
 				 
 			}
 			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
 				
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}
