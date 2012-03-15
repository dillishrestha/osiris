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
		[Widget] Gtk.Notebook notebook1 = null;
		// Notebook1 -- List the citas
		[Widget] Gtk.Calendar calendar1 = null;
		[Widget] Gtk.Entry entry_fecha_seleccionada = null;
		[Widget] Gtk.Entry entry_fecha_final = null;
		[Widget] Gtk.TreeView treeview_lista_agenda = null;
		[Widget] Gtk.CheckButton checkbutton_fecha_final = null;
		[Widget] Gtk.CheckButton checkbutton_canceladas = null;
		[Widget] Gtk.CheckButton checkbutton_px_no_asistieron = null;
		
		[Widget] Gtk.CheckButton checkbutton_filtro_paciente = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Button button_buscar_paciente = null;
		[Widget] Gtk.Button button_aplica_filtropaciente = null;
		
		[Widget] Gtk.CheckButton checkbutton_filtro_doctor = null;
		[Widget] Gtk.Entry entry_id_doctor_consulta = null;
		[Widget] Gtk.Entry entry_nombre_doctor_consulta = null;
		[Widget] Gtk.Button button_busca_medicos_consulta = null;
		[Widget] Gtk.Button button_aplica_filtrodoctor = null;
				
		[Widget] Gtk.CheckButton checkbutton_filtro_especialidad = null;
		[Widget] Gtk.Entry entry_id_especialidad_consulta = null;
		[Widget] Gtk.Entry entry_nombre_especialidad_consulta = null;
		[Widget] Gtk.Button button_busca_especialidad_consulta = null;
		[Widget] Gtk.Button button_aplica_filtroespecialidad = null;
		
		[Widget] Gtk.CheckButton checkbutton_tipo_paciente_consulta = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente_consulta = null;
		[Widget] Gtk.CheckButton checkbutton_tipo_admision_consulta = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision_consulta = null;
		
		[Widget] Gtk.Entry entry_numero_citas = null;
		[Widget] Gtk.Button button_cancela_cita = null;
		[Widget] Gtk.Button button_imprimir_calendario = null;
		
		[Widget] Gtk.Statusbar statusbar_citasqx = null;
							
		// Notebook2 Citas		
		[Widget] Gtk.Calendar calendar2 = null;
		[Widget] Gtk.Entry entry_numero_citapaciente = null;
		[Widget] Gtk.Entry entry_pid_paciente_cita = null;
		[Widget] Gtk.Entry entry_nombre_paciente_cita1 = null;
		[Widget] Gtk.Button button_busca_paciente_cita = null;
		[Widget] Gtk.Button button_infoadmision = null;
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
		[Widget] Gtk.Entry entry_referido_por_cita = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente_cita = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision_cita = null;
		[Widget] Gtk.Entry entry_id_empaseg_cita = null;
		[Widget] Gtk.Entry entry_nombre_empaseg_cita = null;
		[Widget] Gtk.Entry entry_id_doctor_cita = null;
		[Widget] Gtk.Entry entry_nombre_doctor_cita = null;
		[Widget] Gtk.Entry entry_id_especialidad_cita = null;
		[Widget] Gtk.Entry entry_nombre_especialidad_cita = null;
		[Widget] Gtk.ComboBox combobox_hora_cita = null;
		[Widget] Gtk.ComboBox combobox_minutos_cita = null;
				
		// Noteboo3 quirofano
		[Widget] Gtk.Calendar calendar3 = null;
		[Widget] Gtk.Entry entry_fecha_cita_qx = null;
		[Widget] Gtk.Entry entry_numero_citaquirofano = null;
		[Widget] Gtk.CheckButton checkbutton_crea_citaqx = null;
		[Widget] Gtk.RadioButton radiobutton_paciente_conexpe_qx = null;
		[Widget] Gtk.RadioButton radiobutton_paciente_sinexpe_qx = null;
		
		//Ventana de cancelacion de folios
		[Widget] Gtk.Window cancelador_folios = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Entry entry_folio = null;
		[Widget] Gtk.Entry entry_motivo = null;
		[Widget] Gtk.Label label247 = null;
							
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		
		string tipopaciente;
		int id_tipopaciente = 0;
		string tipointernamiento;
		int id_tipointernamiento = 0;
		string hora_cita_qx = "";
		string minutos_cita_qx = "";
		string sexopaciente_cita = "H";
		string estadocivil_cita = "";
		string idempresa = "1";
		string idaseguradora = "1";
		int contador_numerocitas = 0;
		string sql_calendario_citaqx = "SELECT to_char(fecha_programacion,'yyyy-MM-dd') AS fechaprogramacion,hora_programacion,id_numero_citaqx,osiris_his_calendario_citaqx.id_secuencia AS idsecuencia,"+
					"osiris_his_calendario_citaqx.pid_paciente AS pidpaciente,osiris_his_calendario_citaqx.nombre_paciente,"+
					"osiris_his_paciente.nombre1_paciente,osiris_his_paciente.nombre2_paciente,osiris_his_paciente.apellido_paterno_paciente,osiris_his_paciente.apellido_materno_paciente,"+
					"osiris_his_paciente.celular1_paciente," +
					"osiris_his_paciente.telefono_particular1_paciente AS telefonoparticular1_paciente,"+
					"osiris_his_calendario_citaqx.celular1_paciente AS celular1paciente_cita,"+
					"osiris_his_calendario_citaqx.telefono_paciente AS telefonopaciente_cita,"+
					"osiris_his_paciente.email_paciente,osiris_his_calendario_citaqx.email_paciente AS emailpaciente_cita,"+
					"osiris_his_calendario_citaqx.id_tipo_paciente,descripcion_tipo_paciente,"+
					"osiris_his_calendario_citaqx.id_tipo_admisiones,descripcion_admisiones,"+
					"osiris_his_medicos.id_medico,osiris_his_medicos.nombre_medico AS nombremedico,"+
					"osiris_his_tipo_especialidad.id_especialidad,osiris_his_tipo_especialidad.descripcion_especialidad AS descripcionespecialidad,"+
					"motivo_consulta,osiris_his_calendario_citaqx.observaciones AS observaciones_citaqx,referido_por,osiris_his_calendario_citaqx.cancelado AS cancelacitaqx,"+
					"id_quiencreo_cita,osiris_his_calendario_citaqx.fechahora_creacion AS fechahoracreacion,motivo_cancelacion_citaqx,"+
					"osiris_empresas.id_empresa AS idempresa,descripcion_empresa,"+
					"osiris_aseguradoras.id_aseguradora AS idaseguradora,descripcion_aseguradora,"+
					"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
					"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad "+
					"FROM osiris_his_calendario_citaqx,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_his_tipo_admisiones,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas,osiris_aseguradoras "+
					"WHERE osiris_his_calendario_citaqx.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_his_calendario_citaqx.id_tipo_paciente "+
					"AND osiris_his_tipo_admisiones.id_tipo_admisiones = osiris_his_calendario_citaqx.id_tipo_admisiones "+
					"AND osiris_his_medicos.id_medico = osiris_his_calendario_citaqx.id_medico "+
					"AND osiris_his_tipo_especialidad.id_especialidad = osiris_his_calendario_citaqx.id_especialidad "+
					"AND osiris_empresas.id_empresa = osiris_his_calendario_citaqx.id_empresa "+
					"AND osiris_aseguradoras.id_aseguradora = osiris_his_calendario_citaqx.id_aseguradora ";
		string sql_fecha1 = "";
		string sql_fecha1_1 = "";
		string sql_fecha2 = "";
		string sql_fecha2_2 = "";
		string sql_doctor = "";
		string sql_paciente = "";
		string sql_especialidad = "";
		string sql_tipo_paciente = "";
		string sql_tipo_admision = "";
		string sql_citas_canceladas = "AND osiris_his_calendario_citaqx.cancelado = 'false' ";
		
		TreeStore treeViewEngineListaCitas;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		TreeViewColumn col_agenda0;		CellRendererText cellrt0;
		TreeViewColumn col_agenda1;		CellRendererText cellrt1;
		TreeViewColumn col_agenda2;		CellRendererText cellrt2;
		TreeViewColumn col_agenda3;		CellRendererText cellrt3;
		TreeViewColumn col_agenda4;		CellRendererText cellrt4;
		TreeViewColumn col_agenda5;		CellRendererText cellrt5;
		TreeViewColumn col_agenda6;		CellRendererText cellrt6;
		TreeViewColumn col_agenda7;		CellRendererText cellrt7;
		TreeViewColumn col_agenda8;		CellRendererText cellrt8;
		TreeViewColumn col_agenda9;		CellRendererText cellrt9;
		TreeViewColumn col_agenda10;	CellRendererText cellrt10;
		TreeViewColumn col_agenda11;	CellRendererText cellrt11;
		TreeViewColumn col_agenda12;	CellRendererText cellrt12;
		TreeViewColumn col_agenda13;	CellRendererText cellrt13;
		TreeViewColumn col_agenda14;	CellRendererText cellrt14;
		TreeViewColumn col_agenda15;	CellRendererText cellrt15;
		TreeViewColumn col_agenda16;	CellRendererText cellrt16;
		TreeViewColumn col_agenda17;	CellRendererText cellrt17;
		TreeViewColumn col_agenda18;	CellRendererText cellrt18;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
				
		public calendario_citas(string LoginEmp_,string NomEmpleado_,string AppEmpleado_,string ApmEmpleado_,string nombrebd_,int tipo_entrada) 
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
			
			calendar3.DisplayOptions = CalendarDisplayOptions.ShowHeading|CalendarDisplayOptions.ShowDayNames;
			calendar3.MarkDay(uint.Parse(DateTime.Now.ToString("dd")));
			
			calendar1.DaySelected += new EventHandler (on_dayselected_clicked); // Lista
			calendar2.DaySelected += new EventHandler (on_dayselected_clicked);	// Agendar la cita
			calendar3.DaySelected += new EventHandler (on_dayselected_clicked);	// Agendar en quirofano
			
			entry_fecha_seleccionada.Text = DateTime.Now.ToString("yyyy-MM-dd");
			sql_fecha1 = " AND to_char(fecha_programacion,'yyyy-MM-dd') = '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' ";
			sql_fecha1_1 = " AND to_char(fechahora_creacion,'yyyy-MM-dd') = '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' ";
			////////////////////////
			// Calendario de Citas
			/////////////////////////
			// Action de the Click Consulta
			checkbutton_canceladas.Clicked += new EventHandler(on_checkbutton_canceladas_clicked);
			button_busca_medicos_consulta.Clicked += new EventHandler(on_button_busca_medicos_clicked);
			checkbutton_filtro_doctor.Clicked += new EventHandler(on_checkbutton_filtro_doctor_clicked);
			button_aplica_filtrodoctor.Clicked += new EventHandler(on_button_aplica_filtrodoctor_clicked);			
			checkbutton_filtro_paciente.Clicked += new EventHandler(on_checkbutton_filtro_paciente_clicked);
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			button_aplica_filtropaciente.Clicked += new EventHandler(on_button_aplica_filtropaciente_clicked);
			checkbutton_filtro_especialidad.Clicked += new EventHandler(on_checkbutton_filtro_especialidad_clicked);
			button_busca_especialidad_consulta.Clicked += new EventHandler(on_button_busca_especialidad_clicked);
			button_aplica_filtroespecialidad.Clicked += new EventHandler(on_button_aplica_filtroespecialidad_clicked);				
			button_cancela_cita.Clicked += new EventHandler(on_button_cancela_cita_clicked);
			button_imprimir_calendario.Clicked += new EventHandler(on_button_imprimir_calendario_clicked);
			checkbutton_px_no_asistieron.Clicked += new EventHandler(on_checkbutton_px_no_asistieron_clicked);
					
			// Action the Click for Citas
			button_guardar_cita.Clicked += new EventHandler(on_button_guardar_cita_clicked);
			checkbutton_crea_cita.Clicked += new EventHandler(on_checkbutton_crea_cita_clicked);
			button_busca_paciente_cita.Clicked += new EventHandler(on_button_busca_paciente_cita_clicked);
			button_infoadmision.Clicked += new EventHandler(on_button_infoadmision_clicked);
			radiobutton_paciente_conexpe_cita.Clicked += new EventHandler(on_radiobutton_paciente_cita_clicked);
			radiobutton_paciente_sinexpe_cita.Clicked += new EventHandler(on_radiobutton_paciente_cita_clicked);
			radiobutton_hombre_cita.Clicked += new EventHandler(on_sexopaciente_clicked);
			radiobutton_mujer_cita.Clicked += new EventHandler(on_sexopaciente_clicked);
			
			button_busca_empresas_cita.Clicked += new EventHandler(on_button_busca_empresas_cita_clicked);
			button_busca_medicos_cita.Clicked += new EventHandler(on_button_busca_medicos_clicked);
			button_busca_especialidad_cita.Clicked += new EventHandler(on_button_busca_especialidad_clicked);
			llenado_tipo_paciente("","",combobox_tipo_paciente_consulta);
			llenado_tipo_servicio("","",combobox_tipo_admision_consulta);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_numero_citas.ModifyBase(StateType.Normal, new Gdk.Color(244,231,86));		// cambia el fondo del entry
			entry_numero_citapaciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));		// cambia el fondo del entry
			entry_numero_citaquirofano.ModifyBase(StateType.Normal, new Gdk.Color(252,95,91));		// cambia el fondo del entry
			
			Pango.FontDescription fontdesc = new Pango.FontDescription(); //Pango.FontDescription.FromString ("Arial 10");
			fontdesc.Weight = Pango.Weight.Bold; // letra a negrita
			entry_numero_citapaciente.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			entry_numero_citaquirofano.ModifyFont(fontdesc);	// Cambia el tipo de letra del Entry
			entry_numero_citas.ModifyFont(fontdesc);
			
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
			entry_referido_por_cita.Sensitive = false;
			combobox_tipo_paciente_cita.Sensitive = false;
			combobox_tipo_admision_cita.Sensitive = false;
			button_infoadmision.Sensitive = false;
			
			////////////////////////
			// Calendario de Quirofano
			/////////////////////////
			checkbutton_crea_citaqx.Clicked += new EventHandler(on_checkbutton_crea_cita_clicked);
			
			statusbar_citasqx.Pop(0);
			statusbar_citasqx.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_citasqx.HasResizeGrip = false; 
			//notebook1.
			
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
			                                         typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool),typeof(bool));
			treeview_lista_agenda.Model = treeViewEngineListaCitas;
			treeview_lista_agenda.RulesHint = true;
			treeview_lista_agenda.RowActivated += on_seleccion_paciente;
			
			col_agenda0 = new TreeViewColumn();		cellrt0 = new CellRendererText();
			col_agenda0.Title = "Fecha";
			col_agenda0.PackStart(cellrt0, true);
			col_agenda0.AddAttribute (cellrt0, "text", 0);
			col_agenda0.Resizable = true;
			col_agenda0.SortColumnId = (int) Coldatos_agenda.col_agenda0;
			
			col_agenda1 = new TreeViewColumn();		cellrt1 = new CellRendererText();
			col_agenda1.Title = "Hora";
			col_agenda1.PackStart(cellrt1, true);
			col_agenda1.AddAttribute (cellrt1, "text", 1);
			col_agenda1.Resizable = true;
			col_agenda1.SortColumnId = (int) Coldatos_agenda.col_agenda1;
			
			col_agenda2 = new TreeViewColumn();		cellrt2 = new CellRendererText();
			col_agenda2.Title = "N° Cita";
			col_agenda2.PackStart(cellrt2, true);
			col_agenda2.AddAttribute (cellrt2, "text", 2);
			col_agenda2.Resizable = true;
			col_agenda2.SortColumnId = (int) Coldatos_agenda.col_agenda2;
			
			col_agenda3 = new TreeViewColumn();		cellrt3 = new CellRendererText();
			col_agenda3.Title = "N° Expediente";
			col_agenda3.PackStart(cellrt3, true);
			col_agenda3.AddAttribute (cellrt3, "text", 3);
			col_agenda3.Resizable = true;
			col_agenda3.SortColumnId = (int) Coldatos_agenda.col_agenda3;
			
			col_agenda4 = new TreeViewColumn();		cellrt4 = new CellRendererText();
			col_agenda4.Title = "Paciente";
			col_agenda4.PackStart(cellrt4, true);
			col_agenda4.AddAttribute (cellrt4, "text", 4);
			col_agenda4.Resizable = true;
			col_agenda4.SortColumnId = (int) Coldatos_agenda.col_agenda4;
            
			col_agenda5 = new TreeViewColumn();		cellrt5 = new CellRendererText();
			col_agenda5.Title = "Edad";
			col_agenda5.PackStart(cellrt5, true);
			col_agenda5.AddAttribute (cellrt5, "text", 5);
			col_agenda5.Resizable = true;
			col_agenda5.SortColumnId = (int) Coldatos_agenda.col_agenda5;
			
			col_agenda6 = new TreeViewColumn();		cellrt6 = new CellRendererText();
			col_agenda6.Title = "Telefonos";
			col_agenda6.PackStart(cellrt6, true);
			col_agenda6.AddAttribute (cellrt6, "text", 6);
			col_agenda6.Resizable = true;
			col_agenda6.SortColumnId = (int) Coldatos_agenda.col_agenda6;
			
			col_agenda7 = new TreeViewColumn();		cellrt7 = new CellRendererText();
			col_agenda7.Title = "email";
			col_agenda7.PackStart(cellrt7, true);
			col_agenda7.AddAttribute (cellrt7, "text", 7);
			col_agenda7.Resizable = true;
			col_agenda7.SortColumnId = (int) Coldatos_agenda.col_agenda7;
			
			col_agenda8 = new TreeViewColumn();		cellrt8 = new CellRendererText();
			col_agenda8.Title = "Tipo Paciente";
			col_agenda8.PackStart(cellrt8, true);
			col_agenda8.AddAttribute (cellrt8, "text", 8);
			col_agenda8.Resizable = true;
			col_agenda8.SortColumnId = (int) Coldatos_agenda.col_agenda8;
			
			col_agenda9 = new TreeViewColumn();		cellrt9 = new CellRendererText();
			col_agenda9.Title = "Tipo Servicio";
			col_agenda9.PackStart(cellrt9, true);
			col_agenda9.AddAttribute (cellrt9, "text", 9);
			col_agenda9.Resizable = true;
			col_agenda9.SortColumnId = (int) Coldatos_agenda.col_agenda9;
			
			col_agenda10 = new TreeViewColumn();	cellrt10 = new CellRendererText();
			col_agenda10.Title = "Consultorio/QX.";
			col_agenda10.PackStart(cellrt10, true);
			col_agenda10.AddAttribute (cellrt10, "text", 10);
			col_agenda10.Resizable = true;
			col_agenda10.SortColumnId = (int) Coldatos_agenda.col_agenda10;
			
			col_agenda11 = new TreeViewColumn();	cellrt11 = new CellRendererText();
			col_agenda11.Title = "Doctor";
			col_agenda11.PackStart(cellrt11, true);
			col_agenda11.AddAttribute (cellrt11, "text", 11);
			col_agenda11.Resizable = true;
			col_agenda11.SortColumnId = (int) Coldatos_agenda.col_agenda11;
			
			col_agenda12 = new TreeViewColumn();	cellrt12 = new CellRendererText();
			col_agenda12.Title = "Sub-Especialidad";
			col_agenda12.PackStart(cellrt12, true);
			col_agenda12.AddAttribute (cellrt12, "text", 12);
			col_agenda12.Resizable = true;
			col_agenda12.SortColumnId = (int) Coldatos_agenda.col_agenda12;
			
			col_agenda13 = new TreeViewColumn();	cellrt13 = new CellRendererText();
			col_agenda13.Title = "Motivo de Consulta";
			col_agenda13.PackStart(cellrt13, true);
			col_agenda13.AddAttribute (cellrt13, "text", 13);
			col_agenda13.Resizable = true;
			col_agenda13.SortColumnId = (int) Coldatos_agenda.col_agenda13;
						
			col_agenda14 = new TreeViewColumn();	cellrt14 = new CellRendererText();
			col_agenda14.Title = "Observaciones";
			col_agenda14.PackStart(cellrt14, true);
			col_agenda14.AddAttribute (cellrt14, "text", 14);
			col_agenda14.Resizable = true;
			col_agenda14.SortColumnId = (int) Coldatos_agenda.col_agenda14;
			
			col_agenda15 = new TreeViewColumn();	cellrt15 = new CellRendererText();
			col_agenda15.Title = "Referido Por";
			col_agenda15.PackStart(cellrt15, true);
			col_agenda15.AddAttribute (cellrt15, "text", 15);
			col_agenda15.Resizable = true;
			col_agenda15.SortColumnId = (int) Coldatos_agenda.col_agenda15;
			
			col_agenda16 = new TreeViewColumn();	cellrt16 = new CellRendererText();
			col_agenda16.Title = "Institu/Empre/Asegu";
			col_agenda16.PackStart(cellrt16, true);
			col_agenda16.AddAttribute (cellrt16, "text", 16);
			col_agenda16.Resizable = true;
			col_agenda16.SortColumnId = (int) Coldatos_agenda.col_agenda16;
			
			col_agenda17 = new TreeViewColumn();	cellrt17 = new CellRendererText();
			col_agenda17.Title = "Agendado por";
			col_agenda17.PackStart(cellrt17, true);
			col_agenda17.AddAttribute (cellrt17, "text", 17);
			col_agenda17.Resizable = true;
			col_agenda17.SortColumnId = (int) Coldatos_agenda.col_agenda17;
			
			col_agenda18 = new TreeViewColumn();
			cellrt18 = new CellRendererText();
			col_agenda18.Title = "Fecha/Hora";
			col_agenda18.PackStart(cellrt18, true);
			col_agenda18.AddAttribute (cellrt18, "text", 18);
			col_agenda18.Resizable = true;
			col_agenda18.SortColumnId = (int) Coldatos_agenda.col_agenda18;
			
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
			//treeview_lista_agenda.AppendColumn(col_agenda19);
		}
		
		enum Coldatos_agenda
		{
			col_agenda0,col_agenda1,col_agenda2,col_agenda3,col_agenda4,col_agenda5,col_agenda6,col_agenda7,col_agenda8,
			col_agenda9,col_agenda10,col_agenda11,col_agenda12,col_agenda13,col_agenda14,col_agenda15,col_agenda16,
			col_agenda17,col_agenda18	
		}
		
		void on_seleccion_paciente (object obj, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if ( treeview_lista_agenda.Selection.GetSelected(out model, out iterSelected)){
				if((string) model.GetValue(iterSelected, 3) != "0"){
					new osiris.registro_paciente_busca("selecciona",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,(string) model.GetValue(iterSelected, 3));
				}else{
					new osiris.registro_paciente_busca("",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,(string) model.GetValue(iterSelected, 3));
				}
			}
		}
		void on_dayselected_clicked (object obj, EventArgs args)
		{
			Gtk.Calendar activatedCalendar = (Gtk.Calendar) obj;
			if(activatedCalendar.Name.ToString() == "calendar1"){
				if((bool) checkbutton_fecha_final.Active == false){
					entry_fecha_seleccionada.Text = activatedCalendar.GetDate().ToString ("yyyy-MM-dd");
					sql_fecha1 = " AND to_char(fecha_programacion,'yyyy-MM-dd') = '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' ";
					sql_fecha1_1 = " AND to_char(fechahora_creacion,'yyyy-MM-dd') = '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' ";
					sql_fecha2 = "";
					sql_fecha2_2 = "";					
				}else{
					entry_fecha_final.Text = activatedCalendar.GetDate().ToString ("yyyy-MM-dd");
					sql_fecha1 = "";
					sql_fecha1_1 = "";
					sql_fecha2 = " AND to_char(fecha_programacion,'yyyy-MM-dd') >= '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' "+
								" AND to_char(fecha_programacion,'yyyy-MM-dd') <= '"+entry_fecha_final.Text.ToString().Trim()+"' ";
					sql_fecha2_2 = " AND to_char(fechahora_creacion,'yyyy-MM-dd') >= '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' "+
								" AND to_char(fechahora_creacion,'yyyy-MM-dd') <= '"+entry_fecha_final.Text.ToString().Trim()+"' ";
				}
				llenado_lista_citas("");				
			}
			if(activatedCalendar.Name.ToString() == "calendar2"){
				entry_fecha_cita.Text = activatedCalendar.GetDate().ToString ("yyyy-MM-dd");	
			}
			if(activatedCalendar.Name.ToString() == "calendar3"){
				entry_fecha_cita_qx.Text = activatedCalendar.GetDate().ToString ("yyyy-MM-dd");	
			}
			//Console.WriteLine (activatedCalendar.Name.ToString());
			//Console.WriteLine (activatedCalendar.GetDate ().ToString ("yyyy/MM/dd"));			
		}		
		
		// Creation the cita patiente 
		void on_checkbutton_crea_cita_clicked(object obj, EventArgs args)
		{
			Gtk.CheckButton checkbutton_crea_citas_qx = (Gtk.CheckButton) obj;
			
			//Console.WriteLine(checkbutton_crea_citas_qx.Name.ToString());
			
			if(checkbutton_crea_citas_qx.Name.ToString() == "checkbutton_crea_cita"){				
				if(checkbutton_crea_cita.Active == true){ 
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear una nueva CITA ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
			 		if (miResultado == ResponseType.Yes){
						entry_id_doctor_cita.Text = "1";
						entry_nombre_doctor_cita.Text = "";
						entry_id_especialidad_cita.Text = "1";
						entry_nombre_especialidad_cita.Text = "";
						entry_id_empaseg_cita.Text = "1";
						entry_nombre_empaseg_cita.Text = "";
						entry_motivoconsulta.Text = "";
						entry_observaciones_cita.Text = "";
						entry_referido_por_cita.Text = "";
						entry_fecha_nac_cita.Text = "";
						entry_pid_paciente_cita.Text = "0";
						entry_nombre_paciente_cita1.Text = "";
						entry_nombre_paciente_cita2.Text = "";
						entry_numero_citapaciente.Text ="0";
						hora_cita_qx = "";
						minutos_cita_qx = "";
						//id_tipointernamiento = 0;
						//id_tipopaciente = 0;
		 				entry_motivoconsulta.Sensitive = true;
						entry_observaciones_cita.Sensitive = true;
						entry_referido_por_cita.Sensitive = true;
						entry_pid_paciente_cita.Sensitive = true;
						entry_nombre_paciente_cita1.Sensitive = true;
						button_busca_paciente_cita.Sensitive = true;
						radiobutton_paciente_conexpe_cita.Sensitive = true;
						radiobutton_paciente_sinexpe_cita.Sensitive = true;
						button_guardar_cita.Sensitive = true;
						combobox_tipo_paciente_cita.Sensitive = true;
						combobox_tipo_admision_cita.Sensitive = true;
						button_busca_medicos_cita.Sensitive = true;
						button_busca_especialidad_cita.Sensitive = true;
						button_busca_empresas_cita.Sensitive = true;
						button_infoadmision.Sensitive = true;
						llenado_estadocivil("","");
						llenado_tipo_paciente("","",combobox_tipo_paciente_cita);
						llenado_tipo_servicio("","",combobox_tipo_admision_cita);
						llena_horas_citas();
					}else{
						checkbutton_crea_cita.Active = false;
					}			
				}else{
					entry_pid_paciente_cita.Sensitive = false;
					entry_nombre_paciente_cita1.Sensitive = false;
					checkbutton_crea_cita.Active = false;
					entry_motivoconsulta.Sensitive = false;
					entry_observaciones_cita.Sensitive = false;
					entry_referido_por_cita.Sensitive = false;
					button_busca_paciente_cita.Sensitive = false;
					radiobutton_paciente_conexpe_cita.Sensitive = false;
					radiobutton_paciente_sinexpe_cita.Sensitive = false;
					button_guardar_cita.Sensitive = false;
					combobox_tipo_paciente_cita.Sensitive = false;
					combobox_tipo_admision_cita.Sensitive = false;
					button_busca_medicos_cita.Sensitive = false;
					button_busca_especialidad_cita.Sensitive = false;
					button_busca_empresas_cita.Sensitive = false;
					button_infoadmision.Sensitive = false;
				}
			}
			if(checkbutton_crea_citas_qx.Name.ToString() == "checkbutton_crea_citaqx"){
				if(checkbutton_crea_citaqx.Active == true){ 
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear una nueva CITA ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
			 		if (miResultado == ResponseType.Yes){
						
					}
				}
			}
		}
		
		void on_checkbutton_px_no_asistieron_clicked(object obj, EventArgs args)
		{
			llenado_lista_citas("");
		}
		
		void on_radiobutton_paciente_cita_clicked(object obj, EventArgs args)
		{
			Gtk.RadioButton radiobutton_paciente_cita = (Gtk.RadioButton) obj;
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_paciente_conexpe_cita"){
				if (radiobutton_paciente_conexpe_cita.Active == true){					
					entry_pid_paciente_cita.Sensitive = true;
					entry_nombre_paciente_cita1.Sensitive = true;
					button_busca_paciente_cita.Sensitive = true;
					button_infoadmision.Sensitive = true;
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
					button_infoadmision.Sensitive = false;
					entry_nombre_paciente_cita2.Sensitive = true;
					entry_fecha_nac_cita.Sensitive = true;
					entry_edad_paciente_cita.Sensitive = true;
					combobox_estado_civil_cita.Sensitive = true;
					entry_telefono_cita.Sensitive = true;
					entry_celular_cita.Sensitive = true;
					entry_mail_cita.Sensitive = true;
					radiobutton_hombre_cita.Sensitive = true;
					radiobutton_mujer_cita.Sensitive = true;
					entry_pid_paciente_cita.Text = "0";
					entry_nombre_paciente_cita1.Text = "";
				}				
			}			
		}
		
		// cambia el estatus del sexo del paciente
		void on_sexopaciente_clicked (object sender, EventArgs args)
		{
			//Console.WriteLine(radiobutton_masculino.Active.ToString());
			Gtk.RadioButton radiobutton_sexopaciente_cita = (Gtk.RadioButton) sender;
			if(radiobutton_sexopaciente_cita.Name.ToString() ==  "radiobutton_hombre_cita"){
				if (radiobutton_hombre_cita.Active == true){
					sexopaciente_cita = "H";
				}else{
					sexopaciente_cita = "M";}
			}
			if(radiobutton_sexopaciente_cita.Name.ToString() == "radiobutton_mujer_cita"){
				if (radiobutton_mujer_cita.Active == true){
					sexopaciente_cita = "M";
				}else{
					sexopaciente_cita = "H";
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
			combobox_estado_civil_cita.Changed += new EventHandler (onComboBoxChanged_estadocivil);
		}
		
		void onComboBoxChanged_estadocivil (object sender, EventArgs args)
		{
			ComboBox combobox_estado_civil = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_estado_civil.GetActiveIter (out iter)){
				estadocivil_cita = (string) combobox_estado_civil.Model.GetValue(iter,0);
			}
		}
		void llenado_tipo_paciente(string tipo_, string descripcion_, object sender)
		{
			// Tipos de Paciente
			ComboBox combobox_tipo_paciente = sender as ComboBox;
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
			//Console.WriteLine(combobox_tipo_paciente.Name.ToString());
			if (obj == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				if(combobox_tipo_paciente.Name.ToString() == "combobox_tipo_paciente_cita"){				
					tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
					id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
					entry_id_empaseg_cita.Text = "1";
					entry_nombre_empaseg_cita.Text = "";
				}
				if(combobox_tipo_paciente.Name.ToString() == "combobox_tipo_paciente_consulta"){
					if(checkbutton_tipo_paciente_consulta.Active == true){
						sql_tipo_paciente = " AND osiris_his_tipo_pacientes.id_tipo_paciente = '"+combobox_tipo_paciente.Model.GetValue(iter,1).ToString()+"' ";
						llenado_lista_citas("");
					}else{
						sql_tipo_paciente = "";
						llenado_lista_citas("");
					}
				}
			}
		}
		
		void llenado_tipo_servicio(string tipo_, string descripcion_, object sender)
		{
			// Llenado de combobox con los tipos de Admisiones y centros de costos
			ComboBox combobox_tipo_admision = sender as ComboBox;
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
			//Console.WriteLine(combobox_tipo_admision.Name.ToString());
			if (obj == null){
				return;
			}
			TreeIter iter;
			if (combobox_tipo_admision.GetActiveIter (out iter)){
				if(combobox_tipo_admision.Name.ToString() == "combobox_tipo_admision_cita"){
					tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);//Console.WriteLine(tipointernamiento);
					id_tipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);//Console.WriteLine(idtipointernamiento);
				}
				if(combobox_tipo_admision.Name.ToString() == "combobox_tipo_admision_consulta"){
					if(checkbutton_tipo_admision_consulta.Active == true){
						sql_tipo_admision = " AND osiris_his_tipo_admisiones.id_tipo_admisiones = '"+combobox_tipo_admision.Model.GetValue(iter,1).ToString()+"' ";
						llenado_lista_citas("");
					}else{
						sql_tipo_admision = "";
						llenado_lista_citas("");
					}
				}
			}
		}
		
		void llena_horas_citas()
		{
			combobox_hora_cita.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_hora_cita.PackStart(cell2, true);
			combobox_hora_cita.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_hora_cita.Model = store2;
			for(int i = (int) classpublic.horario_cita_inicio; i < (int)classpublic.horario_cita_termino+1 ; i++){				
				store2.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_hora_cita.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
			
			combobox_minutos_cita.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_minutos_cita.PackStart(cell3, true);
			combobox_minutos_cita.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_minutos_cita.Model = store3;
			
			for(int i = (int) 0; i < 60 ; i=i+(int) classpublic.intervalo_minutos){				
				store3.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_minutos_cita.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
		}
		
		void onComboBoxChanged_hora_minutos_cita(object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			Gtk.ComboBox hora_minutos_cita = sender as Gtk.ComboBox;			
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (hora_minutos_cita.GetActiveIter (out iter)){
				if(hora_minutos_cita.Name.ToString() == "combobox_hora_cita"){				
					//Console.WriteLine((string) hora_minutos_cita.Model.GetValue(iter,0));
					hora_cita_qx = (string) hora_minutos_cita.Model.GetValue(iter,0);
				}			
				if(hora_minutos_cita.Name.ToString() == "combobox_minutos_cita"){
					//Console.WriteLine((string) hora_minutos_cita.Model.GetValue(iter,0));
					minutos_cita_qx = (string) hora_minutos_cita.Model.GetValue(iter,0);
				}
			}
		}
		
		void on_button_busca_paciente_cita_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_pid_paciente_cita,entry_nombre_paciente_cita1,entry_fecha_nac_cita,entry_edad_paciente_cita };
			string[] parametros_sql = {"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' ",															
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND apellido_paterno_paciente LIKE '%",
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND nombre1_paciente LIKE '%",
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
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
				//Console.WriteLine("Empresas");
				object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' ",															
										"SELECT * FROM osiris_empresas  WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' "+
										"AND descripcion_empresa LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_empresa_cita"," ORDER BY descripcion_empresa","%' ",0);
				idempresa = entry_id_empaseg_cita.Text.ToString().Trim();
				idaseguradora = "1";		
			}else{
				//Console.WriteLine("Aseguradoras");
				// Buscando aseguradora
				object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_aseguradoras ",															
										"SELECT * FROM osiris_aseguradoras "+
										"WHERE descripcion_aseguradora LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_aseguradoras_cita"," ORDER BY descripcion_aseguradora","%' ",0);
				idaseguradora = entry_id_empaseg_cita.Text.ToString().Trim();
				idempresa = "1";
			}			
		}
		
		void on_button_busca_medicos_clicked(object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			Gtk.Button button_busca_medicos = sender as Gtk.Button;
			//Console.WriteLine(button_busca_medicos.Name.ToString());
			if(button_busca_medicos.Name.ToString() == "button_busca_medicos_cita"){
				object[] parametros_objetos = {entry_id_doctor_cita,entry_nombre_doctor_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' ",															
										"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' "+
										"AND nombre_medico LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_medico_cita"," ORDER BY nombre_medico","%' ",0);
			}
			if(button_busca_medicos.Name.ToString() == "button_busca_medicos_consulta"){
				object[] parametros_objetos = {entry_id_doctor_consulta,entry_nombre_doctor_consulta};
				string[] parametros_sql = {"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' ",															
										"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' "+
										"AND nombre_medico LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_medico_consulta"," ORDER BY nombre_medico","%' ",0);
			}
		}
		
		void on_button_busca_especialidad_clicked(object sender, EventArgs args)
		{
			Gtk.Button button_busca_especialidad = sender as Gtk.Button;
			if(button_busca_especialidad.Name.ToString()== "button_busca_especialidad_cita"){
				object[] parametros_objetos = {entry_id_especialidad_cita,entry_nombre_especialidad_cita};
				string[] parametros_sql = {"SELECT * FROM osiris_his_tipo_especialidad ",															
										"SELECT * FROM osiris_his_tipo_especialidad "+
										"WHERE descripcion_especialidad LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_especialidad_cita"," ORDER BY descripcion_especialidad","%' ",0);
			}
			if(button_busca_especialidad.Name.ToString()== "button_busca_especialidad_consulta"){
				object[] parametros_objetos = {entry_id_especialidad_consulta,entry_nombre_especialidad_consulta};
				string[] parametros_sql = {"SELECT * FROM osiris_his_tipo_especialidad ",															
										"SELECT * FROM osiris_his_tipo_especialidad "+
										"WHERE descripcion_especialidad LIKE '%"};			
				classfind_data.buscandor(parametros_objetos,parametros_sql,"find_especialidad_consulta"," ORDER BY descripcion_especialidad","%' ",0);
			}
			
		}
		
		void on_checkbutton_filtro_paciente_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_paciente.Active == true){
				if(entry_fecha_seleccionada.Text.ToString().Trim() != ""){
					if(entry_pid_paciente.Text.ToString().Trim() != ""){
						sql_fecha1 = "";
						sql_fecha2 = "";
						sql_paciente = "AND osiris_his_calendario_citaqx.pid_paciente = '"+entry_pid_paciente.Text.ToString().Trim()+"' ";
						llenado_lista_citas("");
					}else{
						sql_paciente = "";
						llenado_lista_citas("");
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Debe seleccionar una fecha para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
					checkbutton_filtro_doctor.Active = false;
					sql_paciente = "";
				}
			}else{
				sql_paciente = "";
				sql_fecha1 = " AND to_char(fecha_programacion,'yyyy-MM-dd') = '"+entry_fecha_seleccionada.Text.ToString().Trim()+"' ";
				sql_fecha2 = "";
				llenado_lista_citas("");
			}
		}
		
		void on_button_aplica_filtropaciente_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_paciente.Active == true){
				if(entry_pid_paciente.Text.ToString().Trim() != ""){
					sql_paciente = "AND osiris_his_calendario_citaqx.pid_paciente = '"+entry_pid_paciente.Text.ToString().Trim()+"' ";
					llenado_lista_citas("");
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close, "Debe seleccionar un Paciente para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}else{
				sql_paciente = "";
				llenado_lista_citas("");
			}
		}
		
		void on_checkbutton_filtro_doctor_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_doctor.Active == true){
				if(entry_fecha_seleccionada.Text.ToString().Trim() != ""){
					if (entry_id_doctor_consulta.Text.ToString().Trim() != ""){
						sql_doctor = " AND osiris_his_calendario_citaqx.id_medico = '"+entry_id_doctor_consulta.Text.ToString().Trim()+"' ";
						llenado_lista_citas("");
					}else{
						sql_doctor = "";
						llenado_lista_citas("");
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Debe seleccionar una fecha para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
					checkbutton_filtro_doctor.Active = false;
					sql_doctor = "";
				}
			}else{
				sql_doctor = "";
				llenado_lista_citas("");
			}			
		}
		
		void on_button_aplica_filtrodoctor_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_doctor.Active == true){
				if(entry_id_doctor_consulta.Text.ToString().Trim() != ""){
					sql_doctor = " AND osiris_his_calendario_citaqx.id_medico = '"+entry_id_doctor_consulta.Text.ToString().Trim()+"' ";
					llenado_lista_citas("");
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close, "Debe seleccionar un Doctor para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}else{
				sql_doctor = "";
				llenado_lista_citas("");
			}
		}
		
		void on_checkbutton_filtro_especialidad_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_especialidad.Active == true){
				if(entry_fecha_seleccionada.Text.ToString().Trim() != ""){
					if (entry_id_especialidad_consulta.Text.ToString().Trim() != ""){
						sql_especialidad = " AND osiris_his_tipo_especialidad.id_especialidad = '"+entry_id_especialidad_consulta.Text.ToString().Trim()+"' ";
						llenado_lista_citas("");
					}else{
						sql_especialidad = "";
						llenado_lista_citas("");
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Debe seleccionar una fecha para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
					checkbutton_filtro_doctor.Active = false;
					sql_especialidad = "";
				}
			}else{
				sql_especialidad = "";
				llenado_lista_citas("");
			}
		}
		
		void on_button_aplica_filtroespecialidad_clicked(object sender, EventArgs args)
		{
			if(checkbutton_filtro_especialidad.Active == true){
				if(entry_id_especialidad_consulta.Text.ToString().Trim() != ""){
					sql_especialidad = " AND osiris_his_tipo_especialidad.id_especialidad = '"+entry_id_especialidad_consulta.Text.ToString().Trim()+"' ";
					llenado_lista_citas("");
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
													ButtonsType.Close, "Debe seleccionar una Especialidad para aplicar el filtro...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}else{
				sql_especialidad = "";
				llenado_lista_citas("");
			}
		}
		
		
		void llenado_lista_citas(string type_consulta_sql)
		{
			string nombrepaciente;
			string telefonopaciente;
			string emailpaciente;
			string emprinstitucion_aseguradora;
			string observaciongeneral = "";
			bool asistio_a_consulta = true;
			contador_numerocitas = 0;
			treeViewEngineListaCitas.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = sql_calendario_citaqx+sql_fecha1+sql_fecha2+sql_doctor+sql_tipo_paciente+sql_tipo_admision+sql_especialidad+
									sql_citas_canceladas+sql_paciente+" ORDER BY to_char(fecha_programacion,'yyyy-MM-dd'),hora_programacion ASC;";
				
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				while (lector.Read()){					
					// Verificando que los pacientes de la citas tengan su numero de atencion
					NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion.CreateCommand ();
						comando1.CommandText = "SELECT pid_paciente,fechahora_creacion FROM osiris_erp_cobros_enca WHERE pid_paciente = '"+lector["pidpaciente"].ToString().Trim()+"' "+
											sql_fecha1_1+sql_fecha2_2+";";						
						//Console.WriteLine(comando.CommandText);
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						if (lector1.Read()){
							asistio_a_consulta = true;
						}else{
							asistio_a_consulta = false;
						}
					}catch (NpgsqlException ex){
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion1.Close();
					
					if((bool) checkbutton_px_no_asistieron.Active == true){
						if(asistio_a_consulta == false){
							if((string) lector["pidpaciente"].ToString().Trim() != "0"){
								contador_numerocitas += 1;
								if ((int) lector["pidpaciente"] == 0){
									nombrepaciente = (string) lector["nombre_paciente"].ToString().Trim();
									telefonopaciente = (string) lector["telefonopaciente_cita"]+"  "+(string) lector["celular1paciente_cita"].ToString().Trim();
									emailpaciente = (string) lector["emailpaciente_cita"].ToString().Trim();
								}else{
									nombrepaciente = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
										             (string) lector["nombre2_paciente"].ToString().Trim()+" "+
										             (string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
										             (string) lector["apellido_materno_paciente"].ToString().Trim();
									telefonopaciente = (string) lector["telefonoparticular1_paciente"].ToString().Trim()+"  "+(string) lector["celular1_paciente"].ToString().Trim();
									emailpaciente = (string) lector["email_paciente"].ToString().Trim();
								}
								if((int) lector["idempresa"] >= 1){
									emprinstitucion_aseguradora = (string) lector["descripcion_empresa"];
								}else{
									emprinstitucion_aseguradora = (string) lector["descripcion_aseguradora"];
								}					
								if((bool) lector["cancelacitaqx"] == false){
									observaciongeneral = (string) lector["observaciones_citaqx"].ToString().Trim().ToUpper();
								}else{
									observaciongeneral = (string) lector["motivo_cancelacion_citaqx"].ToString().Trim().ToUpper();
								}
								treeViewEngineListaCitas.AppendValues((string) lector["fechaprogramacion"],
								                                      (string) lector["hora_programacion"],
								                                      (string) lector["id_numero_citaqx"].ToString().Trim(),
								                                      (string) lector["pidpaciente"].ToString().Trim(),
								                					  nombrepaciente.Trim(),
								                                      (string) lector["edad"]+" Años y "+(string) lector["mesesedad"]+" Meses",
								                                      (string) telefonopaciente.Trim(),
								                                      (string) emailpaciente,
								                                      (string) lector["descripcion_tipo_paciente"],
								                                      (string) lector["descripcion_admisiones"],
								                                      "",
								                                      (string) lector["nombremedico"],
								                                      (string) lector["descripcionespecialidad"],
								                                      (string) lector["motivo_consulta"],
								                                      observaciongeneral,
								                                      (string) lector["referido_por"],
								                                      emprinstitucion_aseguradora,
								                                      (string) lector["id_quiencreo_cita"],
								                                      (string) lector["fechahoracreacion"].ToString(),
								                                      (string) lector["idsecuencia"].ToString().Trim(),
								                                      (bool) lector["cancelacitaqx"],
																	  asistio_a_consulta);
								col_agenda0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda2.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda3.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda4.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));	
								col_agenda5.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda6.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda7.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda8.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda9.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda10.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda11.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda12.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda13.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda14.SetCellDataFunc(cellrt14, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda15.SetCellDataFunc(cellrt15, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda16.SetCellDataFunc(cellrt16, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda17.SetCellDataFunc(cellrt17, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_agenda18.SetCellDataFunc(cellrt18, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							}
						}						
					}else{
						contador_numerocitas += 1;
						if ((int) lector["pidpaciente"] == 0){
							nombrepaciente = (string) lector["nombre_paciente"].ToString().Trim();
							telefonopaciente = (string) lector["telefonopaciente_cita"]+"  "+(string) lector["celular1paciente_cita"].ToString().Trim();
							emailpaciente = (string) lector["emailpaciente_cita"].ToString().Trim();
						}else{
							nombrepaciente = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
								             (string) lector["nombre2_paciente"].ToString().Trim()+" "+
								             (string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
								             (string) lector["apellido_materno_paciente"].ToString().Trim();
							telefonopaciente = (string) lector["telefonoparticular1_paciente"].ToString().Trim()+"  "+(string) lector["celular1_paciente"].ToString().Trim();
							emailpaciente = (string) lector["email_paciente"].ToString().Trim();
						}
						if((int) lector["idempresa"] >= 1){
							emprinstitucion_aseguradora = (string) lector["descripcion_empresa"];
						}else{
							emprinstitucion_aseguradora = (string) lector["descripcion_aseguradora"];
						}					
						if((bool) lector["cancelacitaqx"] == false){
							observaciongeneral = (string) lector["observaciones_citaqx"].ToString().Trim().ToUpper();
						}else{
							observaciongeneral = (string) lector["motivo_cancelacion_citaqx"].ToString().Trim().ToUpper();
						}
						treeViewEngineListaCitas.AppendValues((string) lector["fechaprogramacion"],
						                                      (string) lector["hora_programacion"],
						                                      (string) lector["id_numero_citaqx"].ToString().Trim(),
						                                      (string) lector["pidpaciente"].ToString().Trim(),
						                					  nombrepaciente.Trim(),
						                                      (string) lector["edad"]+" Años y "+(string) lector["mesesedad"]+" Meses",
						                                      (string) telefonopaciente.Trim(),
						                                      (string) emailpaciente,
						                                      (string) lector["descripcion_tipo_paciente"],
						                                      (string) lector["descripcion_admisiones"],
						                                      "",
						                                      (string) lector["nombremedico"],
						                                      (string) lector["descripcionespecialidad"],
						                                      (string) lector["motivo_consulta"],
						                                      observaciongeneral,
						                                      (string) lector["referido_por"],
						                                      emprinstitucion_aseguradora,
						                                      (string) lector["id_quiencreo_cita"],
						                                      (string) lector["fechahoracreacion"].ToString(),
						                                      (string) lector["idsecuencia"].ToString().Trim(),
						                                      (bool) lector["cancelacitaqx"],
															  asistio_a_consulta);
						col_agenda0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda2.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda3.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda4.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));	
						col_agenda5.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda6.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda7.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda8.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda9.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda10.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda11.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda12.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda13.SetCellDataFunc(cellrt13, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda14.SetCellDataFunc(cellrt14, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda15.SetCellDataFunc(cellrt15, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda16.SetCellDataFunc(cellrt16, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda17.SetCellDataFunc(cellrt17, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						col_agenda18.SetCellDataFunc(cellrt18, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					}
				}
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
			entry_numero_citas.Text = contador_numerocitas.ToString().Trim();
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool) treeview_lista_agenda.Model.GetValue (iter,20) == false){
				if ((bool) treeview_lista_agenda.Model.GetValue (iter,21) == false){
					(cell as Gtk.CellRendererText).CellBackground = "red";
				}else{
					(cell as Gtk.CellRendererText).CellBackground = "green";
				}
				if((string) treeview_lista_agenda.Model.GetValue (iter,3) == "0"){
					(cell as Gtk.CellRendererText).CellBackground = "white";
				}
				//(cell as Gtk.CellRendererText).CellBackgroundGdk = new Gdk.Color(0,0,0);
			}else{
				(cell as Gtk.CellRendererText).CellBackground = "yellow";
				//(cell as Gtk.CellRendererText).CellBackgroundGdk = new Gdk.Color(12,23,67);
			}
		}
		
		void on_button_guardar_cita_clicked(object sender, EventArgs args)
		{
			string numerocita_qx = "";
			if(checkbutton_crea_cita.Active == true){
				if ((bool) validacion_informacion_cita() == true){ 				 
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar esta CITA ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
	 				if (miResultado == ResponseType.Yes){					
						numerocita_qx = classpublic.lee_ultimonumero_registrado("osiris_his_calendario_citaqx","id_numero_citaqx","WHERE id_tipocita = '1' ");
						entry_numero_citapaciente.Text = numerocita_qx.ToString().Trim();
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd );
				
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							comando.CommandText ="INSERT INTO osiris_his_calendario_citaqx (" +
							"id_numero_citaqx,"+
							"pid_paciente,"+
							"id_tipocita,"+
							"fechahora_creacion,"+
							"nombre_paciente,"+
							"sexo_paciente,"+						
							"fecha_nacimiento_paciente,"+
							"estado_civil_paciente,"+
							"email_paciente,"+
							"telefono_paciente,"+
							"celular1_paciente,"+
							"id_tipo_paciente,"+
							"id_tipo_admisiones,"+
							"id_aseguradora,"+
							"id_empresa,"+
							"id_medico,"+
							"nombre_medico,"+
							"id_especialidad,"+
							"descripcion_especialidad,"+
							"motivo_consulta,"+
							"observaciones,"+
							"referido_por,"+
							"id_quiencreo_cita,"+
							"fecha_programacion,"+
							"hora_programacion "+
							//"folio_de_servicio,"+
							//"id_cirujano,"+
							//"id_neonatologo,"+
							//"id_ayudante,"+
							//"id_anestesiologo,"+
							//"id_circulante1,"+
							//"id_circulante2,"+
							//"id_internista,"+
							//"id_tipo_cirugia,"+
							//"id_diagnostico,
							//"id_quiencreo,"+							
							//"instrumentacion_especial,"+
							//"inicio_cirugia,"+
							//"termino_cirugia,"+
							//"entrada_sala,"+
							//"salida_sala,"+
							//"cirujano,"+
							//"neonatologo,"+
							//"ayudante,"+
							//"anestesiologo,"+
							//"circulante1,"+
							//"circulante2,"+
							//"internista,"+
							//"id_presupuesto,"+							
							//"aseguradora,"+
							//"diagnostico,"+
							//"cirugia,"+							
							//"especialidad_cirugia,"+
							//"tipo_anestecia,"+
							//"id_cancelacion,"+
							//"cancelado,"+							
							//"id_tipocita,"+						
							//"id_habitacion,"+
							//"id_habitacion1,"+
							//"descripcion_qx_utilizado,"+
							")"+" VALUES ('"+
							numerocita_qx.ToString().Trim()+"','"+
							entry_pid_paciente_cita.Text.Trim()+"','"+
							"1','"+
			 				(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							(string) entry_nombre_paciente_cita2.Text.Trim().ToUpper()+"','"+
							sexopaciente_cita+"','"+
							entry_fecha_nac_cita.Text.ToString().Trim()+"','"+
							estadocivil_cita.Trim().ToUpper()+"','"+
							entry_mail_cita.Text.ToString().Trim()+"','"+
							entry_telefono_cita.Text.ToString().Trim().ToUpper()+"','"+
							entry_celular_cita.Text.ToString().Trim().ToUpper()+"','"+
							id_tipopaciente.ToString().Trim()+"','"+
							id_tipointernamiento.ToString().Trim()+"','"+
							idaseguradora+"','"+
							idempresa+"','"+
							entry_id_doctor_cita.Text.ToString().Trim()+"','"+
							entry_nombre_doctor_cita.Text.ToString().Trim()+"','"+
							entry_id_especialidad_cita.Text.ToString().Trim()+"','"+
							entry_nombre_especialidad_cita.Text.ToString().Trim()+"','"+
							entry_motivoconsulta.Text.ToString().Trim().ToUpper()+"','"+
							entry_observaciones_cita.Text.ToString().Trim().ToUpper()+"','"+
							entry_referido_por_cita.Text.ToString().Trim().ToUpper()+"','"+
							LoginEmpleado+"','"+
							entry_fecha_cita.Text+"','"+
							hora_cita_qx+":"+minutos_cita_qx+						
							"')";
							Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();					comando.Dispose();
							checkbutton_crea_cita.Active = false;
							radiobutton_paciente_conexpe_cita.Active = true;
																					
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info, 
												ButtonsType.Close, "Cita a Paciente Creada con EXITO..");
							msgBoxError.Run ();						msgBoxError.Destroy();
							
						}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();				msgBoxError.Destroy();					
						}
						conexion.Close ();
					}
				}
			}
		}
		
		bool validacion_informacion_cita()
		{
			bool response_validation = false;
			if(id_tipointernamiento != 0 && id_tipopaciente != 0 && entry_id_especialidad_cita.Text.ToString().Trim() != "1"
			   && entry_fecha_cita.Text != "" && hora_cita_qx != "" && minutos_cita_qx != ""){
				if((bool) verifica_cita_doctor() == true){
					response_validation = true;
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, 
									"El Doctor ya tiene un Paciente CITADO, Verifique...");
					msgBoxError.Run ();				msgBoxError.Destroy();
					response_validation = false;
				}				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, 
									"Verifique la informacion ya que no esta Completa, NO se creo la CITA");
					msgBoxError.Run ();				msgBoxError.Destroy();
				response_validation = false;
			}
			return response_validation;
		}
		
		bool verifica_cita_doctor()
		{
			bool response_validation = true;
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText = "SELECT * FROM osiris_his_calendario_citaqx WHERE fecha_programacion = '"+entry_fecha_cita.Text.ToString().Trim()+"' "+
							"AND hora_programacion = '"+hora_cita_qx+":"+minutos_cita_qx+"' "+
							"AND id_medico = '"+entry_id_doctor_cita.Text.ToString().Trim()+"' "+
							"AND osiris_his_calendario_citaqx.cancelado = 'false';";				
				Console.WriteLine(comando1.CommandText);
				NpgsqlDataReader lector1 = comando1.ExecuteReader();				
				if (lector1.Read()){
					response_validation = false;
				}else{
					response_validation = true;
				}
			}catch (NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();					
			}
			conexion1.Close ();
			return response_validation;	
		}
		
		void on_button_cancela_cita_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
			gxml.Autoconnect (this);
			cancelador_folios.Show();
			cancelador_folios.Title = "CANCELACION DE CITAS";			
			entry_folio.IsEditable = false;
			label247.Text = "Numero de Cita";
			button_cancelar.Clicked += new EventHandler(on_button_cancelar_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			TreeIter iter;
 			TreeModel model;
 			if (treeview_lista_agenda.Selection.GetSelected (out model, out iter)){
				entry_folio.Text = (string) treeview_lista_agenda.Model.GetValue(iter,2);
			}
		}
		
		void on_button_cancelar_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			if (treeview_lista_agenda.Selection.GetSelected (out model, out iter)){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer CANCELAR esta CITA ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd );
				
					// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "UPDATE osiris_his_calendario_citaqx "+
										"SET cancelado = 'true' , "+
										"fechahora_cancelacion = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
										"id_quien_cancelo = '"+LoginEmpleado+"', "+		
										"motivo_cancelacion_citaqx = '"+entry_motivo.Text.ToString().Trim().ToUpper()+"' "+
				 						"WHERE id_secuencia =  '"+(string) treeview_lista_agenda.Model.GetValue (iter,19)+"';";
						comando.ExecuteNonQuery();					comando.Dispose();
						contador_numerocitas += -1;
						entry_numero_citas.Text = contador_numerocitas.ToString().Trim();
						treeViewEngineListaCitas.Remove (ref iter);
						cancelador_folios.Destroy();						
					}catch (NpgsqlException ex){
		   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				msgBoxError.Destroy();					
					}
					conexion.Close ();
				}
			}
		}
		
		void on_checkbutton_canceladas_clicked(object sender, EventArgs args)
		{
			if(checkbutton_canceladas.Active == true){
				sql_citas_canceladas = "";
			}else{
				sql_citas_canceladas = "AND osiris_his_calendario_citaqx.cancelado = 'false' ";
			}
			llenado_lista_citas("");
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    {
			object[] parametros_objetos = {entry_pid_paciente,entry_nombre_paciente};
			string[] parametros_sql = {"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' ",															
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND apellido_paterno_paciente LIKE '%",
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND nombre1_paciente LIKE '%",
										"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "+
										"AND pid_paciente = '"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_paciente1"," ORDER BY osiris_his_paciente.pid_paciente","%' ",1);
		}
		
		void on_button_infoadmision_clicked(object sender, EventArgs args)
		{
			new osiris.registro_paciente_busca("selecciona",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,this.entry_pid_paciente_cita.Text.ToString().Trim());
		}
		
		void on_button_imprimir_calendario_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_reporte_citasqx(treeview_lista_agenda,treeViewEngineListaCitas);			
		}
		
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}		
	}
}