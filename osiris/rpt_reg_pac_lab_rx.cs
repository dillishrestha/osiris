// created on 11/02/2008 at 04:56 p
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Pre-Programacion, Modificaciones y Ajustes)
//				  Tec. Homero Montoya (Programacion) homerokda@hotmail.com
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
//////////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;
using Glade;

namespace osiris
{
	public class rep_reg_pac_labo_rx
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window busqueda_por_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		[Widget] Gtk.Button button_salir;
		
		// Entradas y botones de la ventana
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Label label178;
		[Widget] Gtk.Label label181;
		//[Widget] Gtk.Entry entry_aseguradora;
		
		//ComboBox
		[Widget] Gtk.ComboBox combobox_tipo_admision;
		[Widget] Gtk.ComboBox combobox_tipo_paciente;
		
		//CheckButtons
		[Widget] Gtk.CheckButton checkbutton_todas_fechas;
		[Widget] Gtk.CheckButton checkbutton_todos_admision;
		[Widget] Gtk.CheckButton checkbutton_todos_paciente;
		[Widget] Gtk.CheckButton checkbutton_todos_doctores;
		
		//radio buttons
		[Widget] Gtk.RadioButton radiobutton_masculino;
		[Widget] Gtk.RadioButton radiobutton_femenino;
		[Widget] Gtk.RadioButton radiobutton_ambos_sexos;
		[Widget] Gtk.RadioButton radiobutton_folio_servicio;
		[Widget] Gtk.RadioButton radiobutton_pid_paciente;
		[Widget] Gtk.RadioButton radiobutton_nombres;
		[Widget] Gtk.RadioButton radiobutton_tipo_admision;
		[Widget] Gtk.RadioButton radiobutton_folio_lab;
		
		//botones
		[Widget] Gtk.Button button_busca_doctores;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		//ventana de busqueda de medicos
		[Widget] Gtk.TreeView lista_de_medicos;
		
		string connectionString;
        string nombrebd;
		string tiporeporte = "REGLAB";
		string titulo = "REPORTE DE REGISTRO DE PACIENTES DE LABORATORIO";
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		private static double headerHeight = (10*72/25.4);
		private static double headerGap = (3*72/25.4);
		
		PrintContext context;
		
		string query_fechas = "";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
				
		string tipointernamiento = "CENTRO MEDICO";
   		int idtipointernamiento = 10;
   	    string tipopaciente = "Todos"; 
		string tipoadmision = "Todos";
		int id_tipoadmision = 0;
		int id_tipopaciente = 0;
		string busqueda = "";
		string query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente > '0' ";
		
		string query_sexo = " "; 
    	string query_tipo_reporte = " ";
    	string query_orden =  " ";
		string query_solicitado;
		string querytipo_reporte = "";
		string nombredepartamento = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rep_reg_pac_labo_rx(string _nombrebd_,string querytipo_reporte_,string nombredepartamento_)
		{
			querytipo_reporte = querytipo_reporte_;
			nombredepartamento = nombredepartamento_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			Glade.XML  gxml = new Glade.XML  (null, "laboratorio.glade", "busqueda_por_fecha", null);
			gxml.Autoconnect  (this);	
			busqueda_por_fecha.Show();
			llenado_cbox_tipo_paciente();
			llenado_combobox_tipo_paciente();
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			radiobutton_ambos_sexos.Active = true;
	    	radiobutton_folio_servicio.Active = true;
	    	titulo = "REPORTE DE REGISTRO DE PACIENTES DE "+nombredepartamento;
	    	combobox_tipo_paciente.Sensitive = false;
			this.combobox_tipo_admision.Sensitive = false;
		    checkbutton_todos_paciente.Active = true;
		   	checkbutton_todos_admision.Active = true;
		    //this.combobox_tipo_admision.Hide();
		    //this.label178.Hide();
		    this.label181.Hide();
		    this.checkbutton_todos_doctores.Hide();
		    this.entry_doctor.Hide();
		    this.button_busca_doctores.Hide();
	    	checkbutton_todos_paciente.Clicked += new EventHandler(on_checkbutton_todos_paciente_clicked);
	    	checkbutton_todas_fechas.Clicked += new EventHandler(on_checkbutton_todas_fechas_clicked);
			checkbutton_todos_admision.Clicked += new EventHandler(on_checkbutton_todos_admision_clicked);
			
	    	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void llenado_combobox_tipo_paciente()
		{
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
               						"ORDER BY descripcion_admisiones;";
				
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
			if (store2.GetIterFirst(out iter2))
			{
				//Console.WriteLine(iter2);
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
		}
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
    		ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
	  			tipoadmision = (string) combobox_tipo_admision.Model.GetValue(iter,0);
		   		id_tipoadmision = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		   		query_solicitado = "AND osiris_his_tipo_admisiones.id_tipo_admisiones = '"+id_tipoadmision.ToString()+"' ";
	    	}
	    }
	    
		void llenado_cbox_tipo_paciente()
		{
			// Tipos de Paciente
		    combobox_tipo_paciente.Clear();
		    CellRendererText cell1 = new CellRendererText();
		    combobox_tipo_paciente.PackStart(cell1, true);
		    combobox_tipo_paciente.AddAttribute(cell1,"text",0);
	        
		    ListStore store1 = new ListStore( typeof (string),typeof (int));
		    combobox_tipo_paciente.Model = store1;	        // lleno de la tabla de his_tipo_de_pacientes
		    
			store1.AppendValues (" ", 0);
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd); 
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_pacientes "+
               							" ORDER BY id_tipo_paciente;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
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
	    }
	    
	    ///////// Activa desactiva combobox de tipo Paciente
		void onComboBoxChanged_tipopaciente(object sender, EventArgs args)
		{
	 		ComboBox combobox_tipo_paciente = sender as ComboBox;
	   		if (sender == null)
	   		{
	   			return;
	   		}
			TreeIter iter;
			
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
		   		id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
		   		query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
		   	}
		}
		
		void imprime_reporte(object sender, EventArgs a)
		{	
			if (id_tipopaciente == 0 && this.checkbutton_todos_paciente.Active == false){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"Seleccione un Tipo de Paciente");
																			
				msgBoxError.Run ();				msgBoxError.Destroy();
			}else{
				tipo_de_sexo(sender, a);
				tipo_orden_query(sender, a);
				print = new PrintOperation ();
				print.JobName = "Reporte de ";
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
        	}
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			if(this.checkbutton_todas_fechas.Active == false){
				rango1 = entry_dia1.Text+"/"+entry_mes1.Text+"/"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"/"+entry_mes2.Text+"/"+entry_ano2.Text;
				query_fechas = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
							"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
			}
			
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	// asigna el numero de folio de ingreso de paciente (FOLIO)
					comando.CommandText = "SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_cobros_deta.folio_de_servicio,'999999999999') AS folioservicio, "+
						"to_char(osiris_erp_cobros_deta.pid_paciente,'999999999999') AS pidpaciente, "+
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
						"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
						"osiris_his_paciente.sexo_paciente,"+
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente,'dd-MM-yyyy') AS fecha_nac, "+
						"osiris_his_tipo_admisiones.descripcion_admisiones, "+
						"osiris_productos.descripcion_producto, "+
						"osiris_erp_cobros_enca.nombre_medico_tratante, "+
						"osiris_erp_cobros_enca.id_medico, "+
						"osiris_erp_cobros_deta.pid_paciente, "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-MM-yyyy') AS fecha_crea, "+
						"osiris_his_tipo_pacientes.id_tipo_paciente, "+
						"to_char(osiris_erp_cobros_deta.folio_interno_dep,'999999999999') AS foliointernodep,"+ 
						"to_char(osiris_erp_cobros_deta.fechahora_solicitud,'dd-MM-yyyy HH24:mi:ss') AS fechahorasolicitud,"+
						"costo_por_unidad,precio_producto_publico,porcentage_ganancia "+
						"FROM osiris_productos,osiris_erp_movcargos,osiris_erp_cobros_enca,osiris_his_medicos,osiris_his_tipo_admisiones,osiris_his_paciente,osiris_grupo_producto,osiris_grupo1_producto,osiris_his_tipo_pacientes,osiris_grupo2_producto,osiris_erp_cobros_deta "+//  
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_erp_cobros_deta.id_producto =  osiris_productos.id_producto "+
						"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
						"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
						"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_cobros_deta.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
						"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
						"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
						"AND osiris_erp_cobros_deta.id_tipo_admisiones2 = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.cantidad_aplicada > 0 "+
						" "+query_fechas+" "+
						" "+query_sexo+" "+
						" "+query_tipo_reporte+" "+
						" "+query_solicitado+" "+
						querytipo_reporte+
						//"AND osiris_erp_cobros_deta.id_tipo_admisiones = '400' "+
						"ORDER BY "+query_orden+" ;";
				Console.WriteLine(comando.CommandText.ToString());			
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if (lector.Read()){
					imprime_encabezado(cr,layout);
					
					while(lector.Read()){
						
					}					
				}
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
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
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("ESTUDIOS_CARGADOS_A_PACIENTES");	Pango.CairoHelper.ShowLayout (cr, layout);
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
		
		void tipo_de_sexo(object sender, EventArgs args)
		{
			if(radiobutton_ambos_sexos.Active == true){ 
				query_sexo = " ";
			}
			if(radiobutton_femenino.Active == true){ 
				query_sexo = "AND osiris_his_paciente.sexo_paciente = 'M' ";
			}
			if(radiobutton_masculino.Active == true){ 
				query_sexo = "AND osiris_his_paciente.sexo_paciente = 'H' "; 
			}
		}
		
		void tipo_orden_query(object sender, EventArgs args)
		{
			if(radiobutton_folio_servicio.Active == true){
				query_orden = "osiris_erp_movcargos.folio_de_servicio ;" ; }
			if(radiobutton_pid_paciente.Active == true){
				query_orden = "osiris_erp_cobros_deta.pid_paciente ;"; }
			if(radiobutton_nombres.Active == true){
				query_orden = "nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente ;"; }
			if(radiobutton_tipo_admision.Active == true){
				query_orden = "id_tipo_paciente ;"; }
			if(radiobutton_folio_lab.Active == true){
				query_orden = "to_char(osiris_erp_cobros_deta.folio_interno_dep,'999999999999') ;";
			}
		}
				
		/////////////////Acciones del boton todos los tipos de pacientes
		void on_checkbutton_todos_paciente_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_paciente.Active == true){
				query_tipo_paciente= " ";
				combobox_tipo_paciente.Sensitive = false;
				tipopaciente = "Todos";
				id_tipopaciente = 0;
			}else{
				combobox_tipo_paciente.Sensitive = true;
			}
		    Console.WriteLine ("entro combox de pacientes");
		}
		
		void on_checkbutton_todos_admision_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_admision.Active == true){
				query_solicitado= " ";
				combobox_tipo_admision.Sensitive = false;
				tipoadmision = "Todos";
				id_tipoadmision = 0;
			}else{
				combobox_tipo_admision.Sensitive = true;
			}
            Console.WriteLine ("entro combox de adm");
		}
		
		
		/////////////////Acciones del boton todas las fechascombobox_tipo_admision
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_fechas.Active == true){
				query_fechas = "";
				entry_dia1.Sensitive = false;
				entry_mes1.Sensitive = false;
				entry_ano1.Sensitive = false;
				entry_dia2.Sensitive = false;
				entry_mes2.Sensitive = false;
				entry_ano2.Sensitive = false;
			}else{
				entry_dia1.Sensitive = true;
				entry_mes1.Sensitive = true;
				entry_ano1.Sensitive = true;
				entry_dia2.Sensitive = true;
				entry_mes2.Sensitive = true;
				entry_ano2.Sensitive = true;
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}



/*
					ContextoImp.MoveTo(70,fila);					ContextoImp.Show(contador_pacientes.ToString().Trim());					
					
					compara = (string)lector ["folioservicio"];
					
					// imprime encabezado y detalle del examen
					
					tomovalor1 = (string)lector ["folioservicio"];
					ContextoImp.MoveTo(140, fila);					ContextoImp.Show(tomovalor1.Trim());
					
					
					tomovalor1 = (string)lector ["pidpaciente"];
					ContextoImp.MoveTo(178, fila);					ContextoImp.Show(tomovalor1.Trim());
					
										
					ContextoImp.MoveTo(500,fila);			ContextoImp.Show((string)lector ["edad"]);
					ContextoImp.MoveTo(465,fila);			ContextoImp.Show((string)lector ["sexo_paciente"]);
					ContextoImp.MoveTo(530,fila);			ContextoImp.Show((string)lector ["fecha_nac"]);
					
					tomovalor1 = (string) lector["nombre_completo"];
					if(tomovalor1.Length > 37){
						tomovalor1 = tomovalor1.Substring(0,37); 
					}
					ContextoImp.MoveTo(217,fila);			ContextoImp.Show(tomovalor1);
					tomovalor1 = (string) lector["nombre_medico_tratante"];
					if(tomovalor1.Length > 18){
						tomovalor1 = tomovalor1.Substring(0,18); 
					}
					ContextoImp.MoveTo(593,fila);					ContextoImp.Show(tomovalor1);
					
					// segunda linea
					 fila-=10;
					 contador += 1;
					contadorprocedimientos += 1;
					salto_pagina(ContextoImp,trabajoImpresion);					
					
					ContextoImp.MoveTo(120,fila);					ContextoImp.Show(contador_examenes.ToString().Trim());
					
					tomovalor1 = (string)lector ["foliointernodep"];
					ContextoImp.MoveTo(140,fila);					ContextoImp.Show(tomovalor1.Trim());
					
					ContextoImp.MoveTo(178,fila);			ContextoImp.Show((string)lector ["fecha_crea"]);
					ContextoImp.MoveTo(243,fila);			ContextoImp.Show((string)lector ["fechahorasolicitud"]);
					tomovalor1 = (string) lector["descripcion_producto"];
					if(tomovalor1.Length > 40)
					{
						tomovalor1 = tomovalor1.Substring(0,40); 
					}
					ContextoImp.MoveTo(355, fila);			ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) lector["descripcion_admisiones"];
					if(tomovalor1.Length > 12)
					{
						tomovalor1 = tomovalor1.Substring(0,12); 
					}
					ContextoImp.MoveTo(506,fila);			ContextoImp.Show(tomovalor1);
				
				 		
					while(lector.Read()){
						
						if ((string) lector ["folioservicio"] == compara){
							
							// imprime solo el detalle
							fila-=10;
	   					    tomovalor1 = (string) lector["descripcion_producto"];
	   					    if(tomovalor1.Length > 40){
								tomovalor1 = tomovalor1.Substring(0,40); 
							}
							ContextoImp.MoveTo(355, fila);			ContextoImp.Show(tomovalor1);
							
							tomovalor1 = (string) lector["descripcion_admisiones"];
							
							if(tomovalor1.Length > 12)
							{
								tomovalor1 = tomovalor1.Substring(0,12); 
							}
							ContextoImp.MoveTo(506,fila);			ContextoImp.Show(tomovalor1);
							contador_examenes += 1;						
							contador += 1;
							contadorprocedimientos += 1;
							salto_pagina(ContextoImp,trabajoImpresion);
							ContextoImp.MoveTo(120,fila);					ContextoImp.Show(contador_examenes.ToString().Trim());
							
							tomovalor1 = (string)lector ["foliointernodep"];
							ContextoImp.MoveTo(140,fila);					ContextoImp.Show(tomovalor1.Trim());
							
							ContextoImp.MoveTo(178,fila);			ContextoImp.Show((string)lector ["fecha_crea"]);
							ContextoImp.MoveTo(243,fila);			ContextoImp.Show((string)lector ["fechahorasolicitud"]);
							
						}else{
							contador_examenes = 1;
							contador_pacientes += 1;
							fila -= 10;
							ContextoImp.MoveTo(70,fila);					ContextoImp.Show(contador_pacientes.ToString().Trim());
							compara = (string)lector ["folioservicio"];						
							ContextoImp.MoveTo(140, fila);					ContextoImp.Show(compara.Trim());
							tomovalor1 = (string)lector ["pidpaciente"];
							ContextoImp.MoveTo(178, fila);					ContextoImp.Show(tomovalor1.Trim());
													
							ContextoImp.MoveTo(500,fila);			ContextoImp.Show((string)lector ["edad"]);
							ContextoImp.MoveTo(465,fila);			ContextoImp.Show((string)lector ["sexo_paciente"]);
							ContextoImp.MoveTo(530,fila);			ContextoImp.Show((string)lector ["fecha_nac"]);
							tomovalor1 = (string) lector["nombre_completo"];
							
							if(tomovalor1.Length > 37){
								tomovalor1 = tomovalor1.Substring(0,37); 
							}
							ContextoImp.MoveTo(217,fila);			ContextoImp.Show(tomovalor1);
							
							tomovalor1 = (string) lector["nombre_medico_tratante"];
							if(tomovalor1.Length > 18){
							tomovalor1 = tomovalor1.Substring(0,18); 
							}
							ContextoImp.MoveTo(593,fila);					ContextoImp.Show(tomovalor1);
							// segunda linea
							contador+=1;
							contadorprocedimientos += 1;
							salto_pagina(ContextoImp,trabajoImpresion);
							fila-=10;
							
							ContextoImp.MoveTo(120,fila);					ContextoImp.Show(contador_examenes.ToString().Trim());
							tomovalor1 = (string)lector ["foliointernodep"];
							ContextoImp.MoveTo(140,fila);					ContextoImp.Show(tomovalor1.Trim());
							
							ContextoImp.MoveTo(178,fila);			ContextoImp.Show((string)lector ["fecha_crea"]);
							ContextoImp.MoveTo(243,fila);			ContextoImp.Show((string)lector ["fechahorasolicitud"]);	
							
							tomovalor1 = (string) lector["descripcion_producto"];
							if(tomovalor1.Length > 40){
								tomovalor1 = tomovalor1.Substring(0,40); 
							}
							ContextoImp.MoveTo(355, fila);			ContextoImp.Show(tomovalor1);
							
							tomovalor1 = (string) lector["descripcion_admisiones"];
							if(tomovalor1.Length > 12)
							{
								tomovalor1 = tomovalor1.Substring(0,12); 
							}
							ContextoImp.MoveTo(506,fila);			ContextoImp.Show(tomovalor1);
							contador+=1;
							contadorprocedimientos += 1;
							salto_pagina(ContextoImp,trabajoImpresion);
							
							// imprime encabezado y detalle del examen
								
						}
					}
					ContextoImp.MoveTo(710,fila);				ContextoImp.Show("TOTAL DE ESTUDIOS:  "+contadorprocedimientos.ToString());
					ContextoImp.MoveTo(710,fila);				ContextoImp.Show("TOTAL DE ESTUDIOS:  "+contadorprocedimientos.ToString());
						
					salto_pagina(ContextoImp,trabajoImpresion);
					*/