// created on 29/01/2008 at 10:11 a
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

namespace osiris
{
	public class reporte_pacientes_con_alta
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rang_fech_pac_sin_alta;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.CheckButton  checkbutton_impr_todo_proce;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_agregar_monto;
		[Widget] Gtk.Button button_salir;
		
		string connectionString;
        string nombrebd;
		string tiporeporte = "CONALTA";
		string titulo = "REPORTE DE PACIENTES CON ALTA";
		
		int columna = 0;
		int fila = -70;
		int contador = 1;
		int numpage = 1;
		
		string query_fechas = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
								
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
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public reporte_pacientes_con_alta (string _nombrebd_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			Glade.XML  gxml = new Glade.XML  (null, "registro_admision.glade", "rang_fech_pac_sin_alta", null);
			gxml.Autoconnect  (this);	
			rang_fech_pac_sin_alta.Show();
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.Editable = false;
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
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void imprime_reporte(object sender, EventArgs args)
		{	
			if (this.checkbutton_impr_todo_proce.Active == true) { 
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
			}else{
				rango1 = entry_dia1.Text+"/"+entry_mes1.Text+"/"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"/"+entry_mes2.Text+"/"+entry_ano2.Text;
				query_fechas = "AND to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
								"AND to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";
			}
			rang_fech_pac_sin_alta.Destroy();
			titulo = "REPORTE DE PACIENTES CON ALTA";
					
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
			imprime_rpt_pac_sin_alta(ContextoImp,trabajoImpresion);
			ContextoImp.ShowPage();
		}
		
///////////////////////////////REPORTE DE PACIENTES CON ALTA/////////////////////////////////////////////////////
///////////////////////////////REPORTE DE PACIENTES CON ALTA/////////////////////////////////////////////////////
		
		void imprime_rpt_pac_sin_alta(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	// asigna el numero de folio de ingreso de paciente (FOLIO)
				if (this.checkbutton_agregar_monto.Active == false)
				{
					comando.CommandText ="SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),"+
								"to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
								"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
								"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
								"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
								"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
								"osiris_erp_movcargos.descripcion_diagnostico_movcargos,"+
								"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi') AS fecha_alta "+
								"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_pacientes, osiris_aseguradoras,osiris_empresas "+
								"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
								"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+
								" "+query_fechas+" "+
								"AND osiris_erp_cobros_enca.reservacion = 'false' "+
								"AND osiris_erp_cobros_enca.alta_paciente = 'true' "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' ;";
				}
				else
				{
					comando.CommandText ="SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
								"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
								"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
								"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
								"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
								"osiris_erp_movcargos.descripcion_diagnostico_movcargos, "+
								"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, descripcion_tipo_paciente, "+
								"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora, "+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa, "+
								"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi') AS fecha_alta, "+
								"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fecha_ingreso, "+
								"to_char(sum(osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totaldeproductos, "+
								"to_char(sum(osiris_erp_cobros_deta.precio_producto * osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totalpreciopublico, "+
								"to_char(sum(osiris_erp_cobros_deta.precio_costo_unitario * osiris_erp_cobros_deta.cantidad_aplicada),'9999999999.99') AS totalpreciocosto "+
								"FROM osiris_erp_movcargos,osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_cobros_deta,osiris_his_tipo_pacientes,osiris_aseguradoras,osiris_empresas "+
								"WHERE osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente "+
								"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
								"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
								"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
								"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_cobros_enca.reservacion = 'false' "+
								" "+query_fechas+" "+
								"AND osiris_erp_cobros_enca.alta_paciente = 'true' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
								"GROUP BY osiris_erp_movcargos.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente, "+
								"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999'), "+
								"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99'), "+
								"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi'), "+
								"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi'), "+
								"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,osiris_erp_movcargos.descripcion_diagnostico_movcargos, "+
								"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora, "+
								"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa;";
				}
				//Console.WriteLine(comando.CommandText.ToString());			
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				string tomovalor1 = "";
				int pidpaciente = 0;
				int contadorprocedimientos = 0;
				decimal total = 0;
				fila=-75;
				
				imprime_encabezado(ContextoImp,trabajoImpresion);
				while(lector.Read()){
					ContextoImp.MoveTo(87, fila);					ContextoImp.Show((string)lector ["pidpaciente"]);
					ContextoImp.MoveTo(110, fila);					ContextoImp.Show((string)lector ["foliodeatencion"]);
					tomovalor1 = (string) lector["nombre_completo"];
					if(tomovalor1.Length > 34){
						tomovalor1 = tomovalor1.Substring(0,34); 
					}
					ContextoImp.MoveTo(150,fila);					ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(314,fila);					ContextoImp.Show((string)lector ["edad"]);
					
					tomovalor1 = (string) lector["nombre_medico_encabezado"];
					if(tomovalor1.Length > 29){
						tomovalor1 = tomovalor1.Substring(0,29); 
					}
					ContextoImp.MoveTo(332,fila);					ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) lector["descripcion_diagnostico_movcargos"];
					if(tomovalor1.Length > 32){
						tomovalor1 = tomovalor1.Substring(0,32); 
					}
					ContextoImp.MoveTo(466,fila);					ContextoImp.Show(tomovalor1);
										
					if((int) lector ["id_aseguradora"] > 1){
						tomovalor1 = (string) lector["descripcion_aseguradora"];
						if(tomovalor1.Length > 16){
							tomovalor1 = tomovalor1.Substring(0,16); 
						}					
					}else{
						tomovalor1 = (string) lector["descripcion_empresa"];
						if(tomovalor1.Length > 16){
						tomovalor1 = tomovalor1.Substring(0,16); 
						}
					}
					ContextoImp.MoveTo(607,fila);					ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(685,fila);					ContextoImp.Show((string)lector ["fecha_alta"]);
					
					if (this.checkbutton_agregar_monto.Active == true){
						ContextoImp.MoveTo(745, fila);	ContextoImp.Show((string)lector ["totalpreciopublico"]);
						total += decimal.Parse((string) lector["totalpreciopublico"]);
					}
					fila-=10;
					contador+=1;
					contadorprocedimientos += 1;
					salto_pagina(ContextoImp,trabajoImpresion);					
				}
				ContextoImp.MoveTo(605,fila);				ContextoImp.Show ("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(605,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(715,fila);				ContextoImp.Show("TOTAL" );
				ContextoImp.MoveTo(715,fila);				ContextoImp.Show("TOTAL" );
				ContextoImp.MoveTo(745,fila);				ContextoImp.Show(total.ToString("C"));
				ContextoImp.MoveTo(745,fila);				ContextoImp.Show(total.ToString("C"));
				contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion);				
				}
			catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente6);
			
			ContextoImp.MoveTo(69.7,-30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");//19.7, 770
			ContextoImp.MoveTo(70, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(69.7, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(70, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(69.7, -50);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(70, -50);			ContextoImp.Show("Conmutador:");
			
			Gnome.Print.Setfont(ContextoImp,fuente11);
			ContextoImp.MoveTo(319.7, -40);			ContextoImp.Show(titulo);
			ContextoImp.MoveTo(320, -40);			ContextoImp.Show(titulo);
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(390, -50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy"));
			ContextoImp.MoveTo(390, -50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy"));
			if(rango1 == "" || rango2 == "") {
				ContextoImp.MoveTo(580, -50);		ContextoImp.Show("");
			}else{
				if(rango1 == rango2) {
					ContextoImp.MoveTo(580, -50);	ContextoImp.Show("FECHA: "+rango1);
				}else{
					ContextoImp.MoveTo(580, -50);	ContextoImp.Show("Rango del "+rango1+" al "+rango2);
				}
			}
			//imprimo el titulo
			imprime_titulo(ContextoImp,trabajoImpresion);
			Gnome.Print.Setfont(ContextoImp,fuente7);//RESTAURO FUENTE A TAMAÑO 7			
		}	
						
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(70.7, -65);					ContextoImp.Show("HAB-EXP."); //| Fecha | Nº Atencion | Paciente | SubTotal al 15 | SubTotal al 0 | IVA | SubTotal Deducible | Coaseguro | Total | Hono. Medico");
			ContextoImp.MoveTo(71, -65);					ContextoImp.Show("HAB-EXP.");
			
			ContextoImp.MoveTo(121,-65);					ContextoImp.Show("PACIENTE");
			ContextoImp.MoveTo(121.5,-65);					ContextoImp.Show("PACIENTE");//80,-70
			
			ContextoImp.MoveTo(310,-65);					ContextoImp.Show("EDAD ");//120,-70
			ContextoImp.MoveTo(310.5,-65);					ContextoImp.Show("EDAD ");//120,-70
			
			ContextoImp.MoveTo(337  ,-65);					ContextoImp.Show("MEDICO TRATANTE");//170,-70
			ContextoImp.MoveTo(337.5,-65);					ContextoImp.Show("MEDICO TRATANTE");
			
			ContextoImp.MoveTo(500,-65);					ContextoImp.Show("DIAGNOSTICO");  
			ContextoImp.MoveTo(500.5,-65);					ContextoImp.Show("DIAGNOSTICO");//290,-70
			
			ContextoImp.MoveTo(612,-65);					ContextoImp.Show("INSTITUCION");
			ContextoImp.MoveTo(612.5,-65);					ContextoImp.Show("INSTITUCION");//360,-70
			
			ContextoImp.MoveTo(680,-65);					ContextoImp.Show("ALTA");
			ContextoImp.MoveTo(680.5,-65);					ContextoImp.Show("ALTA");//
			
			
			ContextoImp.MoveTo(748,-65);					ContextoImp.Show("TOTALES");
			ContextoImp.MoveTo(748.5,-65);					ContextoImp.Show("TOTALES");//420,-70
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(70, -66);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
			} 
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			if (contador > 50 )
	        {
	        	numpage +=1;        	contador=1;	
	        	fila=-75;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion);
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