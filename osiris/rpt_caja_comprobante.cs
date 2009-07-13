///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@gmail.com
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
// Programa		: hscmty.cs
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: rpt_proc_cobranza.cs
using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class 	caja_comprobante
	{
		public string connectionString = "Server=localhost;" +
        	    	                     "Port=5432;" +
            	    	                 "User ID=admin;" +
                	    	             "Password=1qaz2wsx;";
        public string nombrebd;
		public int PidPaciente = 0;
		public int folioservicio = 0;
		public string fecha_admision;
		public string fechahora_alta;
		public string nombre_paciente;
		public string telefono_paciente;
		public string doctor;
		public string cirugia;
		public string fecha_nacimiento;
		public string edadpac;
		public string tipo_paciente;
		public int id_tipopaciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public bool apl_desc_siempre = true;
		public bool apl_desc;
		public string nombrecajero;
		public string totalabonos;
		
		
		// traductor de numeros a letras
		public string[] sUnidades = {"", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", 
									"once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinte", 
									"veintiún", "veintidos", "veintitres", "veinticuatro", "veinticinco", "veintiseis", "veintisiete", "veintiocho", "veintinueve"};
 		public string[] sDecenas = {"", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"};
 		public string[] sCentenas = {"", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"};
  		public string sResultado = "";				
		public int filas=645;
				
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public caja_comprobante ( int PidPaciente_ , int folioservicio_,string _nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_, int idtipopaciente_, string nombrecajero_, string totalabonos_)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;//
			fechahora_alta = entry_fechahora_alta_;//
			nombre_paciente = entry_nombre_paciente_;//
			telefono_paciente = entry_telefono_paciente_;//
			doctor = entry_doctor_;//
			cirugia = cirugia_;//
			tipo_paciente = entry_tipo_paciente_;//
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			nombrecajero = nombrecajero_;
			totalabonos = totalabonos_;			
		
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "COMPROBANTE DE CAJA", 0);
        	int         respuesta = dialogo.Run ();
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
                      	new PrintJobPreview(trabajo, "COMPROBANTE DE CAJA").Show();
                        break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(500.5, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			ContextoImp.MoveTo(501, 740);		ContextoImp.Show(DateTime.Now.ToString("dd-MM-yyyy") );
			
      		Gnome.Print.Setfont (ContextoImp, fuente9);
			ContextoImp.MoveTo(79.5, 710);		ContextoImp.Show(nombre_paciente.ToString());
			ContextoImp.MoveTo(80, 710);		ContextoImp.Show(nombre_paciente.ToString());
						
			ContextoImp.MoveTo(80, 690);		ContextoImp.Show(dir_pac.ToString());
			
			ContextoImp.MoveTo(80, 670);		ContextoImp.Show(telefono_paciente.ToString());
			
			Gnome.Print.Setrgbcolor(ContextoImp,150,0,0);//cambio a color rojo obscuro
			
			ContextoImp.MoveTo(134.5, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			ContextoImp.MoveTo(135, 670);		ContextoImp.Show("PID: "+PidPaciente.ToString());
			
			ContextoImp.MoveTo(189.5, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			ContextoImp.MoveTo(190, 670);		ContextoImp.Show("EDAD: "+edadpac.ToString());
			
			ContextoImp.MoveTo(260.5, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			ContextoImp.MoveTo(261, 670);		ContextoImp.Show("Folio de Servicio: "+folioservicio.ToString());
			
			ContextoImp.MoveTo(395.5, 670);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());
			ContextoImp.MoveTo(396, 670);		ContextoImp.Show("Fecha Admision: "+fecha_admision.ToString());			
			Gnome.Print.Setrgbcolor(ContextoImp,0,0,0);//cambio a color negro
			ContextoImp.MoveTo(134.5, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(135, 670);		ContextoImp.Show("PID: ");
			ContextoImp.MoveTo(189.5, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(190, 670);		ContextoImp.Show("EDAD: ");
			ContextoImp.MoveTo(260.5, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(261, 670);		ContextoImp.Show("Folio de Servicio: ");
			ContextoImp.MoveTo(395.5, 670);		ContextoImp.Show("Fecha Admision: ");
			ContextoImp.MoveTo(396, 670);		ContextoImp.Show("Fecha Admision: ");			
		}
    
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(90.5, filas);		ContextoImp.Show("SERVICIO "+descrp_admin);//635
		ContextoImp.MoveTo(91, filas);			ContextoImp.Show("SERVICIO "+descrp_admin);//635
		filas+=20;
	}
	
	void imprime_subtitulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string tipoproducto,float total)
	{
		Gnome.Print.Setfont (ContextoImp, fuente7);
		if(tipoproducto.Length > 90)
		{
			ContextoImp.MoveTo(100.5, filas);	ContextoImp.Show(tipoproducto.Substring(0,90)); 
			ContextoImp.MoveTo(515, filas);		ContextoImp.Show(total.ToString("C"));
		}else{
			ContextoImp.MoveTo(101, filas);		ContextoImp.Show(tipoproducto.ToString());
			ContextoImp.MoveTo(515, filas);		ContextoImp.Show(total.ToString("C"));
		} 
		filas-=15;
		Gnome.Print.Setfont (ContextoImp, fuente9);
    }
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
	
		for (int i1=0; i1 <= 4; i1++)//5 veces para tasmaño carta
		{		
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try 
	        {
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText ="SELECT "+
						"hscmty_erp_cobros_deta.folio_de_servicio,hscmty_erp_cobros_deta.pid_paciente, "+ 
						"hscmty_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
						"hscmty_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"hscmty_grupo_producto.descripcion_grupo_producto, "+
						"hscmty_productos.id_grupo_producto,  "+
						"to_char(hscmty_erp_cobros_deta.porcentage_descuento,999.99) AS porcdesc, "+
						"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
						"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
						"to_char(hscmty_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
						"to_char(hscmty_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
						"to_char(hscmty_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
						"ltrim(to_char(hscmty_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
						"to_char(hscmty_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
						//"to_char(hscmty_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(hscmty_erp_cobros_deta.cantidad_aplicada * hscmty_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
						"to_char(hscmty_productos.precio_producto_publico,'999999999.99999') AS preciopublico "+
						"FROM "+ 
						"hscmty_erp_cobros_deta,hscmty_his_tipo_admisiones,hscmty_productos,hscmty_grupo_producto "+
						"WHERE "+
						"hscmty_erp_cobros_deta.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
						"AND hscmty_erp_cobros_deta.id_producto = hscmty_productos.id_producto  "+ 
						"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
						"AND hscmty_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
			        	"AND hscmty_erp_cobros_deta.eliminado = 'false' "+
			        	" ORDER BY  hscmty_erp_cobros_deta.id_tipo_admisiones ASC, hscmty_productos.id_grupo_producto;";
	        			//"AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'dd') >= '"+DateTime.Now.ToString("dd")+"'  AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'dd') <= '"+DateTime.Now.ToString("dd")+"' "+
						//"AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'MM') >= '"+DateTime.Now.ToString("MM")+"' AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
						//"AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' AND to_char(hscmty_erp_movcargos.fechahora_admision_registro,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " ;
	        	
	        	//Console.WriteLine("query caja "+comando.CommandText.ToString());
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");					
				filas=635;
	        	int idadmision_ = 0;
	        	int idproducto = 0;
	        	string datos = "";
	        	float porcentajedes =  0;
				float descsiniva = 0;
				float ivadedesc = 0;
				float descuento = 0;
				float ivaprod = 0;
				float subtotal = 0;
				float subt15 = 0;
				float subt0 = 0;
				float sumadesc = 0;
				float sumaiva = 0;
				float total = 0;
				float totalgrupo = 0;
				float totaladm = 0;
				float totaldesc = 0;
				float totaldelmov =0;
				float subtotaldelmov = 0;
				//float deducible = 0;
				//float coaseguro = 0;
					        		
	        	if (lector.Read())
	        	{	
	        		//ContextoImp.BeginPage("Pagina 1");
	        		//VARIABLES
	        		if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101 && (bool) apl_desc_siempre == true
						||(int) lector["idadmisiones"] == 300 && (int)id_tipopaciente == 101 && (bool) apl_desc_siempre == true
						||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101 && (bool) apl_desc_siempre == true){
							apl_desc = true;
					}else{
						if(apl_desc_siempre == true){
							apl_desc = false;
							apl_desc_siempre = false;
						}
					}
					datos = (string) lector["descripcion_producto"];
					subtotal = float.Parse((string) lector["ppcantidad"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
					//ivaprod = float.Parse((string) lector["ivaproducto"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
					porcentajedes =  float.Parse((string) lector["porcdesc"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
					if((bool) lector["aplicar_iva"]== true){
						ivaprod = (subtotal*15)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;
					if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0.00){
						descsiniva = (subtotal*(porcentajedes/100));
						ivadedesc =descsiniva*15/100;
						descuento = descsiniva+ivadedesc;
						//Console.WriteLine(descuento.ToString("C"));
	        		}else{
	        			descuento = float.Parse("0.00", System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
	        		}
	        		sumadesc +=descuento;
	        		//Console.WriteLine("Total: "+total.ToString("C").Replace(",",".")+" porcentaje: "+porcentajedes.ToString("C").Replace(",",".")+"\n"+
	        		//"%  descuento: "+descuento.ToString("C").Replace(",",".")+" sumadesc: "+sumadesc.ToString("C").Replace(",","."));
					totaldesc +=descuento;
					if (apl_desc == false){
						totaldesc = 0;
					}
					totalgrupo +=total;
					totaladm +=total;
					totaldelmov +=total;
					subtotaldelmov +=total;
					
	        		
	        		/////DATOS DE PRODUCTOS
	      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
	     		   	imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
	        		if((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400)//400
	        		{
	        			filas-=30;
	        			imprime_subtitulo(ContextoImp,trabajoImpresion,datos,total);
	        			filas+=35;
	        		}
	       		 	
	        		//DATOS TABLA
					idadmision_ = (int) lector["idadmisiones"];
	        		idproducto = (int) lector["id_grupo_producto"];
					//Console.WriteLine(contador.ToString());
					//Console.WriteLine(contdesc.ToString()+"Tipo pac: "+(int) lector["id_tipo_paciente"]+" aplica descuento: "+apl_desc.ToString()+" aplica siempre: "+apl_desc_siempre.ToString());
	        		while (lector.Read())
	        		{
	        			if (idadmision_ != (int) lector["idadmisiones"])
	        			{	
	        				filas-=30;
			        		ContextoImp.MoveTo(514.7, filas);			ContextoImp.Show(totaladm.ToString("C"));
			        		ContextoImp.MoveTo(515, filas);				ContextoImp.Show(totaladm.ToString("C"));
	        				filas+=30;
	        				//Console.WriteLine("cambio de admision"+" "+(string) lector["descripcion_admisiones"]);
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
							////VARIABLES
	        				datos = (string) lector["descripcion_producto"];
							//cantidadaplicada = float.Parse((string) lector["cantidadaplicada"]);
							subtotal = float.Parse((string) lector["ppcantidad"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							//ivaprod = float.Parse((string) lector["ivaproducto"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							porcentajedes =  float.Parse((string) lector["porcdesc"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							if((bool) lector["aplicar_iva"]== true){
								ivaprod = (subtotal*15)/100;
								subt15 += subtotal;
							}else{
								subt0 += subtotal;
								ivaprod = 0;
							}
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
							sumadesc = 0;
							if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0.00){
								descsiniva = (subtotal*(porcentajedes/100));
								ivadedesc =descsiniva*15/100;
								descuento = descsiniva+ivadedesc;
								//Console.WriteLine(descuento.ToString("C"));
			        		}else{
			        			descuento = float.Parse("0.00", System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			        		}
	        				sumadesc +=descuento;
	        				//Console.WriteLine("Total: "+total.ToString("C").Replace(",",".")+" porcentaje: "+porcentajedes.ToString("C").Replace(",",".")+"\n"+
	        				//"%  descuento: "+descuento.ToString("C").Replace(",",".")+" sumadesc: "+sumadesc.ToString("C").Replace(",","."));
							totaladm = 0;
							totaldesc +=descuento;
							if (apl_desc == false){
								totaldesc = 0;
							}
							totalgrupo +=total;
							totaladm +=total;
							totaldelmov +=total;
							subtotaldelmov +=total;
							
	        				idadmision_ = (int) lector["idadmisiones"];
	        				//ContextoImp.MoveTo(480, filas);			ContextoImp.Show("Total de Area");
	        				////DATOS DE PRODUCTOS
	        				datos = (string) lector["descripcion_producto"];
	        				filas-=30;
	        				imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
	        				if((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400)//400
	        				{
	        					filas-=30;
	        					imprime_subtitulo(ContextoImp,trabajoImpresion,datos,total);
	        					filas+=35;
	        				}
							
	///////////////////////////////// SI LA ADMISION SIGUE SIENDO LA MISMA HACE ESTO://////////////////////////////////////////
						}else{
							 
							///VARIABLES
							datos = (string) lector["descripcion_producto"];
							subtotal = float.Parse((string) lector["ppcantidad"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							//ivaprod = float.Parse((string) lector["ivaproducto"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							porcentajedes =  float.Parse((string) lector["porcdesc"], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
							if((bool) lector["aplicar_iva"]== true){
								ivaprod = (subtotal*15)/100;
								subt15 += subtotal;
							}else{
								subt0 += subtotal;
								ivaprod = 0;
							}
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
							if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0.00){
								descsiniva = (subtotal*(porcentajedes/100));
								ivadedesc =descsiniva*15/100;
								descuento = descsiniva+ivadedesc;
								//Console.WriteLine(descuento.ToString("C"));
			        		}else{
			        			descuento = float.Parse("0.00", System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			        		}
	        				sumadesc +=descuento;
	        				//Console.WriteLine("Total: "+total.ToString("C").Replace(",",".")+" porcentaje: "+porcentajedes.ToString("C").Replace(",",".")+"\n"+
	        				//"%  descuento: "+descuento.ToString("C").Replace(",",".")+" sumadesc: "+sumadesc.ToString("C").Replace(",","."));
							totaldesc +=descuento;
							if (apl_desc == false){
								totaldesc = 0;
							}
							totalgrupo +=total;
							totaladm +=total;
							totaldelmov +=total;
							subtotaldelmov +=total;
							
							//VARIABLES
	        					if((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400)//400
	        					{
	        						filas-=30;
	        				   		imprime_subtitulo(ContextoImp,trabajoImpresion,datos,total);
	        						filas+=35;
	        					}
	        					
	        					if (idproducto != (int) lector["id_grupo_producto"])
	        				    {
	        				    	totalgrupo = 0;
									idproducto = (int) lector["id_grupo_producto"];
								}
						}
					}//termino de ciclo
					
					float apagar = (totaldelmov-totaldesc);
					
        			filas-=30;
        			if (float.Parse(totalabonos) > 0){
        				ContextoImp.MoveTo(90.5, 495);				ContextoImp.Show("SUB-TOTAL");
        				ContextoImp.MoveTo(515, 505);				ContextoImp.Show(apagar.ToString("C"));
        				
        				apagar = (totaldelmov-totaldesc) - float.Parse(totalabonos);
        				ContextoImp.MoveTo(90.5, 495);				ContextoImp.Show("TOTAL DE ABONOS");
        				ContextoImp.MoveTo(515, 495);				ContextoImp.Show(float.Parse(totalabonos).ToString("C"));
        			}
        			
        			ContextoImp.MoveTo(514.7, filas);			ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        			ContextoImp.MoveTo(515, filas);				ContextoImp.Show(totaladm.ToString("C").PadLeft(10));
        			
        			ContextoImp.MoveTo(110, 475);				ContextoImp.Show(traduce_numeros(apagar.ToString("F")));
        			ContextoImp.MoveTo(90.5, 515);				ContextoImp.Show("Total descuento");
        			ContextoImp.MoveTo(515.7, 515);				ContextoImp.Show(totaldesc.ToString("C").PadLeft(10)+" -");
		       		
					ContextoImp.MoveTo(514.7, 465);				ContextoImp.Show(apagar.ToString("C").PadLeft(10));
					ContextoImp.MoveTo(515, 465);				ContextoImp.Show(apagar.ToString("C").PadLeft(10));
					 
					filas-=10;
					ContextoImp.MoveTo(110, 425);				ContextoImp.Show("ATENDIO "+nombrecajero.ToUpper());	
					ContextoImp.MoveTo(514.7, 425);				ContextoImp.Show(apagar.ToString("C").PadLeft(10)); 
					ContextoImp.MoveTo(515, 425);				ContextoImp.Show(apagar.ToString("C").PadLeft(10)); 
					filas-=10;
					ContextoImp.ShowPage();
					
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
			conexion.Close();
		}
	}
	
	public string traduce_numeros (string sNumero) {
			double dNumero;
			double dNumAux = 0;
			char x;
			string sAux;
			
			sResultado = " ";
			try {
				dNumero = Convert.ToDouble (sNumero);
			}
			catch {				
				return "";
			}
 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
  
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0)
				sResultado += ObtenerDecimales (sNumero);
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
		 
		public string ConvertirCadena (double dNumero) {
			double dNumAux = 0;
			char x;
			string sAux;
			 
			sResultado = " ";
 
			if (dNumero > 999999999999)
				return "";
 
			if (dNumero > 999999999) {
				dNumAux = dNumero % 1000000000000;
				sResultado += Numeros (dNumAux, 1000000000) + " mil ";
			}
 
			if (dNumero > 999999) {
				dNumAux = dNumero % 1000000000;
				sResultado += Numeros (dNumAux, 1000000) + " millones ";
			}
 
			if (dNumero > 999) {
				dNumAux = dNumero % 1000000;
				sResultado += Numeros (dNumAux, 1000) + " mil ";
			}
 
			dNumAux = dNumero % 1000;	
			sResultado += Numeros (dNumAux, 1);
 
 
			//Enseguida verificamos si contiene punto, si es así, los convertimos a texto.
			sAux = dNumero.ToString();
 
			if (sAux.IndexOf(".") >= 0)
				sResultado += ObtenerDecimales (sAux);
 
			//Las siguientes líneas convierten el primer caracter a mayúscula.
			sAux = sResultado;
			x = char.ToUpper (sResultado[1]);
			sResultado = x.ToString ();
 
			for (int i = 2; i<sAux.Length; i++)
				sResultado += sAux[i].ToString();
 
			return sResultado;
		}
 
		private string Numeros (double dNumAux, double dFactor) {
			double dCociente = dNumAux / dFactor;
			double dNumero = 0;
			int iNumero = 0;
			string sNumero = "";
			string sTexto = "";
 
			if (dCociente >= 100){
				dNumero = dCociente / 100;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				sTexto  +=  this.sCentenas [iNumero] + " ";
			}
 
			dCociente = dCociente % 100;
			if (dCociente >= 30){
				dNumero = dCociente / 10;			
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += this.sDecenas [iNumero] + " ";
 
				dNumero = dCociente % 10;
				sNumero = dNumero.ToString();
				iNumero = int.Parse (sNumero[0].ToString());
				if (iNumero > 0)
					sTexto  += "y " + this.sUnidades [iNumero] + " ";
			}else{
				dNumero = dCociente;	
				sNumero = dNumero.ToString();
				if (sNumero.Length > 1)
					if (sNumero[1] != '.')
						iNumero = int.Parse (sNumero[0].ToString() + sNumero[1].ToString());
					else
						iNumero = int.Parse (sNumero[0].ToString());
				else
					iNumero = int.Parse (sNumero[0].ToString());
				sTexto  += this.sUnidades[iNumero] + " ";
			}
			return sTexto;
		}		

		private string ObtenerDecimales (string sNumero) {
			string[] sNumPuntos;
			string sTexto = "";
			double dNumero = 0;
			sNumPuntos = sNumero.Split('.');
    		dNumero = Convert.ToDouble(sNumPuntos[1]);
			sTexto = "peso "+dNumero.ToString().Trim()+"/100 M.N."; 
			//sTexto = "peso con " + Numeros(dNumero,1);
			return sTexto;
		}
 }    
}
