// created on 09/02/2011
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion Base y Ajustes)
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
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	/// <summary>
	/// Genera el reporte de:
	/// Notas Medica
	/// Notas de Enfemeria
	/// Indicaciones Medicas
	/// </summary>
	public class rpt_notas_medicas
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		PrintContext context;
		
		//private static double headerHeight = (10*72/25.4);
		//private static double headerGap = (3*72/25.4);
		
		Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
		string titulo_rpt;
		string connectionString;
		string nombrebd;
		string foliodeservicio;
		string name_field;
		string query_notas;
		
		//  Variable para el reporte
		string nombrecompletopac = "";
		string PidPaciente = "";
		string fechnacimintopac = "";
		string edadpac = "";
		string fechingreso = "";
		string fechegreso = "";
		string sexopaciente = "";
		string alergiaconocida = "";
		string diagnostico_admin = "";
		string nombreempleadoreponsable = "";
		string descripcioncuarto = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="foliodeservicio_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="name_field_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="query_notas_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="diagnostico_admin">
		/// A <see cref="System.String"/>
		/// </param>	
		public rpt_notas_medicas (string foliodeservicio_,string name_field_,string query_notas_, string diagnostico_admin_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			foliodeservicio = foliodeservicio_;
			name_field = name_field_;
			query_notas = query_notas_;
			diagnostico_admin = diagnostico_admin_;
			switch (name_field){	
				case "notas_de_evolucion":
					titulo_rpt = "NOTAS_DE_EVOLUCION";
				break;
				case "notas_de_enfermeria":
					titulo_rpt = "NOTAS_DE_ENFERMERIA";
				break;
				case "indicaciones_medicas":
					titulo_rpt = "INDICACIONES_MEDICAS";
				break;
			}
			print = new PrintOperation ();
			print.JobName = "Notas";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);		
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{				
			context = args.Context;
			ejecutar_consulta_reporte(context);			
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{	
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
									
			char[] delimiterChars = {'\n'}; // delimitador de Cadenas
			string[] words;
			string textnote = "";
									
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = query_notas;
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					PidPaciente = (string) lector["pid_paciente"].ToString().Trim();
					nombrecompletopac = (string) lector["nombre1_paciente"].ToString().Trim()+" "+(string) lector["nombre2_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+(string) lector["apellido_materno_paciente"].ToString().Trim();
					fechnacimintopac = (string) lector["fechanacimiento_pac"].ToString().Trim();
					edadpac = (string) lector["edad"].ToString().Trim();
					fechingreso = (string) lector["fechadeingreso"].ToString().Trim();
					
					if((string) lector["fechadeegreso"].ToString().Trim() == "02-01-2000 00:00"){
						fechegreso = "";
					}else{
						fechegreso = (string) lector["fechadeegreso"].ToString().Trim();
					}
					if((string) lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "MASCULINO";
					}else{
						sexopaciente = "FEMENINO";
					}
					alergiaconocida = (string) lector["alegias_paciente"].ToString().Trim();
					descripcioncuarto = (string) lector["descripcion_cuarto"].ToString().Trim()+"/"+(string) lector["numero_cuarto"].ToString().Trim();
					if(titulo_rpt == "NOTAS_DE_ENFERMERIA"){
						nombreempleadoreponsable = (string) lector["nombreempleado"].ToString().Trim();
					}else{
						nombreempleadoreponsable = "";
					}
					imprime_encabezado(cr,layout);
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					layout.FontDescription.Weight = Weight.Normal;		// Letra normal
					if((string) lector[name_field].ToString() != ""){
						layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText("Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"      "+"Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+"     Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						textnote = (string) lector[name_field].ToString().ToUpper(); 
						words =  textnote.Split(delimiterChars); // Separa las Cadenas
						// Recorre la variable
						foreach (string s in words){
							if (s.Length > 0 && s.Length <= 120){
								cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								salto_de_pagina(cr,layout);
							}else{
								int inicio_string_linea = 0;
								int total_string_x_lineas = 130;
								for(int i=1; i <= s.Length/total_string_x_lineas; i++){
									cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString().Substring(inicio_string_linea, total_string_x_lineas));	Pango.CairoHelper.ShowLayout (cr, layout);
									comienzo_linea += separacion_linea;
									salto_de_pagina(cr,layout);
									inicio_string_linea += total_string_x_lineas;
								}
								if(s.Length > (s.Length/total_string_x_lineas)*total_string_x_lineas){
									Console.WriteLine(s.Length.ToString());
									Console.WriteLine(inicio_string_linea);
									cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString().Substring(inicio_string_linea, (s.Length-inicio_string_linea)));	Pango.CairoHelper.ShowLayout (cr, layout);
									comienzo_linea += separacion_linea;
									salto_de_pagina(cr,layout);
								}
							}
						}
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText("Nombre :"+nombreempleadoreponsable);	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
						cr.MoveTo(565*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);
						cr.LineTo(05,comienzo_linea);		// Linea Horizontal 1
						cr.FillExtents();  //. FillPreserve(); 
						cr.SetSourceRGB (0, 0, 0);
						cr.LineWidth = 0.3;
						cr.Stroke();
					}
					while(lector.Read()){
						if(titulo_rpt == "NOTAS_DE_ENFERMERIA"){
							nombreempleadoreponsable = (string) lector["nombreempleado"].ToString().Trim();
						}else{
							nombreempleadoreponsable = "";
						}
						if((string) lector[name_field].ToString() != ""){
							layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
							cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText("Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"      "+"Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+"     Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
							layout.FontDescription.Weight = Weight.Normal;		// Letra normal
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							textnote = (string) lector[name_field].ToString().ToUpper(); 
							words =  textnote.Split(delimiterChars); // Separa las Cadenas
							// Recorre la variable
							foreach (string s in words){
								if (s.Length > 0 && s.Length <= 120){
									cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
									comienzo_linea += separacion_linea;
									salto_de_pagina(cr,layout);
								}else{
									int inicio_string_linea = 0;
									int total_string_x_lineas = 130;
									for(int i=1; i <= s.Length/total_string_x_lineas; i++){
										cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString().Substring(inicio_string_linea, total_string_x_lineas));	Pango.CairoHelper.ShowLayout (cr, layout);
										comienzo_linea += separacion_linea;
										salto_de_pagina(cr,layout);
										inicio_string_linea += total_string_x_lineas;
									}
									if(s.Length > (s.Length/total_string_x_lineas)*total_string_x_lineas){
										Console.WriteLine(s.Length.ToString());
										Console.WriteLine(inicio_string_linea);
										cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText(s.ToString().Substring(inicio_string_linea, (s.Length-inicio_string_linea)));	Pango.CairoHelper.ShowLayout (cr, layout);
										comienzo_linea += separacion_linea;
										salto_de_pagina(cr,layout);
									}
								}
							}
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);	layout.SetText("Nombre :"+nombreempleadoreponsable);	Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							cr.MoveTo(565*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);
							cr.LineTo(05,comienzo_linea);		// Linea Horizontal 1
							cr.FillExtents();  //. FillPreserve(); 
							cr.SetSourceRGB (0, 0, 0);
							cr.LineWidth = 0.3;
							cr.Stroke();
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Console.WriteLine("entra en la impresion del encabezado");
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
			
			desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("FOLIO DE ATENCION");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(510*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(foliodeservicio.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_rpt);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText(titulo_rpt);				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			cr.MoveTo(08*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("INGRESO: "+fechingreso);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("EGRESO: "+fechegreso);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			cr.MoveTo(08*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("EXP.: "+PidPaciente+"    Nombre: "+nombrecompletopac);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(330*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("F. de Nac: "+fechnacimintopac);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(450*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpac);											Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			comienzo_linea += separacion_linea;
			cr.MoveTo(08*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: "+sexopaciente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Alergico a: "+alergiaconocida);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(420*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: "+descripcioncuarto);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(08*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Diagnostico: "+diagnostico_admin);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			cr.Rectangle (05*escala_en_linux_windows, 70*escala_en_linux_windows, 565*escala_en_linux_windows, 40*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea >530){
				cr.ShowPage();
				desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 70;
				numpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}