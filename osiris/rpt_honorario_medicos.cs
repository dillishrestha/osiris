 // created on 21/05/2007 at 05:28 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_proc_cobranza.cs
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_honorario_med
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
		int PidPaciente = 0;
		int folioservicio = 0;
		int id_tipopaciente = 0;
		string nombrebd;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		string tipo_paciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		
		string honorario_med; 
		string numfactu;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_honorario_med(int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_, string honorario_med_, string numfactu_)
		{		
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			honorario_med = honorario_med_; 
			numfactu = numfactu_;			
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reporte Honorarios Medicos";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
      	
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
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{   
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			/*
			filas=635;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de dato s este conectada
			try{
	 			conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	        	        	  
				comando.CommandText ="SELECT nombre1_medico,nombre2_medico,apellido_paterno_medico, "+
	           						"apellido_materno_medico,monto_del_abono "+
									"FROM osiris_his_medicos,osiris_erp_honorarios_medicos "+
									"WHERE "+
									"osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico "+
									"AND osiris_erp_honorarios_medicos.eliminado = false "+
									"AND osiris_erp_honorarios_medicos.folio_de_servicio = '"+folioservicio+"' ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query honorarios: "+comando.CommandText.ToString());
				ContextoImp.BeginPage("Pagina 1");
			
				////DATOS DE PRODUCTOS
				imprime_encabezado(ContextoImp,trabajoImpresion);
				//genera_tabla(ContextoImp,trabajoImpresion);
				contador+=1;
				imprime_titulo(ContextoImp,trabajoImpresion,"HONORARIOS MEDICOS");
				contador+=1;
				string nombremedico;
				decimal totalhono = 0;
				while (lector.Read())///AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
				{	
					//ContextoImp.BeginPage("Pagina 1");
					nombremedico = (string) lector["nombre1_medico"]+" "+(string) lector["nombre2_medico"]+" "+
								(string) lector["apellido_paterno_medico"]+" "+(string) lector["apellido_materno_medico"];
					imprime_subtitulo(ContextoImp,trabajoImpresion,nombremedico,(decimal) lector["monto_del_abono"]);
					contador+=1;
					totalhono = totalhono + (decimal) lector["monto_del_abono"];
				}
				filas-=10;
				ContextoImp.MoveTo(407, filas);			ContextoImp.Show("TOTAL   "+ totalhono.ToString("C"));//625
				ContextoImp.MoveTo(407.6, filas);		ContextoImp.Show("TOTAL   "+ totalhono.ToString("C"));//625
				ContextoImp.ShowPage();
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
			
			*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
	
/////////////////////////////////////////////////////////////////////////////////////////////////////
 	public class rpt_honorario_med_fecha
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		int contadorhonorario;
		int columna = 0;	
		int contador = 1;
				
		string query_fechas = " ";
		string query_medico = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		string tiporeporte = "";
				
		//Declaracion de ventana de error 
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_honorario_med_fecha(string rango1_,string rango2_,string query_fechas_,string _nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_medico_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_fechas = query_fechas_;
			query_medico = query_medico_;
			tiporeporte = tiporeporte_;
			rango1 = rango1_;
			rango2 = rango2_;
			orden = orden_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reporte Honorarios Medicos";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
					
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
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{   
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			/*
			ContextoImp.BeginPage("Pagina 1");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//Verifica que la base de dato s este conectada
			try{
	 			conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	        	        	  
				comando.CommandText ="SELECT "+
				"to_char(osiris_erp_factura_enca.numero_factura,'9999999999') AS numerofactura, to_char(fecha_factura,'dd-MM-yyyy') AS fecha_de_facturacion, to_char(osiris_erp_honorarios_medicos.folio_de_servicio,'999999') AS folioservicio, "+
				"osiris_erp_honorarios_medicos.id_medico AS idmedico, "+  
				"to_char(monto_del_abono,'9999999.99') AS montodelabono, "+
				"to_char(fechahora_abono,'dd-MM-yyyy') AS fechaabono, "+
				"to_char(fecha_pago,'dd-MM-yyyy') AS fechapago, "+ 
				"nombre_medico,descripcion_especialidad, "+
				"osiris_his_paciente.nombre1_paciente || ' ' || "+  
				"osiris_his_paciente.nombre2_paciente || ' ' || "+
				"osiris_his_paciente.apellido_paterno_paciente || ' ' || "+
				"osiris_his_paciente.apellido_materno_paciente AS nombre_paciente "+
				"FROM osiris_erp_honorarios_medicos,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_erp_factura_enca,osiris_erp_cobros_enca,osiris_his_paciente "+
				"WHERE  osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico "+
				"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
				"AND osiris_erp_honorarios_medicos.eliminado = 'false' "+
				"AND osiris_erp_factura_enca.numero_factura = osiris_erp_honorarios_medicos.numero_factura "+
				"AND osiris_erp_cobros_enca.numero_factura =osiris_erp_honorarios_medicos.numero_factura "+
				query_medico+
				"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+query_fechas+" "+orden+";";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				Console.WriteLine(comando.CommandText.ToString());
				ContextoImp.BeginPage("Pagina 1");
			
				////DATOS DE PRODUCTOS
				imprime_encabezado(ContextoImp,trabajoImpresion);
				//genera_tabla(ContextoImp,trabajoImpresion);
				contador = 1;				
				int iddoctores;
				Gnome.Print.Setfont (ContextoImp, fuente7);
				decimal total_honorario_medico = 0;
				string tomovalor1 = "";				
				if (lector.Read()){
																		 
					iddoctores = (int) lector["idmedico"];
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
					ContextoImp.MoveTo(24, fila);			ContextoImp.Show((string) lector["folioservicio"]);	      			
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
					ContextoImp.MoveTo(50, fila);			ContextoImp.Show((string) lector["numerofactura"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       						
					ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["fecha_de_facturacion"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);	      			
					tomovalor1 = (string) lector["nombre_paciente"];
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(145, fila);			ContextoImp.Show(tomovalor1);
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);					
					
					if ((string) lector ["fechapago"] != "01-01-2000" ){
						ContextoImp.MoveTo(510, fila);			ContextoImp.Show((string) lector["fechapago"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
					}
					
					ContextoImp.MoveTo(425, fila);			ContextoImp.Show((string) decimal.Parse((string) lector["montodelabono"]).ToString("C"));
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					fila-=10;
					total_honorario_medico += decimal.Parse((string) lector["montodelabono"]);
					contador+=1;		       		    		       		    
		       		   		       		
					while (lector.Read()){ 
						if (iddoctores != (int) lector["idmedico"]){ 
							salto_pagina(ContextoImp,trabajoImpresion,contador);
							ContextoImp.MoveTo(20, fila+9);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
																"______________________________________________");
							fila-=10;
							contador+=1;
							ContextoImp.MoveTo(340,fila+10);				ContextoImp.Show("T O T A L");
							ContextoImp.MoveTo(425,fila+10);				ContextoImp.Show(total_honorario_medico.ToString("C"));
							
							fila-=10;
							contador+=1;
							iddoctores = (int) lector["idmedico"];	
							total_honorario_medico = 0; 	        			
							salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						total_honorario_medico += decimal.Parse((string) lector["montodelabono"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
						ContextoImp.MoveTo(24, fila);			ContextoImp.Show((string) lector["folioservicio"]);	      			
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
						ContextoImp.MoveTo(50, fila);			ContextoImp.Show((string) lector["numerofactura"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       						
						ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["fecha_de_facturacion"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);	      			
						
						tomovalor1 = (string) lector["nombre_paciente"];
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(145, fila);			ContextoImp.Show(tomovalor1);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						
						if ((string) lector ["fechapago"] != "01-01-2000" ){
						ContextoImp.MoveTo(510, fila);			ContextoImp.Show((string) lector["fechapago"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						ContextoImp.MoveTo(425, fila);			ContextoImp.Show((string) decimal.Parse((string) lector["montodelabono"]).ToString("C"));
						salto_pagina(ContextoImp,trabajoImpresion,contador);
				       		fila-=10;
				       		contador+=1;
					}
					ContextoImp.MoveTo(20, fila+9);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
																"______________________________________________");
					contador+=1;
					fila-=10;
					ContextoImp.MoveTo(340,fila+10);				ContextoImp.Show("T O T A L");
					ContextoImp.MoveTo(425,fila+10);				ContextoImp.Show(total_honorario_medico.ToString("C"));
				    
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
			*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}	    
}
