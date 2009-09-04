// created on 25/01/2008 at 11:20 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Jesus Buentello (Ajustes)
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
using System.IO;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	//reporte_pacientes_sin_alta
	public class reporte_pacientes_sin_alta
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rpt_ocupacion;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_agregar_monto;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.TreeView lista_ocupacion;
		[Widget] Gtk.Entry entry_totalsaldos;
		[Widget] Gtk.Entry entry_totalabonos;
		[Widget] Gtk.Entry entry_total_de_pacientes;
		
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public string tiporeporte = "SINALTA";
		public string titulo = "REPORTE DE PACIENTES SIN ALTA";
		
		public int fila = -70;
		public int contador = 1;
		public int numpage = 1;
		
		public string idcuarto = "";
		public decimal saldos = 0;
		public decimal totabono = 0;
		public decimal totcuenta = 0;
		public decimal sumacuenta = 0;
		public decimal abono = 0;
		public decimal abonomuestra = 0;
		
		private TreeStore treeViewEngineocupacion;
		
		//Declarando las celdas
		public CellRendererText cellrt0;			public CellRendererText cellrt1;
		public CellRendererText cellrt2;			public CellRendererText cellrt3;
		public CellRendererText cellrt4;			public CellRendererText cellrt5;
		public CellRendererText cellrt6;			public CellRendererText cellrt7;
		public CellRendererText cellrt8;			public CellRendererText cellrt9;			
								
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		//public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		//public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		//public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		//public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		//public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		//public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		//public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		//public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public reporte_pacientes_sin_alta(string _nombrebd_)
		{
			nombrebd = _nombrebd_;
			
			Glade.XML  gxml = new Glade.XML  (null, "registro_admision.glade", "rpt_ocupacion", null);
			gxml.Autoconnect  (this);	
			rpt_ocupacion.Show();
			checkbutton_impr_todo_proce.Label = "Agrega Abonos";
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			this.checkbutton_agregar_monto.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
			this.checkbutton_impr_todo_proce.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
			crea_treeview_ocupacion();
			llenando_lista_de_ocupacion();
		}
		
		void crea_treeview_ocupacion()
		{
			treeViewEngineocupacion = new TreeStore(typeof(string),//0
													typeof(string),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(string));//10
			lista_ocupacion.Model = treeViewEngineocupacion;
			lista_ocupacion.RulesHint = true;
			
			TreeViewColumn col_nombre = new TreeViewColumn();
			CellRendererText cellrt0 = new CellRendererText();
			col_nombre.Title = "NOMBRE"; // titulo de la cabecera de la columna, si está visible
			col_nombre.PackStart(cellrt0, true);
			col_nombre.AddAttribute (cellrt0, "text", 0);    // la siguiente columna será 1
			col_nombre.SortColumnId = (int) Col_ocupacion.col_nombre;
			col_nombre.Resizable = true;
			cellrt0.Width = 200;
			
			TreeViewColumn col_folio = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_folio.Title = "Folio";
			col_folio.PackStart(cellrt1, true);
			col_folio.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_folio.SortColumnId = (int) Col_ocupacion.col_folio;
			
			TreeViewColumn col_pid = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_pid.Title = "PID";
			col_pid.PackStart(cellrt2, true);
			col_pid.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_pid.SortColumnId = (int) Col_ocupacion.col_pid;
			
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_fecha.Title = "Fecha de Ingreso";
			col_fecha.PackStart(cellrt11, true);
			col_fecha.AddAttribute (cellrt11, "text", 3); // la siguiente columna será 3
			col_fecha.SortColumnId = (int) Col_ocupacion.col_fecha;
			
			TreeViewColumn col_saldo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_saldo.Title = "Saldo";
			col_saldo.PackStart(cellrt3, true);
			col_saldo.AddAttribute (cellrt3, "text", 4); // la siguiente columna será 4
			col_saldo.SortColumnId = (int) Col_ocupacion.col_saldo;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_abono.Title = "Abonos";
			col_abono.PackStart(cellrt4, true);
			col_abono.AddAttribute (cellrt4, "text", 5); // la siguiente columna será 5
			col_abono.SortColumnId = (int) Col_ocupacion.col_abono;
			
			TreeViewColumn col_paga = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_paga.Title = "A Pagar";
			col_paga.PackStart(cellrt5, true);
			col_paga.AddAttribute (cellrt5, "text", 6); // la siguiente columna será 7
			col_paga.SortColumnId = (int) Col_ocupacion.col_paga;			
			
			TreeViewColumn col_medico = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_medico.Title = "Medico Tratante";
			col_medico.PackStart(cellrt6, true);
			col_medico.AddAttribute (cellrt6, "text", 7); // la siguiente columna será 6
			col_medico.SortColumnId = (int) Col_ocupacion.col_medico;
			col_medico.Resizable = true;
			cellrt6.Width = 200;
			
			TreeViewColumn col_cuarto = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_cuarto.Title = "Habitacion";
			col_cuarto.PackStart(cellrt7, true);
			col_cuarto.AddAttribute (cellrt7, "text", 8); // la siguiente columna será 7
			col_cuarto.SortColumnId = (int) Col_ocupacion.col_cuarto;
						
			TreeViewColumn col_diag = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_diag.Title = "Diagnostico";
			col_diag.PackStart(cellrt8, true);
			col_diag.AddAttribute (cellrt8, "text", 9); // la siguiente columna será 7
			col_diag.SortColumnId = (int) Col_ocupacion.col_diag;
			col_diag.Resizable = true;
			cellrt8.Width = 300;
			
			TreeViewColumn col_tipopac = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_tipopac.Title = "Tipo Paciente";
			col_tipopac.PackStart(cellrt9, true);
			col_tipopac.AddAttribute (cellrt9, "text", 10); // la siguiente columna será 7
			col_tipopac.SortColumnId = (int) Col_ocupacion.col_tipopac;
			//col_tipopac.Resizable = true;
			//cellrt8.Width = 300;
			
			TreeViewColumn col_asegu_empresa = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_asegu_empresa.Title = "Empresa/Aseguradora";
			col_asegu_empresa.PackStart(cellrt10, true);
			col_asegu_empresa.AddAttribute (cellrt10, "text", 11); // la siguiente columna será 7
			col_asegu_empresa.SortColumnId = (int) Col_ocupacion.col_asegu_empresa;
			
			lista_ocupacion.AppendColumn(col_nombre);
			lista_ocupacion.AppendColumn(col_folio);
			lista_ocupacion.AppendColumn(col_pid);
			lista_ocupacion.AppendColumn(col_fecha);
			lista_ocupacion.AppendColumn(col_saldo);
			lista_ocupacion.AppendColumn(col_abono);
			lista_ocupacion.AppendColumn(col_paga);
			lista_ocupacion.AppendColumn(col_medico);
			lista_ocupacion.AppendColumn(col_cuarto);
			lista_ocupacion.AppendColumn(col_diag);
			lista_ocupacion.AppendColumn(col_tipopac);
			lista_ocupacion.AppendColumn(col_asegu_empresa);
		}
		
		enum Col_ocupacion
		{
			col_nombre,
			col_folio,
			col_pid,
			col_fecha,
			col_saldo,
			col_abono,
			col_paga,
			col_medico,
			col_cuarto,
			col_diag,
			col_tipopac,
			col_asegu_empresa
		}
		
		void llenando_lista_de_ocupacion()
		{
			string descri_empresa_aseguradora = "";
			treeViewEngineocupacion.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			int totaldepaciente = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText ="SELECT DISTINCT(hscmty_erp_movcargos.folio_de_servicio),"+							
								"to_char(hscmty_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"to_char(hscmty_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
								"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
								"to_char(to_number(to_char(age('2008-01-26 13:30:39',hscmty_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('2008-01-26 01:30:39',hscmty_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
								"hscmty_erp_cobros_enca.nombre_medico_encabezado, "+
								"hscmty_erp_cobros_enca.id_medico,nombre_medico, "+
								"hscmty_erp_cobros_enca.id_medico_tratante,nombre_medico,"+
								"hscmty_erp_cobros_enca.nombre_medico_tratante,"+
								"hscmty_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
								"hscmty_erp_cobros_enca.id_empresa,descripcion_empresa,"+
								"to_char(hscmty_erp_cobros_enca.id_cuarto,'999999999') AS cuarto, "+
								"to_char(hscmty_erp_cobros_enca.total_abonos,'99999999.99') AS totabonos, "+
								"hscmty_erp_movcargos.descripcion_diagnostico_movcargos,"+
								"to_char(hscmty_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fecha_ingreso, "+
								"to_char(hscmty_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc,"+
								"hscmty_erp_cobros_enca.id_habitacion,to_char(hscmty_his_habitaciones.numero_cuarto,'999999999') AS numerocuarto,"+
								"hscmty_his_habitaciones.descripcion_cuarto,hscmty_his_habitaciones.id_tipo_admisiones AS idtipoadmisiones_habitacion,"+
								"hscmty_his_habitaciones.descripcion_cuarto_corta,"+
								"hscmty_erp_movcargos.id_tipo_paciente AS idtipopaciente, descripcion_tipo_paciente "+
								"FROM hscmty_erp_cobros_enca,hscmty_his_paciente,hscmty_erp_movcargos,hscmty_his_tipo_pacientes,hscmty_aseguradoras,"+
								"hscmty_his_habitaciones,"+
								"hscmty_empresas,hscmty_his_medicos "+
								"WHERE hscmty_erp_cobros_enca.pid_paciente = hscmty_his_paciente.pid_paciente "+
								"AND hscmty_erp_movcargos.folio_de_servicio = hscmty_erp_cobros_enca.folio_de_servicio "+
								"AND hscmty_erp_cobros_enca.id_aseguradora = hscmty_aseguradoras.id_aseguradora "+
								"AND hscmty_erp_movcargos.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente "+
								"AND hscmty_his_paciente.id_empresa = hscmty_empresas.id_empresa "+
								//"AND hscmty_his_medicos.id_medico = hscmty_erp_cobros_enca.id_medico "+
								"AND hscmty_his_medicos.id_medico = hscmty_erp_cobros_enca.id_medico_tratante "+
								"AND hscmty_erp_cobros_enca.reservacion = 'false' "+
								"AND hscmty_erp_cobros_enca.alta_paciente = 'false' "+
								"AND hscmty_erp_cobros_enca.cancelado = 'false' "+
								"AND hscmty_erp_movcargos.id_tipo_admisiones > '16' "+
								"AND hscmty_erp_cobros_enca.id_habitacion = hscmty_his_habitaciones.id_habitacion "+
								"ORDER BY nombre_completo ;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				string foliodeservicio;
				saldos = 0;
				decimal abonomuestra = 0;
				totabono = 0;
				totcuenta = 0;
				sumacuenta = 0;
				abono = 0;
				while (lector.Read()){
					foliodeservicio = (string) lector["foliodeatencion"];
					totaldepaciente += 1;
					
					if (this.checkbutton_agregar_monto.Active == true){
						NpgsqlConnection conexion1;
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	        			// Verifica que la base de datos este conectada
						try{
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
							comando1.CommandText ="SELECT to_char(folio_de_servicio,'9999999999') AS foliodeatencion,"+
								"to_char(sum(cantidad_aplicada),'9999999999.99') AS totaldeproductos,"+
								"to_char(sum(precio_producto * cantidad_aplicada),'9999999999.99') AS totalpreciopublico,"+
								"to_char(sum(precio_costo_unitario * cantidad_aplicada),'9999999999.99') AS totalpreciocosto "+
								"FROM hscmty_erp_cobros_deta "+
								"WHERE eliminado = 'false' "+
								"AND folio_de_servicio = '"+foliodeservicio+"' "+
								"GROUP BY folio_de_servicio; ";
							NpgsqlDataReader lector1 = comando1.ExecuteReader ();
							totcuenta = 0;
							saldos = 0;
							if(lector1.Read()){
								totcuenta = decimal.Parse((string) lector1["totalpreciopublico"]);
								sumacuenta += decimal.Parse((string) lector1["totalpreciopublico"]);
								entry_totalsaldos.Text = sumacuenta.ToString();
								abono = decimal.Parse((string) lector["totabonos"]);
								saldos = totcuenta - abono;
							}
						}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
						}
						conexion1.Close ();
					}else{
						totcuenta = 0;
						sumacuenta = 0;
						abono = 0 ;
						saldos = 0;
						entry_totalsaldos.Text = sumacuenta.ToString();
					}
					if (this.checkbutton_impr_todo_proce.Active == true){
						totabono += decimal.Parse((string) lector["totabonos"]);
						entry_totalabonos.Text = totabono.ToString();
						abono = decimal.Parse((string) lector["totabonos"]);
						if (this.checkbutton_agregar_monto.Active == false){
							saldos = 0;
						}else{
							saldos = totcuenta - abono;
						}
					}else{
						abonomuestra = 0 ;
						abono = 0;
						saldos = 0;
						totabono = 0 ;
						entry_totalabonos.Text = totabono.ToString();
					}
					
					if((int) lector ["id_aseguradora"] > 1){
						descri_empresa_aseguradora =  (string) lector["descripcion_aseguradora"];
					}else{
						descri_empresa_aseguradora =  (string) lector["descripcion_empresa"];						
					}
					idcuarto = (string) lector["numerocuarto"]+"("+(string) lector["descripcion_cuarto_corta"]+")";
					treeViewEngineocupacion.AppendValues ((string) lector["nombre_completo"],//0
													(string) lector["foliodeatencion"],//1
													(string) lector["pidpaciente"],//2
													(string) lector["fecha_ingreso"],//2
													totcuenta.ToString(),//3
													abono.ToString(),//4
													saldos.ToString(),
													(string) lector["nombre_medico_tratante"],
													idcuarto.Trim(),
													//(string) lector["cuarto"]);
													(string) lector["descripcion_diagnostico_movcargos"],
													(string) lector["descripcion_tipo_paciente"],
													descri_empresa_aseguradora);
				}
				this.entry_total_de_pacientes.Text = totaldepaciente.ToString().Trim();
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_checkbutton_agregar_monto_clicked(object sender, EventArgs args)
		{
			llenando_lista_de_ocupacion();
		}
		
		void imprime_reporte(object sender, EventArgs args)
		{	
			titulo = "REPORTE DE OCUPACION";
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
		
		void ComponerPagina (object ContextoImp, object trabajoImpresion)
		{	
			ContextoImp.BeginPage("Pagina 1");
			ContextoImp.Rotate(90);
			imprime_rpt_ocupacion(ContextoImp,trabajoImpresion);
			ContextoImp.ShowPage();
		}
		
///////////////////////////////REPORTE DE OCUPACION/////////////////////////////////////////////////////////
///////////////////////////////REPORTE DE OCUPACION/////////////////////////////////////////////////////////
		void imprime_rpt_ocupacion(object ContextoImp, object trabajoImpresion)
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
					ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
					ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
					ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
					
					ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
					ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
					ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
					
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
					if(tomovalor1.Length > 25){
					tomovalor1 = tomovalor1.Substring(0,25); 
					}
					ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
					//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
					fila -= 08;
					//contador+=1;
					contadorprocedimientos += 0;
					salto_pagina(ContextoImp,trabajoImpresion);
					
					while (this.treeViewEngineocupacion.IterNext(ref iter)){
						Gnome.Print.Setfont (ContextoImp, fuente6);
						ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
						ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
						ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
						
						ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
						ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
						ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
						
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
						if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
						}
						ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
							
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
				ContextoImp.MoveTo(219.5,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(220,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(295.5,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(296,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(384.5,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(385,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(459.5,fila);				ContextoImp.Show(totabono.ToString("C"));
				ContextoImp.MoveTo(460,fila);				ContextoImp.Show(totabono.ToString("C"));
				//contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion);				
		}
		
		void imprime_encabezado(object ContextoImp, object trabajoImpresion)
		{
	 		// Cambiar la fuente
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
		
		void imprime_titulo(object ContextoImp, object trabajoImpresion)
		{	
			Gnome.Print.Setfont(ContextoImp,fuente9);
			ContextoImp.MoveTo(55.5, -65);					ContextoImp.Show("Pid"); //| Fecha | Nº Atencion | Paciente | SubTotal al 15 | SubTotal al 0 | IVA | SubTotal Deducible | Coaseguro | Total | Hono. Medico");
			ContextoImp.MoveTo(56, -65);					ContextoImp.Show("Pid");
			
			ContextoImp.MoveTo(80.5, -65);					ContextoImp.Show("Folio");
			ContextoImp.MoveTo(81, -65);					ContextoImp.Show("Folio");//80,-70
			
			ContextoImp.MoveTo(105.5, -65);					ContextoImp.Show("F. Ingreso");
			ContextoImp.MoveTo(106, -65);					ContextoImp.Show("F. Ingreso");
			
			ContextoImp.MoveTo(160.5, -65);					ContextoImp.Show("Nombre");//120,-70
			ContextoImp.MoveTo(161, -65);					ContextoImp.Show("Nombre");//120,-70
			
			ContextoImp.MoveTo(270.5, -65);					ContextoImp.Show("No. Hab.");//170,-70
			ContextoImp.MoveTo(271, -65);					ContextoImp.Show("No. Hab.");
			
			ContextoImp.MoveTo(325.5, -65);					ContextoImp.Show("Saldo");
			ContextoImp.MoveTo(326, -65);					ContextoImp.Show("Saldo");//360,-70
			
			ContextoImp.MoveTo(365.5, -65);					ContextoImp.Show("Abono");
			ContextoImp.MoveTo(366, -65);					ContextoImp.Show("Abono");//
			
			ContextoImp.MoveTo(415.5, -65);					ContextoImp.Show("S. Pend.");
			ContextoImp.MoveTo(416, -65);					ContextoImp.Show("S. Pend.");//360,-70
			
			ContextoImp.MoveTo(460.5, -65);					ContextoImp.Show("Medico");
			ContextoImp.MoveTo(461, -65);					ContextoImp.Show("Medico");//420,-70
			
			ContextoImp.MoveTo(540.5, -65);					ContextoImp.Show("Diagnostico");
			ContextoImp.MoveTo(541, -65);					ContextoImp.Show("Diagnostico");//420,-70
			
			ContextoImp.MoveTo(660.5, -65);					ContextoImp.Show("T. Paciente");
			ContextoImp.MoveTo(661, -65);					ContextoImp.Show("T. Paciente");
			
			ContextoImp.MoveTo(715.5, -65);					ContextoImp.Show("Aseg./Empresa");
			ContextoImp.MoveTo(716, -65);					ContextoImp.Show("Aseg./Empresa");
		} 
		
		void salto_pagina(object ContextoImp, object trabajoImpresion)
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