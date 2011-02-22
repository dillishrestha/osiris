// created on 02/08/2007 at 10:25 a
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				 
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
	public class imprime_formato_factura
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		string numerofactura = "";
		Gtk.TreeView treeview_detalle_de_factura;
		Gtk.TreeStore treeViewEngineDetaFact;
		string nombrecliente = "";
		string rfccliente = "";
		string curpcliente = "";
		string cpcliente = "";
		string direccioncliente = "";
		string coloniacliente = "";
		string municipiocliente = "";
		string estadocliente = "";
		string telefonocliente = "";
		string faxcliente = "";
		string subtotal_15 = "";
		string subtotal_0 = "";
		string totaliva = "";
		string subtotal = "";
		string deducible = "";
		string coaseguroporcentage = "";
		string totalcoaseguro = "";
		string totalfactura = "";
		string catidadenletras = "";
		string fechafactura = "";
		string LoginEmpleado = "";
		
		protected Gtk.Window MyWin;
		
		class_public classpublic = new class_public();
		
		public imprime_formato_factura(string _numerofactura_,object _treeview_detalle_de_factura_,string _nombrecliente_,string _rfccliente_,
										string _curpcliente_,string _cpcliente_,string _direccioncliente_,string _coloniacliente_,
										string _municipiocliente_,string _estadocliente_,string _telefonocliente_,string _faxcliente_,
										string _subtotal_15_,string _subtotal_0_,string _totaliva_,string _subtotal_,string _deducible_,
										string _coaseguroporcentage_,string _totalcoaseguro_,string _totalfactura_,string _catidadenletras_,
										object _treeViewEngineDetaFact_,string _fechafactura_,string _LoginEmpleado_,bool error_no_existe_)
		{
			treeview_detalle_de_factura = _treeview_detalle_de_factura_ as Gtk.TreeView;
			treeViewEngineDetaFact =  _treeViewEngineDetaFact_ as Gtk.TreeStore;
			
			numerofactura = _numerofactura_;
			nombrecliente = _nombrecliente_;
			rfccliente = _rfccliente_;
			curpcliente = _curpcliente_;
			cpcliente = _cpcliente_;
			direccioncliente = _direccioncliente_;
			coloniacliente = _coloniacliente_;
			municipiocliente = _municipiocliente_; 
			estadocliente = _estadocliente_;
			telefonocliente = _telefonocliente_; 
			faxcliente = _faxcliente_;
			subtotal_15 = _subtotal_15_;
			subtotal_0 = _subtotal_0_;
			totaliva = _totaliva_;
			subtotal = _subtotal_;
			deducible = _deducible_;
			coaseguroporcentage = _coaseguroporcentage_; 
			totalcoaseguro = _totalcoaseguro_; 
			totalfactura = _totalfactura_;
			catidadenletras = _catidadenletras_;
			fechafactura = _fechafactura_;
			LoginEmpleado = _LoginEmpleado_;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			
			if(error_no_existe_ == false){
				print = new PrintOperation ();
				print.JobName = "Imprime Factura";	// Name of the report
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run(PrintOperationAction.PrintDialog, null);
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"La factura seleccionada NO EXISTE verifique...");
				msgBox.Run();	msgBox.Destroy();
			}		
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
			string toma_descrip_prod = "";
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			/*
			int filas = 735;
			ContextoImp.BeginPage("Pagina 1");
			Gnome.Print.Setfont (ContextoImp,fuente8);
			ContextoImp.MoveTo(500, filas);	ContextoImp.Show(fechafactura);
			filas -= 15;
			filas -= 15;
			ContextoImp.MoveTo(030, filas);		ContextoImp.Show(nombrecliente);
			ContextoImp.MoveTo(497, filas-8);	ContextoImp.Show("CONTADO");
			filas -= 15;
			ContextoImp.MoveTo(030, filas);	ContextoImp.Show(direccioncliente+" "+coloniacliente+" "+cpcliente);
			filas -= 15;
			ContextoImp.MoveTo(030, filas);	ContextoImp.Show(municipiocliente+" "+estadocliente);
			ContextoImp.MoveTo(492, filas-8);	ContextoImp.Show("MONTERREY, NL.");
			filas -= 15;
			ContextoImp.MoveTo(030, filas);	ContextoImp.Show("R.F.C "+rfccliente);
			filas -= 15;
			filas -= 15;
			bool varpaso = false;
			decimal variable_paso_03 = 0;
			Gnome.Print.Setfont (ContextoImp,fuente7);
			TreeIter iter;
			if (treeViewEngineDetaFact.GetIterFirst (out iter)){
				if ((string)treeview_detalle_de_factura.Model.GetValue (iter,3) == ""){
				ContextoImp.MoveTo(090, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,1));
				filas -= 08;	
				}else{
				
				//////
					if((bool) treeview_detalle_de_factura.Model.GetValue (iter,4) == false){
							ContextoImp.MoveTo(090, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,1));
							//ContextoImp.MoveTo(450, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,2));
							if ((string)treeview_detalle_de_factura.Model.GetValue (iter,3) == ""){
								ContextoImp.MoveTo(510, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue(iter,3));
							}else{
								ContextoImp.MoveTo(510, filas);	ContextoImp.Show(decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue(iter,3)).ToString("C")); 
							}						
							filas -= 08;
						}else{
							if(varpaso == false){
								filas = 310;
								varpaso = true;
							}
							ContextoImp.MoveTo(090, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,1));
							if ((string)treeview_detalle_de_factura.Model.GetValue (iter,3) == ""){
								ContextoImp.MoveTo(510, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue(iter,3));
							}else{
								ContextoImp.MoveTo(510, filas);	ContextoImp.Show(decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue(iter,3)).ToString("C")); 
							}
							filas -= 08;
						
					}
				}
				//////
				while (treeViewEngineDetaFact.IterNext(ref iter)){
					if((bool) treeview_detalle_de_factura.Model.GetValue (iter,4) == false){
						ContextoImp.MoveTo(090, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,1));
						//ContextoImp.MoveTo(450, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,2));
						if ((string)treeview_detalle_de_factura.Model.GetValue (iter,3) == ""){
							ContextoImp.MoveTo(510, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue(iter,3));
						}else{
							ContextoImp.MoveTo(510, filas);	ContextoImp.Show(decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue(iter,3)).ToString("C")); 
						}						
						filas -= 08;
					}else{
						if(varpaso == false){
							filas = 310;
							varpaso = true;
						}
						ContextoImp.MoveTo(090, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue (iter,1));
						if ((string)treeview_detalle_de_factura.Model.GetValue (iter,3) == ""){
							ContextoImp.MoveTo(510, filas);	ContextoImp.Show((string) treeview_detalle_de_factura.Model.GetValue(iter,3));
						}else{
							ContextoImp.MoveTo(510, filas);	ContextoImp.Show(decimal.Parse((string)treeview_detalle_de_factura.Model.GetValue(iter,3)).ToString("C")); 
						}
						filas -= 08;
					}
				}
			}
			Gnome.Print.Setfont (ContextoImp,fuente8);
			filas = 222;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(subtotal_15.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(030, filas+2);	ContextoImp.Show("("+catidadenletras.ToUpper()+")");
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(subtotal_0.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(totaliva.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(subtotal.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(deducible.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(totalcoaseguro.PadLeft(10));
			filas -= 14;
			ContextoImp.MoveTo(500, filas);		ContextoImp.Show(totalfactura.PadLeft(10));
			filas -= (14*6);
			ContextoImp.MoveTo(300, filas);		ContextoImp.Show(LoginEmpleado+"/"+numerofactura.ToString());
			ContextoImp.ShowPage();
			*/ 
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}
