// created on 18/04/2007 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
//				  Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@gmail.com
//				  Israel Peña cambio de gnomeprint a formato a Pango y Cairo
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_prot_admision.cs
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;
using System.Collections;

namespace osiris
{
	public class protocolo_admision
	{
		string connectionString;
        string nombrebd;
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int separacionlineas = 12;
				
		int PidPaciente = 0;
		int folioservicio = 0;
		string medico_tratante = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
				
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
	
		public protocolo_admision ( int PidPaciente_ , int folioservicio_,string nombrebd_,string medico_tratante_)
		{
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
					
			print = new PrintOperation ();
			print.JobName = "Protocolo de Admision";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);		
       }
		
		void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1; // crea cantidad de copias del reporte
					
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//print.DefaultPageSetup.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		void OnDrawPage (object obj, Gtk.DrawPageArgs args)
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
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
            
        	// Verifica que la base de datos este conectada
        	try
        	{
        		conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
             	comando.CommandText ="SELECT "+
									"osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, osiris_erp_cobros_enca.id_empleado_admision,osiris_erp_cobros_enca.nombre_medico_encabezado,osiris_erp_cobros_enca.id_medico,"+
									"osiris_his_paciente.nombre1_paciente,osiris_his_paciente.nombre2_paciente,osiris_his_paciente.apellido_paterno_paciente,osiris_his_paciente.apellido_materno_paciente, "+
									"to_char(fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa,osiris_his_paciente.sexo_paciente,osiris_his_paciente.direccion_paciente,osiris_his_paciente.numero_casa_paciente, "+
									"osiris_his_paciente.numero_departamento_paciente,osiris_his_paciente.colonia_paciente,osiris_his_paciente.municipio_paciente,osiris_his_paciente.codigo_postal_paciente,osiris_his_paciente.estado_paciente, "+
									"osiris_his_paciente.estado_civil_paciente,osiris_his_paciente.ocupacion_paciente,osiris_his_paciente.telefono_particular1_paciente, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"osiris_empresas.descripcion_empresa,osiris_empresas.direccion_empresa,osiris_empresas.telefono1_empresa,osiris_empresas.numero_direccion_empresa,osiris_empresas.colonia_empresa,osiris_his_tipo_pacientes.descripcion_tipo_paciente, "+
									"osiris_erp_cobros_enca.responsable_cuenta,osiris_erp_cobros_enca.direccion_responsable_cuenta,osiris_erp_cobros_enca.telefono1_responsable_cuenta, "+
									"osiris_erp_cobros_enca.ocupacion_responsable,osiris_erp_cobros_enca.parentezco,osiris_erp_cobros_enca.empresa_labora_responsable, "+
									"osiris_erp_cobros_enca.direccion_emp_responsable,osiris_erp_cobros_enca.telefono_emp_responsable,osiris_erp_cobros_enca.paciente_asegurado, "+
									"osiris_erp_cobros_enca.numero_poliza,osiris_his_medicos.nombre_medico,osiris_his_medicos.id_especialidad,osiris_his_tipo_especialidad.descripcion_especialidad, osiris_his_medicos.cedula_medico, "+
									"osiris_erp_movcargos.folio_de_servicio,to_char(fechahora_admision_registro,'dd-MM-yyyy') AS fecha_reg_adm, "+
									"osiris_erp_cobros_enca.nombre_medico_tratante,"+
									"to_char(fechahora_admision_registro,'HH24:mi:ss') AS hora_reg_adm,osiris_his_tipo_admisiones.descripcion_admisiones, "+
									"osiris_his_tipo_cirugias.descripcion_cirugia,osiris_his_tipo_diagnosticos.id_diagnostico, "+
									"osiris_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos,descripcion_aseguradora,"+
									"osiris_erp_cobros_enca.nombre_empresa_encabezado,observacion_ingreso,osiris_his_paciente.alegias_paciente,osiris_his_paciente.religion_paciente,"+
									"osiris_erp_cobros_enca.id_empresa AS idempresa_enca "+
									"FROM osiris_erp_cobros_enca,osiris_his_medicos,osiris_empresas,osiris_erp_movcargos,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_tipo_diagnosticos,osiris_his_tipo_admisiones,osiris_his_tipo_especialidad,osiris_aseguradoras "+
									"WHERE osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
									"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
									"AND osiris_erp_cobros_enca.pid_paciente = osiris_erp_movcargos.pid_paciente "+
									"AND osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente "+
									"AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_erp_movcargos.id_tipo_cirugia "+
									"AND osiris_his_tipo_diagnosticos.id_diagnostico = osiris_erp_movcargos.id_diagnostico "+
									"AND osiris_empresas.id_empresa = osiris_erp_cobros_enca.id_empresa "+
									"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
									"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_movcargos.id_tipo_paciente "+
									"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
									"AND osiris_his_paciente.pid_paciente = '"+PidPaciente.ToString()+"' "+
        							"AND osiris_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString()+"'";
        						
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if( (bool) lector.Read())
				{
					medico_tratante = (string) lector["nombre_medico_tratante"];
					string edadpac = (string) lector["edad"];
					string mesespac = (string) lector["mesesedad"];
					//string varpaso = (string) lector["descripcion_admisiones"];
					imprime_encabezado(cr,layout);
					fontSize = 8.0;
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					/////////COMIENZA IMPRESION DE CUERPO DE DOCUMENTO//////////////////
					layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
					cr.MoveTo(489.5*escala_en_linux_windows, 20*escala_en_linux_windows);					layout.SetText("Nº Expediente");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(509.5*escala_en_linux_windows, 30*escala_en_linux_windows);					layout.SetText(" "+PidPaciente.ToString());
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(509.5*escala_en_linux_windows, 40*escala_en_linux_windows);					layout.SetText("PID");
					cr.MoveTo(240*escala_en_linux_windows, 100*escala_en_linux_windows);		    		layout.SetText("DATOS GENERALES DEL PACIENTE");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;   // Letra Normal
					int numero_linea = 120;					
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Nombre: "+(string) lector["nombre1_paciente"].ToString().Trim()+" "+ 
																		                 									(string) lector["nombre2_paciente"].ToString().Trim()+" "+
																		                 									(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
																		                 									(string) lector["apellido_materno_paciente"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(56*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("_________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(290*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("F. de Nac: "+(string) lector["fecha_nac_pa"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(329*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("________________");					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(410*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Edad: "+edadpac+" años "+mesespac+" Meses");				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(430*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("______________________");					Pango.CairoHelper.ShowLayout (cr, layout);
					
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("C.U.R.P.: ");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(290*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("R.F.C.: ");		Pango.CairoHelper.ShowLayout (cr, layout);
					
					numero_linea += separacionlineas;
					
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Dirección: "+(string) lector["direccion_paciente"]+"  "+
																	(string) lector["numero_casa_paciente"]+" "+(string) lector["numero_departamento_paciente"]+", Col. "+
																	(string) lector["colonia_paciente"]+ ", CP. "+(string) lector["codigo_postal_paciente"]+", "+(string) lector["municipio_paciente"]+", "+(string) lector["estado_paciente"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(58*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("________________________________________________________________________________________________");					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(445*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Telefono :"+(string) lector["telefono_particular1_paciente"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Ocupación: "+(string) lector["ocupacion_paciente"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(58*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("________________________________");					Pango.CairoHelper.ShowLayout (cr, layout);
					
					if((int) lector["idempresa_enca"] == 1){
						cr.MoveTo(310*escala_en_linux_windows, numero_linea*escala_en_linux_windows);				layout.SetText("Nombre de la Empresa:  "+(string) lector["nombre_empresa_encabezado"]);		Pango.CairoHelper.ShowLayout (cr, layout);
					}else{
						cr.MoveTo(310*escala_en_linux_windows, numero_linea*escala_en_linux_windows);				layout.SetText("Nombre de la Empresa:  "+(string) lector["descripcion_empresa"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					}
					
					cr.MoveTo(411*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("___________________________________");
					
					numero_linea += separacionlineas;
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Dirección Empresa:  "+(string) lector["direccion_empresa"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(90*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("______________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(310*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Tel. de la Empresa:  "+(string) lector["telefono1_empresa"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(380*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("_________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(85*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("______________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);	
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(310*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Estado Civil:"+" "+(string) lector["estado_civil_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(357*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("_______________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					string sexo_paciente = "";
					if((string) lector["sexo_paciente"] == "H"){
						sexo_paciente = "Masculino";
					}else{
						sexo_paciente = "Femenino";					}
					
					cr.MoveTo(500*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Sexo: "+sexo_paciente);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(521*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("____________");					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Religion: "+(string) lector["religion_paciente"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(310*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Alergias: "+(string) lector["alegias_paciente"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					//cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Referido por: ");					Pango.CairoHelper.ShowLayout (cr, layout);
					//numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Observaciones: "+(string) lector["observacion_ingreso"]);					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					numero_linea += separacionlineas;
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(260*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("DATOS DEL RESPONSABLE");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					numero_linea += separacionlineas;
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Nombre de la persona responsable del paciente:  "+(string) lector["responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(193*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("___________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Dirección:  "+(string) lector["direccion_responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(56*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("___________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Tel:  "+(string) lector["telefono1_responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(37*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("_________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Ocupación del responsable:  "+(string) lector["ocupacion_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(126*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("__________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Parentesco:  "+(string) lector["parentezco"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(393*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Empresa donde labora:  "+(string) lector["empresa_labora_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(103*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("_______________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(345*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Tel. de Empresa:  "+(string) lector["telefono_emp_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(406*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Responsable de la cuenta(Aseguradora y/o membresia):  "+(string) lector["descripcion_aseguradora"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(224*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("__________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(450*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("Nº de poliza:  "+(string) lector["numero_poliza"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(498*escala_en_linux_windows, numero_linea*escala_en_linux_windows);					layout.SetText("____________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					numero_linea += separacionlineas;
					cr.MoveTo(20*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("Dirección:  "+(string) lector["direccion_emp_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows, numero_linea*escala_en_linux_windows);						layout.SetText("___________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(270*escala_en_linux_windows, 350*escala_en_linux_windows);					layout.SetText("DATOS DE ADMISION");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 375*escala_en_linux_windows);					    layout.SetText("Nº de Ingreso:  "+ folioservicio.ToString());
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(79*escala_en_linux_windows, 375*escala_en_linux_windows);						layout.SetText("__________");
				    Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(140*escala_en_linux_windows, 375*escala_en_linux_windows);			    	layout.SetText("Nº de habitacion:  ");
			    	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(200*escala_en_linux_windows, 375*escala_en_linux_windows);					layout.SetText("____________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(280*escala_en_linux_windows, 375*escala_en_linux_windows);			    	layout.SetText("Fecha de Admision:  "+ (string) lector["fecha_reg_adm"]);
			    	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows, 375*escala_en_linux_windows);					layout.SetText("_____________");
			    	Pango.CairoHelper.ShowLayout (cr, layout);
			    	cr.MoveTo(430*escala_en_linux_windows, 375*escala_en_linux_windows);			    	layout.SetText("Hora de admision:"+" "+ (string) lector["hora_reg_adm"]); //DateTime.Now.ToString("HH:mm"));
			    	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(495*escala_en_linux_windows, 375*escala_en_linux_windows);					layout.SetText("_____________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					
					if ((int) lector["id_medico"] > 1){
						cr.MoveTo(20*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("Medico 1º Diag.:  "+(string) lector["nombre_medico"]);
						Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(80*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("_____________________________________________");
						Pango.CairoHelper.ShowLayout (cr, layout);
					}else{
						cr.MoveTo(20*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("Medico 1º Diag.:  "+(string) lector["nombre_medico_encabezado"]);
						Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(80*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("_____________________________________________");
						Pango.CairoHelper.ShowLayout (cr, layout);
					}
					
					cr.MoveTo(350*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("Especialidad:  "+(string) lector["descripcion_especialidad"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(400*escala_en_linux_windows, 390*escala_en_linux_windows);					layout.SetText("____________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(383*escala_en_linux_windows, 439*escala_en_linux_windows);					layout.SetText("Firma:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(408*escala_en_linux_windows, 439*escala_en_linux_windows);					layout.SetText("_______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(383*escala_en_linux_windows, 422*escala_en_linux_windows);					layout.SetText("Ced. Prof.:  "+(string) lector["cedula_medico"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(424*escala_en_linux_windows, 422*escala_en_linux_windows);					layout.SetText("______________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 406*escala_en_linux_windows);						layout.SetText("Cirugia: "+(string) lector["descripcion_cirugia"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(50*escala_en_linux_windows, 406*escala_en_linux_windows);						layout.SetText("___________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 422*escala_en_linux_windows);						layout.SetText("Ingresado por: "+(string) lector["id_empleado_admision"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(75*escala_en_linux_windows, 422*escala_en_linux_windows);						layout.SetText("________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(220*escala_en_linux_windows, 453*escala_en_linux_windows);					layout.SetText("PARA SER LLENADO POR EL MEDICO TRATANTE");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(20*escala_en_linux_windows, 465*escala_en_linux_windows);						layout.SetText("Medico Tratante: "+medico_tratante);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(82*escala_en_linux_windows, 465*escala_en_linux_windows);						layout.SetText("__________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 477*escala_en_linux_windows);						layout.SetText("Diagnostico:  "+(string) lector["descripcion_diagnostico_movcargos"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(70*escala_en_linux_windows, 477*escala_en_linux_windows);						layout.SetText("__________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(20*escala_en_linux_windows, 489*escala_en_linux_windows);						layout.SetText("Observaciones: ");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(82*escala_en_linux_windows, 489*escala_en_linux_windows);						layout.SetText("_____________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 501*escala_en_linux_windows);						layout.SetText("Diagnostico provisional (Para ser llenado dentro de las primeras 24 Hrs):");
					Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(20*escala_en_linux_windows, 513*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 525*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 537*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 549*escala_en_linux_windows);						layout.SetText("Diagnostico Final:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 561*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 573*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 585*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 597*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(270*escala_en_linux_windows, 609*escala_en_linux_windows);					layout.SetText("CAUSA DE EGRESO");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(20*escala_en_linux_windows, 621*escala_en_linux_windows);						layout.SetText("Por Mejoria:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows, 621*escala_en_linux_windows);						layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 633*escala_en_linux_windows);						layout.SetText("Evolucion:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows, 633*escala_en_linux_windows);						layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(200*escala_en_linux_windows, 633*escala_en_linux_windows);						layout.SetText("Por traslado:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(245*escala_en_linux_windows, 633*escala_en_linux_windows);						layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(200*escala_en_linux_windows, 621*escala_en_linux_windows);					layout.SetText("Alta Voluntaria:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(260*escala_en_linux_windows, 621*escala_en_linux_windows);					layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(383*escala_en_linux_windows, 621*escala_en_linux_windows);					layout.SetText("Por no Mejoria:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(444*escala_en_linux_windows, 621*escala_en_linux_windows);					layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(383*escala_en_linux_windows, 633*escala_en_linux_windows);					layout.SetText("Por Defunción:");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(444*escala_en_linux_windows, 633*escala_en_linux_windows);					layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 668*escala_en_linux_windows);					layout.SetText("______________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(40*escala_en_linux_windows, 677*escala_en_linux_windows);					layout.SetText("medico_tratante");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(435*escala_en_linux_windows, 677*escala_en_linux_windows);					layout.SetText("FIRMA DE MEDICO TRATANTE");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(400*escala_en_linux_windows, 668*escala_en_linux_windows);						layout.SetText("____________________________________");
		    		Pango.CairoHelper.ShowLayout (cr, layout);
		    		cr.MoveTo(220*escala_en_linux_windows, 735*escala_en_linux_windows);						layout.SetText("Nombre y Firma Paciente o responsable");
		    		Pango.CairoHelper.ShowLayout (cr, layout);
		    		//cr.MoveTo(250*escala_en_linux_windows, 700*escala_en_linux_windows);			    		layout.SetText("verificacion de datos");
					//Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 724*escala_en_linux_windows);						layout.SetText("____________________________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(480*escala_en_linux_windows, 760*escala_en_linux_windows) ;					layout.SetText("Sistema Hospitalario OSIRIS");
					Pango.CairoHelper.ShowLayout (cr, layout);
					
					/*string varpaso = (string) lector["descripcion_admisiones"];
					while ((bool) lector.Read()){
							varpaso = varpaso +", "+(string) lector["descripcion_admisiones"]; 
					}	
					cr.MoveTo(20*escala_en_linux_windows, 410*escala_en_linux_windows);						layout.SetText("Admisión:  "+(string) varpaso);
					cr.MoveTo(60*escala_en_linux_windows, 410*escala_en_linux_windows);						layout.SetText("__________________________________________________________________");
					*/
	        	}
        	
				lector.Close (); 
				conexion.Close ();
				//cr.ShowPage();
			}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}
			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			Gtk.Image image5 = new Gtk.Image();
            image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			cr.Fill();
			//cr.Restore();	
			cr.Paint();
			cr.Restore();
			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			//cr.MoveTo(20*escala_en_linux_windows,10*escala_en_linux_windows);					layout.SetText(classpublic.nombre_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(20*escala_en_linux_windows,20*escala_en_linux_windows);					layout.SetText(classpublic.direccion_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(20*escala_en_linux_windows,30*escala_en_linux_windows);					layout.SetText(classpublic.telefonofax_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(20*escala_en_linux_windows,70*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 12.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(230*escala_en_linux_windows, 55*escala_en_linux_windows);										layout.SetText("PROTOCOLO DE ADMISION");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(280*escala_en_linux_windows, 65*escala_en_linux_windows);	    								layout.SetText("REGISTRO");	
			Pango.CairoHelper.ShowLayout (cr, layout); 
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
		}
			
		void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{	
				
		}
 	}    
 }
