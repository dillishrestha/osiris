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
		int numpage = 1;		
		int comienzo_linea = 0;
		int separacion_linea = 10;  		
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		        
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
		string salahabitacion;
		string especialidad_doctor;
		string dignostico_paciente;
		bool apl_desc_siempre = true;
		bool apl_desc;
		
		int contador = 1;
		
		
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
						string cirugia_,string empresapac_,int idtipopaciente_,string query, string salahabitacion_,string especialidad_doctor_,string dignostico_paciente_)
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
			salahabitacion = salahabitacion_;
			especialidad_doctor = especialidad_doctor_;
			dignostico_paciente = dignostico_paciente_;
			
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
							cr.MoveTo(470*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Total de Desc.");												Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(sumadesc.ToString("N").PadLeft(10)+" -");	Pango.CairoHelper.ShowLayout (cr, layout);
							contador+=1;
							salto_pagina(cr,layout,contador);
							comienzo_linea += separacion_linea;
							cr.MoveTo(470*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Total de Area");											Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(530*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(totaladm.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
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
					cr.MoveTo(470*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Total de Desc.");												Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(sumadesc.ToString("N").PadLeft(10)+" -");	Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					comienzo_linea += separacion_linea;
					cr.MoveTo(470*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Total de Area");											Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(totaladm.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					// Console.WriteLine("contador antes de los totales: "+contador.ToString());
					// TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
					decimal totaldelmov =subtotaldelmov - deducible - coaseguro - totaldesc - totabono - totpago + honorarios;//desctotal;
					
					comienzo_linea += separacion_linea;		    	
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("SUBTOTAL AL "+PorcentIVA.ToString()+"%");		Pango.CairoHelper.ShowLayout (cr, layout);	
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(subt15.ToString("N").PadLeft(10)); 						Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("SUBTOTAL AL 0%");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(subt0.ToString("N").PadLeft(10));Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
		
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("IVA AL "+PorcentIVA.ToString()+"%");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(sumaiva.ToString("N").PadLeft(10)); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("SUB-TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(subtotaldelmov.ToString("N").PadLeft(10));Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
			
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("MENOS DEDUCIBLE");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(deducible.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("MENOS COASEGURO");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(coaseguro.ToString("N").PadLeft(10)+" -");Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("MENOS DESCUENTO");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(totaldesc.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
				
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("TOTAL PAGO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(totabono.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
					
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("TOTAL ABONO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(totpago.ToString("N").PadLeft(10)+" -"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
									
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("HONORARIO MEDICO");Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(honorarios.ToString("N").PadLeft(10)+" +"); Pango.CairoHelper.ShowLayout (cr, layout);
					contador+=1;
					salto_pagina(cr,layout,contador);
								
					comienzo_linea += separacion_linea;
					cr.MoveTo(382*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(totaldelmov.ToString("N").PadLeft(10)); Pango.CairoHelper.ShowLayout (cr, layout);
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
			
			cr.MoveTo(001*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText(classpublic.nombre_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText(classpublic.direccion_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(470*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText("FOLIO DE ATENCION");				Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText(classpublic.telefonofax_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			//cr.MoveTo(001*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 12.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(210*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText("PROCEDIMIENTO DE COBRANZA");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.SetSourceRGB(150,0,0);  // Cambio de color a Rojo
			cr.MoveTo(500*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);					layout.SetText(folioservicio.ToString());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.SetSourceRGB(0,0,0);		// Cambio de color a Negro
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;			
			// Cambiando el tamaño de la fuente
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;			
			cr.MoveTo(224*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);				layout.SetText("DATOS GENERALES DEL PACIENTE");	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;							
			// Cambiando el tamaño de la fuente
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);
			layout.FontDescription = desc;
			cr.MoveTo(001*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("INGRESO: "+ fecha_admision.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("EGRESO: "+ fechahora_alta.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);														layout.SetText("PID: "+PidPaciente.ToString()+"    Nombre: "+ nombre_paciente.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(330*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("F. de Nac: "+fecha_nacimiento.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(450*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpac.ToString());											Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra Normal
			comienzo_linea += separacion_linea;
			cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Direccion: "+dir_pac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);														layout.SetText("Tel. Pac.: "+telefono_paciente.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(380*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Nº de habitacion: "+salahabitacion);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			if((string) tipo_paciente == "Asegurado"){				
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo de paciente:  "+tipo_paciente+"   Aseguradora : "+aseguradora+"   Poliza: ");				Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo de paciente:  "+tipo_paciente+"   Empresa: "+empresapac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			}
			comienzo_linea += separacion_linea;
			if(doctor.ToString().Trim() == ""){
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);									layout.SetText("Medico: ");	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(250*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Especialidad:"+especialidad_doctor);	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Diagnostico: "+dignostico_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Medico: "+doctor.ToString()+"    Especialidad: "+especialidad_doctor);	Pango.CairoHelper.ShowLayout (cr, layout);
				comienzo_linea += separacion_linea;
				cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Diagnostico: "+dignostico_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
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
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(descrp_admin.ToString()+"  "+fech.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
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
			cr.MoveTo(080*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("CANT.");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(025*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("CLAVE.");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(108*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText(tipoproducto);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(385*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("PRECIO");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(430*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("SUB-TOTAL");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(493*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(545*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("TOTAL");			Pango.CairoHelper.ShowLayout (cr, layout);
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
			cr.MoveTo(001*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(idproducto_);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(080*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(cantidadaplicada_);		Pango.CairoHelper.ShowLayout (cr, layout);
			if(datos_.Length > 61)	{				
				cr.MoveTo(110*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText((string) datos_.Substring(0,60));					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(110*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) datos_);							Pango.CairoHelper.ShowLayout (cr, layout);
			} 
			cr.MoveTo(380*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(preciounitario_);Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(430*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(subtotal_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(ivaprod_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(total_.ToString("N").PadLeft(10));			Pango.CairoHelper.ShowLayout (cr, layout);
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