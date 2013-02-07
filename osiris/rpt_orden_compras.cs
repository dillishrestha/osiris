///////////////////////////////////////////////////////////
// project created on 23/10/2008 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// 				: Israel Peña Gonzalez
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@gmail.com 18/11/2010
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
	public class rpt_orden_compras
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string query_general = "";
		float valoriva;
		string titulo = "ORDEN DE COMPRAS ";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_orden_compras(string query_ordenescompra,string query_fechas)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
					
			query_general = "SELECT osiris_erp_ordenes_compras_enca.numero_orden_compra,osiris_erp_ordenes_compras_enca.id_proveedor,osiris_erp_ordenes_compras_enca.descripcion_proveedor," +
							"osiris_erp_ordenes_compras_enca.direccion_proveedor,to_char(id_requisicion,'9999999999') AS idrequisicion,porcentage_iva," +
							"osiris_erp_ordenes_compras_enca.rfc_proveedor,osiris_erp_ordenes_compras_enca.telefonos_proveedor,to_char(osiris_erp_ordenes_compras_enca.fecha_de_entrega,'yyyy-MM-dd') AS fechadeentrega," +
							"osiris_erp_ordenes_compras_enca.lugar_de_entrega,osiris_erp_ordenes_compras_enca.condiciones_de_pago,osiris_erp_ordenes_compras_enca.dep_solicitante," +
							"osiris_erp_ordenes_compras_enca.observaciones,to_char(osiris_erp_ordenes_compras_enca.fecha_deorden_compra,'yyyy-MM-dd') AS fechaordencompra," +
							"to_char(cantidad_comprada,'999999999.99') AS cantidadcomprada,to_char(osiris_erp_requisicion_deta.cantidad_de_embalaje,'999999.99') AS cantidadembalaje," +
							"osiris_productos.id_producto,osiris_productos.descripcion_producto,osiris_catalogo_productos_proveedores.descripcion_producto AS descripcionproducto,osiris_productos.aplicar_iva," +
							"to_char(precio_costo_prov_selec,'999999999.99') AS preciodelproveedor," +
							"osiris_erp_requisicion_deta.tipo_unidad_producto AS tipounidadproducto,tipo_orden_compra," +
							"rfc,emisor,calle,noexterior,nointerior,colonia,municipio,estado,codigopostal "+
							"FROM osiris_erp_ordenes_compras_enca,osiris_erp_proveedores,osiris_erp_requisicion_deta,osiris_productos,osiris_catalogo_productos_proveedores,osiris_erp_emisor " +
							"WHERE osiris_erp_ordenes_compras_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
							"AND osiris_erp_ordenes_compras_enca.numero_orden_compra = osiris_erp_requisicion_deta.numero_orden_compra " +
							"AND osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto " +
							"AND osiris_catalogo_productos_proveedores.id_producto = osiris_erp_requisicion_deta.id_producto " +
							"AND osiris_erp_ordenes_compras_enca.id_proveedor = osiris_catalogo_productos_proveedores.id_proveedor " +
							"AND osiris_erp_ordenes_compras_enca.id_emisor =  osiris_erp_requisicion_deta.id_emisor "+
							"AND osiris_erp_ordenes_compras_enca.id_emisor = osiris_erp_emisor.id_emisor " +
							"AND osiris_catalogo_productos_proveedores.eliminado = 'false' " +
							query_ordenescompra+" " +
							query_fechas+" ORDER BY id_orden_compra;";
					
			print = new PrintOperation ();
			print.JobName = titulo;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);
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
			int contador = 1;
			int numero_ordencompra = 0;
			string facturar_a = "";
			float precios_total = 0;
			float iva_total = 0;
			float total_total = 0;
			float calculo_iva = 0;
			float precio_mas_iva = 0;
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
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
				comando.CommandText = query_general;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){
					numero_ordencompra = (int) lector["numero_orden_compra"];
					facturar_a = classpublic.extract_spaces((string) lector["rfc"].ToString().Trim()+"-"+
								(string) lector["emisor"].ToString().Trim()+" "+
								(string) lector["calle"].ToString().Trim() +
								(string) lector["noexterior"].ToString().Trim()+" "+
								(string) lector["nointerior"].ToString().Trim()+", COL."+
								(string) lector["colonia"].ToString().Trim()+","+
								(string) lector["municipio"].ToString().Trim()+","+
								(string) lector["estado"].ToString().Trim()+","+
								(string) lector["codigopostal"].ToString().Trim());
					//Console.WriteLine((string) lector["descripcion_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["direccion_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["rfc_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["telefonos_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["lugar_de_entrega"].ToString().Trim());
					//Console.WriteLine((string) lector["dep_solicitante"].ToString().Trim());
					//Console.WriteLine((string) lector["observaciones"].ToString().Trim());
					//Console.WriteLine((string) lector["fecha_deorden_compra"].ToString().Trim());
					//Console.WriteLine((string) lector["numero_orden_compra"].ToString().Trim());
					//Console.WriteLine((string) lector["fecha_de_entrega"].ToString().Trim());
					//Console.WriteLine((string) lector["condiciones_de_pago"].ToString().Trim());
					imprime_encabezado(cr,layout,
						(string) lector["descripcion_proveedor"].ToString().Trim(),
						(string) lector["direccion_proveedor"].ToString().Trim(),
						(string) lector["rfc_proveedor"].ToString().Trim(),
						(string) lector["telefonos_proveedor"].ToString().Trim(),
						(string) lector["lugar_de_entrega"].ToString().Trim(),
					    (string) lector["condiciones_de_pago"].ToString().Trim(),
						(string) lector["dep_solicitante"].ToString().Trim(),
						(string) lector["observaciones"].ToString().Trim(),
						(string) lector["fechaordencompra"].ToString().Trim(),
						(string) lector["numero_orden_compra"].ToString().Trim(),
						(string) lector["fechadeentrega"].ToString().Trim(),
					    facturar_a,
					    (string) lector["tipo_orden_compra"].ToString().Trim());
					if((bool) lector["aplicar_iva"] == true){
						calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
						precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
					}else{
						calculo_iva = 0;
						precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
					}
					precios_total = float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
					iva_total += calculo_iva;
					total_total += precio_mas_iva;					
					
					cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
					
					desc = Pango.FontDescription.FromString ("Courier New");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					// Precio del Producto
					cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())  ));					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

					desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					
					comienzo_linea += separacion_linea;
					Console.WriteLine((string) lector["descripcionproducto"].ToString().Trim());
					while(lector.Read()){
						Console.WriteLine((string) lector["descripcionproducto"].ToString().Trim());
						if(numero_ordencompra != (int) lector["numero_orden_compra"]){
							numero_ordencompra = (int) lector["numero_orden_compra"];
							desc = Pango.FontDescription.FromString ("Courier New");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							cr.MoveTo(590*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precios_total));		Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(645*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",iva_total));			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(700*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",total_total));			Pango.CairoHelper.ShowLayout (cr, layout);
							desc = Pango.FontDescription.FromString ("Sans");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
														
							precios_total = 0;
							iva_total = 0;
							total_total = 0;
							
							if((bool) lector["aplicar_iva"] == true){
								calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}else{
								calculo_iva = 0;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}
							precios_total = float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
							iva_total += calculo_iva;
							total_total += precio_mas_iva;
							
							facturar_a = classpublic.extract_spaces((string) lector["rfc"].ToString().Trim()+"-"+
								(string) lector["emisor"].ToString().Trim()+" "+
								(string) lector["calle"].ToString().Trim() +
								(string) lector["noexterior"].ToString().Trim()+" "+
								(string) lector["nointerior"].ToString().Trim()+" "+
								(string) lector["colonia"].ToString().Trim()+","+
								(string) lector["municipio"].ToString().Trim()+","+
								(string) lector["estado"].ToString().Trim()+","+
								(string) lector["codigopostal"].ToString().Trim());
							comienzo_linea = 162;
							contador = 1;
							
							cr.ShowPage();
							imprime_encabezado(cr,layout,
									(string) lector["descripcion_proveedor"].ToString().Trim(),
									(string) lector["direccion_proveedor"].ToString().Trim(),
									(string) lector["rfc_proveedor"].ToString().Trim(),
									(string) lector["telefonos_proveedor"].ToString().Trim(),
									(string) lector["lugar_de_entrega"].ToString().Trim(),
								    (string) lector["condiciones_de_pago"].ToString().Trim(),
									(string) lector["dep_solicitante"].ToString().Trim(),
									(string) lector["observaciones"].ToString().Trim(),
									(string) lector["fechaordencompra"].ToString().Trim(),
									(string) lector["numero_orden_compra"].ToString().Trim(),
									(string) lector["fechadeentrega"].ToString().Trim(),
									facturar_a,
							        (string) lector["tipo_orden_compra"].ToString().Trim());
									cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
									desc = Pango.FontDescription.FromString ("Courier New");									
									// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
									fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
									desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
									// Precio del Producto
									cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())  ));					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

									desc = Pango.FontDescription.FromString ("Sans");									
									// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
									fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
									desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
														
									comienzo_linea += separacion_linea;
						}else{
							if((bool) lector["aplicar_iva"] == true){
								calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}else{
								calculo_iva = 0;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}
							precios_total += float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
							iva_total += calculo_iva;
							total_total += precio_mas_iva;
							cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							desc = Pango.FontDescription.FromString ("Courier New");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							// Precio del Producto
							cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())  ));					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

							desc = Pango.FontDescription.FromString ("Sans");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							
							
							contador += 1;
							comienzo_linea += separacion_linea;
						}
					}
					desc = Pango.FontDescription.FromString ("Courier New");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					cr.MoveTo(590*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precios_total));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(645*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",iva_total));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(700*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",total_total));			Pango.CairoHelper.ShowLayout (cr, layout);
					desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				}
			}catch(NpgsqlException ex){
			
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
				
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,
			                    string descripcion_proveedor_,
					           	string direccion_proveedor_,
						        string rfc_proveedor_,
					        	string telefonos_proveedor_,
				         		string lugar_de_entrega_,
				        	    string condiciones_de_pago_,
					            string dep_solicitante_,
					        	string observaciones_,
					        	string fecha_deorden_compra_,
					        	string numero_orden_compra_,
					        	string fecha_de_entrega_,
		                        string facturar_a_,string tipoordencompra)
			
		{
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
			fontSize = 9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("ORDEN_DE_COMPRAS");				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			cr.MoveTo(655*escala_en_linux_windows, 62*escala_en_linux_windows);		layout.SetText("N° O.COMPRA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 72*escala_en_linux_windows);		layout.SetText(numero_orden_compra_);				Pango.CairoHelper.ShowLayout (cr, layout);
						
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			
			cr.MoveTo(07*escala_en_linux_windows,62*escala_en_linux_windows);			layout.SetText("PROVEEDOR");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText("DIRECCION");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("R.F.C.");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("TELEFONOS");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(380*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("em@il");	Pango.CairoHelper.ShowLayout (cr, layout);
					
			cr.MoveTo(555*escala_en_linux_windows,60*escala_en_linux_windows);			layout.SetText("Fecha Orden Compra");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Lugar de Entrega");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(410*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Fecha de Entrega ");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText("Facturar A:");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(595*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Tipo Orden de Compra");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,110*escala_en_linux_windows);			layout.SetText("Observaciones");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(615*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText("Condicion de Pago");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(540*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText("TOTALES");	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;
			cr.MoveTo(610*escala_en_linux_windows,110*escala_en_linux_windows);			layout.SetText(tipoordencompra);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(65*escala_en_linux_windows,62*escala_en_linux_windows);			layout.SetText(descripcion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(65*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText(direccion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(65*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText(rfc_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText(telefonos_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(495*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText(fecha_de_entrega_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(565*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText(fecha_deorden_compra_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(80*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText(lugar_de_entrega_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText(observaciones_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText(facturar_a_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText(condiciones_de_pago_);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(07*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("N°");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Part.");						Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(07*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("100");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(27*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Cantid.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(27*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText(" Soli.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(27*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(60*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Unidad de");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Medida");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(60*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("PIEZA");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(102*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Empaque");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(102*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Produc.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(102*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(140*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Descripcion del");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(140*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Producto");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(140*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("BOLSA RECOLECTORA DE ORINA UROTEK DE 2 LTS.");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(490*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("# REQ.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(545*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PRECIO");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(595*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("SUB-TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(710*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(600*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows, 435*escala_en_linux_windows);			layout.SetText("CALIDAD: El comprador tendra el derecho de inspeccionar antes de aceptar la mercancia.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(05*escala_en_linux_windows, 445*escala_en_linux_windows);			layout.SetText("PRECIO: El Proveedor facturará a precios y terminos de la Orden de Compra.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(05*escala_en_linux_windows, 455*escala_en_linux_windows);			layout.SetText("ENTREGA: Si no se entega la mercancía dentro del plazo, el Comprador podrá cancelar el pedido o rehusarse a aceptar la mercancia.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			
			cr.MoveTo(55*escala_en_linux_windows, 545*escala_en_linux_windows);			layout.SetText("Firma Solicitante");		Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(350*escala_en_linux_windows, 545*escala_en_linux_windows);		layout.SetText("Firma Autorización");		Pango.CairoHelper.ShowLayout (cr, layout);	
			
			cr.MoveTo(05*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,430);		// vertical 1
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(750,450);		// vertical 2
			
			cr.MoveTo(550*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(550,140);		// vertical 3
			
			cr.MoveTo(650*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(650,100);		// vertical 4
			
			cr.MoveTo(25*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(25,430);		// vertical 5
			
			cr.MoveTo(57*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(57,430);		// vertical 6
			
			cr.MoveTo(100*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(100,430);		// vertical 7
			
			cr.MoveTo(138*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(138,430);		// vertical 8
			
			cr.MoveTo(480*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(480,430);		// vertical 10
			
			cr.MoveTo(530*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(530,430);		// vertical 10			
			
			cr.MoveTo(585*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(585,450);		// vertical 11
			
			cr.MoveTo(640*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(640,450);		// vertical 12
			
			cr.MoveTo(695*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(695,450);
			
			// Linea recuadro de inicio
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,60);		// Linea Horizontal 1
			// Linea divide el lugar de entrega observaciones
			cr.MoveTo(750*escala_en_linux_windows, 100*escala_en_linux_windows);
			cr.LineTo(05,100);		
			// divide tipo de OC. y condiciones de pago
			cr.MoveTo(750*escala_en_linux_windows, 120*escala_en_linux_windows);
			cr.LineTo(550,120);
			// linea inicio de titulos de los conceptos
			cr.MoveTo(750*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(05,140);
			// linea inicio de conceptos
			cr.MoveTo(750*escala_en_linux_windows, 160*escala_en_linux_windows);
			cr.LineTo(05,160);
			// linea final de los conceptos de la orden de compra
			cr.MoveTo(05*escala_en_linux_windows, 430*escala_en_linux_windows);
			cr.LineTo(750,430);
			// linea final de los totales
			cr.MoveTo(750*escala_en_linux_windows, 450*escala_en_linux_windows);
			cr.LineTo(585,450);			
			// Lineas de la Firmas
			cr.MoveTo(35*escala_en_linux_windows, 545*escala_en_linux_windows);	
			cr.LineTo(145,545);			
			cr.MoveTo(320*escala_en_linux_windows, 545*escala_en_linux_windows);	
			cr.LineTo(440,545);
						
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}	
}

/*
	public class rpt_orden_compras
	{
		//private static int pangoScale = 1024;
		//private PrintOperation print;
		//private double fontSize = 8.0;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		
		// Declarando el treeview
		Gtk.TreeView lista_productos_a_comprar;
		Gtk.TreeStore treeViewEngineProductosaComprar;
		
		string titulo = "ORDEN DE COMPRAS ";
		
		int contador = 1;
		int numpage = 1;
		int filas =-174;
		
		//Declaracion de ventana de error:
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public rpt_orden_compras()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			//nombrebd = conexion_a_DB._nombrebd;
			//lista_productos_a_comprar = lista_productos_a_comprar_ as Gtk.TreeView;
			//treeViewEngineProductosaComprar = treeViewEngineProductosaComprar_ as Gtk.TreeStore; 
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "ORDEN DE COMPRAS", 0);
        	
        	int respuesta = dialogo.Run ();
		        	
        	if (respuesta == (int) PrintButtons.Cancel){
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta){
				case (int) PrintButtons.Print:   
                	trabajo.Print (); 
                	break;
                case (int) PrintButtons.Preview:
                	new PrintJobPreview(trabajo, "ORDEN DE COMPRAS").Show();
                	break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
			
			//print = new PrintOperation ();			
			//print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			//print.DrawPage += new DrawPageHandler (OnDrawPage);
			//print.EndPrint += new EndPrintHandler (OnEndPrint);
			//print.Run (PrintOperationAction.PrintDialog, null);			
		}
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			// Crear una fuente 
			//Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
			//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
			Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
			//Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
			Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", Gnome.FontWeight.Bold ,false, 12);
				
			// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA			
			numpage = 1;			
			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			//Encabezado de pagina
			ContextoImp.Rotate(90);
//////////////////////////////////////////////ESTA FUNCION SIRVE PARA CREAR RECTANGULOS Y CUADROS:////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//cuadro FOLIO:
			Gnome.Print.RectStroked(ContextoImp,800,-45,-190,28);
			//Cuadro(PROVEEDOR y REPRESENTANTE)                                 Cuadro(FECHA DE SOLICITUD):                                         Cuadro(No. requisicion):
    	    Gnome.Print.RectStroked(ContextoImp,489,-165,-399,110);             Gnome.Print.RectStroked(ContextoImp,610,-90,-120,35);              Gnome.Print.RectStroked(ContextoImp,800,-90,-190,35);			
			//fecha de entrega                                                  condiciones de pago
		    Gnome.Print.RectStroked(ContextoImp,610,-125,-120,35);              Gnome.Print.RectStroked(ContextoImp,800,-125,-190,35);
			//c lugar de entrega                                                l.a.b. y flete
			Gnome.Print.RectStroked(ContextoImp,610,-165,-120,40);              Gnome.Print.RectStroked(ContextoImp,800,-165,-190,40);
			//cuadro grande
			Gnome.Print.RectStroked(ContextoImp,800,-435,-710,250);
			//Cuadro(importe con letra)                                                                                        
    	    Gnome.Print.RectStroked(ContextoImp,489,-476,-399,40);               
			//elaboro                                                           autorizacion                                                        depto. solicitante
			Gnome.Print.RectStroked(ContextoImp,339,-515,-250,35);              Gnome.Print.RectStroked(ContextoImp,550,-515,-180,35);              Gnome.Print.RectStroked(ContextoImp,800,-515,-190,35);
			//firma de recibido                                                 provedor
			Gnome.Print.RectStroked(ContextoImp,589,-555,-500,35);              Gnome.Print.RectStroked(ContextoImp,800,-555,-190,35);
			//cantidad solicitada
			Gnome.Print.RectStroked(ContextoImp,180,-435,-50,250);
			//descripcion:                                                 
		    Gnome.Print.RectStroked(ContextoImp,570,-435,-350,250);
			//importe:
			 Gnome.Print.RectStroked(ContextoImp,680,-480,-55,295);
////////////////////////////////ESTA FUNCION SIRVE PARA CREAR LINEAS ////////////////////////////////////////////
			Gnome.Print.LineStroked(ContextoImp,90,-210,800,-210);
			
			// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente5);
			ContextoImp.MoveTo(340.5, -25);	            ContextoImp.Show( titulo+"");
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(95, -20);			    ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(95, -30);			    ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(95, -40);			    ContextoImp.Show("Conmutador: ");
			
			Gnome.Print.Setfont(ContextoImp,fuente2);
			ContextoImp.MoveTo(620, -30);			    ContextoImp.Show("FOLIO:");
			ContextoImp.MoveTo(620, -30);			    ContextoImp.Show("FOLIO:");
			
			ContextoImp.MoveTo(95, -70);			    ContextoImp.Show("PROVEEDOR:");
			ContextoImp.MoveTo(95.5, -70);			    ContextoImp.Show("PROVEEDOR:");
			ContextoImp.MoveTo(500, -67);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(500.5, -67);			    ContextoImp.Show("FECHA DE SOLICITUD:");
			ContextoImp.MoveTo(620, -67);			    ContextoImp.Show("ORDEN DE COMPRA:");
			ContextoImp.MoveTo(620.5, -67);			    ContextoImp.Show("ORDEN DE COMPRA:");
			
			ContextoImp.MoveTo(500, -100);			    ContextoImp.Show("FECHA DE ENTREGA:");
			ContextoImp.MoveTo(500.5, -100);			ContextoImp.Show("FECHA DE ENTREGA:");
			ContextoImp.MoveTo(620, -100);			    ContextoImp.Show("CONDICIONES DE PAGO:");
			ContextoImp.MoveTo(620.5, -100);			ContextoImp.Show("CONDICIONES DE PAGO:");
			
			ContextoImp.MoveTo(95, -130);			    ContextoImp.Show("REPRESENTANTE:");
			ContextoImp.MoveTo(95.5, -130);			    ContextoImp.Show("REPRESENTANTE:");
			ContextoImp.MoveTo(500, -135);			    ContextoImp.Show("LUGAR DE ENTREGA:");
            ContextoImp.MoveTo(500.5, -135);			    ContextoImp.Show("LUGAR DE ENTREGA:");
			ContextoImp.MoveTo(620.5, -135);			ContextoImp.Show("L.A.B Y FLETES:");
			ContextoImp.MoveTo(620, -135);			ContextoImp.Show("L.A.B Y FLETES:");
			
			ContextoImp.MoveTo(90, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa, sirvase a remitir los bienes o servicios que a continuacion se detallan:");
			ContextoImp.MoveTo(90.5, -175);			ContextoImp.Show("Con base en la cotizacion presentada por su empresa, sirvase a remitir los bienes o servicios que a continuacion se detallan:");			
			
			ContextoImp.MoveTo(95, -195);			    ContextoImp.Show("No. DE");
			ContextoImp.MoveTo(95, -205);			    ContextoImp.Show("PARTIDA");
			ContextoImp.MoveTo(95.5, -195);			    ContextoImp.Show("No. DE");
			ContextoImp.MoveTo(95.5, -205);			    ContextoImp.Show("PARTIDA");
			ContextoImp.MoveTo(135, -195);			    ContextoImp.Show("CANTIDAD");
			ContextoImp.MoveTo(135, -205);			    ContextoImp.Show("SOLICITADA");
			ContextoImp.MoveTo(135.5, -195);			ContextoImp.Show("CANTIDAD");
			ContextoImp.MoveTo(135.5, -205);			ContextoImp.Show("SOLICITADA");
			ContextoImp.MoveTo(184, -200);			    ContextoImp.Show("UNIDAD ");
			ContextoImp.MoveTo(184.5, -200);			ContextoImp.Show("UNIDAD ");
            ContextoImp.MoveTo(320, -200);			    ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(320.5, -200);			ContextoImp.Show("DESCRIPCION");
			ContextoImp.MoveTo(575, -195);			    ContextoImp.Show(" PRECIO ");
			ContextoImp.MoveTo(575, -205);			    ContextoImp.Show(" UNITARIO");
			ContextoImp.MoveTo(575, -195);			    ContextoImp.Show(" PRECIO ");
			ContextoImp.MoveTo(575.5, -205);			ContextoImp.Show(" UNITARIO");
            ContextoImp.MoveTo(635, -200);			    ContextoImp.Show("IMPORTE ");
			ContextoImp.MoveTo(635.5, -200);			ContextoImp.Show("IMPORTE");
			ContextoImp.MoveTo(710, -200);			    ContextoImp.Show("OBSERVACIONES ");
			ContextoImp.MoveTo(710.5, -200);			ContextoImp.Show("OBSERVACIONES ");
			
			ContextoImp.MoveTo(95, -450);			    ContextoImp.Show("IMPORTE CON LETRAS: ");
			ContextoImp.MoveTo(95.5, -450);			ContextoImp.Show("IMPORTE CON LETRAS: ");
			ContextoImp.MoveTo(570, -445);			    ContextoImp.Show("SUB-TOTAL_15: ");
			ContextoImp.MoveTo(570.5, -445);			ContextoImp.Show("SUB-TOTAL_15: ");
			ContextoImp.MoveTo(570.5, -455);			ContextoImp.Show("SUB-TOTAL_0: ");
			ContextoImp.MoveTo(570.5, -455);			ContextoImp.Show("SUB-TOTAL_0: ");
			ContextoImp.MoveTo(570, -465);			    ContextoImp.Show("16% I.V.A: ");
			ContextoImp.MoveTo(570.5, -465);			ContextoImp.Show("16% I.V.A: ");
			ContextoImp.MoveTo(570, -475);			    ContextoImp.Show("TOTAL: ");
			ContextoImp.MoveTo(570.5, -475);			ContextoImp.Show("TOTAL: ");
			
			ContextoImp.MoveTo(95.5, -490);			ContextoImp.Show("ELABORO: ");
			ContextoImp.MoveTo(95, -490);			ContextoImp.Show("ELABORO: ");
			ContextoImp.MoveTo(380.5, -490);			ContextoImp.Show("AUTORIZACION: ");
			ContextoImp.MoveTo(380, -490);			ContextoImp.Show("AUTORIZACION: ");
			ContextoImp.MoveTo(615, -490);			ContextoImp.Show("DEPTO. SOLICITANTE: ");
			ContextoImp.MoveTo(615.5, -490);			ContextoImp.Show("DEPTO. SOLICITANTE: ");
			
			ContextoImp.MoveTo(95, -530);			ContextoImp.Show("FIRMA DE RECIBIDO: ");
			ContextoImp.MoveTo(95.5, -530);			ContextoImp.Show("FIRMA DE RECIBIDO: ");
			ContextoImp.MoveTo(245, -530);			ContextoImp.Show("NOMBRE DEL RECEPTOR: ");
			ContextoImp.MoveTo(245.5, -530);			ContextoImp.Show("NOMBRE DEL RECEPTOR: ");
			ContextoImp.MoveTo(400, -530);			ContextoImp.Show("FECHA DE RECEPCION: ");
			ContextoImp.MoveTo(400.5, -530);			ContextoImp.Show("FECHA DE RECEPCION: ");
			ContextoImp.MoveTo(615, -530);			ContextoImp.Show("PROVEEDOR: ");
			ContextoImp.MoveTo(615.5, -530);			ContextoImp.Show("PROVEEDOR: ");
			
			ContextoImp.MoveTo(95, -570);			ContextoImp.Show("NOTA: EL pedido surtira en la fecha de entrega indicada en el recuadro de esta orden de compra. ");
			ContextoImp.MoveTo(95.5, -570);			ContextoImp.Show("NOTA: EL pedido surtira en la fecha de entrega indicada en el recuadro de esta orden de compra. ");			
			
			
			
			ContextoImp.ShowPage();
		}
	}
}*/
