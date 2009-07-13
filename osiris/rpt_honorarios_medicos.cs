// created on 21/05/2007 at 05:28 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
//				  Daniel Olivares C. (Mejoras y Adecuaciones) arcangeldoc@gmail.com 05/05/2007
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
using GtkSharp;

namespace osiris
{
	public class rpt_honorario_med
	{
		public string connectionString = "Server=192.168.1.4;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public int PidPaciente = 0;
		public int folioservicio = 0;
		public int id_tipopaciente = 0;
		public string nombrebd;
		public string fecha_admision;
		public string hora_registro;
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
		
		public rpt_honorario_med(int PidPaciente_ , int folioservicio_,string _nombrebd_ ,string entry_fecha_admision_,string entry_hora_registro_,
						string entry_fechahora_alta_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_, string honorario_med_, string numfactu_)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;//
			hora_registro = entry_hora_registro_;//
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
			
		
			PrintJob    trabajo   = new PrintJob (PrintConfig.Default());
        	PrintDialog dialogo   = new PrintDialog (trabajo, "RESUMEN DE FACTURA", 0);
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
                new PrintJobPreview(trabajo, "RESUMEN DE FACTURA").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(PrintContext ContextoImp, PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			
			//ContextoImp.MoveTo(484.7, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			//ContextoImp.MoveTo(485, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			   			
			Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
							
			Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(470.5, 755);			ContextoImp.Show("FOLIO DE ATENCION");
			ContextoImp.MoveTo(471, 755);			ContextoImp.Show("FOLIO DE ATENCION");
							
			Print.Setfont (ContextoImp, fuente12);
			Print.Setrgbcolor(ContextoImp, 150,0,0);
			ContextoImp.MoveTo(520.5,740 );			ContextoImp.Show( folioservicio.ToString());
			ContextoImp.MoveTo(521, 740);			ContextoImp.Show( folioservicio.ToString());
					
			Print.Setfont (ContextoImp, fuente36);
			Print.Setrgbcolor(ContextoImp, 0,0,0);
			ContextoImp.MoveTo(20, 735);				ContextoImp.Show("____________________________");
									    			
			////////////DATOS GENERALES PACIENTE//////////////////
			Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(224.5, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			ContextoImp.MoveTo(225, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			
			Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(444.7, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
								
			ContextoImp.MoveTo(20, 710);			ContextoImp.Show("INGRESO: "+ fecha_admision.ToString()+"  "+ hora_registro.ToString());
			ContextoImp.MoveTo(260, 710);			ContextoImp.Show("EGRESO: "+ fechahora_alta.ToString());
			ContextoImp.MoveTo(460, 710);			ContextoImp.Show("Nº FACT: "+numfactu);
			
			Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(529.5, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			ContextoImp.MoveTo(530, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
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
			if(doctor.ToString() == " " || doctor.ToString() == "")
		{
			ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: ");
			ContextoImp.MoveTo(250, 660);			ContextoImp.Show("Especialidad:");
			ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
		}else{
			ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: "+doctor.ToString()+"           Especialidad:  ");
			ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
		}
      }
      
      void genera_tabla(PrintContext ContextoImp, PrintJob trabajoImpresion)
      {
      	//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
		Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20, 645);				ContextoImp.Show("____________________________");
				
		////COLUMNAS
		int filasl = 617;
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		//columnas tenues
		//int filasc =640;
		Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    
    void imprime_titulo(PrintContext ContextoImp, PrintJob trabajoImpresion, string descrp_admin)
    {
    	Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		//genera_lineac(ContextoImp, trabajoImpresion);
		filas-=10;
		Print.Setfont (ContextoImp, fuente7);
	}
	
	void imprime_subtitulo(PrintContext ContextoImp, PrintJob trabajoImpresion, string grupodelproducto)
	{
		float varpaso = float.Parse(honorario_med);
		Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(29.5, filas);				ContextoImp.Show(grupodelproducto);//625
		ContextoImp.MoveTo(437.6, filas);			ContextoImp.Show("TOTAL   "+varpaso.ToString("C"));//625
		
		filas-=10;		
		Print.Setfont (ContextoImp, fuente7);
		
    }
   
	void ComponerPagina (PrintContext ContextoImp, PrintJob trabajoImpresion)
	{
		filas=635;
		
		ContextoImp.BeginPage("Pagina 1");
		
		////DATOS DE PRODUCTOS
		imprime_encabezado(ContextoImp,trabajoImpresion);
     	genera_tabla(ContextoImp,trabajoImpresion);
     	
     	contador+=1;
     	imprime_titulo(ContextoImp,trabajoImpresion,"HONORARIOS MEDICOS");
        contador+=1;
        	 	
       	imprime_subtitulo(ContextoImp,trabajoImpresion,"HONORARIOS MEDICOS");
       	contador+=1;
       	ContextoImp.ShowPage();
       		 	
	}
 }    
}
