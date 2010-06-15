///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
//				  Daniel Olivares Cuevas (cambio de biblioteca de impresion a GTKPrinter+Pango+Cairo)
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: 
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
	public class proc_cobranza
	{
		string connectionString;
		string nombrebd;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		        
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
		
		int contador = 1;
		int numpage = 1;
		
		int comienzo_linea = 0;
		int separacion_linea = 10;  		
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		
		//query de rango de fechas
		string query_todo = " ";
		string query_rango_fechas = " "; 
		
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string fcreacion = "";
		decimal porcentajedes =  0;
		decimal descsiniva = 0;
		decimal ivadedesc = 0;
		decimal descuento = 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subtotalelim = 0;
		decimal subt15 = 0;
		decimal subt15elim = 0;
		decimal subt0 = 0;
		decimal subt0elim = 0;
		decimal sumadesc = 0;
		decimal sumadescelim = 0;
		decimal sumaiva = 0;
		decimal sumaivaelim = 0;
		decimal total = 0;
		decimal totalelim = 0;
		decimal totaladm = 0;
		decimal totaladmelim = 0;
		decimal totaldesc = 0;
		decimal subtotaldelmov = 0;
		decimal deducible = 0;
		decimal coaseguro = 0;
		//public int contdesc = 0;
		//agrega abonos y pagos honorarios
		decimal totabono = 0;
		decimal totpago = 0;
		decimal honorarios = 0;
		decimal PorcentIVA;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public proc_cobranza (int PidPaciente_,int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string query)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			PorcentIVA = decimal.Parse(classpublic.ivaparaaplicar);
			
			//nombrebd = _nombrebd_;//
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
			query_rango_fechas = query;
			
			print = new PrintOperation ();
			print.JobName = "Procedimiento de Cobranza";	// Name of the report			
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
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
        	//Querys
			query_todo = "SELECT "+
					"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, "+ 
					"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
					"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"osiris_grupo_producto.descripcion_grupo_producto, "+
					"osiris_productos.id_grupo_producto,  "+
					"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
					"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, "+
					"to_char(osiris_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario, "+
					"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
					"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
					//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+					
					"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico, "+
					"to_char(osiris_erp_cobros_enca.total_abonos,'999999999.999') AS totalabono, "+ 
					"to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.999') AS honorario, "+
					"to_char(osiris_erp_cobros_enca.total_pago,'999999999.999') AS totalpago "+
					"FROM "+
					"osiris_erp_cobros_deta,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto "+
					"WHERE "+
					"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
					"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
					"AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"' "+
		        	"AND osiris_erp_cobros_deta.eliminado = 'false' ";
			try{
				//if (numpage == 1){
					Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 8.0;
					desc.Size = (int)(fontSize * pangoScale);
					layout.FontDescription = desc;
 					conexion.Open ();
        			NpgsqlCommand comando; 
        			comando = conexion.CreateCommand (); 
        			comando.CommandText = query_todo + query_rango_fechas + "ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
					Console.WriteLine(query_todo + query_rango_fechas + "ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ");			
        			NpgsqlDataReader lector = comando.ExecuteReader ();
        			//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
					//}
				if (lector.Read()){	
					if (  (int) lector["idadmisiones"] == 100 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 300 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 400 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 920 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true){
						apl_desc = true;
					}else{
						if(apl_desc_siempre == true){
							apl_desc = false;
							apl_desc_siempre = false;
						}
					}
					
					totpago = decimal.Parse((string) lector["totalabono"]);
					totabono = decimal.Parse((string) lector["totalpago"]);
					honorarios = decimal.Parse((string) lector["honorario"]);
				
					datos = (string) lector["descripcion_producto"];
					subtotal = decimal.Parse((string) lector["ppcantidad"]);
					porcentajedes =  decimal.Parse((string) lector["porcdesc"]);
					if((bool) lector["aplicar_iva"]== true) {
						ivaprod = (subtotal*PorcentIVA)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;
					if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
						descsiniva = (subtotal*(porcentajedes/100));
						ivadedesc =descsiniva*PorcentIVA/100;
						descuento = descsiniva+ivadedesc;
						//Console.WriteLine(descuento.ToString("C"));
        			}else{
        				descuento = decimal.Parse("0.00");
        			}
        			sumadesc +=descuento;
        		
        			totaldesc +=descuento;
					if (apl_desc == false){
						totaldesc = 0;
					}
					totaladm +=total;
					subtotaldelmov +=total;
					fcreacion = (string) lector["fechcreacion"];
					//este void crea el encabezado que aparecera en cada pagina
					imprime_encabezado(cr,layout);
					
					///imprime el titulo de cada tipo de admision y sus cargos
					imprime_titulo(cr,layout,(string) lector["descripcion_admisiones"],fcreacion);
					contador+=1;
					salto_pagina(cr,layout,contador);
					imprime_subtitulo(cr,layout,(string) lector["descripcion_grupo_producto"]);
					contador+=1;
					salto_pagina(cr,layout,contador);
					imprime_linea_producto(cr,layout,(string) lector["idproducto"],(string) lector["cantidadaplicada"],datos,(string) lector["preciounitario"],subtotal,ivaprod,total);
					contador+=1;
					salto_pagina(cr,layout,contador);				
					idadmision_ = (int) lector["idadmisiones"];
					idproducto = (int) lector["id_grupo_producto"];		
					
					while (lector.Read()){
						if (  (int) lector["idadmisiones"] == 100 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 300 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 400 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
							||(int) lector["idadmisiones"] == 920 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true){
							apl_desc = true;
						}else{
							if(apl_desc_siempre == true){
								apl_desc = false;
								apl_desc_siempre = false;
							}
						}					
						totpago = decimal.Parse((string) lector["totalabono"]);
						totabono = decimal.Parse((string) lector["totalpago"]);
						honorarios = decimal.Parse((string) lector["honorario"]);
				
						datos = (string) lector["descripcion_producto"];
						subtotal = decimal.Parse((string) lector["ppcantidad"]);
						porcentajedes =  decimal.Parse((string) lector["porcdesc"]);
						if((bool) lector["aplicar_iva"]== true) {
							ivaprod = (subtotal*PorcentIVA)/100;
							subt15 += subtotal;
						}else{
							subt0 += subtotal;
							ivaprod = 0;
						}
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
							descsiniva = (subtotal*(porcentajedes/100));
							ivadedesc =descsiniva*PorcentIVA/100;
							descuento = descsiniva+ivadedesc;
							//Console.WriteLine(descuento.ToString("C"));
        				}else{
        					descuento = decimal.Parse("0.00");
        				}
        				sumadesc +=descuento;
        		
        				totaldesc +=descuento;
						if (apl_desc == false){
							totaldesc = 0;
						}
						totaladm +=total;
						subtotaldelmov +=total;
						fcreacion = (string) lector["fechcreacion"];
						
						//imprime_linea_producto(context,(string) lector["idproducto"],(string) lector["cantidadaplicada"],datos,(string) lector["preciounitario"],subtotal,ivaprod,total);
						contador+=1;
						salto_pagina(cr,layout,contador);
///////////////////////////////// SI LA ADMISION SIGUE SIENDO LA MISMA HACE ESTO://////////////////////////////////////////						
						if(idadmision_ == (int) lector["idadmisiones"] && fcreacion == (string) lector["fechcreacion"]) { //}else{
							//Console.WriteLine("sigue en: "+(string) lector["descripcion_admisiones"]);
						
							///VARIABLES
							datos = (string) lector["descripcion_producto"];
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
							if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
								descsiniva = (subtotal*(porcentajedes/100));
								ivadedesc =descsiniva*PorcentIVA/100;
								descuento = descsiniva+ivadedesc;
								//Console.WriteLine(descuento.ToString("C"));
							}else{
								descuento = decimal.Parse("0.00");
							}
							sumadesc +=descuento;
        				
							totaldesc +=descuento;
							if (apl_desc == false){
								totaldesc = 0;
							}
							//totaladm +=total;
							//subtotaldelmov +=total;
							//Console.WriteLine("fecha no cambio = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
							//DATOS TABLA
							if (idproducto != (int) lector["id_grupo_producto"]){
								idproducto = (int) lector["id_grupo_producto"];
								imprime_subtitulo(cr,layout,(string) lector["descripcion_grupo_producto"]);
        			   			contador+=1;
								salto_pagina(cr,layout,contador);
							}        			 
						}else{ //if (idadmision_ != (int) lector["idadmisiones"]) {					
///////////////////////////////// SI LA ADMISION CAMBIA HACE ESTO://////////////////////////////////////////
							fontSize = 7.0;
							desc.Size = (int)(fontSize * pangoScale);							layout.FontDescription = desc;
							fcreacion = (string) lector["fechcreacion"];
							//Console.WriteLine("cambio de admision"+" "+(string) lector["descripcion_admisiones"]);
							//Console.WriteLine("antes de totales = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
							///IMPRESION DE LOS TOTALES DE AREA
							comienzo_linea += separacion_linea;
							cr.MoveTo(470*escala_en_linux_windows, comienzo_linea);			layout.SetText("Total de Desc.");												Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(sumadesc.ToString("N").PadLeft(10)+" -");	Pango.CairoHelper.ShowLayout (cr, layout);
							contador+=1;
							salto_pagina(cr,layout,contador);
							comienzo_linea += separacion_linea;
							cr.MoveTo(470*escala_en_linux_windows,comienzo_linea);			layout.SetText("Total de Area");											Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(530*escala_en_linux_windows, comienzo_linea);			layout.SetText(totaladm.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
							contador+=1;
							salto_pagina(cr,layout,contador);
        				
        					////VARIABLES
							datos = (string) lector["descripcion_producto"];
							totaladm = 0;
							sumadesc = 0;
							//Console.WriteLine("despues de totales = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
						
							if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
								descsiniva = (subtotal*(porcentajedes/100));
								ivadedesc =descsiniva*PorcentIVA/100;
								descuento = descsiniva+ivadedesc;
								//Console.WriteLine(descuento.ToString("C"));
							}else{
								descuento = decimal.Parse("0.00");
							}
							sumadesc +=descuento;
        				
							totaladm = 0;
							totaldesc +=descuento;
							if (apl_desc == false){
								totaldesc = 0;
							}											
							totaladm += total;
							subtotaldelmov +=total;
							if(fcreacion != (string) lector["fechcreacion"]) {
								fcreacion = (string) lector["fechcreacion"];
							}
							
							//DATOS TABLA
							///imprime el titulo de cada tipo de admision y sus cargos
							imprime_titulo(cr,layout,(string) lector["descripcion_admisiones"],fcreacion);
							contador+=1;
							salto_pagina(cr,layout,contador);							
							idadmision_ = (int) lector["idadmisiones"];        			    
							
							if (idproducto != (int) lector["id_grupo_producto"]){
								idproducto = (int) lector["id_grupo_producto"];
								imprime_subtitulo(cr,layout,(string) lector["descripcion_grupo_producto"]);
								contador+=1;
								salto_pagina(cr,layout,contador);
							}												
						}
						imprime_linea_producto(cr,layout,(string) lector["idproducto"],(string) lector["cantidadaplicada"],datos,(string) lector["preciounitario"],subtotal,ivaprod,total);
					}// Fin del ciclo While
					comienzo_linea += separacion_linea;
					cr.MoveTo(470*escala_en_linux_windows, comienzo_linea);			layout.SetText("Total de Desc.");												Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(sumadesc.ToString("N").PadLeft(10)+" -");	Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					comienzo_linea += separacion_linea;
					cr.MoveTo(470*escala_en_linux_windows,comienzo_linea);			layout.SetText("Total de Area");											Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows, comienzo_linea);			layout.SetText(totaladm.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					// Console.WriteLine("contador antes de los totales: "+contador.ToString());
					// TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
					decimal totaldelmov =subtotaldelmov - deducible - coaseguro - totaldesc - totabono - totpago + honorarios;//desctotal;
					
					comienzo_linea += separacion_linea;		    	
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("SUBTOTAL AL "+PorcentIVA.ToString()+"%");		Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(subt15.ToString("N").PadLeft(10)); 						Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("SUBTOTAL AL 0%");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(subt0.ToString("N").PadLeft(10));Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
		
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("IVA AL "+PorcentIVA.ToString()+"%");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(sumaiva.ToString("N").PadLeft(10)); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("SUB-TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(subtotaldelmov.ToString("N").PadLeft(10));Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
			
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("MENOS DEDUCIBLE");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(deducible.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("MENOS COASEGURO");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(coaseguro.ToString("N").PadLeft(10)+" -");Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("MENOS DESCUENTO");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(totaldesc.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("TOTAL PAGO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(totabono.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("TOTAL ABONO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(totpago.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
									
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("HONORARIO MEDICO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(honorarios.ToString("N").PadLeft(10)+" +"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
								
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea);			layout.SetText("TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(totaldelmov.ToString("N").PadLeft(10)); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					//cr.ShowPage();
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
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
						
			separacion_linea = separacion_linea * escala_en_linux_windows;
			
			cr.MoveTo(001,comienzo_linea);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001,comienzo_linea);					layout.SetText("Direccion: Monterrey - Mexico");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(470*escala_en_linux_windows,comienzo_linea);					layout.SetText("FOLIO DE ATENCION");				Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001,comienzo_linea);					layout.SetText("Telefono: (01)(81) 1158-5166");		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001,comienzo_linea);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 12.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(210*escala_en_linux_windows,comienzo_linea);					layout.SetText("PROCEDIMIENTO DE COBRANZA");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.SetSourceRGB(150,0,0);  // Cambio de color a Rojo
			cr.MoveTo(500*escala_en_linux_windows,comienzo_linea);					layout.SetText(folioservicio.ToString());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.SetSourceRGB(0,0,0);		// Cambio de color a Negro
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;			
			// Cambiando el tamaño de la fuente
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;			
			cr.MoveTo(224*escala_en_linux_windows,comienzo_linea);				layout.SetText("DATOS GENERALES DEL PACIENTE");	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;							
			// Cambiando el tamaño de la fuente
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(001,comienzo_linea);			layout.SetText("INGRESO: "+ fecha_admision.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(420*escala_en_linux_windows,comienzo_linea);			layout.SetText("EGRESO: "+ fechahora_alta.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			cr.MoveTo(001, comienzo_linea);														layout.SetText("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(330*escala_en_linux_windows, comienzo_linea);		layout.SetText("F. de Nac: "+fecha_nacimiento.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(450*escala_en_linux_windows, comienzo_linea);		layout.SetText("Edad: "+edadpac.ToString());											Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			comienzo_linea += separacion_linea;
			cr.MoveTo(001, comienzo_linea);		layout.SetText("Direccion: "+dir_pac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001, comienzo_linea);														layout.SetText("Tel. Pac.: "+telefono_paciente.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(450*escala_en_linux_windows, comienzo_linea);		layout.SetText("Nº de habitacion:  ");	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			if((string) tipo_paciente == "Asegurado"){				
				cr.MoveTo(001, comienzo_linea);		layout.SetText("Tipo de paciente:  "+tipo_paciente+"      	Aseguradora : "+aseguradora+"      Poliza: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(001, comienzo_linea);		layout.SetText("Tipo de paciente:  "+tipo_paciente+"              Empresa: "+empresapac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			}
			comienzo_linea += separacion_linea;
			if(doctor.ToString() == " " || doctor.ToString() == ""){
				cr.MoveTo(001, comienzo_linea);										layout.SetText("Medico: ");	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(250*escala_en_linux_windows, comienzo_linea);			layout.SetText("Especialidad:");	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(001, comienzo_linea);			layout.SetText("Cirugia/Diagnostico : "+cirugia.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(001, comienzo_linea);			layout.SetText("Medico: "+doctor.ToString()+"           Especialidad:  ");	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(001, comienzo_linea);			layout.SetText("Cirugia/Diagnostico: "+cirugia.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			}
			comienzo_linea += separacion_linea;						
			cr.MoveTo(230*escala_en_linux_windows, 730*escala_en_linux_windows);		layout.SetText("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));	Pango.CairoHelper.ShowLayout (cr, layout);
		}
			
		void imprime_titulo(Cairo.Context cr,Pango.Layout layout, string descrp_admin,string fech)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			//LUGAR DE CARGO
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea);			layout.SetText(descrp_admin.ToString()+"  "+fech.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;   // Letra Normal
		}
		
		void imprime_subtitulo(Cairo.Context cr,Pango.Layout layout, string tipoproducto)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			//LUGAR DE CARGO
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			comienzo_linea += separacion_linea;
			cr.MoveTo(080*escala_en_linux_windows, comienzo_linea);				layout.SetText("CANT.");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(025*escala_en_linux_windows, comienzo_linea);				layout.SetText("CLAVE.");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(108*escala_en_linux_windows, comienzo_linea);				layout.SetText(tipoproducto);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(385*escala_en_linux_windows, comienzo_linea);				layout.SetText("PRECIO");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(430*escala_en_linux_windows, comienzo_linea);				layout.SetText("SUB-TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(493*escala_en_linux_windows, comienzo_linea);				layout.SetText("IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(545*escala_en_linux_windows, comienzo_linea);				layout.SetText("TOTAL");			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;   // Letra Normal
		}
		
		void imprime_linea_producto(Cairo.Context cr,Pango.Layout layout,string idproducto_,string cantidadaplicada_,string datos_,string preciounitario_,decimal subtotal_,decimal ivaprod_,decimal total_)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
			//LUGAR DE CARGO
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			comienzo_linea += separacion_linea;
			cr.MoveTo(001*escala_en_linux_windows, comienzo_linea);			layout.SetText(idproducto_);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(080*escala_en_linux_windows, comienzo_linea);			layout.SetText(cantidadaplicada_);		Pango.CairoHelper.ShowLayout (cr, layout);
			if(datos_.Length > 61)	{				
				cr.MoveTo(110*escala_en_linux_windows, comienzo_linea);		layout.SetText((string) datos_.Substring(0,60));					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(110*escala_en_linux_windows,comienzo_linea);		layout.SetText((string) datos_);							Pango.CairoHelper.ShowLayout (cr, layout);
			} 
			cr.MoveTo(380*escala_en_linux_windows,comienzo_linea);			layout.SetText(preciounitario_);Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(430*escala_en_linux_windows,comienzo_linea);			layout.SetText(subtotal_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,comienzo_linea);			layout.SetText(ivaprod_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(530*escala_en_linux_windows,comienzo_linea);			layout.SetText(total_.ToString("N").PadLeft(10));			Pango.CairoHelper.ShowLayout (cr, layout);
		}
		
		void salto_pagina(Cairo.Context cr,Pango.Layout layout,int contador_)
		{			
			if (contador_ > 57 ){
				cr.ShowPage();				
				numpage +=1;
				comienzo_linea = 0;
				separacion_linea = 10;
				contador = 1;
				imprime_encabezado(cr,layout);
			}
		}
				
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}
}
		/*
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: ");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador: ");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador: ");
			ContextoImp.MoveTo(20, 740);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			
			ContextoImp.MoveTo(479.7, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			ContextoImp.MoveTo(480, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			  			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("PROCEDIMIENTO DE COBRANZA");
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("PROCEDIMIENTO DE COBRANZA");
							
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
			
			//Print.Setfont (ContextoImp, fuente8);//444.7,720
			ContextoImp.MoveTo(230.7, 60);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(230, 60);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			
			Gnome.Print.Setfont (ContextoImp, fuente8);		
			ContextoImp.MoveTo(20, 710);			ContextoImp.Show("INGRESO: "+ fecha_admision.ToString());
			ContextoImp.MoveTo(460, 710);			ContextoImp.Show("EGRESO: "+ fechahora_alta.ToString());
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(19.5, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			ContextoImp.MoveTo(20, 700);			ContextoImp.Show("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());
			
			ContextoImp.MoveTo(349.5, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(350, 700);			ContextoImp.Show("F. de Nac: "+fecha_nacimiento.ToString());
			ContextoImp.MoveTo(470.5, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			ContextoImp.MoveTo(471, 700);			ContextoImp.Show("Edad: "+edadpac.ToString());
			
			ContextoImp.MoveTo(20, 690);
			ContextoImp.Show("Direccion: "+dir_pac.ToString());
			
			ContextoImp.MoveTo(20, 670);			ContextoImp.Show("Tel. Pac.: "+telefono_paciente.ToString());
			ContextoImp.MoveTo(450, 670);			ContextoImp.Show("Nº de habitacion:  ");
			
			if((string) tipo_paciente == "Asegurado"){				
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente+"      	Aseguradora : "+aseguradora+"      Poliza: ");
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente+"      	Aseguradora : "+aseguradora+"      Poliza: ");
			}else{
				ContextoImp.MoveTo(19.5, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente+"              Empresa: "+empresapac.ToString());
				ContextoImp.MoveTo(20, 680);		ContextoImp.Show("Tipo de paciente:  "+tipo_paciente+"              Empresa: "+empresapac.ToString());
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
		for (int i1=0; i1 < 28; i1++)//30 veces para tasmaño carta
		{	
            int columnas = 17;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
			ContextoImp.MoveTo(columnas+555, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		//columnas tenues
		//int filasc =640;
		Gnome.Print.Setfont (ContextoImp, fuente36);
		ContextoImp.MoveTo(20,73);		ContextoImp.Show("____________________________");
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    
    void genera_lineac(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		Gnome.Print.Setfont (ContextoImp, fuente11);
		ContextoImp.MoveTo(75, filas);					ContextoImp.Show("|");//52
		ContextoImp.MoveTo(104, filas);					ContextoImp.Show("|");//104
		ContextoImp.MoveTo(375, filas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(425, filas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(475, filas);					ContextoImp.Show("|");
		ContextoImp.MoveTo(523, filas);					ContextoImp.Show("|");
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
    
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin,string fech)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString()+"  "+fech.ToString());//635
		ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString()+"  "+fech.ToString());//635
		//ContextoImp.MoveTo(280, filas);			ContextoImp.Show(fechacargo);//635
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		//genera_lineac(ContextoImp, trabajoImpresion);
		filas-=10;
	}
	
	void imprime_subtitulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string tipoproducto)
	{
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(79.5, filas);			ContextoImp.Show("CANT.");//24.5,625
		ContextoImp.MoveTo(80, filas);				ContextoImp.Show("CANT.");//25,625
		ContextoImp.MoveTo(24.5, filas);			ContextoImp.Show("CLAVE.");//64.5625
		ContextoImp.MoveTo(25, filas);				ContextoImp.Show("CLAVE.");//65,625
		ContextoImp.MoveTo(107.5, filas);			ContextoImp.Show(tipoproducto);//625
		ContextoImp.MoveTo(108, filas);				ContextoImp.Show(tipoproducto);//625
		ContextoImp.MoveTo(384.5, filas);			ContextoImp.Show("PRECIO");//625
		ContextoImp.MoveTo(385, filas);				ContextoImp.Show("PRECIO");//625
		ContextoImp.MoveTo(429.6, filas);			ContextoImp.Show("SUB-TOTAL");//625
		ContextoImp.MoveTo(430, filas);				ContextoImp.Show("SUB-TOTAL");//625
		ContextoImp.MoveTo(492.6, filas);			ContextoImp.Show("IVA");//625
		ContextoImp.MoveTo(493, filas);				ContextoImp.Show("IVA");//625
		ContextoImp.MoveTo(544.6, filas);			ContextoImp.Show("TOTAL");//625
		ContextoImp.MoveTo(545, filas);				ContextoImp.Show("TOTAL");//625
		///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
		genera_lineac(ContextoImp, trabajoImpresion);
		//ContextoImp.MoveTo(20, filas-2);//623
		//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		filas-=10;
    }
   
	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
	{
		//Console.WriteLine("contador antes del if: "+contador_.ToString());
        if (contador_ > 57 )
        {
        	numpage +=1;
        	ContextoImp.ShowPage();
			ContextoImp.BeginPage("Pagina "+numpage.ToString());
			imprime_encabezado(ContextoImp,trabajoImpresion);
     		genera_tabla(ContextoImp,trabajoImpresion);
     		Gnome.Print.Setfont (ContextoImp, fuente7);
        	contador=1;
        	filas=635;
        }
       //Console.WriteLine("contador despues del if: "+contador_.ToString());
	}
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString+nombrebd);
        // Verifica que la base de datos este conectada
        //Querys
		query_todo = "SELECT "+
					"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, "+ 
					"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
					"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"osiris_grupo_producto.descripcion_grupo_producto, "+
					"osiris_productos.id_grupo_producto,  "+
					"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
					"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, "+
					"to_char(osiris_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario, "+
					"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
					"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
					//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+					
					"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico, "+
					"to_char(osiris_erp_cobros_enca.total_abonos,'999999999.999') AS totalabono, "+ 
					"to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.999') AS honorario, "+
					"to_char(osiris_erp_cobros_enca.total_pago,'999999999.999') AS totalpago "+
					"FROM "+
					"osiris_erp_cobros_deta,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto "+
					"WHERE "+
					"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
					"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
					"AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"' "+
		        	"AND osiris_erp_cobros_deta.eliminado = 'false' ";
		try{
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	comando.CommandText = query_todo + query_rango_fechas + "ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
			Console.WriteLine(query_todo + query_rango_fechas + "ORDER BY  to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ");			
        	NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
								
			filas=635;
        					
        	if (lector.Read())
        	{	
        		//VARIABLES para verificar si el procedimiento se le aplica el descuento
        		if (  (int) lector["idadmisiones"] == 100 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
					||(int) lector["idadmisiones"] == 300 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
					||(int) lector["idadmisiones"] == 400 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true
					||(int) lector["idadmisiones"] == 920 && id_tipopaciente == 101 && (bool) apl_desc_siempre == true){
						apl_desc = true;
				}else{
					if(apl_desc_siempre == true){
						apl_desc = false;
						apl_desc_siempre = false;
					}
				}
						//agrega abonos y pagos honorarios
				totpago = decimal.Parse((string) lector["totalabono"]);
				totabono = decimal.Parse((string) lector["totalpago"]);
				honorarios = decimal.Parse((string) lector["honorario"]);
				
				datos = (string) lector["descripcion_producto"];
				subtotal = decimal.Parse((string) lector["ppcantidad"]);
				porcentajedes =  decimal.Parse((string) lector["porcdesc"]);
				if((bool) lector["aplicar_iva"]== true) {
					ivaprod = (subtotal*PorcentIVA)/100;
					subt15 += subtotal;
				}else{
					subt0 += subtotal;
					ivaprod = 0;
				}
				sumaiva += ivaprod;
				total = subtotal + ivaprod;
				if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
					descsiniva = (subtotal*(porcentajedes/100));
					ivadedesc =descsiniva*PorcentIVA/100;
					descuento = descsiniva+ivadedesc;
					//Console.WriteLine(descuento.ToString("C"));
        		}else{
        			descuento = decimal.Parse("0.00");
        		}
        		sumadesc +=descuento;
        		
        		totaldesc +=descuento;
				if (apl_desc == false){
					totaldesc = 0;
				}
				totaladm +=total;
				subtotaldelmov +=total;
				fcreacion = (string) lector["fechcreacion"];
        		
        		/////DATOS DE PRODUCTOS
      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	genera_tabla(ContextoImp,trabajoImpresion);
     		   	
     		   	imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"],fcreacion);
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	
       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
       		 	contador+=1;
       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	genera_lineac(ContextoImp, trabajoImpresion);
        		
        		//DATOS TABLA
				ContextoImp.MoveTo(80, filas);			ContextoImp.Show((string) lector["cantidadaplicada"]);//22	
				ContextoImp.MoveTo(22, filas);			ContextoImp.Show((string) lector["idproducto"]);//55
				if(datos.Length > 61)
				{
					ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.Substring(0,60));  
				}else{
					ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.ToString());
				} 
				ContextoImp.MoveTo(380, filas);			ContextoImp.Show("$"+(string) lector["preciounitario"]);
				ContextoImp.MoveTo(430, filas);			ContextoImp.Show(subtotal.ToString("C").PadLeft(10));
				ContextoImp.MoveTo(480, filas);			ContextoImp.Show(ivaprod.ToString("C").PadLeft(10));
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total.ToString("C").PadLeft(10));
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				idadmision_ = (int) lector["idadmisiones"];
        		idproducto = (int) lector["id_grupo_producto"];
				
				while (lector.Read())
        		{
        			if (contador==1) 
        			{
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"],fcreacion);
		        		contador+=1;
		        		salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	
		       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
		       		 	contador+=1;
		       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	genera_lineac(ContextoImp, trabajoImpresion);
        			}
        			
        			if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101
						||(int) lector["idadmisiones"] == 300 && (int) id_tipopaciente == 101 
						||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101){
							apl_desc = true;
					}else{
						if(apl_desc_siempre == true){
							apl_desc = false;
							apl_desc_siempre = false;
						}
					}
        			
        			subtotal = decimal.Parse((string) lector["ppcantidad"]);
					porcentajedes =  decimal.Parse((string) lector["porcdesc"]);
					if((bool) lector["aplicar_iva"]== true){
						ivaprod = (subtotal*PorcentIVA)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
///////////////////////////////// SI LA ADMISION SIGUE SIENDO LA MISMA HACE ESTO://////////////////////////////////////////
					if(idadmision_ == (int) lector["idadmisiones"] && fcreacion == (string) lector["fechcreacion"]) { //}else{
						//Console.WriteLine("sigue en: "+(string) lector["descripcion_admisiones"]);
						genera_lineac(ContextoImp, trabajoImpresion);
						///VARIABLES
						datos = (string) lector["descripcion_producto"];
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
							descsiniva = (subtotal*(porcentajedes/100));
							ivadedesc =descsiniva*PorcentIVA/100;
							descuento = descsiniva+ivadedesc;
							//Console.WriteLine(descuento.ToString("C"));
        				}else{
        					descuento = decimal.Parse("0.00");
        				}
        				sumadesc +=descuento;
        				
        				totaldesc +=descuento;
						if (apl_desc == false){
							totaldesc = 0;
						}
						totaladm +=total;
						subtotaldelmov +=total;
						//Console.WriteLine("fecha no cambio = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
						//DATOS TABLA
        				if (idproducto != (int) lector["id_grupo_producto"])
        			    {
        			    	idproducto = (int) lector["id_grupo_producto"];
        			   		imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
        			   		contador+=1;
        			   		salto_pagina(ContextoImp,trabajoImpresion,contador);
        			   	}
        			 
        			 }else{ //if (idadmision_ != (int) lector["idadmisiones"]) {					
///////////////////////////////// SI LA ADMISION CAMBIA HACE ESTO://////////////////////////////////////////
						fcreacion = (string) lector["fechcreacion"];
        				//Console.WriteLine("cambio de admision"+" "+(string) lector["descripcion_admisiones"]);
        				//Console.WriteLine("antes de totales = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
						///IMPRESION DE LOS TOTALES DE AREA
						genera_lineac(ContextoImp, trabajoImpresion);
        				ContextoImp.MoveTo(479.7, filas);		ContextoImp.Show("Total de Desc.");
        				ContextoImp.MoveTo(480, filas);			ContextoImp.Show("Total de Desc.");
        				ContextoImp.MoveTo(529.7, filas);		ContextoImp.Show(sumadesc.ToString("C").PadLeft(10)+" -");
        				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(sumadesc.ToString("C").PadLeft(10)+" -");
        				contador+=1;
        				filas-=10;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
        				
        				genera_lineac(ContextoImp, trabajoImpresion);
        				ContextoImp.MoveTo(479.7, filas);		ContextoImp.Show("Total de Area");
        				ContextoImp.MoveTo(480, filas);			ContextoImp.Show("Total de Area");
        				ContextoImp.MoveTo(529.7, filas);		ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        				contador+=1;
        				filas-=10;
        				salto_pagina(ContextoImp,trabajoImpresion,contador);
        				
        				////VARIABLES
        				datos = (string) lector["descripcion_producto"];
						totaladm = 0;
						sumadesc = 0;
						//Console.WriteLine("despues de totales = sumadesc"+sumadesc.ToString()+" totaladm"+totaladm.ToString());
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						
						if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
							descsiniva = (subtotal*(porcentajedes/100));
							ivadedesc =descsiniva*PorcentIVA/100;
							descuento = descsiniva+ivadedesc;
							//Console.WriteLine(descuento.ToString("C"));
        				}else{
        					descuento = decimal.Parse("0.00");
        				}
        				sumadesc +=descuento;
        				
        				//totaladm = 0;
						totaldesc +=descuento;
						if (apl_desc == false){
							totaldesc = 0;
						}
						totaladm +=total;
						subtotaldelmov +=total;
						if(fcreacion != (string) lector["fechcreacion"]) {
							fcreacion = (string) lector["fechcreacion"];
						}
						
						//DATOS TABLA
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"],fcreacion);//(string) lector["fechcreacion"]);
						contador+=1;
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						
        				idadmision_ = (int) lector["idadmisiones"];
        			    
        			   	if (idproducto != (int) lector["id_grupo_producto"])
        			    {
        			    	idproducto = (int) lector["id_grupo_producto"];
        			   		imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
        			   		contador+=1;
        			   		salto_pagina(ContextoImp,trabajoImpresion,contador);
        			   	}	
					}	
					genera_lineac(ContextoImp, trabajoImpresion);
					ContextoImp.MoveTo(80, filas);						ContextoImp.Show((string) lector["cantidadaplicada"]);	
					ContextoImp.MoveTo(22, filas);						ContextoImp.Show((string) lector["idproducto"]);
					if(datos.Length > 64)
					{
					ContextoImp.MoveTo(110, filas);				ContextoImp.Show(datos.Substring(0,60));
					}else{
					ContextoImp.MoveTo(110, filas);				ContextoImp.Show(datos);
					} 
					ContextoImp.MoveTo(380, filas);					ContextoImp.Show("$"+(string) lector["preciounitario"]);
					ContextoImp.MoveTo(430, filas);					ContextoImp.Show(subtotal.ToString("C").PadLeft(10));
					ContextoImp.MoveTo(480, filas);					ContextoImp.Show(ivaprod.ToString("C").PadLeft(10));
					ContextoImp.MoveTo(530, filas);					ContextoImp.Show(total.ToString("C").PadLeft(10));
					contador+=1;			filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}//termino de ciclo
				
        		//genera_lineac(ContextoImp, trabajoImpresion);
       		 	////IMPRESION DE LOS TOTALES DE AREA
        		genera_lineac(ContextoImp, trabajoImpresion);
        		ContextoImp.MoveTo(479.7, filas);				ContextoImp.Show("Total de Desc.");
        		ContextoImp.MoveTo(480, filas);					ContextoImp.Show("Total de Desc.");
        		ContextoImp.MoveTo(529.7, filas);				ContextoImp.Show(sumadesc.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(530, filas);					ContextoImp.Show(sumadesc.ToString("C").PadLeft(10)+" -");
        		contador+=1;
        		filas-=10;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
        		genera_lineac(ContextoImp, trabajoImpresion);
        		ContextoImp.MoveTo(479.7, filas);				ContextoImp.Show("Total de Area");
        		ContextoImp.MoveTo(480, filas);					ContextoImp.Show("Total de Area");
        		ContextoImp.MoveTo(529.7, filas);				ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(530, filas);					ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
        		//Console.WriteLine("contador antes de los totales: "+contador.ToString());
    			///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
    			ContextoImp.MoveTo(20, filas-2);//623
				ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
    			decimal totaldelmov =subtotaldelmov - deducible - coaseguro - totaldesc - totabono - totpago + honorarios;//desctotal;
    			contador+=1;
    			filas-=10;
    			salto_pagina(ContextoImp,trabajoImpresion,contador);
				    	
    			ContextoImp.MoveTo(381.5, filas) ;		ContextoImp.Show("SUBTOTAL AL "+PorcentIVA.ToString()+"%"); 
    			ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL "+PorcentIVA.ToString()+"%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt15.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt15.ToString("C").PadLeft(10)); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUBTOTAL AL 0%");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 0%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt0.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt0.ToString("C").PadLeft(10));
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA AL "+PorcentIVA.ToString()+"%");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA AL "+PorcentIVA.ToString()+"%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(sumaiva.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(sumaiva.ToString("C").PadLeft(10)); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUB-TOTAL");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUB-TOTAL");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotaldelmov.ToString("C").PadLeft(10));
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotaldelmov.ToString("C").PadLeft(10));
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
			
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("MENOS DEDUCIBLE");	
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("MENOS DEDUCIBLE");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(deducible.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(deducible.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("MENOS COASEGURO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("MENOS COASEGURO");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(coaseguro.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(coaseguro.ToString("C").PadLeft(10)+" -");
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("MENOS DESCUENTO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("MENOS DESCUENTO");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totaldesc.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaldesc.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL PAGO");
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL PAGO");
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totabono.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totabono.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL ABONO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL ABONO");
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totpago.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totpago.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("HONORARIO MEDICO");
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("HONORARIO MEDICO");
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(honorarios.ToString("C").PadLeft(10)+" +"); 
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(honorarios.ToString("C").PadLeft(10)+" +"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totaldelmov.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaldelmov.ToString("C").PadLeft(10)); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				ContextoImp.ShowPage();
				//Console.WriteLine("contador totales: "+contador.ToString());
				//genera_totales(ContextoImp, trabajoImpresion,contador,subtotaldelmov,subt15,subt0,sumaiva,deducible,coaseguro, totaldesc);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Este folio no contiene productos aplicados \n"+
							"existentes para que el procedimiento se muestre \n");
				msgBoxError.Run ();
				msgBoxError.Destroy();
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
 }*/

