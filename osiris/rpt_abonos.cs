//////////////////////////////////////////////////////////////////////
// created on 28/02/2008 at 09:47 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Tec. Homero Montoya Galvan (Programacion) homerokda@hotmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes)
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
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class reporte_de_abonos
	{
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Label label142;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.CheckButton checkbutton_export_to = null;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		Pango.FontDescription desc;
		
		string connectionString;
        string nombrebd;
		string tiporeporte = "ABONOS";
		string titulo = "REPORTE DE ABONOS";
		
		string query_fechas = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		string tiporpt = "";
		string LoginEmpleado = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_de_abonos(string _nombrebd_,string tiporpt_,string LoginEmpleado_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tiporpt = tiporpt_;
			LoginEmpleado = LoginEmpleado_;
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
        	rango_de_fecha.Show();
			
        	checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.IsEditable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
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
        	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
        	button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte_abonos);
        	label_orden.Hide();
			label_nom_cliente.Hide();
			label142.Hide();
			radiobutton_cliente.Hide();
			radiobutton_fecha.Hide();
			checkbutton_todos_los_clientes.Hide();
			entry_referencia_inicial.Hide();
			entry_cliente.Hide();
			button_busca_cliente.Hide();		
    	}    
		
		void imprime_reporte_abonos(object sender, EventArgs args)
		{
			if (this.checkbutton_impr_todo_proce.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
				query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+//;//;//'"+rango1+"' "+
								"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
			}
			
			if(tiporpt == "abonos_pagos"){			
				print = new PrintOperation ();
				print.JobName = "Reporte de Abonos y Pagos";
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);	
				rango_de_fecha.Destroy();
			}
			if(tiporpt == "corte_caja"){
				if(LoginEmpleado == "DOLIVARES" || LoginEmpleado =="ADMIN" || LoginEmpleado =="MARGARITAZ" || LoginEmpleado =="IESPINOZAF" || LoginEmpleado == "ELVIAVM"){
					string query_sql = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabonopago,"+
										"osiris_erp_abonos.id_abono,"+
										"to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS foliodeservicio,"+
										"osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombrepaciente,"+
										"osiris_erp_abonos.monto_de_abono_procedimiento AS monto_comprobante,osiris_erp_abonos.concepto_del_abono,numero_recibo_caja AS numerorecibo,"+
										"osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_forma_de_pago.descripcion_forma_de_pago AS forma_de_pago,osiris_erp_abonos.monto_convenio,osiris_erp_abonos.observaciones," +
										"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente "+
										"FROM osiris_erp_cobros_enca,osiris_erp_abonos,osiris_erp_tipo_comprobante, osiris_his_paciente, osiris_erp_forma_de_pago,osiris_erp_movcargos,osiris_his_tipo_pacientes "+
										"WHERE osiris_erp_abonos.eliminado = false "+
										"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
										"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_abonos.folio_de_servicio "+
										"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+ 
										"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
										"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
										"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_movcargos.id_tipo_paciente "+
										query_fechas+
										";";
									
					string[] args_names_field = {"foliodeservicio","pidpaciente","nombrepaciente","numerorecibo","descripcion_tipo_comprobante","monto_comprobante","forma_de_pago","concepto_del_abono","observaciones","monto_convenio","descripcion_tipo_paciente"};
					string[] args_type_field = {"float","float","string","float","string","float","string","string","string","float","string"};
					string[] args_field_text = {};
					string[] args_more_title = {};
					// class_crea_ods.cs
					//Console.WriteLine(query_sql);
					if(checkbutton_export_to.Active == true){
						new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title);
					}else{
						
					}
				}
			}
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
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			decimal total = 0;
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS folio, "+
							"to_char(osiris_erp_abonos.id_abono,'9999999999') AS idabono, "+
							"to_char(numero_recibo_caja,'9999999999') AS recibocaja, "+
							"id_quien_creo, "+
							"to_char(osiris_erp_abonos.monto_de_abono_procedimiento,'9,999,999,999.99') AS abono, "+
							"osiris_erp_abonos.concepto_del_abono AS concepto, "+
							"osiris_erp_abonos.eliminado, "+
							"osiris_erp_abonos.id_quien_elimino, "+
							"osiris_erp_abonos.fechahora_eliminado, "+
							"to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabono, "+
							"osiris_erp_abonos.id_forma_de_pago, "+ 
							"osiris_erp_forma_de_pago.id_forma_de_pago,descripcion_forma_de_pago AS descripago, "+
							"osiris_his_paciente.pid_paciente || '   ' || nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo "+
							"FROM osiris_erp_abonos,osiris_erp_forma_de_pago,osiris_erp_cobros_enca,osiris_his_paciente "+
							"WHERE osiris_erp_abonos.eliminado = false "+
							"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
							"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
							" "+query_fechas+" "+
							"ORDER BY osiris_erp_abonos.folio_de_servicio;";															
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				imprime_encabezado(cr,layout);
				while (lector.Read()){
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["folio"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(40*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(string.Format("{0:C}",decimal.Parse(lector["abono"].ToString())));	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(93*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(lector["fechaabono"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(125*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(lector["recibocaja"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(171*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(lector["nombre_completo"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);

					comienzo_linea += separacion_linea;
					total += decimal.Parse((string) lector["abono"]);
					contadorprocedimientos += 1;
					/*
					tomovalor1 = (string) lector["concepto"];
					if(tomovalor1.Length > 40)
					{
						tomovalor1 = tomovalor1.Substring(0,40); 
					}
					ContextoImp.MoveTo(351, filas);		ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(501, filas);		ContextoImp.Show((string) lector["descripago"]);
					*/
				}
				comienzo_linea += separacion_linea;
				cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("TOTAL DE PAGOS/ABONOS "+string.Format("{0:C}",decimal.Parse(total.ToString())));	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("TOTAL N° ATENCION "+contadorprocedimientos.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}			
		}
 	
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{	
 			desc = Pango.FontDescription.FromString ("Sans");								
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
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText("REPORTE DE PAGO/ABONOS");				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			if(rango1 == "" || rango2 == ""){
				cr.MoveTo(235*escala_en_linux_windows, 45*escala_en_linux_windows);		layout.SetText("Todas Las Fechas");	Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				if(rango1 == rango2) {
					cr.MoveTo(235*escala_en_linux_windows, 45*escala_en_linux_windows);		layout.SetText("FECHA: "+rango1);	Pango.CairoHelper.ShowLayout (cr, layout);
				}else{
					cr.MoveTo(235*escala_en_linux_windows, 45*escala_en_linux_windows);		layout.SetText("Rango del "+rango1+" al "+rango2);	Pango.CairoHelper.ShowLayout (cr, layout);
				}
			}			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			// Creando el Cuadro de Titulos para colocar el nombre del usuario
			cr.Rectangle (05*escala_en_linux_windows, 55*escala_en_linux_windows, 565*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
				
			/*	
			ContextoImp.MoveTo(26,720);				ContextoImp.Show("FOLIO");			
			ContextoImp.MoveTo(56,720);				ContextoImp.Show("MONTO");			
			ContextoImp.MoveTo(93,720); 			ContextoImp.Show("F. ABONO");			
			ContextoImp.MoveTo(134,720); 			ContextoImp.Show("Nº. REC.");			
			ContextoImp.MoveTo(171,720);			ContextoImp.Show("PID Y NOMBRE DEL PACIENTE");			
			ContextoImp.MoveTo(351,720);			ContextoImp.Show("CONCEPTO");			
			ContextoImp.MoveTo(501,720);			ContextoImp.Show("FORMA DE PAGO");
			*/
	  	}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea > 700){
				cr.ShowPage();
				comienzo_linea = 70;
				imprime_encabezado(cr,layout);
			}
		}
	
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
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
	