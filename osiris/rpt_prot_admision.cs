// created on 18/04/2007 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
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
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class protocolo_admision
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public string medico_tratante = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//public System.Drawing.Image myimage;
	
		public protocolo_admision ( int PidPaciente_ , int folioservicio_,string _nombrebd_,string medico_tratante_)
		{
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			nombrebd = _nombrebd_;
			//medico_tratante = medico_tratante_;
		
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "PROTOCOLO DE ADMISION", 0);
        	int         respuesta = dialogo.Run ();
        
			if (respuesta == (int) PrintButtons.Cancel) 
			{
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
                      	new PrintJobPreview(trabajo, "PROTOCOLO DE ADMISION").Show();
                        break;
        	}

			dialogo.Hide (); dialogo.Dispose ();
       }
      
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
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
									"hscmty_erp_cobros_enca.folio_de_servicio,hscmty_erp_cobros_enca.pid_paciente, hscmty_erp_cobros_enca.id_empleado_admision,hscmty_erp_cobros_enca.nombre_medico_encabezado,hscmty_erp_cobros_enca.id_medico,"+
									"hscmty_his_paciente.nombre1_paciente,hscmty_his_paciente.nombre2_paciente,hscmty_his_paciente.apellido_paterno_paciente,hscmty_his_paciente.apellido_materno_paciente, "+
									"to_char(fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fecha_nac_pa,hscmty_his_paciente.sexo_paciente,hscmty_his_paciente.direccion_paciente,hscmty_his_paciente.numero_casa_paciente, "+
									"hscmty_his_paciente.numero_departamento_paciente,hscmty_his_paciente.colonia_paciente,hscmty_his_paciente.municipio_paciente,hscmty_his_paciente.codigo_postal_paciente,hscmty_his_paciente.estado_paciente, "+
									"hscmty_his_paciente.estado_civil_paciente,hscmty_his_paciente.ocupacion_paciente,hscmty_his_paciente.telefono_particular1_paciente, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',hscmty_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"hscmty_empresas.descripcion_empresa,hscmty_empresas.direccion_empresa,hscmty_empresas.telefono1_empresa,hscmty_empresas.numero_direccion_empresa,hscmty_empresas.colonia_empresa,hscmty_his_tipo_pacientes.descripcion_tipo_paciente, "+
									"hscmty_erp_cobros_enca.responsable_cuenta,hscmty_erp_cobros_enca.direccion_responsable_cuenta,hscmty_erp_cobros_enca.telefono1_responsable_cuenta, "+
									"hscmty_erp_cobros_enca.ocupacion_responsable,hscmty_erp_cobros_enca.parentezco,hscmty_erp_cobros_enca.empresa_labora_responsable, "+
									"hscmty_erp_cobros_enca.direccion_emp_responsable,hscmty_erp_cobros_enca.telefono_emp_responsable,hscmty_erp_cobros_enca.paciente_asegurado, "+
									"hscmty_erp_cobros_enca.numero_poliza,hscmty_his_medicos.nombre_medico,hscmty_his_medicos.id_especialidad,hscmty_his_tipo_especialidad.descripcion_especialidad, hscmty_his_medicos.cedula_medico, "+
									"hscmty_erp_movcargos.folio_de_servicio,to_char(fechahora_admision_registro,'dd-MM-yyyy') AS fecha_reg_adm, "+
									"hscmty_erp_cobros_enca.nombre_medico_tratante,"+
									"to_char(fechahora_admision_registro,'HH24:mi:ss') AS hora_reg_adm,hscmty_his_tipo_admisiones.descripcion_admisiones, "+
									"hscmty_his_tipo_cirugias.descripcion_cirugia,hscmty_his_tipo_diagnosticos.id_diagnostico, "+
									"hscmty_his_tipo_diagnosticos.descripcion_diagnostico,descripcion_diagnostico_movcargos,descripcion_aseguradora,"+
									"hscmty_erp_cobros_enca.nombre_empresa_encabezado, "+
									"hscmty_erp_cobros_enca.id_empresa AS idempresa_enca "+
									"FROM hscmty_erp_cobros_enca,hscmty_his_medicos,hscmty_empresas,hscmty_erp_movcargos,hscmty_his_paciente,hscmty_his_tipo_pacientes,hscmty_his_tipo_cirugias,hscmty_his_tipo_diagnosticos,hscmty_his_tipo_admisiones,hscmty_his_tipo_especialidad,hscmty_aseguradoras "+
									"WHERE hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
									"AND hscmty_erp_cobros_enca.folio_de_servicio = hscmty_erp_movcargos.folio_de_servicio "+
									"AND hscmty_erp_cobros_enca.pid_paciente = hscmty_erp_movcargos.pid_paciente "+
									"AND hscmty_erp_movcargos.pid_paciente = hscmty_his_paciente.pid_paciente "+
									"AND hscmty_his_tipo_cirugias.id_tipo_cirugia = hscmty_erp_movcargos.id_tipo_cirugia "+
									"AND hscmty_his_tipo_diagnosticos.id_diagnostico = hscmty_erp_movcargos.id_diagnostico "+
									"AND hscmty_empresas.id_empresa = hscmty_erp_cobros_enca.id_empresa "+
									"AND hscmty_his_medicos.id_especialidad = hscmty_his_tipo_especialidad.id_especialidad "+
									"AND hscmty_his_tipo_pacientes.id_tipo_paciente = hscmty_erp_movcargos.id_tipo_paciente "+
									"AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
									"AND hscmty_aseguradoras.id_aseguradora = hscmty_erp_cobros_enca.id_aseguradora "+
									"AND hscmty_his_paciente.pid_paciente = '"+PidPaciente.ToString()+"' "+
        							"AND hscmty_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString()+"'";
        						
				NpgsqlDataReader lector = comando.ExecuteReader ();		
				ContextoImp.BeginPage("Pagina 1");
				//NUEVO
				// Crear una fuente de tipo Impact
				Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
				//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
				
				// Cambiar la fuente
				Gnome.Print.Setfont (ContextoImp, fuente);
				
				ContextoImp.MoveTo(19.5, 770);
		    	ContextoImp.Show("Hospital Santa Cecilia");
				ContextoImp.MoveTo(20, 770);
		    	ContextoImp.Show("Hospital Santa Cecilia");
		    
		    	ContextoImp.MoveTo(19.5, 760); 
		    	ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
		    	ContextoImp.MoveTo(20, 760);
		    	ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			
				ContextoImp.MoveTo(19.5, 750);
		    	ContextoImp.Show("Conmutador:(81) 81-25-56-10");
				ContextoImp.MoveTo(20, 750);
		    	ContextoImp.Show("Conmutador:(81) 81-25-56-10");
		    
		    	//se cambia el tamaño de texto por ser titulo
		    
				Gnome.Print.Setfont (ContextoImp, fuente2);
				//fin de titulo
			
				ContextoImp.MoveTo(229.5, 740);
				ContextoImp.Show("PROTOCOLO DE ADMISION");
				ContextoImp.MoveTo(279.5, 727);
		    	ContextoImp.Show("REGISTRO");
				ContextoImp.MoveTo(230, 740);
				ContextoImp.Show("PROTOCOLO DE ADMISION");
				ContextoImp.MoveTo(280, 727);
		    	ContextoImp.Show("REGISTRO");
				
				if( (bool) lector.Read())
				{
					medico_tratante = (string) lector["nombre_medico_tratante"];
					string edadpac = (string) lector["edad"];
					string mesespac = (string) lector["mesesedad"];
					//se tienen que crear las fuentes de nuevo debido a que es un while y por lo tanto es un ciclo
					Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
			    	Gnome.Font fuente4 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			    	Gnome.Font fuente5 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
			    
			    	Gnome.Print.Setfont (ContextoImp, fuente3);
			    
					ContextoImp.MoveTo(489.5, 750);					ContextoImp.Show("Nº Expediente");
					ContextoImp.MoveTo(509.5, 740);					ContextoImp.Show(" "+PidPaciente.ToString());
					ContextoImp.MoveTo(509.5, 730);					ContextoImp.Show("PID");
					ContextoImp.MoveTo(490, 750);
					//ContextoImp.Show("9999999");
					ContextoImp.Show("Nº Expediente");
					ContextoImp.MoveTo(510, 740);					ContextoImp.Show(" "+PidPaciente.ToString());
					ContextoImp.MoveTo(510, 730);					ContextoImp.Show("PID");
				
					Gnome.Print.Setfont (ContextoImp, fuente5);
					ContextoImp.MoveTo(20, 720);      				ContextoImp.Show("____________________________");
		    	
		    		Gnome.Print.Setfont (ContextoImp, fuente4);
		    		ContextoImp.MoveTo(239.5, 700);		
		    				    		ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
		    		ContextoImp.MoveTo(240, 700);
		    				    		ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
				
					ContextoImp.MoveTo(20, 680);					ContextoImp.Show("Nombre: "+(string) lector["nombre1_paciente"]+" "+ 
				                 									(string) lector["nombre2_paciente"]+" "+
				                 									(string) lector["apellido_paterno_paciente"]+" "+
				                 									(string) lector["apellido_materno_paciente"]);
					ContextoImp.MoveTo(56, 680);					ContextoImp.Show("_________________________________________________");
				
				
					ContextoImp.MoveTo(290, 680);					ContextoImp.Show("F. de Nac: "+(string) lector["fecha_nac_pa"]);
					ContextoImp.MoveTo(329, 680);					ContextoImp.Show("________________");
					
					ContextoImp.MoveTo(410, 680);					ContextoImp.Show("Edad:  "+edadpac+" años "+mesespac+" Meses");
					ContextoImp.MoveTo(430, 680);					ContextoImp.Show("__________");
				
					ContextoImp.MoveTo(20, 665);					ContextoImp.Show("Dirección: "+(string) lector["direccion_paciente"]+"  "+
																	(string) lector["numero_casa_paciente"]+" "+(string) lector["numero_departamento_paciente"]+", Col. "+
																	(string) lector["colonia_paciente"]+ ", CP. "+(string) lector["codigo_postal_paciente"]+", "+(string) lector["municipio_paciente"]+", "+(string) lector["estado_paciente"]);
					ContextoImp.MoveTo(58, 665);					ContextoImp.Show("________________________________________________________________________________________________");
									
					ContextoImp.MoveTo(20, 650);					ContextoImp.Show("Ocupación: "+(string) lector["ocupacion_paciente"]);
					ContextoImp.MoveTo(58, 650);					ContextoImp.Show("________________________________");
					
					if((int) lector["idempresa_enca"] == 1){
						ContextoImp.MoveTo(320, 650);					ContextoImp.Show("Nombre de la Empresa:  "+(string) lector["nombre_empresa_encabezado"]);
					}else{
						ContextoImp.MoveTo(320, 650);					ContextoImp.Show("Nombre de la Empresa:  "+(string) lector["descripcion_empresa"]);	
					}
					
					ContextoImp.MoveTo(415, 650);					ContextoImp.Show("___________________________________");
				
					ContextoImp.MoveTo(20, 635);					ContextoImp.Show("Dirección Empresa:  "+(string) lector["direccion_empresa"]);
					ContextoImp.MoveTo(90, 635);					ContextoImp.Show("______________________________________");
				
					ContextoImp.MoveTo(310, 635);					ContextoImp.Show("Tel. de la Empresa:  "+(string) lector["telefono1_empresa"]);
					ContextoImp.MoveTo(380, 635);					ContextoImp.Show("_________________________");
				
					ContextoImp.MoveTo(19.5, 620);					ContextoImp.Show("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					ContextoImp.MoveTo(20, 620);					ContextoImp.Show("Tipo de paciente:  "+(string) lector["descripcion_tipo_paciente"]);
					ContextoImp.MoveTo(85, 619.85);					ContextoImp.Show("______________________________");
					ContextoImp.MoveTo(85, 620);					ContextoImp.Show("______________________________");
													
					ContextoImp.MoveTo(310, 620);					ContextoImp.Show("Estado Civil:"+" "+(string) lector["estado_civil_paciente"]);
					ContextoImp.MoveTo(360, 620);					ContextoImp.Show("_______________");
					
					string sexo_paciente = "";
					if((string) lector["sexo_paciente"] == "H"){
						sexo_paciente = "Masculino";
					}else{
						sexo_paciente = "Femenino";
					}
					
					ContextoImp.MoveTo(500, 620);					ContextoImp.Show("Sexo: "+sexo_paciente);//680
					ContextoImp.MoveTo(521, 620);					ContextoImp.Show("______");//680
				
					Gnome.Print.Setfont (ContextoImp, fuente5);
					ContextoImp.MoveTo(20, 610);					ContextoImp.Show("____________________________");
					
					Gnome.Print.Setfont (ContextoImp, fuente4);
					ContextoImp.MoveTo(259.5, 590);		
										ContextoImp.Show("DATOS DEL RESPONSABLE");
					ContextoImp.MoveTo(260, 590);
										ContextoImp.Show("DATOS DEL RESPONSABLE");
				
					ContextoImp.MoveTo(20, 575);					ContextoImp.Show("Nombre de la persona responsable del paciente:  "+(string) lector["responsable_cuenta"]);
					ContextoImp.MoveTo(190, 575);					ContextoImp.Show("___________________________________________________________");
				
					ContextoImp.MoveTo(20, 560);					ContextoImp.Show("Dirección:  "+(string) lector["direccion_responsable_cuenta"]);
					ContextoImp.MoveTo(56, 560);					ContextoImp.Show("___________________________________________________________________________________________");
				
					ContextoImp.MoveTo(490, 560);					ContextoImp.Show("Tel:  "+(string) lector["telefono1_responsable_cuenta"]);
					ContextoImp.MoveTo(507, 560);					ContextoImp.Show("_________________");
				
					ContextoImp.MoveTo(20, 545);					ContextoImp.Show("Ocupación del responsable:  "+(string) lector["ocupacion_responsable"]);
					ContextoImp.MoveTo(120, 545);					ContextoImp.Show("__________________________________");
							
					ContextoImp.MoveTo(350, 545);					ContextoImp.Show("Parentesco:  "+(string) lector["parentezco"]);
					ContextoImp.MoveTo(390, 545);					ContextoImp.Show("________________________");
				
					ContextoImp.MoveTo(20, 530);					ContextoImp.Show("Empresa donde labora:  "+(string) lector["empresa_labora_responsable"]);
					ContextoImp.MoveTo(100, 530);					ContextoImp.Show("_______________________________________");
				
					ContextoImp.MoveTo(345, 530);					ContextoImp.Show("Tel. de Empresa:  "+(string) lector["telefono_emp_responsable"]);
					ContextoImp.MoveTo(404, 530);					ContextoImp.Show("________________________");
					
					ContextoImp.MoveTo(20, 500);					ContextoImp.Show("Responsable de la cuenta(Aseguradora y/o membresia):  "+(string) lector["descripcion_aseguradora"]);
					ContextoImp.MoveTo(220, 500);					ContextoImp.Show("__________________________________________");
				
					ContextoImp.MoveTo(450, 500);					ContextoImp.Show("Nº de poliza:  "+(string) lector["numero_poliza"]);
					ContextoImp.MoveTo(495, 500);					ContextoImp.Show("____________________");
					
					ContextoImp.MoveTo(20, 515);					ContextoImp.Show("Dirección:  "+(string) lector["direccion_emp_responsable"]);
					ContextoImp.MoveTo(60, 515);					ContextoImp.Show("___________________________________________________________________________");
							
					Gnome.Print.Setfont (ContextoImp, fuente5);
					ContextoImp.MoveTo(20, 490);					ContextoImp.Show("____________________________");
				
					Gnome.Print.Setfont (ContextoImp, fuente4);
					ContextoImp.MoveTo(269.5, 470);	
											ContextoImp.Show("DATOS DE ADMISION");
					ContextoImp.MoveTo(270, 470);
											ContextoImp.Show("DATOS DE ADMISION");
				
					ContextoImp.MoveTo(19.5, 455);				    ContextoImp.Show("Nº de Ingreso:  "+ folioservicio.ToString());
					ContextoImp.MoveTo(20, 455);				    ContextoImp.Show("Nº de Ingreso:  "+ folioservicio.ToString());
					ContextoImp.MoveTo(70, 454.85);					ContextoImp.Show("__________");
					ContextoImp.MoveTo(70, 455);					ContextoImp.Show("__________");
				
					ContextoImp.MoveTo(140, 455);			    	ContextoImp.Show("Nº de habitacion:  ");
			    	ContextoImp.MoveTo(200, 455);					ContextoImp.Show("__________");
					
					ContextoImp.MoveTo(280, 455);			    	ContextoImp.Show("Fecha de Admision:  "+ (string) lector["fecha_reg_adm"]);
			    	ContextoImp.MoveTo(350, 455);					ContextoImp.Show("_____________");
			    	
			    	ContextoImp.MoveTo(430, 455);			    	ContextoImp.Show("Hora de admision:"+" "+ (string) lector["hora_reg_adm"]); //DateTime.Now.ToString("HH:mm"));
			    	ContextoImp.MoveTo(495, 455);					ContextoImp.Show("_______");
					
					if ((int) lector["id_medico"] > 1)
					{
						ContextoImp.MoveTo(20, 440);					ContextoImp.Show("Medico 1º Diag.:  "+(string) lector["nombre_medico"]);
						ContextoImp.MoveTo(100, 440);					ContextoImp.Show("_____________________________________________");
					}else{
						ContextoImp.MoveTo(20, 440);					ContextoImp.Show("Medico 1º Diag.:  "+(string) lector["nombre_medico_encabezado"]);
						ContextoImp.MoveTo(100, 440);					ContextoImp.Show("_____________________________________________");
					}
					
					ContextoImp.MoveTo(350, 440);					ContextoImp.Show("Especialidad:  "+(string) lector["descripcion_especialidad"]);
					ContextoImp.MoveTo(400, 440);					ContextoImp.Show("____________________________________");
				
					ContextoImp.MoveTo(20, 425);					ContextoImp.Show("Firma:");
					ContextoImp.MoveTo(45, 425);					ContextoImp.Show("_______________________");
				
					ContextoImp.MoveTo(383, 425);					ContextoImp.Show("Ced. Prof.:  "+(string) lector["cedula_medico"]);
					ContextoImp.MoveTo(424, 425);					ContextoImp.Show("______________________________");
				
					ContextoImp.MoveTo(20, 395);					ContextoImp.Show("Cirugia: "+(string) lector["descripcion_cirugia"]);
					ContextoImp.MoveTo(50, 395);					ContextoImp.Show("___________________________________________________________________________________________________");
					
					ContextoImp.MoveTo(370, 410);					ContextoImp.Show("Ingresado por: "+(string) lector["id_empleado_admision"]);
					ContextoImp.MoveTo(425, 410);					ContextoImp.Show("________________________________");
					
					Gnome.Print.Setfont (ContextoImp, fuente5);
					ContextoImp.MoveTo(20, 385);					ContextoImp.Show("____________________________");
				
					Gnome.Print.Setfont (ContextoImp, fuente4);
					ContextoImp.MoveTo(219.5, 365);
					ContextoImp.Show("PARA SER LLENADO POR EL MEDICO TRATANTE");
					ContextoImp.MoveTo(220, 365);
					ContextoImp.Show("PARA SER LLENADO POR EL MEDICO TRATANTE");
					
					ContextoImp.MoveTo(20, 352);					ContextoImp.Show("Medico Tratante: "+medico_tratante);
					ContextoImp.MoveTo(20.5, 352);					ContextoImp.Show("Medico Tratante: "+medico_tratante);
					
					ContextoImp.MoveTo(20, 340);					ContextoImp.Show("Diagnostico:  "+(string) lector["descripcion_diagnostico_movcargos"]);
					ContextoImp.MoveTo(70, 340);					ContextoImp.Show("__________________________________________________________________________________________________________");
					
					ContextoImp.MoveTo(20, 325);					ContextoImp.Show("Observaciones: ");
					ContextoImp.MoveTo(82, 325);					ContextoImp.Show("_____________________________________________________");
				
					ContextoImp.MoveTo(20, 310);					ContextoImp.Show("Diagnostico provisional (Para ser llenado dentro de las primeras 24 Hrs):");
					ContextoImp.MoveTo(20, 295);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
					ContextoImp.MoveTo(20, 280);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
					ContextoImp.MoveTo(20, 265);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
				
					ContextoImp.MoveTo(20, 250);					ContextoImp.Show("Diagnostico Final:");
					ContextoImp.MoveTo(20, 235);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
					ContextoImp.MoveTo(20, 220);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
					ContextoImp.MoveTo(20, 205);					ContextoImp.Show("____________________________________________________________________________________________________________________________");
				
					Gnome.Print.Setfont (ContextoImp, fuente5);
					ContextoImp.MoveTo(20, 205);					ContextoImp.Show("____________________________");
				
					Gnome.Print.Setfont (ContextoImp, fuente4);
					ContextoImp.MoveTo(269.5, 185);
					ContextoImp.Show("CAUSA DE EGRESO");
					ContextoImp.MoveTo(270, 185);
					ContextoImp.Show("CAUSA DE EGRESO");
				
					ContextoImp.MoveTo(20, 170);					ContextoImp.Show("Por Mejoria:");
					ContextoImp.MoveTo(60, 170);					ContextoImp.Show("______________________");
					
					ContextoImp.MoveTo(20, 155);					ContextoImp.Show("Evolucion:");
					ContextoImp.MoveTo(60, 155);					ContextoImp.Show("______________________");
					
					ContextoImp.MoveTo(20, 140);					ContextoImp.Show("Por traslado:");
					ContextoImp.MoveTo(60, 140);					ContextoImp.Show("______________________");
					
					ContextoImp.MoveTo(200, 170);					ContextoImp.Show("Alta Voluntaria:");
					ContextoImp.MoveTo(261, 170);					ContextoImp.Show("______________________");
				
					ContextoImp.MoveTo(200, 155);					ContextoImp.Show("Por no Mejoria:");
					ContextoImp.MoveTo(261, 155);					ContextoImp.Show("______________________");
					
					ContextoImp.MoveTo(200, 140);					ContextoImp.Show("Por Defunción:");
					ContextoImp.MoveTo(261, 140);					ContextoImp.Show("______________________");
					
					ContextoImp.MoveTo(440, 160);					ContextoImp.Show("______________________________");
					ContextoImp.MoveTo(455, 160);					ContextoImp.Show(medico_tratante);
					ContextoImp.MoveTo(450, 150);					ContextoImp.Show("FIRMA DE MEDICO TRATANTE");
					
					ContextoImp.MoveTo(220, 90);
							    		ContextoImp.Show("____________________________________");
		    		ContextoImp.MoveTo(220, 80);
		    				    		ContextoImp.Show("Nombre y Firma Paciente o responsable");
		    		ContextoImp.MoveTo(250, 70);
		    				    		ContextoImp.Show("verificacion de datos");
					
					
					ContextoImp.MoveTo(20, 50);
					ContextoImp.Show("____________________________________________________________________________________________________________________________");
				
					ContextoImp.MoveTo(480, 40) ;
					ContextoImp.Show("'Salud es fuerza de trabajo'");
					string varpaso = (string) lector["descripcion_admisiones"];
				
					while ((bool) lector.Read())
					{
						varpaso = varpaso +", "+(string) lector["descripcion_admisiones"]; 
					}
					ContextoImp.MoveTo(20, 410);
					ContextoImp.Show("Admisión:  "+(string) varpaso);
					ContextoImp.MoveTo(60, 410);
					ContextoImp.Show("__________________________________________________________________");
				
        		}
        	
				lector.Close (); 
				conexion.Close ();
			
				ContextoImp.ShowPage();
			}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
				return; 
			}
		}
 	}    
 }
