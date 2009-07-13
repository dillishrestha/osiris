// created on 18/06/2007 at 09:51 a
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
// Programa		: rpt_hoja_de_cargos.cs
// Proposito	: Impresion de la hoja de cargos 
// Objeto		: rpt_hoja_de_cargos.cs

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
	public class hoja_cargos
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public string fecha_admision;
		public string fechahora_alta;
		public string nombre_paciente;
		public string telefono_paciente;
		public string doctor;
		public string cirugia;
		public string fecha_nacimiento;
		public string edadpac;
		public int id_tipopaciente = 0;
		public string tipo_paciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public bool apl_desc_siempre = true;
		public bool apl_desc;
		public string area;
		public string LoginEmpleado;
		public string NomEmpleado;
		public string AppEmpleado;
		public string ApmEmpleado;
				
		public int idadmision_ = 0;
		public int idproducto = 0;
		public string datos = "";
		public string query_rango;
		public int tipointernamiento = 10;
		
		public int filas=690;//635
		public int contador = 1;
		public int contadorprod = 0;
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
		
		public hoja_cargos ( int PidPaciente_ , int folioservicio_,string _nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string area_,string NomEmpleado_,string AppEmpleado_,
						string ApmEmpleado_,string LoginEmpleado_, string query_, int tipointernamiento_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			id_tipopaciente = idtipopaciente_;
			tipo_paciente = entry_tipo_paciente_;//
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			query_rango = query_;//
			tipointernamiento = tipointernamiento_;//
			area = area_;							// Recibe el parametro del modulo que manda a imprimir (UCIA, Hospital, Urgencia, etc)
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "Hoja de Registro", 0);
        	int         respuesta = dialogo.Run ();
        	if (respuesta == (int) PrintButtons.Cancel) 	{
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:  trabajo.Print (); 
                break;
                case (int) PrintButtons.Preview: new PrintJobPreview(trabajo, "HOJA DE CARGOS").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			
			  			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("HOJA REGISTROS DE "+area);
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("HOJA REGISTROS DE "+area);
							
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
			ContextoImp.MoveTo(20.7, 720);			ContextoImp.Show("Fecha "+DateTime.Now.ToString("yyyy-MM-dd HH24:mm:ss"));
			ContextoImp.MoveTo(20, 720);			ContextoImp.Show("Fecha "+DateTime.Now.ToString("yyyy-MM-dd HH24:mm:ss"));
			
			ContextoImp.MoveTo(444.7, 720);			ContextoImp.Show("Pagina "+numpage.ToString());
			ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString());
					
			ContextoImp.MoveTo(20, 710);			ContextoImp.Show("INGRESO: "+ fecha_admision.ToString());
			ContextoImp.MoveTo(460, 710);			ContextoImp.Show("EGRESO: "+ fechahora_alta.ToString());
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(470.5, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			ContextoImp.MoveTo(471, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
			ContextoImp.MoveTo(20, 690);			ContextoImp.Show("Direccion: "+dir_pac.ToString());
			
			ContextoImp.MoveTo(20, 670);			ContextoImp.Show("Tel. Pac.: "+telefono_paciente.ToString());
			ContextoImp.MoveTo(450, 670);			ContextoImp.Show("Nº de habitacion:  ");
			
			if((string) aseguradora == "Asegurado")
			{				
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"      	Aseguradora: "+aseguradora.ToString()+"      Poliza: ");
				ContextoImp.MoveTo(20, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"       Aseguradora: "+aseguradora.ToString()+"      Poliza: ");
			}else{
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"              Empresa: "+empresapac.ToString());
				ContextoImp.MoveTo(20, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente.ToString()+"              Empresa: "+empresapac.ToString());
		 	}
		 	
			if(doctor.ToString() == " " || doctor.ToString() == "")
			{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: ");
				ContextoImp.MoveTo(250, 660);			ContextoImp.Show("Especialidad:");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico : "+cirugia.ToString());
			}else{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: "+doctor.ToString()+"           Especialidad:  ");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}
	  }
      
      void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
      {
      	//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20, 645);				ContextoImp.Show("____________________________");
				
		////COLUMNAS
		int filasl = 617;
		//int filas2 = 635;
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		/*for(int i2=0; i2 < 57; i2++)//30 veces para tasmaño carta
		{	
			genera_lineac(ContextoImp,trabajoImpresion,filas2);
			filas2-=10;
		}*/
		//columnas tenues
		//int filasc =640;
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    /*
    void genera_lineac(PrintContext ContextoImp, PrintJob trabajoImpresion,int filasl)
	{
		Gnome.Print.Setfont (ContextoImp, fuente11);
		ContextoImp.MoveTo(75, filasl);						ContextoImp.Show("|");//52
		ContextoImp.MoveTo(104, filasl);					ContextoImp.Show("|");//104
		ContextoImp.MoveTo(475, filasl);					ContextoImp.Show("|");
		ContextoImp.MoveTo(523, filasl);					ContextoImp.Show("|");
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
    */
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(190.5, filas);			ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
		ContextoImp.MoveTo(191, filas);				ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		filas-=10;
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
	
	void imprime_subtitulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string tipoproducto,string fechacreacion)
	{
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(24.5, filas);			ContextoImp.Show("CLAVE.");//64.5625
		ContextoImp.MoveTo(25, filas);				ContextoImp.Show("CLAVE.");//65,625
		ContextoImp.MoveTo(79.5, filas);			ContextoImp.Show("CANT.");//24.5,625
		ContextoImp.MoveTo(80, filas);				ContextoImp.Show("CANT.");//25,625
		ContextoImp.MoveTo(107.5, filas);			ContextoImp.Show("HORA");
		ContextoImp.MoveTo(108, filas);				ContextoImp.Show("HORA");
		ContextoImp.MoveTo(145.5, filas);			ContextoImp.Show(tipoproducto);//625
		ContextoImp.MoveTo(146, filas);				ContextoImp.Show(tipoproducto);//625
		ContextoImp.MoveTo(279.6,filas);			ContextoImp.Show(fechacreacion);
		ContextoImp.MoveTo(280,filas);				ContextoImp.Show(fechacreacion);
		ContextoImp.MoveTo(492.6, filas);			ContextoImp.Show("V.B.");//625
		ContextoImp.MoveTo(493, filas);				ContextoImp.Show("V.B.");//625
		ContextoImp.MoveTo(544.6, filas);			ContextoImp.Show("CAJA");//625
		ContextoImp.MoveTo(545, filas);				ContextoImp.Show("CAJA");//625
		filas-=10;
		Gnome.Print.Setfont (ContextoImp, fuente7);
    }
   
	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
	{
		if (contador_ >= 57 )
        {
        	numpage +=1;
        	ContextoImp.ShowPage();
			ContextoImp.BeginPage("Pagina N");
			imprime_encabezado(ContextoImp,trabajoImpresion);
     		genera_tabla(ContextoImp,trabajoImpresion);
     		Gnome.Print.Setfont (ContextoImp, fuente7);
     		contador=1;
        	filas=635;
        }
    }
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString+nombrebd);
        // Verifica que la base de datos este conectada
        //Querys
        string ampm = " AM. ";
		string query_todo = "SELECT "+
					"hscmty_erp_cobros_deta.folio_de_servicio,hscmty_erp_cobros_deta.pid_paciente, "+ 
					"hscmty_productos.id_grupo_producto,descripcion_producto, "+
					"hscmty_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"hscmty_grupo_producto.descripcion_grupo_producto, "+
					"  "+
					"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
					"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'HH:mi') AS horacreacion,  "+
					"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'HH24') AS tiempocreacion,  "+
					"to_char(hscmty_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
					"to_char(hscmty_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada  "+
					"FROM "+ 
					"hscmty_erp_cobros_deta,hscmty_his_tipo_admisiones,hscmty_productos,hscmty_grupo_producto "+
					"WHERE "+
					"hscmty_erp_cobros_deta.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
					"AND hscmty_erp_cobros_deta.id_tipo_admisiones = '"+tipointernamiento.ToString()+"' "+
					"AND hscmty_erp_cobros_deta.id_producto = hscmty_productos.id_producto  "+ 
					"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
					"AND hscmty_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
		        	"AND id_empleado = '"+LoginEmpleado+"' "+
		        	"AND hscmty_erp_cobros_deta.eliminado = 'false' ";
		try 
        {
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	comando.CommandText =query_todo+query_rango+ "ORDER BY  to_char(hscmty_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd 24HH:mm') ASC, hscmty_productos.id_grupo_producto,hscmty_erp_cobros_deta.id_secuencia; ";
			NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
			
			filas=635;
        	if (lector.Read())
        	{	
        		//VARIABLES
        		datos = (string) lector["descripcion_producto"];
				/////DATOS DE PRODUCTOS
      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	genera_tabla(ContextoImp,trabajoImpresion);
     		   	imprime_titulo(ContextoImp,trabajoImpresion);
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
       		 	contador+=1;
       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	
        		//DATOS TABLA
        		if(int.Parse((string) lector["tiempocreacion"]) >= 12){
        			ampm = " PM. "; 
				}else{
					ampm = " AM. "; 
				}
				
				if(datos.Length > 64) { datos = datos.Substring(0,64); }
				
        		Gnome.Print.Setfont (ContextoImp, fuente7);
				ContextoImp.MoveTo(22, filas);			ContextoImp.Show((string) lector["idproducto"]);//55
				ContextoImp.MoveTo(80, filas);			ContextoImp.Show((string) lector["cantidadaplicada"]);//22
				ContextoImp.MoveTo(110, filas);			ContextoImp.Show((string) lector["horacreacion"]+ampm);
				ContextoImp.MoveTo(148, filas);			ContextoImp.Show(datos.ToString());
				ContextoImp.MoveTo(480, filas);			ContextoImp.Show("_______");
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show("_______");
				Gnome.Print.Setfont (ContextoImp, fuente7);
				contadorprod+=1;
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				//idadmision_ = (int) lector["idadmisiones"];
        		idproducto = (int) lector["id_grupo_producto"];
				
				while (lector.Read())
        		{
        			if (contador==1){
        				imprime_encabezado(ContextoImp,trabajoImpresion);
        				imprime_titulo(ContextoImp,trabajoImpresion);
		        		contador+=1;		salto_pagina(ContextoImp,trabajoImpresion,contador);
		       			imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
		       			contador+=1;	 	salto_pagina(ContextoImp,trabajoImpresion,contador);
        			 	genera_tabla(ContextoImp,trabajoImpresion); 
        			 }
        			
        			///VARIABLES
					datos = (string) lector["descripcion_producto"];
					//DATOS TABLA
        			if (idproducto != (int) lector["id_grupo_producto"])
        			{
        				idproducto = (int) lector["id_grupo_producto"];
        			   	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
        			   	contador+=1;
        			   	salto_pagina(ContextoImp,trabajoImpresion,contador);
        			}
					if(int.Parse((string) lector["tiempocreacion"]) >= 12){
						ampm = " PM. "; 
					}else{
						ampm = " AM. ";
					}
					
					if(datos.Length > 64) { datos = datos.Substring(0,64); }
					
	        		Gnome.Print.Setfont (ContextoImp, fuente7);
					ContextoImp.MoveTo(22, filas);			ContextoImp.Show((string) lector["idproducto"]);//55
					ContextoImp.MoveTo(80, filas);			ContextoImp.Show((string) lector["cantidadaplicada"]);//22
					ContextoImp.MoveTo(110, filas);			ContextoImp.Show((string) lector["horacreacion"]+ampm);
					ContextoImp.MoveTo(148, filas);			ContextoImp.Show(datos.ToString());
					ContextoImp.MoveTo(480, filas);			ContextoImp.Show("_______");
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show("_______");
					Gnome.Print.Setfont (ContextoImp, fuente7);
					contadorprod+=1;
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
				}//termino de ciclo*/
				imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	genera_tabla(ContextoImp,trabajoImpresion);
     		   	Gnome.Print.Setfont (ContextoImp, fuente8);
     		   	ContextoImp.MoveTo(25, filas);		ContextoImp.Show("Total de productos aplicados: "+contadorprod.ToString());
     		   	ContextoImp.MoveTo(25.5, filas);	ContextoImp.Show("Total de productos aplicados: "+contadorprod.ToString());
     		   	contador+=1;
    			filas-=10;
    			salto_pagina(ContextoImp,trabajoImpresion,contador);
    			contador+=1;
    			filas-=10;
    			salto_pagina(ContextoImp,trabajoImpresion,contador);
    			Gnome.Print.Setfont (ContextoImp, fuente7);
    			ContextoImp.MoveTo(80, filas) ;			ContextoImp.Show("_________________________________________"); 
    			ContextoImp.MoveTo(120, filas-10);		ContextoImp.Show("FIRMA de enfermera");
       		 	ContextoImp.MoveTo(350, filas) ;		ContextoImp.Show("_________________________________________"); 
    			ContextoImp.MoveTo(380, filas-10);		ContextoImp.Show("FIRMA de cajera(o) de recibido");
       		 	contador+=1; 
        		filas-=10;
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Este folio no contiene productos aplicados \n"+
							"por usted el dia de HOY para que la hoja de cargos se muestre \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
				
				/*
				double ancho;
				double alto;
				trabajoImpresion.GetPageSize (out ancho, out alto);
				Print.Beginpage (ContextoImp, "Prueba de impresion");
				Print.Moveto (ContextoImp, 1, 600);
				Gdk.Pixbuf logohosp = new Gdk.Pixbuf ("/opt/osiris/img/hsc_logo_menu.png");
				double scala = System.Math.Min (ancho / logohosp.Width, alto / logohosp.Height);
				Print.Scale (ContextoImp, 80, 35);
				Print.Moveto (ContextoImp, 600, 1);
				Print.Pixbuf (ContextoImp, logohosp);
				Print.Moveto (ContextoImp, 600, 1);
				ContextoImp.MoveTo(20,500);
				//Prin.Grestore (ContextoImp);
				Print.Showpage (ContextoImp);
				*/
			}	
		}catch (NpgsqlException ex){
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			return; 
		}
	}
 }    
}
