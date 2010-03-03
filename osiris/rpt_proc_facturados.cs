//////////////////////////////////////////////////////////////////////
// created on 31/08/2007 at 04:16 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Ing. Jesus Buentello (Ajustes , Reportes)
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
	public class reporte_porcedimientos_facturados
	{
		string connectionString;
        string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string tiporeporte = "NO FACTURADOS";
		string titulo = "REPORTE DE PROCEDIMIENTOS NO FACTURADOS";
		
		int columna = 0;
		int fila = -70;
		int contador = 1;
		int numpage = 1;
		
		string query_fechas = " ";
		string query_cliente = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		bool pagados = false;
				
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
		
		public reporte_porcedimientos_facturados (bool pagados_,string rango1_,string rango2_,string query_fechas_,string nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
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
			pagados = pagados_;
			Console.WriteLine(pagados);
 			
 			if(pagados == true) { titulo = "REPORTE DE FFACTURAS NO PAGADAS"; }

 			if(pagados == false) {
				if(tiporeporte == "NO FACTURADOS") { titulo = "REPORTE DE PROCEDIMIENTOS NO FACTURADOS"; }
				if(tiporeporte == "FACTURADOS") { titulo = "REPORTE DE PROCEDIMIENTOS FACTURADOS"; }
			}
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel) 
			{
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
			//imprime_encabezado(ContextoImp,trabajoImpresion,);
			ContextoImp.Rotate(90);
			if(tiporeporte == "NO FACTURADOS")	{	imprime_rpt_no_facturados(ContextoImp,trabajoImpresion);	}
			if(tiporeporte == "FACTURADOS")		{	imprime_rpt_facturados(ContextoImp,trabajoImpresion); 		}		
			ContextoImp.ShowPage();
		}
		
		
		//////////////////////REPORTE PARA PROCEDIMIENTOS CERRADOS Y NO FACTURADOS/////////////////////////////////////
		//////////////////////REPORTE PARA PROCEDIMIENTOS CERRADOS Y NO FACTURADOS/////////////////////////////////////
		//////////////////////REPORTE PARA PROCEDIMIENTOS CERRADOS Y NO FACTURADOS/////////////////////////////////////
		void imprime_rpt_no_facturados(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
		
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"osiris_erp_cobros_enca.pid_paciente,cerrado, alta_paciente, "+
				            	"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
				            	"osiris_empresas.descripcion_empresa,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mm') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mm') AS fecha_egreso,"+
				            	"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"descripcion_tipo_paciente,"+
            					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
				            	"to_char(osiris_erp_cobros_enca.total_procedimiento,'999999999.99') AS totalprocedimiento,"+
				            	"to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.99') AS honorariomedico "+
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
								//"AND osiris_erp_cobros_enca.cerrado = 'true' "+
								"AND osiris_erp_cobros_enca.cancelado = 'false' "+
								"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
								"AND osiris_erp_cobros_enca.id_aseguradora != '17' "+
								"AND osiris_erp_cobros_enca.facturacion = 'false' ;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				Console.WriteLine(comando.CommandText.ToString());
				
				string tomovalor1 = "";
				string tomovalor2 = "";
				string tomonombre = "";
				//int id_tipopaciente = 0;
				int pidpaciente = 0;
				decimal total = 0;
            	int contadorprocedimientos = 0;
				fila=-75;
				
				if(lector.Read())
				{	
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
					
			       	if(!(bool) lector["cerrado"]) { 
			       		tomovalor2 = "NO ESTA CERRADO!"; 
			       	}else{
			       		if(!(bool) lector["alta_paciente"]){
			       			tomovalor2 = "NO TIENE ALTA!!"; 
			       		}else{
			       			tomovalor2 = (string)lector ["fecha_egreso"];
			       		}
					}
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
					ContextoImp.MoveTo(610,fila);					ContextoImp.Show(tomovalor2);
					ContextoImp.MoveTo(690,fila);					ContextoImp.Show(float.Parse((string) lector["totalprocedimiento"]).ToString("C"));
					ContextoImp.MoveTo(760,fila);					ContextoImp.Show(float.Parse((string)lector ["honorariomedico"]).ToString("C"));
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
							
					       	if(!(bool) lector["cerrado"]) { 
					       			tomovalor2 = "NO ESTA CERRADO!"; 
					       	}else{
					       		if(!(bool) lector["alta_paciente"])	{ tomovalor2 = "NO TIENE ALTA!!"; 
					       		}else{ tomovalor2 = (string)lector ["fecha_egreso"]; }
							}
					       	
					       	tomonombre = (string)lector ["nombre_completo"];
							if(tomonombre.Length > 36 )	{tomonombre = tomonombre.Substring(0,36); }
					       	
					       	ContextoImp.MoveTo(71, fila);					ContextoImp.Show((string)lector ["foliodeatencion"]);
							ContextoImp.MoveTo(124,fila);					ContextoImp.Show(((int) lector["pid_paciente"]).ToString());//80,-70
							ContextoImp.MoveTo(145,fila);					ContextoImp.Show(tomonombre);//80,-70
							ContextoImp.MoveTo(314,fila);					ContextoImp.Show((string)lector ["descripcion_tipo_paciente"]);
							ContextoImp.MoveTo(385,fila);					ContextoImp.Show(tomovalor1);
							ContextoImp.MoveTo(545,fila);					ContextoImp.Show((string)lector ["fecha_ingreso"]);
							ContextoImp.MoveTo(610,fila);					ContextoImp.Show(tomovalor2);
							ContextoImp.MoveTo(690,fila);					ContextoImp.Show(float.Parse((string) lector["totalprocedimiento"]).ToString("C"));
							ContextoImp.MoveTo(760,fila);					ContextoImp.Show(float.Parse((string)lector ["honorariomedico"]).ToString("C"));
							fila-=10;
							contador+=1;
							total += decimal.Parse((string) lector["totalprocedimiento"]);
            				contadorprocedimientos += 1;
							salto_pagina(ContextoImp,trabajoImpresion,"");
						}
					}
					ContextoImp.MoveTo(145.7,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
					ContextoImp.MoveTo(146,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
					ContextoImp.MoveTo(610.7,fila);				ContextoImp.Show("TOTAL" );
					ContextoImp.MoveTo(611,fila);				ContextoImp.Show("TOTAL" );
					ContextoImp.MoveTo(690.7,fila);				ContextoImp.Show(total.ToString("C"));
					ContextoImp.MoveTo(691,fila);				ContextoImp.Show(total.ToString("C"));
					//contador+=1;
					//salto_pagina(ContextoImp,trabajoImpresion);
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
///////////////////////////////REPORTE PARA PROCEDIMIENTOS  FACTURADOS////////////////////////////////////////////////////////
///////////////////////////////REPORTE PARA PROCEDIMIENTOS  FACTURADOS/////////////////////////////////////////////////////
		void imprime_rpt_facturados(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
            string query_facturados = "SELECT osiris_erp_factura_enca.id_cliente,descripcion_cliente,"+
									"osiris_erp_factura_enca.numero_factura,osiris_erp_factura_enca.cancelado, "+ 
									"to_char(osiris_erp_factura_enca.fecha_factura, 'dd-MM-yyyy') AS fechacreacion, "+
									"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
									"to_char(osiris_erp_factura_enca.deducible,'99999999.99') AS deducible, "+
									"to_char(osiris_erp_factura_enca.honorario_medico,'9999999999.99') AS honorariomedico_factura,"+
									"to_char(osiris_erp_factura_enca.sub_total_15,'99999999.99') AS subtotal_15, "+ 
									"to_char(osiris_erp_factura_enca.sub_total_0,'99999999.99') AS subtotal_0, "+
									"to_char(osiris_erp_factura_enca.iva_al_15,'99999999.99') AS ivaal_15, "+ 
									"to_char(osiris_erp_factura_enca.valor_coaseguro,'99999999.99') AS valorcoaseguro, "+ 
									"to_char(osiris_erp_cobros_enca.folio_de_servicio, '999999') AS folioservicio, "+
									"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'dd-MM-yyyy') AS fechacreacionproc, "+
									"to_char(osiris_erp_cobros_enca.subtotal15,'99999999.99') AS sub15proc, "+
									"to_char(osiris_erp_cobros_enca.subtotal0,'99999999.99') AS sub0proc, "+
									"(osiris_erp_cobros_enca.subtotal15)*.15 AS ivaproc, "+
									"to_char(osiris_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc "+
									"FROM "+
									"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_factura_enca "+
									"WHERE "+
									"osiris_erp_factura_enca.numero_factura = osiris_erp_cobros_enca.numero_factura  "+
									"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente ";
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	//Console.WriteLine(query_fechas);
               	if(orden == "CLIENTE") {
					comando.CommandText = query_facturados+" "+query_fechas+" "+query_cliente+" "+"AND osiris_erp_factura_enca.pagada = 'false' "+
						" ORDER BY osiris_erp_factura_enca.id_cliente,osiris_erp_factura_enca.numero_factura ";
				}
				if(orden == "FECHA"){
					comando.CommandText = query_facturados+" "+query_fechas+" "+query_cliente+" "+
						" ORDER BY osiris_erp_factura_enca.fechahora_creacion_factura,osiris_erp_factura_enca.id_cliente ";
				}
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
										
					string nombre = "";
					string folio = "";
					int numerofactura = 0;
					int id_cliente = 0;
					string fecha = "";
					string fechaproc = "";
					decimal sub15_fact = 0;
					decimal sub0_fact = 0;
					decimal iva_fact = 0;
					decimal deducible = 0;
					decimal coaseguro = 0;
					decimal sub15_proc = 0;
					decimal sub0_proc = 0;
					decimal iva_proc = 0;
					decimal honomed = 0;
					decimal totaldeproc = 0;
					decimal totaldefactura = 0;
					decimal totalfacturas = 0;
					int contadorfacturas = 0;
					fila=-85;
				
	       			int contadorproc = 0;
					bool primeralinea = true;
					bool masdeunfolio = false;
					
					while(lector.Read())
					{
						//Console.WriteLine("FACT "+numerofactura.ToString()+" Nº AT. "+folio);
						//Console.WriteLine("contadorproc"+contadorproc.ToString());
						//Console.WriteLine("primeralinea"+primeralinea.ToString());
						if(contadorproc >= 1) { primeralinea = false; }//Console.WriteLine(primeralinea.ToString()); } 
						
						if(primeralinea == true) {
							//Console.WriteLine("Tomo valor de primer factura");
							id_cliente = (int) lector["id_cliente"];
							numerofactura = (int) lector["numero_factura"];
							nombre = (string)lector ["nombre_completo"];
							if(nombre.Length > 37){
								nombre = nombre.Substring(0,37);
							}							 
							folio = (string) lector["folioservicio"];
					       	if((bool) lector["cancelado"]){
					       		folio = "CANCELADA!";
					       	}
					       	fecha = (string) lector["fechacreacion"];
					       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
					       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
					       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
					       	deducible = decimal.Parse((string) lector["deducible"]);
					       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
					       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
					       	//subtotales de procedimientos
					       	fechaproc = (string) lector["fechacreacionproc"];
					       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
					       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
					        if(sub15_proc > 0) { iva_proc = (decimal) lector["ivaproc"]; }else{ iva_proc = 0; }
					       	totaldeproc = sub15_proc+sub0_proc+iva_proc;
							totaldefactura = sub15_fact+sub0_fact+iva_fact-deducible-coaseguro;
							imprime_encabezado(ContextoImp, trabajoImpresion,(string) lector["descripcion_cliente"]);
							contadorproc +=1; //Console.WriteLine(contadorproc.ToString());
						}
						
						if(primeralinea == false){
							//Console.WriteLine("entro a determinar siguientes lineas");
							if(numerofactura == (int) lector["numero_factura"])
					       	{
					       		//Console.WriteLine("MISMA FACTURA"+" FACT "+numerofactura.ToString()+" Nº AT. "+folio);
					       		if(folio != (string) lector["folioservicio"]){	masdeunfolio = true;	//       	Console.WriteLine("el folio cambio  "+masdeunfolio.ToString());
							    }else{	masdeunfolio = false;		}//	Console.WriteLine("mismo folio "+folio.ToString()+" "+masdeunfolio.ToString()); }
			            		
			            		if(masdeunfolio == true)
			            		{
			            			//Console.WriteLine("imprimo en mas de un folio");
			            			//ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString()+"*");
			            			ContextoImp.MoveTo(117.5,fila);					ContextoImp.Show(fechaproc);
							       	ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
									ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
									ContextoImp.MoveTo(366,fila);					ContextoImp.Show(sub15_proc.ToString("C"));//170,-70
									ContextoImp.MoveTo(431,fila);					ContextoImp.Show(sub0_proc.ToString("C"));
									ContextoImp.MoveTo(496,fila);					ContextoImp.Show(iva_proc.ToString("C"));//360,-70
									ContextoImp.MoveTo(691,fila);					ContextoImp.Show(totaldeproc.ToString("C"));//560,-70
							    }
		            			
		            			numerofactura = (int) lector["numero_factura"];
								nombre = (string)lector ["nombre_completo"];
								if(nombre.Length > 37) {nombre = nombre.Substring(0,37); } 
								folio = (string) lector["folioservicio"];
						       	if((bool) lector["cancelado"]) { folio = "CANCELADA!"; }
						       	fecha = (string) lector["fechacreacion"];
						       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
						       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
						       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
						       	deducible = decimal.Parse((string) lector["deducible"]);
						       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
						       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
						       	//subtotales de procedimientos
						       	fechaproc = (string) lector["fechacreacionproc"];
						       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
						       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
						        if(sub15_proc > 0) { iva_proc = (decimal) lector["ivaproc"]; }else{ iva_proc = 0; }
						       	totaldeproc = sub15_proc+sub0_proc+iva_proc;
								totaldefactura = sub15_fact+sub0_fact+iva_fact-deducible-coaseguro;
								
								genera_columnas(ContextoImp, trabajoImpresion);
								fila-=10;							contador+=1;
								salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
							}else{
								//Console.WriteLine("OTRA FACTURA "+"FACT "+numerofactura.ToString()+" Nº AT. "+folio);
								if(masdeunfolio == true)
								{
									ContextoImp.MoveTo(117.5,fila);					ContextoImp.Show(fechaproc);
							       	ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
									ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
									ContextoImp.MoveTo(366,fila);					ContextoImp.Show(sub15_proc.ToString("C"));//170,-70
									ContextoImp.MoveTo(431,fila);					ContextoImp.Show(sub0_proc.ToString("C"));
									ContextoImp.MoveTo(496,fila);					ContextoImp.Show(iva_proc.ToString("C"));//360,-70
									ContextoImp.MoveTo(691,fila);					ContextoImp.Show(totaldeproc.ToString("C"));
									genera_columnas(ContextoImp, trabajoImpresion);
									fila-=10;							contador+=1;
									salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
									//Console.WriteLine("impresion de mas de un folio");
									ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());
									ContextoImp.MoveTo(78.5, fila);					ContextoImp.Show(numerofactura.ToString());
									ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);
									
									ContextoImp.MoveTo(192,fila);					ContextoImp.Show("                                                TOTALES");
									
									ContextoImp.MoveTo(366,fila);					ContextoImp.Show(sub15_fact.ToString("C"));//170,-70
									
									ContextoImp.MoveTo(431,fila);					ContextoImp.Show(sub0_fact.ToString("C"));
									
									ContextoImp.MoveTo(496,fila);					ContextoImp.Show(iva_fact.ToString("C"));//360,-70
									
									ContextoImp.MoveTo(561,fila);					ContextoImp.Show(deducible.ToString("C"));
									
									ContextoImp.MoveTo(626,fila);					ContextoImp.Show(coaseguro.ToString("C"));
									
									ContextoImp.MoveTo(691,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
									
									ContextoImp.MoveTo(761,fila);					ContextoImp.Show(honomed.ToString("C"));
									
									ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
								}else{
			            			//Console.WriteLine("impresion de un folio");
			            			ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());
									
									ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);
									
									ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
									
									ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
									
									ContextoImp.MoveTo(366,fila);					ContextoImp.Show(sub15_fact.ToString("C"));//170,-70
									
									ContextoImp.MoveTo(431,fila);					ContextoImp.Show(sub0_fact.ToString("C"));
									
									ContextoImp.MoveTo(496,fila);					ContextoImp.Show(iva_fact.ToString("C"));//360,-70
									
									ContextoImp.MoveTo(561,fila);					ContextoImp.Show(deducible.ToString("C"));
									
									ContextoImp.MoveTo(626,fila);					ContextoImp.Show(coaseguro.ToString("C"));
									
									ContextoImp.MoveTo(691,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
									
									ContextoImp.MoveTo(761,fila);					ContextoImp.Show(honomed.ToString("C"));
									
									ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
								}
		            			genera_columnas(ContextoImp, trabajoImpresion);
								totalfacturas += totaldefactura;
		            			contadorfacturas += 1;
		            			fila-=10;							contador+=1;
								salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
		            			
		            			masdeunfolio = false;
		            			if(id_cliente != (int) lector["id_cliente"]){
									imprime_titulo(ContextoImp, trabajoImpresion,(string) lector["descripcion_cliente"]);
									salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
									id_cliente = (int) lector["id_cliente"];
								}
								
								numerofactura = (int) lector["numero_factura"];
								nombre = (string)lector ["nombre_completo"];
								if(nombre.Length > 37) {nombre = nombre.Substring(0,37); } 
								folio = (string) lector["folioservicio"];
						       	if((bool) lector["cancelado"]) { folio = "CANCELADA!"; }
						       	fecha = (string) lector["fechacreacion"];
						       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
						       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
						       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
						       	deducible = decimal.Parse((string) lector["deducible"]);
						       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
						       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
						       	//subtotales de procedimientos
						       	fechaproc = (string) lector["fechacreacionproc"];
						       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
						       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
						        if(sub15_proc > 0) { iva_proc = (decimal) lector["ivaproc"]; }else{ iva_proc = 0; }
						       	totaldeproc = sub15_proc+sub0_proc+iva_proc;
								totaldefactura = sub15_fact+sub0_fact+iva_fact-deducible-coaseguro;
						    }
						}
					}
					ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());					
					ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);					
					ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);					
					ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);					
					ContextoImp.MoveTo(366,fila);					ContextoImp.Show(sub15_fact.ToString("C"));//170,-70					
					ContextoImp.MoveTo(431,fila);					ContextoImp.Show(sub0_fact.ToString("C"));					
					ContextoImp.MoveTo(496,fila);					ContextoImp.Show(iva_fact.ToString("C"));//360,-70					
					ContextoImp.MoveTo(561,fila);					ContextoImp.Show(deducible.ToString("C"));					
					ContextoImp.MoveTo(626,fila);					ContextoImp.Show(coaseguro.ToString("C"));					
					ContextoImp.MoveTo(691,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70					
					ContextoImp.MoveTo(761,fila);					ContextoImp.Show(honomed.ToString("C"));					
					totalfacturas += totaldefactura;
		  			contadorfacturas += 1;
		  			genera_columnas(ContextoImp, trabajoImpresion);
		  			fila-=10;							contador+=1;
					salto_pagina(ContextoImp,trabajoImpresion,"");
					ContextoImp.MoveTo(70, fila+10);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
																"_______________________________________________________________________________________________");
					ContextoImp.MoveTo(145.7,fila);				ContextoImp.Show("TOTAL DE FACTURAS "+contadorfacturas.ToString());
					ContextoImp.MoveTo(146,fila);				ContextoImp.Show("TOTAL DE FACTURAS "+contadorfacturas.ToString());
					ContextoImp.MoveTo(610.7,fila);				ContextoImp.Show("TOTAL FACTURADO" );
					ContextoImp.MoveTo(611,fila);				ContextoImp.Show("TOTAL FACTURADO" );
					ContextoImp.MoveTo(690.7,fila);				ContextoImp.Show(totalfacturas.ToString("C"));
					ContextoImp.MoveTo(691,fila);				ContextoImp.Show(totalfacturas.ToString("C"));
				
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string cliente)
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
			imprime_titulo(ContextoImp,trabajoImpresion,cliente);
			
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.Rotate(270);
			ContextoImp.MoveTo(56, 810);			ContextoImp.Show("__________________________");
			ContextoImp.MoveTo(56,70);				ContextoImp.Show("__________________________");
			///termino de dibujo de tabla
			ContextoImp.Rotate(90);//RESTAURO EL ORDEN A VERTICAL
			Gnome.Print.Setfont(ContextoImp,fuente7);//RESTAURO FUENTE A TAMAÑO 7
		}	
		
		void genera_columnas(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			//Gnome.Print.Setfont (ContextoImp, fuente12);			
			//ContextoImp.MoveTo(111,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(156.5,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(190,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(360,fila);					ContextoImp.Show("|	");	
			//ContextoImp.MoveTo(425,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(490,fila);					ContextoImp.Show("| ");
			//ContextoImp.MoveTo(555,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(620,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(685,fila);					ContextoImp.Show("|	");
			//ContextoImp.MoveTo(750,fila);					ContextoImp.Show("|	");
			//Gnome.Print.Setfont (ContextoImp, fuente7);
		}
		
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,string cliente)
		{
			if(tiporeporte == "NO FACTURADOS")
			{
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70.7, -65);					ContextoImp.Show("Nº ATENCION"); //| Fecha | Nº Atencion | Paciente | SubTotal al 15 | SubTotal al 0 | IVA | SubTotal Deducible | Coaseguro | Total | Hono. Medico");
				ContextoImp.MoveTo(71, -65);					ContextoImp.Show("Nº ATENCION");
				ContextoImp.MoveTo(119.7,-65);					ContextoImp.Show("PACIENTE");
				ContextoImp.MoveTo(120,-65);					ContextoImp.Show("PACIENTE");//80,-70
				ContextoImp.MoveTo(309.7,-65);					ContextoImp.Show("TIPO ");
				ContextoImp.MoveTo(310,-65);					ContextoImp.Show("TIPO ");//120,-70
				ContextoImp.MoveTo(379.7,-65);					ContextoImp.Show("EMPRESA");
				ContextoImp.MoveTo(380,-65);					ContextoImp.Show("EMPRESA");//170,-70
				ContextoImp.MoveTo(539.7,-65);					ContextoImp.Show("FECHA INGRESO");  
				ContextoImp.MoveTo(540,-65);					ContextoImp.Show("FECHA INGRESO");//290,-70
				ContextoImp.MoveTo(604.7,-65);					ContextoImp.Show("FECHA ENGRESO");
				ContextoImp.MoveTo(605,-65);					ContextoImp.Show("FECHA ENGRESO");//360,-70
				ContextoImp.MoveTo(679.7,-65);					ContextoImp.Show("TOTAL");
				ContextoImp.MoveTo(680,-65);					ContextoImp.Show("TOTAL");//
				ContextoImp.MoveTo(744.7,-65);					ContextoImp.Show("HONO. MEDICOS");
				ContextoImp.MoveTo(745,-65);					ContextoImp.Show("HONO. MEDICOS");//420,-70
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, -66);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
			} 
			if(tiporeporte == "FACTURADOS") 
			{
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila+10);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
				Gnome.Print.Setfont(ContextoImp,fuente10);
				ContextoImp.MoveTo(299.7, fila);				ContextoImp.Show(cliente.ToUpper());
				ContextoImp.MoveTo(300, fila);					ContextoImp.Show(cliente.ToUpper());
				contador+=1;
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
				fila-=10;
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(71.7, fila);					ContextoImp.Show("Nº FACT.");
				ContextoImp.MoveTo(72, fila);					ContextoImp.Show("Nº FACT.");
				ContextoImp.MoveTo(111.7,fila);					ContextoImp.Show("FECHA");
				ContextoImp.MoveTo(112,fila);					ContextoImp.Show("FECHA");
				ContextoImp.MoveTo(157.7,fila);					ContextoImp.Show("Nº ATEN");
				ContextoImp.MoveTo(158,fila);					ContextoImp.Show("Nº ATEN");
				ContextoImp.MoveTo(191.5,fila);					ContextoImp.Show("PACIENTE");
				ContextoImp.MoveTo(192,fila);					ContextoImp.Show("PACIENTE");
				ContextoImp.MoveTo(360.7,fila);					ContextoImp.Show("SUBTOTAL 15");
				ContextoImp.MoveTo(361,fila);					ContextoImp.Show("SUBTOTAL 15");
				ContextoImp.MoveTo(425.7,fila);					ContextoImp.Show("SUBTOTAL 0 ");  
				ContextoImp.MoveTo(426,fila);					ContextoImp.Show("SUBTOTAL 0 ");
				ContextoImp.MoveTo(490.7,fila);					ContextoImp.Show("IVA ");
				ContextoImp.MoveTo(491,fila);					ContextoImp.Show("IVA ");
				ContextoImp.MoveTo(555.7,fila);					ContextoImp.Show("DEDUCIBLE ");
				ContextoImp.MoveTo(556,fila);					ContextoImp.Show("DEDUCIBLE ");
				ContextoImp.MoveTo(620.7,fila);					ContextoImp.Show("COASEGURO ");
				ContextoImp.MoveTo(621,fila);					ContextoImp.Show("COASEGURO ");
				ContextoImp.MoveTo(685.7,fila);					ContextoImp.Show("TOTAL ");
				ContextoImp.MoveTo(686,fila);					ContextoImp.Show("TOTAL ");
				ContextoImp.MoveTo(750.7,fila);					ContextoImp.Show("HONO. MEDICO");
				ContextoImp.MoveTo(751,fila);					ContextoImp.Show("HONO. MEDICO");
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																			"_______________________________________________________________________________________________");
				fila-=10;			contador+=1;
			}
		} 
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,string cliente)
		{
			//Console.WriteLine("contador antes del if: "+contador_.ToString());
	        if (contador > 50 )
	        {
	        	numpage +=1;        	contador=1;	
	        	if(tiporeporte == "NO FACTURADOS") { fila=-75; }else{ fila =-65; }
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion,cliente);
	     	}
	       //Console.WriteLine("contador despues del if: "+contador_.ToString());
		}
	}    

///////////////////////////// REPORTE FACTURAS PAGADAS /////////////////////////////////////////////////
///////////////////////////// REPORTE FACTURAS PAGADAS /////////////////////////////////////////////////
///////////////////////////// REPORTE FACTURAS PAGADAS /////////////////////////////////////////////////

	public class reporte_facturas_pagadas
	{		
		public string connectionString;
		public string nombrebd;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string tiporeporte;
		public string titulo;
		
		public int columna = 0;
		public int fila = -70;
		public int contador = 1;
		public int numpage = 1;
		
		public string query_fechas = " ";
		public string query_cliente = " ";
		public string orden = " ";
		public string rango1 = "";
		public string rango2 = "";
		public string facturas_ = "";
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public reporte_facturas_pagadas(string rango1_,string rango2_,string query_fechas_,string nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_,string _facturas_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
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
			facturas_ = _facturas_;
			
			if(tiporeporte == "FACTURADOS") { titulo = "REPORTE FACTURAS PAGADAS"; }
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
			Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
			int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) Gnome.PrintButtons.Cancel) 
			{
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
		
			if(tiporeporte == "FACTURADOS"){imprime_rpt_pagados(ContextoImp,trabajoImpresion);	}		
			ContextoImp.ShowPage();
		}
		
		void imprime_rpt_pagados(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			string fechapago = "";
			string nombre = "";
			string folio = "";
			int numerofactura = 0;
			int id_cliente = 0;
			string fecha = "";
			string fechaproc = "";
			decimal sub15_fact = 0;
			decimal sub0_fact = 0;
			decimal iva_fact = 0;
			decimal deducible = 0;
			decimal coaseguro = 0;
			decimal sub15_proc = 0;
			decimal sub0_proc = 0;
			decimal iva_proc = 0;
			decimal honomed = 0;
			decimal totaldeproc = 0;
			decimal ttotal = 0;
			decimal totaldefactura = 0;
			decimal totalfacturas = 0;
			int contadorfacturas = 0;
			fila=-85;
			int contadorproc = 0;
			bool primeralinea = true;
			bool masdeunfolio = false;
			string query_fac_pagadas ="SELECT osiris_erp_factura_enca.id_cliente,descripcion_cliente,"+
									"osiris_erp_factura_enca.numero_factura,osiris_erp_factura_enca.cancelado, "+ 
									"to_char(osiris_erp_factura_enca.fecha_factura, 'dd-MM-yyyy') AS fechacreacion, "+
									"to_char(osiris_erp_factura_enca.fechahora_pago_factura, 'dd-MM-yyyy') AS fechapago, "+
									"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
									"to_char(osiris_erp_factura_enca.deducible,'99999999.99') AS deducible, "+
									"to_char(osiris_erp_factura_enca.honorario_medico,'9999999999.99') AS honorariomedico_factura,"+
									"to_char(osiris_erp_factura_enca.sub_total_15,'99999999.99') AS subtotal_15, "+ 
									"to_char(osiris_erp_factura_enca.sub_total_0,'99999999.99') AS subtotal_0, "+
									"to_char(osiris_erp_factura_enca.iva_al_15,'99999999.99') AS ivaal_15, "+ 
									"to_char(osiris_erp_factura_enca.valor_coaseguro,'99999999.99') AS valorcoaseguro, "+ 
									"to_char(osiris_erp_cobros_enca.folio_de_servicio, '999999') AS folioservicio, "+
									"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'dd-MM-yyyy') AS fechacreacionproc, "+
									"to_char(osiris_erp_cobros_enca.subtotal15,'99999999.99') AS sub15proc, "+
									"to_char(osiris_erp_factura_enca.coaseguro,'99999999.99') AS coaseguro, "+
   									"to_char(osiris_erp_cobros_enca.subtotal0,'99999999.99') AS sub0proc, "+
									"(osiris_erp_cobros_enca.subtotal15)*.15 AS ivaproc, "+
									"to_char(osiris_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc, "+
									"to_char(osiris_erp_cobros_enca.total_procedimiento + osiris_erp_cobros_enca.honorario_medico,'9999999999.99') AS honototal,"+
									"osiris_erp_factura_enca.pagada "+
									"FROM "+
									"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_factura_enca "+
									"WHERE "+
									"osiris_erp_factura_enca.numero_factura = osiris_erp_cobros_enca.numero_factura  "+
									"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
									facturas_+
									query_fechas+" "+
									query_cliente;

					 Console.WriteLine(query_fac_pagadas); 

				if(orden == "CLIENTE") {
					query_fac_pagadas = query_fac_pagadas+" ORDER BY osiris_erp_factura_enca.id_cliente,osiris_erp_factura_enca.numero_factura;";
				}else{
					query_fac_pagadas = query_fac_pagadas+" ORDER BY osiris_erp_factura_enca.fechahora_creacion_factura,osiris_erp_factura_enca.numero_factura;";
				}
			
			// Verifica que la base de datos este conectada
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            try{
            	conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_fac_pagadas;
					        
				Console.WriteLine("reporte "+comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				while(lector.Read()){
					if(contadorproc >= 1){
						primeralinea = false;
					 }
					 //Console.WriteLine(primeralinea.ToString()); } 
					
					if(primeralinea == true) {
						//Console.WriteLine("Tomo valor de primer factura");
						id_cliente = (int) lector["id_cliente"];
						numerofactura = (int) lector["numero_factura"];
						nombre = (string)lector ["nombre_completo"];
						if(nombre.Length > 37) {nombre = nombre.Substring(0,37); } 
						folio = (string) lector["folioservicio"];
				       	if((bool) lector["cancelado"]) { folio = "CANCELADA!"; }
				       	fecha = (string) lector["fechacreacion"];
				       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
				       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
				       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
				       	deducible = decimal.Parse((string) lector["deducible"]);
				       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
				       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
				       	//subtotales de procedimientos
				       	fechaproc = (string) lector["fechacreacionproc"];
				       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
				       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
				        if(sub15_proc > 0) { iva_proc = (decimal) lector["ivaproc"]; }else{ iva_proc = 0; }
				        
				       	totaldeproc = sub15_proc + sub0_proc + iva_proc;
						totaldefactura = (sub15_fact + sub0_fact + iva_fact) -(deducible + coaseguro);
						
						imprime_encabezado(ContextoImp, trabajoImpresion,(string) lector["descripcion_cliente"]);
						contadorproc +=1; //Console.WriteLine(contadorproc.ToString());
						fechapago = (string) lector["fechapago"];
						//ttotal = float.Parse((string) lector ["honototal"]);
						ttotal = (totaldefactura + honomed)  - (deducible + coaseguro);
					}
					
					if(primeralinea == false){
						//Console.WriteLine("entro a determinar siguientes lineas");
						if(numerofactura == (int) lector["numero_factura"])
				       	{
				       		//Console.WriteLine("MISMA FACTURA"+" FACT "+numerofactura.ToString()+" Nº AT. "+folio);
				       		if(folio != (string) lector["folioservicio"]){	masdeunfolio = true;	//       	Console.WriteLine("el folio cambio  "+masdeunfolio.ToString());
						    }else{	masdeunfolio = false;		}//	Console.WriteLine("mismo folio "+folio.ToString()+" "+masdeunfolio.ToString()); }
		            		
		            		if(masdeunfolio == true)
		            		{
		            			//Console.WriteLine("imprimo en mas de un folio");
		            			//ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString()+"*");
		            			ContextoImp.MoveTo(117.5,fila);					ContextoImp.Show(fechaproc);
						       	ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
								ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
								ContextoImp.MoveTo(366,fila);					ContextoImp.Show(fechapago);
								ContextoImp.MoveTo(431,fila);					ContextoImp.Show(totaldeproc.ToString("C"));//560,-70
								ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70

								ContextoImp.MoveTo(620,fila);					ContextoImp.Show(deducible.ToString("C"));//560,-70
								ContextoImp.MoveTo(670,fila);					ContextoImp.Show(coaseguro.ToString("C"));
						    }
	            			
	            			numerofactura = (int) lector["numero_factura"];
							nombre = (string)lector ["nombre_completo"];
							if(nombre.Length > 37) {
								nombre = nombre.Substring(0,37);
							}
							 
							folio = (string) lector["folioservicio"];
					       	if((bool) lector["cancelado"]){ 
					       		folio = "CANCELADA!";
					       	}
					       	fecha = (string) lector["fechacreacion"];
					       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
					       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
					       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
					       	deducible = decimal.Parse((string) lector["deducible"]);
					       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
					       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
					       	
					       	fechaproc = (string) lector["fechacreacionproc"];
					       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
					       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
					        if(sub15_proc > 0){
					        	iva_proc = (decimal) lector["ivaproc"];
					        }else{
					        	iva_proc = 0;
					        }
					        
					       	totaldeproc = sub15_proc + sub0_proc + iva_proc;
							totaldefactura = (sub15_fact + sub0_fact + iva_fact) - (deducible+coaseguro);
							
							fechapago = (string) lector["fechapago"];
							ttotal = (totaldefactura + honomed)  - (deducible + coaseguro);
							genera_columnas(ContextoImp, trabajoImpresion);
							fila-=10;							contador+=1;
							salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
						}else{
							if(masdeunfolio == true){
								ContextoImp.MoveTo(117.5,fila);					ContextoImp.Show(fechaproc);
						       	ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
								ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
								ContextoImp.MoveTo(366,fila);					ContextoImp.Show(fechapago);
								ContextoImp.MoveTo(431,fila);					ContextoImp.Show(totaldeproc.ToString("C"));//560,-70
								ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70
								
								ContextoImp.MoveTo(620,fila);					ContextoImp.Show(deducible.ToString("C"));//560,-70
								ContextoImp.MoveTo(670,fila);					ContextoImp.Show(coaseguro.ToString("C"));
								genera_columnas(ContextoImp, trabajoImpresion);
								fila-=10;							contador+=1;
								salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
								ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());
								ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);
								ContextoImp.MoveTo(192,fila);					ContextoImp.Show("                                                TOTALES");
								
								ContextoImp.MoveTo(366,fila);					ContextoImp.Show(fechapago);//170,-70
								ContextoImp.MoveTo(431,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
								ContextoImp.MoveTo(496,fila);					ContextoImp.Show(honomed.ToString("C"));
								ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70

								ContextoImp.MoveTo(620,fila);					ContextoImp.Show(deducible.ToString("C"));//560,-70
								ContextoImp.MoveTo(670,fila);					ContextoImp.Show(coaseguro.ToString("C"));
								ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																				"_______________________________________________________________________________________________");
							}else{
		            			ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());
								ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);
								ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
								ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
								ContextoImp.MoveTo(366,fila);					ContextoImp.Show(fechapago);//170,-70
								ContextoImp.MoveTo(431,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
								ContextoImp.MoveTo(496,fila);					ContextoImp.Show(honomed.ToString("C"));
								ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70
								ContextoImp.MoveTo(620,fila);					ContextoImp.Show(deducible.ToString("C"));//560,-70
								ContextoImp.MoveTo(670,fila);					ContextoImp.Show(coaseguro.ToString("C"));
								ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																				"_______________________________________________________________________________________________");
							}
	            			genera_columnas(ContextoImp, trabajoImpresion);
							totalfacturas += totaldefactura;
	            			contadorfacturas += 1;
	            			fila-=10;							contador+=1;
							salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
	            			
	            			masdeunfolio = false;
	            			if(id_cliente != (int) lector["id_cliente"])
							{
								imprime_titulo(ContextoImp, trabajoImpresion,(string) lector["descripcion_cliente"]);
								salto_pagina(ContextoImp,trabajoImpresion,(string) lector["descripcion_cliente"]);
								id_cliente = (int) lector["id_cliente"];
							}
							
							numerofactura = (int) lector["numero_factura"];
							nombre = (string)lector ["nombre_completo"];
							if(nombre.Length > 37) {nombre = nombre.Substring(0,37); } 
							folio = (string) lector["folioservicio"];
					       	if((bool) lector["cancelado"]) { folio = "CANCELADA!"; }
					       	fecha = (string) lector["fechacreacion"];
					       	sub15_fact = decimal.Parse((string) lector["subtotal_15"]);
					       	sub0_fact = decimal.Parse((string) lector["subtotal_0"]);
					       	iva_fact = decimal.Parse((string) lector["ivaal_15"]);
					       	deducible = decimal.Parse((string) lector["deducible"]);
					       	coaseguro = decimal.Parse((string) lector["valorcoaseguro"]);
					       	honomed = decimal.Parse((string)lector ["honorariomedico_factura"]);
					       	//subtotales de procedimientos
					       	fechaproc = (string) lector["fechacreacionproc"];
					       	sub15_proc = decimal.Parse((string) lector["sub15proc"]);
					       	sub0_proc = decimal.Parse((string) lector["sub0proc"]);
					        if(sub15_proc > 0) { iva_proc = (decimal) lector["ivaproc"]; }else{ iva_proc = 0; }
					        
					       	totaldeproc = sub15_proc + sub0_proc + iva_proc;
							totaldefactura = (sub15_fact + sub0_fact + iva_fact ) - (deducible + coaseguro);
							
							fechapago = (string) lector["fechapago"];
							//ttotal = float.Parse((string) lector ["honototal"]);
							ttotal = (totaldefactura + honomed)  - (deducible + coaseguro);
					    }
					}
				}
				ContextoImp.MoveTo(78, fila);					ContextoImp.Show(numerofactura.ToString());
				ContextoImp.MoveTo(78.5, fila);					ContextoImp.Show(numerofactura.ToString());
				ContextoImp.MoveTo(117,fila);					ContextoImp.Show(fecha);
				ContextoImp.MoveTo(117.5,fila);					ContextoImp.Show(fecha);
				ContextoImp.MoveTo(162,fila);					ContextoImp.Show(folio);
				ContextoImp.MoveTo(162.5,fila);					ContextoImp.Show(folio);
				ContextoImp.MoveTo(192,fila);					ContextoImp.Show(nombre);
				ContextoImp.MoveTo(192.5,fila);					ContextoImp.Show(nombre);
				ContextoImp.MoveTo(366,fila);					ContextoImp.Show(fechapago);//170,-70
				ContextoImp.MoveTo(366.5,fila);					ContextoImp.Show(fechapago);//170,-70
				ContextoImp.MoveTo(431,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
				ContextoImp.MoveTo(431.5,fila);					ContextoImp.Show(totaldefactura.ToString("C"));//560,-70
				ContextoImp.MoveTo(496,fila);					ContextoImp.Show(honomed.ToString("C"));
				ContextoImp.MoveTo(496.5,fila);					ContextoImp.Show(honomed.ToString("C"));
				ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70
				ContextoImp.MoveTo(560,fila);					ContextoImp.Show(ttotal.ToString("C"));//560,-70

				ContextoImp.MoveTo(620,fila);					ContextoImp.Show(deducible.ToString("C"));//560,-70
				ContextoImp.MoveTo(670,fila);					ContextoImp.Show(coaseguro.ToString("C"));
				totalfacturas += totaldefactura;
	  			contadorfacturas += 1;
	  			genera_columnas(ContextoImp, trabajoImpresion);
	  			fila-=10;							contador+=1;
				salto_pagina(ContextoImp,trabajoImpresion,"");
				ContextoImp.MoveTo(70, fila+10);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
															"_______________________________________________________________________________________________");
				ContextoImp.MoveTo(145.7,fila);				ContextoImp.Show("TOTAL DE FACTURAS "+contadorfacturas.ToString());
				ContextoImp.MoveTo(146,fila);				ContextoImp.Show("TOTAL DE FACTURAS "+contadorfacturas.ToString());
				ContextoImp.MoveTo(610.7,fila);				ContextoImp.Show("TOTAL FACTURADO" );
				ContextoImp.MoveTo(611,fila);				ContextoImp.Show("TOTAL FACTURADO" );
				ContextoImp.MoveTo(690.7,fila);				ContextoImp.Show(totalfacturas.ToString("C"));
				ContextoImp.MoveTo(691,fila);				ContextoImp.Show(totalfacturas.ToString("C"));
		
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string cliente)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente6);
			ContextoImp.MoveTo(69.7,-30);			ContextoImp.Show("Hospital Santa Cecilia");//19.7, 770
			ContextoImp.MoveTo(70, -30);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(69.7, -40);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(70, -40);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(69.7, -50);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(70, -50);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
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
			imprime_titulo(ContextoImp,trabajoImpresion,cliente);
			/////////DIBUJANDO LA TABLA/////////
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(68, -55);			ContextoImp.Show("_____________________________________");
			ContextoImp.MoveTo(68, -575);			ContextoImp.Show("_____________________________________");
			Gnome.Print.Setfont (ContextoImp, fuente11);
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.Rotate(270);
			ContextoImp.MoveTo(56, 810);			ContextoImp.Show("__________________________");
			ContextoImp.MoveTo(56,70);				ContextoImp.Show("__________________________");
			///termino de dibujo de tabla
			ContextoImp.Rotate(90);//RESTAURO EL ORDEN A VERTICAL
			Gnome.Print.Setfont(ContextoImp,fuente7);//RESTAURO FUENTE A TAMAÑO 7
		}	
		
		void genera_columnas(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			Gnome.Print.Setfont (ContextoImp, fuente12);			
			ContextoImp.MoveTo(111,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(156.5,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(190,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(360,fila);					ContextoImp.Show("|	");	
			ContextoImp.MoveTo(425,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(490,fila);					ContextoImp.Show("| ");
			ContextoImp.MoveTo(555,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(615,fila);					ContextoImp.Show("| ");
			ContextoImp.MoveTo(665,fila);					ContextoImp.Show("|	");
			Gnome.Print.Setfont (ContextoImp, fuente7);
		}
		
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,string cliente)
		{
		
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila+10);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
				Gnome.Print.Setfont(ContextoImp,fuente10);
				ContextoImp.MoveTo(299.7, fila);				ContextoImp.Show(cliente.ToUpper());
				ContextoImp.MoveTo(300, fila);					ContextoImp.Show(cliente.ToUpper());
				contador+=1;
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila-1);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
				fila-=10;
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(71.9, fila);					ContextoImp.Show("Nº FACT.");
				ContextoImp.MoveTo(72, fila);					ContextoImp.Show("Nº FACT.");
				ContextoImp.MoveTo(111.9,fila);					ContextoImp.Show("|	FECHA");
				ContextoImp.MoveTo(112,fila);					ContextoImp.Show("|	FECHA");
				ContextoImp.MoveTo(157.9,fila);					ContextoImp.Show("|	Nº ATEN");
				ContextoImp.MoveTo(158,fila);					ContextoImp.Show("|	Nº ATEN");
				ContextoImp.MoveTo(191.9,fila);					ContextoImp.Show("|	PACIENTE");
				ContextoImp.MoveTo(192,fila);					ContextoImp.Show("|	PACIENTE");
				ContextoImp.MoveTo(360.7,fila);					ContextoImp.Show("| FECHA DE PAGO");
				ContextoImp.MoveTo(361,fila);					ContextoImp.Show("| FECHA DE PAGO");
				ContextoImp.MoveTo(426.9,fila);					ContextoImp.Show("|	SUB-TOTAL ");
				ContextoImp.MoveTo(426,fila);					ContextoImp.Show("|	SUB-TOTAL ");
				ContextoImp.MoveTo(490.9,fila);					ContextoImp.Show("|	HONO. MEDICO");
				ContextoImp.MoveTo(490,fila);					ContextoImp.Show("|	HONO. MEDICO");
				ContextoImp.MoveTo(560,fila);					ContextoImp.Show("|	TOTAL ");
				ContextoImp.MoveTo(560.9,fila);					ContextoImp.Show("|	TOTAL ");
			    ContextoImp.MoveTo(620,fila);					ContextoImp.Show("|	DEDUCIBLE ");
			    ContextoImp.MoveTo(620.9,fila);					ContextoImp.Show("|	DEDUCIBLE ");
			    ContextoImp.MoveTo(670,fila);					ContextoImp.Show("|	COASEGURO ");
				ContextoImp.MoveTo(670.9,fila);					ContextoImp.Show("|	COASEGURO ");
			
	
			
			
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, fila);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																			"_______________________________________________________________________________________________");
				fila-=10;			contador+=1;
		}
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,string cliente)
		{
			//Console.WriteLine("contador antes del if: "+contador_.ToString());
	        if (contador > 50 )
	        {
	        	numpage +=1;        	contador=1;	
	        	if(tiporeporte == "CERRADOS") { fila=-75; }else{ fila =-65; }
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion,cliente);
	     	}
	       //Console.WriteLine("contador despues del if: "+contador_.ToString());
		}
 	}   
}