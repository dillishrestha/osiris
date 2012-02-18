// created on 07/11/2011
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class pases_a_quirofano
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		PrintContext context;
		
		// Declarando variable publicas
		string connectionString;
		string nombrebd;
		
		// variable publicas
		int pidpaciente;
		int folioservicio;
		int idcentro_costo;
		string LoginEmpleado;
		int idtipopaciente;
		int idempresa_paciente;
		int idaseguradora_paciente;		
		string diagnostico_movcargo = "";
		string nombrecirugia_movcargo = "";
		string descripciontipopaciente = "";
		string tipo_pase = "";
		string query_slq = "";
		
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//[Widget] Gtk.Entry entry_fecha_inicio;
		[Widget] Gtk.Window envio_almacenes = null;
		[Widget] Gtk.Entry entry_dia_inicio = null;
		[Widget] Gtk.Entry entry_mes_inicio = null;
		[Widget] Gtk.Entry entry_ano_inicio = null;
		[Widget] Gtk.Entry entry_dia_termino = null;
		[Widget] Gtk.Entry entry_mes_termino = null;
		[Widget] Gtk.Entry entry_ano_termino = null;
		[Widget] Gtk.HBox hbox1 = null;
		[Widget] Gtk.HBox hbox2 = null;
		[Widget] Gtk.Label label262;
		[Widget] Gtk.CheckButton checkbutton_todos_envios = null;
		[Widget] Gtk.CheckButton checkbutton_seleccion_presupuestos = null;
		[Widget] Gtk.Button button_rep = null;
		[Widget] Gtk.TreeView lista_almacenes = null;
		
		TreeStore treeViewEnginePases;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		public pases_a_quirofano (int pidpaciente_,int folioservicio_,int idcentro_costo_,string LoginEmpleado_,
		                          int idtipopaciente_,int idempresa_paciente_,int idaseguradora_paciente_,
									bool altamedicapaciente,string tipo_pase_)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			pidpaciente = pidpaciente_;
			folioservicio = folioservicio_;
			idcentro_costo = idcentro_costo_;
			LoginEmpleado = LoginEmpleado_;
			idtipopaciente = idtipopaciente_;
			idempresa_paciente = idempresa_paciente_;
			idaseguradora_paciente = idaseguradora_paciente_;
			tipo_pase = tipo_pase_;
			
			if(tipo_pase == "pase_qx_urg"){
				Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "envio_almacenes", null);
				gxml.Autoconnect (this);
				
				entry_dia_inicio.Text = DateTime.Now.ToString("dd");
				entry_mes_inicio.Text = DateTime.Now.ToString("MM");
				entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");			
				entry_dia_termino.Sensitive = false;
				entry_mes_termino.Sensitive = false;
				entry_ano_termino.Sensitive = false;
				entry_dia_inicio.IsEditable = false;
				entry_mes_inicio.IsEditable = false;
				entry_ano_inicio.IsEditable = false;				
				hbox1.Hide();
				hbox2.Hide();
				checkbutton_seleccion_presupuestos.Hide();
				checkbutton_todos_envios.Label = "Nuevo Pase para QX./Urgencias";
				label262.Text = "Cancelar";
				envio_almacenes.Show();
				
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				//button_buscar.Clicked += new EventHandler(on_buscar_clicked);
	           	button_rep.Clicked += new EventHandler(on_printing_pase_qx_clcked);
	          	checkbutton_todos_envios.Clicked += new EventHandler(on_create_pases_qxurg_clicked);
	          	//checkbutton_seleccion_presupuestos.Hide();
				crea_treeview_pases();
				llenado_treeview_pases();
			}
			if(tipo_pase == "pase_de_ingreso"){
				printing_pase();
			}
		}
		
		void crea_treeview_pases()
		{
			treeViewEnginePases = new TreeStore(typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string));
						
				lista_almacenes.Model = treeViewEnginePases;
				lista_almacenes.RulesHint = true;
				//lista_almacenes.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
					
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "N° Pase";
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_fechahora = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_fechahora.Title = "Fech. Hora";
				col_fechahora.PackStart(cellr1, true);
				col_fechahora.AddAttribute (cellr1, "text", 1);
				col_fechahora.SortColumnId = (int) Column_prod.col_fechahora;
				
				TreeViewColumn col_idquiencreo = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_idquiencreo.Title = "Quien creo";
				col_idquiencreo.PackStart(cellr2, true);
				col_idquiencreo.AddAttribute (cellr2, "text", 2);
				col_idquiencreo.SortColumnId = (int) Column_prod.col_idquiencreo;
			
				TreeViewColumn col_departamento = new TreeViewColumn();
				CellRendererText cellr3 = new CellRendererText();
				col_departamento.Title = "Departamento";
				col_departamento.PackStart(cellr3, true);
				col_departamento.AddAttribute (cellr3, "text", 3);
				col_departamento.SortColumnId = (int) Column_prod.col_departamento;
			
				lista_almacenes.AppendColumn(col_idproducto);  // 0
				lista_almacenes.AppendColumn(col_fechahora);  // 1
				lista_almacenes.AppendColumn(col_idquiencreo);  // 2
				lista_almacenes.AppendColumn(col_departamento);  // 3
		}
		
		//  lista_de_productos:
		enum Column_prod
		{
			col_idproducto,
			col_fechahora,
			col_idquiencreo,
			col_departamento
		}
		
		void llenado_treeview_pases()
		{
			treeViewEnginePases.Clear();
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand();
				comando.CommandText = query_slq + "AND osiris_erp_pases_qxurg.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";		
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEnginePases.AppendValues(lector["id_secuencia"].ToString().Trim(),
					                                 lector["fechahoracrea"].ToString().Trim(),
					                                 lector["id_quien_creo"],
					                                 lector["descripcion_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();				
			}
			conexion.Close();			
		}
		
		void on_create_pases_qxurg_clicked (object sender, EventArgs args)
		{
			if (checkbutton_todos_envios.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Question,ButtonsType.YesNo,"Esta seguro de crear un pase para QX/Urgencias");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
			 	if (miResultado == ResponseType.Yes){
			 		checkbutton_todos_envios.Active = false;
					NpgsqlConnection conexion;
					conexion = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						comando.CommandText = "INSERT INTO osiris_erp_pases_qxurg(" +
							"pid_paciente," +
							"folio_de_servicio," +
							"fechahora_creacion," +
							"id_tipo_admisiones," +
							"id_quien_creo," +
							"observaciones," +
							"id_tipo_paciente," +
							"id_empresa," +
							"id_aseguradora" +
							") VALUES ('"+
							pidpaciente.ToString().Trim()+"','"+
							folioservicio.ToString().Trim()+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							idcentro_costo.ToString().Trim()+"','"+
							LoginEmpleado+"','"+
							""+"','"+
							idtipopaciente.ToString().Trim()+"','"+
							idempresa_paciente.ToString().Trim()+"','"+
							idaseguradora_paciente.ToString().Trim()+
							"');";
						
						comando.ExecuteNonQuery();					comando.Dispose();
						llenado_treeview_pases();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();				
					}
					conexion.Close();
			 	}else{
					checkbutton_todos_envios.Active = false;
				}
			}else{
				checkbutton_todos_envios.Active = false;
			}
		}
		
		void on_printing_pase_qx_clcked(object sender, EventArgs args)
		{
			printing_pase();
		}
		
		void printing_pase()
		{
			print = new PrintOperation ();
			print.JobName = "Pase para Servicios Medicos";	// Name of the report
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
			context = args.Context;
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string sexopaciente = "";
			string empresa_o_aseguradora = "";
			string titulo_de_pase = "";
			
			if(tipo_pase=="pase_qx_urg"){
				query_slq = "SELECT osiris_erp_pases_qxurg.id_secuencia,osiris_erp_pases_qxurg.folio_de_servicio AS foliodeservicio," +
							"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
							"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
							"osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.nombre_medico_tratante,"+
							"osiris_erp_pases_qxurg.id_quien_creo,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
							"to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
							"osiris_erp_pases_qxurg.id_tipo_admisiones,descripcion_admisiones," +
							"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
							"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
							 "id_quien_creo " +
						 	"FROM osiris_erp_pases_qxurg,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras "+
							"WHERE osiris_erp_pases_qxurg.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND osiris_erp_pases_qxurg.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_pases_qxurg.id_quien_creo = osiris_empleado.login_empleado "+
							"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";
				titulo_de_pase = "PASE_A_SERVICIO_MEDICO_QUIRURGICO";
			}
			if(tipo_pase=="pase_de_ingreso"){
				query_slq = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS id_secuencia,osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio," +
							"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
							"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
							"osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.nombre_medico_tratante,"+
							"osiris_erp_cobros_enca.id_empleado_admision,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
							"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones," +
							"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
							"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
							 "id_empleado_admision AS id_quien_creo,descripcion_diagnostico_movcargos,nombre_de_cirugia " +
						 	"FROM osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras "+
							"WHERE " +
							"osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_cobros_enca.id_empleado_admision = osiris_empleado.login_empleado "+
							"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
							"AND osiris_erp_cobros_enca.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";
				titulo_de_pase = "PASE_DE_INGRESO";
			}
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
						
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = query_slq;
	        	Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					if (lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "MASCULINO";
					}else{
						sexopaciente = "FEMENINO";
					}
					
					if((int) lector ["id_aseguradora"] > 1){
						empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
					}else{
						empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
					}
					buscar_en_movcargos(lector["foliodeservicio"].ToString().Trim());
					imprime_encabezado(cr,
					                   layout,
					                   lector["descripcion_admisiones"].ToString().Trim(),
					                   lector["id_secuencia"].ToString().Trim(),
					                   lector["fechahoracrea"].ToString().Trim(),
					               		lector["foliodeservicio"].ToString().Trim(),
					                   lector["pidpaciente"].ToString().Trim(),
					                   lector["nombre_completo"].ToString().Trim(),
					               		lector["fechanacpaciente"].ToString().Trim(),
					                   lector["edadpaciente"].ToString().Trim(),
					                   sexopaciente,
					               		diagnostico_movcargo,
					                   nombrecirugia_movcargo,
					                   lector["nombre_medico_tratante"].ToString().Trim(),
					                   "",
					               		lector["id_quien_creo"].ToString().Trim(),
					                   lector["nombresolicitante"].ToString().Trim(),
					              	 	descripciontipopaciente,
					                 empresa_o_aseguradora,titulo_de_pase);
					/*
					imprime_encabezado(cr,
									layout,
									(string) lector["area_quien_solicita"],
					                   lector["folio_de_solicitud"].ToString().Trim(),
					               lector["fechahora_solicitud"].ToString().Trim(),
					                   lector["foliodeservicio"].ToString().Trim(),
					                   lector["pidpaciente"].ToString().Trim(),
					               lector["nombre_completo"].ToString().Trim(),
					                   lector["fechanacpaciente"].ToString().Trim(),
					                   lector["edadpaciente"].ToString().Trim(),
					               sexopaciente,
					               diagnostico_movcargo,
					                   nombrecirugia_movcargo,
					                   lector["nombre_medico_tratante"].ToString().Trim(),
					               lector["descripcion_cuarto"].ToString().Trim()+" "+lector["numero_cuarto"].ToString().Trim(),
					                   lector["id_quien_solicito"].ToString().Trim(),
					               lector["nombresolicitante"].ToString().Trim(),
					                   lector["descripcion_proveedor"].ToString().Trim());
					                  */
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
			conexion.Close();
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string areaquiensolicita,string numerosolicitud,string fechasolicitud, 
		                    string numerodeatencion, string numeroexpediente, string nombrepaciente, string fechanacimiento, string edadpaciente, 
		                    string sexodelpaciente, string descripciondiagnostico, string nombredecirugia, string medicotratante, string numerohabitacion,
		                    string quiensolicito, string nomsolicitante, string tipo_paciente,string empresa_aseguradora,string titulo_de_pase)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//---image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
			
			comienzo_linea = 60;
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
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_de_pase);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita						
								
			desc = Pango.FontDescription.FromString ("Sans");									 
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Area quien Solicito: "+areaquiensolicita);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: ");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha : "+fechasolicitud);						Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			if(tipo_pase == "pase_qx_urg"){
				cr.MoveTo(250*escala_en_linux_windows,comienzo_linea-separacion_linea*escala_en_linux_windows);		layout.SetText("N° de Pase: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numerodeatencion);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+numeroexpediente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Nacimiento: "+fechanacimiento);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: "+sexodelpaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Procedimiento: "+nombredecirugia);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Diagnostico Admision: "+descripciondiagnostico);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Medico Tratante: "+medicotratante);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: "+numerohabitacion);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Usuario: "+quiensolicito);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nom. Cajero: "+nomsolicitante);					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo de Paciente : "+tipo_paciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Empresa o Municipio : "+empresa_aseguradora);							Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			//comienzo_linea += separacion_linea;
			if(tipo_pase == "pase_qx_urg"){
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*2))*escala_en_linux_windows);		layout.SetText("Cirugia :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*4))*escala_en_linux_windows);		layout.SetText("Cirujano :____________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*6))*escala_en_linux_windows);		layout.SetText("Ayudante :____________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*8))*escala_en_linux_windows);		layout.SetText("Anestesiologo :______________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*10))*escala_en_linux_windows);		layout.SetText("Tipo de Anestesia :______________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*12))*escala_en_linux_windows);		layout.SetText("Nom. Proveedor 1 :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*14))*escala_en_linux_windows);		layout.SetText("Nom. Proveedor 2 :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*16))*escala_en_linux_windows);		layout.SetText("Materiales y/o Equipos :");							Pango.CairoHelper.ShowLayout (cr, layout);						
		
				cr.MoveTo(430*escala_en_linux_windows,(comienzo_linea+(separacion_linea*2))*escala_en_linux_windows);		layout.SetText("Sello de Dep. Medico");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(150*escala_en_linux_windows,(comienzo_linea+(separacion_linea*21))*escala_en_linux_windows);		layout.SetText("Sello y Firma Cajero");							Pango.CairoHelper.ShowLayout (cr, layout);
			}
			fontSize = 6.5;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*19))*escala_en_linux_windows);		layout.SetText(nombrepaciente);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*20))*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			if(medicotratante != ""){
				cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*21))*escala_en_linux_windows);		layout.SetText("Dr. "+medicotratante);				Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Fecha: "+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));				Pango.CairoHelper.ShowLayout (cr, layout);

			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			//Console.WriteLine(comienzo_linea.ToString());
			cr.Rectangle (05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows, 565*escala_en_linux_windows, (separacion_linea*18)*escala_en_linux_windows);
			
			if(tipo_pase == "pase_qx_urg"){
				// Linea Vertical
				cr.MoveTo(400*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);
				cr.LineTo(400*escala_en_linux_windows,330);
			}
			//cr.MoveTo(565*escala_en_linux_windows, 383*escala_en_linux_windows);
			//cr.LineTo(05,383);		// Linea Horizontal 4
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();
			
		}
		
		void buscar_en_movcargos(string foliodeservicio)
		{
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),id_tipo_admisiones,osiris_erp_movcargos.id_tipo_paciente," +
	           		"pid_paciente,descripcion_tipo_paciente,descripcion_diagnostico_movcargos,nombre_de_cirugia "+
					"FROM osiris_erp_movcargos,osiris_his_tipo_pacientes "+
					"WHERE osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_movcargos.folio_de_servicio = '"+foliodeservicio+"';";
	        	//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					diagnostico_movcargo = lector["descripcion_diagnostico_movcargos"].ToString().Trim();
					nombrecirugia_movcargo = lector["nombre_de_cirugia"].ToString().Trim();
					descripciontipopaciente = lector["descripcion_tipo_paciente"].ToString().Trim().ToUpper();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

