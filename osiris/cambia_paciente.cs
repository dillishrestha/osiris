// created on 24/01/2008 at 12:13 p
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: 	Homero Montoya (Programacion y diseño glade)
//					Daniel Olivares - arcangeldoc@gmail.com (pre-Programacion)
//				  (Programacion Mono)
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
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
		public class cambia_paciente
		{
		//Declarando ventana de cambios de datos de paciente
		[Widget] Gtk.Window cambio_datos_paciente;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_fechahora_registro;
		[Widget] Gtk.Entry entry_nombre1;
		[Widget] Gtk.Entry entry_nombre2;
		[Widget] Gtk.Entry entry_apellido_paterno;
		[Widget] Gtk.Entry entry_apellido_materno;
		[Widget] Gtk.Entry entry_estado_civil;
		[Widget] Gtk.Entry entry_ocupacion;
		[Widget] Gtk.Entry entry_fechanac;
		[Widget] Gtk.Entry entry_sexo;
		[Widget] Gtk.Entry entry_titulopac;
		[Widget] Gtk.Entry entry_tipo_sangre;
		[Widget] Gtk.Entry entry_curp;
		[Widget] Gtk.Entry entry_rfc;
		[Widget] Gtk.Entry entry_calle;
		[Widget] Gtk.Entry entry_colonia;
		[Widget] Gtk.Entry entry_numeroext;
		[Widget] Gtk.Entry entry_numeroint;
		[Widget] Gtk.Entry entry_cp;
		[Widget] Gtk.ComboBox combobox_estado;
		[Widget] Gtk.ComboBox combobox_municipios;
		[Widget] Gtk.Entry entry_tel1;
		[Widget] Gtk.Entry entry_tel2;
		[Widget] Gtk.Entry entry_fax;
		[Widget] Gtk.Entry entry_telefono_trab1;
		[Widget] Gtk.Entry entry_telefono_trab2;
		[Widget] Gtk.Entry entry_correo;
		[Widget] Gtk.Entry entry_celular1;
		[Widget] Gtk.Entry entry_celular2;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.Entry entry_fecha_defuncion;
		[Widget] Gtk.Entry entry_causa_defuncion;
		[Widget] Gtk.Entry entry_edad;
		[Widget] Gtk.Button button_seleccionar;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Button button_editar;
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		private TreeStore treeViewEngineBusca;
		//private TreeStore treeViewEngineBusca;
		//private TreeStore treeViewEngineBusca2;
		
		//Declaracion de variables
		//variables principales
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string nombrebd;
		public string nomcatalogo;
		public string busqueda = "";
		public string connectionString = "Server=localhost;" +
									"Port=5432;" +
									 "User ID=admin;" +
									"Password=1qaz2wsx;";		
		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public int PidPaciente = 0;							// Toma la actualizacion del pid del paciente
				
		public CellRendererText cel_descripcion;
		
		//Declarando las celdas
		public CellRendererText cellr0;				public CellRendererText cellrt1;
		public CellRendererText cellrt2;			public CellRendererText cellrt3;
		public CellRendererText cellrt4;			public CellRendererText cellrt5;
		public CellRendererText cellrt6;			public CellRendererText cellrt7;
		public CellRendererText cellrt8;			
				
		public string municipios = "";
		public string estado = "";
		public int idestado = 1;
		
		
		public cambia_paciente (string LoginEmpleado_,string NomEmpleado_,string AppEmpleado_,string ApmEmpleado_,string nombrebd_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = nombrebd_;
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cambio_datos_paciente", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        //Muestra ventana de Glade
			cambio_datos_paciente.Show();
			button_buscar.Clicked += new EventHandler(on_button_busca_pacientes_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_editar.Clicked += new EventHandler(on_button_editar_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_seleccionar.Sensitive = false;
			button_editar.Sensitive = false;
			button_guardar.Sensitive = false;
		}	
		
		void on_button_editar_clicked(object sender, EventArgs args)
		{
			activa_los_entry(true);
		}
		
		void on_button_busca_pacientes_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
        	busca_paciente.Show();
            crea_treeview_busqueda("paciente");
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_busqueda_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_nuevo_paciente.Sensitive = false;
			button_selecciona.Sensitive = false;
		}
		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "paciente")
			{
				treeViewEngineBusca = new TreeStore(typeof(int),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
													
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr0, true);
				col_PidPaciente.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 0
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
			
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt1, true);
				col_Nombre1_Paciente.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2 en vez de 1
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt2, true);
				col_Nombre2_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3 en vez de 2
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt3, true);
				col_app_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 4 en vez de 3
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt4, true);
				col_apm_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 5 en vez de 4
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt5, true);
				col_creacion_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 69 en vez de 5
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;
				
				TreeViewColumn col_fecha_nacimiento = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fecha_nacimiento.Title = "Fecha Nacimiento";
				col_fecha_nacimiento.PackStart(cellrt6, true);
				col_fecha_nacimiento.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7 en vez de 6
				col_fecha_nacimiento.SortColumnId = (int) Column.col_fecha_nacimiento;
				
				TreeViewColumn col_tipo_sangre = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_tipo_sangre.Title = "Tipo de Sangre";
				col_tipo_sangre.PackStart(cellrt7, true);
				col_tipo_sangre.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 7
				col_tipo_sangre.SortColumnId = (int) Column.col_tipo_sangre;
				
				TreeViewColumn col_tel1_paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_tel1_paciente.Title = "Telefono 1";
				col_tel1_paciente.PackStart(cellrt8, true);
				col_tel1_paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 9 en vez de 8
				col_tel1_paciente.SortColumnId = (int) Column.col_tel1_paciente;
				
				lista_de_Pacientes.AppendColumn(col_PidPaciente);
				lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
				lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
				lista_de_Pacientes.AppendColumn(col_app_Paciente);
				lista_de_Pacientes.AppendColumn(col_apm_Paciente);
				lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
				lista_de_Pacientes.AppendColumn(col_fecha_nacimiento);
				lista_de_Pacientes.AppendColumn(col_tipo_sangre);
				lista_de_Pacientes.AppendColumn(col_tel1_paciente);
			}
			
		}
			
		enum Column
		{
			col_PidPaciente,
			col_creacion_Paciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fecha_nacimiento,
			col_tipo_sangre,
			col_tel1_paciente
		}
		
		// activa busqueda con boton busqueda
		void on_buscar_busqueda_clicked (object sender, EventArgs a)
		{
			llena_lista_paciente();
		}		
		
		// activa busqueda con boton busqueda de paciente
		// y llena la lista con los pacientes
		void llena_lista_paciente ()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	if ((string) entry_expresion.Text.ToString() == ""){
					comando.CommandText ="SELECT nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,"+
									"apellido_materno_paciente,grupo_sanguineo_paciente,"+
									"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,"+
									"telefono_particular1_paciente,telefono_particular2_paciente,telefono_trabajo1_paciente,"+
									"telefono_trabajo2_paciente,celular1_paciente,celular2_paciente,fax_paciente,email_paciente,"+
									"estado_civil_paciente,sexo_paciente,titulo_paciente,curp_paciente,rfc_paciente,"+
									"colonia_paciente,estado_paciente,fecha_muerte_paciente,causa_muerte_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(hscmty_his_paciente.fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento, "+
									"to_char(hscmty_his_paciente.fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, "+
									"fecha_cambio_info_paciente,historia_movimientos_paciente,id_quien_modifico_paciente,"+
									"id_quienlocreo_paciente,pid_paciente,id_linea,"+
									"ocupacion_paciente,id_empresa,municipio_paciente,activo,observaciones "+
									"FROM hscmty_his_paciente "+
									"WHERE hscmty_his_paciente.activo = true "+
									"ORDER BY hscmty_his_paciente.id_linea;";
					//Console.WriteLine(comando.CommandText);
				}else{              	
					if (radiobutton_busca_apellido.Active == true){
						comando.CommandText = "SELECT nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,"+
									"apellido_materno_paciente,grupo_sanguineo_paciente,"+
									"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,"+
									"telefono_particular1_paciente,telefono_particular2_paciente,telefono_trabajo1_paciente,"+
									"telefono_trabajo2_paciente,celular1_paciente,celular2_paciente,fax_paciente,email_paciente,"+
									"estado_civil_paciente,sexo_paciente,titulo_paciente,curp_paciente,rfc_paciente,"+
									"colonia_paciente,estado_paciente,fecha_muerte_paciente,causa_muerte_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(hscmty_his_paciente.fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento, "+
									"to_char(hscmty_his_paciente.fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, "+
									"fecha_cambio_info_paciente,historia_movimientos_paciente,id_quien_modifico_paciente,"+
									"id_quienlocreo_paciente,pid_paciente,id_linea,"+
									"ocupacion_paciente,id_empresa,municipio_paciente,activo,observaciones "+
									"FROM hscmty_his_paciente "+
									"WHERE hscmty_his_paciente.activo = true "+
									"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"ORDER BY hscmty_his_paciente.id_linea;";
					}
					if (radiobutton_busca_nombre.Active == true){
						comando.CommandText =  "SELECT nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,"+
									"apellido_materno_paciente,grupo_sanguineo_paciente,"+
									"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,"+
									"telefono_particular1_paciente,telefono_particular2_paciente,telefono_trabajo1_paciente,"+
									"telefono_trabajo2_paciente,celular1_paciente,celular2_paciente,fax_paciente,email_paciente,"+
									"estado_civil_paciente,sexo_paciente,titulo_paciente,curp_paciente,rfc_paciente,"+
									"colonia_paciente,estado_paciente,fecha_muerte_paciente,causa_muerte_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(hscmty_his_paciente.fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento, "+
									"to_char(hscmty_his_paciente.fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, "+
									"fecha_cambio_info_paciente,historia_movimientos_paciente,id_quien_modifico_paciente,"+
									"id_quienlocreo_paciente,pid_paciente,id_linea,"+
									"ocupacion_paciente,id_empresa,municipio_paciente,activo,observaciones "+
									"FROM hscmty_his_paciente "+
									"WHERE hscmty_his_paciente.activo = true "+
									"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"ORDER BY hscmty_his_paciente.id_linea;";
					}
					if (radiobutton_busca_expediente.Active == true){
						comando.CommandText = "SELECT nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,"+
									"apellido_materno_paciente,grupo_sanguineo_paciente,"+
									"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,"+
									"telefono_particular1_paciente,telefono_particular2_paciente,telefono_trabajo1_paciente,"+
									"telefono_trabajo2_paciente,celular1_paciente,celular2_paciente,fax_paciente,email_paciente,"+
									"estado_civil_paciente,sexo_paciente,titulo_paciente,curp_paciente,rfc_paciente,"+
									"colonia_paciente,estado_paciente,fecha_muerte_paciente,causa_muerte_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(hscmty_his_paciente.fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento, "+
									"to_char(hscmty_his_paciente.fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion, "+
									"fecha_cambio_info_paciente,historia_movimientos_paciente,id_quien_modifico_paciente,"+
									"id_quienlocreo_paciente,pid_paciente,id_linea,"+
									"ocupacion_paciente,id_empresa,municipio_paciente,activo,observaciones "+
									"FROM hscmty_his_paciente "+
									"WHERE hscmty_his_paciente.activo = true "+
									"AND pid_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' "+
									"ORDER BY id_linea;";
					}
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],
										(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"],
										(string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], 
										(string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
				}				
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_paciente_clicked(object sender, EventArgs a)
		{	
			TreeModel model;
			TreeIter iterSelected;
			if (this.lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) {
				PidPaciente = (int) model.GetValue(iterSelected, 0);
				activa_los_entry(true);
				llena_inf_de_paciente();
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
				
		// Procedimiento para el llenado de los datos del paciente
		void llena_inf_de_paciente()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText ="SELECT to_char(pid_paciente,'9999999999') AS pidpaciente, "+ 
									"nombre1_paciente,"+
									"nombre2_paciente,"+
									"apellido_paterno_paciente,"+
									"apellido_materno_paciente,"+
									"grupo_sanguineo_paciente,"+
									"direccion_paciente,"+
									"numero_casa_paciente,"+
									"numero_departamento_paciente,"+
									"codigo_postal_paciente,"+
									"telefono_particular1_paciente,"+
									"telefono_particular2_paciente,"+
									"telefono_trabajo1_paciente,"+
									"telefono_trabajo2_paciente,"+
									"celular1_paciente,"+
									"celular2_paciente,"+
									"fax_paciente,"+
									"email_paciente,"+
									"sexo_paciente,"+
									"estado_civil_paciente,"+
									"titulo_paciente,"+
									"curp_paciente,"+
									"rfc_paciente,"+
									"colonia_paciente,"+
									"estado_paciente,"+
									"to_char(hscmty_his_paciente.fecha_muerte_paciente,'yyyy-MM-dd HH:mi:ss') AS fech_muerte,"+
									"causa_muerte_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(hscmty_his_paciente.fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento, "+
									"to_char(hscmty_his_paciente.fechahora_registro_paciente,'yyyy-MM-dd HH:mi:ss') AS fech_creacion, "+
									"fecha_cambio_info_paciente,"+
									"historia_movimientos_paciente,"+
									"id_quien_modifico_paciente,"+
									"id_quienlocreo_paciente,"+
									"id_linea,"+
									"ocupacion_paciente,"+
									"id_empresa,"+
									"municipio_paciente,"+
									"activo,"+
									"observaciones "+
									"FROM hscmty_his_paciente "+
									"WHERE hscmty_his_paciente.activo = true "+
									"AND pid_paciente = '"+PidPaciente.ToString()+"' ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					entry_pid_paciente.Text = (string) lector["pidpaciente"];
					entry_nombre1.Text = (string) lector["nombre1_paciente"];
					entry_nombre2.Text = (string) lector["nombre2_paciente"];
					entry_apellido_paterno.Text = (string) lector["apellido_paterno_paciente"];
					entry_apellido_materno.Text = (string) lector["apellido_materno_paciente"];
					entry_fechanac.Text = (string) lector["fech_nacimiento"];
					entry_sexo.Text = (string) lector["sexo_paciente"];
					entry_fechahora_registro.Text = (string) lector["fech_creacion"];
					entry_rfc.Text = (string) lector["rfc_paciente"];
					entry_curp.Text = (string) lector["curp_paciente"];
					entry_ocupacion.Text = (string) lector["ocupacion_paciente"];
					entry_calle.Text = (string) lector["direccion_paciente"];
					entry_numeroext.Text = (string) lector["numero_casa_paciente"];
					entry_colonia.Text = (string) lector["colonia_paciente"];
					entry_cp.Text = (string) lector["codigo_postal_paciente"];
					entry_tel1.Text = (string) lector["telefono_particular1_paciente"];
					entry_telefono_trab1.Text = (string) lector["telefono_trabajo1_paciente"];
					entry_celular1.Text = (string) lector["celular1_paciente"];
					entry_estado_civil.Text = (string) lector["estado_civil_paciente"];
					entry_titulopac.Text = (string) lector["titulo_paciente"];
					entry_tipo_sangre.Text = (string) lector["grupo_sanguineo_paciente"];
					entry_numeroint.Text = (string) lector["numero_departamento_paciente"];
					municipios = (string) lector["municipio_paciente"];
					estado = (string) lector["estado_paciente"];
					entry_tel2.Text = (string) lector["telefono_trabajo2_paciente"];
					entry_fax.Text = (string) lector["fax_paciente"];
					entry_telefono_trab2.Text = (string) lector["telefono_trabajo2_paciente"];
					entry_correo.Text = (string) lector["email_paciente"];
					entry_celular2.Text = (string) lector["celular2_paciente"];
					entry_observaciones.Text = (string) lector["observaciones"];
					entry_fecha_defuncion.Text = (string) lector["fech_muerte"]; 
					entry_causa_defuncion.Text = (string) lector["causa_muerte_paciente"];
					entry_edad.Text = (string) lector["edad"];
					llenado_estados("selecciona",estado,0);
					llenado_municipios("selecciona",municipios);
					button_guardar.Sensitive = true;
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void desactiva_entrys(bool valor)
		{
			entry_pid_paciente.Sensitive = false;
			entry_fechahora_registro.Sensitive = false;
			entry_nombre1.Sensitive = false;
			entry_nombre2.Sensitive = false;
			entry_apellido_paterno.Sensitive = false;
			entry_apellido_materno.Sensitive = false;
			entry_estado_civil.Sensitive = false;
			entry_ocupacion.Sensitive = false;
			entry_fechanac.Sensitive = false;
			entry_sexo.Sensitive = false;
			entry_titulopac.Sensitive = false;
			entry_tipo_sangre.Sensitive = false;
			entry_curp.Sensitive = false;
			entry_rfc.Sensitive = false;
			entry_calle.Sensitive = false;
			entry_colonia.Sensitive = false;
			entry_numeroext.Sensitive = false;
			entry_numeroint.Sensitive = false;
			entry_cp.Sensitive = false;
			combobox_estado.Sensitive = false;
			combobox_municipios.Sensitive = false;
			entry_tel1.Sensitive = false;
			entry_tel2.Sensitive = false;
			entry_fax.Sensitive = false;
			entry_telefono_trab1.Sensitive = false;
			entry_telefono_trab2.Sensitive = false;
			entry_correo.Sensitive = false;
			entry_celular1.Sensitive = false;
			entry_celular2.Sensitive = false;
			entry_observaciones.Sensitive = false;
			entry_fecha_defuncion.Sensitive = false;
			entry_causa_defuncion.Sensitive = false;
			entry_edad.Sensitive = false;
		}
		
		void activa_los_entry(bool valor)
		{
			entry_pid_paciente.Sensitive = valor;
			entry_fechahora_registro.Sensitive = valor;
			entry_nombre1.Sensitive = valor;
			entry_nombre2.Sensitive = valor;
			entry_apellido_paterno.Sensitive = valor;
			entry_apellido_materno.Sensitive = valor;
			entry_estado_civil.Sensitive = valor;
			entry_ocupacion.Sensitive = valor;
			entry_fechanac.Sensitive = valor;
			entry_sexo.Sensitive = valor;
			entry_titulopac.Sensitive = valor;
			entry_tipo_sangre.Sensitive = valor;
			entry_curp.Sensitive = valor;
			entry_rfc.Sensitive = valor;
			entry_calle.Sensitive = valor;
			entry_colonia.Sensitive = valor;
			entry_numeroext.Sensitive = valor;
			entry_numeroint.Sensitive = valor;
			entry_cp.Sensitive = valor;
			combobox_estado.Sensitive = valor;
			combobox_municipios.Sensitive = valor;
			entry_tel1.Sensitive = valor;
			entry_tel2.Sensitive = valor;
			entry_fax.Sensitive = valor;
			entry_telefono_trab1.Sensitive = valor;
			entry_telefono_trab2.Sensitive = valor;
			entry_correo.Sensitive = valor;
			entry_celular1.Sensitive = valor;
			entry_celular2.Sensitive = valor;
			entry_observaciones.Sensitive = valor;
			entry_fecha_defuncion.Sensitive = valor;
			entry_causa_defuncion.Sensitive = valor;
			entry_edad.Sensitive = valor;
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try
				{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "UPDATE hscmty_his_paciente "+
									"SET activo = 'true', "+
									"nombre1_paciente = '"+this.entry_nombre1.Text.Trim()+"',"+
									"nombre2_paciente = '"+this.entry_nombre2.Text.Trim()+"',"+
									"apellido_paterno_paciente = '"+this.entry_apellido_paterno.Text.Trim()+"',"+
									"apellido_materno_paciente = '"+this.entry_apellido_materno.Text.Trim()+"',"+
									"grupo_sanguineo_paciente = '"+this.entry_tipo_sangre.Text.Trim()+"',"+
									"direccion_paciente = '"+this.entry_calle.Text.Trim()+"',"+
									"numero_casa_paciente = '"+this.entry_numeroext.Text.Trim()+"',"+
									"numero_departamento_paciente = '"+this.entry_numeroint.Text.Trim()+"',"+
									"codigo_postal_paciente = '"+this.entry_cp.Text.Trim()+"',"+
									"telefono_particular1_paciente = '"+this.entry_tel1.Text.Trim()+"',"+
									"telefono_particular2_paciente = '"+this.entry_tel2.Text.Trim()+"',"+
									"telefono_trabajo1_paciente = '"+this.entry_telefono_trab1.Text.Trim()+"',"+
									"telefono_trabajo2_paciente = '"+this.entry_telefono_trab2.Text.Trim()+"',"+
									"celular1_paciente = '"+this.entry_celular1.Text.Trim()+"',"+
									"celular2_paciente = '"+this.entry_celular2.Text.Trim()+"',"+
									"fax_paciente = '"+this.entry_fax.Text.Trim()+"',"+
									"email_paciente = '"+this.entry_correo.Text.Trim()+"', "+
									"estado_civil_paciente = '"+this.entry_estado_civil.Text.Trim()+"',"+
									"sexo_paciente = '"+this.entry_sexo.Text.Trim()+"',"+
									"titulo_paciente = '"+this.entry_titulopac.Text.Trim()+"',"+
									"curp_paciente = '"+this.entry_curp.Text.Trim()+"',"+
									"rfc_paciente = '"+this.entry_rfc.Text.Trim()+"',"+
									"colonia_paciente = '"+this.entry_colonia.Text.Trim()+"',"+
									"estado_paciente = '"+estado.ToString().ToUpper()+"',"+
									//+this.estados.Text.Trim()+"',"+
									"fecha_muerte_paciente = '"+this.entry_fecha_defuncion.Text.Trim()+"',"+
									"causa_muerte_paciente = '"+this.entry_causa_defuncion.Text.Trim()+"',"+
									"fecha_nacimiento_paciente = '"+this.entry_fechanac.Text+"',"+
									"fechahora_registro_paciente = '"+this.entry_fechahora_registro.Text+"',"+
									"municipio_paciente = '"+(string) municipios.ToString().ToUpper()+"',"+//+this.municipio.Text.Trim()+"',"+
									"ocupacion_paciente = '"+this.entry_ocupacion.Text.Trim()+"',"+
									"observaciones = '"+this.entry_observaciones.Text.Trim()+"',"+
									"historia_movimientos_paciente = historia_movimientos_paciente || '"+LoginEmpleado+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
									"WHERE  pid_paciente = '"+this.entry_pid_paciente.Text+"';";
						comando.ExecuteNonQuery();
			        	comando.Dispose();
			        	MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"El Paciente se guardo Satisfactoriamente");
						msgBox.Run();
						msgBox.Destroy();
			       	}
					catch (NpgsqlException ex)
					{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
				    }
				    desactiva_entrys(true);
					button_editar.Sensitive = true;
					conexion.Close ();
				}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
				
		void llenado_municipios(string tipo_, string descripcionmunicipio_)
		{
			combobox_municipios.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_municipios.PackStart(cell3, true);
			combobox_municipios.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string));
			combobox_municipios.Model = store3;
			
			if (tipo_ == "selecciona"){
				store3.AppendValues ((string) descripcionmunicipio_);
			}
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT descripcion_municipio FROM hscmty_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               						"ORDER BY descripcion_municipio;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store3.AppendValues ((string) lector["descripcion_municipio"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3))	{ combobox_municipios.SetActiveIter (iter3); }
			combobox_municipios.Changed += new EventHandler (onComboBoxChanged_municipios);
		}
		
		void llenado_estados(string tipo_, string descripcionestado_, int idestado_)
		{
			combobox_estado.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_estado.PackStart(cell4, true);
			combobox_estado.AddAttribute(cell4,"text",0);
	        
			ListStore store4 = new ListStore( typeof (string),typeof (int));
			combobox_estado.Model = store4;
			if (tipo_ == "selecciona"){
				store4.AppendValues ((string) descripcionestado_, (int) idestado_);
			}
	       	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM hscmty_estados ORDER BY descripcion_estado;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store4.AppendValues ((string) lector["descripcion_estado"], (int) lector["id_estado"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	      	        	        
			TreeIter iter4;
			if (store4.GetIterFirst(out iter4)){
				combobox_estado.SetActiveIter (iter4);
			}
			combobox_estado.Changed += new EventHandler (onComboBoxChanged_estado);
		}
		
		void onComboBoxChanged_municipios (object sender, EventArgs args)
		{
			ComboBox combobox_municipios = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_municipios.GetActiveIter (out iter)){
				municipios = (string) combobox_municipios.Model.GetValue(iter,0);
			}
		}
	    
		void onComboBoxChanged_estado (object sender, EventArgs args)
		{
			ComboBox combobox_estado = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_estado.GetActiveIter (out iter)){	
				estado = (string) combobox_estado.Model.GetValue(iter,0); 
				idestado = (int) combobox_estado.Model.GetValue(iter,1);
				municipios = "";
				llenado_municipios("nuevo"," ");
			}
		}
		
		// Esto es indispensable para que funcione
		[GLib.ConnectBefore ()]   	      
		void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){				
				args.RetVal = true;
				llena_lista_paciente();
			}
		}
	}
}