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
// Programa		: 
// Proposito	: 
// Objeto		: 
//////////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
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
		int numpage = 1;
		int separacion_linea = 10;
        
    	int PidPaciente;
    	int folioservicio;
		string nombrepaciente;
		string medico_primer_diag;
    	string medico_tratante;
    	string cirugia;
    	string nombre_delmes = "";
		
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
    
		public conse_info (int PidPaciente_ , int folioservicio_,string nombrebd_,string nombrepaciente_, string medico_primer_diag_,string medico_tratante_,string cirugia_)
		{
			PidPaciente = PidPaciente_;
    		folioservicio = folioservicio_;
			nombrepaciente = nombrepaciente_;
    		medico_primer_diag = medico_primer_diag_;
			medico_tratante = medico_tratante_;
    		cirugia = cirugia_;			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);			
			// Verifica que la base de DateTime.Now.ToString("MM")s este conectada
        	try{
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
         		//Console.WriteLine(comando.CommandText);
         		NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){					
					//medico_tratante = (string) lector["nombre_medico_tratante"].ToString().Trim();
					print = new PrintOperation ();
					print.JobName = "Autorizacion para Cirugia";
					print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
					print.DrawPage += new DrawPageHandler (OnDrawPage);
					print.EndPrint += new EndPrintHandler (OnEndPrint);
					print.Run (PrintOperationAction.PrintDialog, null);
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error,ButtonsType.Close,"No hay informacion para mostrar... verifique...");
					msgBoxError.Run ();		msgBoxError.Destroy();					
				}				
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}
		}
		
		void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{									
			print.NPages = 1; // crea cantidad de copias del reporte
		}
		
		void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{
			PrintContext context = args.Context;
			crea_consentimiento(context);
		}
		
		void crea_consentimiento(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			crea_encbezado(cr,layout);
			formato_consentimiento2(cr,layout);
		}
			
		void crea_encbezado(Cairo.Context cr,Pango.Layout layout)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
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
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente	
			fontSize =11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(155*escala_en_linux_windows, 85*escala_en_linux_windows);										layout.SetText("AUTORIZACION PARA TRATAMIENTOS MEDICOS,");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(145*escala_en_linux_windows, 95*escala_en_linux_windows);										layout.SetText("PROCEDIMIENTOS DE DIAGNOSTICO TERAPEUTICOS");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows, 105*escala_en_linux_windows);										layout.SetText("Y/O INTERVENCIONES");
			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Pango.Weight.Normal;		// Letra Normal
		}
		
		void formato_consentimiento1(Cairo.Context cr,Pango.Layout layout)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");	
			layout.Alignment = Pango.Alignment.Center;
			fontSize =9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			int filatex=100;
	    	//parrafo 1
	    	cr.MoveTo(001, filatex*escala_en_linux_windows);										layout.SetText("La Norma Oficial Mexicana NOM-168-SSA1-1998, Del Expediente Clinico, la Ley General De Salud asi como los artículos");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+10)*escala_en_linux_windows);									layout.SetText("80, 81 y 83 del Reglamento de la Ley General de Salud, sustentan que Usted tiene derecho a ser informado(a) por su ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+20)*escala_en_linux_windows);									layout.SetText("médico tratante sobre su estado de salud y los procedimientos de diagnóstico que le serán realizados, entre los cuales ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+30)*escala_en_linux_windows);									layout.SetText("se cuentan estudios de laboratorio, pruebas electrofisiológicas y estudios de imagen diagnóstica; así mismo, tiene derecho ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+40)*escala_en_linux_windows);									layout.SetText("a ser informado(a) de las características, los beneficios y riesgos inherentes a los procedimientos terapéuticos "); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+50)*escala_en_linux_windows);									layout.SetText("(farmacológicos, anetésicos, quirúrgicos y de rehabilitación) que le  han sido propuestos y que se realizarán en éste ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+60)*escala_en_linux_windows);									layout.SetText("hospital. Este documento le permite otorgar su consentimiento informado y autorizar al HOSPITAL XXXXXXXXXX"); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+70)*escala_en_linux_windows);									layout.SetText("                   a  su médico  tratante, DR. (A):_________________________________________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+80)*escala_en_linux_windows);									layout.SetText(" y  a su equipo de salud y personal del hospital, a realizar:");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+90)*escala_en_linux_windows);									layout.SetText("______________________________________________________________________________________________________"); 
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+110)*escala_en_linux_windows);									layout.SetText("______________________________________________________________________________________________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+130)*escala_en_linux_windows);									layout.SetText("");
			//parrafo 2
			cr.MoveTo(001, (filatex+150)*escala_en_linux_windows);									layout.SetText("Usted ha sido instruido por su médico tratante de los riesgos y peligros de continuar con su actual estado de salud");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+160)*escala_en_linux_windows);									layout.SetText("sin tratamiento. El beneficio buscado y esperado es el restablecimiento y/o mejoría de su salud, propósito único de su");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+170)*escala_en_linux_windows);									layout.SetText("atención en este hospital. Entre los posible riesgos y complicaciones de los procedimientos diagnósticos, tratamientos");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+180)*escala_en_linux_windows);									layout.SetText("médicos, anestésicos, quirúrgicos y de rehabilitación se cuentan infecciones, reacciones alérgicas a medicamentos, ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+190)*escala_en_linux_windows);									layout.SetText("substancias anestésicas, o medios de contraste radiológico. ");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+200)*escala_en_linux_windows);									layout.SetText("");
			//parrafo 3
			cr.MoveTo(001, (filatex+210)*escala_en_linux_windows);									layout.SetText("Además puede existir riesgo de hemorragias y coágulos de sangre en venas o arterias de diversos órganos. Puede haber");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+220)*escala_en_linux_windows);									layout.SetText("otros riesgos y complicaciones que dependerán de las características de cada procedimiento. En casos aislados y extremos");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+230)*escala_en_linux_windows);								    layout.SetText("puede haber riesgos de fallecimiento debido a un procedimiento. Es importante que Usted sepa que la prevención y diagnóstico");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+240)*escala_en_linux_windows);								    layout.SetText("inmediato, en caso de que se presenta alguna de éstas complicaciones, constituyen la parte mas importante de la atención");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+250)*escala_en_linux_windows);								    layout.SetText("profesional a que será Usted sometido(a) durante su estancia en el hospital.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			//parrafo 4
			cr.MoveTo(001, (filatex+280)*escala_en_linux_windows);								    layout.SetText("Es  posible que al momento de practicar un procedimiento, diagnóstico, tratamiento  médico, anestésico, quirúrgico o de");			
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+290)*escala_en_linux_windows);								    layout.SetText("rehabilitación se descubra un problema de salud que requiera de un procedimiento simultáneo adicional. Mediante su firma,");
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+300)*escala_en_linux_windows);								    layout.SetText("Usted autoriza al médico tratante, a su equipo de salud y al personal del hospital, a realizar éste procedimiento adicional,");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+310)*escala_en_linux_windows);								    layout.SetText("aunque no hubiese sido explícitamente  consignado en este documento. ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+330)*escala_en_linux_windows);								    layout.SetText("Por lo antes dicho, otorgo mi consentimiento informado mediante mi firma al calce, en compañía de los testigos, siempre ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+340)*escala_en_linux_windows);								    layout.SetText("y cuando en todo momento se apliquen  los procedimientos conforme dispongan  la  Normas  Oficiales Mexicanas ");
		 	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+350)*escala_en_linux_windows);								 	layout.SetText("relacionadas con el procedimiento a realizar.");
		    //Line de fecha
		    //nom_mes();
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
		    Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001, (filatex+370)*escala_en_linux_windows);							    	layout.SetText("Monterrey N.L., siendo las  "+ DateTime.Now.ToString("HH:mm")+ " horas del día  "+ DateTime.Now.ToString("dd")+ " de "+" del año "+DateTime.Now.ToString("yyyy"));
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
		
		void formato_consentimiento2(Cairo.Context cr,Pango.Layout layout)
		{
			separacion_linea = 16;
			int columna_inicio = 70;
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");	
			//layout.Alignment = Pango.Alignment.Right;
			layout.Alignment = Pango.Alignment.Center;
			layout.Justify = true;
			fontSize =12.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			int filatex=150;			
			string texto_autorizacion = "Para dar cumplimiento a las disposiciones vigentes marcados por la ley "+
								"general de salud y a los articulos 30, 80, 81, 82, 83. Y demás "+
								"correlacionados con el reglamento de la ley general de salud en "+
								"materia de prestación de servicios de atención médica. "+
								"Yo, "+nombrepaciente+
								"Autorizo para que se me practique en PRACTIMED, cuantas curaciones, "+
								"procedimientos diagnósticos y terapéuticos medicas y operaciones "+
								"Acepto y autorizó a que el, "+
								"Dr. "+medico_tratante+
								"Médico responsable de mi caso, que apegado de los privilegios "+
								"clínicos otorgados por PRACTIMED practique u ordene cuanto examen, "+
								"reconocimiento curación o procedimiento diagnostico y terapeuticos o "+
								"quirúrgico sean necesarios. Acepto y autorizo al citado medico para que "+
								"solicite interconsulta a otro médico para la atención de mi padecimiento "+
								"o de cualquier consecuencia de mismo aceptando desde ahora todos y "+
								"cada uno de los riesgos inherentes e implícitos de la atencion a la que "+
								"acepto ser sometido. Así mismo autorizo a mi médico y a PRACTIMED a"+
								"que disponga de los órganos o tejidos extirpados para su estudio si así lo"+
								"consideran necesario.";
			
	    	//parrafo 1
	    	cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Para dar cumplimiento a las disposiciones vigentes marcados por la ley");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;			
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("general  de  salud  y   a  los  articulos   30,  80,  81,  82,  83.   Y  demás");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("correlacionados  con  el  reglamento  de  la   ley  general  de  salud  en");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("materia de prestación de servicios de atención médica.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Yo, "+nombrepaciente);
			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Autorizo para que se me practique en PRACTIMED, cuantas curaciones,");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("procedimientos  diagnósticos  y  terapéuticos  medicas  y  operaciones");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("quirúrgicas que requiera derivadas de mi actual estado de salud.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows , filatex*escala_en_linux_windows);			layout.SetText("Acepto y autorizó a que el,");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Dr. "+medico_tratante);
			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Médico   responsable   de  mi  caso,  que   apegado   de  los  privilegios");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("clínicos otorgados por "+classpublic.nombre_empresa2);
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("practique   u  ordene  cuanto   examen,   reconocimiento   curación  o");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("procedimiento diagnostico y terapeuticos o quirúrgico sean necesarios.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Acepto y autorizo al citado  medico  para  que  solicite  interconsulta");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("a otro médico para la atención  de  mi  padecimiento  o  de  cualquier");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("consecuencia  de  mismo  aceptando  desde   ahora   todos  y  cada");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("uno   de    los   riesgos   inherentes   e    implícitos    de    la");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("atencion a la que acepto ser sometido. Así mismo autorizo a mi médico");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("y   a  "+classpublic.nombre_empresa2+"  a  que  disponga  de  los  órganos  o");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("tejidos extirpados para su estudio si así lo consideran necesario.");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			filatex += separacion_linea;
			fontSize = 11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(columna_inicio*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Monterrey, Nuevo León a los "+DateTime.Now.ToString("dd")+" días del mes de "+classpublic.nom_mes(DateTime.Now.ToString("MM"))+" del año "+DateTime.Now.ToString("yyyy"));
			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 12.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			filatex += separacion_linea;
			filatex += separacion_linea;
			filatex += separacion_linea;
			filatex += separacion_linea;
			cr.MoveTo(115*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("___________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(330*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("___________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(145*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Paciente");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(370*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Tutor");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			filatex += separacion_linea;
			filatex += separacion_linea;
			filatex += separacion_linea;
			cr.MoveTo(225*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("___________________");
			Pango.CairoHelper.ShowLayout (cr, layout);
			filatex += separacion_linea;
			cr.MoveTo(260*escala_en_linux_windows, filatex*escala_en_linux_windows);				layout.SetText("Médico");
			Pango.CairoHelper.ShowLayout (cr, layout);			
		}
		
		void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{	
				
		}
	}
}