using System;
using Gtk;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using Gnome;

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
		[Widget] Gtk.Entry entry_empresa = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Entry entry_aseguradora = null;
		
		//ComboBox
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		
		//CheckButtons
		[Widget] Gtk.CheckButton checkbutton_todas_fechas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_admision = null;
		[Widget] Gtk.CheckButton checkbutton_todos_paciente = null;
		[Widget] Gtk.CheckButton checkbutton_todas_empresas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_doctores = null;
		[Widget] Gtk.CheckButton checkbutton_todas_aseguradoras = null;
				
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
		[Widget] Gtk.Button button_busca_aseguradoras = null;
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
		
		string connectionString;
        string nombrebd;
	    string tipointernamiento = "CENTRO MEDICO";
   		int idtipointernamiento = 10;
   	    string tipopaciente = "Membresias"; 
		int id_tipopaciente = 100;
		int idmedico = 1;
		int idempresa = 1;
		int idaseguradora = 1;
		string motivo = "";
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		string busqueda = "";
		int 	filas = 684;
		int numpage = 1;
		int contador = 1;
		
    	string query_reporte = "";
    	string query_tipo_admision  = "AND osiris_erp_movcargos.id_tipo_admisiones = '0' ";
		string query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '200' ";
    	string query_rango_fechas = "AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') >= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "+
										"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') <= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "; 
    	/*query_rango_fechas = "AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) >= '"+DateTime.Now.ToString("yyyy")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) <= '"+DateTime.Now.ToString("yyyy")+"' "+  
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) >= '"+DateTime.Now.ToString("MM")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) <= '"+DateTime.Now.ToString("MM")+"' "+
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) >= '"+DateTime.Now.ToString("dd")+"'  AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) <= '"+DateTime.Now.ToString("dd")+"' " ;
		*/
		string query_sexo = " "; 
    	string query_empresa = " "; //"AND osiris_erp_cobros_enca.id_empresa = 3 "; 
    	string query_aseguradora = " ";
    	string query_tipo_reporte = " ";
    	string query_medico = " ";
    	string query_orden =  "ORDER BY osiris_erp_movcargos.folio_de_servicio;";
    		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		
		class_conexion conexion_a_DB = new class_conexion();
                               
		//////PARTE PRINCIPAL (MAIN) DEL PROGRAMA/////////                       	          
		public rptAdmision (string nombrebd_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			//crea la ventana de glade
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "rango_rep_adm", null);
			gxml.Autoconnect (this);
			//muestra la ventana de reportes
			rango_rep_adm.Show();
		
			query_reporte = "SELECT "+
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
				"descripcion_tipo_paciente,id_empleado_admision,osiris_erp_cobros_enca.id_aseguradora, "+
				"osiris_erp_cobros_enca.id_empresa,public.osiris_erp_cobros_enca.id_medico,osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_cobros_enca.motivo_cancelacion, "+
				"descripcion_aseguradora,numero_certificado,numero_poliza,osiris_erp_cobros_enca.numero_factura,"+
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
       		
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			// Imprime reporte
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_busca_doctores.Clicked += new EventHandler(on_button_busca_doctores_clicked);
			button_busca_empresa.Clicked += new EventHandler(on_button_busca_empresa_clicked);
			button_busca_aseguradoras.Clicked += new EventHandler(button_busca_aseguradoras_clicked);
			
			// Llenado de combobox
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
			if (store2.GetIterFirst(out iter2))
			{
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
               	while (lector.Read())
				{
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
	    	checkbutton_todas_aseguradoras.Clicked += new EventHandler(on_checkbutton_todas_aseguradoras_clicked);
	    	
	    	checkbutton_todas_aseguradoras.ChildVisible = false;
	    	button_busca_aseguradoras.ChildVisible = false;
	    	entry_aseguradora.ChildVisible = false;
	    	
		    // Desactivando Combobox en la entrada para que el usuario lo pueda elegir o activar
		    combobox_tipo_admision.Sensitive = false;
		    combobox_tipo_paciente.Sensitive = false;
		    //Activando los check buttons paa que se inicialicen activos
		    checkbutton_todos_admision.Active = true;
		    checkbutton_todos_paciente.Active = true;
		    checkbutton_todas_empresas.Active = true;
		    checkbutton_todos_doctores.Active = true;
		    
		    // Salir de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); 
		}
		///////////////////TERMINA PARTE PRINCIPAL DE PROGRAMA//////////////
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
	  		ComboBox combobox_tipo_admision = sender as ComboBox;
	  		if (sender == null) { return; }
			TreeIter iter;
			if (combobox_tipo_admision.GetActiveIter (out iter))
	   		{
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
			if (combobox_tipo_paciente.GetActiveIter (out iter))
		  	{
		   		tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
		   		id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
		   		
	   			if(id_tipopaciente == 400) {
	   				checkbutton_todas_aseguradoras.ChildVisible = true;
	   				button_busca_aseguradoras.ChildVisible = true;
			    	entry_aseguradora.ChildVisible = true;
			    	checkbutton_todas_aseguradoras.Active = false;
	   			}else{
	   				checkbutton_todas_aseguradoras.ChildVisible = false;
			    	button_busca_aseguradoras.ChildVisible = false;
			    	entry_aseguradora.ChildVisible = false;
	   			}
		   		query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
		    }
		}
		
		void button_busca_aseguradoras_clicked(object sender, EventArgs args)
		{
			busqueda = "aseguradora";
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
	        
	        // Muestra ventana de Glade
			//busca_empresas.Show();
			
			// Activa la salida de la ventana
			entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda; 
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de Medico
			button_selecciona.Clicked += new EventHandler(on_selecciona_aseguradora_clicked);
			// Llena treeview 
			button_busca_empresas.Clicked += new EventHandler(on_button_llena_aseguradoras_clicked);
			
			treeViewEngineAseguradora = new ListStore( typeof(int), typeof(string));
			lista_de_empresas.Model = treeViewEngineAseguradora;
			lista_de_empresas.RulesHint = true;
			
			lista_de_empresas.RowActivated += on_selecciona_aseguradora_clicked;  // Doble click selecciono empresa*/
			
			TreeViewColumn col_idempresa = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idempresa.Title = "ID Aseguradora"; // titulo de la cabecera de la columna, si está visible
			col_idempresa.PackStart(cellr0, true);
			col_idempresa.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
            
			TreeViewColumn col_nombrempresa = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nombrempresa.Title = "Nombre Aseguradora";
			col_nombrempresa.PackStart(cellrt1, true);
			col_nombrempresa.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			
			lista_de_empresas.AppendColumn(col_idempresa);
			lista_de_empresas.AppendColumn(col_nombrempresa);
		}
		
		void on_button_llena_aseguradoras_clicked(object sender, EventArgs args)
		{
			llenando_lista_aseguradoras();
		}
		
		void llenando_lista_aseguradoras()
		{
			treeViewEngineAseguradora.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				if ((string) entry_expresion.Text.Trim().ToUpper() == " ")
				{
					comando.CommandText = "SELECT * FROM osiris_aseguradoras ORDER BY descripcion_aseguradora;";
				}else{
					comando.CommandText = "SELECT * FROM osiris_aseguradoras "+
								"WHERE descripcion_aseguradora LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_aseguradora;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineAseguradora.AppendValues ((int) lector["id_aseguradora"],(string)lector["descripcion_aseguradora"]);
				}					
            }catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
        	conexion.Close ();
		}
		
		void on_selecciona_aseguradora_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_empresas.Selection.GetSelected(out model, out iterSelected)) {
				idaseguradora = (int) model.GetValue(iterSelected, 0);
				query_aseguradora = "AND osiris_erp_cobros_enca.id_aseguradora = '"+idaseguradora.ToString()+"' ";
				entry_aseguradora.Text = (string) model.GetValue(iterSelected, 1);
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;		win.Toplevel.Destroy();
			}
		}
		
		void on_button_busca_doctores_clicked(object sender, EventArgs args)
		{
			busqueda = "medicos";
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          (this);
	        llenado_cmbox_tipo_busqueda();
	        entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			
			treeViewEngineMedicos = new TreeStore( typeof(int),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string));
			lista_de_medicos.Model = treeViewEngineMedicos;
			lista_de_medicos.RulesHint = true;
				
			lista_de_medicos.RowActivated += on_selecciona_medico_clicked;  // Doble click selecciono paciente
				
			TreeViewColumn col_idmedico = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
			col_idmedico.PackStart(cellr0, true);
			col_idmedico.AddAttribute (cellr0, "text", 0);
			col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
            
			TreeViewColumn col_nomb1medico = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nomb1medico.Title = "1º Nombre";
			col_nomb1medico.PackStart(cellrt1, true);
			col_nomb1medico.AddAttribute (cellrt1, "text", 1);
			col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
            
            TreeViewColumn col_nomb2medico = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_nomb2medico.Title = "2º Nombre";
			col_nomb2medico.PackStart(cellrt2, true);
			col_nomb2medico.AddAttribute (cellrt2, "text", 2);
			col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
			
			TreeViewColumn col_appmedico = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_appmedico.Title = "Apellido Paterno";
			col_appmedico.PackStart(cellrt3, true);
			col_appmedico.AddAttribute (cellrt3, "text", 3);
			col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
			
			TreeViewColumn col_apmmedico = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_apmmedico.Title = "Apellido Materno";
			col_apmmedico.PackStart(cellrt4, true);
			col_apmmedico.AddAttribute (cellrt4, "text", 4);
			col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
            
			TreeViewColumn col_espemedico = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_espemedico.Title = "Especialidad";
			col_espemedico.PackStart(cellrt5, true);
			col_espemedico.AddAttribute (cellrt5, "text", 5);
			col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
                                  
			lista_de_medicos.AppendColumn(col_idmedico);
			lista_de_medicos.AppendColumn(col_nomb1medico);
			lista_de_medicos.AppendColumn(col_nomb2medico);
			lista_de_medicos.AppendColumn(col_appmedico);
			lista_de_medicos.AppendColumn(col_apmmedico);
			lista_de_medicos.AppendColumn(col_espemedico);
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,
			col_nomb1medico,
			col_nomb2medico,
			col_appmedico,
			col_apmmedico,
			col_espemedico,
		}
		
		void llenado_cmbox_tipo_busqueda()
		{
			combobox_tipo_busqueda.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_tipo_busqueda.PackStart(cell1, true);
			combobox_tipo_busqueda.AddAttribute(cell1,"text",0);
	        ListStore store1 = new ListStore( typeof (string),typeof (int));
			combobox_tipo_busqueda.Model = store1;
	        
			//store1.AppendValues ("",0);
			store1.AppendValues ("PRIMER NOMBRE",1);
			store1.AppendValues ("SEGUNDO NOMBRE",2);
			store1.AppendValues ("APELLIDO PATERNO",3);
			store1.AppendValues ("APELLIDO MATERNO",4);
			store1.AppendValues ("CEDULA MEDICA",5);
			store1.AppendValues ("ESPECIALIDAD",6);
			store1.AppendValues ("ID_MEDICO",7);
				              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_tipo_busqueda.SetActiveIter (iter1);
			}
			combobox_tipo_busqueda.Changed += new EventHandler (onComboBoxChanged_tipo_busqueda);
		}
		
		void onComboBoxChanged_tipo_busqueda (object sender, EventArgs args)
		{
	    	ComboBox combobox_tipo_busqueda = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;		
			int numbusqueda = 0;
			if (combobox_tipo_busqueda.GetActiveIter (out iter))
			{
				numbusqueda = (int) combobox_tipo_busqueda.Model.GetValue(iter,1);
				tipo_de_busqueda_de_medico(numbusqueda);
				llenando_lista_de_medicos();
			}
		}
		
		void tipo_de_busqueda_de_medico(int numbusqueda)
		{
			if(numbusqueda == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";	}//Console.WriteLine(tipobusqueda); }
			if(numbusqueda == 7)  { tipobusqueda = "AND osiris_his_medicos.id_medico LIKE '"; }//Console.WriteLine(tipobusqueda); }
		}		
		
		void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)) 
 			{
				idmedico =(int) model.GetValue(iterSelected, 0);
				query_medico = "AND osiris_his_medicos.id_medico = '"+idmedico.ToString()+"' ";
 				entry_doctor.Text = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
 							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4);
 				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_llena_medicos_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_medicos();
		}
		
		void llenando_lista_de_medicos()
		{
			TreeIter iter;
			if (combobox_tipo_busqueda.GetActiveIter(out iter))
			{
				if((int) combobox_tipo_busqueda.Model.GetValue(iter,1) > 0) {
					treeViewEngineMedicos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper().Trim() == "")
						{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,999999) AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-MM-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-MM-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,999999) AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-MM-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-MM-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
								  		"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						}
						//Console.WriteLine("query de busqueda de medicos"+comando.CommandText);
						NpgsqlDataReader lector = comando.ExecuteReader ();
						
						while (lector.Read())
						{
							treeViewEngineMedicos.AppendValues ((int) lector["id_medico"],//0
										(string) lector["nombre1_medico"],//1
										(string) lector["nombre2_medico"],//2
										(string) lector["apellido_paterno_medico"],//3
										(string) lector["apellido_materno_medico"],//4
										(string) lector["descripcion_especialidad"]//5
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
				}else{	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, " selecione un tipo de busqueda ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}
		}
		
			
		/////////////////////////////////////////////////////////777777
		void on_button_busca_empresa_clicked(object sender, EventArgs args)
		{
			busqueda = "empresa";
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
	        
	        // Muestra ventana de Glade
			//busca_empresas.Show();
			
			// Activa la salida de la ventana
			entry_expresion.KeyPressEvent += onKeyPressEvent_busqueda; 
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de Medico
			button_selecciona.Clicked += new EventHandler(on_selecciona_empresa_clicked);
			// Llena treeview 
			button_busca_empresas.Clicked += new EventHandler(on_button_llena_empresas_clicked);
			
			treeViewEngineEmpresa = new ListStore( typeof(int), typeof(string));
			lista_de_empresas.Model = treeViewEngineEmpresa;
			lista_de_empresas.RulesHint = true;
			
			lista_de_empresas.RowActivated += on_selecciona_empresa_clicked;  // Doble click selecciono empresa*/
			
			TreeViewColumn col_idempresa = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idempresa.Title = "ID Empresa"; // titulo de la cabecera de la columna, si está visible
			col_idempresa.PackStart(cellr0, true);
			col_idempresa.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
            
			TreeViewColumn col_nombrempresa = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nombrempresa.Title = "Nombre Empresa";
			col_nombrempresa.PackStart(cellrt1, true);
			col_nombrempresa.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			
			lista_de_empresas.AppendColumn(col_idempresa);
			lista_de_empresas.AppendColumn(col_nombrempresa);
		}
		
		void on_button_llena_empresas_clicked(object sender, EventArgs args)
		{
			llenando_lista_empresas();
		}
		
		void llenando_lista_empresas()
		{
			treeViewEngineEmpresa.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				if ((string) entry_expresion.Text.Trim().ToUpper() == " ")
				{
					comando.CommandText = "SELECT * FROM osiris_empresas ORDER BY descripcion_empresa;";
				}else{
					comando.CommandText = "SELECT * FROM osiris_empresas "+
								"WHERE descripcion_empresa LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_empresa;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineEmpresa.AppendValues ((int) lector["id_empresa"],(string)lector["descripcion_empresa"]);
				}					
            }catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
        	conexion.Close ();
		}
		
		void on_selecciona_empresa_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_empresas.Selection.GetSelected(out model, out iterSelected)) {
				idempresa = (int) model.GetValue(iterSelected, 0);
				query_empresa = "AND osiris_empresas.id_empresa = '"+idempresa.ToString()+"' ";
				//Console.WriteLine("seleccion "+query_empresa);
				entry_empresa.Text = (string) model.GetValue(iterSelected, 1);
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;		win.Toplevel.Destroy();
			}
		}
		
		/////////////////Acciones del boton todas las admisiones
		void on_checkbutton_todos_admision_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_admision.Active == true)
			{
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
			if (checkbutton_todos_paciente.Active == true)
			{
				query_tipo_paciente= " ";
				combobox_tipo_paciente.Sensitive = false;
			}else{
				//query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
				combobox_tipo_paciente.Sensitive = true;
			}
		}
		
		void on_checkbutton_todas_empresas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_empresas.Active == true)
			{
				button_busca_empresa.Sensitive = false;
				query_empresa = " ";			
			}else{
				//query_empresa = "AND osiris_empresas.id_empresa = '"+idempresa.ToString()+"' ";
				button_busca_empresa.Sensitive = true;
			}
		}
		
		void on_checkbutton_todos_doctores_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_doctores.Active == true)
			{
				button_busca_doctores.Sensitive = false;
				query_medico = " ";			
			}else{
				//query_medico = "AND osiris_his_medicos.id_medico = '"+idmedico.ToString()+"' ";
				button_busca_doctores.Sensitive = true;
			}
		}
		
		void on_checkbutton_todas_aseguradoras_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_aseguradoras.Active == true)
			{
				button_busca_aseguradoras.Sensitive = false;
				query_aseguradora = " ";			
			}else{
				//query_medico = "AND osiris_his_medicos.id_medico = '"+idmedico.ToString()+"' ";
				button_busca_aseguradoras.Sensitive = true;
			}
		} 
		
		/////////////////Acciones del boton todas las fechas
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_fechas.Active == true)
			{
				query_rango_fechas= " ";
				entry_dia_inicial.Sensitive = false;
				entry_mes_inicial.Sensitive = false;
				entry_ano_inicial.Sensitive = false;
				entry_dia_final.Sensitive = false;
				entry_mes_final.Sensitive = false;
				entry_ano_final.Sensitive = false;
			}else
			{	
				query_rango_fechas = "AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
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
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(busqueda == "medicos") { llenando_lista_de_medicos(); } 		
				if(busqueda == "empresa") { this.llenando_lista_empresas(); }
			}
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
			if(radiobutton_folio_servicio.Active == true) { query_orden = "ORDER BY osiris_erp_movcargos.folio_de_servicio ;" ; }
			if(radiobutton_pid_paciente.Active == true) { query_orden = "ORDER BY  osiris_erp_movcargos.pid_paciente ;"; }
			if(radiobutton_nombres.Active == true) { query_orden = "ORDER BY nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente ;"; }
			if(radiobutton_doctores.Active == true) { query_orden = "ORDER BY  nombre1_medico || ' ' || nombre2_medico || ' ' || apellido_paterno_medico || ' ' || apellido_materno_medico ;"; }
			if(radiobutton_tipo_admision.Active == true) { query_orden = "ORDER BY osiris_his_tipo_admisiones.id_tipo_admisiones ;"; }
		}
////////////////////////////////////VOID PARA IMPRESION DE PAGINA/////////////////////////////	
		void on_button_imprimir_clicked(object sender, EventArgs a)
		{		
			numpage = 1;
			filas = 684;
			contador = 1;
			tipo_de_reporte_a_mostrar(sender, a);
	    	tipo_de_sexo(sender, a);
			tipo_orden_query(sender, a);
			
			Gnome.PrintJob trabajoImpresion  = new Gnome.PrintJob();
       		Gnome.PrintDialog dialogoimpresion   = new Gnome.PrintDialog (trabajoImpresion, "REPORTE DE ADMISION", 0);
       		 int respuesta = dialogoimpresion.Run ();
          	if (respuesta == (int) Gnome.PrintButtons.Cancel) 
			{
				dialogoimpresion.Hide (); 
				dialogoimpresion.Dispose (); 
				return;
			}
		
			Gnome.PrintContext contextoImprimir = trabajoImpresion.Context;
			ComponerPagina (contextoImprimir, trabajoImpresion);
       	 	trabajoImpresion.Close (); 
        
        	switch (respuesta)
        	{
				case (int) Gnome.PrintButtons.Print:   
					trabajoImpresion.Print (); 
         	   		break;
         	    case (int) Gnome.PrintButtons.Preview:
         	     	Gnome.PrintJobPreview vistaprevia = new Gnome.PrintJobPreview(trabajoImpresion, "REPORTE DE ADMISION");
         	       	vistaprevia.Show();
         	    break;
        	}

			dialogoimpresion.Hide (); dialogoimpresion.Dispose ();
       	 
		}   	
    	
////////////////////////////////////VOID PARA FORMATO DE PAGINA/////////////////////////////
    	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Cambiar la fuente
			//Console.WriteLine("imprimo encabezado");
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.5, 770);		    			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);		    			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.5, 760);		    			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(20, 760);		    			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(19.5, 750);		    			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);		    			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			//ContextoImp.MoveTo(464.5, 760);		   			ContextoImp.Show("Fecha de reporte: "+DateTime.Now.ToString("dd-MM-yyyy"));
			//ContextoImp.MoveTo(465, 760);		    			ContextoImp.Show("Fecha de reporte: "+DateTime.Now.ToString("dd-MM-yyyy"));
			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(189.5, 740);						ContextoImp.Show("REPORTE DE ADMISIONES Y REGISTRO ");
			ContextoImp.MoveTo(190, 740);						ContextoImp.Show("REPORTE DE ADMISIONES Y REGISTRO ");	

    		Gnome.Print.Setfont (ContextoImp, fuente9);
    		ContextoImp.MoveTo(59.5, 710);						ContextoImp.Show("PID");
    		ContextoImp.MoveTo(60, 710);						ContextoImp.Show("PID");
			
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.5, 720);						ContextoImp.Show("FOLIO");
			ContextoImp.MoveTo(20, 720);						ContextoImp.Show("FOLIO");
			ContextoImp.MoveTo(19.5, 710);						ContextoImp.Show("SERVICIO");
			ContextoImp.MoveTo(20, 710);						ContextoImp.Show("SERVICIO");
   		
   			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(99.5, 720);		    			ContextoImp.Show("FECHA");
			ContextoImp.MoveTo(100, 720);		    			ContextoImp.Show("FECHA");
			ContextoImp.MoveTo(99.5, 710);		    			ContextoImp.Show("ADMISION");
			ContextoImp.MoveTo(100, 710);		    			ContextoImp.Show("ADMISION");
			
			Gnome.Print.Setfont (ContextoImp, fuente9);
			ContextoImp.MoveTo(154.5,710);		    			ContextoImp.Show("NOMBRE DEL PACIENTE");
			ContextoImp.MoveTo(155,710);		    			ContextoImp.Show("NOMBRE DEL PACIENTE");
			ContextoImp.MoveTo(319.5, 710);						ContextoImp.Show("TIPO PACIENTE");
			ContextoImp.MoveTo(320, 710);						ContextoImp.Show("TIPO PACIENTE");
	
			ContextoImp.MoveTo(464.5, 710);						ContextoImp.Show("TIPOS DE ADMISIONES");
			ContextoImp.MoveTo(465, 710);						ContextoImp.Show("TIPOS DE ADMISIONES");
			
			Gnome.Print.Setfont (ContextoImp, fuente7);
			ContextoImp.MoveTo(230.7, 50);						ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha de reporte "+DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
			ContextoImp.MoveTo(230, 50);						ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha de reporte "+DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
		}
    	
    	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			if (contador > 62)
			{
				numpage +=1;
				//Console.WriteLine("pagina "+numpage.ToString());
				ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
				filas = 684;
				contador = 1;
			}
		}
    	
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	    
        	// Verifica que la base de datos este conectada
        	try
        	{
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
					query_rango_fechas = "AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				}
			 	
				comando.CommandText = query_reporte + query_tipo_admision + query_tipo_paciente + query_sexo + 
									query_empresa + query_aseguradora + query_tipo_reporte + query_medico +
									query_rango_fechas + query_orden;
				Console.WriteLine(comando.CommandText.ToString());
 				NpgsqlDataReader lector = comando.ExecuteReader ();
		
				ContextoImp.BeginPage("Pagina1");
				int total = 0;
        		string edad = "";
        		//int columnas=20;
        		
				while (lector.Read())
				{
					//Console.WriteLine("contador inicial "+contador.ToString());
					if(contador == 1 ) { imprime_encabezado(ContextoImp, trabajoImpresion); }
					
					Gnome.Print.Setfont (ContextoImp, fuente7);
			
					int pidpaciente = (int) lector["pid_paciente"];//se transforma el pid de integer (numerico) a string (cadena de caracteres) para poder ser leido
					int folioregist = (int) lector["folio_de_servicio"];
					string nom_paciente = (string) lector["nombre1_paciente"]+" "+ 
				            	     (string) lector["nombre2_paciente"]+" "+
				        	         (string) lector["apellido_paterno_paciente"]+" "+
				    	             (string) lector["apellido_materno_paciente"];
					
					if(int.Parse((string) lector["edad"]) > 0) { edad = (string) lector["edad"]+" Años "; 
					} else { edad = (string) lector["mesesedad"]+" Meses ";
					}   
					
					ContextoImp.MoveTo(19.5, filas+8);
					ContextoImp.Show("______________________________________________________________________________________________________________________________________________");
					
					ContextoImp.MoveTo(19.5, filas);					ContextoImp.Show(folioregist.ToString());
					ContextoImp.MoveTo(20, filas);						ContextoImp.Show(folioregist.ToString());
					ContextoImp.MoveTo(60, filas);						ContextoImp.Show(pidpaciente.ToString());
					ContextoImp.MoveTo(100, filas);			        	ContextoImp.Show((string) lector["fech_reg_adm"]);
		        	ContextoImp.MoveTo(150,filas);		    	    	ContextoImp.Show(nom_paciente);
				    ContextoImp.MoveTo(320, filas);         			ContextoImp.Show((string) lector["descripcion_tipo_paciente"]);
					ContextoImp.MoveTo(465, filas);						ContextoImp.Show((string) lector["descripcion_admisiones"]);
					filas -= 10;
					contador += 1;
					//Console.WriteLine("primera linea "+contador.ToString());
					salto_pagina(ContextoImp, trabajoImpresion);
					
					////////SEGUNDA FILA DE DATOS///////////////////////////
					if((int) lector["id_aseguradora"] > 1)
					{
						if((int) lector["id_empresa"] > 1)
						{
							string segundalinea = "Usuario: "+(string) lector["id_empleado_admision"]+"   Hora: "+(string) lector["hora_reg_adm"]+"   Edad: "+edad+"   Aseguradora: "+(string) lector["descripcion_aseguradora"]+"   Empresa: "+(string) lector["descripcion_empresa"];
							if(segundalinea.Length > 350) { segundalinea = segundalinea.Substring(0,350); }
							ContextoImp.MoveTo(20, filas);		ContextoImp.Show(segundalinea);
						}else{
							ContextoImp.MoveTo(20, filas);
							ContextoImp.Show("Usuario: "+(string) lector["id_empleado_admision"]+"   Hora: "+(string) lector["hora_reg_adm"]+"   Edad: "+edad+"   Aseguradora: "+(string) lector["descripcion_aseguradora"] );	
						}
					}else{
						if((int) lector["id_empresa"] > 1){
							ContextoImp.MoveTo(20, filas);
							ContextoImp.Show("Usuario: "+(string) lector["id_empleado_admision"]+"   Hora: "+(string) lector["hora_reg_adm"]+"   Edad: "+edad+"   Empresa: "+(string) lector["descripcion_empresa"]);
						}else{
							ContextoImp.MoveTo(20, filas);
							ContextoImp.Show("Usuario: "+(string) lector["id_empleado_admision"]+"   Hora: "+(string) lector["hora_reg_adm"]+"   Edad: "+edad);
						}
						//if((int) lector["id_tipo_paciente"
					}
					filas -= 10;
					contador += 1;
					//Console.WriteLine("segunda linea "+contador.ToString());
					salto_pagina(ContextoImp, trabajoImpresion);
					
					
					//////////TERCERA FILA (OPCIONAL)//////////////////////
					if((bool) lector["cancelado"])
					{
						//filas -= 10;
						//contador += 1;
						//salto_pagina(ContextoImp, trabajoImpresion);
						motivo = (string) lector ["motivo_cancelacion"];
						ContextoImp.MoveTo(19.5, filas);				ContextoImp.Show("CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO");
						ContextoImp.MoveTo(20, filas);					ContextoImp.Show("CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO CANCELADO");
						//Console.WriteLine(folioregist.ToString()+" motivo: "+motivo.ToString());
						filas -= 10;
						contador += 1;
						//Console.WriteLine("primera linea cancelado "+contador.ToString());
						salto_pagina(ContextoImp, trabajoImpresion);
						if(motivo.Length > 300) { motivo = motivo.Substring(0,300); }
						ContextoImp.MoveTo(20, filas);		ContextoImp.Show("Motivo cancelacion:  " +motivo.ToString());
						contador += 1;
						filas -= 10;
						//Console.WriteLine("segunda linea cancelado "+contador.ToString());
						salto_pagina(ContextoImp, trabajoImpresion);
					}else{
						if((int) lector["id_tipo_admisiones"] == 100 || (int) lector["id_tipo_admisiones"] == 500 || (int) lector["id_tipo_admisiones"] == 600
							|| (int) lector["id_tipo_admisiones"] == 810 || (int) lector["id_tipo_admisiones"] == 820 || (int) lector["id_tipo_admisiones"] == 830
							|| (int) lector["id_tipo_admisiones"] == 700 || (int) lector["id_tipo_admisiones"] == 710 || (int) lector["id_tipo_admisiones"] == 930)
						{
							
							//filas -= 10;
							//contador += 1;
							//salto_pagina(ContextoImp, trabajoImpresion);
							ContextoImp.MoveTo(20, filas);
																
							ContextoImp.Show("Medico: "+(string) lector["nombre_medico_tratante"]+"   Diag. Admision: "+(string) lector["descripcion_diagnostico_movcargos"]);
								
							filas -= 10;
							contador += 1;
							salto_pagina(ContextoImp, trabajoImpresion);
							ContextoImp.MoveTo(20, filas);				ContextoImp.Show("Tipo Cirugia: "+(string) lector["tipo_cirugia"] + " Cirugia:"+(string) lector["descripcion_cirugia"]);
							filas -= 10;
							contador += 1;
							salto_pagina(ContextoImp, trabajoImpresion);							
						}else{
							//contador += 1;
							//salto_pagina(ContextoImp, trabajoImpresion);
							//filas -= 10;
						}
					}					
					
					////////FILA DE DATOS EXTRAS/////////////////
					if((int) pidpaciente == 255 || (int) pidpaciente == 605)     //MUNICIPIO SAN NICOLAS DE LOS GARZA NUEVO LEON
					{
						ContextoImp.MoveTo(20, filas);
						ContextoImp.Show("Datos de trabajador: "+(string) lector["responsable_cuenta"]);
						filas -= 10;
						contador += 1;
						salto_pagina(ContextoImp, trabajoImpresion);
					}
					
					total +=1;
					
					//if (contador >= 62)
					//{
					//	ContextoImp.ShowPage();
					//	ContextoImp.BeginPage("Pagina N");
					//	filas = 684;
					//	contador = 1;
					//	salto_pagina(ContextoImp, trabajoImpresion);
					//}
					//Console.WriteLine("folio de registro "+folioregist.ToString());
				}	
				if(total == 0){
					ContextoImp.MoveTo(190.5, filas);
					ContextoImp.Show("NO EXISTEN ADMISIONES DE ESE TIPO");
					ContextoImp.MoveTo(191, filas);
					ContextoImp.Show("NO EXISTEN ADMISIONES DE ESE TIPO");
					filas-=10;
					ContextoImp.MoveTo(220.5, filas);
					ContextoImp.Show("Total de Admisiones: "+(string) total.ToString());
					ContextoImp.MoveTo(221, filas);
					ContextoImp.Show("Total de Admisiones: "+(string) total.ToString());
				}else{
					filas -= 10;
					ContextoImp.MoveTo(19.5, filas);
					ContextoImp.Show("Total de Admisiones: "+(string) total.ToString());
					ContextoImp.MoveTo(20, filas);
					ContextoImp.Show("Total de Admisiones: "+(string) total.ToString());
				}
        		lector.Close (); 
				conexion.Close ();
			
				//ContextoImp.SetLineWidth(10);
				ContextoImp.ShowPage();
			
			}
			catch (NpgsqlException ex)
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
				return; 
			}
		}
	}
}