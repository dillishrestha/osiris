///////////////////////////////////////////////////////////
// created on 26/07/2007 at 04:18 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
//				  Cambio a GtkPrint+Pango+Cairo
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
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class inventario_almacen_reporte
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
        string nombrebd;
		int idalmacen;
		string almacen;
		string mesinventario;
		string anoinventario;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
				//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public inventario_almacen_reporte (int id_almacen_,string almacen_,string mesinventario_,string ano_inventario_,
										string LoginEmpleado_,string NomEmpleado_,string AppEmpleado_,
										string ApmEmpleado_,string nombrebd_)
		{
			idalmacen = id_almacen_;
			almacen = almacen_;
			mesinventario = mesinventario_;
			anoinventario = ano_inventario_;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			print = new PrintOperation ();
			print.JobName = "Reporte de Inventario Fisico";	// Name of the report
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
		}
      	
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;			
			ejecutar_consulta_reporte(context);
		}
						
		void ejecutar_consulta_reporte(PrintContext context)
		{	
			decimal total_inventario = 0;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			imprime_encabezado(cr,layout);
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	        	comando.CommandText = "SELECT descripcion_producto,eliminado, "+
								"id_quien_creo,osiris_productos.aplicar_iva,osiris_inventario_almacenes.id_almacen,  "+
								"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
								"to_char(osiris_inventario_almacenes.id_producto,'999999999999') AS idproducto, "+
								//"to_char(osiris_inventario_almacenes."+mesinventario.ToString()+",'99999.99') AS stock, "+
								"to_char(osiris_inventario_almacenes.stock,'999999.99') AS stock, "+
								"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
								"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
								"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
								"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageganancia, "+
								"to_char(osiris_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss') AS fechcreacion "+
								"FROM "+
								"osiris_inventario_almacenes,osiris_productos,osiris_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
								"WHERE osiris_inventario_almacenes.id_producto = osiris_productos.id_producto "+ 
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
								"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
								"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
								"AND osiris_inventario_almacenes.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_inventario_almacenes.id_almacen = '"+(int) idalmacen +"' "+
								"AND osiris_inventario_almacenes.ano_inventario = '"+(string) anoinventario +"' "+
								"AND osiris_inventario_almacenes.mes_inventario = '"+mesinventario+"' "+
								"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_inventario_almacenes.fechahora_alta,'yyyy-mm-dd HH:mm:ss');";
							
	        	NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					comienzo_linea = 80;
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["idproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
					if(lector["descripcion_producto"].ToString().Length > 65){
						cr.MoveTo(65*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim().Substring(0,65));				Pango.CairoHelper.ShowLayout (cr, layout);				
					}else{
						cr.MoveTo(65*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
					}
					cr.MoveTo(340*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["stock"].ToString());				Pango.CairoHelper.ShowLayout (cr, layout);				
					cr.MoveTo(420*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproducto"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(460*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["embalaje"].ToString());		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(540*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproductounitario"].ToString().Trim())));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(600*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["porcentageganancia"].ToString().Trim())));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(600*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["porcentageganancia"].ToString().Trim())));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(680*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproductounitario"].ToString().Trim()) * decimal.Parse(lector["stock"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);

					total_inventario += decimal.Parse(lector["costoproductounitario"].ToString().Trim()) * decimal.Parse(lector["stock"].ToString());
					comienzo_linea += separacion_linea;
					while(lector.Read()){
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["idproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
						if(lector["descripcion_producto"].ToString().Length > 65){
							cr.MoveTo(65*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim().Substring(0,65));				Pango.CairoHelper.ShowLayout (cr, layout);				
						}else{
							cr.MoveTo(65*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["descripcion_producto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);				
						}
						cr.MoveTo(340*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["stock"].ToString());				Pango.CairoHelper.ShowLayout (cr, layout);				
						cr.MoveTo(420*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproducto"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(460*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(lector["embalaje"].ToString());		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(540*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproductounitario"].ToString().Trim())));		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(600*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:F}",decimal.Parse(lector["porcentageganancia"].ToString().Trim())));		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(680*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",decimal.Parse(lector["costoproductounitario"].ToString().Trim()) * decimal.Parse(lector["stock"].ToString())));		Pango.CairoHelper.ShowLayout (cr, layout);
						total_inventario += decimal.Parse(lector["costoproductounitario"].ToString().Trim()) * decimal.Parse(lector["stock"].ToString());
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout);
					}
					comienzo_linea += separacion_linea;
					cr.MoveTo(680*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(string.Format("{0:C}",total_inventario));		Pango.CairoHelper.ShowLayout (cr, layout);

				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
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
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(340*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("Reporte de Inventario Fisico "+almacen);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(340*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText("Mes de "+classpublic.nom_mes(mesinventario)+" del "+anoinventario);				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(350*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("Stock");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(410*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("Precio Prod.");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("Pack");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(530*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("Costo Unit.");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(590*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("% Ganancia");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(670*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("$ Inventario");				Pango.CairoHelper.ShowLayout (cr, layout);

			// Creando el Cuadro de Titulos
			cr.Rectangle (05*escala_en_linux_windows, 60*escala_en_linux_windows, 750*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea >530){
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 80;
				numpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
			
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}    
}
