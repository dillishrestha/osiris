//////////////////////////////////////////////////////////
// created on 08/02/2008 at 08:39 a
// Sistema Hospitalario OSIRIS
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
	public class imprime_consumo_productos
	{
		// Declarando el treeview
		Gtk.TreeView lista_resumen_productos;
		Gtk.TreeStore treeViewEngineResumen;
		
		string titulo = "REPORTE DE CONSUMO DE PRODUCTOS";
		
		int contador = 1;
		int numpage = 1;
		string ano_consumo = "";
			
		// Declarando variable de fuente para la impresion
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		Gnome.Font fuente36 = Gnome.Font.FindClosest("Luxi Sans", 36);
		
		public imprime_consumo_productos(object _lista_resumen_productos_,object _treeViewEngineResumen_, string _ano_consumo_)
		{
			lista_resumen_productos = _lista_resumen_productos_ as Gtk.TreeView;
			treeViewEngineResumen =  _treeViewEngineResumen_ as Gtk.TreeStore;
			ano_consumo = _ano_consumo_;
								
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "CONSUMO DE PRODUCTOS", 0);
        	        	
        	//gnome_print_job_new  trabajo   = new gnome_print_job_new();
        	//PrintDialog dialogo   = new PrintDialog (trabajo, "RESUMEN DE FACTURA", 0);
        	
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
                new PrintJobPreview(trabajo, "CONSUMO DE PRODUCTOS").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose ();		
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
			int filas = -75;
			string toma_descrip_prod = "";
			
			ContextoImp.BeginPage("Pagina 1");
			ContextoImp.Rotate(90);
			// Cambiar la fuente
			Gnome.Print.Setfont(ContextoImp,fuente6);
			
			ContextoImp.MoveTo(69.7,-30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");//19.7, 770
			ContextoImp.MoveTo(70, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(69.7, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(70, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(69.7, -50);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(70, -50);			ContextoImp.Show("Conmutador:");
			
			Gnome.Print.Setfont(ContextoImp,fuente11);
			ContextoImp.MoveTo(319.7, -40);			ContextoImp.Show(titulo+" "+ano_consumo);
			ContextoImp.MoveTo(320, -40);			ContextoImp.Show(titulo+" "+ano_consumo);
			Gnome.Print.Setfont(ContextoImp,fuente7);
			ContextoImp.MoveTo(390, -50);			ContextoImp.Show("PAGINA "+numpage+"  Fecha Impresion: "+DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
			Gnome.Print.Setfont(ContextoImp,fuente7);
			filas -= 08;
			ContextoImp.MoveTo(385, filas);	ContextoImp.Show("ENE");
			ContextoImp.MoveTo(410, filas);	ContextoImp.Show("FEB");
			ContextoImp.MoveTo(435, filas);	ContextoImp.Show("MAR");
			ContextoImp.MoveTo(460, filas);	ContextoImp.Show("ABR");
			ContextoImp.MoveTo(485, filas);	ContextoImp.Show("MAY");
			ContextoImp.MoveTo(510, filas);	ContextoImp.Show("JUN");
			ContextoImp.MoveTo(535, filas);	ContextoImp.Show("JUL");
			ContextoImp.MoveTo(560, filas);	ContextoImp.Show("AGO");
			ContextoImp.MoveTo(585, filas);	ContextoImp.Show("SEP");
			ContextoImp.MoveTo(610, filas);	ContextoImp.Show("OCT");
			ContextoImp.MoveTo(635, filas);	ContextoImp.Show("NOV");
			ContextoImp.MoveTo(660, filas);	ContextoImp.Show("DIC");
			ContextoImp.MoveTo(685, filas);	ContextoImp.Show("TOTAL");
			ContextoImp.MoveTo(710, filas);	ContextoImp.Show("PROME.");
			ContextoImp.MoveTo(743, filas);	ContextoImp.Show("$ TOTAL");
			filas -= 08;
			
			TreeIter iter;
			if (treeViewEngineResumen.GetIterFirst (out iter)){
				//Console.WriteLine("Hola");
				ContextoImp.MoveTo(70, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,0));
				toma_descrip_prod = (string) lista_resumen_productos.Model.GetValue (iter,1);
				if(toma_descrip_prod.Length > 61){
					toma_descrip_prod = toma_descrip_prod.Substring(0,60);
				}
				ContextoImp.MoveTo(120, filas);	ContextoImp.Show(toma_descrip_prod);
				ContextoImp.MoveTo(180, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,2));
				// Meses
				ContextoImp.MoveTo(385, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());
				ContextoImp.MoveTo(410, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());
				ContextoImp.MoveTo(435, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());
				ContextoImp.MoveTo(460, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());
				ContextoImp.MoveTo(485, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());
				ContextoImp.MoveTo(510, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());
				ContextoImp.MoveTo(535, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());
				ContextoImp.MoveTo(560, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());
				ContextoImp.MoveTo(585, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());
				ContextoImp.MoveTo(610, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());
				ContextoImp.MoveTo(635, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());
				ContextoImp.MoveTo(660, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());
				ContextoImp.MoveTo(685, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());
				ContextoImp.MoveTo(710, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString("F").Trim());
				ContextoImp.MoveTo(743, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,16));
				//ContextoImp.MoveTo(1050, filas); ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,18));*/
				filas -= 08;	
				while (treeViewEngineResumen.IterNext(ref iter)){
					//Console.WriteLine("Hola");
					ContextoImp.MoveTo(70, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,0));
					toma_descrip_prod = (string) lista_resumen_productos.Model.GetValue (iter,1);
					if(toma_descrip_prod.Length > 61){
						toma_descrip_prod = toma_descrip_prod.Substring(0,60);
					}
					ContextoImp.MoveTo(120, filas);	ContextoImp.Show(toma_descrip_prod);
					ContextoImp.MoveTo(180, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,2));
					// Meses
					ContextoImp.MoveTo(385, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,3)).ToString().Trim());
					ContextoImp.MoveTo(410, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,4)).ToString().Trim());
					ContextoImp.MoveTo(435, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,5)).ToString().Trim());
					ContextoImp.MoveTo(460, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,6)).ToString().Trim());
					ContextoImp.MoveTo(485, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,7)).ToString().Trim());
					ContextoImp.MoveTo(510, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,8)).ToString().Trim());
					ContextoImp.MoveTo(535, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,9)).ToString().Trim());
					ContextoImp.MoveTo(560, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,10)).ToString().Trim());
					ContextoImp.MoveTo(585, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,11)).ToString().Trim());
					ContextoImp.MoveTo(610, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,12)).ToString().Trim());
					ContextoImp.MoveTo(635, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,13)).ToString().Trim());
					ContextoImp.MoveTo(660, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,14)).ToString().Trim());
					ContextoImp.MoveTo(685, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,15)).ToString().Trim());
					ContextoImp.MoveTo(710, filas);	ContextoImp.Show(float.Parse((string) lista_resumen_productos.Model.GetValue (iter,18)).ToString("F").Trim());
					ContextoImp.MoveTo(743, filas);	ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,16));
					//ContextoImp.MoveTo(1050, filas); ContextoImp.Show((string) lista_resumen_productos.Model.GetValue (iter,18));*/
					filas -= 08;
				}
			}			
			ContextoImp.ShowPage();
		}
	}
}
