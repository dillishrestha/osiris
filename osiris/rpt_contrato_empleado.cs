using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

namespace osiris
{
	public class rpt_contrato_empleado
	{
	   public string tiempocontrato;
	   public string tiempoc;
	   public string tipocontrato ;
	   public string appaterno ;
	   public string apmaterno ;
	   public string nom1 ;
	   public string nom2 ;
	   public string edad ;
	   public string direccion;
	   public string colonia;
	   public string calle;
	   public string numero;
	   public string depto;
	   public string puesto;
	   public string jornada;
	   public string funcion;
	   public string tiempocomida;
	   public string fechacontrato;
	   public string sueldo;
	   public string tipopago;
	   public string fechacobro;
	   public string tipopagosubstring;
	   public string nacionalidad;
	
	  public rpt_contrato_empleado(string tipocontrato_ , 
	                               string appaterno_, 
	                               string apmaterno_,
	                               string nom1_,
	                               string nom2_,
	                               string edad_,
	                               string calle_,
	                               string colonia_,
	                               string numero_,
	                               string funcion_,
	                               string depto_,
	                               string puesto_,
	                               string jornada_,
	                               string tiempocomida_,
	                               string fechacontrato_,
	                               string sueldo_,
	                               string tipopago_,
	                               string nacionalidad_
	                               )
		{
		
		tipocontrato = tipocontrato_;
		appaterno = appaterno_;
		apmaterno = apmaterno_;
		nom1 = nom1_;
		nom2 = nom2_;
		edad = edad_;
		nacionalidad = nacionalidad_;
		//direccion = calle_+" #"+numero_+", Colonia: "+colonia_;
		calle=calle_;
		numero=numero_;
		colonia=colonia_;
		depto = depto_;
		funcion = funcion_;
		puesto = puesto_;
		jornada = jornada_;
		tiempocomida = tiempocomida_;
		fechacontrato = fechacontrato_;
		sueldo = sueldo_;
		tipopago = tipopago_;
		tipopagosubstring = tipopago.Substring( tipopago.IndexOf("(")+1 , tipopago.Length -  tipopago.IndexOf("(")-2 );
		
		if (tipopagosubstring == "QUINCENAL" )
		{fechacobro = "Quince y ultimo del mes";}
		if (tipopagosubstring == "SEMANAL" )
		{fechacobro = "Sabados del mes";}
		
		
		Console.WriteLine("clase imprimir");
		Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
   		Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "CONTRATO DE EMPLEADO", 0);
       	
       	
       	
       	int respuesta = dialogo.Run ();

			    if (respuesta == (int) PrintButtons.Cancel) 
					{   //boton Cancelar
						Console.WriteLine("Impresión cancelada");
						dialogo.Hide (); 
						dialogo.Dispose (); 
						return;
					}
	        Gnome.PrintContext ctx = trabajo.Context;
   	     	ComponerPagina(ctx, trabajo); 
	       	trabajo.Close();
			        switch (respuesta)
			        {   //imprimir
		   	            case (int) PrintButtons.Print:   
						trabajo.Print (); 
		           	    break;
		           	    //vista previa
		                case (int) PrintButtons.Preview:
		                Console.WriteLine ("vista previa");
		                
						new PrintJobPreview(trabajo, "CONTRATO DE EMPLEADO").Show();
						
		                //dialogo.Icon = Gdk.Pixbuf.LoadFromResource("/home/egonzalez/Desktop/osiris/blam.png");
		                
		                break;
		        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
		
		
		
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{ 
		
		
		
		         
		        ContextoImp.BeginPage("Pagina 1");
			// Crear una fuente 
				  
				//Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
				//Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 10);
				// ESTA FUNCION ES PARA QUE EL TEXTO SALGA EN NEGRITA
				Gnome.Font fuente4 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 12);
				Gnome.Font fuente5 = Gnome.Font.FindClosestFromWeightSlant("Bitstream Vera Sans", FontWeight.Bold ,false, 10);
				//Encabezado de pagina
			 
			
			    
			    Gnome.Print.Setfont (ContextoImp, fuente4);
			    
				ContextoImp.MoveTo(150, 750);
			    ContextoImp.Show("CONTRATO INDIVIDUAL DE TRABAJO");
			    
			    
			   // Console.WriteLine(this.tipocontrato.Substring(0,this.tipocontrato.Length));
			    
			   // if para tipo de contrato
			    if (tipocontrato == "PRACTICAS")
			    {ContextoImp.MoveTo(230, 735);
			    ContextoImp.Show("PRACTICAS");
			    tiempoc = "tiempo determinado";}
			    else {
			    
					    if(tipocontrato == "HONORARIOS (ASIMILABLES)")
					    {ContextoImp.MoveTo(170, 735);
					    ContextoImp.Show( tipocontrato);
					    tiempocontrato = "determinado";
					    tiempoc = "determinado";}
					    else {
						     if(tipocontrato=="INDETERMINADO")
						     {ContextoImp.MoveTo(160, 735);
						     ContextoImp.Show("POR TIEMPO " + tipocontrato);
						     tiempoc = "tiempo " + tipocontrato;
						     Gnome.Print.Setfont (ContextoImp, fuente2);
						     
							 ContextoImp.MoveTo(20, 660);   
						     ContextoImp.Show("que hemos convenido en celebrar un Contrato Individual de Trabajo y como personal "+ funcion+ " al tenor de las");
						     ContextoImp.MoveTo(20, 650);
						     ContextoImp.Show("siguientes:");
						     
						     ContextoImp.MoveTo(20, 540);
		                     ContextoImp.Show("SEGUNDA: La vigencia del presente contrato es por tiempo Indeterminado a partir del "+fechacontrato);  

		                     }
						     
						        else{
						        ContextoImp.MoveTo(140, 735);
						        ContextoImp.Show("POR TIEMPO " + tipocontrato);
						        tiempocontrato = "Indeterminado";
						        tiempoc = "tiempo " +tipocontrato;
						        Gnome.Print.Setfont (ContextoImp, fuente2);
						        
						        ContextoImp.MoveTo(20, 660);
								ContextoImp.Show("que hemos convenido en celebrar un Contrato Individual de Trabajo por " + tiempoc+" y como personal ");
						        ContextoImp.MoveTo(20, 650);
						        ContextoImp.Show(funcion+ " al tenor de las siguientes:");
						        
						        ContextoImp.MoveTo(20, 540);
		                        ContextoImp.Show("SEGUNDA: La vigencia del presente contrato es de 1 a 3 meses por tiempo determinado a partir del "+fechacontrato);  
		
						        
						        }
						      }  
					  
			     }
		        
			    //contrato
			    
			  
			    Gnome.Print.Setfont (ContextoImp, fuente2);
			    
   
			    
				ContextoImp.MoveTo(20, 700);
				ContextoImp.Show("En la ciudad de Monterrey, Nuevo León  a " + DateTime.Today.ToLongDateString()+ ", los que suscribimos el presente, por una parte HOSPITAL ");
				ContextoImp.MoveTo(20, 690);
				ContextoImp.Show("SANTA CECILIA DE MONTERREY, S.A. DE C.V. con registro Federal de Contribuyentes de HSC021105D5A  con una actividad");
				ContextoImp.MoveTo(20, 680);
		        ContextoImp.Show("de Servicios Hospitalarios y con domicilio fiscal en Galeana 641-A Nte. Colonia Centro que en lo sucesivo se le denominará"); 
		        ContextoImp.MoveTo(20, 670);
		        ContextoImp.Show("''EL PATRON'' y por la otra, a "+ appaterno+ " " + apmaterno+ " " +nom1 + " " +nom2 + " su propio derecho como TRABAJADOR, hacemos constar");
		
		        //linea en el if
		        //linea en el if
		        ContextoImp.MoveTo(20, 620);
		        ContextoImp.Show("CLAUSULAS:");
		        ContextoImp.MoveTo(20, 600);
		        ContextoImp.Show("PRIMERA: Para los efectos del articulo 25 de la Ley Federal del Trabajo, el HOSPITAL SANTA CECILIA DE MONTERREY, S.A. ");
		        ContextoImp.MoveTo(20, 590);
		        ContextoImp.Show("DE C.V. El trabajador declara llamarse, "+ appaterno+ " " +apmaterno+ " "+nom1+ " "+nom2+" de "+edad+" años de edad, nacionalidad "); 
		        ContextoImp.MoveTo(20, 580);
		        ContextoImp.Show(nacionalidad + ", y con domicilio en calle " + calle+" #"+numero+", Colonia: "+colonia + " el prestador se obliga a realizar");
		        ContextoImp.MoveTo(20, 570);
		        ContextoImp.Show("los trabajos de tipo de funcion "+funcion + ", que se le asignen en el area de "+depto+" ocupando " );
		        ContextoImp.MoveTo(20, 560);
		        ContextoImp.Show("el puesto de "+puesto+ " para los cuales fue contratado.");
		        
		        
		       
		        //segunda clausula esta en el if
		        
		        ContextoImp.MoveTo(20, 520);
		        ContextoImp.Show("TERCERO: El prestador se obliga a brindar los servicios personales que se especifican en la cláusla anterior, subordinado ");
		        ContextoImp.MoveTo(20, 510);
		        ContextoImp.Show("jurídicamente al patrón, con esmero y eficiencia, en las oficinas (o talleres) del patrón y en cualquier lugar de esta ciudad");
		        ContextoImp.MoveTo(20, 500);
		        ContextoImp.Show("donde el patrón desempeñe actividades queda expresamenteconvenido que el trabajador acatará en el desempeño de su ");
		        ContextoImp.MoveTo(20, 490);
		        ContextoImp.Show("trabajo todas las disposiciones del Reglamento Interior de Trabajo,  todas las ordenes, circulares y disposiciones que dicte"); 
		        ContextoImp.MoveTo(20, 480);
		        ContextoImp.Show("el patrón y todos los ordenamientos legales que le sean aplicables");
		        
		        
		        ContextoImp.MoveTo(20, 460);
		        ContextoImp.Show("CUARTA: La duración de la jornada será inicialmente de "+jornada+" horas diarias, teniendo "+tiempocomida + " para ingerir sus alimentos.");
		        ContextoImp.MoveTo(20, 450);
		        ContextoImp.Show("El horario se podrá modificar de acuerdo a las necesidades de la institución y/o el servicio.");
		        
		        ContextoImp.MoveTo(20, 430);
		        ContextoImp.Show("QUINTA: El tiempo extra se autorizará solo previa solicitud por escrito siempre y cuando se justifique la tarea a realizar.");
		        ContextoImp.MoveTo(20, 420);
		        ContextoImp.Show("Está deberá ser solicitada por el jefe inmediato y autorizada por el Cordinador del área."); 
		
		        ContextoImp.MoveTo(20, 400);
		        ContextoImp.Show("SEXTA: El trabajador esta obligado a registrar la entrada y salida de sus labores, por lo que el incumplimiento de este registro "); 
		        ContextoImp.MoveTo(20, 390);
		        ContextoImp.Show("indicara la falta de injusticia a sus labores, para todos los efectos legales.");
		        
		        ContextoImp.MoveTo(20, 370);
		        ContextoImp.Show("SEPTIMA: El trabajador percibirá por la prestación de los servicios a que se refiere este contrato, un salario nominal de "); 
		        ContextoImp.MoveTo(20, 360);
		        ContextoImp.Show("$"+sueldo+" pesos mensuales los cuales cobrara "+tipopagosubstring+ " y al cual se aplicará la parte proporcional correspondiente al ");
		        ContextoImp.MoveTo(20, 350);
		        ContextoImp.Show("descanso semanal, conforme a lo dispuesto en el artículo 72 de la Ley Federal del Trabajo."); 
		        ContextoImp.MoveTo(20, 340);
		        ContextoImp.Show("El sueldo se le cubrirá los días "+fechacobro+", en moneda de curso legal y en las oficinas del patrónestando obligados ");
		        ContextoImp.MoveTo(20, 330);
		        ContextoImp.Show("el trabajador a firmar las constancias de pago respectivas, teniendo en cuenta lo dispuesto en los articulos 108 y 109 ");
		        ContextoImp.MoveTo(20, 320);
		        ContextoImp.Show("de la Ley de la Materia.");
		        
		        ContextoImp.MoveTo(20, 300);
		        ContextoImp.Show("OCTAVA: Por cada seis días de trabajo el trabajador tendrá un descanso semanal de un día con pago de salario íntegro, ");
		        ContextoImp.MoveTo(20, 290);
		        ContextoImp.Show("deacuerdo al que se le asigne en el área. También disfrutara de los días de descandso obligatorios señalados en Ley Federal ");
		        ContextoImp.MoveTo(20, 280);
		        ContextoImp.Show("del Trabajo que caigan dentro de la temporalidad de este contrato.");



		        ContextoImp.MoveTo(20, 260);
		        ContextoImp.Show("NOVENA: El trabajador percibirá por concepto de vacaciones una remuneración ''proporcionada al tiempo de servicios prestados");
		        ContextoImp.MoveTo(20, 250);
		        ContextoImp.Show("con una prima del 25%'' sobre el salario correspondiente a las mismas, teniendo en cuenta el término de la relaciacón de trabajo");
		        ContextoImp.MoveTo(20, 240);
		        ContextoImp.Show("con arreglo a lo dispuesto en los articulos 76/79 y 80 de la Ley Federal del Trabajo. Tambien percibira con base en un aguinaldo");
		        ContextoImp.MoveTo(20, 230);
		        ContextoImp.Show("anual fijado el equivalente a 15 días de salario, la parte proporcional el tiempo trabajado, conforme al parrafo segundo  del ");
		        ContextoImp.MoveTo(20, 220);
		        ContextoImp.Show("articulo 87 de la Ley Federal del Trabajo en vigor.");
		        
		        ContextoImp.MoveTo(20, 200);
		        ContextoImp.Show("DECIMA: En caso de faltas injustificadas al trabajo, se podrán deducir dichas faltas del periodo de prestación de servicios ");
		        ContextoImp.MoveTo(20, 190);
		        ContextoImp.Show("computables para fijar las vacaciones, reduciendose estas proporcionalmente.");
		     
		        ContextoImp.MoveTo(20, 170);
		        ContextoImp.Show("DECIMA PRIMERA: El trabajador conviene en someterse a los reconocimientos médicos que periódicamente ordene el patrón");
		        ContextoImp.MoveTo(20, 160);
		        ContextoImp.Show("en los términos de la Fracción X de articulo 134 de la Ley Federal del Trabajo, en la inteligencia de que el médico que practique");
		        ContextoImp.MoveTo(20, 150);
		        ContextoImp.Show("será retribuido por el patrón.");
		        
		        ContextoImp.MoveTo(20,130);
		        ContextoImp.Show("DECIMA SEGUNDA: El trabajador será capacitado o adiestrado en los términos de los planes y programas establecidos que se ");
		        ContextoImp.MoveTo(20,120);
		        ContextoImp.Show("establezcan por el patrón.");
		        
		        ContextoImp.MoveTo(20,100);
		        ContextoImp.Show("DECIMA TERCERA: Son causa de rescicion de contrato el incumplimiento a cualquiera de sus dispociciones establecidas que se ");
		        ContextoImp.MoveTo(20,90);
		        ContextoImp.Show("establezcan por el patrón.");
		        
		        
		        ContextoImp.ShowPage();
		        ContextoImp.BeginPage("Pagina 2");
		        Gnome.Print.Setfont (ContextoImp, fuente2);
		        
                	      
		        ContextoImp.MoveTo(20,750 );
		        ContextoImp.Show("DECIMO CUARTA: Ambas partes convienen en que al vencimiento del término estipulado en este contrato quedará terminado ");
		        ContextoImp.MoveTo(20,740 );
		        ContextoImp.Show("automáticamente, sin necesidad de aviso ni de ningún otro requisito, y cesarán todos sus efectos, de acuerdo con la fracción III");
		        ContextoImp.MoveTo(20,730 );
		        ContextoImp.Show("del artículo 53 de la Ley Federal del Trabajo.");
		     
		        ContextoImp.MoveTo(20,710 );
		        ContextoImp.Show("DECIMO QUINTA: Este contrato se podra terminar sin responsabilidad para las partes con quince dias naturales de anticipación");
		        ContextoImp.MoveTo(20,700);
		        ContextoImp.Show("mediante aviso por escrito.");
		        
		        ContextoImp.MoveTo(20,680);
		        ContextoImp.Show("DECIMO SEXTA: Ambas partes declararan que respecto a las obligaciones y derechos que mutuamente les corresponden y que no hayan ");
		        ContextoImp.MoveTo(20,670);
		        ContextoImp.Show("sido motivo de la cláusula expresa en el presente contrato se sujetan a las dispociciones de la Ley de la Materia.");
		  
		        ContextoImp.MoveTo(20,650);
		        ContextoImp.Show("Leído que fue por ambas partes éste contrato, impuestas de su contenido, lo firmaran al calce para su Constancia Legal el dia ");
		        ContextoImp.MoveTo(20,640);
		        ContextoImp.Show(DateTime.Today.Day.ToString() + " del mes "+ DateTime.Today.ToLongDateString().Substring(3,DateTime.Today.ToLongDateString().Length -11) + " del año "+ DateTime.Today.Year.ToString());
		        
		        ContextoImp.MoveTo(150,90);
		        
		       
		    
		        Gnome.Print.Setfont (ContextoImp, fuente4);
		        ContextoImp.MoveTo(160, 460);
		        ContextoImp.Show("Monterrey, N.L. a " + DateTime.Today.ToLongDateString());
		        
		        ContextoImp.MoveTo(80,300);
		        ContextoImp.Show("EL PATRON");
		        ContextoImp.MoveTo(400,300);
		        ContextoImp.Show("EL PRESTADOR");
		        
		        
		        Gnome.Print.Setfont (ContextoImp, fuente3);
		        ContextoImp.MoveTo(20,275);
		        ContextoImp.Show("HOSPITAL SANTA CECILIA DE MONTERREY,S.A. DE C.V.");
		        ContextoImp.MoveTo(20,260);
		        ContextoImp.Show("REPRESENTANTE LEGAL");
		        ContextoImp.MoveTo(90,185);
		        ContextoImp.Show("TESTIGOS");
		        ContextoImp.MoveTo(20,140);
		        ContextoImp.Show("________________________________________");
		        ContextoImp.MoveTo(30,125);
		        ContextoImp.Show("LIC. BLANCA ALICIA FLORES BERNAL");
		        
			    ContextoImp.MoveTo(340,290);
		        ContextoImp.Show("________________________________________");
		        ContextoImp.MoveTo(340,275);
		        ContextoImp.Show(nom1+ " "+nom2+ " " +appaterno+ " " +apmaterno);
		        
		        
		        ContextoImp.MoveTo(420,185);
		        ContextoImp.Show("TESTIGOS");
		        ContextoImp.MoveTo(340,140);
		        ContextoImp.Show("__________________________________________");
		        ContextoImp.MoveTo(340,125);
		        ContextoImp.Show("C.P. JUAN ANTONIO GONZALEZ GOMEZ");
		        
		        Gnome.Print.Setfont (ContextoImp, fuente5);
		        ContextoImp.MoveTo(40,110);
		        ContextoImp.Show("JEFA DE RECURSOS HUMANOS");
		        ContextoImp.MoveTo(370,110);
		        ContextoImp.Show("GENERADOR DE NOMINAS");
		        
		        ContextoImp.ShowPage();
        		
		}
		
		
		
	}
}