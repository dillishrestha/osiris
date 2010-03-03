// created on 26/06/2007 at 09:42 a
// Sistemas Hospitalario OSIRIS
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
	public class notas_de_cargos
	{
		string connectionString;
        string nombrebd;
		int PidPaciente = 0;
		int folioservicio = 0;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		int id_tipopaciente = 0;
		string tipo_paciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool apl_desc_siempre = true;
		bool apl_desc;
		string area;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string query_rango;
				
		int filas=635;
		int contador = 1;
		int contadorprod = 0;
		int numpage = 1;
						
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public notas_de_cargos ( int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string area_,string NomEmpleado_,string AppEmpleado_,
						string ApmEmpleado_,string LoginEmpleado_, string query_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
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
			query_rango = query_;
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "HOJA DE CARGOS", 0);
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
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:");
			
			  			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(200.5, 740);			ContextoImp.Show("HOJA DE CARGOS");
			ContextoImp.MoveTo(201, 740);			ContextoImp.Show("HOJA DE CARGOS");
							
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
			ContextoImp.MoveTo(20.7, 720);			ContextoImp.Show("Fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(20, 720);			ContextoImp.Show("Fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			
			//ContextoImp.MoveTo(444.7, 720);			ContextoImp.Show("Pagina "+numpage.ToString());
			//ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString());
					
			ContextoImp.MoveTo(20, 710);			ContextoImp.Show("INGRESO: "+ fecha_admision.ToString());
			ContextoImp.MoveTo(460, 710);			ContextoImp.Show("EGRESO: "+ fechahora_alta.ToString());
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(529.5, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			ContextoImp.MoveTo(530, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
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
		int filas2 = 635;
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas-1, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+1, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+2, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+553, filasl);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+554, filasl);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+556, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		filas2 = 635;
		for(int i2=0; i2 < 57; i2++)//30 veces para tasmaño carta
		{	
			Gnome.Print.Setfont (ContextoImp, fuente11);
			ContextoImp.MoveTo(75, filas2);						ContextoImp.Show("|");//52
			filas2-=10;
		}
		filas2 = 635;
		for(int i3=0; i3 < 37; i3++)//30 veces para tasmaño carta
		{	
			Gnome.Print.Setfont (ContextoImp, fuente7);
			ContextoImp.MoveTo(20, filas2);
			ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
			filas2-=15;
		}
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
		Gnome.Print.Setfont (ContextoImp, fuente7);
    }
    
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(210.5, filas);			ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
		ContextoImp.MoveTo(211, filas);				ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
		filas-=10;
		Gnome.Print.Setfont (ContextoImp, fuente9);
		ContextoImp.MoveTo(26.5, filas);			ContextoImp.Show("CANTIDAD");//64.5625
		ContextoImp.MoveTo(27, filas);				ContextoImp.Show("CANTIDAD");//65,625
		ContextoImp.MoveTo(210.5, filas);			ContextoImp.Show("DESCRIPCION DEL PRODUCTO");//107.5
		ContextoImp.MoveTo(211, filas);				ContextoImp.Show("DESCRIPCION DEL PRODUCTO");//625
		Gnome.Print.Setfont (ContextoImp, fuente9);
	}
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		ContextoImp.BeginPage("Pagina 1");
		filas=635;
        //VARIABLES
        imprime_encabezado(ContextoImp,trabajoImpresion);
     	genera_tabla(ContextoImp,trabajoImpresion);
     	imprime_titulo(ContextoImp,trabajoImpresion);
     	ContextoImp.ShowPage();
    }
 }    
}
