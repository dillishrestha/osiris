// created on 18/06/2007 at 09:51 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 
//  25/05/2010   Ing. Daniel Olivares Cuevas (Programacion y Traspasado a GTKPrint) arcangeldoc@gmail.com
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
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class hoja_cargos
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		
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
		string entry_id_habitacion;
				
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string query_rango;
		int tipointernamiento = 10;
		
		int filas=690;//635
		int contador = 1;
		int contadorprod = 0;
		int numpage = 1;		
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public hoja_cargos ( int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string area_,string NomEmpleado_,string AppEmpleado_,
						string ApmEmpleado_,string LoginEmpleado_, string query_, int tipointernamiento_,string entry_id_habitacion_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			fecha_admision = entry_fecha_admision_;
			fechahora_alta = entry_fechahora_alta_;
			nombre_paciente = entry_nombre_paciente_;
			telefono_paciente = entry_telefono_paciente_;
			doctor = entry_doctor_;
			cirugia = cirugia_;
			id_tipopaciente = idtipopaciente_;
			tipo_paciente = entry_tipo_paciente_;
			aseguradora = entry_aseguradora_;
			edadpac = edadpac_;
			fecha_nacimiento = fecha_nacimiento_;
			dir_pac = dir_pac_;
			empresapac = empresapac_;
			query_rango = query_;
			tipointernamiento = tipointernamiento_;
			area = area_;							// Recibe el parametro del modulo que manda a imprimir (UCIA, Hospital, Urgencia, etc)
			entry_id_habitacion = entry_id_habitacion_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
					
			if((bool) valida_impresion_enfermera() == true){			
				print = new PrintOperation ();						
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Este folio no contiene productos aplicados \n"+
							"por usted el dia de HOY para que la hoja de cargos se muestre \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			PrintContext context = args.Context;											
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			
			ejecutar_consulta_reporte(context);
			
			// crea una pagina nueva
			/*
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			
			cr.ShowPage();
			layout = null;
			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(100,100);	layout.SetText("Prueba de Impresion--------------------------------");		Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.ShowPage();
			cr.MoveTo(100,100);	layout.SetText("Prueba de Impresion--------------------------------");		Pango.CairoHelper.ShowLayout (cr, layout);*/
		}
		void ejecutar_consulta_reporte(PrintContext context)
		{
			imprime_encabezado(context);
		}
		
      	
		void imprime_encabezado(PrintContext context)
		{
			//Console.WriteLine("entra en la impresion del encabezado");
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			//context.PageSetup.Orientation = PageOrientation.Landscape;
			
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("FOLIO DE ATENCION");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(510*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(folioservicio.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(480*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("HOJA REGISTROS DE "+area);				Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 8.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(220*escala_en_linux_windows,45*escala_en_linux_windows);			layout.SetText("DATOS GENERALES DEL PACIENTE");			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,55*escala_en_linux_windows);			layout.SetText("INGRESO:"+fecha_admision.Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(465*escala_en_linux_windows,55*escala_en_linux_windows);			layout.SetText("EGRESO:"+fechahora_alta.Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("EXP.: "+PidPaciente.ToString()+"	Nombre Paciente:"+nombre_paciente.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(340*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("Fecha de Nacimiento: "+fecha_nacimiento.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(500*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("Edad: "+edadpac.ToString());							Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText("Direccion: "+dir_pac.ToString());							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,85*escala_en_linux_windows);			layout.SetText("Tel. Pac.: "+telefono_paciente.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,85*escala_en_linux_windows);			layout.SetText("Nº Hab/Sala: "+entry_id_habitacion.Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			if((string) aseguradora == "Asegurado"){
				cr.MoveTo(05*escala_en_linux_windows,95*escala_en_linux_windows);			layout.SetText("Tipo de paciente: "+tipo_paciente.ToString()+"	Aseguradora: "+aseguradora.ToString()+"	Poliza: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(05*escala_en_linux_windows,95*escala_en_linux_windows);			layout.SetText("Tipo de paciente: "+tipo_paciente.ToString()+"	Empresa: "+empresapac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			}
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			if(doctor.ToString() == " " || doctor.ToString() == ""){
				cr.MoveTo(05*escala_en_linux_windows,105*escala_en_linux_windows);			layout.SetText("Medico: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
		
		bool valida_impresion_enfermera()
		{
			return true;
		}
		
		/*
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      									
			ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString());
					
						
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());						
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(471, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
			ContextoImp.MoveTo(20, 690);			ContextoImp.Show("Direccion: "+dir_pac.ToString());
			
			ContextoImp.MoveTo(20, 670);			ContextoImp.Show("Tel. Pac.: "+telefono_paciente.ToString());
			ContextoImp.MoveTo(450, 670);			ContextoImp.Show("Nº de habitacion:  ");
			
			if((string) aseguradora == "Asegurado")
			{				
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente: "+tipo_paciente.ToString()+"      	Aseguradora: "+aseguradora.ToString()+"      Poliza: ");
				
			}else{
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente: "+tipo_paciente.ToString()+"       Empresa: "+empresapac.ToString());
				
		 	}
		 	
			if(doctor.ToString() == " " || doctor.ToString() == "")
			{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: ");
				ContextoImp.MoveTo(250,660);			ContextoImp.Show("Especialidad:");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico : "+cirugia.ToString());
			}else{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: "+doctor.ToString()+"  Especialidad:  ");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}
	  }
      
          
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente9);
		ContextoImp.MoveTo(190.5, filas);			ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
		ContextoImp.MoveTo(191, filas);				ContextoImp.Show("Usuario: "+LoginEmpleado+" -- "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);//635
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
					"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, "+ 
					"osiris_productos.id_grupo_producto,descripcion_producto, "+
					"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"osiris_grupo_producto.descripcion_grupo_producto, "+
					"  "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mi') AS horacreacion,  "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH24') AS tiempocreacion,  "+
					"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada  "+
					"FROM "+ 
					"osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto "+
					"WHERE "+
					"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+tipointernamiento.ToString()+"' "+
					"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
					"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
		        	"AND id_empleado = '"+LoginEmpleado+"' "+
		        	"AND osiris_erp_cobros_deta.eliminado = 'false' ";
		try{
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	comando.CommandText =query_todo+query_rango+ "ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd 24HH:mm') ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
			NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
			
			filas=635;
        	if (lector.Read()){	
        		//VARIABLES
        		datos = (string) lector["descripcion_producto"];
				/////DATOS DE PRODUCTOS
      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
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
				
				while (lector.Read()){
        			if (contador==1){
        				imprime_encabezado(ContextoImp,trabajoImpresion);
        				imprime_titulo(ContextoImp,trabajoImpresion);
		        		contador+=1;		salto_pagina(ContextoImp,trabajoImpresion,contador);
		       			imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
		       			contador+=1;	 	salto_pagina(ContextoImp,trabajoImpresion,contador);
        			 	
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
					
				}//termino de ciclo
				imprime_encabezado(ContextoImp,trabajoImpresion);
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
	*/
 }    
}
