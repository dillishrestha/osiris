// created on 18/04/2007 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
//				  Daniel Olivares C. (Adecuaciones y reprogramacion) arcangeldoc@gmail.com 05/05/2007
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
using Glade;
using GtkSharp;

namespace osiris
{
	public class proc_totales
	{
		string connectionString;
        int PidPaciente = 0;
		int folioservicio = 0;
		int id_tipopaciente = 0;
		string nombrebd;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		string tipo_paciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		string query_rango_fechas = "";
		
		int filas=635;
		int contador = 1;
		int numpage = 1;
		
		////////variables que utilizo dentro del ciclo
		int idadmision_ = 0;					//tipo de admision en donde se realizaron los cargos...
		int idgrupoproducto = 0;					// el codigo del producto
		decimal precio_por_cantidad = 0;		//esta variable se utiliza para ir guwerdandop el precio de un producto dependiendo de cuanto se aplico de este
		decimal iva_del_grupo = 0;					//es un valor en donde se van a ir sumando cada iva que se le aplica al producto
		decimal porcentagedesc = 0;			//es el el descuento en porciento si es que existe un descuento
		decimal descuento_neto = 0;			// valor desc sin iva
		decimal descurento_del_grupo = 0;		//el descuento que se aplica en cada grupo de productos
		decimal iva_de_descuento = 0;			// valor iva del descuento 
		decimal descuento_del_grupo = 0;		// suma del iva del desc y del desc neto
		decimal subtotal_del_grupo = 0;		//subtotal del grupo de productos
		decimal subtotal_al_15_grupo = 0;		//es el subtotal de los productos que contienen iva en un grupo de productos
		decimal subtotal_al_0_grupo = 0;		//es el subtotal de los productos que no contienen iva en un grupo de productos
		decimal subtotal_al_15 = 0;			//es el subtotal de los productos que contienen iva en todo el movimiento
		decimal subtotal_al_0 = 0;			//es el subtotal de los productos que no contienen iva en todo el movimiento
		decimal deducible = 0;					//es el dedicible de impuestos plicado en la factura
		decimal coaseguro = 0;					//es el valor de coaseguro que se descuenta del total facturado
		decimal total_del_grupo = 0;			//precio total del grupo de productos
		decimal total_de_iva = 0;				//suma de todos los ivas de todos los lugares y grupos de productos
		decimal total_de_descuento_neto =0;	//es el descuento neto de facturacion
		decimal total_de_iva_descuento =0;	//es el iva del descuento neto de facturacion
		decimal total_descuento=0;			//es la la suma del descuento neto y el iva del descuento neto de facturacion
		/// restar abonos y pago final
		decimal totabono = 0;
		decimal totpago = 0;
		decimal honorarios = 0;
		decimal valoriva;
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public proc_totales ( int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string query)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
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
			query_rango_fechas = query;
			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = decimal.Parse(classpublic.ivaparaaplicar);	
					
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "RESUMEN DE FACTURA", 0);
        	
        	//gnome_print_job_new  trabajo   = new gnome_print_job_new();
        	//PrintDialog dialogo   = new PrintDialog (trabajo, "RESUMEN DE FACTURA", 0);
        	
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
                new PrintJobPreview(trabajo, "RESUMEN DE FACTURA").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      	
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
      		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(19.7, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(20, 770);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(19.7, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(20, 760);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(19.7, 750);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(20, 750);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			
			//ContextoImp.MoveTo(484.7, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			//ContextoImp.MoveTo(485, 770);			ContextoImp.Show("Fo-tes-11/Rev.02/20-mar-07");
			   			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(220.5, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
			ContextoImp.MoveTo(221, 740);			ContextoImp.Show("RESUMEN DE FACTURA");
							
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(470.5, 755);			ContextoImp.Show("FOLIO DE ATENCION");
			ContextoImp.MoveTo(471, 755);			ContextoImp.Show("FOLIO DE ATENCION");
							
			Gnome.Print.Setfont (ContextoImp, fuente12);
			Gnome.Print.Setrgbcolor(ContextoImp, 150,0,0);
			ContextoImp.MoveTo(520.5,740 );			ContextoImp.Show( folioservicio.ToString());
			ContextoImp.MoveTo(521, 740);			ContextoImp.Show( folioservicio.ToString());
					
			Gnome.Print.Setfont (ContextoImp, fuente36);
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);
											    			
			////////////DATOS GENERALES PACIENTE//////////////////
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(224.5, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			ContextoImp.MoveTo(225, 720);			ContextoImp.Show("DATOS GENERALES DEL PACIENTE");
			
			Gnome.Print.Setfont (ContextoImp, fuente8);
			ContextoImp.MoveTo(444.7, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(445, 720);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
								
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
			if(doctor.ToString() == " " || doctor.ToString() == ""){
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: ");
				ContextoImp.MoveTo(250, 660);			ContextoImp.Show("Especialidad:");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}else{
				ContextoImp.MoveTo(20, 660);			ContextoImp.Show("Medico: "+doctor.ToString()+"           Especialidad:  ");
				ContextoImp.MoveTo(20, 650);			ContextoImp.Show("Cirugia/Diagnostico: "+cirugia.ToString());
			}
		}
      
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin)
		{
    	Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString().ToUpper());//+"  "+fech.ToString());//635
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		//genera_lineac(ContextoImp, trabajoImpresion);
		filas-=10;
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
	
	void imprime_subtitulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string grupodelproducto)
	{
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(29.5, filas);				ContextoImp.Show(grupodelproducto);//625
		ContextoImp.MoveTo(30, filas);				ContextoImp.Show(grupodelproducto);//625
		ContextoImp.MoveTo(217.5, filas);			ContextoImp.Show("SUBTOTAL");//24.5,625
		ContextoImp.MoveTo(218, filas);				ContextoImp.Show("SUBTOTAL");//25,625
		ContextoImp.MoveTo(273.5, filas);			ContextoImp.Show("IVA");//24.5,625
		ContextoImp.MoveTo(274, filas);				ContextoImp.Show("IVA");//25,625
		ContextoImp.MoveTo(327.5, filas);			ContextoImp.Show("TOTAL");//625
		ContextoImp.MoveTo(328, filas);				ContextoImp.Show("TOTAL");//625
		ContextoImp.MoveTo(382.6, filas);			ContextoImp.Show("DESC NETO");//625
		ContextoImp.MoveTo(383, filas);				ContextoImp.Show("DESC NETO");//625
		ContextoImp.MoveTo(437.6, filas);			ContextoImp.Show("IVA DESC");//625
		ContextoImp.MoveTo(438, filas);				ContextoImp.Show("IVA DESC");//625
		ContextoImp.MoveTo(492.6, filas);			ContextoImp.Show("TOTAL DESC");//625
		ContextoImp.MoveTo(493, filas);				ContextoImp.Show("TOTAL DESC");//625
		filas-=10;		
		Gnome.Print.Setfont (ContextoImp, fuente7);
		//ContextoImp.MoveTo(20, filas-2);//623
		//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		//filas-=10;
		}
   
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
		{
		//Console.WriteLine("contador antes del if: "+contador_.ToString());
        if (contador_ >= 58 )
        {
        	numpage +=1;
        	ContextoImp.ShowPage();
			ContextoImp.BeginPage("Pagina N");
			imprime_encabezado(ContextoImp,trabajoImpresion);
     		Gnome.Print.Setfont (ContextoImp, fuente7);
        	contador=1;
        	filas=635;
        }
       //Console.WriteLine("contador despues del if: "+contador_.ToString());
		}
	
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
		aplicar_descuento = true;
		aplicar_siempre = true;
				
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString+nombrebd);
        // Verifica que la base de dato s este conectada
        try{
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand ();
        	        	  
           	comando.CommandText ="SELECT "+
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
						//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'9999999.99') AS ppcantidad,"+
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
			        	"AND osiris_erp_cobros_deta.eliminado = 'false' "+
						query_rango_fechas+
			        	" ORDER BY  osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
        	//Console.WriteLine(comando.CommandText);
        	NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query totales: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
								
			filas=635;
        	decimal ivaproducto = 0;
        	if (lector.Read())///AQUI SE LEE LA PRIMERA LINEA PARA DESPUES COMPARAR LAS ADMISIONES
        	{	
        		idadmision_ = (int) lector["idadmisiones"];				//obtengo valor de admision para futura comparacion
        		idgrupoproducto = (int) lector["id_grupo_producto"];		//obtengo valor del grupo para futura comparacion
        		
        		//Verifica si el lugar de procedencia del producto permite aplicar descuento
        		//Console.WriteLine("idadmision_ = "+idadmision_.ToString()+"    id_tipopaciente = "+id_tipopaciente.ToString()+" aplicar_siempre = "+aplicar_siempre.ToString());
		        if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101 
				  ||(int) lector["idadmisiones"] == 300 && (int) id_tipopaciente == 101 
				  ||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101
				  ||(int) lector["idadmisiones"] == 920 && (int) id_tipopaciente == 101){
					aplicar_descuento = true;
				}else{
					if (aplicar_siempre == true){
						aplicar_siempre = false;
						aplicar_descuento = false;							
					}
				}
					//agrega abonos y pagos honorarios
				totpago = decimal.Parse((string) lector["totalabono"]);
				totabono = decimal.Parse((string) lector["totalpago"]);
				honorarios = decimal.Parse((string) lector["honorario"]);
				
				//dandole valores a las variables
				precio_por_cantidad = decimal.Parse((string) lector["ppcantidad"]);
				ivaproducto = (precio_por_cantidad*15)/100;
				
				porcentagedesc =  decimal.Parse((string) lector["porcdesc"]);
												
				if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
					descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
					iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
					descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
				}else{
					descuento_neto += 0;
					iva_de_descuento += 0;
					descuento_del_grupo += 0;
				}
				
				if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
					total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
					total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
					total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
				}
				
				if((bool) lector["aplicar_iva"]== true){
					subtotal_al_15 += precio_por_cantidad;
					subtotal_al_15_grupo += precio_por_cantidad;
					total_de_iva += ivaproducto;
					iva_del_grupo += ivaproducto;
					
				}else{
					ivaproducto = 0;
					subtotal_al_0 += precio_por_cantidad;
					subtotal_al_0_grupo += precio_por_cantidad;
				}
				
				subtotal_del_grupo =subtotal_al_15_grupo+subtotal_al_0_grupo;
				
				/////DATOS DE PRODUCTOS
      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	     		   	
     		   	imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	
       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
       		 	contador+=1;
       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	
        		while (lector.Read())	/////COMIENZA EL CICLO DE LECTURA MDE PRODUCTOS APLICADOS
        		{	
        		
        			precio_por_cantidad = decimal.Parse((string) lector["ppcantidad"]);
        			//precio_por_cantidad = decimal.Parse((string) lector["cantidadaplicada"])* decimal.Parse((string) lector["preciopublico"]);
        			
					ivaproducto = (precio_por_cantidad*15)/100;
					
					porcentagedesc =  decimal.Parse((string) lector["porcdesc"]);
					
        			if (contador==1){
						imprime_encabezado(ContextoImp,trabajoImpresion);
						
        			}
        			
        			//Verifica si el lugar de procedencia del producto permite aplicar descuento
		        	if ((int) lector["idadmisiones"] == 100 && (int) id_tipopaciente == 101 
					  ||(int) lector["idadmisiones"] == 300 && (int) id_tipopaciente == 101 
					  ||(int) lector["idadmisiones"] == 400 && (int) id_tipopaciente == 101){
						aplicar_descuento = true;
					}else{
						if (aplicar_siempre == true){
							aplicar_siempre = false;
							aplicar_descuento = false;							
						}
					}
        			    	
        			//VERIFICACION DE TIPO DE ADMISION
        			if ((idadmision_ == (int) lector["idadmisiones"]) && (idgrupoproducto == (int) lector["id_grupo_producto"])){
 						        				
        				if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
							descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
							iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
						}else{
							descuento_neto += 0;
							iva_de_descuento += 0;
							descuento_del_grupo += 0;
						}
						
						if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
							total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
							total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
						}
							
						if((bool) lector["aplicar_iva"]== true){
							subtotal_al_15 += precio_por_cantidad;
							subtotal_al_15_grupo += precio_por_cantidad;
							total_de_iva += ivaproducto;
						}else{
							ivaproducto = 0;
							subtotal_al_0 += precio_por_cantidad;
							subtotal_al_0_grupo += precio_por_cantidad;
						}
						
						subtotal_del_grupo = subtotal_al_15_grupo+subtotal_al_0_grupo;
						iva_del_grupo += ivaproducto;
					}else{
						if ((idadmision_ != (int) lector["idadmisiones"]) ){
							Gnome.Print.Setfont (ContextoImp, fuente7);
        					///IMPRESION DE LOS TOTALES DE AREA
        					total_del_grupo = subtotal_del_grupo + iva_del_grupo;   
							ContextoImp.MoveTo(217.5, filas);		ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(218, filas);			ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(273.5, filas);		ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(274, filas);			ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(327.5, filas);		ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(328, filas);			ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(382.6, filas);		ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(383, filas);			ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(437.6, filas);		ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(438, filas);			ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(492.6, filas);		ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(493, filas);			ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(20, filas-2);//623
							contador+=1;
        					filas-=10;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
							
							imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
							contador+=1;
							salto_pagina(ContextoImp,trabajoImpresion,contador);
							
							
							
							imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
	       		 			contador+=1;
	       		 			salto_pagina(ContextoImp,trabajoImpresion,contador);
	       		 			
	       		 			subtotal_al_0_grupo = 0;
							subtotal_al_15_grupo = 0;
							iva_del_grupo = 0;
							
							descuento_neto = 0;
							iva_de_descuento = 0;
							descuento_del_grupo = 0;
							
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
								iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
								descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							}else{
								descuento_neto += 0;
								iva_de_descuento += 0;
								descuento_del_grupo += 0;
							}
						
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
								total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
								total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							}
														
							if((bool) lector["aplicar_iva"]== true){
								subtotal_al_15 += precio_por_cantidad;
								subtotal_al_15_grupo += precio_por_cantidad;
								total_de_iva += ivaproducto;
								iva_del_grupo += ivaproducto;
							}else{
								ivaproducto = 0;
								subtotal_al_0 += precio_por_cantidad;
								subtotal_al_0_grupo += precio_por_cantidad;
							}
							subtotal_del_grupo =subtotal_al_15_grupo+subtotal_al_0_grupo;
							idadmision_ = 0;				//limpio admsiosion para que no entre al otro if
						}
						
						if ((idgrupoproducto != (int) lector["id_grupo_producto"]) && (idadmision_ == (int) lector["idadmisiones"])){
							Gnome.Print.Setfont (ContextoImp, fuente7);
							total_del_grupo = subtotal_del_grupo + iva_del_grupo;
							
        					///IMPRESION DE LOS TOTALES DE AREA
							ContextoImp.MoveTo(217.5, filas);		ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(218, filas);			ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(273.5, filas);		ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(274, filas);			ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(327.5, filas);		ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(328, filas);			ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        					ContextoImp.MoveTo(382.6, filas);		ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(383, filas);			ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(437.6, filas);		ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(438, filas);			ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(492.6, filas);		ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(493, filas);			ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        					ContextoImp.MoveTo(20, filas-2);//623
							contador+=1;
        					filas-=10;
        					salto_pagina(ContextoImp,trabajoImpresion,contador);
        					
	        				imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
	       		 			contador+=1;
	       		 			salto_pagina(ContextoImp,trabajoImpresion,contador);
	       		 			
	       		 			subtotal_al_0_grupo = 0;
							subtotal_al_15_grupo = 0;
							iva_del_grupo = 0;
							
							descuento_neto = 0;
							iva_de_descuento = 0;
							descuento_del_grupo = 0;
							
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								descuento_neto += ((precio_por_cantidad*porcentagedesc)/100);
								iva_de_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
								descuento_del_grupo += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							}else{
								descuento_neto += 0;
								iva_de_descuento += 0;
								descuento_del_grupo += 0;
							}
						
							if(aplicar_descuento == true && aplicar_siempre == true && porcentagedesc > 0){
								total_de_descuento_neto+= ((precio_por_cantidad*porcentagedesc)/100);
								total_de_iva_descuento += ((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
								total_descuento += ((precio_por_cantidad*porcentagedesc)/100)+((((precio_por_cantidad*porcentagedesc)/100)*valoriva)/100);
							}
							
							if((bool) lector["aplicar_iva"]== true){
								subtotal_al_15 += precio_por_cantidad;
								subtotal_al_15_grupo += precio_por_cantidad;
								total_de_iva += ivaproducto;
								iva_del_grupo += ivaproducto;
								
							}else{
								ivaproducto = 0;
								subtotal_al_0 += precio_por_cantidad;
								subtotal_al_0_grupo += precio_por_cantidad;
							}
							subtotal_del_grupo =subtotal_al_15_grupo+subtotal_al_0_grupo;
						}
						idadmision_ = (int) lector["idadmisiones"];				//obtengo valor de admision para futura comparacion
						idgrupoproducto = (int) lector["id_grupo_producto"];		//obtengo valor del grupo para futura comparacion
					}
        			
				}//LLAVE INDICADORA DEL TERMINO DE CICLO WHILE
				if(aplicar_descuento == false && aplicar_siempre == false){
					total_de_descuento_neto = 0;
					total_de_iva_descuento = 0;
					total_descuento = 0;
				}
								
				total_del_grupo = subtotal_del_grupo + iva_del_grupo;
				
        		imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	Gnome.Print.Setfont (ContextoImp, fuente7);
       		 	ContextoImp.MoveTo(217.5, filas);		ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(218, filas);			ContextoImp.Show(subtotal_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(273.5, filas);		ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(274, filas);			ContextoImp.Show(iva_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(327.5, filas);		ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(328, filas);			ContextoImp.Show(total_del_grupo.ToString("C").PadLeft(10));
        		ContextoImp.MoveTo(382.6, filas);		ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(383, filas);			ContextoImp.Show(descuento_neto.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(437.6, filas);		ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(438, filas);			ContextoImp.Show(iva_de_descuento.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(492.6, filas);		ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(493, filas);			ContextoImp.Show(descuento_del_grupo.ToString("C").PadLeft(10)+" -");
        		ContextoImp.MoveTo(20, filas-2);//623
				contador+=1;
        		filas-=10;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
    			//Print.Setfont (ContextoImp, fuente1);
    			ContextoImp.MoveTo(381.5, filas) ;		ContextoImp.Show("SUBTOTAL AL 16%"); 
    			ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 16%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotal_al_15.ToString("C").PadLeft(10));  
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotal_al_15.ToString("C").PadLeft(10)); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUBTOTAL AL 0%");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 0%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotal_al_0.ToString("C").PadLeft(10));  
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotal_al_0.ToString("C").PadLeft(10)); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
		
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA AL 16%");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA AL 16%");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(total_de_iva.ToString("C").PadLeft(10));  
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total_de_iva.ToString("C").PadLeft(10));  
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				decimal subtotaldelmovimiento = subtotal_al_15+subtotal_al_0+total_de_iva;
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUB-TOTAL");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUB-TOTAL");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotaldelmovimiento.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotaldelmovimiento.ToString("C").PadLeft(10)); 
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
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("DESCUENTO NETO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("DESCUENTO NETO");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(total_de_descuento_neto.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total_de_descuento_neto.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA DE DESCUENTO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA DE DESCUENTO");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(total_de_iva_descuento.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total_de_iva_descuento.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("MENOS TOTAL DESCUENTO");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("MENOS TOTAL DESCUENTO");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(total_descuento.ToString("C").PadLeft(10)+" -"); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total_descuento.ToString("C").PadLeft(10)+" -"); 
				contador+=1;
				filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				////////////////////
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
				
				//////
				decimal totaldelmovimiento = subtotaldelmovimiento-coaseguro-deducible-total_descuento-totabono-totpago+honorarios;
				ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL");
				ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL");	
				ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totaldelmovimiento.ToString("C").PadLeft(10)); 
				ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaldelmovimiento.ToString("C").PadLeft(10)); 
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
			//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		}
	}
 }
}