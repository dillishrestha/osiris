//////////////////////////////////////////////////////////
// created on 15/04/2010
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares Cuevas - arcangeldoc@gmail.com (Programacion)
//               
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPO.  See the
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
//////////////////////////////////////////////////////////	

using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class calendario_citas
	{				
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Ventana Principal //
		[Widget] Gtk.Window agenda_calendario_medico = null;
		// Notebook1 -- List the citas
		[Widget] Gtk.Calendar calendar1 = null;
		[Widget] Gtk.Entry entry_fecha_seleccionada = null;
		[Widget] Gtk.TreeView treeview_lista_agenda = null;
						
		// Notebook2 Citas
		[Widget] Gtk.Calendar calendar2 = null;
		[Widget] Gtk.Entry entry_numero_citapaciente  = null;
		[Widget] Gtk.Entry entry_fecha_cita = null;
		[Widget] Gtk.Entry entry_nombre_paciente_cita2 = null;
		[Widget] Gtk.Entry entry_fecha_nac_cita = null;
		[Widget] Gtk.Entry entry_edad_paciente_cita = null;
		[Widget] Gtk.ComboBox combobox_estado_civil_cita = null;
		[Widget] Gtk.Entry entry_telefono_cita = null;
		[Widget] Gtk.Entry entry_celular_cita = null;
		[Widget] Gtk.Entry entry_mail_cita = null;
		[Widget] Gtk.CheckButton checkbutton_crea_cita = null;
		[Widget] Gtk.RadioButton radiobutton_paciente_conexpe_cita = null;
		[Widget] Gtk.RadioButton radiobutton_paciente_sinexpe_cita = null;
		[Widget] Gtk.Button button_guardar_cita = null;
		[Widget] Gtk.Button button_busca_medicos_cita = null;
		[Widget] Gtk.Button button_especialidad_cita = null;
		[Widget] Gtk.Button button_busca_empresas_cita = null;
		[Widget] Gtk.RadioButton radiobutton_hombre_cita = null;
		[Widget] Gtk.RadioButton radiobutton_mujer_cita = null;
		[Widget] Gtk.Entry entry_motivoconsulta = null;
		[Widget] Gtk.Entry entry_observaciones_cita = null;
		[Widget] Gtk.Button button_busca_paciente = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;		
				
		// Noteboo3 quirofano
		[Widget] Gtk.Calendar calendar3 = null;
		[Widget] Gtk.Entry entry_fecha_cita_qx = null;
		[Widget] Gtk.Entry entry_numero_citaquirofano = null;
							
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		
		TreeStore treeViewEngineListaCitas;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public calendario_citas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
						
			Glade.XML gxml = new Glade.XML (null, "quirofano.glade", "agenda_calendario_medico", null);
			gxml.Autoconnect (this);        
			
			// show the window
			agenda_calendario_medico.Show();
			
			// creating treeview citas
			crea_treeview_citas();
						
			// show opcion the calendar
			calendar1.DisplayOptions = CalendarDisplayOptions.ShowHeading|CalendarDisplayOptions.ShowDayNames;
			calendar1.MarkDay(uint.Parse(DateTime.Now.ToString("dd")));
			//calendar1.Year = int.Parse(DateTime.Now.ToString("yyyy"));
			//calendar1.Month = int.Parse(DateTime.Now.ToString("MM"));
			calendar2.DisplayOptions = CalendarDisplayOptions.ShowHeading|CalendarDisplayOptions.ShowDayNames;
			calendar2.MarkDay(uint.Parse(DateTime.Now.ToString("dd")));
						
			calendar1.DaySelected += new EventHandler (on_dayselected_clicked);
			calendar2.DaySelected += new EventHandler (on_dayselected_clicked);
			calendar3.DaySelected += new EventHandler (on_dayselected_clicked);
			// Action the Click
			checkbutton_crea_cita.Clicked += new EventHandler(on_checkbutton_crea_cita_clicked);
			
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_numero_citapaciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));		// cambia el fondo del entry
			entry_numero_citaquirofano.ModifyBase(StateType.Normal, new Gdk.Color(252,95,91));		// cambia el fondo del entry
			
			Pango.FontDescription fontdesc = new Pango.FontDescription(); //Pango.FontDescription.FromString ("Arial 10");
			fontdesc.Weight = Pango.Weight.Bold; // letra a negrita
			entry_numero_citapaciente.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			entry_numero_citaquirofano.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			
			entry_nombre_paciente_cita2.Sensitive = false;
			entry_fecha_nac_cita.Sensitive = false;
			entry_edad_paciente_cita.Sensitive = false;
			combobox_estado_civil_cita.Sensitive = false;
			entry_telefono_cita.Sensitive = false;
			entry_celular_cita.Sensitive = false;
			radiobutton_paciente_conexpe_cita.Sensitive = false;
			radiobutton_paciente_sinexpe_cita.Sensitive = false;
			button_guardar_cita.Sensitive = false;
			entry_mail_cita.Sensitive = false;
			button_busca_paciente.Sensitive = false;
			button_busca_medicos_cita.Sensitive = false;
			button_especialidad_cita.Sensitive = false;
			button_busca_empresas_cita.Sensitive = false;
			radiobutton_hombre_cita.Sensitive = false;
			radiobutton_mujer_cita.Sensitive = false;
			entry_motivoconsulta.Sensitive = false;
			entry_observaciones_cita.Sensitive = false;
			combobox_tipo_paciente.Sensitive = false;
			combobox_tipo_admision.Sensitive = false;
			
			//object[] param_name_object = {entry_nombre_paciente,entry_fecha_nac_cita,entry_edad_paciente_cita,combobox_estado_civil};
			//activa_desactiva(param_name_object);
		}
		
		//void activa_desactiva(object[] args)
		//{
		//	Console.WriteLine("nº de argumentos: {0}", args.Length);
		 //   Gtk.Entry objeto_array = (object) args[0] as Gtk.Entry;
		//	Console.WriteLine(objeto_array.Name.ToString());
		//	for (int i = 0; i < args.Length; i++){
		//		Console.WriteLine("args[{0}] = {1}", i, args[i],@args[i]);
		//		Console.Write(args[i].ToString());
				
		//	}			
		//}
		
		void crea_treeview_citas()
		{
			treeViewEngineListaCitas = new TreeStore(typeof(string));
		}
		
		void on_dayselected_clicked (object obj, EventArgs args)
		{
			Gtk.Calendar activatedCalendar = (Gtk.Calendar) obj;
			if(activatedCalendar.Name.ToString() == "calendar1"){
				entry_fecha_seleccionada.Text = activatedCalendar.GetDate().ToString ("yyyy/MM/dd");	
			}
			if(activatedCalendar.Name.ToString() == "calendar2"){
				entry_fecha_cita.Text = activatedCalendar.GetDate().ToString ("yyyy/MM/dd");	
			}
			if(activatedCalendar.Name.ToString() == "calendar3"){
				entry_fecha_cita_qx.Text = activatedCalendar.GetDate().ToString ("yyyy/MM/dd");	
			}
			//Console.WriteLine (activatedCalendar.Name.ToString());
			//Console.WriteLine (activatedCalendar.GetDate ().ToString ("yyyy/MM/dd"));
			
		}
		
		void on_checkbutton_crea_cita_clicked(object sender, EventArgs args)
		{
			if(checkbutton_crea_cita.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear una nueva CITA ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){					
	 				entry_motivoconsulta.Sensitive = true;
					entry_observaciones_cita.Sensitive = true;
					button_busca_paciente.Sensitive = true;
					radiobutton_paciente_conexpe_cita.Sensitive = true;
					radiobutton_paciente_sinexpe_cita.Sensitive = true;
					button_guardar_cita.Sensitive = true;
					combobox_tipo_paciente.Sensitive = true;
					combobox_tipo_admision.Sensitive = true;
					llenado_estadocivil("","");
					llenado_tipo_paciente();
					llenado_tipo_servicio();
				}else{
					checkbutton_crea_cita.Active = false;
				}			
			}else{
				checkbutton_crea_cita.Active = false;
				entry_motivoconsulta.Sensitive = false;
				entry_observaciones_cita.Sensitive = false;
				button_busca_paciente.Sensitive = false;
				radiobutton_paciente_conexpe_cita.Sensitive = false;
				radiobutton_paciente_sinexpe_cita.Sensitive = false;
				button_guardar_cita.Sensitive = false;
				combobox_tipo_paciente.Sensitive = false;
				combobox_tipo_admision.Sensitive = false;				
			}
		}
		
		// Estado Civil
		void llenado_estadocivil(string tipo_, string descripcion_)
		{
			combobox_estado_civil_cita.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_estado_civil_cita.PackStart(cell, true);
			combobox_estado_civil_cita.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_estado_civil_cita.Model = store;
			
			if(tipo_ == "selecciona"){
				store.AppendValues ((string)descripcion_);
			}
	        
			store.AppendValues ("Casado(a)");
			store.AppendValues ("Soltero(a)");
			store.AppendValues ("Separado(a)");
			store.AppendValues ("Viudo(a)");
			store.AppendValues ("Union Libre");
			store.AppendValues ("Divorciado(a)");
	        
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_estado_civil_cita.SetActiveIter (iter);
			}
			//combobox_estado_civil_cita.Changed += new EventHandler (onComboBoxChanged_estadocivil);
		}
		
		void llenado_tipo_paciente()
		{
			// Tipos de Paciente
			combobox_tipo_paciente.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_paciente.PackStart(cell1, true);
			combobox_tipo_paciente.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_paciente.Model = store1;
			store1.Clear();
			
			store1.AppendValues ("",0);
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT * FROM osiris_his_tipo_pacientes ORDER BY descripcion_tipo_paciente;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					store1.AppendValues ((string) lector["descripcion_tipo_paciente"].ToString().ToUpper(),(int) lector["id_tipo_paciente"]);
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();	        	              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_paciente.SetActiveIter (iter1);
			}
			//combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipopaciente);
		}
		
		void llenado_tipo_servicio()
		{
			// Llenado de combobox con los tipos de Admisiones y centros de costos
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
	        
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND cuenta_mayor = '4000' AND activo_admision = 'true' "+
	           							//"AND cuenta_mayor_ingreso = '4000' "+
	           							//"AND grupo = 'MED' "+
	               						"ORDER BY id_tipo_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read())
				{
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();	        
			
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)) {
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			//combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}		
	}
}