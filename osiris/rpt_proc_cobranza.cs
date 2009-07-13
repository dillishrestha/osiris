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
	public class proc_cobranza
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
		public int id_tipopaciente = 0;
		public string tipo_paciente;
		public string aseguradora;
		public string dir_pac;
		public string empresapac;
		public bool apl_desc_siempre = true;
		public bool apl_desc;
		
		public int filas=635;
		public int contador = 1;
		public int numpage = 1;
		
		//query de rango de fechas
		public string query_todo = " ";
		public string query_rango_fechas = " "; 
		
		public int idadmision_ = 0;
		public int idproducto = 0;
		public string datos = "";
		public string fcreacion = "";
		public decimal porcentajedes =  0;
		public decimal descsiniva = 0;
		public decimal ivadedesc = 0;
		public decimal descuento = 0;
		public decimal ivaprod = 0;
		public decimal subtotal = 0;
		public decimal subtotalelim = 0;
		public decimal subt15 = 0;
		public decimal subt15elim = 0;
		public decimal subt0 = 0;
		public decimal subt0elim = 0;
		public decimal sumadesc = 0;
		public decimal sumadescelim = 0;
		public decimal sumaiva = 0;
		public decimal sumaivaelim = 0;
		public decimal total = 0;
		public decimal totalelim = 0;
		public decimal totaladm = 0;
		public decimal totaladmelim = 0;
		public decimal totaldesc = 0;
		public decimal subtotaldelmov = 0;
		public decimal deducible = 0;
		public decimal coaseguro = 0;
		//public int contdesc = 0;
		//agrega abonos y pagos honorarios
		public decimal totabono = 0;
		public decimal totpago = 0;
		public decimal honorarios = 0;
				
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public proc_cobranza ( int PidPaciente_ , int folioservicio_,string _nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string query)
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
			id_tipopaciente = idtipopaciente_;
			tipo_paciente = entry_tipo_paciente_;//
			aseguradora = entry_aseguradora_;//
			edadpac = edadpac_;//
			fecha_nacimiento = fecha_nacimiento_;//
			dir_pac = dir_pac_;//
			empresapac = empresapac_;//
			query_rango_fechas = query;	
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "PROCEDIMIENTO COBRANZA", 0);
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
                new PrintJobPreview(trabajo, "PROCEDIMIENTO COBRANZA").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Hospital Santa Cecilia");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion: Isacc Garza #200 Ote. Centro Monterrey, NL.");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:(81) 81-25-56-10");
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
					"hscmty_erp_cobros_deta.folio_de_servicio,hscmty_erp_cobros_deta.pid_paciente, "+ 
					"hscmty_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
					"hscmty_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"hscmty_grupo_producto.descripcion_grupo_producto, "+
					"hscmty_productos.id_grupo_producto,  "+
					"to_char(hscmty_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
					"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
					"to_char(hscmty_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
					"to_char(hscmty_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
					"to_char(hscmty_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, "+
					"to_char(hscmty_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario, "+
					"ltrim(to_char(hscmty_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
					"to_char(hscmty_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
					"to_char(hscmty_erp_cobros_deta.cantidad_aplicada * hscmty_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
					//"to_char(hscmty_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+					
					"to_char(hscmty_productos.precio_producto_publico,'999999999.99999') AS preciopublico, "+
					"to_char(hscmty_erp_cobros_enca.total_abonos,'999999999.999') AS totalabono, "+ 
					"to_char(hscmty_erp_cobros_enca.honorario_medico,'999999999.999') AS honorario, "+
					"to_char(hscmty_erp_cobros_enca.total_pago,'999999999.999') AS totalpago "+
					"FROM "+
					"hscmty_erp_cobros_deta,hscmty_erp_cobros_enca,hscmty_his_tipo_admisiones,hscmty_productos,hscmty_grupo_producto "+
					"WHERE "+
					"hscmty_erp_cobros_deta.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones "+
					"AND hscmty_erp_cobros_deta.id_producto = hscmty_productos.id_producto  "+ 
					"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
					"AND hscmty_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
					"AND hscmty_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"' "+
		        	"AND hscmty_erp_cobros_deta.eliminado = 'false' ";
		try 
        {
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	comando.CommandText = query_todo + query_rango_fechas + "ORDER BY  to_char(hscmty_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, hscmty_erp_cobros_deta.id_tipo_admisiones ASC, hscmty_productos.id_grupo_producto,hscmty_erp_cobros_deta.id_secuencia; ";
			Console.WriteLine(query_todo + query_rango_fechas + "ORDER BY  to_char(hscmty_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, hscmty_erp_cobros_deta.id_tipo_admisiones ASC, hscmty_productos.id_grupo_producto,hscmty_erp_cobros_deta.id_secuencia; ");			
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
					ivaprod = (subtotal*15)/100;
					subt15 += subtotal;
				}else{
					subt0 += subtotal;
					ivaprod = 0;
				}
				sumaiva += ivaprod;
				total = subtotal + ivaprod;
				if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0){
					descsiniva = (subtotal*(porcentajedes/100));
					ivadedesc =descsiniva*15/100;
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
						ivaprod = (subtotal*15)/100;
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
							ivadedesc =descsiniva*15/100;
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
							ivadedesc =descsiniva*15/100;
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
				    	
    			ContextoImp.MoveTo(381.5, filas) ;		ContextoImp.Show("SUBTOTAL AL 15%"); 
    			ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 15%");	
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
		
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA AL 15%");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA AL 15%");	
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
 }    
}
