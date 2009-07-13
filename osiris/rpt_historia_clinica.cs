// rpt_historia_clinica.cs created with MonoDevelop
// User: ipena at 12:05 p 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class rpt_historia_clinica
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
		
		//Entrys:
		public int foliodeservicio;
		public string pid_paciente;
		public string fpp;
		public string fum;
		public string fup;
		public string fecha_admision;
		public string fecha_nacimiento;
		
		public string nombre_paciente;
		public string edad_paciente;
		public string enfermedad_padre;
		public string enfermedad_madre;
		public string enfermedad_hermanos;
		public string enfermedad_hijos;
		public string enfermedad_apaternos;
		public string enfermedad_amaternos;
		public string otros_ahf; 
		public string tipo_casahabit;
		public string observaciones;
		public string medicamentos;
		public string otros_app;
        public string ivsa;
		public string ritmo;
		public string contracepcion;
		public string pap; 
		public string otros_ago;
		public string perinatales;
		public int peso;
		public string patologicos;
		public string alumbramiento; 
		public string infecciones;
		public string cirugias;
		public string alergias;
		public string hospitalizaciones;
		public string traumatismos;
		public string inmunizaciones;
		public string des_psicomotor;
		public string otros_hcp; 
		 //SpinButtons:
		public int edad_madre;
		public int edad_padre;
		public int novivos_hermanos;
		public int novivos_hijos;
		public int novivos_amaternos;
		public int novivos_apaternos;
		public int nomuertos_hermanos;
		public int nomuertos_hijos;
		public int nomuertos_apaternos;
		public int nomuertos_amaternos;
		public int menarca;
		public int aborto;
		public int cesarea;
		public int gestacion;
		public int parto;
		public int ed_madre;
		public int edad_gestional;
		public int no_embarazo;
		//Combobox:
		public string vivomuertopadre;
		public string vivomuertomadre;
		public string pntabaquismo;
		public string pnalcoholismo;
		public string pndrogas;
		public string pncronicodegenerativos;
		public string pnhospitalizaciones;
		public string pnquirurgicos;
		public string pnalergicos;
		public string pntraumaticos;
		public string pnneurologicos;
		//FICHA DE IDENTIDAD
		public string sexo;
		public string estado_civil;
		public string lugar_origen;
		public string religion ;
		public string escolaridad ;
		public string ocupacion;
		public string residencia_actual;
		
		public string obsercronicodegenerativo;
		public string obseralergicos;
		public string obserhospitalizaciones;
		public string obsertraumaticos;
		public string obserquirurgicos;
		public string obserneurologicos; 
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public rpt_historia_clinica(string pid_paciente_,string nombre_paciente_,string nombrebd_,string fpp_,string fum_,string fup_,string edad_paciente_,string fecha_admision_,string fecha_nacimiento_)
		{
			pid_paciente = pid_paciente_;
			nombre_paciente = nombre_paciente_;
			nombrebd = nombrebd_;
			fpp = fpp_;
			fum = fum_;
			fup = fup_;
			edad_paciente = edad_paciente_;
			fecha_admision = fecha_admision_;
			fecha_nacimiento = fecha_nacimiento_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "historia clinica del paciente", 0);
        	
        	int respuesta = dialogo.Run ();
        	
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
                new PrintJobPreview(trabajo, "historia clinica del paciente").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			ContextoImp.BeginPage("Pagina 1");
            
			// Crear una fuente 
			Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			Gnome.Font fuente4 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
			Gnome.Font fuente5 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
			
			Gnome.Print.Setfont(ContextoImp,fuente4);
			ContextoImp.MoveTo(488, 770);             ContextoImp.Show(""+pid_paciente);
			Gnome.Print.Setfont(ContextoImp,fuente5);	
			
			ContextoImp.MoveTo(66, 680);           ContextoImp.Show("    "+nombre_paciente);
			ContextoImp.MoveTo(445, 680);		ContextoImp.Show("    "+fecha_admision);
			ContextoImp.MoveTo(76, 667);		ContextoImp.Show("                               " +fecha_nacimiento);
			ContextoImp.MoveTo(248, 667);		ContextoImp.Show(""+ edad_paciente);
			ContextoImp.MoveTo(215, 275);		ContextoImp.Show("                        "+fum);
			ContextoImp.MoveTo(450, 275);		ContextoImp.Show(fpp);
			ContextoImp.MoveTo(215, 250);		ContextoImp.Show("                   "+fup);
			
			/*ContextoImp.MoveTo(33, 680);		ContextoImp.Show("FICHA DE IDENTIDAD :");
			ContextoImp.MoveTo(33, 610);		ContextoImp.Show("ANTECEDENTES HEREDO FAMILIAR :");
			ContextoImp.MoveTo(33, 530);		ContextoImp.Show("ANTECEDENTES PERSONALES NO PATOLOGICOS :");
			ContextoImp.MoveTo(33, 460);		ContextoImp.Show("ANTECEDENTES PERSONALES PATOLOGICOS :");
			ContextoImp.MoveTo(33, 350);		ContextoImp.Show("ANTECEDENTES GINECO OBSTETRICIOS :");
			ContextoImp.MoveTo(33, 200);		ContextoImp.Show("HISTORIA CLINICA PEDIATRICA :");
			*/
			
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1;
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText ="SELECT * "+
					                  "FROM hscmty_his_historia_clinica,hscmty_his_paciente,hscmty_erp_cobros_enca "+
						              "WHERE hscmty_his_paciente.historia_clinica = 'true' "+
						              "AND hscmty_erp_cobros_enca.pid_paciente = hscmty_his_historia_clinica.pid_paciente "+
						              "AND hscmty_his_historia_clinica.pid_paciente = '"+pid_paciente+"';";
				
			    Console.WriteLine(comando1.CommandText.ToString());
				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				if(lector1.Read()){
					//entrys:
					foliodeservicio = (int) lector1["folio_de_servicio"];
					enfermedad_padre = (string) lector1["descripcion_enfermedad_padre"];
					enfermedad_madre = (string) lector1["descripcion_enfermedad_madre"];
					enfermedad_hermanos = (string) lector1["descripcion_enfermedad_hermanos"];
					enfermedad_hijos = (string) lector1["descripcion_enfermedad_hijos"];
					enfermedad_apaternos = (string) lector1["descripcion_enfermedad_apaternos"];
					enfermedad_amaternos = (string) lector1["descripcion_enfermedad_amaternos"];
					otros_ahf = (string) lector1["observaciones_heredo_familiar"];
					tipo_casahabit = (string) lector1["tipo_casahabitacion"];
					observaciones = (string) lector1["no_patologicos_observaciones"];
					medicamentos = (string) lector1["medicamentos_actuales"];
					otros_app = (string) lector1["observaciones_patologicos"];
	                ivsa = (string) lector1["ginecoobstetricios_ivsa"];
	                ritmo = (string) lector1["ginecoobstetricios_ritmo"];
	                contracepcion = (string) lector1["ginecoobstetricios_contracepcion"];
	                pap = (string) lector1["ginecoobstetricios_pap"];
	                otros_ago = (string) lector1["ginecoobstetricios_otros"];
					perinatales = (string) lector1["hcpediatrica_perinatales"];
	                peso = (int) lector1["hcpediatrica_peso"]; 						
					patologicos = (string) lector1["hcpediatrica_patologicos"];
					alumbramiento = (string) lector1["hcpediatrica_alumbramiento"];
				    infecciones = (string) lector1["hcpediatrica_infecciones"];	
					cirugias = (string) lector1["hcpediatrica_cirugias"];	
					alergias = (string) lector1["hcpediatrica_alergias"];	
					hospitalizaciones = (string) lector1["hcpediatrica_hospitalizaciones"];	
					traumatismos = (string) lector1["hcpediatrica_traumatismos"];	
					inmunizaciones = (string) lector1["hcpediatrica_inmunizaciones"];	
					des_psicomotor = (string) lector1["hcpediatrica_des_psicomotor"];	
					otros_hcp = (string) lector1["hcpediatrica_otros"];
                    //nuevos entrys:
					obsercronicodegenerativo = (string) lector1["observaciones_cdegenerativos"];
		        	obseralergicos = (string) lector1["observaciones_alergicos"];
		       	 	obserhospitalizaciones = (string) lector1["observaciones_hosp"];
		        	obsertraumaticos = (string) lector1["observaciones_traumaticos"];
		        	obserquirurgicos = (string) lector1["observaciones_quirur"];
		       	    obserneurologicos = (string) lector1["observaciones_neurolog"];
					//spinbuttons:
					edad_madre = (int) lector1["madre_edad"];
					edad_padre = (int) lector1["padre_edad"];
					novivos_hermanos = (int) lector1["hermanos_nrovivos"];
					novivos_hijos = (int) lector1["hijos_nrovivos"];
					novivos_amaternos = (int) lector1["abuelosmaternos_nrovivos"];
					novivos_apaternos = (int) lector1["abuelospaternos_nrovivos"];
					nomuertos_hermanos = (int) lector1["hermanos_nromuertos"];
					nomuertos_hijos = (int) lector1["hijos_nromuertos"];
					nomuertos_apaternos = (int) lector1["abuelospaternos_nromuertos"];
					nomuertos_amaternos = (int) lector1["abuelosmaternos_nromuertos"];
					menarca = (int) lector1["ginecoobstetricios_menarca"];
					aborto = (int) lector1["ginecoobstetricios_aborto"];
					cesarea = (int) lector1["ginecoobstetricios_cesarea"];
					gestacion = (int) lector1["ginecoobstetricios_gestacion"];
					parto = (int) lector1["ginecoobstetricios_parto"];
					ed_madre = (int) lector1["hcpediatrica_edad_madre"];
					edad_gestional = (int) lector1["hcpediatrica_edadgestional"];
					no_embarazo = (int) lector1["hcpediatrica_noembarazo"];
					//Combobox
				    vivomuertopadre = (string) lector1["padre_v_m"];
					vivomuertomadre = (string) lector1["madre_v_m"];
					pntabaquismo = (string) lector1["tabaquismo_p_n"];
					pnalcoholismo = (string) lector1["alcoholismo_p_n"];
					pndrogas = (string) lector1["drogas_p_n"];
					pncronicodegenerativos = (string) lector1["cronico_degenerativo_p_n"];
					pnhospitalizaciones = (string) lector1["hospitalizaciones_p_n"];
					pnquirurgicos = (string) lector1["quirurgicos_p_n"];
					pnalergicos = (string) lector1["alergicos_p_n"];
					pntraumaticos = (string) lector1["traumaticos_p_n"];
					pnneurologicos = (string) lector1["neurologicos_p_n"];
					//FICHA DE IDENTIDAD:
					//fecha_nacimiento = (string) lector1["fechanacimiento"];
					sexo = (string) lector1["sexo_paciente"];
					estado_civil = (string) lector1["estado_civil_paciente"];
					lugar_origen = (string) lector1["estado_paciente"];
					//fecha_reg = (string) lector1["fechahoraregistro"];
					religion = (string) lector1["religion_paciente"];
					escolaridad = (string) lector1["escolaridad_paciente"];
					ocupacion = (string) lector1["ocupacion_paciente"];
					residencia_actual = (string) lector1["direccion_paciente"];
					
					if(vivomuertopadre == "VIVO"){
						 ContextoImp.MoveTo(180, 600);		        ContextoImp.Show("       SI");
					}else{
						 ContextoImp.MoveTo(270, 600);		        ContextoImp.Show("       SI");
					}
					
					if(vivomuertomadre == "VIVO"){
								ContextoImp.MoveTo(180, 588);		ContextoImp.Show("       SI");
					}else{
							ContextoImp.MoveTo(270, 588);		    ContextoImp.Show("       SI");
				    }
				    
				    if(pntabaquismo == "POSITIVO"){
						ContextoImp.MoveTo(160, 475);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 475);		        ContextoImp.Show("NO");
					}
					if(pnalcoholismo == "POSITIVO"){
						ContextoImp.MoveTo(160, 463);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 463);		        ContextoImp.Show("NO");
					}
					if(pndrogas == "POSITIVO"){
						ContextoImp.MoveTo(160, 450);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 450);		        ContextoImp.Show("NO");
					}
					if(pncronicodegenerativos == "POSITIVO"){
						ContextoImp.MoveTo(160, 398);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 398);		        ContextoImp.Show("NO");
					}
					if(pnhospitalizaciones == "POSITIVO"){
						ContextoImp.MoveTo(160, 385);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 385);	            ContextoImp.Show("NO");
					}
					if(pnquirurgicos == "POSITIVO"){
						ContextoImp.MoveTo(160, 377);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 377);		        ContextoImp.Show("NO");
					}
					if(pnalergicos == "POSITIVO"){
						ContextoImp.MoveTo(160, 360);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 360);		        ContextoImp.Show("NO");
					}
					if(pntraumaticos == "POSITIVO"){
						ContextoImp.MoveTo(160, 350);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 350);		        ContextoImp.Show("NO");
					}
					if(pnneurologicos == "POSITIVO"){
						ContextoImp.MoveTo(160, 337);		        ContextoImp.Show("SI");
					}else{
						ContextoImp.MoveTo(160, 337);		        ContextoImp.Show("NO");
					}
					if(otros_app == " "){
						ContextoImp.MoveTo(160, 325);	            ContextoImp.Show("NO");
					}else{
						ContextoImp.MoveTo(160, 325);		        ContextoImp.Show("SI");
					}
					
					Gnome.Print.Setfont(ContextoImp,fuente4);
					ContextoImp.MoveTo(520, 759);		ContextoImp.Show("       " + foliodeservicio);
					Gnome.Print.Setfont(ContextoImp,fuente5);
					/////////////////////nuevos entrys://////////////////////////////////////////
					ContextoImp.MoveTo(200, 398);		        ContextoImp.Show(obsercronicodegenerativo);
					ContextoImp.MoveTo(200, 385);		        ContextoImp.Show(obserhospitalizaciones);
					ContextoImp.MoveTo(200, 377);		        ContextoImp.Show(obserquirurgicos);
					ContextoImp.MoveTo(200, 360);		        ContextoImp.Show(obseralergicos);
					ContextoImp.MoveTo(200, 350);		        ContextoImp.Show(obsertraumaticos);
					ContextoImp.MoveTo(200, 337);		        ContextoImp.Show(obserneurologicos);
					ContextoImp.MoveTo(200, 325);	            ContextoImp.Show(otros_app);
					//Ficha de identidad(470)680////////////////////////////////////////////////////////////////////////////////////////////////////
					ContextoImp.MoveTo(445, 665);		ContextoImp.Show("                     " +religion);
					ContextoImp.MoveTo(33, 654);		ContextoImp.Show("             " +sexo);
					ContextoImp.MoveTo(66, 642);		ContextoImp.Show("                        " +estado_civil);
					ContextoImp.MoveTo(90, 629);		ContextoImp.Show("                    " +lugar_origen);
					ContextoImp.MoveTo(445, 655);		ContextoImp.Show("                     " +escolaridad);
					ContextoImp.MoveTo(445, 643);		ContextoImp.Show("                  " +ocupacion);
					ContextoImp.MoveTo(445, 630);		ContextoImp.Show("                              " +residencia_actual);
	                //Antesedentes Heredo Familiar(380)610////////////////////////////////////////////////////////////////////////////////
					//padre
					ContextoImp.MoveTo(319, 600);		ContextoImp.Show("                    "+edad_padre);//AHF SpinButtons:
					ContextoImp.MoveTo(450, 600);		ContextoImp.Show("                "+enfermedad_padre);
					//madre
					ContextoImp.MoveTo(319, 588);		ContextoImp.Show("                     "+edad_madre);//AHF SpinButtons:
					ContextoImp.MoveTo(450, 588);		ContextoImp.Show("                "+enfermedad_madre);
					//hermanos
					ContextoImp.MoveTo(180, 568);		ContextoImp.Show("       "+novivos_hermanos);//AHF SpinButtons:
					ContextoImp.MoveTo(250, 568);		ContextoImp.Show("              "+nomuertos_hermanos);//AHF SpinButtons:
					ContextoImp.MoveTo(340, 568);		ContextoImp.Show("                 "+enfermedad_hermanos);
					//hijos
					ContextoImp.MoveTo(180, 555);		ContextoImp.Show("       "+novivos_hijos);//AHF SpinButtons:
					ContextoImp.MoveTo(250, 556);		ContextoImp.Show("               "+nomuertos_hijos);//AHF SpinButtons:
					ContextoImp.MoveTo(340, 556);		ContextoImp.Show("                  "+enfermedad_hijos);
					//apaternos
					ContextoImp.MoveTo(250, 534);		ContextoImp.Show("               "+nomuertos_apaternos);//AHF SpinButtons:
					ContextoImp.MoveTo(340, 533);		ContextoImp.Show("              "+enfermedad_apaternos);
					ContextoImp.MoveTo(180, 533);		ContextoImp.Show("       "+novivos_apaternos);//AHF SpinButtons:
					//amaternos
					ContextoImp.MoveTo(180, 515);		ContextoImp.Show("       "+novivos_amaternos);//AHF SpinButtons:
					ContextoImp.MoveTo(250, 515);		ContextoImp.Show("               "+nomuertos_amaternos);//AHF SpinButtons:
					ContextoImp.MoveTo(340, 516);		ContextoImp.Show("              "+enfermedad_amaternos);
					ContextoImp.MoveTo(33, 501);		ContextoImp.Show("                  "+otros_ahf);
					//Antesedentes Personales No Patologicos(300)530////////////////////////////////////////////////////////////
					ContextoImp.MoveTo(66, 440);		ContextoImp.Show("                               "+tipo_casahabit);
					ContextoImp.MoveTo(33, 427);		ContextoImp.Show("            "+observaciones);
					//Antesedentes Personales Patologicos(240)460////////////////////////////////////////////////////////////
					ContextoImp.MoveTo(100, 315);		ContextoImp.Show("                    "+medicamentos);
					//Antesedentes Gineco Obstetricios(150)350////////////////////////////////////////////////////////////
					ContextoImp.MoveTo(280, 287);		ContextoImp.Show(""+ivsa);
					ContextoImp.MoveTo(66, 287);		ContextoImp.Show("                      " +menarca);//AGO SpinButtons:
					ContextoImp.MoveTo(33, 275);		ContextoImp.Show("                    " +ritmo);
					ContextoImp.MoveTo(66, 264);		ContextoImp.Show("                                         "+contracepcion);
					ContextoImp.MoveTo(495, 264);		ContextoImp.Show(""+aborto);//AGO SpinButtons:
					ContextoImp.MoveTo(439, 264);		ContextoImp.Show(""+cesarea);//AGO SpinButtons:
					ContextoImp.MoveTo(269, 264);		ContextoImp.Show(""+gestacion);//AGO SpinButtons:
					ContextoImp.MoveTo(343, 264);		ContextoImp.Show(""+parto);//AGO SpinButtons:
					ContextoImp.MoveTo(380, 250);		ContextoImp.Show("" +pap);
					ContextoImp.MoveTo(33, 250);		ContextoImp.Show("                  " +otros_ago);
					//Historia Clinica Pediatrica(100)200////////////////////////////////////////////////////////////
					ContextoImp.MoveTo(77, 222);		ContextoImp.Show("           "+perinatales);
					ContextoImp.MoveTo(325, 222);		ContextoImp.Show("                             "+ed_madre);//HCP Spinbuttons:
					ContextoImp.MoveTo(190, 222);		ContextoImp.Show("                "+no_embarazo);//HCP Spinbuttons:
					ContextoImp.MoveTo(490, 222);		ContextoImp.Show("         "+peso);
					ContextoImp.MoveTo(190, 210);		ContextoImp.Show("                        "+alumbramiento);
					ContextoImp.MoveTo(490, 210);		ContextoImp.Show("      "+edad_gestional);//HCP Spinbuttons:
					ContextoImp.MoveTo(77, 200);		ContextoImp.Show("             "+patologicos);
					ContextoImp.MoveTo(190, 200);		ContextoImp.Show("                "+infecciones);
					ContextoImp.MoveTo(490, 200);		ContextoImp.Show("       "+hospitalizaciones);
					ContextoImp.MoveTo(190, 188);		ContextoImp.Show("                "+cirugias);
					ContextoImp.MoveTo(490, 188);		ContextoImp.Show("            "+traumatismos);
					ContextoImp.MoveTo(190, 176);		ContextoImp.Show("              "+alergias);
					ContextoImp.MoveTo(66, 161);		ContextoImp.Show("                                  "+inmunizaciones);
					ContextoImp.MoveTo(90, 150);		ContextoImp.Show("                                    "+des_psicomotor);
					ContextoImp.MoveTo(33, 137);		ContextoImp.Show("                     "+otros_hcp);
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion1.Close ();
			
			ContextoImp.ShowPage();
		}
	}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class imprime_pag2
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
		
		//PAGINA2:  
		public string pid_paciente;
		public string motivoingreso;
		public string padecimientoactual;
		public string ta;
		public string fc;
		public string fr;
		public string temp;
		public string pso;
		public string talla; 
		public string habitus_ext;
		public string cabeza;
		public string cuello;
		public string torax;
		public string abdomen;
		public string extremidades;
		public string genitourinario;
		public string neurologico; 
		public string diagnosticos;
		public string plan_diag;
		public string nombre_plan_diag;
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//,string ta_,string fc_,string fr_,string temp_,string pso_,string talla_
		public imprime_pag2(string pid_paciente_,string nombrebd_)
		{
			pid_paciente = pid_paciente_;
			nombrebd = nombrebd_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "historia clinica del paciente Pagina2", 0);
        	
        	int respuesta = dialogo.Run ();
        	
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
                new PrintJobPreview(trabajo, "historia clinica del pacienteb Pagina2").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			ContextoImp.BeginPage("Pagina 1");
            
			// Crear una fuente 
			Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			
			//Gnome.Print.Setfont(ContextoImp,fuente3);
			
			
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1;
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText ="SELECT * "+
					                  "FROM hscmty_his_historia_clinica "+
						              "WHERE hscmty_his_historia_clinica.pid_paciente = '"+pid_paciente+"';";
				
			    Console.WriteLine(comando1.CommandText.ToString());
				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				if(lector1.Read()){
					
					//pagina2
					motivoingreso = (string) lector1["motivo_de_ingreso"];	
					padecimientoactual = (string) lector1["padecimiento_actual"];	
					ta = (string) lector1["psesion_arterial"].ToString();	
					fc = (string) lector1["frecuencia_cardiaca"].ToString();	
					fr = (string) lector1["frecuencia_respiratoria"].ToString();	
					temp = (string) lector1["temperatura"].ToString();
				    pso = (string) lector1["peso"].ToString();
				    talla = (string) lector1["talla"].ToString();
				    habitus_ext = (string) lector1["habitus_exterior"];
				    cabeza = (string) lector1["cabeza"];	 	
	                cuello = (string) lector1["cuello"];							
					torax = (string) lector1["torax"];	
					abdomen = (string) lector1["abdomen"];	
					extremidades = (string) lector1["extremidades"];	
					genitourinario = (string) lector1["genitourinario"];	
					neurologico = (string) lector1["neurologico"];	
					diagnosticos = (string) lector1["diagnosticos"];	
					plan_diag = (string) lector1["plan_diagnostico"];
					nombre_plan_diag = (string) lector1["nombre_plan_diag"];
					
					/*if(habitus_ext.Length > 49){
						habitusextrecortado = habitus_ext.Substring(0,46)  ; 
					}else{
						habitusextrecortado = habitus_ext;
					}
					if (cabeza.Length > 49){
						cabezarecortado = cabeza.Substring(0,46)  ; 
					}else{
						cabezarecortado = cabeza;
					}*/
					
					Gnome.Print.Setfont(ContextoImp,fuente3);
				//PAGINA2:
				   //MOTIVO DE INGRESO://////////////////////////////////////////////////////////                
					ContextoImp.MoveTo(28, 725);		ContextoImp.Show(motivoingreso);
					//PADECIMIENTO ACTUAL://////////////////////////////////////////////////////////
					ContextoImp.MoveTo(28, 671.5);		ContextoImp.Show(padecimientoactual);
					//EXPLORACION FISICA://////////////////////////////////////////////////////////
					ContextoImp.MoveTo(33, 546);	ContextoImp.Show("     " +ta);
					ContextoImp.MoveTo(100, 546);ContextoImp.Show("     " +fc);
					ContextoImp.MoveTo(146, 546);		ContextoImp.Show("              " +fr);
					ContextoImp.MoveTo(210, 546);		ContextoImp.Show("         " +temp);
					ContextoImp.MoveTo(290, 546);		ContextoImp.Show("           " +pso);
					ContextoImp.MoveTo(342, 546);		ContextoImp.Show("                         " +talla); 
					
					ContextoImp.MoveTo(66, 525);		ContextoImp.Show("                                                " + habitus_ext);
					
					ContextoImp.MoveTo(33, 487);		ContextoImp.Show("                               " +cabeza);
					
					ContextoImp.MoveTo(33, 437);		ContextoImp.Show("                               " +cuello);
					
					ContextoImp.MoveTo(33, 417);		ContextoImp.Show("                               " +torax);
					
					ContextoImp.MoveTo(33, 377);		ContextoImp.Show("                                  " +abdomen);
					
					ContextoImp.MoveTo(33, 353);		ContextoImp.Show("                                        " +extremidades);
					
					ContextoImp.MoveTo(66, 325);		ContextoImp.Show("                                     " +genitourinario);
					
					ContextoImp.MoveTo(36, 306);		ContextoImp.Show("                                      " +neurologico); 
					//DIAGNOSTICOS://////////////////////////////////////////////////////////
					ContextoImp.MoveTo(28, 253);		ContextoImp.Show(diagnosticos);
					//PLAN DIAGNOSTICO//////////////////////////////////////////////////////////
					ContextoImp.MoveTo(28, 201);		ContextoImp.Show(plan_diag);
					ContextoImp.MoveTo(66, 140);		ContextoImp.Show("      "+nombre_plan_diag);
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion1.Close ();
			
			ContextoImp.ShowPage();
		}
	}
}

		