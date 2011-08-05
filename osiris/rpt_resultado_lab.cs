// created on 05/02/2008 at 06:54 p
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
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
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class imprime_resultadolab
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
        string nombrebd;
		int PidPaciente = 0;
		int folioservicio = 0;
		string LoginEmpleado;
		string dir_paciente;
		string edadpac;
		string empresapac;
		string folio_laboratorio;
		string fecha_solucitud;
		string nombre_paciente;
		string quimicoautorizado;
		string fecha_nac;
		string tipo_examen;
		string tipo_paciente;
		string hora_solicitud_estudio;
		string sexopaciente;
		string procedencia;
		string medicotratante;
		string nombre_estudio;
		string observa;
		string cedulaquimico;
		bool checkbutton_parametros;
		
		Gtk.ListStore treeViewEngineresultados;
		Gtk.TreeView lista_de_resultados;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//public System.Drawing.Image myimage;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
	
		public imprime_resultadolab (object _lista_de_resultados_,object _treeViewEngineresultados_,string _LoginEmpleado_,string nombrebd_,
									string _dir_paciente_,string _edadpac_,string _empresapac_,string entry_folio_laboratorio_res,
									string _entry_fecha_solicitud_res_,int PidPaciente_,string _entry_nombre_paciente_,
									string _quimicoaut_,int _folioservicio_,string _fecha_nacimiento_,
									string _tipo_examen_,string _entry_tipo_paciente_,string _hora_solicitud_estudio_,
									string _sexopaciente_, string _procedencia_,string _medicotratante_,string _nombre_estudio_,
									string _observa_,string _cedulaquimico_)
		{
			lista_de_resultados = _lista_de_resultados_ as Gtk.TreeView;
			treeViewEngineresultados = _treeViewEngineresultados_ as Gtk.ListStore;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			PidPaciente = PidPaciente_;
			folioservicio = _folioservicio_;
			LoginEmpleado = _LoginEmpleado_;
			dir_paciente = _dir_paciente_;
			edadpac = _edadpac_;
			empresapac = _empresapac_;
			folio_laboratorio = entry_folio_laboratorio_res;
			fecha_solucitud = _entry_fecha_solicitud_res_;
			nombre_paciente = _entry_nombre_paciente_;
			quimicoautorizado = _quimicoaut_;
			fecha_nac = _fecha_nacimiento_;
			tipo_examen = _tipo_examen_;
			tipo_paciente = _entry_tipo_paciente_;
			hora_solicitud_estudio = _hora_solicitud_estudio_;
			sexopaciente = _sexopaciente_ ;
			if(sexopaciente == "H"){ // Hombre
				sexopaciente = "MASCULINO";
			}else{// Mujer
				sexopaciente = "FEMENINO";
			}
			procedencia = _procedencia_;
			medicotratante = _medicotratante_;
			nombre_estudio = _nombre_estudio_;
			observa = _observa_;
			cedulaquimico = _cedulaquimico_;
			checkbutton_parametros = true; //_checkbutton_parametros_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Resultados de Laboratorio";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
		}
      
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			//para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
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
			
			imprime_encabezado(cr,layout);	
			comienzo_linea += separacion_linea;
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(this.nombre_estudio);		Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			
			cr.MoveTo(570*escala_en_linux_windows,(comienzo_linea+separacion_linea)*escala_en_linux_windows);
			cr.LineTo(05,comienzo_linea+separacion_linea);		// Linea Horizontal 4
			cr.Stroke();
			
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			cr.MoveTo(100*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("PARAMETROS");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("RESULTADOS");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("V.R.");		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			
			int lineahorizontal = comienzo_linea;
			
			TreeIter iter;
			if ( this.treeViewEngineresultados.GetIterFirst (out iter)){
				if((bool) this.lista_de_resultados.Model.GetValue (iter,5) == true){
					if((string) this.lista_de_resultados.Model.GetValue (iter,0) != ""){
						cr.MoveTo(570*escala_en_linux_windows,(comienzo_linea)*escala_en_linux_windows);
						cr.LineTo(05,comienzo_linea);		// Linea Horizontal 4
						cr.LineWidth = 0.1;
						cr.Stroke();
					}
					cr.MoveTo(25*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,0));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(230*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,1));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,2));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(570*escala_en_linux_windows,(comienzo_linea+separacion_linea)*escala_en_linux_windows);
					cr.LineTo(05,comienzo_linea+separacion_linea);		// Linea Horizontal 4
					cr.Stroke();
					comienzo_linea += separacion_linea;
				}	
				while (treeViewEngineresultados.IterNext(ref iter)){
					if((bool) this.lista_de_resultados.Model.GetValue (iter,5) == true){
						if((string) this.lista_de_resultados.Model.GetValue (iter,0) != ""){
							cr.MoveTo(570*escala_en_linux_windows,(comienzo_linea)*escala_en_linux_windows);
							cr.LineTo(05,comienzo_linea);		// Linea Horizontal 4
							cr.LineWidth = 0.1;
							cr.Stroke();
						}
						cr.MoveTo(25*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,0));		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(230*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,1));		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(360*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) this.lista_de_resultados.Model.GetValue (iter,2));		Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
					}
				}
			}
			cr.MoveTo(570*escala_en_linux_windows,(comienzo_linea)*escala_en_linux_windows);
			cr.LineTo(05,comienzo_linea);
			cr.LineWidth = 0.1;
			
			cr.MoveTo(05*escala_en_linux_windows, lineahorizontal*escala_en_linux_windows);
			cr.LineTo(05,comienzo_linea);
			
			cr.MoveTo(570*escala_en_linux_windows, lineahorizontal*escala_en_linux_windows);
			cr.LineTo(570,comienzo_linea);
			
			cr.Stroke();
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Observaciones: "+observa);		Pango.CairoHelper.ShowLayout (cr, layout);			
			
			cr.MoveTo(300*escala_en_linux_windows,660*escala_en_linux_windows);		layout.SetText("Quimico: ");		Pango.CairoHelper.ShowLayout (cr, layout);			
			cr.MoveTo(300*escala_en_linux_windows,670*escala_en_linux_windows);		layout.SetText(quimicoautorizado);	Pango.CairoHelper.ShowLayout (cr, layout);			
			cr.MoveTo(300*escala_en_linux_windows,680*escala_en_linux_windows);		layout.SetText("Ced.Prof. "+cedulaquimico);		Pango.CairoHelper.ShowLayout (cr, layout);			
			
			Gtk.Image image5 = new Gtk.Image();
            image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/firma_quimica.jpg");   // en Linux
			//image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,05,comienzo_linea*escala_en_linux_windows);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(130, 91, Gdk.InterpType.Bilinear),300*escala_en_linux_windows,570*escala_en_linux_windows);
			Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,300*escala_en_linux_windows,570*escala_en_linux_windows);
			cr.Fill();
			cr.Paint();
			cr.Restore();
			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//---image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(479*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			
			comienzo_linea += separacion_linea;
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(479*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
	
			comienzo_linea += separacion_linea;
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			comienzo_linea += separacion_linea;
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(200*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("RESULTADOS DE LABORATORIO");				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			// Cambiando el tamaño de la fuente
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			
			cr.Rectangle (05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows, 570*escala_en_linux_windows, 70*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
						
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(07*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Solicitud: "+fecha_solucitud);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: ");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Validacion: ");						Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+folioservicio.ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+PidPaciente.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombre_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Nacimiento: "+fecha_nac);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpac+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: "+sexopaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;
			comienzo_linea += separacion_linea;
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Procedimiento: ");	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Diagnostico Admision: ");	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Medico Tratante: "+medicotratante);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: ");		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Departamento quien Solicita: "+procedencia);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
 	}    
}