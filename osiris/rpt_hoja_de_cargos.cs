// created on 18/06/2007 at 09:51 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 
//  25/05/2010   Ing. Daniel Olivares Cuevas (Programacion y Traspasado a GTKPrint) arcangeldoc@gmail.com
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
// Programa		: rpt_hoja_de_cargos.cs
// Proposito	: Impresion de la hoja de cargos 
// Objeto		: rpt_hoja_de_cargos.cs

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class hoja_cargos
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		
		string connectionString;
        string nombrebd;
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
		string area;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string entry_id_habitacion;
		string entry_especialidad;
				
		int idadmision_ = 0;
		string query_rango;
		int tipointernamiento = 10;
		
		int comienzo_linea = 145;
		int separacion_linea = 10;
		int contador_productos = 0;
		
		string query_todo = "SELECT "+
					"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente,"+ 
					"osiris_productos.id_grupo_producto,descripcion_producto,"+
					"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
					"osiris_grupo_producto.descripcion_grupo_producto,"+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,"+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mi') AS horacreacion,"+
					"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH24') AS tiempocreacion,"+
					"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,"+
					"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada  "+
					"FROM "+ 
					"osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto "+
					"WHERE "+
					"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+					
					"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+ 
					"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+					
		        	"AND osiris_erp_cobros_deta.eliminado = 'false' ";
		string sql_tipoadmision = "";
		string sql_numerofolio = "";
		string sql_loginempleado = "";
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public hoja_cargos ( int PidPaciente_ , int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string area_,string NomEmpleado_,string AppEmpleado_,
						string ApmEmpleado_,string LoginEmpleado_, string query_, int tipointernamiento_,string entry_id_habitacion_,string entry_especialidad_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			fecha_admision = entry_fecha_admision_;
			fechahora_alta = entry_fechahora_alta_;
			nombre_paciente = entry_nombre_paciente_;
			telefono_paciente = entry_telefono_paciente_;
			doctor = entry_doctor_;
			cirugia = cirugia_;
			id_tipopaciente = idtipopaciente_;
			tipo_paciente = entry_tipo_paciente_;
			aseguradora = entry_aseguradora_;
			edadpac = edadpac_;
			fecha_nacimiento = fecha_nacimiento_;
			dir_pac = dir_pac_;
			empresapac = empresapac_;
			query_rango = query_;
			tipointernamiento = tipointernamiento_;
			area = area_;							// Recibe el parametro del modulo que manda a imprimir (UCIA, Hospital, Urgencia, etc)
			entry_id_habitacion = entry_id_habitacion_;
			entry_especialidad = entry_especialidad_;
			
			sql_tipoadmision = " AND osiris_erp_cobros_deta.id_tipo_admisiones = '"+tipointernamiento.ToString()+"' ";
			sql_numerofolio = " AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString()+"' ";			
			sql_loginempleado = " AND id_empleado = '"+LoginEmpleado+"' ";
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
					
			if((bool) valida_impresion_enfermera() == true){			
				print = new PrintOperation ();						
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Este folio no contiene productos aplicados \n"+
							"por usted el dia de HOY para que la hoja de cargos se muestre \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			PrintContext context = args.Context;											
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
			string descripcion_producto_aplicado;
			int idgrupoproducto;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        //Querys
	        string ampm = "";
			try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	        	comando.CommandText =query_todo+sql_tipoadmision+sql_numerofolio+sql_loginempleado+query_rango+ "ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd 24HH:mm') ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
	        	if (lector.Read()){	
					contador_productos += 1;
	        		descripcion_producto_aplicado = (string) lector["descripcion_producto"];
					idgrupoproducto = (int) lector["id_grupo_producto"];
					if(descripcion_producto_aplicado.Length > 70) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,70); }
					if(int.Parse((string) lector["tiempocreacion"]) >= 12){	ampm = " PM. "; }else{	ampm = " AM. "; }
					imprime_encabezado(context);
	     		   	imprime_subtitulo(context,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
					
					cr.MoveTo(08*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["idproducto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
	       		 	cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["cantidadaplicada"]);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(112*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["horacreacion"]+ampm);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(150*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("__________");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("__________");	Pango.CairoHelper.ShowLayout (cr, layout);
					
					comienzo_linea += separacion_linea;
					while (lector.Read()){
						contador_productos += 1;
						if (idgrupoproducto != (int) lector["id_grupo_producto"]){
        					idgrupoproducto = (int) lector["id_grupo_producto"];
							imprime_subtitulo(context,(string) lector["descripcion_grupo_producto"],(string) lector ["fechcreacion"]);
							salto_de_pagina(context);
						}
						descripcion_producto_aplicado = (string) lector["descripcion_producto"];
						if(descripcion_producto_aplicado.Length > 70) { descripcion_producto_aplicado = descripcion_producto_aplicado.Substring(0,70); }
						if(int.Parse((string) lector["tiempocreacion"]) >= 12){	ampm = " PM. "; }else{	ampm = " AM. "; }						
						cr.MoveTo(08*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["idproducto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
	       		 		cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["cantidadaplicada"]);	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(112*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText((string) lector["horacreacion"]+ampm);	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(150*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText(descripcion_producto_aplicado);	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("__________");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("__________");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						salto_de_pagina(context);
					}
					comienzo_linea += separacion_linea;
					cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("Total de productos aplicados: "+contador_productos.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;
					comienzo_linea += separacion_linea;
					cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("_________________________________________");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("_________________________________________");	Pango.CairoHelper.ShowLayout (cr, layout);
					comienzo_linea += separacion_linea;					
					cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("FIRMA de Enfermera");	Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(370*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);	layout.SetText("FIRMA de Cajera(o) de Recibido");	Pango.CairoHelper.ShowLayout (cr, layout);
				}	
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);				
			}			
		}
		      	
		void imprime_encabezado(PrintContext context)
		{
			//Console.WriteLine("entra en la impresion del encabezado");
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			//context.PageSetup.Orientation = PageOrientation.Landscape;
			
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
			fontSize = 8.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(480*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("FOLIO DE ATENCION");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(510*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(folioservicio.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;		layout = null;							layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(225*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("HOJA REGISTROS DE "+area);				Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 8.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(220*escala_en_linux_windows,45*escala_en_linux_windows);			layout.SetText("DATOS GENERALES DEL PACIENTE");			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,55*escala_en_linux_windows);			layout.SetText("INGRESO:"+fecha_admision.Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(465*escala_en_linux_windows,55*escala_en_linux_windows);			layout.SetText("EGRESO:"+fechahora_alta.Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("EXP.: "+PidPaciente.ToString()+"	Nombre Paciente:"+nombre_paciente.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(340*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("Fecha de Nacimiento: "+fecha_nacimiento.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(500*escala_en_linux_windows,65*escala_en_linux_windows);			layout.SetText("Edad: "+edadpac.ToString());							Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText("Direccion: "+dir_pac.ToString());							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,85*escala_en_linux_windows);			layout.SetText("Tel. Pac.: "+telefono_paciente.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,85*escala_en_linux_windows);			layout.SetText("Nº Hab/Sala: "+entry_id_habitacion.Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			if((string) aseguradora == "Asegurado"){
				cr.MoveTo(05*escala_en_linux_windows,95*escala_en_linux_windows);			layout.SetText("Tipo de paciente: "+tipo_paciente.ToString()+"	Aseguradora: "+aseguradora.ToString()+"	Poliza: ");					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(05*escala_en_linux_windows,95*escala_en_linux_windows);			layout.SetText("Tipo de paciente: "+tipo_paciente.ToString()+"	Empresa: "+empresapac.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			}
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(05*escala_en_linux_windows,105*escala_en_linux_windows);			layout.SetText("Medico: "+doctor.ToString());					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,105*escala_en_linux_windows);			layout.SetText("Especialidad: "+entry_especialidad.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,115*escala_en_linux_windows);			layout.SetText("Cirugia/Diagnostico: "+cirugia.ToString());		Pango.CairoHelper.ShowLayout (cr, layout);
			// Creando el Cuadro de Titulos para colocar el nombre del usuario
			cr.Rectangle (05*escala_en_linux_windows, 125*escala_en_linux_windows, 565*escala_en_linux_windows, 15*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
			cr.MoveTo(08*escala_en_linux_windows,128*escala_en_linux_windows);			layout.SetText("Usuario que realizo los cargos: "+LoginEmpleado+" -- "+NomEmpleado.Trim()+" "+AppEmpleado.Trim()+" "+ApmEmpleado.Trim());		Pango.CairoHelper.ShowLayout (cr, layout);			
		}
		
		void imprime_subtitulo(PrintContext context,string tipoproducto, string fechacreacion)
		{		
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 7.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(09*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Codigo");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Cantidad");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Hora");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(160*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(tipoproducto);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(315*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(fechacreacion);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(490*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("V.B.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(540*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Caja");					Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
			comienzo_linea += separacion_linea;
    	}
		
		void salto_de_pagina(PrintContext context)			
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			//context.PageSetup.Orientation = PageOrientation.Landscape;
			if(comienzo_linea > 660){
				cr.ShowPage();
				layout = null;
				layout = context.CreatePangoLayout ();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 145;
				imprime_encabezado(context);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		bool valida_impresion_enfermera()
		{
			bool response = false;
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	        	comando.CommandText =query_todo+sql_tipoadmision+sql_numerofolio+sql_loginempleado+query_rango+" ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd 24HH:mm') ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
	        	if (lector.Read()){
					response = true;
				}else{
					response = false;
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);				
			}
			return response;
		}
	}    
}
