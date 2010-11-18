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
/// //////////////////////////////////////////////////////
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
		
		string sql_compcaja = "";
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') >= '"+DateTime.Now.ToString("dd")+"'  AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') <= '"+DateTime.Now.ToString("dd")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') >= '"+DateTime.Now.ToString("MM")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " ;
		//Declaracion de ventana de error
		string sql_foliodeservicio = "";
		string sql_numerocomprobante = "";
		string sql_orderquery = " ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
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
			
			Console.WriteLine(tipocomprobante);
			
			if (tipocomprobante == "CAJA"){			
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
			}
			if (tipocomprobante == "SERVICIO"){			
				sql_numerocomprobante = "AND osiris_erp_comprobante_servicio.numero_comprobante_servicio = '"+numero_comprobante.ToString().Trim()+"' ";
			}
			if (tipocomprobante == "PAGARE"){
				sql_numerocomprobante = "AND osiris_erp_pagares.numero_comprobante_servicio = '"+numero_comprobante.ToString().Trim()+"' ";
			}
			sql_foliodeservicio = "AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
			print = new PrintOperation ();
			print.JobName = "IMPRIME COMPROBANTE DE "+tipocomprobante;
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
			bool apl_desc = false;
			bool apl_desc_siempre = true;
			int toma_tipoadmisiones = 0;
			int toma_grupoproducto = 0;
			float toma_valor_total = 0;
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
	        	Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					if (tipocomprobante == "CAJA"){
						toma_valor_total = float.Parse((string) lector["montodelabono"]);		
					}
					toma_tipoadmisiones = (int) lector["idadmisiones"];
					toma_grupoproducto = (int) lector["id_grupo_producto"];
					busca_tipoadmisiones(lector["foliodeservicio"].ToString().Trim());
					imprime_encabezado(cr,layout,numero_comprobante.ToString().Trim(),lector["foliodeservicio"].ToString().Trim(),lector["pidpaciente"].ToString().Trim(),
					               lector["nombre_completo"].ToString().Trim(),lector["descripcion_empresa"].ToString().Trim(),lector["telefono_particular1_paciente"].ToString().Trim(),
					               lector["fechcreacion"].ToString().Trim()+" "+lector["horacreacion"].ToString().Trim(),lector["concepto_comprobante"].ToString().Trim(),lector["observacionesvarias"].ToString().Trim(),
					                   toma_valor_total);
					comienzo_linea += separacion_linea;
					comienzo_linea += separacion_linea;
					if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960){	
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
		                        string conceptocomprobante, string observacionescomprobante, float tomavalortotal )
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
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText("COMPROBANTE DE "+tipocomprobante);				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(479*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("N° Folio "+numerocomprobante);				Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 8.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
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
			
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*23)*escala_en_linux_windows);		layout.SetText("Concepto    : "+conceptocomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText("Observacion : "+observacionescomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText("Atendio por : "+nombreempleado);	Pango.CairoHelper.ShowLayout (cr, layout);		

			if (tipocomprobante == "CAJA"){
				fontSize = 9.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(350*escala_en_linux_windows,comienzo_linea+(separacion_linea*23)*escala_en_linux_windows);		layout.SetText("T O T A L : "+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);		
			}
			fontSize = 8.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
		
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
	        	Console.WriteLine(comando.CommandText.ToString());
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

/*
			
      	
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
			if(tipoproducto.Length > 90){
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
						"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, "+ 
						"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
						"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"osiris_grupo_producto.descripcion_grupo_producto, "+
						"osiris_productos.id_grupo_producto,  "+
						"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
						"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
						"to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
						"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
						"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
						//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
						"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico "+
						"FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto "+
						"WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' "+
			        	"AND osiris_erp_cobros_deta.eliminado = 'false' "+
			        	" ORDER BY  osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
	        			//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') >= '"+DateTime.Now.ToString("dd")+"'  AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') <= '"+DateTime.Now.ToString("dd")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') >= '"+DateTime.Now.ToString("MM")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " ;
	        	
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
						ivaprod = (subtotal * valoriva)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;
					if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0.00){
						descsiniva = (subtotal*(porcentajedes/100));
						ivadedesc = (descsiniva * valoriva) / 100;
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
								ivaprod = (subtotal * valoriva)/100;
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
								ivadedesc =descsiniva * valoriva/100;
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
								ivaprod = (subtotal * valoriva)/100;
								subt15 += subtotal;
							}else{
								subt0 += subtotal;
								ivaprod = 0;
							}
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
							if(apl_desc == true && apl_desc_siempre == true && porcentajedes > 0.00){
								descsiniva = (subtotal*(porcentajedes/100));
								ivadedesc = (descsiniva * valoriva) /100;
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
        			
        			//ContextoImp.MoveTo(110, 475);				ContextoImp.Show(classpublic.ConvertirCadena(apagar.ToString("F"),"Peso"));
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
	
	}    
}
*/