///////////////////////////////////////////////////////////
// created on 25/05/2007 at 08:36 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: ing. Juan Antonio Peña Gonzalez (Programacion)
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
// Programa		: rpt_paquetes.cs
// Proposito	: muestra articulos en paquetes de cirugia
// Objeto		: rpt_paquetes.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;


namespace osiris
{
	public class paquetes_reporte
	{
		string connectionString;
        string nombrebd;
		string cirugia = "";
		string medico = "";
		int id;
		string tiporeporte = "";
		string deposito_minimo = "";
		string dias_internamiento = "";
		string tel_medico = "";
		string tel_opcional = "";
		string fax = "";
		string notas = "";
		string numpresupuesto = "";
		string titulo = "PAQUETES DE CIRUGIA";
		string schars = "";
		bool rptconprecio = true;
		
		int filas=690;//635
		int contador = 1;
		int numpage = 1;
		
		//variables para rangos de fecha
				
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string fcreacion = "";
		decimal cantaplicada= 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subt15 = 0;
		decimal subt0 = 0;
		decimal sumaiva = 0;
		decimal total = 0;
		decimal totaladm = 0;
		decimal subtotaldelmov = 0;
		decimal deducible = 0;
		decimal coaseguro = 0;
		decimal valoriva;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
				
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public paquetes_reporte ( int _id_ ,string nombcirugia,string _medico_,string _nombrebd_,string tiporeporte_,
								string deposito_minimo_,string dias_internamiento_,string tel_medico_,
								string tel_opcional_,string fax_,string numpresupuesto_,string notas_,bool rptconprecio_,string presupuesto_seleccionados_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			cirugia = nombcirugia; 
			id = _id_;
			medico = _medico_;
			tiporeporte = tiporeporte_;
			deposito_minimo = deposito_minimo_;
			dias_internamiento = dias_internamiento_;
			tel_medico = tel_medico_;
			tel_opcional = tel_opcional_;
			fax = fax_;
			notas = notas_;
			numpresupuesto = numpresupuesto_;
			rptconprecio = rptconprecio_;
			valoriva = decimal.Parse(classpublic.ivaparaaplicar);
			
			if(tiporeporte == "presupuestos") { 
				titulo = "PRESUPUESTO DE CIRUGIA";
			}else{
				titulo = "PAQUETES DE CIRUGIA";
			}
			
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 			dialogo.Dispose (); 
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
                new PrintJobPreview(trabajo, titulo).Show();
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
			
			Gnome.Print.Setfont (ContextoImp, fuente12);
			ContextoImp.MoveTo(200.5, 740);			ContextoImp.Show(titulo);
			ContextoImp.MoveTo(201, 740);			ContextoImp.Show(titulo);
			
			Gnome.Print.Setfont (ContextoImp, fuente36);
			ContextoImp.MoveTo(20, 738);			ContextoImp.Show("____________________________");
			
			if(tiporeporte == "presupuestos") {
				Gnome.Print.Setfont (ContextoImp, fuente10);
				ContextoImp.MoveTo(444.7, 770);		ContextoImp.Show("PRESUPUESTO Nº "+numpresupuesto);
				Gnome.Print.Setfont (ContextoImp, fuente7);
				
				ContextoImp.MoveTo(21.7, 728);			ContextoImp.Show(cirugia);
				ContextoImp.MoveTo(22, 728);			ContextoImp.Show(cirugia);
				ContextoImp.MoveTo(350, 728);			ContextoImp.Show("Medico: "+medico.ToUpper());
				ContextoImp.MoveTo(349.7, 728);			ContextoImp.Show("Medico: ");
				ContextoImp.MoveTo(20, 720);			ContextoImp.Show("Telefono Medico: "+tel_medico.ToString());
				ContextoImp.MoveTo(19.7, 720);			ContextoImp.Show("Telefono Medico: ");
				ContextoImp.MoveTo(200, 720);			ContextoImp.Show("FAX: "+fax.ToString());
				ContextoImp.MoveTo(199.7, 720);			ContextoImp.Show("FAX: ");
				ContextoImp.MoveTo(350, 720);			ContextoImp.Show("Tel. Opcional: "+tel_opcional.ToString());
				ContextoImp.MoveTo(349.7, 720);			ContextoImp.Show("Tel. Opcional: ");
				ContextoImp.MoveTo(320,712);			ContextoImp.Show("Dias de Internamiento: "+dias_internamiento.ToString());
				ContextoImp.MoveTo(319.7,712);			ContextoImp.Show("Dias de Internamiento: ");
				ContextoImp.MoveTo(20,702);				ContextoImp.Show("NOTAS: "+notas);
				ContextoImp.MoveTo(19.7,702);			ContextoImp.Show("NOTAS: ");
			}else{
				Gnome.Print.Setfont (ContextoImp, fuente10);
				ContextoImp.MoveTo(21, 720);			ContextoImp.Show("ID Cirugia: "+id);
				ContextoImp.MoveTo(20.7, 720);			ContextoImp.Show("ID Cirugia: "+id);
				
				ContextoImp.MoveTo(131, 720);			ContextoImp.Show(cirugia);
				ContextoImp.MoveTo(130.7, 720);			ContextoImp.Show(cirugia);
				
				ContextoImp.MoveTo(21,710);				ContextoImp.Show("Dias de Internamiento: "+dias_internamiento.ToString());
				ContextoImp.MoveTo(20.7,710);			ContextoImp.Show("Dias de Internamiento: ");
			}
			
			//Print.Setrgbcolor(ContextoImp, 150,0,0);///cambio color de fuente a rojo
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(230.7, 50);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(230, 50);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			
			
						
			Gnome.Print.Setfont (ContextoImp, fuente9);
			
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);//regreso color fuente a negro
	  }
      
      void genera_tabla(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
      {
      	//////////////////DIBUJANDO TABLA (START DRAWING TABLE)////////////////////////
		Gnome.Print.Setfont (ContextoImp, fuente36);
		//ContextoImp.MoveTo(20, 700);				ContextoImp.Show("____________________________");//20,710
				
		////COLUMNAS
		int filasl = 670;
		for (int i1=0; i1 < 31; i1++)//30 veces para tamaño carta, 28 tamaño A2
		{	
            int columnas = 17;
			Gnome.Print.Setfont (ContextoImp, fuente36);
			//ContextoImp.MoveTo(columnas, filasl-.8);		ContextoImp.Show("|");
			//ContextoImp.MoveTo(columnas-1, filasl-.8);	ContextoImp.Show("|");
			//ContextoImp.MoveTo(columnas+556, filasl);		ContextoImp.Show("|");
			//ContextoImp.MoveTo(columnas+557, filasl);		ContextoImp.Show("|");
			filasl-=20;
		}
		//columnas tenues
		//int filasc =640;
		Gnome.Print.Setfont (ContextoImp, fuente36);
		//ContextoImp.MoveTo(20,66);		ContextoImp.Show("____________________________");//20,73
		///FIN DE DIBUJO DE TABLA (END DRAWING TABLE)///////
    }
    
    void genera_lineac(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		//Gnome.Print.Setfont (ContextoImp, fuente11);
		//ContextoImp.MoveTo(75, filas);					ContextoImp.Show("|");//52
		//ContextoImp.MoveTo(104, filas);					ContextoImp.Show("|");//104
		if(rptconprecio == true) 
       	{
			//ContextoImp.MoveTo(375, filas);					ContextoImp.Show("|");
			//ContextoImp.MoveTo(425, filas);					ContextoImp.Show("|");
			//ContextoImp.MoveTo(475, filas);					ContextoImp.Show("|");
			//ContextoImp.MoveTo(523, filas);					ContextoImp.Show("|");
		}
		Gnome.Print.Setfont (ContextoImp, fuente7);
	}
    
    void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion, string descrp_admin)
    {
    	Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas+8);
		//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		Gnome.Print.Setfont (ContextoImp, fuente9);
		//LUGAR DE CARGO
		ContextoImp.MoveTo(200.5, filas);			ContextoImp.Show(descrp_admin.ToString());//635
		ContextoImp.MoveTo(201, filas);				ContextoImp.Show(descrp_admin.ToString());//635
		Gnome.Print.Setfont (ContextoImp, fuente7);
		ContextoImp.MoveTo(20, filas-2);//633
		//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
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
		if(rptconprecio == true) 
       	{
			ContextoImp.MoveTo(384.5, filas);			ContextoImp.Show("PRECIO");//625
			ContextoImp.MoveTo(385, filas);				ContextoImp.Show("PRECIO");//625
			ContextoImp.MoveTo(429.6, filas);			ContextoImp.Show("SUB-TOTAL");//625
			ContextoImp.MoveTo(430, filas);				ContextoImp.Show("SUB-TOTAL");//625
			ContextoImp.MoveTo(492.6, filas);			ContextoImp.Show("IVA");//625
			ContextoImp.MoveTo(493, filas);				ContextoImp.Show("IVA");//625
			ContextoImp.MoveTo(544.6, filas);			ContextoImp.Show("TOTAL");//625
			ContextoImp.MoveTo(545, filas);				ContextoImp.Show("TOTAL");//625
		}
		///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
		genera_lineac(ContextoImp, trabajoImpresion);
		//ContextoImp.MoveTo(20, filas-2);//623
		//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
		filas-=10;
    }
   
	void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion,int contador_)
	{
		//Console.WriteLine("contador antes del if: "+contador_.ToString());
        if (contador_ > 63 )//57
        {
        	numpage +=1;
        	ContextoImp.ShowPage();
			ContextoImp.BeginPage("Pagina N");
			schars = "";
			imprime_encabezado(ContextoImp,trabajoImpresion);
     		genera_tabla(ContextoImp,trabajoImpresion);
     		Gnome.Print.Setfont (ContextoImp, fuente7);
     		contador=1;
        	filas=690;
        }
       //Console.WriteLine("contador despues del if: "+contador_.ToString());
	}
	
	void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
	{
		decimal precioventaconvenido;
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString+nombrebd);
        // Verifica que la base de datos este conectada
          
        try 
        {
 			conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
        	
        	if(tiporeporte == "paquetes")
        	{
           		comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
							"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_his_tipo_cirugias.precio_de_venta,'999999999999') AS precioventa, "+
							"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_grupo_producto "+
							"WHERE "+
							"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+id.ToString() +"' "+
							"ORDER BY osiris_his_cirugias_deta.id_tipo_admisiones,osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
        	}else{
	        	if(tiporeporte == "presupuestos")
	        	{
	        		comando.CommandText = "SELECT descripcion_producto,descripcion_admisiones, "+
							"id_empleado,osiris_his_presupuestos_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_presupuestos_deta.id_tipo_admisiones,  "+
							"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999999') AS precioventa, "+
							"to_char(osiris_his_presupuestos_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_presupuestos_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_presupuestos_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_presupuestos_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_presupuestos_enca,osiris_his_presupuestos_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_grupo_producto "+
							"WHERE "+
							"osiris_his_presupuestos_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_his_presupuestos_enca.id_presupuesto = osiris_his_presupuestos_deta.id_presupuesto "+
							"AND osiris_his_presupuestos_deta.eliminado = 'false' "+ 
							"AND osiris_his_presupuestos_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_presupuestos_deta.id_presupuesto IN ('"+id.ToString()+"') "+							
							"ORDER BY osiris_his_presupuestos_deta.id_tipo_admisiones,osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
	        	}
        	}	
        	
        	NpgsqlDataReader lector = comando.ExecuteReader ();
        	//Console.WriteLine("query proc cobr: "+comando.CommandText.ToString());
			ContextoImp.BeginPage("Pagina 1");
								
			filas=690;
        	if (lector.Read())
        	{	
        		precioventaconvenido = decimal.Parse((string) lector["precioventa"]);
        		
        		datos = (string) lector["descripcion_producto"];
        		cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
				subtotal = decimal.Parse((string) lector["preciopublico"])*cantaplicada;
				
				if((bool) lector["aplicar_iva"]== true){
					ivaprod = (subtotal*valoriva)/100;
					subt15 += subtotal;
				}else{
					subt0 += subtotal;
					ivaprod = 0;
				}
				sumaiva += ivaprod;
				total = subtotal + ivaprod;				
        		totaladm += total;
				subtotaldelmov += total;
					
        		/////DATOS DE PRODUCTOS
      		  	imprime_encabezado(ContextoImp,trabajoImpresion);
      		  	genera_tabla(ContextoImp,trabajoImpresion);
     		   	
     		   	imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
        		contador+=1;
        		salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	//genera_lineac(ContextoImp, trabajoImpresion);
       		 	
       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
       		 	contador+=1;
       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
       		 	genera_lineac(ContextoImp, trabajoImpresion);
        		
        		//DATOS TABLA
				ContextoImp.MoveTo(80, filas);			ContextoImp.Show((string) lector["cantidadaplicada"]);//22	
				ContextoImp.MoveTo(22, filas);			ContextoImp.Show((string) lector["idproducto"]);//55
				if(rptconprecio == true)
				{
					if(datos.Length > 64) { datos = datos.Substring(0,60); }
					ContextoImp.MoveTo(110, filas);			ContextoImp.Show(datos.ToString());
					ContextoImp.MoveTo(380, filas);			ContextoImp.Show("$"+(string) lector["preciopublico"]);
					ContextoImp.MoveTo(430, filas);			ContextoImp.Show(subtotal.ToString("C"));
					ContextoImp.MoveTo(480, filas);			ContextoImp.Show(ivaprod.ToString("C"));
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total.ToString("C"));
				}else{
					if(datos.Length > 115) { datos = datos.Substring(0,115); }
					ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.ToString());
				}
				contador+=1;			filas-=10;
				salto_pagina(ContextoImp,trabajoImpresion,contador);
				idadmision_ = (int) lector["id_tipo_admisiones"];
        		idproducto = (int) lector["id_grupo_producto"];
				
				while (lector.Read())
        		{
        			if (contador==1){
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
		        		contador+=1;
		        		salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	
		       		 	imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
		       		 	contador+=1;
		       		 	salto_pagina(ContextoImp,trabajoImpresion,contador);
		       		 	genera_lineac(ContextoImp, trabajoImpresion);
        			}
        			
        			datos = (string) lector["descripcion_producto"];
					cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
					subtotal = decimal.Parse((string) lector["preciopublico"]) * cantaplicada;
					
					if((bool) lector["aplicar_iva"]== true){
						ivaprod = (subtotal*valoriva)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;
					totaladm +=total;
					subtotaldelmov +=total;
        			
					if(idadmision_ == (int) lector["id_tipo_admisiones"])
					{
						genera_lineac(ContextoImp, trabajoImpresion);
						
						//DATOS TABLA
        				if (idproducto != (int) lector["id_grupo_producto"])
        				{
        					idproducto = (int) lector["id_grupo_producto"];
        					imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
        			   		contador+=1;
        			   		salto_pagina(ContextoImp,trabajoImpresion,contador);
        			   		genera_lineac(ContextoImp, trabajoImpresion);
        				}
					}else{////////SI LA ADMISION CAMBIA HACE ESTO	
						if(rptconprecio == true) 
       		 			{
							///IMPRESION DE LOS TOTALES DE AREA
							salto_pagina(ContextoImp,trabajoImpresion,contador);
	        				genera_lineac(ContextoImp, trabajoImpresion);
	        				ContextoImp.MoveTo(479.7, filas);		ContextoImp.Show("Total de Area");
	        				ContextoImp.MoveTo(480, filas);			ContextoImp.Show("Total de Area");
	        				//ContextoImp.MoveTo(529.7, filas);		ContextoImp.Show(totaladm.ToString("C"));
	        				//ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaladm.ToString("C"));
		        			contador+=1;
		        			filas-=10;
		        			salto_pagina(ContextoImp,trabajoImpresion,contador);
		        			totaladm = 0;
		        			genera_lineac(ContextoImp, trabajoImpresion);
	        			}/*
						datos = (string) lector["descripcion_producto"];
						cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
						subtotal = decimal.Parse((string) lector["preciopublico"])*cantaplicada;
						
						if((bool) lector["aplicar_iva"]== true){
							ivaprod = (subtotal*15)/100;
							subt15 += subtotal;
						}else{
							subt0 += subtotal;
							ivaprod = 0;
						}
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						totaladm +=total;
						subtotaldelmov +=total;
						*/
						imprime_titulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_admisiones"]);
						contador+=1;
						salto_pagina(ContextoImp,trabajoImpresion,contador);
						
						idadmision_ = (int) lector["id_tipo_admisiones"];
						if (idproducto != (int) lector["id_grupo_producto"])
        				{
							idproducto = (int) lector["id_grupo_producto"];
							imprime_subtitulo(ContextoImp,trabajoImpresion,(string) lector["descripcion_grupo_producto"]);
	        			   	contador+=1;
							salto_pagina(ContextoImp,trabajoImpresion,contador);
							genera_lineac(ContextoImp, trabajoImpresion);
						}
					}
					
        			ContextoImp.MoveTo(80, filas);					ContextoImp.Show((string) lector["cantidadaplicada"]);//22	
					ContextoImp.MoveTo(22, filas);					ContextoImp.Show((string) lector["idproducto"]);//55
					if(rptconprecio == true)
					{
						if(datos.Length > 64) { datos = datos.Substring(0,60); }
						ContextoImp.MoveTo(110, filas);			ContextoImp.Show(datos.ToString());
						ContextoImp.MoveTo(380, filas);			ContextoImp.Show("$"+(string) lector["preciopublico"]);
						ContextoImp.MoveTo(430, filas);			ContextoImp.Show(subtotal.ToString("C"));
						ContextoImp.MoveTo(480, filas);			ContextoImp.Show(ivaprod.ToString("C"));
						ContextoImp.MoveTo(530, filas);			ContextoImp.Show(total.ToString("C"));
					}else{
						if(datos.Length > 115) { datos = datos.Substring(0,115); }
						ContextoImp.MoveTo(110, filas);		ContextoImp.Show(datos.ToString());
					}
					contador+=1;		filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
				}//SE TERMINA EL CICLO
        		//imprime_encabezado(ContextoImp,trabajoImpresion);
     		   	//genera_tabla(ContextoImp,trabajoImpresion);
       		 	if(rptconprecio == true) 
       		 	{
	       		 	////IMPRESION DE LOS TOTALES DE AREA
	        		genera_lineac(ContextoImp, trabajoImpresion);
	        		ContextoImp.MoveTo(479.7, filas);				ContextoImp.Show("Total de Area");
	        		ContextoImp.MoveTo(480, filas);					ContextoImp.Show("Total de Area");
	        		//ContextoImp.MoveTo(529.7, filas);				ContextoImp.Show(totaladm.ToString("C"));
	        		//ContextoImp.MoveTo(530, filas);					ContextoImp.Show(totaladm.ToString("C"));
	        		contador+=1;
	        		salto_pagina(ContextoImp,trabajoImpresion,contador);
	        		
	        		///TOTAL QUE SE LE COBRARA AL PACIENTE O AL RESPONSABLE DEL PACIENTE
	    			ContextoImp.MoveTo(20, filas-2);//623
					//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
	    			decimal totaldelmov =subtotaldelmov - deducible - coaseguro;//desctotal;
	    			contador+=1;
	    			filas-=10;
	    			salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas) ;		ContextoImp.Show("SUBTOTAL AL "+valoriva.ToString().Trim()); 
	    			ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL "+valoriva.ToString().Trim());	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt15.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt15.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					Gnome.Print.Setfont (ContextoImp,fuente10);
					ContextoImp.MoveTo(50.5, filas);		ContextoImp.Show("PRECIO DE VENTA "+precioventaconvenido.ToString("C"));
					ContextoImp.MoveTo(51, filas);			ContextoImp.Show("PRECIO DE VENTA "+precioventaconvenido.ToString("C"));
					Gnome.Print.Setfont (ContextoImp, fuente7);
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUBTOTAL AL 0%");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUBTOTAL AL 0%");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subt0.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subt0.ToString("C"));
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					Gnome.Print.Setfont (ContextoImp,fuente10);
					ContextoImp.MoveTo(50.5,filas);			ContextoImp.Show("DEPOSITO MINIMO: "+(decimal.Parse(deposito_minimo)).ToString("C"));
					ContextoImp.MoveTo(51,filas);			ContextoImp.Show("DEPOSITO MINIMO: "+(decimal.Parse(deposito_minimo)).ToString("C"));
					Gnome.Print.Setfont (ContextoImp, fuente7);
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("IVA AL  "+valoriva.ToString().Trim());
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("IVA AL  "+valoriva.ToString().Trim());	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(sumaiva.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(sumaiva.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("SUB-TOTAL");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("SUB-TOTAL");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(subtotaldelmov.ToString("C"));
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(subtotaldelmov.ToString("C"));
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
					
					ContextoImp.MoveTo(381.5, filas);		ContextoImp.Show("TOTAL");
					ContextoImp.MoveTo(382, filas);			ContextoImp.Show("TOTAL");	
					ContextoImp.MoveTo(529.5, filas);		ContextoImp.Show(totaldelmov.ToString("C")); 
					ContextoImp.MoveTo(530, filas);			ContextoImp.Show(totaldelmov.ToString("C")); 
					contador+=1;
					filas-=10;
					salto_pagina(ContextoImp,trabajoImpresion,contador);
				}else{
					ContextoImp.MoveTo(20, filas-2);//623
					//ContextoImp.Show("________________________________________________________________________________________________________________________________________________");
	    		}
				ContextoImp.ShowPage();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "NO contiene productos aplicados \n"+"existentes para que se muestre \n");
				msgBoxError.Run ();		msgBoxError.Destroy();
			}	
		}catch (NpgsqlException ex){
			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			return; 
		}
	}
 }    
}

//string prueba = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
			//ContextoImp.MoveTo(22, 720); ContextoImp.Show(prueba.ToString());
			//ContextoImp.MoveTo(22, 710); ContextoImp.Show(prueba.Length.ToString());
			/*
			Console.WriteLine("cirugia "+cirugia.Length);
			float nchars = (86 - cirugia.Length)/2;
			Console.WriteLine("nchars "+nchars.ToString());
			
			for (int i3 = 0; i3 < nchars; i3++) {
				schars += nchars;
			}*/
			
		//Console.WriteLine("cirugia "+cirugia.Length);
			//float nchars = (200 - cirugia.Length)/2;
			//Console.WriteLine("nchars "+nchars.ToString());
			/*
			for (int i3 = 0; i3 <= nchars; i3++)	
			{	
				if(cirugia.Length > 30 ){ schars += "   ";}
				else{schars += "  ";}
			}*/