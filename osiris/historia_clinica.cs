//////////////////////////////////////////////////////////
// historia_clinica.cs created with MonoDevelop
// User: ipena at 09:36 a 16/07/2008
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor   Ing. R. Israel Peña Gonzalez	(Programation & Glade's window)
//
// mejoras Ing. Daniel Olivares Cuevas 25/07/2011 arcangeldoc@gmail.com (Programation & Glade's window)
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using SmartXLS;			// libreria para crear archivo xls es de paga
// libreria creada con el proyecto AODL 1.4 que usa 
using AODL;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Content;
using AODL.Document.Content.Tables;
using AODL.Document;
using AODL.Package;
using AODL.Document.Collections;
using NUnit.Framework;

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
		
		[Widget] Gtk.ComboBox combobox_enfermedad_padre = null;
		[Widget] Gtk.ComboBox combobox_enfermedad_madre = null;
		[Widget] Gtk.ComboBox combobox_enfermedad_hermanos = null;
		[Widget] Gtk.ComboBox combobox_enfermedad_hijos = null;
		[Widget] Gtk.ComboBox combobox_enfermedad_apaternos = null;
		[Widget] Gtk.ComboBox combobox_enfermedad_amaternos = null;
		[Widget] Gtk.Entry entry_otros_ahf;
		      
		//Antecedentes Personales NO Patologicos
		[Widget] Gtk.Entry entry_tipo_casahabit;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.ComboBox combobox_tabaquismo;
		[Widget] Gtk.ComboBox combobox_alcoholismo;
		[Widget] Gtk.ComboBox combobox_drogas;
		
		//Antecedentes Personales Patologicos     
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
		[Widget] Gtk.Frame frame27 = null;
		
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
		
		[Widget] Gtk.ComboBox combobox_cabeza_craneo = null;
		[Widget] Gtk.ComboBox combobox_cabeza_cara = null;
		[Widget] Gtk.ComboBox combobox_nariz_mucosa = null;
		[Widget] Gtk.ComboBox combobox_nariz_cornetes = null;
		[Widget] Gtk.ComboBox combobox_nariz_polipos = null;   // si no
		[Widget] Gtk.ComboBox combobox_nariz_septum = null;
		[Widget] Gtk.ComboBox combobox_cuello_cilindrico = null;
		[Widget] Gtk.ComboBox combobox_cuello_traquea = null;
		[Widget] Gtk.ComboBox combobox_cuello_movilidad = null;
		[Widget] Gtk.ComboBox combobox_cuello_ganglios = null; // si no
		[Widget] Gtk.ComboBox combobox_oido_timpano_der = null;
		[Widget] Gtk.ComboBox combobox_oido_timpano_izq = null;
		[Widget] Gtk.ComboBox combobox_oido_condaud_der = null;
		[Widget] Gtk.ComboBox combobox_oido_condaud_izq = null;
		[Widget] Gtk.ComboBox combobox_oido_pabellon_der = null;
		[Widget] Gtk.ComboBox combobox_oido_pabellon_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_pupilas_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_pupilas_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_cornea_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_cornea_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_idencolor_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_idencolor_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_movolcular_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_movolcular_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_refolcular_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_refolcular_izq = null;
		[Widget] Gtk.ComboBox combobox_ojos_ptregion_der = null;
		[Widget] Gtk.ComboBox combobox_ojos_ptregion_izq = null;		
		[Widget] Gtk.ComboBox combobox_cavoral_encias = null;
		[Widget] Gtk.ComboBox combobox_cavoral_mucosa = null;
		[Widget] Gtk.ComboBox combobox_cavoral_paladar = null;
		[Widget] Gtk.ComboBox combobox_cavoral_lengua = null;
		[Widget] Gtk.ComboBox combobox_cavoral_amigdalas = null;
		[Widget] Gtk.ComboBox combobox_cavoral_dentadura = null;
		[Widget] Gtk.ComboBox combobox_torax_simetria = null;
		[Widget] Gtk.ComboBox combobox_torax_amplexacion = null;
		[Widget] Gtk.ComboBox combobox_torax_ventilacion = null;
		[Widget] Gtk.ComboBox combobox_torax_amplexion = null;
		[Widget] Gtk.ComboBox combobox_torax_murmullo = null;
		[Widget] Gtk.ComboBox combobox_areacard_ritmo = null;
		[Widget] Gtk.ComboBox combobox_areacard_intensidad = null;
		[Widget] Gtk.ComboBox combobox_areacard_ruidos = null;
		[Widget] Gtk.ComboBox combobox_areacard_soplos = null;
		[Widget] Gtk.ComboBox combobox_collumb_conformacion = null;
		[Widget] Gtk.ComboBox combobox_collumb_arcosmov = null;
		[Widget] Gtk.ComboBox combobox_collumb_marcha = null;
		[Widget] Gtk.ComboBox combobox_collumb_puntdoloroso = null;
		
		[Widget] Gtk.ComboBox combobox_abdomen_conformacion = null;
		[Widget] Gtk.ComboBox combobox_abdomen_peristalsis = null;
		[Widget] Gtk.ComboBox combobox_abdomen_visceromegalias = null;
		[Widget] Gtk.ComboBox combobox_abdomen_hernias = null;
		[Widget] Gtk.ComboBox combobox_abdomen_exostisis = null;
		[Widget] Gtk.ComboBox combobox_abdomen_auscultacion = null;
		[Widget] Gtk.Entry entry_abdomen_nt_conformacion = null;
		[Widget] Gtk.Entry entry_abdomen_nt_peristalsis = null;
		[Widget] Gtk.Entry entry_abdomen_nt_visceromagalias = null;
		[Widget] Gtk.Entry entry_abdomen_nt_hernias = null;
		[Widget] Gtk.Entry entry_abdomen_nt_exostisis = null;
		[Widget] Gtk.Entry entry_abdomen_nt_auscultacion = null;
		[Widget] Gtk.Entry entry_abdomen_otros = null;
		
		[Widget] Gtk.ComboBox combobox_mienbrotorax_integr_der = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_integr_izq = null;
		[Widget] Gtk.Entry entry_mienbrotorax_integr_otros = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_mov_der = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_mov_izq = null;
		[Widget] Gtk.Entry entry_mienbrotorax_mov_otros = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_rot_der = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_rot_izq = null;
		[Widget] Gtk.Entry entry_mienbrotorax_rot_otros = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_fza_der = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_fza_izq = null;
		[Widget] Gtk.Entry entry_mienbrotorax_fza_otros = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_plso_der = null;
		[Widget] Gtk.ComboBox combobox_mienbrotorax_plso_izq = null;
		[Widget] Gtk.Entry entry_mienbrotorax_plso_otros = null;
		[Widget] Gtk.Entry entry_mienbrotorax_otros = null;
		
		//combobox_miempelv_inte_der = null;
		//combobox_miempelv_inte_izq = null;
		//entry_miempelv_inte_otros = null;
		[Widget] Gtk.Button button_exportar_xls = null;
		
		[Widget] Gtk.Statusbar statusbar5 = null;
		string fecha_admision;
		string fecha_nacimiento;
		
		string vivomuertopadre = "VIVO"; 
		string vivomuertomadre = "VIVO"; 
		string pntabaquismo = "";
		string pnalcoholismo = "";
		string pndrogas = "";
		string pncronicodegenerativos = "";
		string pnhospitalizaciones = "";
		string pnquirurgicos = "";
		string pnalergicos = "";
		string pntraumaticos = "";
		string pnneurologicos = "";
		string enfermedad_padre = "";
		string enfermedad_madre = "";
		
		//public int tipodellenado;
		bool editando = false;
		
		string nombre_paciente;
		string pid_paciente;
		string edad;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string NomEmpleados;
					
		string connectionString;
		string nombrebd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;	
		class_conexion conexion_a_DB = new class_conexion();
		
		string[] args_causa_de_muerte = {"","DIABETES","HIPERTENSION","ENF. DEL CORAZON","ENF. DE PULMONES","CANCER O LEUCEMIA","EMBOLIA","ENF. MENTALES"};
		string[] args_vivo_muerto = {"","VIVO","MUERTO"};
		string[] args_si_no = {"","SI","NO"};
		string[] args_normal_anormal = {"","NORMAL","ANORMAL"};
		string[] args_conformacion = {"","PLANO","GLOBOSO","CONCAVO"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8};
		
		public historia_clinica(string nombre_paciente_,string pid_paciente_,string edad_,string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string fecha_admision_,string fecha_nacimiento_ ) 
		{
			nombre_paciente = nombre_paciente_;
			pid_paciente = pid_paciente_;
			edad = edad_;
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			fecha_admision = fecha_admision_;
			fecha_nacimiento = fecha_nacimiento_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
            Glade.XML gxml = new Glade.XML (null, "urgencia.glade", "historia_clinica_del_paciente", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			historia_clinica_del_paciente.Show();			
			editando = false;
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//Guarda datos de la reservacion
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			// exportar a xls
			button_exportar_xls.Clicked += new EventHandler(on_button_exportar_xls_clicked);
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
		
			//llenado_combobox(0,"",combobox_enfermedad_padre,args_causa_de_muerte,args_id_causa_de_muerte);
			llenado_combobox(0,"",combobox_enfermedad_padre,args_causa_de_muerte,args_id_array);			
			llenado_combobox(0,"",combobox_enfermedad_madre,args_causa_de_muerte,args_id_array);
			llenado_combobox(0,"",combobox_enfermedad_hermanos,args_causa_de_muerte,args_id_array);
			llenado_combobox(0,"",combobox_enfermedad_hijos,args_causa_de_muerte,args_id_array);
			llenado_combobox(0,"",combobox_enfermedad_apaternos,args_causa_de_muerte,args_id_array);
			llenado_combobox(0,"",combobox_enfermedad_amaternos,args_causa_de_muerte,args_id_array);
			
			//entrys id enfermedad (sirve para ingresar el id de la enfermedad y solo se pueda con numeros)
			/*this.entry_idenf_padre.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_madre.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_hermanos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_hijos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_apaternos.KeyPressEvent += onKeyPressEventactual;
		    this.entry_idenf_amaternos.KeyPressEvent += onKeyPressEventactual; */			
			// Buscar Enfermedades/id Enfermedades
			button_buscar1.Sensitive = false;
			button_buscar2.Sensitive = false;
			button_buscar3.Sensitive = false;
			button_buscar4.Sensitive = false;
			button_buscar5.Sensitive = false;
			button_buscar6.Sensitive = false;
			entry_idenf_padre.Sensitive = false;
			entry_idenf_madre.Sensitive = false;
			entry_idenf_hermanos.Sensitive = false;
			entry_idenf_hijos.Sensitive = false;
			entry_idenf_apaternos.Sensitive = false;
			entry_idenf_amaternos.Sensitive = false;
			frame27.Sensitive = false;
			// (trayendo las variables del programa urgencias: nombre, pid y edad)
			entry_nombre_paciente.Text = nombre_paciente_;
			entry_pid_paciente.Text = pid_paciente_.Trim();
			entry_edad_paciente.Text = edad_.Trim();					
			//llenado de los ComboBox Antecedentes Heredo Familiar:
			llenado_combobox(1,"VIVO",combobox_vivomuerto_padre,args_vivo_muerto,args_id_array);
			llenado_combobox(1,"VIVO",combobox_vivomuerto_madre,args_vivo_muerto,args_id_array);			
			// llenado de Combobox positivo Negativo			
			llenado_combobox(1,"SI",combobox_tabaquismo,args_si_no,args_id_array);
			llenado_combobox(1,"SI",combobox_alcoholismo,args_si_no,args_id_array);
			llenado_combobox(1,"NO",combobox_drogas,args_si_no,args_id_array);			
			// llenado combobox exploracion fisica
			llenado_combobox(1,"NORMAL",combobox_cabeza_craneo,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cabeza_cara,args_normal_anormal,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_nariz_mucosa,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_nariz_cornetes,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_nariz_polipos,args_si_no,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_nariz_septum,args_normal_anormal,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_cuello_cilindrico,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cuello_traquea,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cuello_movilidad,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_cuello_ganglios,args_si_no,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_oido_timpano_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_oido_timpano_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_oido_condaud_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_oido_condaud_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_oido_pabellon_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_oido_pabellon_izq,args_normal_anormal,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_ojos_pupilas_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_pupilas_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_cornea_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_cornea_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_idencolor_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_idencolor_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_movolcular_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_movolcular_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_refolcular_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_refolcular_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_ptregion_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_ojos_ptregion_izq,args_normal_anormal,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_cavoral_encias,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cavoral_mucosa,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cavoral_paladar,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cavoral_lengua,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cavoral_amigdalas,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_cavoral_dentadura,args_normal_anormal,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_torax_simetria,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_torax_amplexacion,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_torax_ventilacion,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_torax_amplexion,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_torax_murmullo,args_si_no,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_areacard_ritmo,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_areacard_intensidad,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_areacard_ruidos,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_areacard_soplos,args_si_no,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_collumb_conformacion,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_collumb_arcosmov,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_collumb_marcha,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_collumb_puntdoloroso,args_si_no,args_id_array);
			llenado_combobox(1,"NO",combobox_collumb_puntdoloroso,args_si_no,args_id_array);
			
			llenado_combobox(1,"GLOBOSO",combobox_abdomen_conformacion,args_conformacion,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_abdomen_peristalsis,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NO",combobox_abdomen_visceromegalias,args_si_no,args_id_array);
			llenado_combobox(1,"NO",combobox_abdomen_hernias,args_si_no,args_id_array);
			llenado_combobox(1,"NO",combobox_abdomen_exostisis,args_si_no,args_id_array);
			llenado_combobox(1,"NO",combobox_abdomen_auscultacion,args_si_no,args_id_array);
			
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_integr_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_integr_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_mov_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_mov_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_rot_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_rot_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_fza_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_fza_izq,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_plso_der,args_normal_anormal,args_id_array);
			llenado_combobox(1,"NORMAL",combobox_mienbrotorax_plso_izq,args_normal_anormal,args_id_array);
			
			
			//SpinButtons Antecedentes Heredo Familiar:
			spinbutton_edad_madre.SetRange(0, 150);
			spinbutton_edad_padre.SetRange(0, 150);
			spinbutton_novivos_hermanos.SetRange(0, 50);
			spinbutton_novivos_hijos.SetRange(0, 30);
			spinbutton_novivos_amaternos.SetRange(0, 2);
			spinbutton_novivos_apaternos.SetRange(0, 2);
			spinbutton_nomuertos_hermanos.SetRange(0, 50);			
			spinbutton_nomuertos_hijos.SetRange(0, 30);
			spinbutton_nomuertos_apaternos.SetRange(0, 2);
			spinbutton_nomuertos_apaternos.ValueChanged += HandleSpinbutton_nomuertos_apaternosValueChanged;
			spinbutton_nomuertos_amaternos.SetRange(0, 2);			
			//SpinButtons Antecedentes Gineco Obsterricios:
			spinbutton_menarca.SetRange(0, 150);
			spinbutton_a.SetRange(0, 50);
			spinbutton_c.SetRange(0, 50);
			spinbutton_g.SetRange(0, 100);
			spinbutton_p.SetRange(0, 50);			
			//SpinButtons Historia Clinica Pediatrica:
			spinbutton_ed_madre.SetRange(0, 150);
			spinbutton_edad_gestional.SetRange(0, 150);
			spinbutton_no_embarazo.SetRange(0, 50);

			muestra_datos_paciente();
		}

		void HandleSpinbutton_nomuertos_apaternosValueChanged (object sender, EventArgs e)
		{
			Console.WriteLine("prueba"+spinbutton_novivos_apaternos.Text.ToString());
			if(int.Parse(spinbutton_novivos_apaternos.Text.ToString()) > 1 ){
				spinbutton_novivos_apaternos.Text = "0";
			}
		}
		
		void on_button_exportar_xls_clicked(object sender, EventArgs args)
		{
			export_xls_sigma();		
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
					                  "FROM osiris_his_paciente,osiris_his_hc_hfamiliar "+
						              "WHERE osiris_his_paciente.historia_clinica = 'true' "+
						              "AND osiris_his_hc_hfamiliar.pid_paciente = '"+this.entry_pid_paciente.Text+"';";
				
			    Console.WriteLine(comando1.CommandText.ToString());
				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
					   
				if(lector1.Read()){
				button_guardar.Sensitive = false;                       this.entry_nombre_paciente.Sensitive = false;			        this.entry_pid_paciente.Sensitive = false;        this.entry_edad_paciente.Sensitive = false;
				button_editar_his_clin.Sensitive = true;                this.button_imprimir.Sensitive = true;                         this.button_imprimir_p2.Sensitive = true;
			//Antecedentes Heredo Familiar:     
				//this.combobox_enfermedad_padre.Sensitive = false;               this.entry_enfermedad_madre.Sensitive = false;                 this.entry_enfermedad_amaternos.Sensitive = false; 
				//this.entry_enfermedad_hermanos.Sensitive = false;            this.entry_enfermedad_hijos.Sensitive = false;                 this.entry_enfermedad_apaternos.Sensitive = false;             
				//this.entry_otros_ahf.Sensitive = false; 
			 //Antecedentes Personales NO Patologicos:
				//this.entry_tipo_casahabit.Sensitive = false;                 this.entry_observaciones.Sensitive = false;           
			 //Antecedentes Personales Patologicos:
				entry_otros_app.Sensitive = false;                      this.entry_medicamentos.Sensitive = false;
			    ////entrys nuevos:
				entry_cronicodegenerativo.Sensitive = false;
		        entry_alergicos.Sensitive = false;
		        entry_obs_hospi.Sensitive = false;
		        entry_traumaticos.Sensitive = false;
		        entry_quirurgicos.Sensitive = false;
		        entry_neurologicos.Sensitive = false;
			//Pagina 2: Antecedentes Gineco Obstetricios:                           
				entry_ivsa.Sensitive = false;                           this.entry_ritmo.Sensitive = false;                            this.entry_fum.Sensitive = false;                  this.entry_fpp.Sensitive = false;                           
				entry_pap.Sensitive = false;                            this.entry_contracepcion.Sensitive = false;                    this.entry_otros_ago.Sensitive = false;            this.entry_fup.Sensitive = false;
			 //Historia Clinica Pediatrica:                     
				entry_perinatales.Sensitive = false;                    this.entry_peso.Sensitive = false;                             this.entry_patologicos.Sensitive = false;          this.entry_alumbramiento.Sensitive = false; 
				entry_infecciones.Sensitive = false;                    this.entry_alergias.Sensitive = false;                         this.entry_hospitalizaciones.Sensitive = false;    this.entry_traumatismos.Sensitive = false; 
				entry_cirugias.Sensitive = false;                       this.entry_inmunizaciones.Sensitive = false;                   this.entry_des_psicomotor.Sensitive = false;       this.entry_otros_hcp.Sensitive = false; 
			 //Pagina 3: Motivo de Ingreso:
				entry_motivoingreso.Sensitive = false;                  this.entry_padecimientoactual.Sensitive = false;               this.entry_ta.Sensitive = false;                   this.entry_fc.Sensitive = false; 
				entry_fr.Sensitive = false;                             this.entry_temp.Sensitive = false;                             this.entry_pso.Sensitive = false;                  this.entry_talla.Sensitive = false; 
				entry_habitus_ext.Sensitive = false;                    this.entry_cabeza.Sensitive = false;                           this.entry_cuello.Sensitive = false;               this.entry_torax.Sensitive = false; 
				entry_abdomen.Sensitive = false;                        this.entry_extremidades.Sensitive = false;                     this.entry_genitourinario.Sensitive = false;       this.entry_neurologico.Sensitive = false; 
				entry_diagnosticos.Sensitive = false;                   this.entry_plan_diag.Sensitive = false;                        this.entry_nombre_plan_diag.Sensitive = false; 			
             //combobox:			
				combobox_vivomuerto_padre.Sensitive = false;                 combobox_vivomuerto_madre.Sensitive = false;       
				combobox_alcoholismo.Sensitive = false;                      combobox_tabaquismo.Sensitive = false;                         combobox_drogas.Sensitive = false;
				 
			 //spinbuttons:
				spinbutton_a.Sensitive = false;                         this.spinbutton_c.Sensitive = false;                           this.spinbutton_ed_madre.Sensitive = false;                         this.spinbutton_g.Sensitive = false;                         this.spinbutton_nomuertos_hijos.Sensitive = false;
				spinbutton_edad_gestional.Sensitive = false;            this.spinbutton_edad_madre.Sensitive = false;                  this.spinbutton_edad_padre.Sensitive = false;                       this.spinbutton_menarca.Sensitive = false;                  this.spinbutton_novivos_amaternos.Sensitive = false;
				spinbutton_no_embarazo.Sensitive = false;               this.spinbutton_nomuertos_amaternos.Sensitive = false;         this.spinbutton_nomuertos_apaternos.Sensitive = false;              this.spinbutton_nomuertos_hermanos.Sensitive = false;        this.spinbutton_novivos_apaternos.Sensitive = false;
				spinbutton_novivos_hermanos.Sensitive = false;          this.spinbutton_novivos_hijos.Sensitive = false;               this.spinbutton_p.Sensitive = false;
				//id_quien_actualizo,fechahora_actualizacion(UPDATE)
				
				//entrys:
				//entry_enfermedad_padre.Text = (string) lector1["descripcion_enfermedad_padre"];
				//entry_enfermedad_madre.Text = (string) lector1["descripcion_enfermedad_madre"].ToString().Trim();
				//entry_enfermedad_hermanos.Text = (string) lector1["descripcion_enfermedad_hermanos"].ToString().Trim();
				//entry_enfermedad_hijos.Text = (string) lector1["descripcion_enfermedad_hijos"].ToString().Trim();
				//entry_enfermedad_apaternos.Text = (string) lector1["descripcion_enfermedad_apaternos"].ToString().Trim();
				//entry_enfermedad_amaternos.Text = (string) lector1["descripcion_enfermedad_amaternos"].ToString().Trim();
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
				entry_cronicodegenerativo.Text = (string) lector1["observaciones_cdegenerativos"].ToString();
		        entry_alergicos.Text = (string) lector1["observaciones_alergicos"].ToString();
		       	entry_obs_hospi.Text = (string) lector1["observaciones_hosp"].ToString();
		        entry_traumaticos.Text = (string) lector1["observaciones_traumaticos"].ToString();
		        entry_quirurgicos.Text = (string) lector1["observaciones_quirur"].ToString();
		       	entry_neurologicos.Text = (string) lector1["observaciones_neurolog"].ToString();
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
					llenado_combobox(1,vivomuertopadre,combobox_vivomuerto_padre,args_vivo_muerto,args_id_array);
					llenado_combobox(1,vivomuertomadre,combobox_vivomuerto_madre,args_vivo_muerto,args_id_array);
					//llenado de los ComboBox Antecedentes Personales NO Patologicos 
					llenado_combobox(1,pntabaquismo,combobox_tabaquismo,args_si_no,args_id_array);
					llenado_combobox(1,pnalcoholismo,combobox_alcoholismo,args_si_no,args_id_array);
					llenado_combobox(1,pnalcoholismo,combobox_drogas,args_si_no,args_id_array);
					//llenado de los ComboBox Antecedentes Personales Patologicos
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion1.Close ();
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string[] args_array,int[] args_id_array)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}
			for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
				store.AppendValues (args_array[colum_field],args_id_array[colum_field]);
			}
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}
		
		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_tabaquismo":
					pntabaquismo = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_alcoholismo":
					pnalcoholismo = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_drogas":
					pndrogas = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_vivomuerto_padre":
					vivomuertopadre = (string) onComboBoxChanged.Model.GetValue(iter,0);
					//Console.WriteLine("vivomuertopadre = "+vivomuertopadre);
					break;
				case "combobox_vivomuerto_madre":
					vivomuertomadre = (string) onComboBoxChanged.Model.GetValue(iter,0);
					//Console.WriteLine("vivomuertomadre = "+vivomuertomadre);
					break;
				case "combobox_enfermedad_padre":
					enfermedad_padre = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_enfermedad_madre":
					enfermedad_madre = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				}
			}
		}
		
		///////////////////////GUARDAR O GRABAR HISTORIA CLINICA DEL PACIENTE///////////////////////////////////////////////////////////////
		void on_button_guardar_clicked(object sender, EventArgs args)
		{         //Pagina 1:
			if( this.entry_pid_paciente.Text == ""  ||                 this.entry_nombre_paciente.Text == "" ||                      this.entry_edad_paciente.Text == ""   ||           
			    Convert.ToString(combobox_vivomuerto_padre) == "" ||    Convert.ToString(combobox_vivomuerto_madre) == "" || 
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
					comando.CommandText = "UPDATE osiris_his_historia_clinica "+
						    "SET " +
							"id_quien_actualizo = '" +LoginEmpleado+"', "+
							"fechahora_actualizacion = '" +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
							//"descripcion_enfermedad_padre = '" +this.entry_enfermedad_padre.Text+"', "+
							//"descripcion_enfermedad_madre = '" +this.entry_enfermedad_madre.Text+"', "+
							//"descripcion_enfermedad_hermanos = '" +this.entry_enfermedad_hermanos.Text+"', "+
							//"descripcion_enfermedad_hijos = '" +this.entry_enfermedad_hijos.Text+"', "+
							//"descripcion_enfermedad_apaternos = '" +this.entry_enfermedad_apaternos.Text+"', "+
							//"descripcion_enfermedad_amaternos = '" +this.entry_enfermedad_amaternos.Text+"', "+
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
					comando.CommandText = "UPDATE osiris_his_paciente "+
						                  "SET historia_clinica = 'true' "+ 
							              "WHERE pid_paciente = '"+this.entry_pid_paciente.Text+"'; ";
					Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
					comando.Dispose();
					comando.CommandText = "INSERT INTO osiris_his_historia_clinica ( "+
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
											// "'"+this.entry_enfermedad_padre.Text+"', "+              //4  
											// "'"+this.entry_enfermedad_madre.Text+"', "+              //5
											// "'"+this.entry_enfermedad_hermanos.Text+"', "+           //6  
											 //"'"+this.entry_enfermedad_hijos.Text+"', "+              //7
											 //"'"+this.entry_enfermedad_apaternos.Text+"', "+          //8
											 //"'"+this.entry_enfermedad_amaternos.Text+"', "+          //9   
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
			   this.entry_medicamentos.Text == "" ||
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
			   entry_otros_app.Text == "" ||
			   entry_medicamentos.Text == "" ||
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
			 //this.entry_enfermedad_padre.Sensitive = true;               this.entry_enfermedad_madre.Sensitive = true;                 this.entry_enfermedad_amaternos.Sensitive = true; 
			 //this.entry_enfermedad_hermanos.Sensitive = true;            this.entry_enfermedad_hijos.Sensitive = true;                 this.entry_enfermedad_apaternos.Sensitive = true;             
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
			//this.entry_enfermedad_padre.Sensitive = false;               this.entry_enfermedad_madre.Sensitive = false;                 this.entry_enfermedad_amaternos.Sensitive = false; 
			//this.entry_enfermedad_hermanos.Sensitive = false;            this.entry_enfermedad_hijos.Sensitive = false;                 this.entry_enfermedad_apaternos.Sensitive = false;             
			//this.entry_otros_ahf.Sensitive = false; 
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
	
	
		public void export_xls_sigma()
		{
			WorkBook book = new WorkBook();
		
            try{                
				book.read("template_de_historia_clinica_institucional.xls");
				book.Sheet = 0;
			
				book.setText(1, 1, "PRACTIMED");
				book.setText(3, 1, entry_nombre_paciente.Text.Trim().ToUpper());
				if (vivomuertopadre == "VIVO"){
					book.setText(12, 1, "X");
				}else{
					book.setText(12, 2, "X");
				}
				switch (enfermedad_padre){
				case "DIABETES":					book.setText(12, 3, "X");
					break;
				case "HIPERTENSION":				book.setText(12, 5, "X");
					break;
				case "ENF. DEL CORAZON":			book.setText(12, 7, "X");
					break;
				case "ENF. DE PULMONES":			book.setText(12, 9, "X");
					break;
				case "CANCER O LEUCEMIA":			book.setText(12, 11, "X");
					break;
				case "EMBOLIA":						book.setText(12, 13, "X");
					break;
				case "ENF. MENTALES":				book.setText(12, 15, "X");
					break;
				}
				
				if(vivomuertomadre == "VIVO"){
					book.setText(13, 1, "X");
				}else{
					book.setText(13, 2, "X");
				}
				switch (enfermedad_madre){
				case "DIABETES":					book.setText(13, 3, "X");
					break;
				case "HIPERTENSION":				book.setText(13, 5, "X");
					break;
				case "ENF. DEL CORAZON":			book.setText(13, 7, "X");
					break;
				case "ENF. DE PULMONES":			book.setText(13, 9, "X");
					break;
				case "CANCER O LEUCEMIA":			book.setText(13, 11, "X");
					break;
				case "EMBOLIA":						book.setText(13, 13, "X");
					break;
				case "ENF. MENTALES":				book.setText(13, 15, "X");
					break;
				}
				
				
				// book.setSheetName(1, "Ex Ingreso IPAS");	// Coloca el nombre de la hoja
                book.Sheet = 1;			// numero de hoja actual comienza en 0
				Console.WriteLine(book.Sheet.ToString());
				//book.NumSheets = 1;
		    	book.setText(6, 0, "sheet2");
                
                book.Sheet = 2;
				Console.WriteLine(book.Sheet.ToString());
                book.setText(5, 1, "sheet3");
                book.setText(6, 1, "sheet3");


            //        book.setText(1, 4, "Mar");
		    //        book.setText(1, 5, "Apr");
		    //        book.setText(2, 1, "Practimed 1");
		    //        book.setText(3, 1, "Practimed 2");
		    //        book.setText(4, 1, "Practimed 3");
		    //        book.setText(5, 1, "Practimed 4");
		    //        book.setText(6, 1, "Practimed 5");
		    //        book.setText(7, 1, "Total");
		    //        book.setFormula(2, 2, "RAND()*100");
		    //        book.setSelection(2, 2, 2, 5);
		    //        book.editCopyRight();
		    //        book.setSelection(2, 2, 6, 5);
		    //        book.editCopyDown();
		    //        book.setFormula(7, 2, "SUM(C3:C7)");
		    //        book.setSelection("C8:F8");
		    //        book.editCopyRight();
			
				book.Sheet = 0;
				book.write("formato_historia_clinica_sigma.xls");
			 
            }catch (System.Exception ex){
                Console.Error.WriteLine(ex);
            }

            // SE USA PARA ABRIR LA APLICACION
            System.Diagnostics.Process.Start("formato_historia_clinica_sigma.xls");
		}
	}
}