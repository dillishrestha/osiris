//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
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
// Programa		:
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
	public class reporte_porcedimientos_cerrados
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
        string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string tiporeporte = "CERRADOS";
		string titulo = "REPORTE DE PROCEDIMIENTOS CERRADOS";
				
		string query_fechas = " ";
		string query_cliente = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_porcedimientos_cerrados (string rango1_,string rango2_,string query_fechas_,string nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_fechas = query_fechas_;
			query_cliente = query_cliente_;
			tiporeporte = tiporeporte_;
			rango1 = rango1_;
			rango2 = rango2_;
			orden = orden_;
			
			if(tiporeporte == "CERRADOS") { titulo = "REPORTE DE PROCEDIMIENTOS CERRADOS"; }
			print = new PrintOperation ();
			print.JobName = titulo;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);	
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
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			/*
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
	
				comando.CommandText ="SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"osiris_erp_cobros_enca.pid_paciente,cerrado, alta_paciente, "+
				            	"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
				            	"osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mm') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mm') AS fecha_egreso,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_cerrado,'dd-MM-yyyy HH24:mm') AS fechahoracerrado,"+
				            	"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"descripcion_tipo_paciente,"+
            					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
				            	"to_char(osiris_erp_cobros_enca.total_procedimiento + osiris_erp_cobros_enca.honorario_medico,'999999999.99') AS totalprocedimiento,"+
				            	"to_char(osiris_erp_cobros_enca.contador_cerrados,'9999999') AS contadorcerrado "+
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_pacientes, "+
				            	"osiris_aseguradoras,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+ 
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_his_paciente.id_empresa = osiris_empresas.id_empresa "+ 
								" "+query_fechas+" "+
								"AND osiris_erp_cobros_enca.reservacion = 'false' "+
								"AND osiris_erp_cobros_enca.cerrado = 'true' "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
								"AND osiris_erp_cobros_enca.id_aseguradora != '17'; ";
								
				Console.WriteLine(comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				
				string tomovalor1 = "";
				string tomovalor2 = "";
				string tomonombre = "";
				//int id_tipopaciente = 0;
				int pidpaciente = 0;
				decimal total = 0;
            	int contadorprocedimientos = 0;
				fila=-75;
				
				if(lector.Read()){	
					pidpaciente = (int) lector["pid_paciente"];
					//id_tipopaciente = (int) lector ["idtipopaciente"];
					
					if((int) lector ["id_aseguradora"] > 1){
						tomovalor1 = (string) lector["descripcion_aseguradora"];
					}else{
						tomovalor1 = (string) lector["descripcion_empresa"];
					}
					
					if(tomovalor1.Length > 37){
						tomovalor1 = tomovalor1.Substring(0,37);
					}
					
					tomovalor2 = (string) lector["fechahoracerrado"];
					
					tomonombre = (string)lector ["nombre_completo"];
					if(tomonombre.Length > 36 ){
						tomonombre = tomonombre.Substring(0,36);
					}
					
			       	imprime_encabezado(ContextoImp,trabajoImpresion,"");			       	
			       	ContextoImp.MoveTo(71, fila);					ContextoImp.Show((string)lector ["foliodeatencion"]);
					ContextoImp.MoveTo(124,fila);					ContextoImp.Show(((int) lector["pid_paciente"]).ToString());//80,-70
					ContextoImp.MoveTo(145,fila);					ContextoImp.Show(tomonombre);//80,-70
					ContextoImp.MoveTo(314,fila);					ContextoImp.Show((string)lector ["descripcion_tipo_paciente"]);
					ContextoImp.MoveTo(385,fila);					ContextoImp.Show(tomovalor1);
					ContextoImp.MoveTo(545,fila);					ContextoImp.Show((string)lector ["fecha_ingreso"]);
					ContextoImp.MoveTo(685,fila);					ContextoImp.Show(tomovalor2);
					ContextoImp.MoveTo(610,fila);					ContextoImp.Show((string) lector["fecha_egreso"]);
					ContextoImp.MoveTo(750,fila);					ContextoImp.Show(float.Parse((string)lector ["totalprocedimiento"]).ToString("C"));
					fila-=10;
					contador+=1;
					total += decimal.Parse((string) lector["totalprocedimiento"]);
            		contadorprocedimientos += 1;
					
					while(lector.Read())
					{
						if(pidpaciente != (int) lector["pid_paciente"])
						{
							pidpaciente = (int) lector["pid_paciente"];
							//id_tipopaciente = (int) lector ["idtipopaciente"];
							
							if((int) lector ["id_aseguradora"] > 1){
								tomovalor1 = (string) lector["descripcion_aseguradora"];
							}else{tomovalor1 = (string) lector["descripcion_empresa"];}
							
							if(tomovalor1.Length > 37) {tomovalor1 = tomovalor1.Substring(0,37); } 
							 
					       	tomovalor2 = (string)lector ["fechahoracerrado"];
					       	
					       	tomonombre = (string)lector ["nombre_completo"];
							if(tomonombre.Length > 36 )	{tomonombre = tomonombre.Substring(0,36); }
					       	
					       	ContextoImp.MoveTo(71, fila);					ContextoImp.Show((string)lector ["foliodeatencion"]);
							ContextoImp.MoveTo(124,fila);					ContextoImp.Show(((int) lector["pid_paciente"]).ToString());//80,-70
							ContextoImp.MoveTo(145,fila);					ContextoImp.Show(tomonombre);//80,-70
							ContextoImp.MoveTo(314,fila);					ContextoImp.Show((string)lector ["descripcion_tipo_paciente"]);
							ContextoImp.MoveTo(385,fila);					ContextoImp.Show(tomovalor1);
							ContextoImp.MoveTo(545,fila);					ContextoImp.Show((string)lector ["fecha_ingreso"]);
							ContextoImp.MoveTo(685,fila);					ContextoImp.Show(tomovalor2);
							ContextoImp.MoveTo(610,fila);					ContextoImp.Show((string) lector["fecha_egreso"]);
							ContextoImp.MoveTo(750,fila);					ContextoImp.Show(float.Parse((string)lector ["totalprocedimiento"]).ToString("C"));
							fila-=10;
							contador+=1;
							total += decimal.Parse((string) lector["totalprocedimiento"]);
            				contadorprocedimientos += 1;
							salto_pagina(ContextoImp,trabajoImpresion,"");
						}
					}
					ContextoImp.MoveTo(610,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
					ContextoImp.MoveTo(610,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
					ContextoImp.MoveTo(685,fila);				ContextoImp.Show("TOTAL" );
					ContextoImp.MoveTo(685,fila);				ContextoImp.Show("TOTAL" );
					ContextoImp.MoveTo(750,fila);				ContextoImp.Show(total.ToString("C"));
					ContextoImp.MoveTo(750,fila);				ContextoImp.Show(total.ToString("C"));
					//contador+=1;
					//salto_pagina(ContextoImp,trabajoImpresion);
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			*/
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}   
}