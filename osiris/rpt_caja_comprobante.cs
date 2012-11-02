///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
//				  Traspaso a GTKPrint 23/09/2010
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
	public class caja_comprobante
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 60;
		int separacion_linea = 10;
		int numpage = 1;
		int numero_comprobante;
		
		string connectionString;
        string nombrebd;
		float valoriva;
		string tipocomprobante = "";
		string tipo_paciente = "";
		string empresapac = "";
		string nombreempleado = "";
		string titulo_comprobante = "";
		
		PrintContext context;
		
		string sql_compcaja = "";
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') >= '"+DateTime.Now.ToString("dd")+"'  AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') <= '"+DateTime.Now.ToString("dd")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') >= '"+DateTime.Now.ToString("MM")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " ;
		//Declaracion de ventana de error
		string sql_foliodeservicio = "";
		string sql_numerocomprobante = "";
		string sql_orderquery = "";
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public caja_comprobante(int numero_comprobante_, string tipo_comprobante_, int folioservicio_, string sql_consulta_, string nombreempleado_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tipocomprobante = tipo_comprobante_;
			numero_comprobante = numero_comprobante_;
			sql_compcaja = sql_consulta_;
			nombreempleado = nombreempleado_;
			
			//Console.WriteLine(tipocomprobante);
			
			if (tipocomprobante == "CAJA"){			
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
			}
			if (tipocomprobante == "ABONO"){
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_orderquery = "";
				sql_foliodeservicio = "AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
			}
			if (tipocomprobante == "SERVICIO"){			
				sql_numerocomprobante = "AND osiris_erp_comprobante_servicio.numero_comprobante_servicio = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
			}
			if (tipocomprobante == "PAGARE"){
				sql_numerocomprobante = "AND osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ";
			}
			
			print = new PrintOperation ();
			print.JobName = "IMPRIME COMPROBANTE DE "+tipocomprobante;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);					
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 3;  // crea cantidad de copias del reporte			
			
			if (tipocomprobante == "PAGARE"){
				print.NPages = 1;  // crea cantidad de copias del reporte
			}
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
			bool apl_desc = false;
			bool apl_desc_siempre = true;
			int toma_tipoadmisiones = 0;
			int toma_grupoproducto = 0;
			float toma_valor_total = 0;
			comienzo_linea = 60;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
						
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = sql_compcaja+sql_numerocomprobante+sql_foliodeservicio+sql_orderquery;
	        	Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					switch (tipocomprobante){	
					case "CAJA":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						titulo_comprobante = tipocomprobante+"_"+ (string) lector["descripcion_tipo_comprobante"];
						break;
					case "PAGARE":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						titulo_comprobante = tipocomprobante;
						break;
					case "SERVICO":
						titulo_comprobante = tipocomprobante;
						break;
					case "ABONO":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						titulo_comprobante = "CAJA_"+tipocomprobante;
						break;
					}					
					busca_tipoadmisiones(lector["foliodeservicio"].ToString().Trim());
					imprime_encabezado(cr,layout,numero_comprobante.ToString().Trim(),lector["foliodeservicio"].ToString().Trim(),lector["pidpaciente"].ToString().Trim(),
					               lector["nombre_completo"].ToString().Trim(),lector["descripcion_empresa"].ToString().Trim(),lector["telefono_particular1_paciente"].ToString().Trim(),
					               lector["fechcreacion"].ToString().Trim()+" "+lector["horacreacion"].ToString().Trim(),lector["concepto_comprobante"].ToString().Trim(),lector["observacionesvarias"].ToString().Trim(),
					                   toma_valor_total,lector["nombre_medico_encabezado"].ToString().Trim());
					if(tipocomprobante == "CAJA" || tipocomprobante == "SERVICIO"){
						toma_tipoadmisiones = (int) lector["idadmisiones"];
						toma_grupoproducto = (int) lector["id_grupo_producto"];
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 920 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960){	
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							cr.MoveTo(25*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						}else{
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						}					
						while (lector.Read()){
							if (toma_tipoadmisiones != (int) lector["idadmisiones"]){
								comienzo_linea += separacion_linea;
								cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960){
									comienzo_linea += separacion_linea;
									cr.MoveTo(25*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
									
								}
								toma_tipoadmisiones = (int) lector["idadmisiones"];
								toma_grupoproducto = (int) lector["id_grupo_producto"];							
							}else{
								if(toma_grupoproducto != (int) lector["id_grupo_producto"]){
									comienzo_linea += separacion_linea;
									cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
									toma_grupoproducto = (int) lector["id_grupo_producto"];
								}
								if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960){
									comienzo_linea += separacion_linea;
									cr.MoveTo(25*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);								
								}
							}
						}
					}
					if(tipocomprobante == "ABONO"){
						
					
					}
					if(tipocomprobante == "PAGARE"){
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("PAGARÉ");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° 1/1");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("BUENO POR "+toma_valor_total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						cr.MoveTo(270*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Monterrey, Nuevo León a los "+DateTime.Now.ToString("dd")+" días del mes de "+classpublic.nom_mes(DateTime.Now.ToString("MM"))+" del año "+DateTime.Now.ToString("yyyy"));	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						fontSize = 6.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						cr.MoveTo(370*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Lugar y fecha de expedición");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Debo(mos) y pagaré(mos) indicionalmente por este Pagaré a la orden de :");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText(classpublic.nombre_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("en Monterrey, Nuevo León el "+lector["vencimiento_pagare"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("La cantidad de:");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(toma_valor_total.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);							
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Valor Recibido a mi (nuestra) entera satisfacción. Este pagaré forma parte de una serie numerada del 1 al 1 y todos están sujetos a  la");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("condición de que, al no pagarse cualquiera de ellos a su vencimiento, serán exigible todos los que le sigan en número, ademas de  los");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("ya vencido, desde la fecha de vencimiento a este documento hasta el día de su  liquidacion, causara intereses  moratorios  al tipo  del");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("_____% mensual, pagadero en esta ciudad juntamente con el principal.");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Datos del Deudor");	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Nombre: "+lector["nombre_completo"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Acepto(amos)");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Dirección: "+lector["direccion_paciente"].ToString().Trim()+" "+lector["numero_casa_paciente"].ToString().Trim()+lector["numero_departamento_paciente"].ToString().Trim()+" "+lector["colonia_paciente"].ToString().Trim()+" CP. "+lector["codigo_postal_paciente"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Población: "+lector["municipio_paciente"].ToString().Trim()+", "+lector["estado_paciente"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Firma(s) ___________________________");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Datos y firma(s) del(os) Aval(es)");	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Nombre:_____________________________________________________ ");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Dirección:___________________________________________________");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Población:_____________________________________________ Telefono :______________");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Firma(s) ___________________________");	Pango.CairoHelper.ShowLayout (cr, layout);

						
					}
				}								
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
			conexion.Close();
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string numerocomprobante, string numerodeatencion, string numeroexpediente, 
		                        string nombrepaciente, string descripcion_empmuni, string telefono_paciente, string fechahoraregistro,
		                        string conceptocomprobante, string observacionescomprobante, float tomavalortotal,string doctor_admision )
		{
			comienzo_linea = 60;
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
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			if (tipocomprobante == "PAGARE"){
				cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_comprobante);	Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("COMPROBANTE_"+titulo_comprobante);	Pango.CairoHelper.ShowLayout (cr, layout);
			}
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(479*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("N° Folio "+numerocomprobante);				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numerodeatencion);	Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+numeroexpediente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Telefono: "+telefono_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Admision: "+fechahoraregistro);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo Paciente: "+tipo_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Empr./Munic.: "+descripcion_empmuni);	Pango.CairoHelper.ShowLayout (cr, layout);		
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Doctor: "+doctor_admision);	Pango.CairoHelper.ShowLayout (cr, layout);		
			
			if (tipocomprobante == "CAJA"){
				
				fontSize = 7.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;
				// numeros en letras
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(tomavalortotal.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);		
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(400*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText("T O T A L : "+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);		
			}
			
			if (tipocomprobante == "ABONO"){
				
				fontSize = 7.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;
				// numeros en letras
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(tomavalortotal.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);		
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(400*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText("T O T A L : "+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);		
			}
			/*
			if (tipocomprobante == "PAGARE"){				
				fontSize = 7.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;
				// numeros en letras
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(tomavalortotal.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);		
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(300*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText("VALOR TOTAL DEL PAGARE: "+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);		
			}*/
			
			if (tipocomprobante != "PAGARE"){
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
				
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*23)*escala_en_linux_windows);		layout.SetText("Concepto    : "+conceptocomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText("Observacion : "+observacionescomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText("Atendio por : "+nombreempleado);	Pango.CairoHelper.ShowLayout (cr, layout);		
	
				fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
				desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			}
		}
				
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		void busca_tipoadmisiones(string foliodeservicio)
		{
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),id_tipo_admisiones,osiris_erp_movcargos.id_tipo_paciente,pid_paciente,descripcion_tipo_paciente "+
					"FROM osiris_erp_movcargos,osiris_his_tipo_pacientes "+
					"WHERE osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_movcargos.folio_de_servicio = '"+foliodeservicio+"';";
	        	//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					tipo_paciente = lector["descripcion_tipo_paciente"].ToString().Trim();					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
			conexion.Close();
		}
	}
}