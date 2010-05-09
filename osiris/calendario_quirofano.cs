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
		[Widget] Gtk.Entry entry_numero_citapaciente = null;
		[Widget] Gtk.Entry entry_pid_paciente_cita = null;
		[Widget] Gtk.Entry entry_nombre_paciente_cita1 = null;
		[Widget] Gtk.Button button_busca_paciente_cita = null;
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
		[Widget] Gtk.Button button_busca_especialidad_cita = null;
		[Widget] Gtk.Button button_busca_empresas_cita = null;
		[Widget] Gtk.RadioButton radiobutton_hombre_cita = null;
		[Widget] Gtk.RadioButton radiobutton_mujer_cita = null;
		[Widget] Gtk.Entry entry_motivoconsulta = null;
		[Widget] Gtk.Entry entry_observaciones_cita = null;
		[Widget] Gtk.Entry entry_referido_por = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.Entry entry_id_empaseg_cita = null;
		[Widget] Gtk.Entry entry_nombre_empaseg_cita = null;
		[Widget] Gtk.Entry entry_id_doctor_cita = null;
		[Widget] Gtk.Entry entry_nombre_doctor_cita = null;
		[Widget] Gtk.Entry entry_id_especialidad_cita = null;
		[Widget] Gtk.Entry entry_nombre_especialidad_cita = null;
				
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
		
		string tipopaciente;
		int id_tipopaciente;
		string tipointernamiento;
		int id_tipointernamiento;
		
		TreeStore treeViewEngineListaCitas;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		
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
			button_guardar_cita.Clicked += new EventHandler(on_button_guardar_cita_clicked);
			checkbutton_crea_cita.Clicked += new EventHandler(on_checkbutton_crea_cita_clicked);
			button_busca_paciente_cita.Clicked += new EventHandler(on_button_busca_paciente_cita_clicked);
			radiobutton_paciente_conexpe_cita.Clicked += new EventHandler(on_radiobutton_paciente_cita_clicked);
			radiobutton_paciente_sinexpe_cita.Clicked += new EventHandler(on_radiobutton_paciente_cita_clicked);
			button_busca_empresas_cita.Clicked += new EventHandler(on_button_busca_empresas_cita_clicked);
			button_busca_medicos_cita.Clicked += new EventHandler(on_button_busca_medicos_cita_clicked);
			button_busca_especialidad_cita.Clicked += new EventHandler(on_button_busca_especialidad_cita_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_numero_citapaciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));		// cambia el fondo del entry
			entry_numero_citaquirofano.ModifyBase(StateType.Normal, new Gdk.Color(252,95,91));		// cambia el fondo del entry
			
			Pango.FontDescription fontdesc = new Pango.FontDescription(); //Pango.FontDescription.FromString ("Arial 10");
			fontdesc.Weight = Pango.Weight.Bold; // letra a negrita
			entry_numero_citapaciente.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			entry_numero_citaquirofano.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			
			radiobutton_paciente_conexpe_cita.Sensitive = false;
			radiobutton_paciente_sinexpe_cita.Sensitive = false;
			
			entry_pid_paciente_cita.Sensitive = false;
			entry_nombre_paciente_cita1.Sensitive = false;
			
			entry_nombre_paciente_cita2.Sensitive = false;
			entry_fecha_nac_cita.Sensitive = false;
			entry_edad_paciente_cita.Sensitive = false;
			combobox_estado_civil_cita.Sensitive = false;
			entry_telefono_cita.Sensitive = false;
			entry_celular_cita.Sensitive = false;
			entry_mail_cita.Sensitive = false;
			radiobutton_hombre_cita.Sensitive = false;
			radiobutton_mujer_cita.Sensitive = false;
			
			button_guardar_cita.Sensitive = false;
			button_busca_paciente_cita.Sensitive = false;
			button_busca_medicos_cita.Sensitive = false;
			button_busca_especialidad_cita.Sensitive = false;
			button_busca_empresas_cita.Sensitive = false;			
			entry_motivoconsulta.Sensitive = false;
			entry_observaciones_cita.Sensitive = false;
			entry_referido_por.Sensitive = false;
			combobox_tipo_paciente.Sensitive = false;
			combobox_tipo_admision.Sensitive = false;
			
			//object[] param_name_object = {entry_nombre_paciente,entry_fecha_nac_cita,entry_edad_paciente_cita,combobox_estado_civil};
			//activa_desactiva(param_name_object);
		}
		
		//void activa_desactiva(object[] args)
		//{
		//	Console.WriteLine("nº de argumentos: {0}", args.Length);
		//  Gtk.Entry objeto_array = (object) args[0] as Gtk.Entry;
		//	Console.WriteLine(objeto_array.Name.ToString());
		//	for (int i = 0; i < args.Length; i++){
		//		Console.WriteLine("args[{0}] = {1}", i, args[i],@args[i]);
		//		Console.Write(args[i].ToString());				
		//	}			
		//}
		
		void crea_treeview_citas()
		{
			treeViewEngineListaCitas = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),
			                                         typeof(string),typeof(string),typeof(string),typeof(string),
			                                         typeof(string),typeof(string),typeof(string),typeof(string),
			                                         typeof(string),typeof(string),typeof(string),typeof(string),
			                                         typeof(string),typeof(string),typeof(string),typeof(string));
			treeview_lista_agenda.Model = treeViewEngineListaCitas;
			treeview_lista_agenda.RulesHint = true;
			
			TreeViewColumn col_agenda0 = new TreeViewColumn();
			CellRendererText cellrt0 = new CellRendererText();
			col_agenda0.Title = "Fecha";
			col_agenda0.PackStart(cellrt0, true);
			col_agenda0.AddAttribute (cellrt0, "text", 0);
			col_agenda0.Resizable = true;
			//col_agenda0.SortColumnId = (int) Coldatos_agenda.col_agenda0;
			
			TreeViewColumn col_agenda1 = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_agenda1.Title = "Hora";
			col_agenda1.PackStart(cellrt1, true);
			col_agenda1.AddAttribute (cellrt1, "text", 1);
			col_agenda1.Resizable = true;
			//col_agenda1.SortColumnId = (int) Coldatos_agenda.col_agenda1;
			
			TreeViewColumn col_agenda2 = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_agenda2.Title = "N° Cita";
			col_agenda2.PackStart(cellrt2, true);
			col_agenda2.AddAttribute (cellrt2, "text", 2);
			col_agenda2.Resizable = true;
			//col_agenda2.SortColumnId = (int) Coldatos_agenda.col_agenda2;
			
			TreeViewColumn col_agenda3 = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_agenda3.Title = "N° Expediente";
			col_agenda3.PackStart(cellrt3, true);
			col_agenda3.AddAttribute (cellrt3, "text", 3);
			col_agenda3.Resizable = true;
			//col_agenda3.SortColumnId = (int) Coldatos_agenda.col_agenda3;
			
			TreeViewColumn col_agenda4 = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_agenda4.Title = "Nombre 1";
			col_agenda4.PackStart(cellrt1, true);
			col_agenda4.AddAttribute (cellrt1, "text", 4);
			col_agenda4.Resizable = true;
			//col_agenda4.SortColumnId = (int) Column.col_agenda4;
            
			TreeViewColumn col_agenda5 = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_agenda5.Title = "Nombre 2";
			col_agenda5.PackStart(cellrt5, true);
			col_agenda5.AddAttribute (cellrt5, "text", 5);
			col_agenda5.Resizable = true;
			//col_agenda5.SortColumnId = (int) Column.col_agenda5;
            
			TreeViewColumn col_agenda6 = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_agenda6.Title = "Apellido Paterno";
			col_agenda6.PackStart(cellrt6, true);
			col_agenda6.AddAttribute (cellrt6, "text", 6);
			col_agenda6.Resizable = true;
			//col_agenda6.SortColumnId = (int) Column.col_agenda5;
            
			TreeViewColumn col_agenda7 = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_agenda7.Title = "Apellido Materno";
			col_agenda7.PackStart(cellrt7, true);
			col_agenda7.AddAttribute (cellrt7, "text", 7);
			col_agenda7.Resizable = true;
			//col_agenda7.SortColumnId = (int) Column.col_agenda7;			
			
			TreeViewColumn col_agenda8 = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_agenda8.Title = "Edad";
			col_agenda8.PackStart(cellrt8, true);
			col_agenda8.AddAttribute (cellrt8, "text", 8);
			col_agenda8.Resizable = true;
			//col_agenda8.SortColumnId = (int) Coldatos_agenda.col_agenda8;
			
			TreeViewColumn col_agenda9 = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_agenda9.Title = "Telefono";
			col_agenda9.PackStart(cellrt9, true);
			col_agenda9.AddAttribute (cellrt9, "text", 9);
			col_agenda9.Resizable = true;
			//col_agenda9.SortColumnId = (int) Coldatos_agenda.col_agenda9;
			
			TreeViewColumn col_agenda10 = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_agenda10.Title = "email";
			col_agenda10.PackStart(cellrt10, true);
			col_agenda10.AddAttribute (cellrt10, "text", 10);
			col_agenda10.Resizable = true;
			//col_agenda10.SortColumnId = (int) Coldatos_agenda.col_agenda10;
			
			TreeViewColumn col_agenda11 = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_agenda11.Title = "Tipo Paciente";
			col_agenda11.PackStart(cellrt11, true);
			col_agenda11.AddAttribute (cellrt11, "text", 11);
			col_agenda11.Resizable = true;
			//col_agenda11.SortColumnId = (int) Coldatos_agenda.col_agenda11;
			
			TreeViewColumn col_agenda12 = new TreeViewColumn();
			CellRendererText cellrt12 = new CellRendererText();
			col_agenda12.Title = "Tipo Servicio";
			col_agenda12.PackStart(cellrt12, true);
			col_agenda12.AddAttribute (cellrt12, "text", 12);
			col_agenda12.Resizable = true;
			//col_agenda12.SortColumnId = (int) Coldatos_agenda.col_agenda12;
			
			TreeViewColumn col_agenda13 = new TreeViewColumn();
			CellRendererText cellrt13 = new CellRendererText();
			col_agenda13.Title = "Consultorio/QX.";
			col_agenda13.PackStart(cellrt13, true);
			col_agenda13.AddAttribute (cellrt13, "text", 13);
			col_agenda13.Resizable = true;
			//col_agenda13.SortColumnId = (int) Coldatos_agenda.col_agenda13;
			
			TreeViewColumn col_agenda14 = new TreeViewColumn();
			CellRendererText cellrt14 = new CellRendererText();
			col_agenda14.Title = "Doctor";
			col_agenda14.PackStart(cellrt14, true);
			col_agenda14.AddAttribute (cellrt14, "text", 14);
			col_agenda14.Resizable = true;
			//col_agenda14.SortColumnId = (int) Coldatos_agenda.col_agenda14;
			
			TreeViewColumn col_agenda15 = new TreeViewColumn();
			CellRendererText cellrt15 = new CellRendererText();
			col_agenda15.Title = "Especialidad";
			col_agenda15.PackStart(cellrt15, true);
			col_agenda15.AddAttribute (cellrt15, "text", 15);
			col_agenda15.Resizable = true;
			//col_agenda15.SortColumnId = (int) Coldatos_agenda.col_agenda15;
			
			TreeViewColumn col_agenda16 = new TreeViewColumn();
			CellRendererText cellrt16 = new CellRendererText();
			col_agenda16.Title = "Motivo de Consulta";
			col_agenda16.PackStart(cellrt16, true);
			col_agenda16.AddAttribute (cellrt16, "text", 16);
			col_agenda16.Resizable = true;
			//col_agenda16.SortColumnId = (int) Coldatos_agenda.col_agenda16;
			
			TreeViewColumn col_agenda17 = new TreeViewColumn();
			CellRendererText cellrt17 = new CellRendererText();
			col_agenda17.Title = "Observaciones";
			col_agenda17.PackStart(cellrt17, true);
			col_agenda17.AddAttribute (cellrt17, "text", 17);
			col_agenda17.Resizable = true;
			//col_agenda17.SortColumnId = (int) Coldatos_agenda.col_agenda17;
			
			TreeViewColumn col_agenda18 = new TreeViewColumn();
			CellRendererText cellrt18 = new CellRendererText();
			col_agenda18.Title = "Agendado por";
			col_agenda18.PackStart(cellrt18, true);
			col_agenda18.AddAttribute (cellrt18, "text", 18);
			col_agenda18.Resizable = true;
			//col_agenda18.SortColumnId = (int) Coldatos_agenda.col_agenda18;
			
			TreeViewColumn col_agenda19 = new TreeViewColumn();
			CellRendererText cellrt19 = new CellRendererText();
			col_agenda19.Title = "Fecha/Hora";
			col_agenda19.PackStart(cellrt19, true);
			col_agenda19.AddAttribute (cellrt19, "text", 19);
			col_agenda19.Resizable = true;
			//col_agenda18.SortColumnId = (int) Coldatos_agenda.col_agenda19;
			
			treeview_lista_agenda.AppendColumn(col_agenda0);
			treeview_lista_agenda.AppendColumn(col_agenda1);
			treeview_lista_agenda.AppendColumn(col_agenda2);
			treeview_lista_agenda.AppendColumn(col_agenda3);
			treeview_lista_agenda.AppendColumn(col_agenda4);
			treeview_lista_agenda.AppendColumn(col_agenda5);
			treeview_lista_agenda.AppendColumn(col_agenda6);
			treeview_lista_agenda.AppendColumn(col_agenda7);
			treeview_lista_agenda.AppendColumn(col_agenda8);
			treeview_lista_agenda.AppendColumn(col_agenda9);
			treeview_lista_agenda.AppendColumn(col_agenda10);
			treeview_lista_agenda.AppendColumn(col_agenda11);
			treeview_lista_agenda.AppendColumn(col_agenda12);
			treeview_lista_agenda.AppendColumn(col_agenda13);
			treeview_lista_agenda.AppendColumn(col_agenda14);
			treeview_lista_agenda.AppendColumn(col_agenda15);
			treeview_lista_agenda.AppendColumn(col_agenda16);
			treeview_lista_agenda.AppendColumn(col_agenda17);
			treeview_lista_agenda.AppendColumn(col_agenda18);
			treeview_lista_agenda.AppendColumn(col_agenda19);
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
		
		void on_checkbutton_crea_cita_clicked(object obj, EventArgs args)
		{
			if(checkbutton_crea_cita.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear una nueva CITA ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){					
	 				entry_motivoconsulta.Sensitive = true;
					entry_observaciones_cita.Sensitive = true;
					entry_referido_por.Sensitive = true;
					entry_pid_paciente_cita.Sensitive = true;
					entry_nombre_paciente_cita1.Sensitive = true;
					button_busca_paciente_cita.Sensitive = true;
					radiobutton_paciente_conexpe_cita.Sensitive = true;
					radiobutton_paciente_sinexpe_cita.Sensitive = true;
					button_guardar_cita.Sensitive = true;
					combobox_tipo_paciente.Sensitive = true;
					combobox_tipo_admision.Sensitive = true;
					button_busca_medicos_cita.Sensitive = true;
					button_busca_especialidad_cita.Sensitive = true;
					button_busca_empresas_cita.Sensitive = true;
					llenado_estadocivil("","");
					llenado_tipo_paciente("","");
					llenado_tipo_servicio("","");
				}else{
					checkbutton_crea_cita.Active = false;
				}			
			}else{
				entry_pid_paciente_cita.Sensitive = false;
				entry_nombre_paciente_cita1.Sensitive = false;
				checkbutton_crea_cita.Active = false;
				entry_motivoconsulta.Sensitive = false;
				entry_observaciones_cita.Sensitive = false;
				entry_referido_por.Sensitive = false;
				button_busca_paciente_cita.Sensitive = false;
				radiobutton_paciente_conexpe_cita.Sensitive = false;
				radiobutton_paciente_sinexpe_cita.Sensitive = false;
				button_guardar_cita.Sensitive = false;
				combobox_tipo_paciente.Sensitive = false;
				combobox_tipo_admision.Sensitive = false;
				button_busca_medicos_cita.Sensitive = false;
				button_busca_especialidad_cita.Sensitive = false;
				button_busca_empresas_cita.Sensitive = false;
			}
		}
		
		void on_radiobutton_paciente_cita_clicked(object obj, EventArgs args)
		{
			Gtk.RadioButton radiobutton_paciente_cita = (Gtk.RadioButton) obj;
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_paciente_conexpe_cita"){
				if (radiobutton_paciente_conexpe_cita.Active == true){					
					entry_pid_paciente_cita.Sensitive = true;
					entry_nombre_paciente_cita1.Sensitive = true;
					button_busca_paciente_cita.Sensitive = true;
					entry_nombre_paciente_cita2.Sensitive = false;
					entry_fecha_nac_cita.Sensitive = false;
					entry_edad_paciente_cita.Sensitive = false;
					combobox_estado_civil_cita.Sensitive = false;
					entry_telefono_cita.Sensitive = false;
					entry_celular_cita.Sensitive = false;
					entry_mail_cita.Sensitive = false;
					radiobutton_hombre_cita.Sensitive = false;
					radiobutton_mujer_cita.Sensitive = false;
				}				
			}
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_paciente_sinexpe_cita"){
				if (radiobutton_paciente_sinexpe_cita.Active == true){					
					entry_pid_paciente_cita.Sensitive = false;
					entry_nombre_paciente_cita1.Sensitive = false;
					button_busca_paciente_cita.Sensitive = false;
					entry_nombre_paciente_cita2.Sensitive = true;
					entry_fecha_nac_cita.Sensitive = true;
					entry_edad_paciente_cita.Sensitive = true;
					combobox_estado_civil_cita.Sensitive = true;
					entry_telefono_cita.Sensitive = true;
					entry_celular_cita.Sensitive = true;
					entry_mail_cita.Sensitive = true;
					radiobutton_hombre_cita.Sensitive = true;
					radiobutton_mujer_cita.Sensitive = true;
				}				
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
	        
			store.AppendValues ("");
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
		
		void llenado_tipo_paciente(string tipo_, string descripcion_)
		{
			// Tipos de Paciente
			combobox_tipo_paciente.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_paciente.PackStart(cell1, true);
			combobox_tipo_paciente.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_paciente.Model = store1;
			store1.Clear();
			
			if(tipo_ == "selecciona"){
				store1.AppendValues ((string) descripcion_);
			}
			store1.AppendValues ("",1);
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
			combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipopaciente);
		}
		
		void onComboBoxChanged_tipopaciente(object obj, EventArgs args)
		{
			ComboBox combobox_tipo_paciente = obj as ComboBox;
			if (obj == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
				id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
				entry_id_empaseg_cita.Text = "";
				entry_nombre_empaseg_cita.Text = "";
			}	
		}
		
		void llenado_tipo_servicio(string tipo_, string descripcion_)
		{
			// Llenado de combobox con los tipos de Admisiones y centros de costos
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			
			if(tipo_ == "selecciona"){
				store2.AppendValues ((string)descripcion_);
			}
				        
	      	NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND grupo = 'MED' AND activo_admision = 'true' "+
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
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
		}
		
		void onComboBoxChanged_tipo_admision (object obj, EventArgs args)
		{
			ComboBox combobox_tipo_admision = obj as ComboBox;
			if (obj == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_admision.GetActiveIter (out iter)){
				tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);//Console.WriteLine(tipointernamiento);
				id_tipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);//Console.WriteLine(idtipointernamiento);
			}
		}
		
		void on_button_busca_paciente_cita_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_pid_paciente_cita,entry_nombre_paciente_cita1};
			string[] parametros_sql = {"SELECT * FROM osiris_his_paciente WHERE activo = 'true' ",															
										"SELECT * FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND apellido_paterno_paciente LIKE '%",
										"SELECT * FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND nombre1_paciente LIKE '%",
										"SELECT * FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND pid_paciente = '"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_paciente_cita"," ORDER BY pid_paciente","%' ",1);
		}
		
		void on_button_busca_empresas_cita_clicked(object sender, EventArgs args)
		{
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			if (id_tipopaciente != 400){
				object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' ",															
										"SELECT * FROM osiris_empresas  WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' "+
										"AND descripcion_empresa LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_empresa_cita"," ORDER BY descripcion_empresa","%' ",0);
			}else{
				// Buscando aseguradora
				object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_aseguradoras ",															
										"SELECT * FROM osiris_aseguradoras "+
										"WHERE descripcion_aseguradora LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_aseguradoras_cita"," ORDER BY descripcion_aseguradora","%' ",0);
			}			
		}
		
		void on_button_busca_medicos_cita_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_doctor_cita,entry_nombre_doctor_cita};
			string[] parametros_sql = {"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' ",															
										"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' "+
										"AND nombre_medico LIKE '%"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_medico_cita"," ORDER BY nombre_medico","%' ",0);
		}
		
		void on_button_busca_especialidad_cita_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_especialidad_cita,entry_nombre_especialidad_cita};
			string[] parametros_sql = {"SELECT * FROM osiris_his_tipo_especialidad ",															
										"SELECT * FROM osiris_his_tipo_especialidad "+
										"WHERE descripcion_especialidad LIKE '%"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_especialidad_cita"," ORDER BY descripcion_especialidad","%' ",0);
		}
		
		void on_button_guardar_cita_clicked(object sender, EventArgs args)
		{
			if(checkbutton_crea_cita.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar esta CITA ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
					
				}
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}		
	}
}