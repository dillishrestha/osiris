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
	public class reporte_porcedimientos_cerrados
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string tiporeporte = "CERRADOS";
		public string titulo = "REPORTE DE PROCEDIMIENTOS CERRADOS";
		
		public int columna = 0;
		public int fila = -70;
		public int contador = 1;
		public int numpage = 1;
		
		public string query_fechas = " ";
		public string query_cliente = " ";
		public string orden = " ";
		public string rango1 = "";
		public string rango2 = "";
				
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
		
		public reporte_porcedimientos_cerrados (string rango1_,string rango2_,string query_fechas_,string _nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_)
		{
			nombrebd = _nombrebd_;
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
			if(tiporeporte == "CERRADOS")	{	imprime_rpt_proc_cerrados(ContextoImp,trabajoImpresion);	}
			ContextoImp.ShowPage();
		}
					
///////////////////////////////REPORTE PARA PROCEDIMIENTOS  CERRADOS////////////////////////////////////////////////////////
///////////////////////////////REPORTE PARA PROCEDIMIENTOS  CERRADOS/////////////////////////////////////////////////////
		void imprime_rpt_proc_cerrados(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
		
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
	
				comando.CommandText ="SELECT DISTINCT(hscmty_erp_movcargos.folio_de_servicio),to_char(hscmty_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
								"hscmty_erp_cobros_enca.pid_paciente,cerrado, alta_paciente, "+
				            	"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
				            	"hscmty_empresas.descripcion_empresa,"+
				            	"to_char(hscmty_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mm') AS fecha_ingreso,"+
				            	"to_char(hscmty_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mm') AS fecha_egreso,"+
				            	"to_char(hscmty_erp_cobros_enca.fechahora_cerrado,'dd-MM-yyyy HH24:mm') AS fechahoracerrado,"+
				            	"hscmty_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"descripcion_tipo_paciente,"+
            					"hscmty_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
				            	"to_char(hscmty_erp_cobros_enca.total_procedimiento + hscmty_erp_cobros_enca.honorario_medico,'999999999.99') AS totalprocedimiento,"+
				            	"to_char(hscmty_erp_cobros_enca.contador_cerrados,'9999999') AS contadorcerrado "+
				            	"FROM "+ 
				            	"hscmty_erp_cobros_enca,hscmty_his_paciente,hscmty_erp_movcargos,hscmty_his_tipo_pacientes, "+
				            	"hscmty_aseguradoras,hscmty_empresas "+
				            	"WHERE "+
				            	"hscmty_erp_cobros_enca.pid_paciente = hscmty_his_paciente.pid_paciente "+
				            	"AND hscmty_erp_movcargos.folio_de_servicio = hscmty_erp_cobros_enca.folio_de_servicio "+
				            	"AND hscmty_erp_cobros_enca.id_aseguradora = hscmty_aseguradoras.id_aseguradora "+ 
								"AND hscmty_erp_movcargos.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente "+
								"AND hscmty_his_paciente.id_empresa = hscmty_empresas.id_empresa "+ 
								" "+query_fechas+" "+
								"AND hscmty_erp_cobros_enca.reservacion = 'false' "+
								"AND hscmty_erp_cobros_enca.cerrado = 'true' "+
								"AND hscmty_erp_cobros_enca.cancelado = 'false' "+
								"AND hscmty_erp_movcargos.id_tipo_admisiones > '16' "+
								"AND hscmty_erp_cobros_enca.id_aseguradora != '17'; ";
								
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
			if(tiporeporte == "CERRADOS")
			{
				int filal = -66;
				for (int i1=0; i1 < 52; i1++)//30 veces para tamaño carta
				{
					ContextoImp.MoveTo(119.5,filal);				ContextoImp.Show("| ");
					ContextoImp.MoveTo(309.5,filal);				ContextoImp.Show("|	");
					ContextoImp.MoveTo(379.5,filal);				ContextoImp.Show("|	");
					ContextoImp.MoveTo(539.5,filal);				ContextoImp.Show("|	");  
					ContextoImp.MoveTo(604.5,filal);				ContextoImp.Show("|	");
					ContextoImp.MoveTo(679.5,filal);				ContextoImp.Show("|	");
					ContextoImp.MoveTo(744.5,filal);				ContextoImp.Show("|	");
					filal-=10;
				}
			}
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
			ContextoImp.MoveTo(620,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(685,fila);					ContextoImp.Show("|	");
			ContextoImp.MoveTo(750,fila);					ContextoImp.Show("|	");
			Gnome.Print.Setfont (ContextoImp, fuente7);
		}
		
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,string cliente)
		{
			if(tiporeporte == "CERRADOS")
			{
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70.7, -65);					ContextoImp.Show("Nº ATENCION"); //| Fecha | Nº Atencion | Paciente | SubTotal al 15 | SubTotal al 0 | IVA | SubTotal Deducible | Coaseguro | Total | Hono. Medico");
				ContextoImp.MoveTo(71, -65);					ContextoImp.Show("Nº ATENCION");
				ContextoImp.MoveTo(119.7,-65);					ContextoImp.Show("| PACIENTE");
				ContextoImp.MoveTo(120,-65);					ContextoImp.Show("|	PACIENTE");//80,-70
				ContextoImp.MoveTo(309.7,-65);					ContextoImp.Show("|	TIPO ");
				ContextoImp.MoveTo(310,-65);					ContextoImp.Show("|	TIPO ");//120,-70
				ContextoImp.MoveTo(379.7,-65);					ContextoImp.Show("|	EMPRESA");
				ContextoImp.MoveTo(380,-65);					ContextoImp.Show("|	EMPRESA");//170,-70
				ContextoImp.MoveTo(539.7,-65);					ContextoImp.Show("|	FECHA INGRESO");  
				ContextoImp.MoveTo(540,-65);					ContextoImp.Show("|	FECHA INGRESO");//290,-70
				ContextoImp.MoveTo(604.7,-65);					ContextoImp.Show("|	FECHA ALTA");
				ContextoImp.MoveTo(605,-65);					ContextoImp.Show("|	FECHA ALTA");//360,-70
				ContextoImp.MoveTo(679.7,-65);					ContextoImp.Show("|	FECHA CIERRE");
				ContextoImp.MoveTo(680,-65);					ContextoImp.Show("|	FECHA CIERRE");//
				ContextoImp.MoveTo(744.7,-65);					ContextoImp.Show("|	TOTAL");
				ContextoImp.MoveTo(745,-65);					ContextoImp.Show("|	TOTAL");//420,-70
				Gnome.Print.Setfont(ContextoImp,fuente7);
				ContextoImp.MoveTo(70, -66);					ContextoImp.Show   ("_______________________________________________________________________________________________"+
																					"_______________________________________________________________________________________________");
			} 
			
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