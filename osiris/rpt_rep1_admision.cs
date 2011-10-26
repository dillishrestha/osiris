// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 03/06/2010
//				  Traspaso a GTKprint y la creacion de la clase
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
//////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rptAdmision
	{
		[Widget] Gtk.Window rango_rep_adm = null;
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Entradas y botones de la ventana
		[Widget] Gtk.Entry entry_dia_inicial = null;
		[Widget] Gtk.Entry entry_mes_inicial = null;
		[Widget] Gtk.Entry entry_ano_inicial = null;
		[Widget] Gtk.Entry entry_dia_final = null;
		[Widget] Gtk.Entry entry_mes_final = null;
		[Widget] Gtk.Entry entry_ano_final = null;
		[Widget] Gtk.Entry entry_id_empaseg_cita = null;
		[Widget] Gtk.Entry entry_nombre_empaseg_cita = null;
		[Widget] Gtk.Entry entry_id_doctor_consulta = null;
		[Widget] Gtk.Entry entry_nombre_doctor_consulta = null;
						
		//ComboBox
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		
		//CheckButtons
		[Widget] Gtk.CheckButton checkbutton_todas_fechas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_admision = null;
		[Widget] Gtk.CheckButton checkbutton_todos_paciente = null;
		[Widget] Gtk.CheckButton checkbutton_todas_empresas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_doctores = null;
						
		//radio buttons
		[Widget] Gtk.RadioButton radiobutton_masculino = null;
		[Widget] Gtk.RadioButton radiobutton_femenino = null;
		[Widget] Gtk.RadioButton radiobutton_ambos_sexos = null;
		[Widget] Gtk.RadioButton radiobutton_cancelados = null;
		[Widget] Gtk.RadioButton radiobutton_no_cancelados = null;
		[Widget] Gtk.RadioButton radiobutton_reporte_general = null;
		[Widget] Gtk.RadioButton radiobutton_folio_servicio = null;
		[Widget] Gtk.RadioButton radiobutton_pid_paciente = null;
		[Widget] Gtk.RadioButton radiobutton_nombres = null;
		[Widget] Gtk.RadioButton radiobutton_doctores = null;
		[Widget] Gtk.RadioButton radiobutton_tipo_admision = null;
		
		//botones
		[Widget] Gtk.Button button_busca_empresa = null;
		[Widget] Gtk.Button button_busca_doctores = null;
		[Widget] Gtk.Button button_imprimir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		
		//ventana de busqueda de medicos
		[Widget] Gtk.TreeView lista_de_medicos = null;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda = null;
		
		//ventana de busqueda de empresas
		[Widget] Gtk.TreeView lista_de_empresas = null;
		[Widget] Gtk.Button button_busca_empresas = null;
		
		private TreeStore treeViewEngineMedicos;
		private ListStore treeViewEngineEmpresa;
		private ListStore treeViewEngineAseguradora;
		
		protected Gtk.Window MyWinError;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 95;
		int separacion_linea = 10;
		int numerpage = 1;
		int contador = 0;
		string connectionString;
        string nombrebd;
	    string tipointernamiento = "CENTRO MEDICO";
   		int idtipointernamiento = 10;
   	    string tipopaciente = "Membresias"; 
		int id_tipopaciente = 100;
		string motivo = "";
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		string busqueda = "";
				
    	string query_reporte = "SELECT "+
				"osiris_erp_movcargos.id_tipo_admisiones,osiris_his_tipo_admisiones.descripcion_admisiones, "+
				"osiris_erp_movcargos.folio_de_servicio,osiris_erp_movcargos.folio_de_servicio_dep,osiris_erp_cobros_enca.cancelado, "+
				"to_char(fechahora_admision_registro,'dd-MM-yyyy') AS fech_reg_adm, "+ 
				"fechahora_admision_registro,osiris_erp_movcargos.id_tipo_paciente, "+
				"osiris_erp_movcargos.pid_paciente,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente, "+
				"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
				"osiris_aseguradoras.id_aseguradora, grupo_sanguineo_paciente, "+
				"to_char(fechahora_admision_registro,'HH24:mi') AS hora_reg_adm, "+ 
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy'),'9999'),'9999') AS edad,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				"direccion_paciente,numero_casa_paciente,codigo_postal_paciente,estado_civil_paciente,osiris_erp_cobros_enca.responsable_cuenta, "+
				"colonia_paciente,numero_departamento_paciente,ocupacion_paciente,sexo_paciente, "+
				"to_char(fecha_nacimiento_paciente,'dd-MM-yyyy') AS fech_nacimiento, "+
				"descripcion_tipo_paciente,id_empleado_admision,osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
				"osiris_erp_cobros_enca.id_empresa,public.osiris_erp_cobros_enca.id_medico,osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_cobros_enca.motivo_cancelacion, "+
				"numero_certificado,numero_poliza,osiris_erp_cobros_enca.numero_factura,"+
				//"sub_total_15,sub_total_0,iva_al_15,osiris_erp_factura_enca.honorario_medico,"+
				"historial_facturados,total_procedimiento,tipo_cirugia,"+
				"descripcion_empresa, osiris_his_medicos.nombre_medico,osiris_his_tipo_cirugias.id_tipo_cirugia, descripcion_cirugia, empresa_labora_responsable, "+				
				"nombre_medico_encabezado,"+
				"id_medico_tratante,nombre_medico_tratante,osiris_erp_movcargos.descripcion_diagnostico_cie10 "+
				"FROM "+
				"osiris_erp_movcargos, osiris_his_paciente, osiris_his_tipo_pacientes, osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_aseguradoras,osiris_empresas, "+ 
				"osiris_his_tipo_cirugias,osiris_his_medicos "+   //,osiris_erp_factura_enca "+
				"WHERE  "+
				"osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente  "+
				"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente  "+
				"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio  "+
				"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente  "+
				"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones  "+
				"AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
				"AND osiris_empresas.id_empresa = osiris_erp_cobros_enca.id_empresa "+
				//"AND osiris_empresas.id_empresa = osiris_his_paciente.id_empresa "+  // enlase empresa con el paciente
				//"AND osiris_erp_factura_enca.numero_factura = osiris_erp_cobros_enca.numero_factura "+
				//"AND osiris_empresas.id_empresa = osiris_his_medicos.id_empresa "+
				//"AND osiris_empresas.id_empresa = 3 "+//se aactiva para cuando se quiera ver san nicolas
				"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
				"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico ";
    	string query_tipo_admision  = "AND osiris_erp_movcargos.id_tipo_admisiones = '0' ";
		string query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '200' ";
    	string query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "+
										"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "; 
    	/*query_rango_fechas = "AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) >= '"+DateTime.Now.ToString("yyyy")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) <= '"+DateTime.Now.ToString("yyyy")+"' "+  
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) >= '"+DateTime.Now.ToString("MM")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) <= '"+DateTime.Now.ToString("MM")+"' "+
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) >= '"+DateTime.Now.ToString("dd")+"'  AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) <= '"+DateTime.Now.ToString("dd")+"' " ;
		*/
		string query_sexo = " "; 
    	string query_empresa = " "; //"AND osiris_erp_cobros_enca.id_empresa = 3 "; 
    	string query_aseguradora = " ";
    	string query_tipo_reporte = " ";
    	string query_medico = " ";
    	string query_orden =  "ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";
		
		string tipo_de_salida = ""; 
		
		string idempresa = "1";
		string idaseguradora = "1";
		string idmedico = "1";
    		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
                               
		/// <summary>
		/// Genera reporte inteligente de consulta
		/// </summary>
		/// <param name="nombrebd_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="tipo_reporte impresora o archivo">
		/// A <see cref="System.String"/>
		/// </param>
		public rptAdmision (string nombrebd_,string tipo_de_salida_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tipo_de_salida = tipo_de_salida_;
			//crea la ventana de glade
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "rango_rep_adm", null);
			gxml.Autoconnect (this);
			//muestra la ventana de reportes
			rango_rep_adm.Show();
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			// Imprime reporte
			if(tipo_de_salida == "impresora"){
				button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			}
			if(tipo_de_salida == "archivo"){
				button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);	
			}
			
			button_busca_doctores.Clicked += new EventHandler(on_button_busca_doctores_clicked);
			button_busca_empresa.Clicked += new EventHandler(on_button_busca_empresa_clicked);
			
			//Activo los valores por default de busqueda;
	    	radiobutton_ambos_sexos.Active = true;
	    	radiobutton_folio_servicio.Active = true;
	    	radiobutton_reporte_general.Active = true;
	    	
	    	//acciones al dar click en los botones
	    	checkbutton_todos_admision.Clicked += new EventHandler(on_checkbutton_todos_admision_clicked);
	    	checkbutton_todos_paciente.Clicked += new EventHandler(on_checkbutton_todos_paciente_clicked);
	    	checkbutton_todas_fechas.Clicked += new EventHandler(on_checkbutton_todas_fechas_clicked);
	    	checkbutton_todas_empresas.Clicked += new EventHandler(on_checkbutton_todas_empresas_clicked);
	    	checkbutton_todos_doctores.Clicked += new EventHandler(on_checkbutton_todos_doctores_clicked);
	    	    	   		    	
		    // Desactivando Combobox en la entrada para que el usuario lo pueda elegir o activar
		    combobox_tipo_admision.Sensitive = false;
		    combobox_tipo_paciente.Sensitive = false;
		    //Activando los check buttons paa que se inicialicen activos
		    checkbutton_todos_admision.Active = true;
		    checkbutton_todos_paciente.Active = true;
		   		    
		    // Salir de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			llenado_combobox();
		}
		
		void llenado_combobox()
		{
			combobox_tipo_admision.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
				        	        
			// lleno de la tabla de his_tipo_de_admisiones
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
               						"WHERE cuenta_mayor = 4000 "+
               						" ORDER BY descripcion_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read()){
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
						
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
	    
		    // Tipos de Paciente
		    combobox_tipo_paciente.Clear();
		    CellRendererText cell1 = new CellRendererText();
		    combobox_tipo_paciente.PackStart(cell1, true);
		    combobox_tipo_paciente.AddAttribute(cell1,"text",0);
	        
		    ListStore store1 = new ListStore( typeof (string),typeof (int));
		    combobox_tipo_paciente.Model = store1;
	        // lleno de la tabla de his_tipo_de_pacientes
			 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_pacientes "+
               						//"WHERE id_tipo_paciente > 10  "+
               						" ORDER BY id_tipo_paciente;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//store1.AppendValues ("", 0);
               	while (lector.Read()){
					store1.AppendValues ((string) lector["descripcion_tipo_paciente"], (int) lector["id_tipo_paciente"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	                  
		    TreeIter iter1;
		    if (store1.GetIterFirst(out iter1))
		    {
		       	combobox_tipo_paciente.SetActiveIter (iter1);
	    	}
	    	combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipopaciente);
		}
				
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
	  		ComboBox combobox_tipo_admision = sender as ComboBox;
	  		if (sender == null) { return; }
			TreeIter iter;
			if (combobox_tipo_admision.GetActiveIter (out iter)){
	   			tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
	   			idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
	   			query_tipo_admision = "AND osiris_erp_movcargos.id_tipo_admisiones = '"+idtipointernamiento.ToString()+"' ";
		   	}
		}
		
		///////// Activa desactiva combobox de tipo Paciente
		void onComboBoxChanged_tipopaciente(object sender, EventArgs args)
		{
	 		ComboBox combobox_tipo_paciente = sender as ComboBox;
	   		if (sender == null) { return; }
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
		   		tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
		   		id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
		   		query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
		    }
		}
		
		void on_button_busca_doctores_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_doctor_consulta,entry_nombre_doctor_consulta};
			string[] parametros_sql = {"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' ",															
										"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' "+
										"AND nombre_medico LIKE '%"};			
			classfind_data.buscandor(parametros_objetos,parametros_sql,"find_medico_consulta"," ORDER BY nombre_medico","%' ",0);
			idmedico = entry_id_doctor_consulta.Text.ToString().Trim();
		}
		
			
		void on_button_busca_empresa_clicked(object sender, EventArgs args)
		{
			//query_empresa = "AND hscmty_empresas.id_empresa = '"+idempresa.ToString()+"' ";
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			Console.WriteLine(id_tipopaciente.ToString());
			if(checkbutton_todos_paciente.Active == false){
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
		}
		
		/////////////////Acciones del boton todas las admisiones
		void on_checkbutton_todos_admision_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_admision.Active == true){
				combobox_tipo_admision.Sensitive = false;
				query_tipo_admision = " ";			
			}else{
				//query_tipo_admision = "AND osiris_erp_movcargos.id_tipo_admisiones = '"+idtipointernamiento.ToString()+"' ";
				combobox_tipo_admision.Sensitive = true;
			}
		}
		/////////////////Acciones del boton todos los tipos de pacientes
		void on_checkbutton_todos_paciente_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_paciente.Active == true){
				query_tipo_paciente= " ";
				combobox_tipo_paciente.Sensitive = false;
			}else{
				//query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
				combobox_tipo_paciente.Sensitive = true;
			}
		}
		
		void on_checkbutton_todas_empresas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_empresas.Active == false){
				button_busca_empresa.Sensitive = true;
				query_empresa = " ";
				query_aseguradora = " ";
			}else{
				if (id_tipopaciente != 400){
					query_empresa = "AND osiris_empresas.id_empresa = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}else{
					query_aseguradora = "AND hscmty_erp_cobros_enca.id_aseguradora = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}
				button_busca_empresa.Sensitive = false;
			}
		}
		
		void on_checkbutton_todos_doctores_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_doctores.Active == false){
				button_busca_doctores.Sensitive = true;
				query_medico = " ";			
			}else{
				query_medico = "AND osiris_his_medicos.id_medico = '"+entry_id_doctor_consulta.Text.ToString()+"' ";
				button_busca_doctores.Sensitive = false;
			}
		}
		
		/////////////////Acciones del boton todas las fechas
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_fechas.Active == true){
				query_rango_fechas= " ";
				entry_dia_inicial.Sensitive = false;
				entry_mes_inicial.Sensitive = false;
				entry_ano_inicial.Sensitive = false;
				entry_dia_final.Sensitive = false;
				entry_mes_final.Sensitive = false;
				entry_ano_final.Sensitive = false;
			}else{	
				query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				entry_dia_inicial.Sensitive = true;
				entry_mes_inicial.Sensitive = true;
				entry_ano_inicial.Sensitive = true;
				entry_dia_final.Sensitive = true;
				entry_mes_final.Sensitive = true;
				entry_ano_final.Sensitive = true;
			}
		}
	
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void tipo_de_reporte_a_mostrar(object sender, EventArgs args)
		{
			if(radiobutton_reporte_general.Active == true) { query_tipo_reporte = " "; }
			if(radiobutton_cancelados.Active == true) { query_tipo_reporte = " AND cancelado = 'true' "; }
			if(radiobutton_no_cancelados.Active == true) { query_tipo_reporte = " AND cancelado = 'false' "; }	
		}
		
		void tipo_de_sexo(object sender, EventArgs args)
		{
			if(radiobutton_ambos_sexos.Active == true) { query_sexo = "  "; }
			if(radiobutton_masculino.Active == true) { query_sexo = "AND sexo_paciente = 'H'"; }
			if(radiobutton_femenino.Active == true) { query_sexo = "AND sexo_paciente = 'M'"; }
		}
		
		void tipo_orden_query(object sender, EventArgs args)
		{
			if(radiobutton_folio_servicio.Active == true) { query_orden = "ORDER BY osiris_erp_cobros_enca.folio_de_servicio ;" ; }
			if(radiobutton_pid_paciente.Active == true) { query_orden = "ORDER BY  osiris_erp_cobros_enca.pid_paciente ;"; }
			if(radiobutton_nombres.Active == true) { query_orden = "ORDER BY nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente ;"; }
			if(radiobutton_doctores.Active == true) { query_orden = "ORDER BY  nombre1_medico || ' ' || nombre2_medico || ' ' || apellido_paterno_medico || ' ' || apellido_materno_medico ;"; }
			if(radiobutton_tipo_admision.Active == true) { query_orden = "ORDER BY osiris_his_tipo_admisiones.id_tipo_admisiones ;"; }
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs a)
		{		
			tipo_de_reporte_a_mostrar(sender, a);
	    	tipo_de_sexo(sender, a);
			tipo_orden_query(sender, a);
			
			if(tipo_de_salida == "impresora"){
				genera_reporte_admision();
			}
			if(tipo_de_salida == "archivo"){
				
				if (id_tipopaciente != 400){
					query_empresa = "AND osiris_erp_comprobante_servicio.id_empresa = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}else{
					query_aseguradora = "AND hscmty_erp_cobros_enca.id_aseguradora = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}
				if (checkbutton_todos_paciente.Active == true){
					query_tipo_paciente= " ";					
				}else{
					query_tipo_paciente = "AND osiris_erp_comprobante_servicio.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";					
				}				
				if (checkbutton_todas_fechas.Active == true){
					query_rango_fechas= " ";
					entry_dia_inicial.Sensitive = false;
					entry_mes_inicial.Sensitive = false;
					entry_ano_inicial.Sensitive = false;
					entry_dia_final.Sensitive = false;
					entry_mes_final.Sensitive = false;
					entry_ano_final.Sensitive = false;
				}else{	
					query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				}
				
				//"AND osiris_erp_comprobante_servicio.id_tipo_paciente = '102' " +
				//"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'MM') = '10' " +
				//"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy') = '2011' " +
				//"AND osiris_erp_comprobante_servicio.id_empresa = '31' " +
				//"ORDER BY osiris_erp_cobros_deta.folio_de_servicio ASC;";
				
				string query_sql = "SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente,sexo_paciente,"+
					"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"osiris_grupo_producto.descripcion_grupo_producto, osiris_productos.id_grupo_producto,  to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc," +
						 "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mm') AS horacreacion," +
						 "to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada," +
						 "to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod," +
						 "to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto," +
						 "to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad," +
						 "to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_comprobante_servicio.numero_comprobante_servicio AS numerorecibo," +
						 "osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente," +
						 "to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,to_char(to_number(to_char(age('2011-01-20 05:25:11',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
						 "telefono_particular1_paciente,osiris_erp_comprobante_servicio.observaciones AS nro_oficio,osiris_erp_comprobante_servicio.observaciones2 AS nro_nomina,osiris_erp_comprobante_servicio.observaciones3 AS departamento," +
						 "osiris_erp_comprobante_servicio.concepto_del_comprobante AS concepto_comprobante,osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado AS medico_tratante," +
						 "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora "+
						 "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_comprobante_servicio,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_aseguradoras " +
						 "WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
						 "AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto " +
						 "AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
						 "AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente " +
						 "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa " +
						 "AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
						 "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio " +
						 "AND osiris_erp_comprobante_servicio.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio " +
						 "AND osiris_erp_cobros_deta.eliminado = 'false' " +
						
						query_tipo_paciente + 
						query_sexo + 
						query_empresa + 
						query_aseguradora +
						query_tipo_reporte +
						query_rango_fechas + 
						query_orden;
								
				string[] args_names_field = {"foliodeservicio","pidpaciente","descripcion_admisiones","descripcion_grupo_producto","fechcreacion","idproducto","descripcion_producto",
										"cantidadaplicada","preciounitario","numerorecibo","nombre_paciente","nro_oficio","nro_nomina","departamento","medico_tratante"};
				string[] args_type_field = {"float","float","string","string","string","float","string","float","float","float","string","string","string","string","string"};
				
				// class_crea_ods.cs
				//Console.WriteLine(query_sql);
				new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field);
			}
		}
		
		void genera_reporte_admision()
		{			
			print = new PrintOperation ();
			print.JobName = "Reporte Admisiones";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			//imprime_encabezado(cr,layou);
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string fechas_registros = "";
			string edad;
			int contador = 0;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);				layout.FontDescription = desc;
			
			comienzo_linea = 85;
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	    
        	// Verifica que la base de datos este conectada
        	try{
        		conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
             	
             	if (checkbutton_todas_fechas.Active == true){
					query_rango_fechas= " ";
					entry_dia_inicial.Sensitive = false;
					entry_mes_inicial.Sensitive = false;
					entry_ano_inicial.Sensitive = false;
					entry_dia_final.Sensitive = false;
					entry_mes_final.Sensitive = false;
					entry_ano_final.Sensitive = false;
				}else{	
					query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				}
			 	
				comando.CommandText = query_reporte + query_tipo_admision + query_tipo_paciente + query_sexo + 
									query_empresa + query_aseguradora + query_tipo_reporte + query_medico +
									query_rango_fechas + query_orden;
				Console.WriteLine(comando.CommandText.ToString());
 				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					if(int.Parse((string) lector["edad"]) > 0){
						edad = (string) lector["edad"]+" Años"; 
					}else{
						edad = (string) lector["mesesedad"]+" Meses";
					}
					fechas_registros = (string) lector["fech_reg_adm"].ToString();
					imprime_encabezado(cr,layout);
					layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("-->"+(string) lector["fech_reg_adm"].ToString()+"<--");			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;// Letra negrita
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["folio_de_servicio"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(50*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["pid_paciente"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(90*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["hora_reg_adm"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(115*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(((string) lector["nombre1_paciente"].ToString().Trim()+" "+ 
				            	     																						(string) lector["nombre2_paciente"].ToString().Trim()+" "+
				        	         																						(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
				    	             																						(string) lector["apellido_materno_paciente"].ToString().Trim()));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(320*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(edad);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(365*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcion_tipo_paciente"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(475*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcion_admisiones"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Medico: "+(string) lector["nombre_medico_encabezado"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(280*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Diag.Admision: "+(string) lector["descripcion_diagnostico_movcargos"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Tipo Cirugia: "+(string) lector["tipo_cirugia"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(460*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Usuario: "+(string) lector["id_empleado_admision"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					if((bool) lector["cancelado"]){
						layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("C A N C E L A D O");			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(100*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Motivo :"+(string) lector ["motivo_cancelacion"].ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
					}
					contador += 1;
					while (lector.Read()){
						cr.Rectangle (05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows, 565*escala_en_linux_windows, 0*escala_en_linux_windows);
						cr.FillExtents();  //. FillPreserve(); 
						cr.SetSourceRGB (0, 0, 0);
						cr.LineWidth = 0.2;
						cr.Stroke();
						if(int.Parse((string) lector["edad"]) > 0){
							edad = (string) lector["edad"]+" Años"; 
						}else{
							edad = (string) lector["mesesedad"]+" Meses";
						}
						if(fechas_registros != (string) lector["fech_reg_adm"].ToString()){
							comienzo_linea += separacion_linea;
							cr.Rectangle (05*escala_en_linux_windows, comienzo_linea-5*escala_en_linux_windows, 565*escala_en_linux_windows, 1*escala_en_linux_windows);
							cr.FillPreserve();  //. FillPreserve(); FillExtents()
							cr.SetSourceRGB (0, 0, 0);
							cr.LineWidth = 0.5;
							cr.Stroke();
							layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
							cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("-->"+(string) lector["fech_reg_adm"].ToString()+"<--");			Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							fechas_registros = (string) lector["fech_reg_adm"].ToString();
						}
						layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["folio_de_servicio"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						cr.MoveTo(50*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["pid_paciente"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(90*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["hora_reg_adm"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(115*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(((string) lector["nombre1_paciente"].ToString().Trim()+" "+ 
				            	     																						(string) lector["nombre2_paciente"].ToString().Trim()+" "+
				        	         																						(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
				    	             																						(string) lector["apellido_materno_paciente"].ToString().Trim()));			Pango.CairoHelper.ShowLayout (cr, layout);
						
						cr.MoveTo(320*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(edad);			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(365*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcion_tipo_paciente"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(475*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcion_admisiones"].ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Medico: "+(string) lector["nombre_medico_encabezado"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(280*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Diag.Admision: "+(string) lector["descripcion_diagnostico_movcargos"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Tipo Cirugia: "+(string) lector["tipo_cirugia"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(460*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Usuario: "+(string) lector["id_empleado_admision"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						if((bool) lector["cancelado"]){
							layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
							cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("C A N C E L A D O");			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(100*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Motivo :"+(string) lector ["motivo_cancelacion"].ToString().Trim());			Pango.CairoHelper.ShowLayout (cr, layout);
							layout.FontDescription.Weight = Weight.Normal;		// Letra normal
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
						}
						contador += 1;						
					}
					comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
					layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Total de Admisiones: "+contador.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;		// Letra normal
				}else{
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
				return; 
			}		
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Console.WriteLine("entra en la impresion del encabezado");
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numerpage.ToString());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;	
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("REPORTE REGISTRO DE PACIENTES");				Pango.CairoHelper.ShowLayout (cr, layout);
			// Creando el Cuadro de Titulos para colocar el nombre del usuario
			cr.Rectangle (05*escala_en_linux_windows, 55*escala_en_linux_windows, 565*escala_en_linux_windows, 25*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(20*escala_en_linux_windows,58*escala_en_linux_windows);			layout.SetText("Fecha");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,68*escala_en_linux_windows);			layout.SetText("N° Atte.");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(55*escala_en_linux_windows,68*escala_en_linux_windows);			layout.SetText("PID");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(92*escala_en_linux_windows,68*escala_en_linux_windows);			layout.SetText("Hora");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(125*escala_en_linux_windows,68*escala_en_linux_windows);			layout.SetText("Nombre Paciente");	Pango.CairoHelper.ShowLayout (cr, layout);
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			//context.PageSetup.Orientation = PageOrientation.Landscape;
			if(comienzo_linea > 700){
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 7.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 85;
				numerpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}