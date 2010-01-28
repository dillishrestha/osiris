///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
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
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{	
	public class conse_info
	{
		string connectionString;
		string nombrebd;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows = 1;		// Linux = 1  Windows = 8		
        
    	int PidPaciente;
    	int folioservicio;
    	string medico;
    	string cirugia;
    	string mes = "";
		
		class_conexion conexion_a_DB = new class_conexion();
    
		public conse_info (int PidPaciente_ , int folioservicio_,string nombrebd_,string doctor_,string cirugia_)
		{
			PidPaciente = PidPaciente_;
    		folioservicio = folioservicio_;
    		medico = doctor_;
    		cirugia = cirugia_;			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			print = new PrintOperation ();			
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
    		
			//Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
        	//Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "CONSENTIMIENTO INFORMADO", 0);
        	//int         respuesta = dialogo.Run ();
			/*          
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
			dialogo.Hide (); dialogo.Dispose ();*/        
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
		
		void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			PrintContext context = args.Context;											
			print.NPages = 1; // crea cantidad de copias del reporte
		}
		
		void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{
			PrintContext context = args.Context;
			crea_consentimiento(context);
		}
		
		void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{	
				
		}
		
		void crea_consentimiento(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
            crea_encbezado(context);
        	// Verifica que la base de DateTime.Now.ToString("MM")s este conectada
        	/*try
        	{
        		conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
        	
        		comando.CommandText ="SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente,osiris_his_paciente.nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente, "+
        						"osiris_erp_cobros_enca.responsable_cuenta,osiris_erp_cobros_enca.id_medico,osiris_his_medicos.nombre_medico ,osiris_his_tipo_cirugias.descripcion_cirugia,osiris_his_tipo_diagnosticos.id_diagnostico,"+
        						"osiris_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos "+
        						"FROM osiris_erp_cobros_enca,osiris_his_medicos,osiris_erp_movcargos,osiris_his_paciente,osiris_his_tipo_cirugias,osiris_his_tipo_diagnosticos "+
        						"WHERE osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
        						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
        						"AND osiris_erp_cobros_enca.pid_paciente = osiris_erp_movcargos.pid_paciente "+
        						"AND osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente "+
        						"AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_erp_movcargos.id_tipo_cirugia "+
        						"AND osiris_his_tipo_diagnosticos.id_diagnostico = osiris_erp_movcargos.id_diagnostico "+
        						"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
         						"AND osiris_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString()+"' "+
         						"LIMIT 1";
         	
         		//NpgsqlDataReader lector = comando.ExecuteReader ();
				
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}*/
				
				
				
		}
			
		void crea_encbezado(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			cr.MoveTo(001,10*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,20*escala_en_linux_windows);					layout.SetText("Direccion: Monterrey - Mexico");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,30*escala_en_linux_windows);					layout.SetText("Telefono: (01)(81) 1158-5166");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,40*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize =10.0;												layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(230*escala_en_linux_windows, 55*escala_en_linux_windows);										layout.SetText("CONSENTIMIENTO INFORMADO");
			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Pango.Weight.Normal;		// Letra Normal
			
			int filatex=100;
	    	//parrafo 1
	    	cr.MoveTo(001, filatex*escala_en_linux_windows);										layout.SetText("La  Norma  Oficial Mexicana NOM-168-SSA1-1998, Del Expediente Clinico,  la  Ley General De  Salud asi como los artículos ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+10)*escala_en_linux_windows);									layout.SetText("80, 81 y 83 del  Reglamento  de la Ley General de  Salud, sustentan  que Usted tiene derecho a ser  informado(a)  por  su ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+20)*escala_en_linux_windows);									layout.SetText("médico  tratante  sobre  su  estado  de  salud  y  los procedimientos  de  diagnóstico que le serán realizados, entre los cuales ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+30)*escala_en_linux_windows);									layout.SetText("se cuentan  estudios de laboratorio, pruebas electrofisiológicas  y  estudios de imagen diagnóstica; así mismo, tiene derecho ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+40)*escala_en_linux_windows);									layout.SetText("a  ser  informado(a)  de   las  características,  los   beneficios    y   riesgos  inherentes  a  los  procedimientos   terapéuticos "); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+50)*escala_en_linux_windows);									layout.SetText("(farmacológicos, anetésicos, quirúrgicos y  de rehabilitación) que le  han sido propuestos  y  que  se realizarán en  éste ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+60)*escala_en_linux_windows);									layout.SetText("hospital. Este documento le permite otorgar su consentimiento informado y autorizar al HOSPITAL SANTA CECILIA DE"); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+70)*escala_en_linux_windows);									layout.SetText("MONTERREY S.A. DE C.V. a  su médico  tratante, DR. (A):_________________________________________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+80)*escala_en_linux_windows);									layout.SetText(" y  a su equipo de salud y personal del hospital, a realizar:");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+90)*escala_en_linux_windows);									layout.SetText("______________________________________________________________________________________________________"); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+110)*escala_en_linux_windows);									layout.SetText("______________________________________________________________________________________________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+130)*escala_en_linux_windows);									layout.SetText("");
			//parrafo 2
			cr.MoveTo(001, (filatex+150)*escala_en_linux_windows);									layout.SetText("  Usted  ha  sido  instruido  por  su  médico  tratante  de  los  riesgos  y  peligros  de  continuar con su  actual estado de salud");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+160)*escala_en_linux_windows);									layout.SetText("sin  tratamiento.  El  beneficio  buscado  y  esperado  es  el  restablecimiento  y/o  mejoría de su salud, propósito único de su");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+170)*escala_en_linux_windows);									layout.SetText("atención  en  este  hospital.  Entre  los  posible  riesgos  y  complicaciones  de  los  procedimientos  diagnósticos,  tratamientos");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+180)*escala_en_linux_windows);									layout.SetText("médicos,  anestésicos, quirúrgicos y  de  rehabilitación  se  cuentan  infecciones,  reacciones  alérgicas  a  medicamentos, ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+190)*escala_en_linux_windows);									layout.SetText("substancias anestésicas, o medios de contraste radiológico. ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+200)*escala_en_linux_windows);									layout.SetText("");
			//parrafo 3
			cr.MoveTo(001, (filatex+210)*escala_en_linux_windows);									layout.SetText("  Además puede  existir  riesgo de hemorragias y coágulos  de  sangre  en venas o arterias de diversos órganos. Puede haber");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+220)*escala_en_linux_windows);									layout.SetText("otros riesgos y complicaciones que dependerán de las características de cada procedimiento. En casos aislados y extremos");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+230)*escala_en_linux_windows);								    layout.SetText("puede haber riesgos de fallecimiento debido a un procedimiento. Es importante que Usted sepa que la prevención y diagnóstico");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+240)*escala_en_linux_windows);								    layout.SetText("inmediato, en caso de que se presenta alguna de éstas complicaciones, constituyen la parte mas importante de la atención");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+250)*escala_en_linux_windows);								    layout.SetText("profesional  a que será Usted sometido(a) durante su estancia en el hospital.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			//parrafo 4
			cr.MoveTo(001, (filatex+280)*escala_en_linux_windows);								    layout.SetText("   Es  posible que al momento de practicar un procedimiento, diagnóstico, tratamiento  médico,  anestésico, quirúrgico o de");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+290)*escala_en_linux_windows);								    layout.SetText("rehabilitación se descubra un problema de salud que requiera de un procedimiento simultáneo adicional. Mediante su firma,");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+300)*escala_en_linux_windows);								    layout.SetText("Usted autoriza al médico tratante, a su equipo de  salud  y  al personal del hospital, a realizar  éste  procedimiento adicional,");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+310)*escala_en_linux_windows);								    layout.SetText("aunque  no  hubiese  sido  explícitamente  consignado  en  este  documento. ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+330)*escala_en_linux_windows);								    layout.SetText("Por lo antes dicho, otorgo mi consentimiento informado mediante mi firma al calce, en compañía de los testigos, siempre ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+340)*escala_en_linux_windows);								    layout.SetText("y cuando en todo momento se apliquen  los procedimientos conforme dispongan  la  Normas  Oficiales Mexicanas ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+350)*escala_en_linux_windows);								 	layout.SetText("relacionadas con el procedimiento a realizar.");
		    //Line de fecha
		    nom_mes();
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+370)*escala_en_linux_windows);							    	layout.SetText("Monterrey N.L., siendo las  "+ DateTime.Now.ToString("HH:mm")+ " horas del día  "+ DateTime.Now.ToString("dd")+ " de "+mes+" del año "+DateTime.Now.ToString("yyyy"));
	    	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(180*escala_en_linux_windows, (filatex+410)*escala_en_linux_windows);							    layout.SetText("______________________________________________________");
		    //linea de firmas		
			cr.MoveTo(270*escala_en_linux_windows, (filatex+420)*escala_en_linux_windows);							    layout.SetText("Nombre y Firma");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(180*escala_en_linux_windows, (filatex+430)*escala_en_linux_windows);							    layout.SetText("Paciente, Padre, Madre y/o Tutor o Responsable Legal.");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+460)*escala_en_linux_windows);								    layout.SetText("");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(180*escala_en_linux_windows, (filatex+500)*escala_en_linux_windows);							    layout.SetText("______________________________________________________");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(270*escala_en_linux_windows, (filatex+510)*escala_en_linux_windows);							    layout.SetText("Nombre y firma");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(255*escala_en_linux_windows, (filatex+520)*escala_en_linux_windows);							    layout.SetText("Médico Responsable");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+520)*escala_en_linux_windows);								    layout.SetText("");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+550)*escala_en_linux_windows);								    layout.SetText("____________________________________");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows, (filatex+560)*escala_en_linux_windows);								    layout.SetText("Nombre y firma");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(75*escala_en_linux_windows, (filatex+570)*escala_en_linux_windows);								    layout.SetText("Testigo");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(390*escala_en_linux_windows, (filatex+550)*escala_en_linux_windows);							    layout.SetText("____________________________________");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(460*escala_en_linux_windows, (filatex+560)*escala_en_linux_windows);							    layout.SetText("Nombre y firma");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(485*escala_en_linux_windows, (filatex+570)*escala_en_linux_windows);							    layout.SetText("Testigo");
			Pango.CairoHelper.ShowLayout (cr, layout);
		}
		
		
		//void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		//{
			/*NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
            
        	// Verifica que la base de DateTime.Now.ToString("MM")s este conectada
        	try
        	{
        		conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
        	
        		comando.CommandText ="SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente,osiris_his_paciente.nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente, "+
        						"osiris_erp_cobros_enca.responsable_cuenta,osiris_erp_cobros_enca.id_medico,osiris_his_medicos.nombre_medico ,osiris_his_tipo_cirugias.descripcion_cirugia,osiris_his_tipo_diagnosticos.id_diagnostico,"+
        						"osiris_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos "+
        						"FROM osiris_erp_cobros_enca,osiris_his_medicos,osiris_erp_movcargos,osiris_his_paciente,osiris_his_tipo_cirugias,osiris_his_tipo_diagnosticos "+
        						"WHERE osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
        						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
        						"AND osiris_erp_cobros_enca.pid_paciente = osiris_erp_movcargos.pid_paciente "+
        						"AND osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente "+
        						"AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_erp_movcargos.id_tipo_cirugia "+
        						"AND osiris_his_tipo_diagnosticos.id_diagnostico = osiris_erp_movcargos.id_diagnostico "+
        						"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
         						"AND osiris_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString()+"' "+
         						"LIMIT 1";
         	
         		//NpgsqlDataReader lector = comando.ExecuteReader ();
		*/		
				//ContextoImp.BeginPage("Concentimiento Informado");
				//NUEVO
				// Crear una fuente de tipo Impact
				/*Gnome.Font fuente = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 12);
				Gnome.Font fuente1 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 36);
				Gnome.Font fuente2 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 10);
				Gnome.Font fuente3 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 6);
				Gnome.Font fuente4 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 8);
				*/		
				//Encabezado de pagina
				/*Gnome.Print.Setfont (ContextoImp, fuente3);
			
				ContextoImp.MoveTo(19.5, 750);
			    ContextoImp.Show("Sistema Hospitalario OSIRIS");
				ContextoImp.MoveTo(20, 750);
		    	ContextoImp.Show("Sistema Hospitalario OSIRIS");
		    
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
		    	*/
			    /*int filas=710;
			    for (int i1=0; i1 < 160; i1++)
				{
				filas-=5;
				ContextoImp.MoveTo(588, filas);
      			ContextoImp.Show("|");
				}*/
		    
		    	/*Pie de pagina
		    	ContextoImp.MoveTo(80, filatex-590);
		    	ContextoImp.Show("Este documento ha sido revisado y aceptado por la Comisión Estatal de Arbitraje Médico.");
		    	Gnome.Print.Setfont (ContextoImp, fuente3);
		    	ContextoImp.MoveTo(490, filatex-610);
		    	ContextoImp.Show("FO-DMH-03/REV.02/22-SEP-06");
		    	Gnome.Print.Setfont (ContextoImp, fuente1);
		    	ContextoImp.MoveTo(20, filatex-650);
      			ContextoImp.Show("____________________________");
      			*/
	      		//lector start
 	     		/*if ((bool) lector.Read())
      			{
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
		    	}*/
				/*
				lector.Close (); 
				conexion.Close ();
		
				ContextoImp.ShowPage();
				
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}*/
		//}
	}
}