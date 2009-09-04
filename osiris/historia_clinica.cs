// historia_clinica.cs created with MonoDevelop
// User: ipena at 09:36 a 16/07/2008
// Autor    	: Israel Peña Gonzalez - el_rip@hotmail.com (Programacion Mono)
// Licencia		: GLP                                                                                          
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)  
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
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
	public class historia_clinica
	{
		// Declarando ventana principal
		[Widget] Gtk.Window historia_clinica_del_paciente; 
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		//buton guardar
		[Widget] Gtk.Button button_guardar;
		//imprimir
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_imprimir_p2;
		//Editar
		[Widget] Gtk.Button button_editar_his_clin;
		//Ficha de Identidad:
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_edad_paciente;
		[Widget] Gtk.Entry entry_sexo;
		//NoteBook:
		[Widget] Gtk.Notebook notebook1;
		/////////PAGINA 1////////////////////////////////////
		//Antecedentes Heredo Familiar:
		[Widget] Gtk.SpinButton spinbutton_novivos_hermanos;
		[Widget] Gtk.SpinButton spinbutton_novivos_hijos;
		[Widget] Gtk.SpinButton spinbutton_novivos_apaternos;
		[Widget] Gtk.SpinButton spinbutton_novivos_amaternos;
		[Widget] Gtk.SpinButton spinbutton_nomuertos_hermanos;
		[Widget] Gtk.SpinButton spinbutton_nomuertos_hijos;
		[Widget] Gtk.SpinButton spinbutton_nomuertos_apaternos;
		[Widget] Gtk.SpinButton spinbutton_nomuertos_amaternos;
		[Widget] Gtk.ComboBox combobox_vivomuerto_padre;
		[Widget] Gtk.ComboBox combobox_vivomuerto_madre;
		[Widget] Gtk.SpinButton spinbutton_edad_padre;
		[Widget] Gtk.SpinButton spinbutton_edad_madre;
		[Widget] Gtk.Entry entry_enfermedad_padre;
		[Widget] Gtk.Entry entry_enfermedad_madre;
		[Widget] Gtk.Entry entry_enfermedad_hermanos;
		[Widget] Gtk.Entry entry_enfermedad_hijos;
		[Widget] Gtk.Entry entry_enfermedad_apaternos;
		[Widget] Gtk.Entry entry_enfermedad_amaternos;
		[Widget] Gtk.Entry entry_otros_ahf;
		      
		//Antecedentes Personales NO Patologicos
		[Widget] Gtk.Entry entry_tipo_casahabit;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.ComboBox combobox_tabaquismo;
		[Widget] Gtk.ComboBox combobox_alcoholismo;
		[Widget] Gtk.ComboBox combobox_drogas;
		
		//Antecedentes Personales Patologicos     
		[Widget] Gtk.ComboBox combobox_cronico_degenerativos;
        [Widget] Gtk.ComboBox combobox_hospitalizaciones;
        [Widget] Gtk.ComboBox combobox_quirurgicos;
        [Widget] Gtk.ComboBox combobox_alergicos;
        [Widget] Gtk.ComboBox combobox_traumaticos;
        [Widget] Gtk.ComboBox combobox_neurologicos;  
		[Widget] Gtk.Entry entry_medicamentos;
		[Widget] Gtk.Entry entry_otros_app;
		//entrys nuevos:
		[Widget] Gtk.Entry entry_cronicodegenerativo;
		[Widget] Gtk.Entry entry_alergicos;
		[Widget] Gtk.Entry entry_obs_hospi;
		[Widget] Gtk.Entry entry_traumaticos;
		[Widget] Gtk.Entry entry_quirurgicos;
		[Widget] Gtk.Entry entry_neurologicos;
		
		//pagina 2
		//Antecedentes Gineco Obstetricios     
		[Widget] Gtk.SpinButton spinbutton_menarca;
		[Widget] Gtk.Entry entry_ivsa;
		[Widget] Gtk.Entry entry_ritmo;
		[Widget] Gtk.Entry entry_contracepcion;
		[Widget] Gtk.SpinButton spinbutton_g;
		[Widget] Gtk.SpinButton spinbutton_p;
		[Widget] Gtk.SpinButton spinbutton_c;
		[Widget] Gtk.SpinButton spinbutton_a;
		[Widget] Gtk.Entry entry_pap;
		[Widget] Gtk.Entry entry_otros_ago;
		
		//fechas:
		[Widget] Gtk.Entry entry_fum;
	    [Widget] Gtk.Entry entry_fup;
		[Widget] Gtk.Entry entry_fpp;
		
		//Historia Clinica Pediatrica
		[Widget] Gtk.Entry entry_perinatales;
		[Widget] Gtk.SpinButton spinbutton_no_embarazo;
		[Widget] Gtk.SpinButton spinbutton_ed_madre;
		[Widget] Gtk.Entry entry_peso;
		[Widget] Gtk.Entry entry_patologicos;
		[Widget] Gtk.Entry entry_alumbramiento;
		[Widget] Gtk.Entry entry_infecciones;
		[Widget] Gtk.Entry entry_cirugias;
        [Widget] Gtk.Entry entry_alergias;
        [Widget] Gtk.SpinButton spinbutton_edad_gestional;
        [Widget] Gtk.Entry entry_hospitalizaciones;
        [Widget] Gtk.Entry entry_traumatismos;
        [Widget] Gtk.Entry entry_inmunizaciones;
        [Widget] Gtk.Entry entry_des_psicomotor;
        [Widget] Gtk.Entry entry_otros_hcp;
		
		//id Enfermedades:
		[Widget] Gtk.Entry entry_idenf_padre;   
		[Widget] Gtk.Entry entry_idenf_madre;
		[Widget] Gtk.Entry entry_idenf_hermanos;
		[Widget] Gtk.Entry entry_idenf_hijos;
		[Widget] Gtk.Entry entry_idenf_apaternos;
		[Widget] Gtk.Entry entry_idenf_amaternos;
		
		//Buscar Enfermedades   
		[Widget] Gtk.Button button_buscar1;	
		[Widget] Gtk.Button button_buscar2;
		[Widget] Gtk.Button button_buscar3;
		[Widget] Gtk.Button button_buscar4;
		[Widget] Gtk.Button button_buscar5;
		[Widget] Gtk.Button button_buscar6;
		
		/////////PAGINA 3/////////////////////////////////////////////
		[Widget] Gtk.Entry entry_motivoingreso;
		[Widget] Gtk.Entry entry_padecimientoactual;
		[Widget] Gtk.Entry entry_ta;//presion arterial
		[Widget] Gtk.Entry entry_fc;//frecuencia cardiaca
		[Widget] Gtk.Entry entry_fr;//frecuencia respiratoria
		[Widget] Gtk.Entry entry_temp;
		[Widget] Gtk.Entry entry_pso;
		[Widget] Gtk.Entry entry_talla;
		[Widget] Gtk.Entry entry_habitus_ext;
		[Widget] Gtk.Entry entry_cabeza;
		[Widget] Gtk.Entry entry_cuello;
		[Widget] Gtk.Entry entry_torax;
		[Widget] Gtk.Entry entry_abdomen;
		[Widget] Gtk.Entry entry_extremidades;
		[Widget] Gtk.Entry entry_genitourinario;
		[Widget] Gtk.Entry entry_neurologico;
        [Widget] Gtk.Entry entry_diagnosticos;
		[Widget] Gtk.Entry entry_plan_diag;
		[Widget] Gtk.Entry entry_nombre_plan_diag;
		
		public string fecha_admision;
		public string fecha_nacimiento;
		
		public string vivomuertopadre = ""; 
		public string vivomuertomadre = ""; 
		public string pntabaquismo = "";
		public string pnalcoholismo = "";
		public string pndrogas = "";
		public string pncronicodegenerativos = "";
		public string pnhospitalizaciones = "";
		public string pnquirurgicos = "";
		public string pnalergicos = "";
		public string pntraumaticos = "";
		public string pnneurologicos = "";
		
		//public int tipodellenado;
		public bool editando = false;
		
		public string nombre_paciente;
		public string pid_paciente;
		public string edad;
		
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		public string NomEmpleados;
					
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public historia_clinica(string nombre_paciente_,string pid_paciente_,string edad_,string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_,string fecha_admision_,string fecha_nacimiento_ ) 
		{
			nombre_paciente = nombre_paciente_;
			pid_paciente = pid_paciente_;
			edad = edad_;
			LoginEmpleado = LoginEmp;
			nombrebd = _nombrebd_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			fecha_admision = fecha_admision_;
			fecha_nacimiento = fecha_nacimiento_;
			
            Glade.XML gxml = new Glade.XML (null, "urgencia.glade", "historia_clinica_del_paciente", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			historia_clinica_del_paciente.Show();
			
			editando = false;
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//Guarda datos de la reservacion
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			//Imprimir:
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_historia_clinica_clicked);
			this.button_imprimir_p2.Clicked += new EventHandler(on_button_imprimir_p2_clicked);
			//Editar:
			this.button_editar_his_clin.Clicked += new EventHandler(on_button_editar_his_clin_clicked);
			this.button_editar_his_clin.Sensitive = false;
			this.button_imprimir.Sensitive = false;      
			this.button_imprimir_p2.Sensitive = false;
			
			this.entry_fpp.Text = DateTime.Now.ToString("yyyy-MM-dd");
			this.entry_fum.Text = DateTime.Now.ToString("yyyy-MM-dd");
			this.entry_fup.Text = DateTime.Now.ToString("yyyy-MM-dd");
			
			// Validando que solo se escriben numeros PAGINA 1:
			this.entry_pid_paciente.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_ed_madre.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_edad_gestional.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_edad_madre.KeyPressEvent += onKeyPressEventactual;
			this.entry_edad_paciente.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_edad_padre.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_no_embarazo.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_a.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_c.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_p.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_g.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_nomuertos_amaternos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_nomuertos_apaternos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_nomuertos_hermanos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_nomuertos_hijos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_novivos_amaternos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_novivos_apaternos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_novivos_hermanos.KeyPressEvent += onKeyPressEventactual;
			this.spinbutton_novivos_hijos.KeyPressEvent += onKeyPressEventactual;
			this.entry_peso.KeyPressEvent += onKeyPressEventactual;
			this.entry_perinatales.KeyPressEvent += onKeyPressEventactual;
			
			//entrys id enfermedad (sirve para ingresar el id de la enfermedad y solo se pueda con numeros)
			/*this.entry_idenf_padre.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_madre.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_hermanos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_hijos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_apaternos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_amaternos.KeyPressEvent += onKeyPressEventactual; */
			
			// Buscar Enfermedades/id Enfermedades
			this.button_buscar1.Sensitive = false;
			this.button_buscar2.Sensitive = false;
			this.button_buscar3.Sensitive = false;
			this.button_buscar4.Sensitive = false;
			this.button_buscar5.Sensitive = false;
			this.button_buscar6.Sensitive = false;
			this.entry_idenf_padre.Sensitive = false;
			this.entry_idenf_madre.Sensitive = false;
			this.entry_idenf_hermanos.Sensitive = false;
			this.entry_idenf_hijos.Sensitive = false;
			this.entry_idenf_apaternos.Sensitive = false;
			this.entry_idenf_amaternos.Sensitive = false;
			
			// (trayendo las variables del programa urgencias: nombre, pid y edad)
			this.entry_nombre_paciente.Text = nombre_paciente_;
			this.entry_pid_paciente.Text = pid_paciente_.Trim();
			this.entry_edad_paciente.Text = edad_.Trim();
			
			//llenado de los ComboBox Antecedentes Heredo Familiar:
			llenado_padre(0,"");
			llenado_madre(0,"");
			
			//llenado de los ComboBox Antecedentes Personales NO Patologicos  
			llenado_tabaquismo(0,"");
			llenado_alcoholismo(0,"");
			llenado_drogas(0,"");
			
			//llenado de los ComboBox Antecedentes Personales Patologicos
			llenado_cronicodegenerativos(0,"");
			llenado_hospitalizaciones(0,"");
			llenado_quirurgicos(0,"");
			llenado_alergicos(0,"");
			llenado_traumaticos(0,"");
			llenado_neurologicos(0,"");
			
			//SpinButtons Antecedentes Heredo Familiar:
			this.spinbutton_edad_madre.SetRange(0, 150);
			this.spinbutton_edad_padre.SetRange(0, 150);
			this.spinbutton_novivos_hermanos.SetRange(0, 50);
			this.spinbutton_novivos_hijos.SetRange(0, 30);
			this.spinbutton_novivos_amaternos.SetRange(0, 2);
			this.spinbutton_novivos_apaternos.SetRange(0, 2);
			this.spinbutton_nomuertos_hermanos.SetRange(0, 50);
			this.spinbutton_nomuertos_hijos.SetRange(0, 30);
			this.spinbutton_nomuertos_apaternos.SetRange(0, 2);
			this.spinbutton_nomuertos_amaternos.SetRange(0, 2);
			
			//SpinButtons Antecedentes Gineco Obsterricios:
			this.spinbutton_menarca.SetRange(0, 150);
			this.spinbutton_a.SetRange(0, 50);
			this.spinbutton_c.SetRange(0, 50);
			this.spinbutton_g.SetRange(0, 100);
			this.spinbutton_p.SetRange(0, 50);
			
			//SpinButtons Historia Clinica Pediatrica:
			this.spinbutton_ed_madre.SetRange(0, 150);
			this.spinbutton_edad_gestional.SetRange(0, 150);
			this.spinbutton_no_embarazo.SetRange(0, 50);

			muestra_datos_paciente();
		}
		
		void muestra_datos_paciente()
		{
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1;
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText ="SELECT * "+
					                  "FROM hscmty_his_historia_clinica,hscmty_his_paciente "+
						              "WHERE hscmty_his_paciente.historia_clinica = 'true' "+
						              "AND hscmty_his_historia_clinica.pid_paciente = '"+this.entry_pid_paciente.Text+"';";
				
			    Console.WriteLine(comando1.CommandText.ToString());
				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
					   
				if(lector1.Read()){
				this.button_guardar.Sensitive = false;                       this.entry_nombre_paciente.Sensitive = false;			        this.entry_pid_paciente.Sensitive = false;        this.entry_edad_paciente.Sensitive = false;
				this.button_editar_his_clin.Sensitive = true;                this.button_imprimir.Sensitive = true;                         this.button_imprimir_p2.Sensitive = true;
			//Antecedentes Heredo Familiar:     
				this.entry_enfermedad_padre.Sensitive = false;               this.entry_enfermedad_madre.Sensitive = false;                 this.entry_enfermedad_amaternos.Sensitive = false; 
				this.entry_enfermedad_hermanos.Sensitive = false;            this.entry_enfermedad_hijos.Sensitive = false;                 this.entry_enfermedad_apaternos.Sensitive = false;             
				this.entry_otros_ahf.Sensitive = false; 
			 //Antecedentes Personales NO Patologicos:
				this.entry_tipo_casahabit.Sensitive = false;                 this.entry_observaciones.Sensitive = false;           
			 //Antecedentes Personales Patologicos:
				this.entry_otros_app.Sensitive = false;                      this.entry_medicamentos.Sensitive = false;
			    ////entrys nuevos:
				this.entry_cronicodegenerativo.Sensitive = false;
		        this.entry_alergicos.Sensitive = false;
		        this.entry_obs_hospi.Sensitive = false;
		        this.entry_traumaticos.Sensitive = false;
		        this.entry_quirurgicos.Sensitive = false;
		        this.entry_neurologicos.Sensitive = false;
			//Pagina 2: Antecedentes Gineco Obstetricios:                           
				this.entry_ivsa.Sensitive = false;                           this.entry_ritmo.Sensitive = false;                            this.entry_fum.Sensitive = false;                  this.entry_fpp.Sensitive = false;                           
				this.entry_pap.Sensitive = false;                            this.entry_contracepcion.Sensitive = false;                    this.entry_otros_ago.Sensitive = false;            this.entry_fup.Sensitive = false;
			 //Historia Clinica Pediatrica:                     
				this.entry_perinatales.Sensitive = false;                    this.entry_peso.Sensitive = false;                             this.entry_patologicos.Sensitive = false;          this.entry_alumbramiento.Sensitive = false; 
				this.entry_infecciones.Sensitive = false;                    this.entry_alergias.Sensitive = false;                         this.entry_hospitalizaciones.Sensitive = false;    this.entry_traumatismos.Sensitive = false; 
				this.entry_cirugias.Sensitive = false;                       this.entry_inmunizaciones.Sensitive = false;                   this.entry_des_psicomotor.Sensitive = false;       this.entry_otros_hcp.Sensitive = false; 
			 //Pagina 3: Motivo de Ingreso:
				this.entry_motivoingreso.Sensitive = false;                  this.entry_padecimientoactual.Sensitive = false;               this.entry_ta.Sensitive = false;                   this.entry_fc.Sensitive = false; 
				this.entry_fr.Sensitive = false;                             this.entry_temp.Sensitive = false;                             this.entry_pso.Sensitive = false;                  this.entry_talla.Sensitive = false; 
				this.entry_habitus_ext.Sensitive = false;                    this.entry_cabeza.Sensitive = false;                           this.entry_cuello.Sensitive = false;               this.entry_torax.Sensitive = false; 
				this.entry_abdomen.Sensitive = false;                        this.entry_extremidades.Sensitive = false;                     this.entry_genitourinario.Sensitive = false;       this.entry_neurologico.Sensitive = false; 
				this.entry_diagnosticos.Sensitive = false;                   this.entry_plan_diag.Sensitive = false;                        this.entry_nombre_plan_diag.Sensitive = false; 			
             //combobox:			
				combobox_vivomuerto_padre.Sensitive = false;                 combobox_vivomuerto_madre.Sensitive = false;       
				combobox_alcoholismo.Sensitive = false;                      combobox_tabaquismo.Sensitive = false;                         combobox_drogas.Sensitive = false;
				combobox_traumaticos.Sensitive = false;                      combobox_alergicos.Sensitive = false;                          combobox_neurologicos.Sensitive = false;      
				combobox_hospitalizaciones.Sensitive = false;                combobox_cronico_degenerativos.Sensitive = false;              combobox_quirurgicos.Sensitive = false;       
			 //spinbuttons:
				this.spinbutton_a.Sensitive = false;                         this.spinbutton_c.Sensitive = false;                           this.spinbutton_ed_madre.Sensitive = false;                         this.spinbutton_g.Sensitive = false;                         this.spinbutton_nomuertos_hijos.Sensitive = false;
				this.spinbutton_edad_gestional.Sensitive = false;            this.spinbutton_edad_madre.Sensitive = false;                  this.spinbutton_edad_padre.Sensitive = false;                       this.spinbutton_menarca.Sensitive = false;                  this.spinbutton_novivos_amaternos.Sensitive = false;
				this.spinbutton_no_embarazo.Sensitive = false;               this.spinbutton_nomuertos_amaternos.Sensitive = false;         this.spinbutton_nomuertos_apaternos.Sensitive = false;              this.spinbutton_nomuertos_hermanos.Sensitive = false;        this.spinbutton_novivos_apaternos.Sensitive = false;
				this.spinbutton_novivos_hermanos.Sensitive = false;          this.spinbutton_novivos_hijos.Sensitive = false;               this.spinbutton_p.Sensitive = false;
			//id_quien_actualizo,fechahora_actualizacion(UPDATE)
				
				    //entrys:
					entry_enfermedad_padre.Text = (string) lector1["descripcion_enfermedad_padre"];
					entry_enfermedad_madre.Text = (string) lector1["descripcion_enfermedad_madre"].ToString().Trim();
					entry_enfermedad_hermanos.Text = (string) lector1["descripcion_enfermedad_hermanos"].ToString().Trim();
					entry_enfermedad_hijos.Text = (string) lector1["descripcion_enfermedad_hijos"].ToString().Trim();
					entry_enfermedad_apaternos.Text = (string) lector1["descripcion_enfermedad_apaternos"].ToString().Trim();
					entry_enfermedad_amaternos.Text = (string) lector1["descripcion_enfermedad_amaternos"].ToString().Trim();
					entry_otros_ahf.Text = (string) lector1["observaciones_heredo_familiar"].ToString().Trim();
					entry_tipo_casahabit.Text = (string) lector1["tipo_casahabitacion"].ToString().Trim();
					entry_observaciones.Text = (string) lector1["no_patologicos_observaciones"].ToString().Trim();
					entry_medicamentos.Text = (string) lector1["medicamentos_actuales"].ToString().Trim();
					entry_otros_app.Text = (string) lector1["observaciones_patologicos"].ToString().Trim();
	                entry_ivsa.Text = (string) lector1["ginecoobstetricios_ivsa"].ToString().Trim();
	                entry_ritmo.Text = (string) lector1["ginecoobstetricios_ritmo"].ToString().Trim();
	                entry_contracepcion.Text = (string) lector1["ginecoobstetricios_contracepcion"].ToString().Trim();
	                entry_pap.Text = (string) lector1["ginecoobstetricios_pap"].ToString().Trim();
	                entry_otros_ago.Text = (string) lector1["ginecoobstetricios_otros"].ToString().Trim();
	                entry_fum.Text = (string) lector1["ginecoobstetricios_fum"].ToString().Trim();
	                entry_fup.Text = (string) lector1["ginecoobstetricios_fup"].ToString().Trim();
	                entry_fpp.Text = (string) lector1["ginecoobstetricios_fpp"].ToString().Trim();
	                entry_perinatales.Text = (string) lector1["hcpediatrica_perinatales"].ToString().Trim();
	                entry_peso.Text = (string) lector1["hcpediatrica_peso"].ToString().Trim(); 						
					entry_patologicos.Text = (string) lector1["hcpediatrica_patologicos"].ToString().Trim();
					entry_alumbramiento.Text = (string) lector1["hcpediatrica_alumbramiento"].ToString().Trim();
				    entry_infecciones.Text = (string) lector1["hcpediatrica_infecciones"].ToString().Trim();	
					entry_cirugias.Text = (string) lector1["hcpediatrica_cirugias"].ToString().Trim();	
					entry_alergias.Text = (string) lector1["hcpediatrica_alergias"].ToString().Trim();	
					entry_hospitalizaciones.Text = (string) lector1["hcpediatrica_hospitalizaciones"].ToString().Trim();	
					entry_traumatismos.Text = (string) lector1["hcpediatrica_traumatismos"].ToString().Trim();	
					entry_inmunizaciones.Text = (string) lector1["hcpediatrica_inmunizaciones"].ToString().Trim();	
					entry_des_psicomotor.Text = (string) lector1["hcpediatrica_des_psicomotor"].ToString().Trim();	
					entry_otros_hcp.Text = (string) lector1["hcpediatrica_otros"].ToString().Trim();
					//pagina3	
					entry_motivoingreso.Text = (string) lector1["motivo_de_ingreso"].ToString().Trim();	
					entry_padecimientoactual.Text = (string) lector1["padecimiento_actual"].ToString().Trim();	
					entry_ta.Text = (string) lector1["psesion_arterial"].ToString().Trim();	
					entry_fc.Text = (string) lector1["frecuencia_cardiaca"].ToString().Trim();	
					entry_fr.Text = (string) lector1["frecuencia_respiratoria"].ToString().Trim();	
					entry_temp.Text = (string) lector1["temperatura"].ToString().Trim();
				    entry_pso.Text = (string) lector1["peso"].ToString().Trim();
				    entry_talla.Text = (string) lector1["talla"].ToString().Trim();
				    entry_habitus_ext.Text = (string) lector1["habitus_exterior"].ToString().Trim();
				    entry_cabeza.Text = (string) lector1["cabeza"].ToString().Trim();	 	
	                entry_cuello.Text = (string) lector1["cuello"].ToString().Trim();							
					entry_torax.Text = (string) lector1["torax"].ToString().Trim();	
					entry_abdomen.Text = (string) lector1["abdomen"].ToString().Trim();	
					entry_extremidades.Text = (string) lector1["extremidades"].ToString().Trim();	
					entry_genitourinario.Text = (string) lector1["genitourinario"].ToString().Trim();	
					entry_neurologico.Text = (string) lector1["neurologico"].ToString().Trim();	
					entry_diagnosticos.Text = (string) lector1["diagnosticos"].ToString().Trim();	
					entry_plan_diag.Text = (string) lector1["plan_diagnostico"].ToString().Trim();
					entry_nombre_plan_diag.Text = (string) lector1["nombre_plan_diag"].ToString().Trim();
					//entrys nuevos
					this.entry_cronicodegenerativo.Text = (string) lector1["observaciones_cdegenerativos"].ToString();
		        	this.entry_alergicos.Text = (string) lector1["observaciones_alergicos"].ToString();
		       	 	this.entry_obs_hospi.Text = (string) lector1["observaciones_hosp"].ToString();
		        	this.entry_traumaticos.Text = (string) lector1["observaciones_traumaticos"].ToString();
		        	this.entry_quirurgicos.Text = (string) lector1["observaciones_quirur"].ToString();
		       	    this.entry_neurologicos.Text = (string) lector1["observaciones_neurolog"].ToString();
					//spinbuttons:
					this.spinbutton_edad_madre.Text = (string) lector1["madre_edad"].ToString();
					this.spinbutton_edad_padre.Text = (string) lector1["padre_edad"].ToString();
					this.spinbutton_novivos_hermanos.Text = (string) lector1["hermanos_nrovivos"].ToString();
					this.spinbutton_novivos_hijos.Text = (string) lector1["hijos_nrovivos"].ToString();
					this.spinbutton_novivos_amaternos.Text = (string) lector1["abuelosmaternos_nrovivos"].ToString();
					this.spinbutton_novivos_apaternos.Text = (string) lector1["abuelospaternos_nrovivos"].ToString();
					this.spinbutton_nomuertos_hermanos.Text = (string) lector1["hermanos_nromuertos"].ToString();
					this.spinbutton_nomuertos_hijos.Text = (string) lector1["hijos_nromuertos"].ToString();
					this.spinbutton_nomuertos_apaternos.Text = (string) lector1["abuelospaternos_nromuertos"].ToString();
					this.spinbutton_nomuertos_amaternos.Text = (string) lector1["abuelosmaternos_nromuertos"].ToString();
					this.spinbutton_menarca.Text = (string) lector1["ginecoobstetricios_menarca"].ToString();
					this.spinbutton_a.Text = (string) lector1["ginecoobstetricios_aborto"].ToString();
					this.spinbutton_c.Text = (string) lector1["ginecoobstetricios_cesarea"].ToString();
					this.spinbutton_g.Text = (string) lector1["ginecoobstetricios_gestacion"].ToString();
					this.spinbutton_p.Text = (string) lector1["ginecoobstetricios_parto"].ToString();
					this.spinbutton_ed_madre.Text = (string) lector1["hcpediatrica_edad_madre"].ToString();
					this.spinbutton_edad_gestional.Text = (string) lector1["hcpediatrica_edadgestional"].ToString();
					this.spinbutton_no_embarazo.Text = (string) lector1["hcpediatrica_noembarazo"].ToString();
					// Asignando Variables
				    // tipodellenado;
					//Console.WriteLine(vivomuertopadre);
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
					
					//llenado de los ComboBox Antecedentes Heredo Familiar:
					llenado_padre(1,vivomuertopadre);
					llenado_madre(1,vivomuertomadre);
					//llenado de los ComboBox Antecedentes Personales NO Patologicos  
					llenado_tabaquismo(1,pntabaquismo);
					llenado_alcoholismo(1,pnalcoholismo);
					llenado_drogas(1,pndrogas);
					//llenado de los ComboBox Antecedentes Personales Patologicos
					llenado_cronicodegenerativos(1,pncronicodegenerativos);
					llenado_hospitalizaciones(1,pnhospitalizaciones);
					llenado_quirurgicos(1,pnquirurgicos);
					llenado_alergicos(1,pnalergicos);
					llenado_traumaticos(1,pntraumaticos);
					llenado_neurologicos(1,pnneurologicos);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion1.Close ();
		}
		
		/////////LLENADO DE COMBOS: Antecedentes Heredo Familiar:////////////////////////////////////////////////////////////
		void llenado_padre( int tipodellenado, string vivomuertopadre )
		{
			//Console.WriteLine("1" + vivomuertopadre);
			combobox_vivomuerto_padre.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_vivomuerto_padre.PackStart(cell, true);
			combobox_vivomuerto_padre.AddAttribute(cell,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string));
			combobox_vivomuerto_padre.Model = store1;
			
			if ((int) tipodellenado == 1){
				store1.AppendValues ((string) vivomuertopadre );
			}
	        
			store1.AppendValues ("");
			store1.AppendValues ("Vivo");
			store1.AppendValues ("Muerto");
			
	       TreeIter iter1;
			if (store1.GetIterFirst(out iter1))
			{
				combobox_vivomuerto_padre.SetActiveIter (iter1);
			}
			combobox_vivomuerto_padre.Changed += new EventHandler (onComboBoxChanged_vivomuerto_padre);
		}
		void onComboBoxChanged_vivomuerto_padre (object sender, EventArgs args)
		{
			ComboBox combobox_vivomuerto_padre = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter1;
			if (combobox_vivomuerto_padre.GetActiveIter (out iter1)){
				vivomuertopadre = (string) combobox_vivomuerto_padre.Model.GetValue(iter1,0);
			}
		}
		
		void llenado_madre( int tipodellenado, string vivomuertomadre )
		{
			combobox_vivomuerto_madre.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_vivomuerto_madre.PackStart(cell, true);
			combobox_vivomuerto_madre.AddAttribute(cell,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string));
			combobox_vivomuerto_madre.Model = store1;
			
			if ((int) tipodellenado == 1){
				store1.AppendValues ((string) vivomuertomadre);
			}
	        
			store1.AppendValues ("");
			store1.AppendValues ("Vivo");
			store1.AppendValues ("Muerto");
			
	       TreeIter iter1;
			if (store1.GetIterFirst(out iter1))
			{
				combobox_vivomuerto_madre.SetActiveIter (iter1);
			}
			combobox_vivomuerto_madre.Changed += new EventHandler (onComboBoxChanged_vivomuerto_madre);
		}
		void onComboBoxChanged_vivomuerto_madre (object sender, EventArgs args)
		{
			ComboBox combobox_vivomuerto_madre = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter1;
			if (combobox_vivomuerto_madre.GetActiveIter (out iter1)){
				vivomuertomadre = (string) combobox_vivomuerto_madre.Model.GetValue(iter1,0);
			}
		}
		/////////LLENADO DE COMBOS: Antecedentes Personales NO Patologicos (Positivo o Negativo)/////////////////////////////////////
		void llenado_tabaquismo(int tipodellenado, string pntabaquismo)
		{
			combobox_tabaquismo.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_tabaquismo.PackStart(cell, true);
			combobox_tabaquismo.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_tabaquismo.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pntabaquismo);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_tabaquismo.SetActiveIter (iter);
			}
			combobox_tabaquismo.Changed += new EventHandler (onComboBoxChanged_positivo_negativo_tab);
		}
		void onComboBoxChanged_positivo_negativo_tab (object sender, EventArgs args)
		{
			ComboBox combobox_tabaquismo = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_tabaquismo.GetActiveIter (out iter)){
				pntabaquismo = (string) combobox_tabaquismo.Model.GetValue(iter,0);
			}
		}
		void llenado_alcoholismo(int tipodellenado, string pnalcoholismo)
		{
			combobox_alcoholismo.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_alcoholismo.PackStart(cell, true);
			combobox_alcoholismo.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_alcoholismo.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pnalcoholismo);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_alcoholismo.SetActiveIter (iter);
			}
			combobox_alcoholismo.Changed += new EventHandler (onComboBoxChanged_positivo_negativo_alco);
		}
		void onComboBoxChanged_positivo_negativo_alco (object sender, EventArgs args)
		{
			ComboBox combobox_alcoholismo = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_alcoholismo.GetActiveIter (out iter)){
				pnalcoholismo = (string) combobox_alcoholismo.Model.GetValue(iter,0);
			}
		}
		
		void llenado_drogas(int tipodellenado, string pndrogas)
		{
			combobox_drogas.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_drogas.PackStart(cell, true);
			combobox_drogas.AddAttribute(cell,"text",0);
				        
			ListStore store = new ListStore( typeof (string));
			combobox_drogas.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pndrogas);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_drogas.SetActiveIter (iter);
			}
			combobox_drogas.Changed += new EventHandler (onComboBoxChanged_positivo_negativo_drogas);
		}
		void onComboBoxChanged_positivo_negativo_drogas (object sender, EventArgs args)
		{
			ComboBox combobox_drogas = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_drogas.GetActiveIter (out iter)){
				pndrogas = (string) combobox_drogas.Model.GetValue(iter,0);
			}
		}
        ///////////LLENADO DE COMBOS: Antecedentes Personales Patologicos (Positivo o Negativo)/////////////////////////////////////
		void llenado_cronicodegenerativos(int tipodellenado, string pncronicodegenerativos)
		{
			combobox_cronico_degenerativos.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_cronico_degenerativos.PackStart(cell, true);
			combobox_cronico_degenerativos.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_cronico_degenerativos.Model = store;
	        
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pncronicodegenerativos);
			}
			
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_cronico_degenerativos.SetActiveIter (iter);
			}
			combobox_cronico_degenerativos.Changed += new EventHandler (onComboBoxChanged_cronicodegenerativos);
		}
		void onComboBoxChanged_cronicodegenerativos (object sender, EventArgs args)
		{
			ComboBox combobox_cronico_degenerativos = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_cronico_degenerativos.GetActiveIter (out iter)){
				pncronicodegenerativos = (string) combobox_cronico_degenerativos.Model.GetValue(iter,0);
			}
		}
		
		void llenado_hospitalizaciones(int tipodellenado, string pnhospitalizaciones)
		{
			combobox_hospitalizaciones.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_hospitalizaciones.PackStart(cell, true);
			combobox_hospitalizaciones.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_hospitalizaciones.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pnhospitalizaciones);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_hospitalizaciones.SetActiveIter (iter);
			}
			combobox_hospitalizaciones.Changed += new EventHandler (onComboBoxChanged_hospitalizaciones);
		}
		void onComboBoxChanged_hospitalizaciones (object sender, EventArgs args)
		{
			ComboBox combobox_hospitalizaciones = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_hospitalizaciones.GetActiveIter (out iter)){
				pnhospitalizaciones = (string) combobox_hospitalizaciones.Model.GetValue(iter,0);
			}
		}
		
		void llenado_quirurgicos(int tipodellenado, string pnquirurgicos)
		{
			combobox_quirurgicos.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_quirurgicos.PackStart(cell, true);
			combobox_quirurgicos.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_quirurgicos.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pnquirurgicos);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_quirurgicos.SetActiveIter (iter);
			}
			combobox_quirurgicos.Changed += new EventHandler (onComboBoxChanged_quirurgicos);
		}
		void onComboBoxChanged_quirurgicos (object sender, EventArgs args)
		{
			ComboBox combobox_quirurgicos = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_quirurgicos.GetActiveIter (out iter)){
				pnquirurgicos = (string) combobox_quirurgicos.Model.GetValue(iter,0);
			}
		}
		
		void llenado_alergicos(int tipodellenado, string pnalergicos)
		{
			combobox_alergicos.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_alergicos.PackStart(cell, true);
			combobox_alergicos.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_alergicos.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pnalergicos);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_alergicos.SetActiveIter (iter);
			}
			combobox_alergicos.Changed += new EventHandler (onComboBoxChanged_alergicos);
		}
		void onComboBoxChanged_alergicos (object sender, EventArgs args)
		{
			ComboBox combobox_alergicos = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_alergicos.GetActiveIter (out iter)){
				pnalergicos = (string) combobox_alergicos.Model.GetValue(iter,0);
			}
		}
		
		void llenado_traumaticos(int tipodellenado, string pntraumaticos )
		{
			combobox_traumaticos.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_traumaticos.PackStart(cell, true);
			combobox_traumaticos.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_traumaticos.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pntraumaticos);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
	       TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_traumaticos.SetActiveIter (iter);
			}
			combobox_traumaticos.Changed += new EventHandler (onComboBoxChanged_traumaticos);
		}
		void onComboBoxChanged_traumaticos (object sender, EventArgs args)
		{
			ComboBox combobox_traumaticos = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_traumaticos.GetActiveIter (out iter)){
				pntraumaticos = (string) combobox_traumaticos.Model.GetValue(iter,0);
			}
		}
		
		void llenado_neurologicos(int tipodellenado, string pnneurologicos)
		{
			combobox_neurologicos.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_neurologicos.PackStart(cell, true);
			combobox_neurologicos.AddAttribute(cell,"text",0);
	        
			ListStore store = new ListStore( typeof (string));
			combobox_neurologicos.Model = store;
			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) pnneurologicos);
			}
	        
			store.AppendValues ("");
			store.AppendValues ("Positivo");
			store.AppendValues ("Negativo");
			
			TreeIter iter;
			if (store.GetIterFirst(out iter))
			{
				combobox_neurologicos.SetActiveIter (iter);
			}
			combobox_neurologicos.Changed += new EventHandler (onComboBoxChanged_neurologicos);
		}
		void onComboBoxChanged_neurologicos (object sender, EventArgs args)
		{
			ComboBox combobox_neurologicos = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (combobox_neurologicos.GetActiveIter (out iter)){
				pnneurologicos = (string) combobox_neurologicos.Model.GetValue(iter,0);
			}
		}
		///////////////////////GUARDAR O GRABAR HISTORIA CLINICA DEL PACIENTE///////////////////////////////////////////////////////////////
		void on_button_guardar_clicked(object sender, EventArgs args)
		{         //Pagina 1:
			if( this.entry_pid_paciente.Text == ""  ||                 this.entry_nombre_paciente.Text == "" ||                      this.entry_edad_paciente.Text == ""   ||           
			    Convert.ToString(combobox_vivomuerto_padre) == "" ||    Convert.ToString(combobox_vivomuerto_madre) == "" || 
			    Convert.ToString(combobox_traumaticos) == "" ||         Convert.ToString(combobox_alergicos) == ""  ||                Convert.ToString(combobox_neurologicos) == "" ||
			    Convert.ToString(combobox_hospitalizaciones) == "" ||   Convert.ToString(combobox_cronico_degenerativos) == ""  ||    Convert.ToString(combobox_quirurgicos) == "" ||
                //Spinbuttons:
			    this.spinbutton_ed_madre.Text == "" ||                  this.spinbutton_edad_madre.Text == " " ||
			    this.spinbutton_edad_padre.Text == "" ||                                     
			    //Pagina 2: Motivo de Ingreso:
			    this.entry_motivoingreso.Text == "" ||                  this.entry_padecimientoactual.Text == "" ||                   this.entry_ta.Text == "" ||                              this.entry_fc.Text == "" ||
			    this.entry_fr.Text == "" ||                             this.entry_temp.Text == "" ||                                 this.entry_pso.Text == "" ||                             this.entry_talla.Text == "" ||
			    this.entry_habitus_ext.Text == "" ||                    this.entry_cabeza.Text == "" ||                               this.entry_cuello.Text == "" ||                          this.entry_torax.Text == "" ||
			    this.entry_abdomen.Text == "" ||                        this.entry_extremidades.Text == "" ||                         this.entry_genitourinario.Text == "" ||                  this.entry_neurologico.Text == "" ||
			    this.entry_diagnosticos.Text == "" ||                   this.entry_plan_diag.Text == "" ||                            this.entry_nombre_plan_diag.Text == "")
		   {
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				                                               ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();
			
			}else{		
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				
				if (miResultado == ResponseType.Yes){
					guarda_historia_clinica();
				}else{
						
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}		
			}
		}
		
		void guarda_historia_clinica ()
		{	
			if(editando == true) {
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();						 
					comando.CommandText = "UPDATE hscmty_his_historia_clinica "+
						    "SET " +
							"id_quien_actualizo = '" +LoginEmpleado+"', "+
							"fechahora_actualizacion = '" +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
							"descripcion_enfermedad_padre = '" +this.entry_enfermedad_padre.Text+"', "+
							"descripcion_enfermedad_madre = '" +this.entry_enfermedad_madre.Text+"', "+
							"descripcion_enfermedad_hermanos = '" +this.entry_enfermedad_hermanos.Text+"', "+
							"descripcion_enfermedad_hijos = '" +this.entry_enfermedad_hijos.Text+"', "+
							"descripcion_enfermedad_apaternos = '" +this.entry_enfermedad_apaternos.Text+"', "+
							"descripcion_enfermedad_amaternos = '" +this.entry_enfermedad_amaternos.Text+"', "+
							"observaciones_heredo_familiar = '" +this.entry_otros_ahf.Text+"', "+
	                        "tipo_casahabitacion = '" +this.entry_tipo_casahabit.Text+"', "+
							"no_patologicos_observaciones = '" +this.entry_observaciones.Text+"', "+
	                        "medicamentos_actuales = '" +this.entry_medicamentos.Text+"', "+
							"observaciones_patologicos = '" +this.entry_otros_app.Text+"', "+
							//pagina 2
							"ginecoobstetricios_ivsa = '" +this.entry_ivsa.Text+"', "+
							"ginecoobstetricios_ritmo = '" +this.entry_ritmo.Text+"', "+
							"ginecoobstetricios_contracepcion = '" +this.entry_contracepcion.Text+"', "+
							"ginecoobstetricios_pap = '" +this.entry_pap.Text+"', "+
							"ginecoobstetricios_otros = '" +this.entry_otros_ago.Text +"', "+
							"ginecoobstetricios_fum = '" +DateTime.Now.ToString("yyyy-MM-dd")+"', "+     //'" +this.entry_fum.Text+"', "+
							"ginecoobstetricios_fpp = '" +DateTime.Now.ToString("yyyy-MM-dd")+"', "+     //'" +this.entry_fpp.Text+"', "+
							"ginecoobstetricios_fup = '" +DateTime.Now.ToString("yyyy-MM-dd")+"', "+     //'" +this.entry_fup.Text +"', "+
							"hcpediatrica_perinatales = '" +this.entry_perinatales.Text+"', "+
							"hcpediatrica_peso = '" +this.entry_peso.Text+"', "+
							"hcpediatrica_patologicos = '" +this.entry_patologicos.Text+"', "+
							"hcpediatrica_alumbramiento = '" +this.entry_alumbramiento.Text+"', "+
							"hcpediatrica_infecciones = '" +this.entry_infecciones.Text+"', "+
							"hcpediatrica_alergias = '" +this.entry_alergias.Text+"', "+
							"hcpediatrica_hospitalizaciones = '" +this.entry_hospitalizaciones.Text+"', "+
							"hcpediatrica_traumatismos = '" +this.entry_traumatismos.Text+"', "+
							"hcpediatrica_cirugias = '" +this.entry_cirugias.Text+"', "+
							"hcpediatrica_inmunizaciones = '" +this.entry_inmunizaciones.Text+"', "+
							"hcpediatrica_des_psicomotor = '" +this.entry_des_psicomotor.Text+"', "+
							"hcpediatrica_otros = '" +this.entry_otros_hcp.Text+"', "+
							////PAG.3://////
							"padecimiento_actual = '" +entry_padecimientoactual.Text+"', "+
							"motivo_de_ingreso = '" +entry_motivoingreso.Text+"', "+
							"psesion_arterial = '" +entry_ta.Text+"', "+
							"frecuencia_cardiaca = '" +entry_fc.Text+"', "+
							"frecuencia_respiratoria = '" +entry_fr.Text+"', "+
							"temperatura = '" +entry_temp.Text+"', "+
							"peso = '" +entry_pso.Text+"', "+
							"talla = '" +entry_talla.Text+"', "+
							"habitus_exterior = '" +entry_habitus_ext.Text+"', "+
							"cabeza = '" +entry_cabeza.Text+"', "+
							"cuello = '" +entry_cuello.Text+"', "+
							"torax = '" +entry_torax.Text+"', "+
							"abdomen = '" +entry_abdomen.Text+"', "+
							"extremidades = '" +entry_extremidades.Text+"', "+
							"genitourinario = '" +entry_genitourinario.Text+"', "+
							"neurologico = '" +entry_neurologico.Text+"', "+
							"diagnosticos = '" +entry_diagnosticos.Text+"', "+
							"plan_diagnostico = '" +entry_plan_diag.Text+"', "+
							"nombre_plan_diag = '" +entry_nombre_plan_diag.Text+"', "+
							//nuevos entrys
							"observaciones_cdegenerativos = '" +this.entry_cronicodegenerativo.Text+"', "+ 
		        	        "observaciones_alergicos = '" +this.entry_alergicos.Text+"', "+ 
		       	 			"observaciones_hosp = '" +this.entry_obs_hospi.Text+"', "+ 
		        			"observaciones_traumaticos = '" +this.entry_traumaticos.Text+"', "+ 
		        			"observaciones_quirur = '" +this.entry_quirurgicos.Text+"', "+ 
		       	   		    "observaciones_neurolog = '" +this.entry_neurologicos.Text+"', "+ 
							//////COMBOBOX://////////
						    "padre_v_m = '" +vivomuertopadre.ToString().ToUpper()+"', "+
							"madre_v_m = '" +vivomuertomadre.ToString().ToUpper()+"', "+
							"tabaquismo_p_n = '" +pntabaquismo.ToString().ToUpper()+"', "+
							"alcoholismo_p_n = '" +pnalcoholismo.ToString().ToUpper()+"', "+
							"drogas_p_n = '" +pndrogas.ToString().ToUpper()+"', "+
							"cronico_degenerativo_p_n = '" +pncronicodegenerativos.ToString().ToUpper()+"', "+
							"hospitalizaciones_p_n = '" +pnhospitalizaciones.ToString().ToUpper()+"', "+
							"quirurgicos_p_n = '" +pnquirurgicos.ToString().ToUpper()+"', "+
							"alergicos_p_n = '" +pnalergicos.ToString().ToUpper()+"', "+
							"traumaticos_p_n = '" +pntraumaticos.ToString().ToUpper()+"', "+
							"neurologicos_p_n = '" +pnneurologicos.ToString().ToUpper()+"', "+
							/////SPINBUTTONS://////
							"padre_edad = '" +this.spinbutton_edad_padre.Text+"', "+              
							"madre_edad = '" +this.spinbutton_edad_madre.Text+"', "+               
							"hermanos_nrovivos = '" +this.spinbutton_novivos_hermanos.Text+"', "+         
							"hermanos_nromuertos = '" +this.spinbutton_nomuertos_hermanos.Text+"', "+        
							"hijos_nrovivos = '" +this.spinbutton_novivos_hijos.Text+"', "+            
							"hijos_nromuertos = '" +this.spinbutton_nomuertos_hijos.Text+"', "+           
							"abuelospaternos_nrovivos = '" +this.spinbutton_novivos_apaternos.Text+"', "+         
							"abuelospaternos_nromuertos = '" +this.spinbutton_nomuertos_apaternos.Text+"', "+       
							"abuelosmaternos_nrovivos = '" +this.spinbutton_novivos_amaternos.Text+"', "+         
							"abuelosmaternos_nromuertos = '" +this.spinbutton_nomuertos_amaternos.Text+"', "+       
							"ginecoobstetricios_menarca = '" +this.spinbutton_menarca.Text+"', "+                                                                              
							"ginecoobstetricios_gestacion = '" +this.spinbutton_g.Text+"', "+                         
							"ginecoobstetricios_parto = '" +this.spinbutton_p.Text+"', "+                         
							"ginecoobstetricios_cesarea = '" +this.spinbutton_c.Text+"', "+                         
							"ginecoobstetricios_aborto = '" +this.spinbutton_a.Text+"', "+                         
							"hcpediatrica_noembarazo = '" +this.spinbutton_no_embarazo.Text+"', "+               
							"hcpediatrica_edad_madre = '" +this.spinbutton_ed_madre.Text+"', "+                  
							"hcpediatrica_edadgestional = '" +this.spinbutton_edad_gestional.Text+"' "+
							"WHERE pid_paciente = '"+this.entry_pid_paciente.Text+"'; ";
				
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();
				comando.Dispose();
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
					                                          ButtonsType.Ok,"La Actualizacion se efectuo satisfactoriamente");										
					msgBox.Run ();	msgBox.Destroy();
					cierra_campos ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error, 
					                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
				
			}
			if (editando == false) {
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();						 
					comando.CommandText = "UPDATE hscmty_his_paciente "+
						                  "SET historia_clinica = 'true' "+ 
							              "WHERE pid_paciente = '"+this.entry_pid_paciente.Text+"'; ";
					Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
					comando.Dispose();
					comando.CommandText = "INSERT INTO hscmty_his_historia_clinica ( "+
					                	     "id_quien_creo, "+                                 //1                               
									         "fechahora_creacion, "+                            //2
											 "pid_paciente, "+                                  //3
									 ////(Antecedentes Heredo Familiar )////
											 "descripcion_enfermedad_padre,"+                   //4  
											 "descripcion_enfermedad_madre,"+                   //5
							                 "descripcion_enfermedad_hermanos, "+               //6  
	        								 "descripcion_enfermedad_hijos, "+                  //7
											 "descripcion_enfermedad_apaternos, "+              //8
											 "descripcion_enfermedad_amaternos, "+              //9
											 "observaciones_heredo_familiar, "+                 //10
										 ////(Antecedentes Personales No Patologicos )////
											 "tipo_casahabitacion, "+                           //11              
											 "no_patologicos_observaciones, "+                  //12
										 ////(Antecedentes Personales Patologicos)////
											 "medicamentos_actuales, "+                         //13             
											 "observaciones_patologicos, "+                     //14
										 //pagina2
							             ////(Antecedentes Gineco Obstetricios)////
											 "ginecoobstetricios_ivsa, "+                       //15
											 "ginecoobstetricios_ritmo, "+                      //16
											 "ginecoobstetricios_contracepcion, "+              //17 
											 "ginecoobstetricios_pap, "+                        //18
											 "ginecoobstetricios_otros, "+                      //19
											 "ginecoobstetricios_fum, "+                        //20
											 "ginecoobstetricios_fpp, "+                        //21
											 "ginecoobstetricios_fup, "+                        //22
										 ////(Historia Clinica Pediatrica)////
											 "hcpediatrica_perinatales, "+                      //23
											 "hcpediatrica_peso, "+                             //24
											 "hcpediatrica_patologicos, "+                      //25                   
											 "hcpediatrica_alumbramiento, "+                    //26
											 "hcpediatrica_infecciones, "+                      //27                     
											 "hcpediatrica_alergias, "+                         //28                        
											 "hcpediatrica_hospitalizaciones, "+                //29 
											 "hcpediatrica_traumatismos, "+                     //30                  
											 "hcpediatrica_cirugias, "+                         //31                   
											 "hcpediatrica_inmunizaciones, "+                   //32                  
											 "hcpediatrica_des_psicomotor, "+                   //33                   
											 "hcpediatrica_otros, "+                            //34
										 //////////( PAGINA  3)//////////////
											 "padecimiento_actual, "+                           //35                        
											 "motivo_de_ingreso, "+                             //36                    
											 "psesion_arterial, "+                              //37                   
											 "frecuencia_cardiaca, "+                           //38                      
											 "frecuencia_respiratoria, "+                       //39                 
											 "temperatura,"+                                    //40
											 "peso, "+                                          //41                              
											 "talla, "+                                         //42                                   
											 "habitus_exterior, "+                              //43                              
											 "cabeza, "+                                        //44                                
											 "cuello, "+                                        //45                                  
											 "torax, "+                                         //46                                   
											 "abdomen, "+                                       //47                                 
											 "extremidades, "+                                  //48                            
											 "genitourinario, "+                                //49                            
											 "neurologico, "+                                   //50                               
											 "diagnosticos, "+                                  //51                              
											 "plan_diagnostico, "+                              //52
											 "nombre_plan_diag, "+                              //53
										//entrys nuevos
											 "observaciones_cdegenerativos, "+  
		        	                         "observaciones_alergicos, "+ 
		       	 			                 "observaciones_hosp, "+ 
		        			                 "observaciones_traumaticos, "+  
		        			                 "observaciones_quirur, "+ 
		       	   		                     "observaciones_neurolog, "+
							            //////Combobox://////////
							                 "padre_v_m, "+                                     //54
							                 "madre_v_m, "+                                     //55
							                 "tabaquismo_p_n, "+                                //56
							                 "alcoholismo_p_n, "+                               //57
							                 "drogas_p_n, "+                                    //58
	                                         "cronico_degenerativo_p_n, "+                      //59
							                 "hospitalizaciones_p_n, "+                         //60
							                 "quirurgicos_p_n, "+                               //61
							                 "alergicos_p_n, "+                                 //62
							                 "traumaticos_p_n, "+                               //63
							                 "neurologicos_p_n, "+                              //64
							            ///////Spinbuttons://///////
											 "padre_edad, "+                                    //65
											 "madre_edad, "+                                    //66
											 "hermanos_nrovivos, "+                             //67
											 "hermanos_nromuertos, "+                           //68 
											 "hijos_nrovivos, "+                                //69
											 "hijos_nromuertos, "+                              //70
											 "abuelospaternos_nrovivos, "+                      //71  
											 "abuelospaternos_nromuertos, "+                    //72    
											 "abuelosmaternos_nrovivos, "+                      //73
											 "abuelosmaternos_nromuertos, "+                    //74
											 "ginecoobstetricios_menarca, "+                    //75
											 "ginecoobstetricios_gestacion, "+                  //76
											 "ginecoobstetricios_parto, "+                      //77
											 "ginecoobstetricios_cesarea, "+                    //78
											 "ginecoobstetricios_aborto, "+                     //79
											 "hcpediatrica_noembarazo, "+                       //80
											 "hcpediatrica_edad_madre, "+                       //81
											 "hcpediatrica_edadgestional ) "+                   //82
										///////////VALUES Entrys/////////////
											 "VALUES ('"+LoginEmpleado+"', "+                         //1   
											 "'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+  //2
											 "'"+this.entry_pid_paciente.Text+"', "+                  //3
										/////////////(AEF)/////////////////////////////////
											 "'"+this.entry_enfermedad_padre.Text+"', "+              //4  
											 "'"+this.entry_enfermedad_madre.Text+"', "+              //5
											 "'"+this.entry_enfermedad_hermanos.Text+"', "+           //6  
											 "'"+this.entry_enfermedad_hijos.Text+"', "+              //7
											 "'"+this.entry_enfermedad_apaternos.Text+"', "+          //8
											 "'"+this.entry_enfermedad_amaternos.Text+"', "+          //9   
											 "'"+this.entry_otros_ahf.Text+"', "+                     //10 
										/////////////(APNP)/////////////////////////////////
											 "'"+this.entry_tipo_casahabit.Text+"', "+                //11
											 "'"+this.entry_observaciones.Text+"', "+                 //12
										/////////////(APP)/////////////////////////////////
											 "'"+this.entry_medicamentos.Text+"', "+                  //13
											 "'"+this.entry_otros_app.Text+"', "+                     //14
										/////////////(Pag.2)/////////////////////////////////
							            /////////////(AGO)/////////////////////////////////
											 "'"+this.entry_ivsa.Text+"', "+                          //15
											 "'"+this.entry_ritmo.Text+"', "+                         //16
											 "'"+this.entry_contracepcion.Text+"', "+                 //17
											 "'"+this.entry_pap.Text+"', "+                           //18    
											 "'"+this.entry_otros_ago.Text+"', "+                     //19
											 "'"+this.entry_fum.Text+"', "+                           //20
											 "'"+this.entry_fpp.Text+"', "+                           //21
											 "'"+this.entry_fup.Text+"', "+                           //22
										 /////////////(HCP)/////////////////////////////////
											 "'"+this.entry_perinatales.Text+"', "+                   //23
											 "'"+this.entry_peso.Text+"', "+                          //24
									    	 "'"+this.entry_patologicos.Text+"', "+                   //25
										     "'"+this.entry_alumbramiento.Text+"', "+                 //26
											 "'"+this.entry_infecciones.Text+"', "+                   //27
								    		 "'"+this.entry_alergias.Text+"', "+                      //28
									  	     "'"+this.entry_hospitalizaciones.Text+"', "+             //29
										     "'"+this.entry_traumatismos.Text+"', "+                  //30
											 "'"+this.entry_cirugias.Text+"', "+                      //31
											 "'"+this.entry_inmunizaciones.Text+"', "+                //32
											 "'"+this.entry_des_psicomotor.Text+"', "+                //33
											 "'"+this.entry_otros_hcp.Text+"', "+                     //34
										 /////////////(Pag.3)/////////////////////////////////
											 "'"+entry_padecimientoactual.Text+"', "+                 //35
											 "'"+entry_motivoingreso.Text+"', "+  
											 "'"+entry_ta.Text+"', "+     
											 "'"+entry_fc.Text+"', "+  
											 "'"+entry_fr.Text+"', "+      
											 "'"+entry_temp.Text+"', "+ 
											 "'"+entry_pso.Text+"', "+ 
											 "'"+entry_talla.Text+"', "+ 
											 "'"+entry_habitus_ext.Text+"', "+  
											 "'"+entry_cabeza.Text+"', "+ 
											 "'"+entry_cuello.Text+"', "+  
											 "'"+entry_torax.Text+"', "+  
											 "'"+entry_abdomen.Text+"', "+     
											 "'"+entry_extremidades.Text+"', "+     
											 "'"+entry_genitourinario.Text+"', "+ 
											 "'"+entry_neurologico.Text+"', "+    
											 "'"+entry_diagnosticos.Text+"', "+   
											 "'"+entry_plan_diag.Text+"', "+    
											 "'"+entry_nombre_plan_diag.Text+"', "+                    //53
							             //entrys nuevos
							                 "'"+this.entry_cronicodegenerativo.Text+"', "+  
											 "'"+this.entry_alergicos.Text+"', "+ 
											 "'"+this.entry_obs_hospi.Text+"', "+ 
											 "'"+this.entry_traumaticos.Text+"', "+   
											 "'"+this.entry_quirurgicos.Text+"', "+ 
											 "'"+this.entry_neurologicos.Text+"', "+
							             /////////////Combobox:////////////////////////////////////////////
											 "'"+vivomuertopadre.ToString().ToUpper()+"', "+           //54
							                 "'"+vivomuertomadre.ToString().ToUpper()+"', "+
							                 "'"+pntabaquismo.ToString().ToUpper()+"', "+
							                 "'"+pnalcoholismo.ToString().ToUpper()+"', "+
							                 "'"+pndrogas.ToString().ToUpper()+"', "+
							                 "'"+pncronicodegenerativos.ToString().ToUpper()+"', "+
							                 "'"+pnhospitalizaciones.ToString().ToUpper()+"', "+
							                 "'"+pnquirurgicos.ToString().ToUpper()+"', "+
							                 "'"+pnalergicos.ToString().ToUpper()+"', "+
							                 "'"+pntraumaticos.ToString().ToUpper()+"', "+
							                 "'"+pnneurologicos.ToString().ToUpper()+"', "+            //64
										 /////////////Spinbuttons://///////////////////////////////
											 "'"+this.spinbutton_edad_padre.Text+"', "+                //65
											 "'"+this.spinbutton_edad_madre.Text+"', "+                //66
											 "'"+this.spinbutton_novivos_hermanos.Text+"', "+          //67
											 "'"+this.spinbutton_nomuertos_hermanos.Text+"', "+        //68
											 "'"+this.spinbutton_novivos_hijos.Text+"', "+             //69
											 "'"+this.spinbutton_nomuertos_hijos.Text+"', "+           //70
											 "'"+this.spinbutton_novivos_apaternos.Text+"', "+         //71
											 "'"+this.spinbutton_nomuertos_apaternos.Text+"', "+       //72
											 "'"+this.spinbutton_novivos_amaternos.Text+"', "+         //73
											 "'"+this.spinbutton_nomuertos_amaternos.Text+"', "+       //74
							                 "'"+this.spinbutton_menarca.Text+"', "+                   //75                                                           
											 "'"+this.spinbutton_g.Text+"', "+                         //76
											 "'"+this.spinbutton_p.Text+"', "+                         //77
											 "'"+this.spinbutton_c.Text+"', "+                         //78
											 "'"+this.spinbutton_a.Text+"', "+                         //79
											 "'"+this.spinbutton_no_embarazo.Text+"', "+               //80
											 "'"+this.spinbutton_ed_madre.Text+"', "+                  //81
											 "'"+this.spinbutton_edad_gestional.Text+"');";            //82
									
					//Console.WriteLine(spinbutton_edad_padre);		                                                                
					Console.WriteLine(comando.CommandText);				
					comando.ExecuteNonQuery();
					comando.Dispose();
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
					                                          ButtonsType.Ok,"La Historia Clinica del Paciente se guardo satisfactoriamente");										
					msgBox.Run ();	msgBox.Destroy();
				
					cierra_campos ();
				
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
				editando = false;
			}
		}
		
		///////////////////////BUTTON IMPRIMIR HISTORIA CLINICA DEL PACIENTE///////////////////////////////////////////////////////////////
		void on_button_imprimir_historia_clinica_clicked(object sender, EventArgs args)
		{     //Pagina 1:
			if( this.entry_pid_paciente.Text == ""  ||                 this.entry_nombre_paciente.Text == "" ||                      this.entry_edad_paciente.Text == ""  ||                       
		       //Antecedentes Heredo Familiar:     
			   Convert.ToString(combobox_vivomuerto_padre) == "" ||    Convert.ToString(combobox_vivomuerto_madre) == "" ||          
			  this.entry_otros_ahf.Text == "" ||
			   //Antecedentes Personales NO Patologicos:
			   Convert.ToString(combobox_alcoholismo) == "" ||         Convert.ToString(combobox_tabaquismo) == "" ||                Convert.ToString(combobox_drogas) == "" ||               this.entry_tipo_casahabit.Text == "" ||              
			   this.entry_observaciones.Text == "" || 
			   //Antecedentes Personales Patologicos:
			   Convert.ToString(combobox_traumaticos) == "" ||         Convert.ToString(combobox_alergicos) == ""  ||                Convert.ToString(combobox_neurologicos) == "" ||         this.entry_otros_app.Text == "" ||
			   Convert.ToString(combobox_hospitalizaciones) == "" ||   Convert.ToString(combobox_cronico_degenerativos) == ""  ||    Convert.ToString(combobox_quirurgicos) == "" ||          this.entry_medicamentos.Text == "" ||
			   //Historia Clinica Pediatrica:                     
			   this.entry_perinatales.Text == "" ||                    this.entry_peso.Text == "" ||                                 this.entry_patologicos.Text == "" ||                     this.entry_alumbramiento.Text == "" ||
			   this.entry_infecciones.Text == "" ||                    this.entry_alergias.Text == "" ||                             this.entry_hospitalizaciones.Text == "" ||               this.entry_traumatismos.Text == "" ||
			   this.entry_cirugias.Text == "" ||                       this.entry_inmunizaciones.Text == "" ||                       this.entry_des_psicomotor.Text == "" ||                  this.entry_otros_hcp.Text == "" ||
			   //Pagina 2: Motivo de Ingreso:
			   this.entry_motivoingreso.Text == "" ||                  this.entry_padecimientoactual.Text == "" ||                   this.entry_ta.Text == "" ||                              this.entry_fc.Text == "" ||
			   this.entry_fr.Text == "" ||                             this.entry_temp.Text == "" ||                                 this.entry_pso.Text == "" ||                             this.entry_talla.Text == "" ||
			   this.entry_habitus_ext.Text == "" ||                    this.entry_cabeza.Text == "" ||                               this.entry_cuello.Text == "" ||                          this.entry_torax.Text == "" ||
			   this.entry_abdomen.Text == "" ||                        this.entry_extremidades.Text == "" ||                         this.entry_genitourinario.Text == "" ||                  this.entry_neurologico.Text == "" ||
			   this.entry_diagnosticos.Text == "" ||                   this.entry_plan_diag.Text == "" ||                            this.entry_nombre_plan_diag.Text == "")
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				                                               ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();
			
			}else{ 
				new osiris.rpt_historia_clinica(entry_pid_paciente.Text,entry_nombre_paciente.Text,this.nombrebd,entry_fpp.Text,entry_fum.Text,entry_fup.Text,entry_edad_paciente.Text,this.fecha_admision,this.fecha_nacimiento);
			}
		}
		
		void on_button_imprimir_p2_clicked(object sender, EventArgs args)
		{     //Pagina 1:
			if( this.entry_pid_paciente.Text == ""  ||                 this.entry_nombre_paciente.Text == "" ||                      this.entry_edad_paciente.Text == ""  ||                       
		       //Antecedentes Heredo Familiar:     
			   Convert.ToString(combobox_vivomuerto_padre) == "" ||    Convert.ToString(combobox_vivomuerto_madre) == "" ||          
			  this.entry_otros_ahf.Text == "" ||
			   //Antecedentes Personales NO Patologicos:
			   Convert.ToString(combobox_alcoholismo) == "" ||         Convert.ToString(combobox_tabaquismo) == "" ||                Convert.ToString(combobox_drogas) == "" ||               this.entry_tipo_casahabit.Text == "" ||              
			   this.entry_observaciones.Text == "" || 
			   //Antecedentes Personales Patologicos:
			   Convert.ToString(combobox_traumaticos) == "" ||         Convert.ToString(combobox_alergicos) == ""  ||                Convert.ToString(combobox_neurologicos) == "" ||         this.entry_otros_app.Text == "" ||
			   Convert.ToString(combobox_hospitalizaciones) == "" ||   Convert.ToString(combobox_cronico_degenerativos) == ""  ||    Convert.ToString(combobox_quirurgicos) == "" ||          this.entry_medicamentos.Text == "" ||
			   //Historia Clinica Pediatrica:                     
			   this.entry_perinatales.Text == "" ||                    this.entry_peso.Text == "" ||                                 this.entry_patologicos.Text == "" ||                     this.entry_alumbramiento.Text == "" ||
			   this.entry_infecciones.Text == "" ||                    this.entry_alergias.Text == "" ||                             this.entry_hospitalizaciones.Text == "" ||               this.entry_traumatismos.Text == "" ||
			   this.entry_cirugias.Text == "" ||                       this.entry_inmunizaciones.Text == "" ||                       this.entry_des_psicomotor.Text == "" ||                  this.entry_otros_hcp.Text == "" ||
			   //Pagina 2: Motivo de Ingreso:
			   this.entry_motivoingreso.Text == "" ||                  this.entry_padecimientoactual.Text == "" ||                   this.entry_ta.Text == "" ||                              this.entry_fc.Text == "" ||
			   this.entry_fr.Text == "" ||                             this.entry_temp.Text == "" ||                                 this.entry_pso.Text == "" ||                             this.entry_talla.Text == "" ||
			   this.entry_habitus_ext.Text == "" ||                    this.entry_cabeza.Text == "" ||                               this.entry_cuello.Text == "" ||                          this.entry_torax.Text == "" ||
			   this.entry_abdomen.Text == "" ||                        this.entry_extremidades.Text == "" ||                         this.entry_genitourinario.Text == "" ||                  this.entry_neurologico.Text == "" ||
			   this.entry_diagnosticos.Text == "" ||                   this.entry_plan_diag.Text == "" ||                            this.entry_nombre_plan_diag.Text == "")
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
				                                               ButtonsType.Close,"Favor de llenar toda la informaciòn correspondiente");
				msgBoxError.Run ();					msgBoxError.Destroy();
			//,entry_ta.Text,entry_fc.Text,entry_fr.Text,entry_temp.Text,entry_pso.Text,entry_talla.Text
			}else{ 
				new osiris.imprime_pag2(entry_pid_paciente.Text,this.nombrebd);
			}
		}
		
		////////////////////// EDITAR HISTORIA CLINICA /////////////////////////////////////////////////////////////////////////////////////////////////
		void on_button_editar_his_clin_clicked (object sender, EventArgs args)
		{
			 editando = true;
			 this.button_guardar.Sensitive = true;                       this.entry_nombre_paciente.Sensitive = true;			       this.entry_pid_paciente.Sensitive = true;        this.entry_edad_paciente.Sensitive = true;
			 this.button_imprimir.Sensitive = true;                      this.button_imprimir_p2.Sensitive = true;
            //Antecedentes Heredo Familiar:     
			 this.entry_enfermedad_padre.Sensitive = true;               this.entry_enfermedad_madre.Sensitive = true;                 this.entry_enfermedad_amaternos.Sensitive = true; 
			 this.entry_enfermedad_hermanos.Sensitive = true;            this.entry_enfermedad_hijos.Sensitive = true;                 this.entry_enfermedad_apaternos.Sensitive = true;             
			 this.entry_otros_ahf.Sensitive = true; 
			 //Antecedentes Personales NO Patologicos:
			 this.entry_tipo_casahabit.Sensitive = true;                 this.entry_observaciones.Sensitive = true;           
			 //Antecedentes Personales Patologicos:
			 this.entry_otros_app.Sensitive = true;                      this.entry_medicamentos.Sensitive = true;
            //nuevos entrys:
			 this.entry_cronicodegenerativo.Sensitive = true;
		     this.entry_alergicos.Sensitive = true;
		     this.entry_obs_hospi.Sensitive = true;
		     this.entry_traumaticos.Sensitive = true;
		     this.entry_quirurgicos.Sensitive = true;
		     this.entry_neurologicos.Sensitive = true;
			//pagina 2: Antecedentes Gineco Obstetricios:                           
			 this.entry_ivsa.Sensitive = true;                           this.entry_ritmo.Sensitive = true;                            this.entry_fum.Sensitive = true;                  this.entry_fpp.Sensitive = true;                           
			 this.entry_pap.Sensitive = true;                            this.entry_contracepcion.Sensitive = true;                    this.entry_otros_ago.Sensitive = true;            this.entry_fup.Sensitive = true;
			 //Historia Clinica Pediatrica:                     
			 this.entry_perinatales.Sensitive = true;                    this.entry_peso.Sensitive = true;                             this.entry_patologicos.Sensitive = true;          this.entry_alumbramiento.Sensitive = true; 
			 this.entry_infecciones.Sensitive = true;                    this.entry_alergias.Sensitive = true;                         this.entry_hospitalizaciones.Sensitive = true;    this.entry_traumatismos.Sensitive = true; 
			 this.entry_cirugias.Sensitive = true;                       this.entry_inmunizaciones.Sensitive = true;                   this.entry_des_psicomotor.Sensitive = true;       this.entry_otros_hcp.Sensitive = true; 
			 //Pagina 3: Motivo de Ingreso:
			 this.entry_motivoingreso.Sensitive = true;                  this.entry_padecimientoactual.Sensitive = true;               this.entry_ta.Sensitive = true;                   this.entry_fc.Sensitive = true; 
			 this.entry_fr.Sensitive = true;                             this.entry_temp.Sensitive = true;                             this.entry_pso.Sensitive = true;                  this.entry_talla.Sensitive = true;
			 this.entry_habitus_ext.Sensitive = true;                    this.entry_cabeza.Sensitive = true;                           this.entry_cuello.Sensitive = true;               this.entry_torax.Sensitive = true;
			 this.entry_abdomen.Sensitive = true;                        this.entry_extremidades.Sensitive = true;                     this.entry_genitourinario.Sensitive = true;       this.entry_neurologico.Sensitive = true; 
			 this.entry_diagnosticos.Sensitive = true;                   this.entry_plan_diag.Sensitive = true;                        this.entry_nombre_plan_diag.Sensitive = true; 			
             //combobox:			
			 combobox_vivomuerto_padre.Sensitive = true;                 combobox_vivomuerto_madre.Sensitive = true;       
			 combobox_alcoholismo.Sensitive = true;                      combobox_tabaquismo.Sensitive = true;                         combobox_drogas.Sensitive = true;
			 combobox_traumaticos.Sensitive = true;                      combobox_alergicos.Sensitive = true;                          combobox_neurologicos.Sensitive = true;     
			 combobox_hospitalizaciones.Sensitive = true;                combobox_cronico_degenerativos.Sensitive = true;              combobox_quirurgicos.Sensitive = true;      
			 //spinbuttons:
			 this.spinbutton_a.Sensitive = true;                         this.spinbutton_c.Sensitive = true;                           this.spinbutton_ed_madre.Sensitive = true;                         this.spinbutton_g.Sensitive =true;                          this.spinbutton_nomuertos_hijos.Sensitive = true;
			 this.spinbutton_edad_gestional.Sensitive = true;            this.spinbutton_edad_madre.Sensitive = true;                  this.spinbutton_edad_padre.Sensitive = true;                       this.spinbutton_menarca.Sensitive = true;                   this.spinbutton_novivos_amaternos.Sensitive = true;
			 this.spinbutton_no_embarazo.Sensitive = true;               this.spinbutton_nomuertos_amaternos.Sensitive = true;         this.spinbutton_nomuertos_apaternos.Sensitive = true;              this.spinbutton_nomuertos_hermanos.Sensitive = true;        this.spinbutton_novivos_apaternos.Sensitive = true;
			 this.spinbutton_novivos_hermanos.Sensitive = true;          this.spinbutton_novivos_hijos.Sensitive = true;               this.spinbutton_p.Sensitive = true;
		}
		
		void cierra_campos ()
		{
			this.button_guardar.Sensitive = false;                       this.entry_nombre_paciente.Sensitive = false;			        this.entry_pid_paciente.Sensitive = false;        this.entry_edad_paciente.Sensitive = false;
			this.button_editar_his_clin.Sensitive = true;                this.button_imprimir.Sensitive = true;                         this.button_imprimir_p2.Sensitive = true;
			//Antecedentes Heredo Familiar:     
			this.entry_enfermedad_padre.Sensitive = false;               this.entry_enfermedad_madre.Sensitive = false;                 this.entry_enfermedad_amaternos.Sensitive = false; 
			this.entry_enfermedad_hermanos.Sensitive = false;            this.entry_enfermedad_hijos.Sensitive = false;                 this.entry_enfermedad_apaternos.Sensitive = false;             
			this.entry_otros_ahf.Sensitive = false; 
			//Antecedentes Personales NO Patologicos:
			this.entry_tipo_casahabit.Sensitive = false;                 this.entry_observaciones.Sensitive = false;           
			//Antecedentes Personales Patologicos:
			this.entry_otros_app.Sensitive = false;                      this.entry_medicamentos.Sensitive = false;
			//entrys nuevos
			this.entry_cronicodegenerativo.Sensitive = false;
		    this.entry_alergicos.Sensitive = false;
		    this.entry_obs_hospi.Sensitive = false;
		    this.entry_traumaticos.Sensitive = false;
		    this.entry_quirurgicos.Sensitive = false;
		    this.entry_neurologicos.Sensitive = false;
			//pagina 2 Antecedentes Gineco Obstetricios:                           
			this.entry_ivsa.Sensitive = false;                           this.entry_ritmo.Sensitive = false;                            this.entry_fum.Sensitive = false;                  this.entry_fpp.Sensitive = false;                           
			this.entry_pap.Sensitive = false;                            this.entry_contracepcion.Sensitive = false;                    this.entry_otros_ago.Sensitive = false;            this.entry_fup.Sensitive = false;
			//Historia Clinica Pediatrica:                     
			this.entry_perinatales.Sensitive = false;                    this.entry_peso.Sensitive = false;                             this.entry_patologicos.Sensitive = false;          this.entry_alumbramiento.Sensitive = false; 
			this.entry_infecciones.Sensitive = false;                    this.entry_alergias.Sensitive = false;                         this.entry_hospitalizaciones.Sensitive = false;    this.entry_traumatismos.Sensitive = false; 
			this.entry_cirugias.Sensitive = false;                       this.entry_inmunizaciones.Sensitive = false;                   this.entry_des_psicomotor.Sensitive = false;       this.entry_otros_hcp.Sensitive = false; 
			//Pagina 3: Motivo de Ingreso:
			this.entry_motivoingreso.Sensitive = false;                  this.entry_padecimientoactual.Sensitive = false;               this.entry_ta.Sensitive = false;                   this.entry_fc.Sensitive = false; 
			this.entry_fr.Sensitive = false;                             this.entry_temp.Sensitive = false;                             this.entry_pso.Sensitive = false;                  this.entry_talla.Sensitive = false; 
			this.entry_habitus_ext.Sensitive = false;                    this.entry_cabeza.Sensitive = false;                           this.entry_cuello.Sensitive = false;               this.entry_torax.Sensitive = false; 
			this.entry_abdomen.Sensitive = false;                        this.entry_extremidades.Sensitive = false;                     this.entry_genitourinario.Sensitive = false;       this.entry_neurologico.Sensitive = false; 
			this.entry_diagnosticos.Sensitive = false;                   this.entry_plan_diag.Sensitive = false;                        this.entry_nombre_plan_diag.Sensitive = false; 			
			//combobox:			
			combobox_vivomuerto_padre.Sensitive = false;                 combobox_vivomuerto_madre.Sensitive = false;       
			combobox_alcoholismo.Sensitive = false;                      combobox_tabaquismo.Sensitive = false;                         combobox_drogas.Sensitive = false;
			combobox_traumaticos.Sensitive = false;                      combobox_alergicos.Sensitive = false;                          combobox_neurologicos.Sensitive = false;      
			combobox_hospitalizaciones.Sensitive = false;                combobox_cronico_degenerativos.Sensitive = false;              combobox_quirurgicos.Sensitive = false;       
			//spinbuttons:
			this.spinbutton_a.Sensitive = false;                         this.spinbutton_c.Sensitive = false;                           this.spinbutton_ed_madre.Sensitive = false;                         this.spinbutton_g.Sensitive = false;                         this.spinbutton_nomuertos_hijos.Sensitive = false;
			this.spinbutton_edad_gestional.Sensitive = false;            this.spinbutton_edad_madre.Sensitive = false;                  this.spinbutton_edad_padre.Sensitive = false;                       this.spinbutton_menarca.Sensitive = false;                  this.spinbutton_novivos_amaternos.Sensitive = false;
			this.spinbutton_no_embarazo.Sensitive = false;               this.spinbutton_nomuertos_amaternos.Sensitive = false;         this.spinbutton_nomuertos_apaternos.Sensitive = false;              this.spinbutton_nomuertos_hermanos.Sensitive = false;        this.spinbutton_novivos_apaternos.Sensitive = false;
			this.spinbutton_novivos_hermanos.Sensitive = false;          this.spinbutton_novivos_hijos.Sensitive = false;               this.spinbutton_p.Sensitive = false;
			//id_quien_actualizo,fechahora_actualizacion(UPDATE)
		}
		
		// Valida entradas que solo sean numericas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEventactual(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮº/*";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace  && args.Event.Key != Gdk.Key.Delete)
			{
				args.RetVal = true;
			}
		}
			
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

