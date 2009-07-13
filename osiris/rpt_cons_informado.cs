///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
// Proposito	: Pagos en Caja 
// Objeto		: .cs
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{	
	public class conse_info
	{
		public string connectionString = "Server=localhost;" +
            	                         "Port=5432;" +
                	                     "User ID=admin;" +
                    	                 "Password=1qaz2wsx;";
        public string nombrebd;   
    	public int PidPaciente;
    	public int folioservicio;
    	public string medico;
    	public string cirugia;
    	public string mes = "";
    
		public conse_info (int PidPaciente_ , int folioservicio_,string _nombrebd_,string doctor_,string cirugia_)
		{
			nombrebd = _nombrebd_;
			PidPaciente = PidPaciente_;
    		folioservicio = folioservicio_;
    		medico = doctor_;
    		cirugia = cirugia_;
    		
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "CONSENTIMIENTO INFORMADO", 0);
        	int         respuesta = dialogo.Run ();
			          
			if (respuesta == (int) PrintButtons.Cancel){
				Console.WriteLine("Impresión cancelada");
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
        	trabajo.Close();             
        	switch (respuesta)
        	{
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
                  		Console.WriteLine ("vista previa");
                      	new PrintJobPreview(trabajo, "CONSENTIMIENTO INFORMADO").Show();
                        break;
        	}
			dialogo.Hide (); dialogo.Dispose ();        
		}
		
		void nom_mes()
		{
			if (DateTime.Now.ToString("MM") == "01"){
				mes ="Enero";
				Console.WriteLine("primer if:"+mes.ToString());
			}
			if (DateTime.Now.ToString("MM") == "02"){
				mes ="Febrero";
			}
			if (DateTime.Now.ToString("MM") == "03"){
				mes ="Marzo";
			}
			if (DateTime.Now.ToString("MM") == "04"){
				mes ="Abril";
			}
			if (DateTime.Now.ToString("MM") == "05"){
				mes ="Mayo";
			}
			if (DateTime.Now.ToString("MM") == "06"){
				mes ="Junio";
			}
			if (DateTime.Now.ToString("MM") == "07"){
				mes ="Julio";
			}
			if (DateTime.Now.ToString("MM") == "08"){
				mes ="Agosto";
			}
			if (DateTime.Now.ToString("MM") == "09"){
				mes ="Septiembre";
			}
			if (DateTime.Now.ToString("MM") == "10"){
				mes ="Octubre";
			}
			if (DateTime.Now.ToString("MM") == "11"){
				mes ="Noviembre";
			}
			if (DateTime.Now.ToString("MM") == "12"){
				mes ="Diciembre";
			}
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			/*NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
            
        	// Verifica que la base de DateTime.Now.ToString("MM")s este conectada
        	try
        	{
        		conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
        	
        		comando.CommandText ="SELECT hscmty_erp_cobros_enca.folio_de_servicio,hscmty_erp_cobros_enca.pid_paciente,hscmty_his_paciente.nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente, "+
        						"hscmty_erp_cobros_enca.responsable_cuenta,hscmty_erp_cobros_enca.id_medico,hscmty_his_medicos.nombre_medico ,hscmty_his_tipo_cirugias.descripcion_cirugia,hscmty_his_tipo_diagnosticos.id_diagnostico,"+
        						"hscmty_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos "+
        						"FROM hscmty_erp_cobros_enca,hscmty_his_medicos,hscmty_erp_movcargos,hscmty_his_paciente,hscmty_his_tipo_cirugias,hscmty_his_tipo_diagnosticos "+
        						"WHERE hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
        						"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
        						"AND hscmty_erp_cobros_enca.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
        						"AND hscmty_erp_movcargos.pid_paciente = hscmty_his_paciente.pid_paciente "+
        						"AND hscmty_his_tipo_cirugias.id_tipo_cirugia = hscmty_erp_movcargos.id_tipo_cirugia "+
        						"AND hscmty_his_tipo_diagnosticos.id_diagnostico = hscmty_erp_movcargos.id_diagnostico "+
        						"AND hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
         						"AND hscmty_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString()+"' "+
         						"LIMIT 1";
         	
         		//NpgsqlDataReader lector = comando.ExecuteReader ();
		*/		
				ContextoImp.BeginPage("Concentimiento Informado");
				//NUEVO
				// Crear una fuente de tipo Impact
				Gnome.Font fuente = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 12);
				Gnome.Font fuente1 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 36);
				Gnome.Font fuente2 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 10);
				Gnome.Font fuente3 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 6);
				Gnome.Font fuente4 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 8);
						
				//Encabezado de pagina
				Gnome.Print.Setfont (ContextoImp, fuente3);
			
				ContextoImp.MoveTo(19.5, 750);
			    ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
				ContextoImp.MoveTo(20, 750);
		    	ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
		    
			    ContextoImp.MoveTo(479.5, 750);
			    ContextoImp.Show("FO-DMH-03/REV.02/22-SEP-06");
			    ContextoImp.MoveTo(480, 750);
			    ContextoImp.Show("FO-DMH-03/REV.02/22-SEP-06");
		    
			    ContextoImp.MoveTo(19.5, 740); 
			    ContextoImp.Show("Direccion: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			    ContextoImp.MoveTo(20, 740);
			    ContextoImp.Show("Direccion: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			
				ContextoImp.MoveTo(19.5, 730);
		    	ContextoImp.Show("Conmutador:(81) 81-25-56-10");
				ContextoImp.MoveTo(20, 730);
		    	ContextoImp.Show("Conmutador:(81) 81-25-56-10");
		    
		    	Gnome.Print.Setfont (ContextoImp, fuente);
				ContextoImp.MoveTo(219.5, 720);
		    	ContextoImp.Show("CONSENTIMIENTO INFORMADO");
				ContextoImp.MoveTo(220, 720);
		    	ContextoImp.Show("CONSENTIMIENTO INFORMADO");
		    
		    	Gnome.Print.Setfont (ContextoImp, fuente1);
		    	ContextoImp.MoveTo(20, 715);
      			ContextoImp.Show("____________________________");
		    
		    
		    	int filatex=680;
		    	Gnome.Print.Setfont (ContextoImp, fuente2);
		    
		    	//parrafo 1
		    	ContextoImp.MoveTo(20, filatex);
				ContextoImp.Show("La  Norma  Oficial Mexicana NOM-168-SSA1-1998, Del Expediente Clinico,  la  Ley General De  Salud asi como los artículos ");
				ContextoImp.MoveTo(20, filatex-10);
				ContextoImp.Show("80, 81 y 83 del  Reglamento  de la Ley General de  Salud, sustentan  que Usted tiene derecho a ser  informado(a)  por  su ");
				ContextoImp.MoveTo(20, filatex-20);
				ContextoImp.Show("médico  tratante  sobre  su  estado  de  salud  y  los procedimientos  de  diagnóstico que le serán realizados, entre los cuales ");
				ContextoImp.MoveTo(20, filatex-30);
				ContextoImp.Show("se cuentan  estudios de laboratorio, pruebas electrofisiológicas  y  estudios de imagen diagnóstica; así mismo, tiene derecho ");
				ContextoImp.MoveTo(20, filatex-40);
				ContextoImp.Show("a  ser  informado(a)  de   las  características,  los   beneficios    y   riesgos  inherentes  a  los  procedimientos   terapéuticos "); 
				ContextoImp.MoveTo(20, filatex-50);
				ContextoImp.Show("(farmacológicos, anetésicos, quirúrgicos y  de rehabilitación) que le  han sido propuestos  y  que  se realizarán en  éste ");
				ContextoImp.MoveTo(20, filatex-60);
				ContextoImp.Show("hospital. Este documento le permite otorgar su consentimiento informado y autorizar al HOSPITAL SANTA CECILIA DE"); 
 				ContextoImp.MoveTo(20, filatex-70);
				ContextoImp.Show("MONTERREY S.A. DE C.V. a  su médico  tratante, DR. (A):_________________________________________________");
				ContextoImp.MoveTo(20, filatex-80);
				ContextoImp.Show(" y  a su equipo de salud y personal del hospital, a realizar:");
				ContextoImp.MoveTo(20, filatex-90);//100
				ContextoImp.Show("______________________________________________________________________________________________________"); 
				ContextoImp.MoveTo(20, filatex-110);
				ContextoImp.Show("______________________________________________________________________________________________________");
				ContextoImp.MoveTo(20, filatex-130);
				ContextoImp.Show("");
				//parrafo 2
				ContextoImp.MoveTo(20, filatex-150);
				ContextoImp.Show("  Usted  ha  sido  instruido  por  su  médico  tratante  de  los  riesgos  y  peligros  de  continuar con su  actual estado de salud");
				ContextoImp.MoveTo(20, filatex-160);
				ContextoImp.Show("sin  tratamiento.  El  beneficio  buscado  y  esperado  es  el  restablecimiento  y/o  mejoría de su salud, propósito único de su");
				ContextoImp.MoveTo(20, filatex-170);
				ContextoImp.Show("atención  en  este  hospital.  Entre  los  posible  riesgos  y  complicaciones  de  los  procedimientos  diagnósticos,  tratamientos");
				ContextoImp.MoveTo(20, filatex-180);
				ContextoImp.Show("médicos,  anestésicos, quirúrgicos y  de  rehabilitación  se  cuentan  infecciones,  reacciones  alérgicas  a  medicamentos, ");
				ContextoImp.MoveTo(20, filatex-190);
				ContextoImp.Show("substancias anestésicas, o medios de contraste radiológico. ");
				ContextoImp.MoveTo(20, filatex-200);
				ContextoImp.Show("");
				//parrafo 3
				ContextoImp.MoveTo(20, filatex-210);
				ContextoImp.Show("  Además puede  existir  riesgo de hemorragias y coágulos  de  sangre  en venas o arterias de diversos órganos. Puede haber");
				ContextoImp.MoveTo(20, filatex-220);
				ContextoImp.Show("otros riesgos y complicaciones que dependerán de las características de cada procedimiento. En casos aislados y extremos");
				ContextoImp.MoveTo(20, filatex-230);
			    ContextoImp.Show("puede haber riesgos de fallecimiento debido a un procedimiento. Es importante que Usted sepa que la prevención y diagnóstico");			
				ContextoImp.MoveTo(20, filatex-240);
			    ContextoImp.Show("inmediato, en caso de que se presenta alguna de éstas complicaciones, constituyen la parte mas importante de la atención");			
				ContextoImp.MoveTo(20, filatex-250);
			    ContextoImp.Show("profesional  a que será Usted sometido(a) durante su estancia en el hospital.");
				
				//parrafo 4
				ContextoImp.MoveTo(20, filatex-280);
			    ContextoImp.Show("   Es  posible que al momento de practicar un procedimiento, diagnóstico, tratamiento  médico,  anestésico, quirúrgico o de");			
				ContextoImp.MoveTo(20, filatex-290);
			    ContextoImp.Show("rehabilitación se descubra un problema de salud que requiera de un procedimiento simultáneo adicional. Mediante su firma,");
			    ContextoImp.MoveTo(20, filatex-300);
			    ContextoImp.Show("Usted autoriza al médico tratante, a su equipo de  salud  y  al personal del hospital, a realizar  éste  procedimiento adicional,");
			 	ContextoImp.MoveTo(20, filatex-310);
			    ContextoImp.Show("aunque  no  hubiese  sido  explícitamente  consignado  en  este  documento. ");
			 	ContextoImp.MoveTo(20, filatex-330);
			    ContextoImp.Show("Por lo antes dicho, otorgo mi consentimiento informado mediante mi firma al calce, en compañía de los testigos, siempre ");
			 	ContextoImp.MoveTo(20, filatex-340);
			    ContextoImp.Show("y cuando en todo momento se apliquen  los procedimientos conforme dispongan  la  Normas  Oficiales Mexicanas ");
			 	ContextoImp.MoveTo(20, filatex-350);
			 	ContextoImp.Show("relacionadas con el procedimiento a realizar.");
			    //Line de fecha
			    nom_mes();
			    ContextoImp.MoveTo(19.7, filatex-370);
			    ContextoImp.Show("Monterrey N.L., siendo las  "+ DateTime.Now.ToString("HH:mm")+ " horas del día  "+ DateTime.Now.ToString("dd")+ " de "+mes+" del año "+DateTime.Now.ToString("yyyy"));
		    	ContextoImp.MoveTo(20, filatex-370);
		    	ContextoImp.Show("Monterrey N.L., siendo las  "+ DateTime.Now.ToString("HH:mm")+ " horas del día  "+ DateTime.Now.ToString("dd")+ " de "+mes+" del año "+DateTime.Now.ToString("yyyy"));
		    	ContextoImp.MoveTo(180, filatex-410);
			    ContextoImp.Show("______________________________________________________");
			    //linea de firmas
			    ContextoImp.MoveTo(270, filatex-420);
			    ContextoImp.Show("Nombre y Firma");
			    ContextoImp.MoveTo(180, filatex-430);
			    ContextoImp.Show("Paciente, Padre, Madre y/o Tutor o Responsable Legal.");
			    ContextoImp.MoveTo(20, filatex-460);
			    ContextoImp.Show("");
			    ContextoImp.MoveTo(180, filatex-500);
			    ContextoImp.Show("______________________________________________________");
			    ContextoImp.MoveTo(270, filatex-510);
			    ContextoImp.Show("Nombre y firma");
			    ContextoImp.MoveTo(255, filatex-520);
			    ContextoImp.Show("Médico Responsable");
			    ContextoImp.MoveTo(20, filatex-520);
			    ContextoImp.Show("");
			    ContextoImp.MoveTo(20, filatex-550);
			    ContextoImp.Show("____________________________________");
			    ContextoImp.MoveTo(60, filatex-560);
			    ContextoImp.Show("Nombre y firma");
			    ContextoImp.MoveTo(75, filatex-570);
			    ContextoImp.Show("Testigo");
			    ContextoImp.MoveTo(390, filatex-550);
			    ContextoImp.Show("____________________________________");
			    ContextoImp.MoveTo(460, filatex-560);
			    ContextoImp.Show("Nombre y firma");
			    ContextoImp.MoveTo(485, filatex-570);
			    ContextoImp.Show("Testigo");
		    
			    /*int filas=710;
			    for (int i1=0; i1 < 160; i1++)
				{
				filas-=5;
				ContextoImp.MoveTo(588, filas);
      			ContextoImp.Show("|");
				}*/
		    
		    	//Pie de pagina
		    	ContextoImp.MoveTo(80, filatex-590);
		    	ContextoImp.Show("Este documento ha sido revisado y aceptado por la Comisión Estatal de Arbitraje Médico.");
		    	Gnome.Print.Setfont (ContextoImp, fuente3);
		    	ContextoImp.MoveTo(490, filatex-610);
		    	ContextoImp.Show("FO-DMH-03/REV.02/22-SEP-06");
		    	Gnome.Print.Setfont (ContextoImp, fuente1);
		    	ContextoImp.MoveTo(20, filatex-650);
      			ContextoImp.Show("____________________________");
      		
	      		//lector start
 	     	/*	if ((bool) lector.Read())
      			{*/
					Gnome.Print.Setfont (ContextoImp, fuente2);
				
					//lectura de DateTime.Now.ToString("MM")s de paciente, diagnostico y doctor
      				ContextoImp.MoveTo(20, filatex-70);//80
					ContextoImp.Show("MONTERREY S.A. DE C.V. a  su médico  tratante, DR. (A):  "+ medico.ToString()); //+(string) lector["nombre_medico"]);
					Gnome.Print.Setfont (ContextoImp, fuente4);
					ContextoImp.MoveTo(20, filatex-90);
					ContextoImp.Show(cirugia.ToString());//(string) lector["descripcion_cirugia"]);
				
					Gnome.Print.Setfont (ContextoImp, fuente2);
					ContextoImp.MoveTo(250, filatex-500);
			    	ContextoImp.Show(medico.ToString());//(string) lector["nombre_medico"]);
			    	ContextoImp.ShowPage();
		    	/*}
			
				lector.Close (); 
				conexion.Close ();
		
				ContextoImp.ShowPage();
			
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}*/
		}
	}
}