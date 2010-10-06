///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
// created on 22/09/2010 at 17:20 p
// 				
// Autor    	: Ing. Daniel Olivares C. GTKPrint con Pango y Cairo arcangeldoc@gmail.com
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
/// //////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_solicitud_labrx
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		int linea_detalle1;
		int linea_detalle2;
		string departament;
		string agrupacion_lab_rx;
		string query_general;
		
		string connectionString;
        string nombrebd;
		
		class_public classpublic = new class_public();
		class_conexion conexion_a_DB = new class_conexion();
		
		protected Gtk.Window MyWinError;
		
		public rpt_solicitud_labrx (string departament_,int id_tipoadmisiones_,string agrupacion_lab_rx_,string query_numerosolicitud)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			departament = departament_;
			agrupacion_lab_rx = agrupacion_lab_rx_;
			
			query_general = "SELECT area_quien_solicita,folio_de_solicitud,fechahora_solicitud "+
							"FROM osiris_his_solicitudes_labrx "+
							"WHERE id_tipo_admisiones = '"+id_tipoadmisiones_.ToString().Trim()+"' "+query_numerosolicitud;
			
			print = new PrintOperation ();
			print.JobName = departament;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);	
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
			PrintContext context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
				comando.CommandText = query_general;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
		       	if (lector.Read()){
					comienzo_linea = 05;
					imprime_encabezado(cr,layout);
					comienzo_linea = 55;
					imprime_cuerpo(cr,layout, (string) lector["area_quien_solicita"],lector["folio_de_solicitud"].ToString().Trim(),lector["fechahora_solicitud"].ToString().Trim());
					linea_detalle1 = comienzo_linea;
					linea_detalle1 += separacion_linea;
					cr.MoveTo(07*escala_en_linux_windows,linea_detalle1*escala_en_linux_windows);		layout.SetText("Examemfmdsmfdsmf85832495834958n....");					Pango.CairoHelper.ShowLayout (cr, layout);
					
					comienzo_linea = 395;			
					imprime_encabezado(cr,layout);
					comienzo_linea = 395+55;
					imprime_cuerpo(cr,layout,(string) lector["area_quien_solicita"],lector["folio_de_solicitud"].ToString().Trim(),lector["fechahora_solicitud"].ToString().Trim());
					linea_detalle2 = comienzo_linea;
					linea_detalle2 += separacion_linea;
					
					cr.MoveTo(07*escala_en_linux_windows,linea_detalle2*escala_en_linux_windows);		layout.SetText("Examemfmdsmfdsmf85832495834958n....");					Pango.CairoHelper.ShowLayout (cr, layout);

					while (lector.Read()){
						
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);				
			}
			
			
						
			
		}
	 
		void imprime_cuerpo(Cairo.Context cr,Pango.Layout layout,string areaquiensolicita,string numerosolicitud,string fechasolicitud)
		{			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Area quien Solicito: "+areaquiensolicita);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: ");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Envio: "+fechasolicitud);						Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea-separacion_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: ");						Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Nacimiento: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: ");						Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: ");						Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Procedimiento: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Diagnostico: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Medico Tratante: ");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: ");							Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Usuario: ");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nom. Solicitante: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Gabinete o Proveedor : ");							Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Estudio(s) Solicitado(s) : ");							Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			
			Console.WriteLine(comienzo_linea.ToString());
			cr.Rectangle (05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows, 565*escala_en_linux_windows, 180*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
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
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(200*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(departament);				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			
			cr.MoveTo(565*escala_en_linux_windows, 383*escala_en_linux_windows);
			cr.LineTo(05,383);		// Linea Horizontal 4
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
		}	
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}
