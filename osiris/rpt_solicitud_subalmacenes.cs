// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Jesus Buentello G. (Programacion Base)
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@gmail.com 27/05/2010
//				  Traspaso a GTKprint y la creacion de la clase
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
//////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_solicitud_subalmacenes
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		
		string connectionString;
        string nombrebd;
		int comienzo_linea = 135;
		int separacion_linea = 10;
		int numpage = 1;
		
		string query_general = "";
		
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_solicitud_subalmacenes (int idsubalmacen, string query_numerosolicitud, string query_subalmacen, string query_fechas)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			query_general = "SELECT DISTINCT (osiris_his_solicitudes_deta.folio_de_solicitud), "+
	        		       		"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy'),"+
								"to_char(osiris_his_solicitudes_deta.folio_de_solicitud,'999999999') AS foliosol,"+
								"to_char(osiris_his_solicitudes_deta.fechahora_solicitud,'dd-MM-yyyy HH24:mi') AS fecha_sol,"+
								"to_char(osiris_his_solicitudes_deta.fechahora_autorizado,'dd-MM-yyyy') AS fecha_autorizado,"+
								"to_char(osiris_his_solicitudes_deta.fecha_envio_almacen,'dd-MM-yyyy HH24:mi') AS fecha_envio,"+								
								"osiris_his_solicitudes_deta.id_quien_solicito,"+
								"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
								"osiris_his_solicitudes_deta.id_producto,"+
								"osiris_his_solicitudes_deta.sin_stock,"+	
								"osiris_his_solicitudes_deta.solicitado_erroneo,"+
								"osiris_his_solicitudes_deta.surtido," +
								"osiris_his_solicitudes_deta.observaciones_solicitud,"+
								"osiris_empleado.id_empleado,"+	
								"osiris_productos.descripcion_producto,"+
								"to_char(osiris_his_solicitudes_deta.cantidad_solicitada,'9999999.99') AS cantsol,"+
								"to_char(osiris_his_solicitudes_deta.cantidad_autorizada,'9999999.99') AS cantaut,"+
								"osiris_his_solicitudes_deta.id_almacen AS idalmacen,osiris_almacenes.descripcion_almacen,osiris_almacenes.id_almacen,solicitud_stock,pre_solicitud,"+
								"osiris_empleado.nombre1_empleado || ' ' || "+"osiris_empleado.nombre2_empleado || ' ' || "+"osiris_empleado.apellido_paterno_empleado || ' ' || "+ 
								"osiris_empleado.apellido_materno_empleado AS nombreempl,"+
								"osiris_his_solicitudes_deta.folio_de_servicio AS foliodeatencion,"+
								"osiris_his_solicitudes_deta.pid_paciente AS pidpaciente,nombre_paciente,procedimiento_qx,diagnostico_qx,"+
								"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente "+
								"FROM osiris_his_solicitudes_deta,osiris_his_paciente,osiris_almacenes,osiris_productos,osiris_empleado "+
								"WHERE osiris_his_solicitudes_deta.id_almacen = osiris_almacenes.id_almacen "+
								"AND osiris_his_paciente.pid_paciente = osiris_his_solicitudes_deta.pid_paciente "+
								"AND osiris_empleado.login_empleado = osiris_his_solicitudes_deta.id_empleado "+
								"AND osiris_his_solicitudes_deta.folio_de_solicitud > 0 "+
								"AND osiris_productos.cobro_activo = 'true' "+
								"AND osiris_his_solicitudes_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_his_solicitudes_deta.eliminado = 'false' "+
								query_numerosolicitud+
								query_subalmacen+
								query_fechas+" ORDER BY osiris_his_solicitudes_deta.id_almacen,osiris_his_solicitudes_deta.folio_de_solicitud,osiris_productos.descripcion_producto;";
			
			print = new PrintOperation ();
			print.JobName = "Solicitudes Sub-Almacenes";
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
			int numero_solicitud;
			int numero_almacen;
			int contador_productos = 0;
			string toma_descrip_prod;
			string fechaautorizacion = "";
			string comentario = "";
			string nombrepaciente = "";
			string tiposolictud = "SOLICITUD A PACIENTE";
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
				comando.CommandText = query_general;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
		       	if (lector.Read()){
					numero_solicitud = (int) lector["folio_de_solicitud"];
					numero_almacen = (int) lector["idalmacen"];
					toma_descrip_prod = (string) lector["descripcion_producto"];
					nombrepaciente = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
									(string) lector["nombre2_paciente"].ToString().Trim()+" "+
									(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
									(string) lector["apellido_materno_paciente"].ToString().Trim();
					if(toma_descrip_prod.Length > 69){	toma_descrip_prod = toma_descrip_prod.Substring(0,68);	}
					if( (string) lector["fecha_autorizado"] == "02-01-2000"){
						fechaautorizacion = "";
					}else{
						fechaautorizacion = (string) lector["fecha_autorizado"];
					}
					if((bool) lector["solicitud_stock"] == true){
						tiposolictud = "SOLICITUD PARA STOCK DEL SUB-ALAMACEN";	
					}else{
						tiposolictud = "SOLICITUD A PACIENTE";
					}
					if((bool) lector["pre_solicitud"] == true){
						tiposolictud = "PRE-SOLICITUD PARA CIRUGIAS PROGRAMADAS";
						nombrepaciente = (string) lector["nombre_paciente"].ToString().Trim();
					}else{
						tiposolictud = "SOLICITUD A PACIENTE";
					}
					//comprueba las notas de los resultado en el reporte
					if((bool) lector["sin_stock"] == true){		comentario = "sin stock";	}
					if((bool) lector["solicitado_erroneo"] == true){		comentario = "Pedido Erroneo";	}						
					if((bool) lector["surtido"] == true){	comentario = "surtido";	}
					if(float.Parse((string) lector["cantaut"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
						comentario = "No surtido";
					}
					imprime_encabezado(cr,layout,(string) lector["descripcion_almacen"],(string) lector["foliosol"],(string) lector["fecha_envio"],
							                   (string) lector["id_quien_solicito"],(string) lector["nombreempl"],
							                   (string) lector["foliodeatencion"].ToString().Trim(),(string) lector["pidpaciente"].ToString().Trim(),nombrepaciente,
					                   			tiposolictud,(string) lector["procedimiento_qx"].ToString().Trim(),(string) lector["diagnostico_qx"].ToString().Trim(),(string) lector["observaciones_solicitud"].ToString());
								
					cr.MoveTo(15*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantsol"]);				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idproducto"]);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(120*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(toma_descrip_prod);						Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(405*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantaut"]);				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(465*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(fechaautorizacion);				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(520*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(comentario);				Pango.CairoHelper.ShowLayout (cr, layout);
					contador_productos += 1;
					comienzo_linea += separacion_linea;
					while (lector.Read()){						
						if(numero_solicitud != (int) lector["folio_de_solicitud"] || numero_almacen != (int) lector["idalmacen"]){
							cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Productos Solicitados :"+contador_productos.ToString());				Pango.CairoHelper.ShowLayout (cr, layout);
							nombrepaciente = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
											(string) lector["nombre2_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_materno_paciente"].ToString().Trim();
							numero_solicitud = (int) lector["folio_de_solicitud"];
							numero_almacen = (int) lector["idalmacen"];
							if((bool) lector["solicitud_stock"] == true){
								tiposolictud = "SOLICITUD PARA STOCK DEL SUB-ALAMACEN";	
							}
							if((bool) lector["pre_solicitud"] == true){
								tiposolictud = "PRE-SOLICITUD PARA CIRUGIAS PROGRAMADAS";
								nombrepaciente = (string) lector["nombre_paciente"].ToString().Trim();
							}
							if((bool) lector["solicitud_stock"] == false && (bool) lector["pre_solicitud"] == false){
								tiposolictud = "SOLICITUD A PACIENTE";
							}
							cr.ShowPage();
							comienzo_linea = 135;
							contador_productos = 0;
							imprime_encabezado(cr,layout,(string) lector["descripcion_almacen"],(string) lector["foliosol"],(string) lector["fecha_envio"],
							                   (string) lector["id_quien_solicito"],(string) lector["nombreempl"],
							                   (string) lector["foliodeatencion"].ToString().Trim(),(string) lector["pidpaciente"].ToString().Trim(),
							                   nombrepaciente,tiposolictud,(string) lector["procedimiento_qx"].ToString().Trim(),
							                   (string) lector["diagnostico_qx"].ToString().Trim(),(string) lector["observaciones_solicitud"].ToString());							
						}
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 69){	toma_descrip_prod = toma_descrip_prod.Substring(0,68);		}
						if( (string) lector["fecha_autorizado"] == "02-01-2000"){	
							fechaautorizacion = "";
						}else{
							fechaautorizacion = (string) lector["fecha_autorizado"];
						}
						//comprueba las notas de los resultado en el reporte
						if((bool) lector["sin_stock"] == true){	comentario = "sin stock";}
						if((bool) lector["solicitado_erroneo"] == true){	comentario = "Pedido Erroneo";		}						
						if((bool) lector["surtido"] == true){	comentario = "surtido";	}
						if(float.Parse((string) lector["cantaut"]) == 0 && (bool) lector["sin_stock"] == false && (bool) lector["solicitado_erroneo"] == false && (bool) lector["surtido"] == false){
							comentario = "No surtido";
						}
						cr.MoveTo(15*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantsol"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(60*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idproducto"]);		Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(120*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(toma_descrip_prod);					Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(405*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantaut"]);			Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(465*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(fechaautorizacion);				Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(520*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(comentario);				Pango.CairoHelper.ShowLayout (cr, layout);
						contador_productos += 1;
						comienzo_linea += separacion_linea;
						salto_de_pagina(cr,layout,(string) lector["descripcion_almacen"],(string) lector["foliosol"],(string) lector["fecha_envio"],(string) lector["id_quien_solicito"],(string) lector["nombreempl"],
						                (string) lector["foliodeatencion"].ToString().Trim(),(string) lector["pidpaciente"].ToString().Trim(),nombrepaciente,tiposolictud,
						                (string) lector["procedimiento_qx"].ToString().Trim(),(string) lector["diagnostico_qx"].ToString().Trim(),(string) lector["observaciones_solicitud"].ToString());						
					}
					cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Productos Solicitados :"+contador_productos.ToString());				Pango.CairoHelper.ShowLayout (cr, layout);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);				
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string descripcion_almacen,string numerosolicitud,
		                        string fechaenvio,string idusuario,string nombreusr,
		                        string numeroatencion,string pidpaciente,string nombrepaciente,string tiposolicitud,
		                        string procedimientoqx,string diagnosticoqx,string obs_solicitud)
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
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText("PEDIDOS DE SUB-ALMACENES");				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows, 55*escala_en_linux_windows);		layout.SetText("Area quien Solicito: "+descripcion_almacen);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(250*escala_en_linux_windows,55*escala_en_linux_windows);		layout.SetText("N° de Solicitud: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(440*escala_en_linux_windows,55*escala_en_linux_windows);		layout.SetText("Fecha Envio: "+fechaenvio);						Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,65*escala_en_linux_windows);		layout.SetText("Tipo de Solicitud: "+tiposolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(250*escala_en_linux_windows,55*escala_en_linux_windows);		layout.SetText("N° de Solicitud: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,75*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numeroatencion);					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,75*escala_en_linux_windows);		layout.SetText("N° Expe.: "+pidpaciente);						Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,75*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,85*escala_en_linux_windows);		layout.SetText("Procedimiento: "+procedimientoqx);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,85*escala_en_linux_windows);		layout.SetText("Diagnostico: "+diagnosticoqx);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,95*escala_en_linux_windows);		layout.SetText("Observaciones: "+obs_solicitud);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,105*escala_en_linux_windows);		layout.SetText("Usuario: "+idusuario);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,105*escala_en_linux_windows);		layout.SetText("Nom. Solicitante: "+nombreusr);					Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			// Creando el Cuadro de Titulos para colocar el nombre del usuario
			cr.Rectangle (05*escala_en_linux_windows, 115*escala_en_linux_windows, 565*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			layout.FontDescription.Weight = Weight.Bold;		// Letra normal
			cr.MoveTo(20*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Cantidad");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(70*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Codigo");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(150*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Descripción Producto");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Cant.Surtida");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(460*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Fech.Autorizado");		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(530*escala_en_linux_windows, 118*escala_en_linux_windows);			layout.SetText("Nota");					Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal		
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout,string descripcion_almacen,string numerosolicitud,
		                     string fechaenvio,string idusuario,string nombreusr,
		                     string numeroatencion,string pidpaciente,string nombrepaciente,string tiposolicitud,
		                     string procedimientoqx,string diagnosticoqx,string obs_solicitud)			
		{
			//context.PageSetup.Orientation = PageOrientation.Landscape;
			if(comienzo_linea > 660){
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 135;
				numpage += 1;
				imprime_encabezado(cr,layout,descripcion_almacen,numerosolicitud,fechaenvio,idusuario,nombreusr,numeroatencion,pidpaciente,nombrepaciente,tiposolicitud,procedimientoqx,diagnosticoqx,obs_solicitud);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}