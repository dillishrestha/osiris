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
// Programa		: facturador.cs
// Proposito	: Facturador general
// Objeto		: tesoreria.cs
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
	public class imprime_formato_factura
	{
		public string numerofactura = "";
		public Gtk.TreeView treeview_detalle_de_factura;
		public Gtk.TreeStore treeViewEngineDetaFact;
		public string nombrecliente = "";
		public string rfccliente = "";
		public string curpcliente = "";
		public string cpcliente = "";
		public string direccioncliente = "";
		public string coloniacliente = "";
		public string municipiocliente = "";
		public string estadocliente = "";
		public string telefonocliente = "";
		public string faxcliente = "";
		public string subtotal_15 = "";
		public string subtotal_0 = "";
		public string totaliva = "";
		public string subtotal = "";
		public string deducible = "";
		public string coaseguroporcentage = "";
		public string totalcoaseguro = "";
		public string totalfactura = "";
		public string catidadenletras = "";
		public string fechafactura = "";
		public string LoginEmpleado = "";
		
		// Declarando variable de fuente para la impresion
		public Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		public Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		public Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		public Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		public Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		public Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		public Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		public Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		protected Gtk.Window MyWin;
		
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
			
			if(error_no_existe_ == false){	
				Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
	        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "FORMATO DE FACTURA", 0);
	        	
	        	int respuesta = dialogo.Run ();
	        	
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
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"La factura seleccionada NO EXISTE verifique...");
				msgBox.Run();	msgBox.Destroy();
			}		
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
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
		}
	}
}
