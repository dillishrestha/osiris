///////////////////////////////////////////////////////////
// project created on 16/05/2011
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@gmail.com (Programacion Mono)
//				  Traspaso a Libreria GTKPrint
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
//////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;

namespace osiris
{
    public class imprime_mov_productos
    {
        private static int pangoScale = 1024;
        private PrintOperation print;
        private double fontSize = 8.0;
        int escala_en_linux_windows;        // Linux = 1  Windows = 8
        int comienzo_linea = 70;
        int separacion_linea = 10;
        int numpage = 1;
       
        string connectionString;                       
        string nombrebd;
       
        // Declarando el treeview
        Gtk.TreeView lista_resumen_productos;
        Gtk.TreeStore treeViewEngineResumen;       
       
        string titulo = "MOVIMIENTOS DE PRODUCTOS POR PACIENTE";
        string query1  = "";
		
         //Declaracion de ventana de error:
        protected Gtk.Window MyWinError;
        protected Gtk.Window MyWin;
       
        class_conexion conexion_a_DB = new class_conexion();
        class_public classpublic = new class_public();
                                                                                                                          
        public imprime_mov_productos(string entry_total_aplicado_,string entry_dia1_,string entry_mes1_,string entry_ano1_, string entry_dia2_,string entry_mes2_,string entry_ano2_  ,object _lista_resumen_productos_,object _treeViewEngineResumen_, string _query1_,  string _nombrebd_, string titulopagina_)
        {
            lista_resumen_productos = _lista_resumen_productos_ as Gtk.TreeView;
            treeViewEngineResumen =  _treeViewEngineResumen_ as Gtk.TreeStore;
            query1  = _query1_;
            connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
            nombrebd = conexion_a_DB._nombrebd;
            entry_dia1 = entry_dia1_;
            entry_mes1 = entry_mes1_;
            entry_ano1 = entry_ano1_;
            entry_dia2 = entry_dia2_;
            entry_mes2= entry_mes2_;
            entry_ano2 = entry_ano2_;
            escala_en_linux_windows = classpublic.escala_linux_windows;
           
            print = new PrintOperation ();
            print.JobName = "Movimiento de Productos por Paciente";    // Name of the report
            print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
            print.DrawPage += new DrawPageHandler (OnDrawPage);
            print.EndPrint += new EndPrintHandler (OnEndPrint);
            print.Run(PrintOperationAction.PrintDialog, null);                   
        } 
       
        private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
        {
            print.NPages = 1;  // crea cantidad de copias del reporte           
            // para imprimir horizontalmente el reporte
            print.PrintSettings.Orientation = PageOrientation.Landscape;
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
			float subtota_por_prod = 0;
            Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");                                   
            // cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna                   
            fontSize = 8.0;            layout = null;            layout = context.CreatePangoLayout();
            desc.Size = (int)(fontSize * pangoScale);        layout.FontDescription = desc;
            NpgsqlConnection conexion;
            conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            try{
                imprime_encabezado(cr,layout);
                fontSize = 6.0;            layout = null;            layout = context.CreatePangoLayout();
                desc.Size = (int)(fontSize * pangoScale);        layout.FontDescription = desc;
                conexion.Open ();
                NpgsqlCommand comando;
                comando = conexion.CreateCommand ();
                comando.CommandText = query1;
                Console.WriteLine(comando.CommandText.ToString());
                NpgsqlDataReader lector = comando.ExecuteReader();
                string codigoprod= "";
                if (lector.Read()){
					codigoprod  = (string) lector["idproducto"];
                    cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["idproducto"]);                    Pango.CairoHelper.ShowLayout (cr, layout);
                    cr.MoveTo(75*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["descripcion_producto"]);        Pango.CairoHelper.ShowLayout (cr, layout);
                    comienzo_linea += separacion_linea;
					cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["foliodeservicio"]);        Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(55*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["pidpaciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
                    cr.MoveTo(105*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["nombre_paciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText(lector ["cantidadaplicada"].ToString());        Pango.CairoHelper.ShowLayout (cr, layout);
                    comienzo_linea += separacion_linea;
					salto_de_pagina(cr,layout);
					subtota_por_prod = float.Parse(lector ["cantidadaplicada"].ToString());
                    while (lector.Read()){
						if(codigoprod  != (string) lector["idproducto"]){
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText(subtota_por_prod.ToString());                    Pango.CairoHelper.ShowLayout (cr, layout);
							
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
                            codigoprod  = (string) lector["idproducto"];							
                            cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["idproducto"]);                    Pango.CairoHelper.ShowLayout (cr, layout);
                            cr.MoveTo(75*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["descripcion_producto"]);        Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["foliodeservicio"]);        Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(55*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["pidpaciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
		                    cr.MoveTo(105*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["nombre_paciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText(lector ["cantidadaplicada"].ToString());        Pango.CairoHelper.ShowLayout (cr, layout);
		                    comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							subtota_por_prod = float.Parse(lector ["cantidadaplicada"].ToString());
						}else{
							cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["foliodeservicio"]);        Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(55*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["pidpaciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
                       		cr.MoveTo(105*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText((string) lector["nombre_paciente"]);        Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);            layout.SetText(lector ["cantidadaplicada"].ToString());        Pango.CairoHelper.ShowLayout (cr, layout);
                       		comienzo_linea += separacion_linea;
							salto_de_pagina(cr,layout);
							subtota_por_prod += float.Parse(lector ["cantidadaplicada"].ToString());
						}
                    }
                }
            }catch (NpgsqlException ex){
                MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
                                                                MessageType.Error,
                                                                ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
                msgBoxError.Run ();        msgBoxError.Destroy();
            }
        }
       
        void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
        {
            Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");                               
            //cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna                   
            fontSize = 8.0;
            desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
            layout.FontDescription.Weight = Weight.Bold;        // Letra negrita
            cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);            layout.SetText(classpublic.nombre_empresa);            Pango.CairoHelper.ShowLayout (cr, layout);
            cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);            layout.SetText(classpublic.direccion_empresa);        Pango.CairoHelper.ShowLayout (cr, layout);
            cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);            layout.SetText(classpublic.telefonofax_empresa);    Pango.CairoHelper.ShowLayout (cr, layout);
            fontSize = 6.0;
            desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
            cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);            layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));        Pango.CairoHelper.ShowLayout (cr, layout);
            cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);            layout.SetText("N° Page :"+numpage.ToString().Trim());        Pango.CairoHelper.ShowLayout (cr, layout);
            cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);            layout.SetText("Sistema Hospitalario OSIRIS");        Pango.CairoHelper.ShowLayout (cr, layout);
            // Cambiando el tamaño de la fuente           
            fontSize = 10.0;
            desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
            cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);            layout.SetText(titulo);                    Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
            desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
        }
       
        void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)           
        {
            if(comienzo_linea >530){
                cr.ShowPage();
                Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");                               
                fontSize = 8.0;        desc.Size = (int)(fontSize * pangoScale);                    layout.FontDescription = desc;
                comienzo_linea = 70;
                numpage += 1;
                imprime_encabezado(cr,layout);
            }
        }
       
        private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
        {
        }
    }
}                               