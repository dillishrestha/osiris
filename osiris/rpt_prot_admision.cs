// created on 18/04/2007 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
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
// Objeto		: rpt_prot_admision.cs
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class protocolo_admision
	{
		string connectionString;
        string nombrebd;
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows = 1;		// Linux = 1  Windows = 8
				
		int PidPaciente = 0;
		int folioservicio = 0;
		string medico_tratante = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//public System.Drawing.Image myimage;
		
		class_conexion conexion_a_DB = new class_conexion();
	
		public protocolo_admision ( int PidPaciente_ , int folioservicio_,string _nombrebd_,string medico_tratante_)
		{
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			//nombrebd = _nombrebd_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			//medico_tratante = medico_tratante_;
		
			print = new PrintOperation ();			
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
		/*	//PrintJob trabajo   = new PrintJob (PrintConfig.Default());
        	//PrintDialog dialogo   = new PrintDialog (trabajo, "PROTOCOLO DE ADMISION", 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}

        	PrintContext ctx = trabajo.Context;
        
        	ComponerPagina(ctx, trabajo); 

        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
                      	new PrintJobPreview(trabajo, "PROTOCOLO DE ADMISION").Show();
                        break;
        	}

			dialogo.Hide (); dialogo.Dispose ();*/
       }
		
		void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			PrintContext context = args.Context;											
			print.NPages = 1; // crea cantidad de copias del reporte
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
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
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
									"osiris_erp_cobros_enca.nombre_empresa_encabezado, "+
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
					imprime_encabezado(context);
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
					cr.MoveTo(20*escala_en_linux_windows, 120*escala_en_linux_windows);						layout.SetText("Nombre: "+(string) lector["nombre1_paciente"]+" "+ 
																		                 									(string) lector["nombre2_paciente"]+" "+
																		                 									(string) lector["apellido_paterno_paciente"]+" "+
																		                 									(string) lector["apellido_materno_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(56*escala_en_linux_windows, 120*escala_en_linux_windows);						layout.SetText("_________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(290*escala_en_linux_windows, 120*escala_en_linux_windows);					layout.SetText("F. de Nac: "+(string) lector["fecha_nac_pa"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(329*escala_en_linux_windows, 120*escala_en_linux_windows);					layout.SetText("________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(410*escala_en_linux_windows, 120*escala_en_linux_windows);					layout.SetText("Edad:  "+edadpac+" años "+mesespac+" Meses");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(430*escala_en_linux_windows, 120*escala_en_linux_windows);					layout.SetText("______________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 130*escala_en_linux_windows);						layout.SetText("Dirección: "+(string) lector["direccion_paciente"]+"  "+
																	(string) lector["numero_casa_paciente"]+" "+(string) lector["numero_departamento_paciente"]+", Col. "+
																	(string) lector["colonia_paciente"]+ ", CP. "+(string) lector["codigo_postal_paciente"]+", "+(string) lector["municipio_paciente"]+", "+(string) lector["estado_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(58*escala_en_linux_windows, 130*escala_en_linux_windows);						layout.SetText("________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 150*escala_en_linux_windows);						layout.SetText("Ocupación: "+(string) lector["ocupacion_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(58*escala_en_linux_windows, 150*escala_en_linux_windows);						layout.SetText("________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					
					if((int) lector["idempresa_enca"] == 1){
						cr.MoveTo(320*escala_en_linux_windows, 150*escala_en_linux_windows);				layout.SetText("Nombre de la Empresa:  "+(string) lector["nombre_empresa_encabezado"]);		Pango.CairoHelper.ShowLayout (cr, layout);
					}else{
						cr.MoveTo(320*escala_en_linux_windows, 150*escala_en_linux_windows);				layout.SetText("Nombre de la Empresa:  "+(string) lector["descripcion_empresa"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					}
					
					cr.MoveTo(411*escala_en_linux_windows, 150*escala_en_linux_windows);					layout.SetText("___________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 175*escala_en_linux_windows);						layout.SetText("Dirección Empresa:  "+(string) lector["direccion_empresa"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(90*escala_en_linux_windows, 175*escala_en_linux_windows);						layout.SetText("______________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(310*escala_en_linux_windows, 175*escala_en_linux_windows);					layout.SetText("Tel. de la Empresa:  "+(string) lector["telefono1_empresa"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(380*escala_en_linux_windows, 175*escala_en_linux_windows);					layout.SetText("_________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(19.5*escala_en_linux_windows, 190*escala_en_linux_windows);					layout.SetText("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 190*escala_en_linux_windows);						layout.SetText("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(85*escala_en_linux_windows, 190*escala_en_linux_windows);						layout.SetText("______________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);	
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(310*escala_en_linux_windows, 190*escala_en_linux_windows);					layout.SetText("Estado Civil:"+" "+(string) lector["estado_civil_paciente"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(357*escala_en_linux_windows, 190*escala_en_linux_windows);					layout.SetText("_______________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					string sexo_paciente = "";
					if((string) lector["sexo_paciente"] == "H"){
						sexo_paciente = "Masculino";
					}else{
						sexo_paciente = "Femenino";
					}
					
					cr.MoveTo(500*escala_en_linux_windows, 190*escala_en_linux_windows);					layout.SetText("Sexo: "+sexo_paciente);//680
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(521*escala_en_linux_windows, 190*escala_en_linux_windows);					layout.SetText("____________");//680
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(260*escala_en_linux_windows, 210*escala_en_linux_windows);					layout.SetText("DATOS DEL RESPONSABLE");
					Pango.CairoHelper.ShowLayout (cr, layout);
					layout.FontDescription.Weight = Weight.Normal;
					cr.MoveTo(20*escala_en_linux_windows, 225*escala_en_linux_windows);						layout.SetText("Nombre de la persona responsable del paciente:  "+(string) lector["responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(193*escala_en_linux_windows, 225*escala_en_linux_windows);					layout.SetText("___________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 240*escala_en_linux_windows);						layout.SetText("Dirección:  "+(string) lector["direccion_responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(56*escala_en_linux_windows, 240*escala_en_linux_windows);						layout.SetText("___________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 258*escala_en_linux_windows);						layout.SetText("Tel:  "+(string) lector["telefono1_responsable_cuenta"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(37*escala_en_linux_windows, 258*escala_en_linux_windows);						layout.SetText("_________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 275*escala_en_linux_windows);						layout.SetText("Ocupación del responsable:  "+(string) lector["ocupacion_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(126*escala_en_linux_windows, 275*escala_en_linux_windows);					layout.SetText("__________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows, 275*escala_en_linux_windows);					layout.SetText("Parentesco:  "+(string) lector["parentezco"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(393*escala_en_linux_windows, 275*escala_en_linux_windows);					layout.SetText("________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 290*escala_en_linux_windows);						layout.SetText("Empresa donde labora:  "+(string) lector["empresa_labora_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(103*escala_en_linux_windows, 290*escala_en_linux_windows);					layout.SetText("_______________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(345*escala_en_linux_windows, 290*escala_en_linux_windows);					layout.SetText("Tel. de Empresa:  "+(string) lector["telefono_emp_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(406*escala_en_linux_windows, 290*escala_en_linux_windows);					layout.SetText("________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 307*escala_en_linux_windows);						layout.SetText("Responsable de la cuenta(Aseguradora y/o membresia):  "+(string) lector["descripcion_aseguradora"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(224*escala_en_linux_windows, 307*escala_en_linux_windows);					layout.SetText("__________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(450*escala_en_linux_windows, 307*escala_en_linux_windows);					layout.SetText("Nº de poliza:  "+(string) lector["numero_poliza"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(498*escala_en_linux_windows, 307*escala_en_linux_windows);					layout.SetText("____________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(20*escala_en_linux_windows, 322*escala_en_linux_windows);						layout.SetText("Dirección:  "+(string) lector["direccion_emp_responsable"]);
					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows, 322*escala_en_linux_windows);						layout.SetText("___________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);		
					layout.FontDescription.Weight = Weight.Bold;
					cr.MoveTo(0*escala_en_linux_windows, 340*escala_en_linux_windows);						layout.SetText("_______________________________________________________________________________________________________________");
					Pango.CairoHelper.ShowLayout (cr, layout);
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
					
					if ((int) lector["id_medico"] > 1)
					{
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
					cr.MoveTo(480*escala_en_linux_windows, 760*escala_en_linux_windows) ;					layout.SetText("'Salud es fuerza de trabajo'");
					Pango.CairoHelper.ShowLayout (cr, layout);
					
					/*string varpaso = (string) lector["descripcion_admisiones"];
					while ((bool) lector.Read())
						{
							varpaso = varpaso +", "+(string) lector["descripcion_admisiones"]; 
						}	
					cr.MoveTo(20*escala_en_linux_windows, 410*escala_en_linux_windows);						layout.SetText("Admisión:  "+(string) varpaso);
					cr.MoveTo(60*escala_en_linux_windows, 410*escala_en_linux_windows);						layout.SetText("__________________________________________________________________");
					*/
	        	}
        	
				lector.Close (); 
				conexion.Close ();
				cr.ShowPage();
			}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}
			
		}
		
		void imprime_encabezado(PrintContext context)
		{
			Console.WriteLine("entra en la impresion del encabezado");
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			cr.MoveTo(001,10*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,20*escala_en_linux_windows);					layout.SetText("Direccion: Monterrey - Mexico");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,30*escala_en_linux_windows);					layout.SetText("Telefono: (01)(81) 1158-5166");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(001,40*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 12.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(230*escala_en_linux_windows, 55*escala_en_linux_windows);										layout.SetText("PROTOCOLO DE ADMISION");
			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(280*escala_en_linux_windows, 65*escala_en_linux_windows);	    								layout.SetText("REGISTRO");	
			Pango.CairoHelper.ShowLayout (cr, layout); 
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
		}
			
		void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{	
				
		}
		
    	 /*
		void ComponerPagina (Gnome.PrintContext cr, Gnome.PrintJob trabajoImpresion)
		{
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
									"osiris_erp_cobros_enca.nombre_empresa_encabezado, "+
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
				cr.BeginPage("Pagina 1");
				//NUEVO
				// Crear una fuente de tipo Impact
				Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
				//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
				
				// Cambiar la fuente
				Gnome.Print.Setfont (cr, fuente);
				
				cr.MoveTo(19.5, 770);
		    	layout.SetText("Sistema Hospitalario OSIRIS");
				cr.MoveTo(20, 770);
		    	layout.SetText("Sistema Hospitalario OSIRIS");
		    
		    	cr.MoveTo(19.5, 760); 
		    	layout.SetText("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
		    	cr.MoveTo(20, 760);
		    	layout.SetText("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			
				cr.MoveTo(19.5, 750);
		    	layout.SetText("Conmutador:(81) 81-25-56-10");
				cr.MoveTo(20, 750);
		    	layout.SetText("Conmutador:(81) 81-25-56-10");
		    
		    	//se cambia el tamaño de texto por ser titulo
		    
				Gnome.Print.Setfont (cr, fuente2);
				//fin de titulo
			
				cr.MoveTo(229.5, 740);
				layout.SetText("PROTOCOLO DE ADMISION");
				cr.MoveTo(279.5, 727);
		    	layout.SetText("REGISTRO");
				cr.MoveTo(230, 740);
				layout.SetText("PROTOCOLO DE ADMISION");
				cr.MoveTo(280, 727);
		    	layout.SetText("REGISTRO");
				
				if( (bool) lector.Read())
				{
					medico_tratante = (string) lector["nombre_medico_tratante"];
					string edadpac = (string) lector["edad"];
					string mesespac = (string) lector["mesesedad"];
					//se tienen que crear las fuentes de nuevo debido a que es un while y por lo tanto es un ciclo
					Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
			    	Gnome.Font fuente4 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			    	Gnome.Font fuente5 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
			    
			    	Gnome.Print.Setfont (cr, fuente3);
			    
					cr.MoveTo(489.5, 750);					layout.SetText("Nº Expediente");
					cr.MoveTo(509.5, 740);					layout.SetText(" "+PidPaciente.ToString());
					cr.MoveTo(509.5, 730);					layout.SetText("PID");
					cr.MoveTo(490, 750);
					//layout.SetText("9999999");
					layout.SetText("Nº Expediente");
					cr.MoveTo(510, 740);					layout.SetText(" "+PidPaciente.ToString());
					cr.MoveTo(510, 730);					layout.SetText("PID");
				
					Gnome.Print.Setfont (cr, fuente5);
					cr.MoveTo(20, 720);      				layout.SetText("____________________________");
		    	
		    		Gnome.Print.Setfont (cr, fuente4);
		    		cr.MoveTo(239.5, 700);		
		    				    		layout.SetText("DATOS GENERALES DEL PACIENTE");
		    		cr.MoveTo(240, 700);
		    				    		layout.SetText("DATOS GENERALES DEL PACIENTE");
				
					cr.MoveTo(20, 680);					layout.SetText("Nombre: "+(string) lector["nombre1_paciente"]+" "+ 
				                 									(string) lector["nombre2_paciente"]+" "+
				                 									(string) lector["apellido_paterno_paciente"]+" "+
				                 									(string) lector["apellido_materno_paciente"]);
					cr.MoveTo(56, 680);					layout.SetText("_________________________________________________");
				
				
					cr.MoveTo(290, 680);					layout.SetText("F. de Nac: "+(string) lector["fecha_nac_pa"]);
					cr.MoveTo(329, 680);					layout.SetText("________________");
					
					cr.MoveTo(410, 680);					layout.SetText("Edad:  "+edadpac+" años "+mesespac+" Meses");
					cr.MoveTo(430, 680);					layout.SetText("__________");
				
					cr.MoveTo(20, 665);					layout.SetText("Dirección: "+(string) lector["direccion_paciente"]+"  "+
																	(string) lector["numero_casa_paciente"]+" "+(string) lector["numero_departamento_paciente"]+", Col. "+
																	(string) lector["colonia_paciente"]+ ", CP. "+(string) lector["codigo_postal_paciente"]+", "+(string) lector["municipio_paciente"]+", "+(string) lector["estado_paciente"]);
					cr.MoveTo(58, 665);					layout.SetText("________________________________________________________________________________________________");
									
					cr.MoveTo(20, 650);					layout.SetText("Ocupación: "+(string) lector["ocupacion_paciente"]);
					cr.MoveTo(58, 650);					layout.SetText("________________________________");
					
					if((int) lector["idempresa_enca"] == 1){
						cr.MoveTo(320, 650);					layout.SetText("Nombre de la Empresa:  "+(string) lector["nombre_empresa_encabezado"]);
					}else{
						cr.MoveTo(320, 650);					layout.SetText("Nombre de la Empresa:  "+(string) lector["descripcion_empresa"]);	
					}
					
					cr.MoveTo(415, 650);					layout.SetText("___________________________________");
				
					cr.MoveTo(20, 635);					layout.SetText("Dirección Empresa:  "+(string) lector["direccion_empresa"]);
					cr.MoveTo(90, 635);					layout.SetText("______________________________________");
				
					cr.MoveTo(310, 635);					layout.SetText("Tel. de la Empresa:  "+(string) lector["telefono1_empresa"]);
					cr.MoveTo(380, 635);					layout.SetText("_________________________");
				
					cr.MoveTo(19.5, 620);					layout.SetText("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					cr.MoveTo(20, 620);					layout.SetText("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					cr.MoveTo(85, 619.85);					layout.SetText("______________________________");
					cr.MoveTo(85, 620);					layout.SetText("______________________________");
													
					cr.MoveTo(310, 620);					layout.SetText("Estado Civil:"+" "+(string) lector["estado_civil_paciente"]);
					cr.MoveTo(360, 620);					layout.SetText("_______________");
					
					string sexo_paciente = "";
					if((string) lector["sexo_paciente"] == "H"){
						sexo_paciente = "Masculino";
					}else{
						sexo_paciente = "Femenino";
					}
					
					cr.MoveTo(500, 620);					layout.SetText("Sexo: "+sexo_paciente);//680
					cr.MoveTo(521, 620);					layout.SetText("______");//680
				
					Gnome.Print.Setfont (cr, fuente5);
					cr.MoveTo(20, 610);					layout.SetText("____________________________");
					
					Gnome.Print.Setfont (cr, fuente4);
					cr.MoveTo(259.5, 590);		
										layout.SetText("DATOS DEL RESPONSABLE");
					cr.MoveTo(260, 590);
										layout.SetText("DATOS DEL RESPONSABLE");
				
					cr.MoveTo(20, 575);					layout.SetText("Nombre de la persona responsable del paciente:  "+(string) lector["responsable_cuenta"]);
					cr.MoveTo(190, 575);					layout.SetText("___________________________________________________________");
				
					cr.MoveTo(20, 560);					layout.SetText("Dirección:  "+(string) lector["direccion_responsable_cuenta"]);
					cr.MoveTo(56, 560);					layout.SetText("___________________________________________________________________________________________");
				
					cr.MoveTo(490, 560);					layout.SetText("Tel:  "+(string) lector["telefono1_responsable_cuenta"]);
					cr.MoveTo(507, 560);					layout.SetText("_________________");
				
					cr.MoveTo(20, 545);					layout.SetText("Ocupación del responsable:  "+(string) lector["ocupacion_responsable"]);
					cr.MoveTo(120, 545);					layout.SetText("__________________________________");
							
					cr.MoveTo(350, 545);					layout.SetText("Parentesco:  "+(string) lector["parentezco"]);
					cr.MoveTo(390, 545);					layout.SetText("________________________");
				
					cr.MoveTo(20, 530);					layout.SetText("Empresa donde labora:  "+(string) lector["empresa_labora_responsable"]);
					cr.MoveTo(100, 530);					layout.SetText("_______________________________________");
				
					cr.MoveTo(345, 530);					layout.SetText("Tel. de Empresa:  "+(string) lector["telefono_emp_responsable"]);
					cr.MoveTo(404, 530);					layout.SetText("________________________");
					
					cr.MoveTo(20, 500);					layout.SetText("Responsable de la cuenta(Aseguradora y/o membresia):  "+(string) lector["descripcion_aseguradora"]);
					cr.MoveTo(220, 500);					layout.SetText("__________________________________________");
				
					cr.MoveTo(450, 500);					layout.SetText("Nº de poliza:  "+(string) lector["numero_poliza"]);
					cr.MoveTo(495, 500);					layout.SetText("____________________");
					
					cr.MoveTo(20, 515);					layout.SetText("Dirección:  "+(string) lector["direccion_emp_responsable"]);
					cr.MoveTo(60, 515);					layout.SetText("___________________________________________________________________________");
							
					Gnome.Print.Setfont (cr, fuente5);
					cr.MoveTo(20, 490);					layout.SetText("____________________________");
				
					Gnome.Print.Setfont (cr, fuente4);
					cr.MoveTo(269.5, 470);	
											layout.SetText("DATOS DE ADMISION");
					cr.MoveTo(270, 470);
											layout.SetText("DATOS DE ADMISION");
				
					cr.MoveTo(19.5, 455);				    layout.SetText("Nº de Ingreso:  "+ folioservicio.ToString());
					cr.MoveTo(20, 455);				    layout.SetText("Nº de Ingreso:  "+ folioservicio.ToString());
					cr.MoveTo(70, 454.85);					layout.SetText("__________");
					cr.MoveTo(70, 455);					layout.SetText("__________");
				
					cr.MoveTo(140, 455);			    	layout.SetText("Nº de habitacion:  ");
			    	cr.MoveTo(200, 455);					layout.SetText("__________");
					
					cr.MoveTo(280, 455);			    	layout.SetText("Fecha de Admision:  "+ (string) lector["fecha_reg_adm"]);
			    	cr.MoveTo(350, 455);					layout.SetText("_____________");
			    	
			    	cr.MoveTo(430, 455);			    	layout.SetText("Hora de admision:"+" "+ (string) lector["hora_reg_adm"]); //DateTime.Now.ToString("HH:mm"));
			    	cr.MoveTo(495, 455);					layout.SetText("_______");
					
					if ((int) lector["id_medico"] > 1)
					{
						cr.MoveTo(20, 440);					layout.SetText("Medico 1º Diag.:  "+(string) lector["nombre_medico"]);
						cr.MoveTo(100, 440);					layout.SetText("_____________________________________________");
					}else{
						cr.MoveTo(20, 440);					layout.SetText("Medico 1º Diag.:  "+(string) lector["nombre_medico_encabezado"]);
						cr.MoveTo(100, 440);					layout.SetText("_____________________________________________");
					}
					
					cr.MoveTo(350, 440);					layout.SetText("Especialidad:  "+(string) lector["descripcion_especialidad"]);
					cr.MoveTo(400, 440);					layout.SetText("____________________________________");
				
					cr.MoveTo(20, 425);					layout.SetText("Firma:");
					cr.MoveTo(45, 425);					layout.SetText("_______________________");
				
					cr.MoveTo(383, 425);					layout.SetText("Ced. Prof.:  "+(string) lector["cedula_medico"]);
					cr.MoveTo(424, 425);					layout.SetText("______________________________");
				
					cr.MoveTo(20, 395);					layout.SetText("Cirugia: "+(string) lector["descripcion_cirugia"]);
					cr.MoveTo(50, 395);					layout.SetText("___________________________________________________________________________________________________");
					
					cr.MoveTo(370, 410);					layout.SetText("Ingresado por: "+(string) lector["id_empleado_admision"]);
					cr.MoveTo(425, 410);					layout.SetText("________________________________");
					
					Gnome.Print.Setfont (cr, fuente5);
					cr.MoveTo(20, 385);					layout.SetText("____________________________");
				
					Gnome.Print.Setfont (cr, fuente4);
					cr.MoveTo(219.5, 365);
					layout.SetText("PARA SER LLENADO POR EL MEDICO TRATANTE");
					cr.MoveTo(220, 365);
					layout.SetText("PARA SER LLENADO POR EL MEDICO TRATANTE");
					
					cr.MoveTo(20, 352);					layout.SetText("Medico Tratante: "+medico_tratante);
					cr.MoveTo(20.5, 352);					layout.SetText("Medico Tratante: "+medico_tratante);
					
					cr.MoveTo(20, 340);					layout.SetText("Diagnostico:  "+(string) lector["descripcion_diagnostico_movcargos"]);
					cr.MoveTo(70, 340);					layout.SetText("__________________________________________________________________________________________________________");
					
					cr.MoveTo(20, 325);					layout.SetText("Observaciones: ");
					cr.MoveTo(82, 325);					layout.SetText("_____________________________________________________");
				
					cr.MoveTo(20, 310);					layout.SetText("Diagnostico provisional (Para ser llenado dentro de las primeras 24 Hrs):");
					cr.MoveTo(20, 295);					layout.SetText("____________________________________________________________________________________________________________________________");
					cr.MoveTo(20, 280);					layout.SetText("____________________________________________________________________________________________________________________________");
					cr.MoveTo(20, 265);					layout.SetText("____________________________________________________________________________________________________________________________");
				
					cr.MoveTo(20, 250);					layout.SetText("Diagnostico Final:");
					cr.MoveTo(20, 235);					layout.SetText("____________________________________________________________________________________________________________________________");
					cr.MoveTo(20, 220);					layout.SetText("____________________________________________________________________________________________________________________________");
					cr.MoveTo(20, 205);					layout.SetText("____________________________________________________________________________________________________________________________");
				
					Gnome.Print.Setfont (cr, fuente5);
					cr.MoveTo(20, 205);					layout.SetText("____________________________");
				
					Gnome.Print.Setfont (cr, fuente4);
					cr.MoveTo(269.5, 185);
					layout.SetText("CAUSA DE EGRESO");
					cr.MoveTo(270, 185);
					layout.SetText("CAUSA DE EGRESO");
				
					cr.MoveTo(20, 170);					layout.SetText("Por Mejoria:");
					cr.MoveTo(60, 170);					layout.SetText("______________________");
					
					cr.MoveTo(20, 155);					layout.SetText("Evolucion:");
					cr.MoveTo(60, 155);					layout.SetText("______________________");
					
					cr.MoveTo(20, 140);					layout.SetText("Por traslado:");
					cr.MoveTo(60, 140);					layout.SetText("______________________");
					
					cr.MoveTo(200, 170);					layout.SetText("Alta Voluntaria:");
					cr.MoveTo(261, 170);					layout.SetText("______________________");
				
					cr.MoveTo(200, 155);					layout.SetText("Por no Mejoria:");
					cr.MoveTo(261, 155);					layout.SetText("______________________");
					
					cr.MoveTo(200, 140);					layout.SetText("Por Defunción:");
					cr.MoveTo(261, 140);					layout.SetText("______________________");
					
					cr.MoveTo(440, 160);					layout.SetText("______________________________");
					cr.MoveTo(455, 160);					layout.SetText(medico_tratante);
					cr.MoveTo(450, 150);					layout.SetText("FIRMA DE MEDICO TRATANTE");
					
					cr.MoveTo(220, 90);
							    		layout.SetText("____________________________________");
		    		cr.MoveTo(220, 80);
		    				    		layout.SetText("Nombre y Firma Paciente o responsable");
		    		cr.MoveTo(250, 70);
		    				    		layout.SetText("verificacion de datos");
					
					
					cr.MoveTo(20, 50);
					layout.SetText("____________________________________________________________________________________________________________________________");
				
					cr.MoveTo(480, 40) ;
					layout.SetText("'Salud es fuerza de trabajo'");
					string varpaso = (string) lector["descripcion_admisiones"];
				
					while ((bool) lector.Read())
					{
						varpaso = varpaso +", "+(string) lector["descripcion_admisiones"]; 
					}
					cr.MoveTo(20, 410);
					layout.SetText("Admisión:  "+(string) varpaso);
					cr.MoveTo(60, 410);
					layout.SetText("__________________________________________________________________");
				
        		}
        	
				lector.Close (); 
				conexion.Close ();
			
				layout.SetText(Page();
			}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}
			
		}
		*/
 	}    
 }
