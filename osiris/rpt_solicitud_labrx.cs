///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
// created on 22/09/2010 at 17:20 p
// 				
// Autor    	: Ing. Daniel Olivares C. GTKPrint con Pango y Cairo arcangeldoc@gmail.com
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
	public class rpt_solicitud_labrx
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		int linea_detalle1;
		int linea_detalle2;
		string departament;
		string agrupacion_lab_rx;
		string query_general;
		
		string connectionString;
        string nombrebd;
		
		class_public classpublic = new class_public();
		class_conexion conexion_a_DB = new class_conexion();
		
		protected Gtk.Window MyWinError;
		
		public rpt_solicitud_labrx (string departament_,int id_tipoadmisiones_,string agrupacion_lab_rx_,string query_numerosolicitud)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			departament = departament_;
			agrupacion_lab_rx = agrupacion_lab_rx_;
			
			query_general = "SELECT osiris_erp_movcargos.folio_de_servicio,osiris_his_solicitudes_labrx.area_quien_solicita,osiris_his_solicitudes_labrx.folio_de_solicitud,"+
							"osiris_his_solicitudes_labrx.fechahora_solicitud,osiris_his_solicitudes_labrx.folio_de_servicio AS foliodeservicio,osiris_his_solicitudes_labrx.pid_paciente AS pidpaciente,"+
							"osiris_his_solicitudes_labrx.id_quien_solicito,osiris_his_solicitudes_labrx.id_proveedor,osiris_his_solicitudes_labrx.id_producto,osiris_his_solicitudes_labrx.cantidad_solicitada,"+
							"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
							"osiris_his_paciente.sexo_paciente,osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_erp_movcargos.nombre_de_cirugia,osiris_erp_cobros_enca.nombre_medico_tratante,"+
							"osiris_erp_cobros_enca.id_habitacion,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.numero_cuarto,osiris_empleado.login_empleado,"+
							"nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,osiris_erp_proveedores.descripcion_proveedor,"+
							"osiris_productos.descripcion_producto "+
							"FROM osiris_his_solicitudes_labrx,osiris_his_paciente,osiris_erp_movcargos,osiris_erp_cobros_enca,osiris_his_habitaciones,osiris_empleado,osiris_erp_proveedores,osiris_productos "+
							"WHERE osiris_his_solicitudes_labrx.id_tipo_admisiones2 = '"+id_tipoadmisiones_.ToString().Trim()+"' "+
							"AND osiris_his_solicitudes_labrx.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_his_solicitudes_labrx.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio " +
							"AND osiris_his_solicitudes_labrx.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion "+
							"AND osiris_his_solicitudes_labrx.id_quien_solicito = osiris_empleado.login_empleado "+
							"AND osiris_his_solicitudes_labrx.id_proveedor = osiris_erp_proveedores.id_proveedor "+
							"AND osiris_his_solicitudes_labrx.id_producto = osiris_productos.id_producto "+
							query_numerosolicitud;
			
			print = new PrintOperation ();
			print.JobName = departament;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);	
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			//para imprimir horizontalmente el reporte
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
			string sexopaciente = "";
			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
				comando.CommandText = query_general;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
		       	if (lector.Read()){
					if (lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "HOMBRE";
					}else{
						sexopaciente = "MUJER";
					}
					comienzo_linea = 05;
					imprime_encabezado(cr,layout);
					comienzo_linea = 55;
					
					imprime_cuerpo(cr,layout, (string) lector["area_quien_solicita"],lector["folio_de_solicitud"].ToString().Trim(),
					               lector["fechahora_solicitud"].ToString().Trim(),lector["foliodeservicio"].ToString().Trim(),lector["pidpaciente"].ToString().Trim(),
					               lector["nombre_completo"].ToString().Trim(),lector["fechanacpaciente"].ToString().Trim(),lector["edadpaciente"].ToString().Trim(),
					               sexopaciente,lector["descripcion_diagnostico_movcargos"].ToString().Trim(),lector["nombre_de_cirugia"].ToString().Trim(),lector["nombre_medico_tratante"].ToString().Trim(),
					               lector["descripcion_cuarto"].ToString().Trim()+" "+lector["numero_cuarto"].ToString().Trim(),lector["id_quien_solicito"].ToString().Trim(),
					               lector["nombresolicitante"].ToString().Trim(),lector["descripcion_proveedor"].ToString().Trim());
					               
					linea_detalle1 = comienzo_linea;
					linea_detalle1 += separacion_linea;
					cr.MoveTo(07*escala_en_linux_windows,linea_detalle1*escala_en_linux_windows);		layout.SetText(lector["cantidad_solicitada"].ToString().Trim()+ " " +lector["id_producto"].ToString().Trim()+" "+lector["descripcion_producto"].ToString().Trim());						Pango.CairoHelper.ShowLayout (cr, layout);
					
					comienzo_linea = 395;			
					imprime_encabezado(cr,layout);
					comienzo_linea = 395+55;
					imprime_cuerpo(cr,layout, (string) lector["area_quien_solicita"],lector["folio_de_solicitud"].ToString().Trim(),
					               lector["fechahora_solicitud"].ToString().Trim(),lector["foliodeservicio"].ToString().Trim(),lector["pidpaciente"].ToString().Trim(),
					               lector["nombre_completo"].ToString().Trim(),lector["fechanacpaciente"].ToString().Trim(),lector["edadpaciente"].ToString().Trim(),
					               sexopaciente,lector["descripcion_diagnostico_movcargos"].ToString().Trim(),lector["nombre_de_cirugia"].ToString().Trim(),lector["nombre_medico_tratante"].ToString().Trim(),
					               lector["descripcion_cuarto"].ToString().Trim()+" "+lector["numero_cuarto"].ToString().Trim(),lector["id_quien_solicito"].ToString().Trim(),
					               lector["nombresolicitante"].ToString().Trim(),lector["descripcion_proveedor"].ToString().Trim());
					linea_detalle2 = comienzo_linea;
					linea_detalle2 += separacion_linea;					
					cr.MoveTo(07*escala_en_linux_windows,linea_detalle2*escala_en_linux_windows);		layout.SetText(lector["cantidad_solicitada"].ToString().Trim()+ " " +lector["id_producto"].ToString().Trim()+" "+lector["descripcion_producto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);

					while (lector.Read()){
						linea_detalle1 += separacion_linea;
						cr.MoveTo(07*escala_en_linux_windows,linea_detalle1*escala_en_linux_windows);		layout.SetText(lector["cantidad_solicitada"].ToString().Trim()+ " " +lector["id_producto"].ToString().Trim()+" "+lector["descripcion_producto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
						linea_detalle2 += separacion_linea;					
						cr.MoveTo(07*escala_en_linux_windows,linea_detalle2*escala_en_linux_windows);		layout.SetText(lector["cantidad_solicitada"].ToString().Trim()+ " " +lector["id_producto"].ToString().Trim()+" "+lector["descripcion_producto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);				
			}			
		}
	 
		void imprime_cuerpo(Cairo.Context cr,Pango.Layout layout,string areaquiensolicita,string numerosolicitud,string fechasolicitud, 
		                    string numerodeatencion, string numeroexpediente, string nombrepaciente, string fechanacimiento, string edadpaciente, 
		                    string sexodelpaciente, string descripciondiagnostico, string nombredecirugia, string medicotratante, string numerohabitacion,
		                    string quiensolicito, string nomsolicitante, string nombregabinete)
		{			
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									 
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Area quien Solicito: "+areaquiensolicita);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: ");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Envio: "+fechasolicitud);						Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea-separacion_linea*escala_en_linux_windows);		layout.SetText("N° de Solicitud: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numerodeatencion);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+numeroexpediente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Nacimiento: "+fechanacimiento);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: "+sexodelpaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Procedimiento: "+nombredecirugia);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Diagnostico Admision: "+descripciondiagnostico);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Medico Tratante: "+medicotratante);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: "+numerohabitacion);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Usuario: "+quiensolicito);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nom. Solicitante: "+nomsolicitante);					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Gabinete o Proveedor : "+nombregabinete);			Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Estudio(s) Solicitado(s) : ");							Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(150*escala_en_linux_windows,(comienzo_linea+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Firma Solicitante");							Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.5;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*19))*escala_en_linux_windows);		layout.SetText(nombrepaciente);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*20))*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			if(medicotratante != ""){
				cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*21))*escala_en_linux_windows);		layout.SetText("Dr. "+medicotratante);				Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Fecha: "+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));				Pango.CairoHelper.ShowLayout (cr, layout);

			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			//Console.WriteLine(comienzo_linea.ToString());
			cr.Rectangle (05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows, 565*escala_en_linux_windows, 180*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.5;
			cr.Stroke();
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//---image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
								
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(479*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			
			comienzo_linea += separacion_linea;
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(479*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
	
			comienzo_linea += separacion_linea;
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
						
			comienzo_linea += separacion_linea;
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(200*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(departament);				Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			
			cr.MoveTo(565*escala_en_linux_windows, 383*escala_en_linux_windows);
			cr.LineTo(05,383);		// Linea Horizontal 4
			
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
		}	
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}
