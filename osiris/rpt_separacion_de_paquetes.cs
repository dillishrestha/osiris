// rpt_separacion_de_paquetes.cs created with MonoDevelop
// User: ipena at 09:50 a 14/05/2008
//   AUTOR:  ISRAEL (Programacion) 
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.IO;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	
	public class rpt_separacion_de_paquetes
	{
		//declarando la ventana 
		[Widget] Gtk.Window rpt_ocupacion;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_agregar_monto;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.TreeView lista_ocupacion;
		[Widget] Gtk.Entry entry_totalsaldos;
		[Widget] Gtk.Entry entry_totalabonos;
		[Widget] Gtk.Label label243;
		[Widget] Gtk.Label label244;
		
		string connectionString;
        string nombrebd;
		string tiporeporte = "SEPARACION DE PAQUETES";
		string titulo = "REPORTE SEPARACION DE PAQUETES";
		
		int fila = -70;
		int contador = 1;
		int numpage = 1;
		
		int idcuarto = 0;
		decimal saldos = 0;
		decimal totabono = 0;
		decimal totcuenta = 0;
		decimal sumacuenta = 0;
		decimal abono = 0;
	//	public decimal abonomuestra = 0;
		
		private TreeStore treeViewEngineocupacion;
		
		//Declarando las celdas
		CellRendererText cellrt0;			CellRendererText cellrt1;
		CellRendererText cellrt2;			CellRendererText cellrt3;
		CellRendererText cellrt4;			CellRendererText cellrt5;
		CellRendererText cellrt6;			CellRendererText cellrt7;
		CellRendererText cellrt8;			CellRendererText cellrt9;			
								
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_separacion_de_paquetes(string nombrebd_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML  gxml = new Glade.XML  (null, "registro_admision.glade", "rpt_ocupacion", null);
			gxml.Autoconnect  (this);	
			rpt_ocupacion.Show();
			rpt_ocupacion.Title = "Reporte de Separacion Paquetes";
			
			this.button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			this.checkbutton_agregar_monto.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
			this.checkbutton_impr_todo_proce.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
			crea_treeview_ocupacion();
			llenando_lista_de_ocupacion();
			
			//this.checkbutton_agregar_monto.Hide();
			this.checkbutton_impr_todo_proce.Hide();
			this.entry_totalabonos.Hide();
			this.entry_totalsaldos.Hide();
			this.label243.Hide();
			this.label244.Hide();
	    }
		
		void on_checkbutton_agregar_monto_clicked(object sender, EventArgs args)
		{
			llenando_lista_de_ocupacion();
		}
		
	    void crea_treeview_ocupacion()
		{
				treeViewEngineocupacion = new TreeStore(typeof(string),
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
				lista_ocupacion.Model = treeViewEngineocupacion;
				lista_ocupacion.RulesHint = true;
				
				TreeViewColumn col_nombre = new TreeViewColumn();
				CellRendererText cellrt0 = new CellRendererText();
				col_nombre.Title = "NOMBRE"; // titulo de la cabecera de la columna, si está visible
				col_nombre.PackStart(cellrt0, true);
				col_nombre.AddAttribute (cellrt0, "text", 0);   
				col_nombre.SortColumnId = (int) Col_ocupacion.col_nombre;
				col_nombre.Resizable = true;
				cellrt0.Width = 200;
				
				TreeViewColumn col_folio = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_folio.Title = "Folio";
				col_folio.PackStart(cellrt1, true);
				col_folio.AddAttribute (cellrt1, "text", 1); 
				col_folio.SortColumnId = (int) Col_ocupacion.col_folio;
				
				TreeViewColumn col_pid = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_pid.Title = "PID";
				col_pid.PackStart(cellrt2, true);
				col_pid.AddAttribute (cellrt2, "text", 2); 
				col_pid.SortColumnId = (int) Col_ocupacion.col_pid;
				
				TreeViewColumn col_fecha = new TreeViewColumn();
				CellRendererText cellrt11 = new CellRendererText();
				col_fecha.Title = "Fecha de Ingreso";
				col_fecha.PackStart(cellrt11, true);
				col_fecha.AddAttribute (cellrt11, "text", 3);
				col_fecha.SortColumnId = (int) Col_ocupacion.col_fecha;
				
			    TreeViewColumn col_abono = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_abono.Title = "Abonos";
				col_abono.PackStart(cellrt4, true);
				col_abono.AddAttribute (cellrt4, "text", 5); 
				col_abono.SortColumnId = (int) Col_ocupacion.col_abono;
				
				TreeViewColumn col_medico = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_medico.Title = "Medico Tratante";
				col_medico.PackStart(cellrt6, true);
				col_medico.AddAttribute (cellrt6, "text", 7); 
				col_medico.SortColumnId = (int) Col_ocupacion.col_medico;
				col_medico.Resizable = true;
				cellrt6.Width = 200;
				
				TreeViewColumn col_diag = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_diag.Title = "Diagnostico";
				col_diag.PackStart(cellrt8, true);
				col_diag.AddAttribute (cellrt8, "text", 9); 
				col_diag.SortColumnId = (int) Col_ocupacion.col_diag;
				col_diag.Resizable = true;
				cellrt8.Width = 300;
				
				TreeViewColumn col_tipopac = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_tipopac.Title = "Tipo Paciente";
				col_tipopac.PackStart(cellrt9, true);
				col_tipopac.AddAttribute (cellrt9, "text", 10); 
				col_tipopac.SortColumnId = (int) Col_ocupacion.col_tipopac;
				
				TreeViewColumn col_asegu_empresa = new TreeViewColumn();
				CellRendererText cellrt10 = new CellRendererText();
				col_asegu_empresa.Title = "Empresa/Aseguradora";
				col_asegu_empresa.PackStart(cellrt10, true);
				col_asegu_empresa.AddAttribute (cellrt10, "text", 11); 
				col_asegu_empresa.SortColumnId = (int) Col_ocupacion.col_asegu_empresa;
				
				TreeViewColumn col_fecha_reservacion = new TreeViewColumn();
				CellRendererText cellrt12 = new CellRendererText();
				col_fecha_reservacion.Title = "Fecha de Reservacion";
				col_fecha_reservacion.PackStart(cellrt12, true);
				col_fecha_reservacion.AddAttribute (cellrt12, "text", 12); 
				col_fecha_reservacion.SortColumnId = (int) Col_ocupacion.col_fecha_reservacion;
				
				lista_ocupacion.AppendColumn(col_nombre);
				lista_ocupacion.AppendColumn(col_folio);
				lista_ocupacion.AppendColumn(col_pid);
				lista_ocupacion.AppendColumn(col_fecha);
				lista_ocupacion.AppendColumn(col_abono);
				lista_ocupacion.AppendColumn(col_medico);
				lista_ocupacion.AppendColumn(col_diag);
				lista_ocupacion.AppendColumn(col_tipopac);
				lista_ocupacion.AppendColumn(col_asegu_empresa);
				lista_ocupacion.AppendColumn(col_fecha_reservacion);
		}
		
		enum Col_ocupacion
		{
				col_nombre,
				col_folio,
				col_pid,
				col_fecha,
				col_abono,
				col_medico,
				col_diag,
				col_tipopac,
				col_asegu_empresa,
				col_fecha_reservacion
		}
		
		void llenando_lista_de_ocupacion()
		{
				string descri_empresa_aseguradora = "";
				treeViewEngineocupacion.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
				
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
		        // Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					// asigna el numero de folio de ingreso de paciente (FOLIO)
					comando.CommandText ="SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),"+							
								"to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
								"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
								"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
								"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
								"osiris_erp_cobros_enca.id_medico,nombre_medico, "+
								"osiris_erp_cobros_enca.id_medico_tratante,nombre_medico,"+
								"osiris_erp_cobros_enca.nombre_medico_tratante,"+
								"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
								"to_char(osiris_erp_cobros_enca.id_cuarto,'999999999') AS cuarto, "+
								"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totabonos, "+
								"osiris_erp_movcargos.descripcion_diagnostico_movcargos,"+
								"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fecha_ingreso, "+
						        "to_char(osiris_erp_cobros_enca.fecha_reservacion,'dd-MM-yyyy HH24:mi')AS fecha_reservacion, "+  ///////fecha de reservacion////////
								"to_char(osiris_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc,"+
								"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, descripcion_tipo_paciente "+
								"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_aseguradoras,osiris_empresas,osiris_his_medicos "+
								"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
								"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico "+
								//"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico_tratante "+
								"AND osiris_erp_cobros_enca.reservacion = 'true' "+
								//"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
								"ORDER BY nombre_completo ;";
					//Console.WriteLine(comando.CommandText);	
					NpgsqlDataReader lector = comando.ExecuteReader ();
					
					string foliodeservicio;
					saldos = 0;
					//decimal abonomuestra = 0;
					totabono = 0;
					totcuenta = 0;
					sumacuenta = 0;
					abono = 0;
					while (lector.Read()){
						foliodeservicio = (string) lector["foliodeatencion"];
						if (this.checkbutton_agregar_monto.Active == true){
							abono = decimal.Parse((string) lector["totabonos"]);
						}else{
							totcuenta = 0;
							sumacuenta = 0;
							abono = 0 ;
							saldos = 0;
							entry_totalsaldos.Text = sumacuenta.ToString();
						}
						
						if((int) lector ["id_aseguradora"] > 1){
							descri_empresa_aseguradora =  (string) lector["descripcion_aseguradora"];
						}else{
							descri_empresa_aseguradora =  (string) lector["descripcion_empresa"];						
						}
						
						treeViewEngineocupacion.AppendValues ((string) lector["nombre_completo"],//0
														(string) lector["foliodeatencion"],//1
														(string) lector["pidpaciente"],//2
														(string) lector["fecha_ingreso"],//2
						                                totcuenta.ToString(),//3
														abono.ToString(),//4
														saldos.ToString(),
														(string) lector["nombre_medico_tratante"],
														idcuarto.ToString(),
														//(string) lector["cuarto"]);
														(string) lector["descripcion_diagnostico_movcargos"],
														(string) lector["descripcion_tipo_paciente"],
														descri_empresa_aseguradora,
						                                (string) lector["fecha_reservacion"]);
					}     
				}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
		}
		
		   void imprime_reporte(object sender, EventArgs args)
		{	
				titulo = "REPORTE SEPARACION DE PAQUETES";
				Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
	        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
	        	int         respuesta = dialogo.Run ();
	        	if (respuesta == (int) Gnome.PrintButtons.Cancel){
					dialogo.Hide (); 		dialogo.Dispose (); 
					return;
				}

	        	Gnome.PrintContext ctx = trabajo.Context;        
	        	ComponerPagina(ctx, trabajo); 
	        	trabajo.Close();
	             
	        	switch (respuesta)
	        	{
	                  case (int) Gnome.PrintButtons.Print:   
	                  		trabajo.Print (); 
	                  		break;
	                  case (int) Gnome.PrintButtons.Preview:
	                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
	                        break;
	        	}
	        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		   void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
				ContextoImp.BeginPage("Pagina 1");
				ContextoImp.Rotate(90);
				imprime_rpt_separacion_paquetes(ContextoImp,trabajoImpresion);
				ContextoImp.ShowPage();
		}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////REPORTE DE SEPARACION DE PAQUETES/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		void imprime_rpt_separacion_paquetes(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
				TreeIter iter;
				string tomovalor1 = "";
				fila = -75;
				int contadorprocedimientos = 0;
				contador = 0;
				numpage = 1;
				imprime_encabezado(ContextoImp,trabajoImpresion);
				if (this.treeViewEngineocupacion.GetIterFirst (out iter)){
					Gnome.Print.Setfont (ContextoImp, fuente6);
				    ContextoImp.MoveTo(43, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
					ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
					ContextoImp.MoveTo(110.5, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(254.5,fila);					ContextoImp.Show(tomovalor1);//nombre
					ContextoImp.MoveTo(375.5, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
					
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(440,fila);					ContextoImp.Show(tomovalor1);//medico
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(530,fila);					ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(650,fila);					ContextoImp.Show(tomovalor1);//topo paciente
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
					if(tomovalor1.Length > 25){
					tomovalor1 = tomovalor1.Substring(0,25); 
					}
					ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
				
				    tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,12);
					if(tomovalor1.Length > 25){
					tomovalor1 = tomovalor1.Substring(0,25); 
					}
				    ContextoImp.MoveTo(170, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,12));//fecha reservacion
				    //ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
					fila -= 08;
					//contador+=1;
					contadorprocedimientos += 0;
					salto_pagina(ContextoImp,trabajoImpresion);
					
					while (this.treeViewEngineocupacion.IterNext(ref iter)){
						Gnome.Print.Setfont (ContextoImp, fuente6);
						ContextoImp.MoveTo(43, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
						ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
						ContextoImp.MoveTo(110.5, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(254.5,fila);					ContextoImp.Show(tomovalor1);//nombre
						
						ContextoImp.MoveTo(375.5, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
						
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(440,fila);					ContextoImp.Show(tomovalor1);//medico
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(530,fila);					ContextoImp.Show(tomovalor1);
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(650,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
						if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
						}
						ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
					
					    tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,12);
					    if(tomovalor1.Length > 25){
					    tomovalor1 = tomovalor1.Substring(0,25); 
					    }
				        ContextoImp.MoveTo(170, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,12));//fecha reservacion
				    
						//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
						
						fila -= 08;
						contador+=1;
						contadorprocedimientos += 1;
						salto_pagina(ContextoImp,trabajoImpresion);
					}
					fila-=10;
					contador+=1;
					contadorprocedimientos += 1;
					salto_pagina(ContextoImp,trabajoImpresion);					
				}
			    Gnome.Print.Setfont (ContextoImp, fuente9);
				ContextoImp.MoveTo(99.5,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(100,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(384.5,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(385,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(459.5,fila);				ContextoImp.Show(totabono.ToString("C"));
				ContextoImp.MoveTo(460,fila);				ContextoImp.Show(totabono.ToString("C"));
				//contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion);				
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{  
				Gnome.Print.Setfont (ContextoImp, fuente6);
				ContextoImp.MoveTo(65.5, -30);			ContextoImp.Show("Hospital Santa Cecilia");
				ContextoImp.MoveTo(66, -30);			ContextoImp.Show("Hospital Santa Cecilia");
				ContextoImp.MoveTo(65.5, -40);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
				ContextoImp.MoveTo(66, -40);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
				ContextoImp.MoveTo(65.5, -50);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
				ContextoImp.MoveTo(66, -50);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
				Gnome.Print.Setfont(ContextoImp,fuente11);
				ContextoImp.MoveTo(350.5, -40);			ContextoImp.Show(titulo);
				ContextoImp.MoveTo(351, -40);			ContextoImp.Show(titulo);
				Gnome.Print.Setfont (ContextoImp, fuente10);
				ContextoImp.MoveTo(330.7, -550);		ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				ContextoImp.MoveTo(330, -550);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				Gnome.Print.Setfont (ContextoImp, fuente9);
				Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);//regreso color fuente a negro
				imprime_titulo(ContextoImp,trabajoImpresion);
			}
        
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
				Gnome.Print.Setfont(ContextoImp,fuente9);
	            ContextoImp.MoveTo(55.5, -65);					ContextoImp.Show("Pid:"); 
				ContextoImp.MoveTo(56, -65);					    ContextoImp.Show("Pid:");
				
				ContextoImp.MoveTo(80.5, -65);					ContextoImp.Show("Folio:");
				ContextoImp.MoveTo(81, -65);					    ContextoImp.Show("Folio:");
				
				ContextoImp.MoveTo(110.5, -65);					ContextoImp.Show("F. Ingreso:");
				ContextoImp.MoveTo(111, -65);					    ContextoImp.Show("F. Ingreso:");
				
				ContextoImp.MoveTo(170.5, -65);					ContextoImp.Show("F. Reservacion:");
				ContextoImp.MoveTo(171, -65);					    ContextoImp.Show("F. Reservacion:");
				
				ContextoImp.MoveTo(254.5, -65);					ContextoImp.Show("Nombre:");
				ContextoImp.MoveTo(255, -65);					    ContextoImp.Show("Nombre:");
				
				ContextoImp.MoveTo(375.5, -65);					ContextoImp.Show("Abono:");
				ContextoImp.MoveTo(376, -65);					    ContextoImp.Show("Abono:");
				
				ContextoImp.MoveTo(440.5, -65);					ContextoImp.Show("Medico:");
				ContextoImp.MoveTo(441, -65);					    ContextoImp.Show("Medico:");
				
				ContextoImp.MoveTo(530.5, -65);					ContextoImp.Show("Diagnostico:");
				ContextoImp.MoveTo(531, -65);					    ContextoImp.Show("Diagnostico:");
				
				ContextoImp.MoveTo(650.5, -65);					ContextoImp.Show("T. Paciente:");
				ContextoImp.MoveTo(651, -65);					    ContextoImp.Show("T. Paciente:");
				
				ContextoImp.MoveTo(715.5, -65);					ContextoImp.Show("Aseg./Empresa:");
				ContextoImp.MoveTo(716, -65);					    ContextoImp.Show("Aseg./Empresa:");
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
		        if (contador > 55 ){
		        	numpage +=1;        	contador=1;
		        	fila = -75;
		        	ContextoImp.ShowPage();
					ContextoImp.BeginPage("Pagina "+numpage.ToString());
					ContextoImp.Rotate(90);
					imprime_encabezado(ContextoImp,trabajoImpresion);
		     	}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
				//Console.WriteLine(args.Event.Key);
				//Console.WriteLine(Convert.ToChar(args.Event.Key));
				string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
				if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
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
