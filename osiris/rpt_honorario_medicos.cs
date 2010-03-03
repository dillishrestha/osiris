 // created on 21/05/2007 at 05:28 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
// Objeto		: rpt_proc_cobranza.cs
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	public class rpt_honorario_med
	{
		public string connectionString;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public int id_tipopaciente = 0;
		public string nombrebd;
		public string fecha_admision;
		public string fechahora_alta;
		public string nombre_paciente;
		public string telefono_paciente;
		public string doctor;
		public string cirugia;
		public string fecha_nacimiento;
		public string edadpac;
		public string tipo_paciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public bool aplicar_descuento = true;
		public bool aplicar_siempre = false;
		
		public string honorario_med; 
		public string numfactu;
		
		public int filas=635;
		public int contador = 1;
		public int numpage = 1;
						
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
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_honorario_med(int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_, string honorario_med_, string numfactu_)
		{		
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			honorario_med = honorario_med_; 
			numfactu = numfactu_;
			
		
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
			Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "RESUMEN DE FACTURA", 0);
			int         respuesta = dialogo.Run ();
			if (respuesta == (int) PrintButtons.Cancel){
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
					new PrintJobPreview(trabajo, "HONORARIOS MEDICOS").Show();
				break;
			}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistemas Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistemas Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:");
			
			//ContextoImp.MoveTo(484.7, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			//ContextoImp.MoveTo(485, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			   			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("HONORARIOS MEDICOS");
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("HONORARIOS MEDICOS");
							
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(470.5, 755);			ContextoImp.Show("FOLIO DE ATENCION");
			ContextoImp.MoveTo(471, 755);			ContextoImp.Show("FOLIO DE ATENCION");
							
			Gnome.Print.Setfont (ContextoImp, fuente12);
			Gnome.Print.Setrgbcolor(ContextoImp, 150,0,0);
			ContextoImp.MoveTo(520.5,740 );			ContextoImp.Show( folioservicio.ToString());
			ContextoImp.MoveTo(521, 740);			ContextoImp.Show( folioservicio.ToString());
					
			Gnome.Print.Setfont (ContextoImp, fuente36);
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);
			ContextoImp.MoveTo(20, 735);				ContextoImp.Show("____________________________");
									    			
			////////////DATOS GENERALES PACIENTE//////////////////
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(224.5, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			ContextoImp.MoveTo(225, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(444.7, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
								
			ContextoImp.MoveTo(20, 710);			ContextoImp.Show("INGRESO: "+ fecha_admision.ToString());
			ContextoImp.MoveTo(260, 710);			ContextoImp.Show("EGRESO: "+ fechahora_alta.ToString());
			ContextoImp.MoveTo(460, 710);			ContextoImp.Show("Nº FACT: "+numfactu);
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(470.5, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			ContextoImp.MoveTo(470, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
			ContextoImp.MoveTo(20, 690);
			ContextoImp.Show("Direccion: "+dir_pac.ToString());
			
			ContextoImp.MoveTo(20, 670);			ContextoImp.Show("Tel. Pac.: "+telefono_paciente.ToString());
			ContextoImp.MoveTo(450, 670);			ContextoImp.Show("Nº de habitacion:  ");
			
			if(aseguradora.ToString() == "Asegurado")
			{				
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"      	Aseguradora: "+aseguradora.ToString()+"      Poliza: ");
				ContextoImp.MoveTo(20, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"       Aseguradora: "+aseguradora.ToString()+"      Poliza: ");
			}else{
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"              Empresa: "+empresapac.ToString());
				ContextoImp.MoveTo(20, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"              Empresa: "+empresapac.ToString());
			}
			/*if(doctor.ToString() == " " || doctor.ToString() == "")
			{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: ");
				ContextoImp.MoveTo(250, 660);			ContextoImp.Show("Especialidad:");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}else{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: "+doctor.ToString()+"           Especialidad:  ");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}*/
		}
      
		void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20, 645);				ContextoImp.Show("____________________________");
					
			////COLUMNAS
			int filasl = 617;
			for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
			{	
	            int columnas = 17;
				Gnome.Print.Setfont (ContextoImp, fuente36);
				ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
				ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
				filasl-=20;
			}
			//columnas tenues
			//int filasc =640;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
			///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
		}
    
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin)
		{
			Gnome.Print.Setfont (ContextoImp, fuente7);
			ContextoImp.MoveTo(20, filas+8);
			ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
			Gnome.Print.Setfont (ContextoImp, fuente9);
			//LUGAR DE CARGO
			ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
			ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
			Gnome.Print.Setfont (ContextoImp, fuente7);
			ContextoImp.MoveTo(20, filas-2);//633
			ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
			//genera_lineac(ContextoImp, trabajoImpresion);
			filas-=10;
			Gnome.Print.Setfont (ContextoImp, fuente7);
		}
	
		void imprime_subtitulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string medico,decimal montoabono)
		{
			//float varpaso = float.Parse(honorario_med);
			Gnome.Print.Setfont (ContextoImp, fuente7);
			//ContextoImp.MoveTo(29.5, filas);			ContextoImp.Show(grupodelproducto);//625
			//ContextoImp.MoveTo(437.6, filas);			ContextoImp.Show("TOTAL   "+varpaso.ToString("C"));//625
			ContextoImp.MoveTo(29.5, filas);			ContextoImp.Show(medico);//625
			ContextoImp.MoveTo(437.6, filas);			ContextoImp.Show( montoabono.ToString("C"));//625
			filas-=10;		
			Gnome.Print.Setfont (ContextoImp, fuente7);
		}
   
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			filas=635;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de dato s este conectada
			try{
	 			conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	        	        	  
				comando.CommandText ="SELECT nombre1_medico,nombre2_medico,apellido_paterno_medico, "+
	           						"apellido_materno_medico,monto_del_abono "+
									"FROM osiris_his_medicos,osiris_erp_honorarios_medicos "+
									"WHERE "+
									"osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico "+
									"AND osiris_erp_honorarios_medicos.eliminado = false "+
									"AND osiris_erp_honorarios_medicos.folio_de_servicio = '"+folioservicio+"' ;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query honorarios: "+comando.CommandText.ToString());
				ContextoImp.BeginPage("Pagina 1");
			
				////DATOS DE PRODUCTOS
				imprime_encabezado(ContextoImp,trabajoImpresion);
				//genera_tabla(ContextoImp,trabajoImpresion);
				contador+=1;
				imprime_titulo(ContextoImp,trabajoImpresion,"HONORARIOS MEDICOS");
				contador+=1;
				string nombremedico;
				decimal totalhono = 0;
				while (lector.Read())///AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
				{	
					//ContextoImp.BeginPage("Pagina 1");
					nombremedico = (string) lector["nombre1_medico"]+" "+(string) lector["nombre2_medico"]+" "+
								(string) lector["apellido_paterno_medico"]+" "+(string) lector["apellido_materno_medico"];
					imprime_subtitulo(ContextoImp,trabajoImpresion,nombremedico,(decimal) lector["monto_del_abono"]);
					contador+=1;
					totalhono = totalhono + (decimal) lector["monto_del_abono"];
				}
				filas-=10;
				ContextoImp.MoveTo(407, filas);			ContextoImp.Show("TOTAL   "+ totalhono.ToString("C"));//625
				ContextoImp.MoveTo(407.6, filas);		ContextoImp.Show("TOTAL   "+ totalhono.ToString("C"));//625
				ContextoImp.ShowPage();
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	}
	
/////////////////////////////////////////////////////////////////////////////////////////////////////
 	public class rpt_honorario_med_fecha
	{
		public string connectionString;
		public string nombrebd;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
		
		public int contadorhonorario;
		public int columna = 0;
		public int fila = 675;
		public int contador = 1;
		public int numpage = 1;
		
		public string tiporeporte = "NO FACTURADOS";
		public string titulo = "REPORTE DE HONORARIOS MEDICOS";
		
		public string query_fechas = " ";
		public string query_medico = " ";
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
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_honorario_med_fecha(string rango1_,string rango2_,string query_fechas_,string _nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_medico_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_fechas = query_fechas_;
			query_medico = query_medico_;
			tiporeporte = tiporeporte_;
			rango1 = rango1_;
			rango2 = rango2_;
			orden = orden_;
					
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
			Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "REPORTE DE HONORARIOS MEDICOS FECHA", 0);
			int         respuesta = dialogo.Run ();
			if (respuesta == (int) PrintButtons.Cancel){
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
			ComponerPagina2(ctx, trabajo); 
			trabajo.Close();
			switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, "").Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina2 (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			ContextoImp.BeginPage("Pagina 1");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//Verifica que la base de dato s este conectada
			try{
	 			conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	        	        	  
				comando.CommandText ="SELECT "+
				"to_char(osiris_erp_factura_enca.numero_factura,'9999999999') AS numerofactura, to_char(fecha_factura,'dd-MM-yyyy') AS fecha_de_facturacion, to_char(osiris_erp_honorarios_medicos.folio_de_servicio,'999999') AS folioservicio, "+
				"osiris_erp_honorarios_medicos.id_medico AS idmedico, "+  
				"to_char(monto_del_abono,'9999999.99') AS montodelabono, "+
				"to_char(fechahora_abono,'dd-MM-yyyy') AS fechaabono, "+
				"to_char(fecha_pago,'dd-MM-yyyy') AS fechapago, "+ 
				"nombre_medico,descripcion_especialidad, "+
				"osiris_his_paciente.nombre1_paciente || ' ' || "+  
				"osiris_his_paciente.nombre2_paciente || ' ' || "+
				"osiris_his_paciente.apellido_paterno_paciente || ' ' || "+
				"osiris_his_paciente.apellido_materno_paciente AS nombre_paciente "+
				"FROM osiris_erp_honorarios_medicos,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_erp_factura_enca,osiris_erp_cobros_enca,osiris_his_paciente "+
				"WHERE  osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico "+
				"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
				"AND osiris_erp_honorarios_medicos.eliminado = 'false' "+
				"AND osiris_erp_factura_enca.numero_factura = osiris_erp_honorarios_medicos.numero_factura "+
				"AND osiris_erp_cobros_enca.numero_factura =osiris_erp_honorarios_medicos.numero_factura "+
				query_medico+
				"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+query_fechas+" "+orden+";";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				Console.WriteLine(comando.CommandText.ToString());
				ContextoImp.BeginPage("Pagina 1");
			
				////DATOS DE PRODUCTOS
				imprime_encabezado(ContextoImp,trabajoImpresion);
				//genera_tabla(ContextoImp,trabajoImpresion);
				contador = 1;
				
				int iddoctores;
				Gnome.Print.Setfont (ContextoImp, fuente7);
				decimal total_honorario_medico = 0;
				string tomovalor1 = "";
			
				
				
				if (lector.Read())///AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
				{
																		 
					iddoctores = (int) lector["idmedico"];
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
					ContextoImp.MoveTo(24, fila);			ContextoImp.Show((string) lector["folioservicio"]);	      			
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
					ContextoImp.MoveTo(50, fila);			ContextoImp.Show((string) lector["numerofactura"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);		       						
					ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["fecha_de_facturacion"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);	      			
					tomovalor1 = (string) lector["nombre_paciente"];
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(145, fila);			ContextoImp.Show(tomovalor1);
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
					salto_pagina(ContextoImp,trabajoImpresion,contador);					
					
					if ((string) lector ["fechapago"] != "01-01-2000" ){
						ContextoImp.MoveTo(510, fila);			ContextoImp.Show((string) lector["fechapago"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
					}
					
					ContextoImp.MoveTo(425, fila);			ContextoImp.Show((string) decimal.Parse((string) lector["montodelabono"]).ToString("C"));
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					fila-=10;
					total_honorario_medico += decimal.Parse((string) lector["montodelabono"]);
					contador+=1;		       		    		       		    
		       		   		       		
					while (lector.Read())///AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
					{ 
						if (iddoctores != (int) lector["idmedico"]){ 
							salto_pagina(ContextoImp,trabajoImpresion,contador);
							ContextoImp.MoveTo(20, fila+9);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
																"______________________________________________");
							fila-=10;
							contador+=1;
							ContextoImp.MoveTo(340,fila+10);				ContextoImp.Show("T O T A L");
							ContextoImp.MoveTo(425,fila+10);				ContextoImp.Show(total_honorario_medico.ToString("C"));
							
							fila-=10;
							contador+=1;
							iddoctores = (int) lector["idmedico"];	
							total_honorario_medico = 0; 	        			
							salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						total_honorario_medico += decimal.Parse((string) lector["montodelabono"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
						ContextoImp.MoveTo(24, fila);			ContextoImp.Show((string) lector["folioservicio"]);	      			
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       		
						ContextoImp.MoveTo(50, fila);			ContextoImp.Show((string) lector["numerofactura"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);		       						
						ContextoImp.MoveTo(90, fila);			ContextoImp.Show((string) lector["fecha_de_facturacion"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);	      			
						
						tomovalor1 = (string) lector["nombre_paciente"];
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(145, fila);			ContextoImp.Show(tomovalor1);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						ContextoImp.MoveTo(290, fila);			ContextoImp.Show((string) lector["nombre_medico"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						
						if ((string) lector ["fechapago"] != "01-01-2000" ){
						ContextoImp.MoveTo(510, fila);			ContextoImp.Show((string) lector["fechapago"]);
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						}
						ContextoImp.MoveTo(425, fila);			ContextoImp.Show((string) decimal.Parse((string) lector["montodelabono"]).ToString("C"));
						salto_pagina(ContextoImp,trabajoImpresion,contador);
				       		fila-=10;
				       		contador+=1;
					}
					ContextoImp.MoveTo(20, fila+9);			ContextoImp.Show   ("_______________________________________________________________________________________________"+
																"______________________________________________");
					contador+=1;
					fila-=10;
					ContextoImp.MoveTo(340,fila+10);				ContextoImp.Show("T O T A L");
					ContextoImp.MoveTo(425,fila+10);				ContextoImp.Show(total_honorario_medico.ToString("C"));
				    
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	    
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Cambiar la fuente
			//genera_tabla(ContextoImp,trabajoImpresion);
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			
			ContextoImp.MoveTo(240, 720);			ContextoImp.Show("Fecha de impresiom");
			ContextoImp.MoveTo(340, 720.5);			ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			   			   			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(230, 730);			ContextoImp.Show("REPORTE HONORARIOS MEDICOS");
			ContextoImp.MoveTo(230.5, 730);			ContextoImp.Show("REPORTE HONORARIOS MEDICOS");
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(24.5, 690);			ContextoImp.Show("NºATN.");
			ContextoImp.MoveTo(24, 690);			ContextoImp.Show("NºATN.");
	
			ContextoImp.MoveTo(60.5, 690);			ContextoImp.Show("FACT.");
			ContextoImp.MoveTo(60, 690);			ContextoImp.Show("FACT.");

			ContextoImp.MoveTo(90.5, 690);			ContextoImp.Show("FECHA REG.");
			ContextoImp.MoveTo(90, 690);			ContextoImp.Show("FECHA REG.");

		   	ContextoImp.MoveTo(145.5, 690);			ContextoImp.Show("PACIENTE");
			ContextoImp.MoveTo(145, 690);			ContextoImp.Show("PACIENTE");
	
			ContextoImp.MoveTo(290, 690);			ContextoImp.Show("NOM. MEDICO");
			ContextoImp.MoveTo(290.5, 690);			ContextoImp.Show("NOM. MEDICO");

			ContextoImp.MoveTo(405.5, 690);			ContextoImp.Show("MTO HONORARIO");
			ContextoImp.MoveTo(405, 690);			ContextoImp.Show("MTO HONORARIO");
			
			ContextoImp.MoveTo(489.5, 690);			ContextoImp.Show("FECHA DE PAGO");
			ContextoImp.MoveTo(490, 690);			ContextoImp.Show("FECHA DE PAGO");
		}
		
		/*void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20, 700);				ContextoImp.Show("____________________________");
					
			////COLUMNAS
			int filasl = 671;
			for (int i1=0; i1 < 30; i1++)//30 veces para tasmaño carta
			{	
				int columnas = 17;
				Gnome.Print.Setfont (ContextoImp, fuente36);
				ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
				ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
				filasl-=20;
			}
			//columnas tenues
			//int filasc =640;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20,86);		ContextoImp.Show("____________________________");
			///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
		}*/

		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
			//Console.WriteLine("contador antes del if: "+contador_.ToString());
			if (contador_ > 57 )
			{
				numpage +=1;
				ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				imprime_encabezado(ContextoImp,trabajoImpresion);
				//genera_tabla(ContextoImp,trabajoImpresion);
				Gnome.Print.Setfont (ContextoImp, fuente7);
				contador=1;
				fila = 675;
			}
			//Console.WriteLine("contador despues del if: "+contador_.ToString());
		}
	}	    
}
