
using System;
using Gtk;
using Npgsql;
using System.Data;
using Glade;
using Gnome;

namespace osiris
{
	public class rpt_ocupacion_hospitalaria
	{
		string tiporeporte = "SINALTA";
		string titulo = "REPORTE DE PACIENTES SIN ALTA";
		
		int fila = -70;
		int contador = 1;
		int numpage = 1;
		
		decimal sumacuenta = 0;
		decimal totabono = 0;
		
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		Gnome.Font fuente6 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
		//Gnome.Font fuente7 = Gnome.Font.FindClosest("Bitstream Vera Sans", 7);
		//Gnome.Font fuente8 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
		Gnome.Font fuente9 = Gnome.Font.FindClosest("Bitstream Vera Sans", 9);
		Gnome.Font fuente10 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
		Gnome.Font fuente11 = Gnome.Font.FindClosest("Bitstream Vera Sans", 11);
		//Gnome.Font fuente12 = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
		//Gnome.Font fuente36 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
		
		private TreeStore treeViewEngineocupacion;
		
		public rpt_ocupacion_hospitalaria (object treeViewEngineocupacion_,decimal sumacuenta_,decimal totabono_)
		{
			treeViewEngineocupacion = (object) treeViewEngineocupacion_ as Gtk.TreeStore;
			sumacuenta = sumacuenta_;
			totabono = totabono_;
			
			titulo = "REPORTE DE OCUPACION";
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, titulo, 0);
        	int         respuesta = dialogo.Run ();
        	if (respuesta == (int) Gnome.PrintButtons.Cancel){
				dialogo.Hide (); 		dialogo.Dispose (); 
				return;
			}

        	Gnome.PrintContext ctx = trabajo.Context;        
        	ComponerPagina(ctx, trabajo); 
        	trabajo.Close();
             
        	switch (respuesta)
        	{
                  case (int) Gnome.PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) Gnome.PrintButtons.Preview:
                      	new Gnome.PrintJobPreview(trabajo, titulo).Show();
                        break;
        	}
        	dialogo.Hide (); dialogo.Dispose ();
		}
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			ContextoImp.BeginPage("Pagina 1");
			ContextoImp.Rotate(90);
			imprime_rpt_ocupacion(ContextoImp,trabajoImpresion);
			ContextoImp.ShowPage();
		}
		
		void imprime_rpt_ocupacion(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
				TreeIter iter;
				string tomovalor1 = "";
				fila = -75;
				int contadorprocedimientos = 0;
				contador = 0;
				numpage = 1;
				imprime_encabezado(ContextoImp,trabajoImpresion);
				if (this.treeViewEngineocupacion.GetIterFirst (out iter)){
					Gnome.Print.Setfont (ContextoImp, fuente6);
					ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
					ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
					ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
					
					ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
					ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
					ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
					
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
					if(tomovalor1.Length > 20){
					tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
					if(tomovalor1.Length > 30){
						tomovalor1 = tomovalor1.Substring(0,30); 
					}
					ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
					if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
					}
					ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
					
					tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
					if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
					}
					ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
					//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
					fila -= 08;
					//contador+=1;
					contadorprocedimientos += 0;
					salto_pagina(ContextoImp,trabajoImpresion);
					
					while (this.treeViewEngineocupacion.IterNext(ref iter)){
						Gnome.Print.Setfont (ContextoImp, fuente6);
						ContextoImp.MoveTo(45, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,2));//pid
						ContextoImp.MoveTo(70, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,1));//folio
						ContextoImp.MoveTo(105, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,3));//fecha ingreso
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,0);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(160,fila);					ContextoImp.Show(tomovalor1);//nombre
						
						ContextoImp.MoveTo(285, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,8));//habitacion
						ContextoImp.MoveTo(325, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,4));//saldo
						ContextoImp.MoveTo(365, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,5));//abono
						
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,6);
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(415,fila);					ContextoImp.Show(tomovalor1);//a pagar
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,7);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(460,fila);					ContextoImp.Show(tomovalor1);//medico
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,9);//diagnostico
						if(tomovalor1.Length > 30){
							tomovalor1 = tomovalor1.Substring(0,30); 
						}
						ContextoImp.MoveTo(540,fila);					ContextoImp.Show(tomovalor1);
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,10);
						if(tomovalor1.Length > 20){
						tomovalor1 = tomovalor1.Substring(0,20); 
						}
						ContextoImp.MoveTo(660,fila);					ContextoImp.Show(tomovalor1);//topo paciente
						
						tomovalor1 = (string) this.treeViewEngineocupacion.GetValue (iter,11);
						if(tomovalor1.Length > 25){
						tomovalor1 = tomovalor1.Substring(0,25); 
						}
						ContextoImp.MoveTo(715,fila);					ContextoImp.Show(tomovalor1);//topo paciente
							
						//ContextoImp.MoveTo(725, fila);	ContextoImp.Show((string) this.treeViewEngineocupacion.GetValue (iter,10));//empresa
						
						fila -= 08;
						contador+=1;
						contadorprocedimientos += 1;
						salto_pagina(ContextoImp,trabajoImpresion);
					}
					fila-=10;
					contador+=1;
					contadorprocedimientos += 1;
					salto_pagina(ContextoImp,trabajoImpresion);					
				}
				Gnome.Print.Setfont (ContextoImp, fuente9);
				ContextoImp.MoveTo(99.5,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(100,fila);				ContextoImp.Show("TOTAL PROC. "+contadorprocedimientos.ToString());
				ContextoImp.MoveTo(219.5,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(220,fila);				ContextoImp.Show("TOT. SALDOS" );
				ContextoImp.MoveTo(295.5,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(296,fila);				ContextoImp.Show(sumacuenta.ToString("C"));
				ContextoImp.MoveTo(384.5,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(385,fila);				ContextoImp.Show("TOT. ABONOS" );
				ContextoImp.MoveTo(459.5,fila);				ContextoImp.Show(totabono.ToString("C"));
				ContextoImp.MoveTo(460,fila);				ContextoImp.Show(totabono.ToString("C"));
				//contadorprocedimientos += 1;
				salto_pagina(ContextoImp,trabajoImpresion);				
		}
		
		void imprime_encabezado(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
	 		// Cambiar la fuente
			Gnome.Print.Setfont (ContextoImp, fuente6);
			ContextoImp.MoveTo(65.5, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(66, -30);			ContextoImp.Show("Sistema Hospitalario OSIRIS");
			ContextoImp.MoveTo(65.5, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(66, -40);			ContextoImp.Show("Direccion:");
			ContextoImp.MoveTo(65.5, -50);			ContextoImp.Show("Conmutador:");
			ContextoImp.MoveTo(66, -50);			ContextoImp.Show("Conmutador:");
			Gnome.Print.Setfont(ContextoImp,fuente11);
			ContextoImp.MoveTo(350.5, -40);			ContextoImp.Show(titulo);
			ContextoImp.MoveTo(351, -40);			ContextoImp.Show(titulo);
			Gnome.Print.Setfont (ContextoImp, fuente10);
			ContextoImp.MoveTo(330.7, -550);		ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			ContextoImp.MoveTo(330, -550);			ContextoImp.Show("Pagina "+numpage.ToString()+"  fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Gnome.Print.Setfont (ContextoImp, fuente9);
			Gnome.Print.Setrgbcolor(ContextoImp, 0,0,0);//regreso color fuente a negro
			imprime_titulo(ContextoImp,trabajoImpresion);
	  	}
		
		void imprime_titulo(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{	
			Gnome.Print.Setfont(ContextoImp,fuente9);
			ContextoImp.MoveTo(55.5, -65);					ContextoImp.Show("Pid"); //| Fecha | Nº Atencion | Paciente | SubTotal al 15 | SubTotal al 0 | IVA | SubTotal Deducible | Coaseguro | Total | Hono. Medico");
			ContextoImp.MoveTo(56, -65);					ContextoImp.Show("Pid");
			
			ContextoImp.MoveTo(80.5, -65);					ContextoImp.Show("Folio");
			ContextoImp.MoveTo(81, -65);					ContextoImp.Show("Folio");//80,-70
			
			ContextoImp.MoveTo(105.5, -65);					ContextoImp.Show("F. Ingreso");
			ContextoImp.MoveTo(106, -65);					ContextoImp.Show("F. Ingreso");
			
			ContextoImp.MoveTo(160.5, -65);					ContextoImp.Show("Nombre");//120,-70
			ContextoImp.MoveTo(161, -65);					ContextoImp.Show("Nombre");//120,-70
			
			ContextoImp.MoveTo(270.5, -65);					ContextoImp.Show("No. Hab.");//170,-70
			ContextoImp.MoveTo(271, -65);					ContextoImp.Show("No. Hab.");
			
			ContextoImp.MoveTo(325.5, -65);					ContextoImp.Show("Saldo");
			ContextoImp.MoveTo(326, -65);					ContextoImp.Show("Saldo");//360,-70
			
			ContextoImp.MoveTo(365.5, -65);					ContextoImp.Show("Abono");
			ContextoImp.MoveTo(366, -65);					ContextoImp.Show("Abono");//
			
			ContextoImp.MoveTo(415.5, -65);					ContextoImp.Show("S. Pend.");
			ContextoImp.MoveTo(416, -65);					ContextoImp.Show("S. Pend.");//360,-70
			
			ContextoImp.MoveTo(460.5, -65);					ContextoImp.Show("Medico");
			ContextoImp.MoveTo(461, -65);					ContextoImp.Show("Medico");//420,-70
			
			ContextoImp.MoveTo(540.5, -65);					ContextoImp.Show("Diagnostico");
			ContextoImp.MoveTo(541, -65);					ContextoImp.Show("Diagnostico");//420,-70
			
			ContextoImp.MoveTo(660.5, -65);					ContextoImp.Show("T. Paciente");
			ContextoImp.MoveTo(661, -65);					ContextoImp.Show("T. Paciente");
			
			ContextoImp.MoveTo(715.5, -65);					ContextoImp.Show("Aseg./Empresa");
			ContextoImp.MoveTo(716, -65);					ContextoImp.Show("Aseg./Empresa");
		} 
		
		void salto_pagina(Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{
	        if (contador > 55 ){
	        	numpage +=1;        	contador=1;
	        	fila = -75;
	        	ContextoImp.ShowPage();
				ContextoImp.BeginPage("Pagina "+numpage.ToString());
				ContextoImp.Rotate(90);
				imprime_encabezado(ContextoImp,trabajoImpresion);
	     	}
		}
	}
}
