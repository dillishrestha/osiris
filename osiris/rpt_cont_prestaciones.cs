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
	public class con_prest
	{
		public string connectionString = "Server=localhost;" +
            	                         "Port=5432;" +
                	                     "User ID=admin;" +
                    	                 "Password=1qaz2wsx;";
        public string nombrebd;
    	public int PidPaciente;
	    public int folioservicio;
	    public string medico;
    
		public con_prest (int PidPaciente_ , int folioservicio_,string _nombrebd_,string doctor)
		{
			nombrebd = _nombrebd_;
			PidPaciente = PidPaciente_;
   		 	folioservicio = folioservicio_;
   		 	medico = doctor;
			Gnome.PrintJob    trabajo   = new Gnome.PrintJob (PrintConfig.Default ());
   		    Gnome.PrintDialog dialogo   = new Gnome.PrintDialog (trabajo, "CONTRATO DE PRESTACIONES", 0);
       		int respuesta = dialogo.Run ();

			if (respuesta == (int) PrintButtons.Cancel) 
			{
				Console.WriteLine("Impresión cancelada");
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
                Console.WriteLine ("vista previa");
				new PrintJobPreview(trabajo, "CONTRATO DE PRESTACIONES").Show();
                        break;
        	}
			dialogo.Hide (); dialogo.Dispose ();
		}
      
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
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
									"hscmty_erp_cobros_enca.folio_de_servicio, "+
									"hscmty_erp_cobros_enca.pid_paciente, "+
									"hscmty_his_paciente.nombre1_paciente, "+
									"hscmty_his_paciente.nombre2_paciente, "+
									"hscmty_his_paciente.apellido_paterno_paciente, "+
									"hscmty_his_paciente.apellido_materno_paciente, "+
									"hscmty_his_paciente.direccion_paciente, "+
									"hscmty_his_paciente.numero_casa_paciente, "+
									"hscmty_his_paciente.numero_departamento_paciente, "+
									"hscmty_his_paciente.colonia_paciente, "+
									"hscmty_his_paciente.codigo_postal_paciente, "+
									"hscmty_his_paciente.municipio_paciente, "+
									"hscmty_his_paciente.estado_paciente, "+
									"hscmty_his_paciente.telefono_particular1_paciente, "+
									"hscmty_erp_cobros_enca.responsable_cuenta, "+
									"hscmty_erp_cobros_enca.direccion_responsable_cuenta, "+
									"hscmty_erp_cobros_enca.telefono1_responsable_cuenta, "+
									"to_char(hscmty_erp_cobros_enca.fechahora_creacion, 'dd-MM-yyyy') AS fecha_ing, "+
									"to_char(hscmty_erp_cobros_enca.fechahora_creacion, 'HH:mm') AS hora_ing, "+
									"hscmty_erp_cobros_enca.id_medico, "+
									"hscmty_his_medicos.nombre_medico "+
									"FROM hscmty_erp_cobros_enca,hscmty_his_medicos,hscmty_his_paciente "+
									"WHERE hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
									"AND hscmty_erp_cobros_enca.id_medico = hscmty_his_medicos.id_medico "+
        							"AND hscmty_his_paciente.pid_paciente = '"+PidPaciente.ToString()+"' "+
        							"AND hscmty_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"'";
         
       		  	NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				//NUEVO
				// Crear una fuente 
				Gnome.Font fuente = Gnome.Font.FindClosest("Bitstream Vera Sans", 12);
				Gnome.Font fuente1 = Gnome.Font.FindClosest("Bitstream Vera Sans", 36);
				Gnome.Font fuente2 = Gnome.Font.FindClosest("Bitstream Vera Sans", 8);
				Gnome.Font fuente3 = Gnome.Font.FindClosest("Bitstream Vera Sans", 6);
						
				//Encabezado de pagina
				Gnome.Print.Setfont (ContextoImp, fuente2);
				ContextoImp.MoveTo(19.5, 750);
			    ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
			    ContextoImp.MoveTo(20, 750);
			    ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(19.5, 740); 
			    ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			    ContextoImp.MoveTo(20, 740);
			    ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
				ContextoImp.MoveTo(19.5, 730);
			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			    ContextoImp.MoveTo(20, 730);
			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			    Gnome.Print.Setfont (ContextoImp, fuente);
				ContextoImp.MoveTo(89.5, 708);
			    ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
				ContextoImp.MoveTo(90, 708);
			    ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
			    Gnome.Print.Setfont (ContextoImp, fuente1);
			    ContextoImp.MoveTo(20, 708);
   		   		ContextoImp.Show("____________________________");
		    int filatex=680;
			    //DATOS GENERALES DEL USUARIO
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(19.5, filatex);
				ContextoImp.Show("DATOS GENERALES DEL USUARIO:");
				ContextoImp.MoveTo(20, filatex);
				ContextoImp.Show("DATOS GENERALES DEL USUARIO:");
				Gnome.Print.Setfont (ContextoImp, fuente2);	
				ContextoImp.MoveTo(20, filatex-10);
				ContextoImp.Show("Nombre:_______________________________________________________________________________");
				ContextoImp.MoveTo(20, filatex-20);
				ContextoImp.Show("Domicilio:_______________________________________________________________________________________________________");
				ContextoImp.MoveTo(20, filatex-30);
				ContextoImp.Show("Telefono:_____________________________________________________________________________");
				//DATOS DEL DEMANDANTE
				Gnome.Print.Setfont (ContextoImp, fuente3);
				ContextoImp.MoveTo(19.5, filatex-50);
				ContextoImp.Show("DATOS GENERALES DEL DEMANDANTE DEL SERVICIO:");
				ContextoImp.MoveTo(20, filatex-50);
				ContextoImp.Show("DATOS GENERALES DEL DEMANDANTE DEL SERVICIO:");
				Gnome.Print.Setfont (ContextoImp, fuente2);
				ContextoImp.MoveTo(20, filatex-60);
				ContextoImp.Show("Nombre:________________________________________________________________________________");
				ContextoImp.MoveTo(20, filatex-70);
				ContextoImp.Show("Domicilio:__________________________________________________________________________________________________________________"); 
 				ContextoImp.MoveTo(20, filatex-80);
				ContextoImp.Show("Telefono:______________________________________________________________________________");
				ContextoImp.MoveTo(20, filatex-90);
				ContextoImp.Show("Nombre del medico tratante del usuario:_____________________________________________________________");
				ContextoImp.MoveTo(20, filatex-100);
				ContextoImp.Show("Fecha y hora de ingreso:____________________________________________________________________________");
				//PARRAFO 1
				Gnome.Print.Setfont (ContextoImp, fuente3);
				ContextoImp.MoveTo(20, filatex-120);
				ContextoImp.Show("CONTRATO  DE  PRESTACION  DE  SERVICIOS  HOSPITALARIOS,  que  celebran  por  una  parte el Hospital  Santa  Cecilia  de  Monterrey  SA. de CV.  En lo sucesivo  ''EL HOSPITAL''  y  por  otra");
				ContextoImp.MoveTo(20, filatex-128);
				ContextoImp.Show("parte ''EL USUARIO Y/O DEMANDENTE DEL SERVICIO'' cuyos datos aparecen en la caratula del presente documento, al tenor de las siguientes declaraciones y clausulas:");
				ContextoImp.MoveTo(269.5, filatex-140);
				ContextoImp.Show("D E C L A R A C I O N E S");
				ContextoImp.MoveTo(270, filatex-140);
				ContextoImp.Show("D E C L A R A C I O N E S");
				ContextoImp.MoveTo(19.5, filatex-145);
				ContextoImp.Show("I.DECLARA ''EL HOSPITAL'':");
				ContextoImp.MoveTo(20, filatex-145);
				ContextoImp.Show("I.DECLARA ''EL HOSPITAL'':");
				ContextoImp.MoveTo(20, filatex-150);
				ContextoImp.Show("I.1  Que  es  una  empresa  legalmente  constituida  conforme la Legislacion Mexicana,  autorizada  por la  secretaria  de Hacienda  y  Credito Publico, según consta  en  la   Escritura  Publica");
				ContextoImp.MoveTo(20, filatex-158);
				ContextoImp.Show("no.24423  del  05  de  noviembre  del  2002,  pasada ante fe  del  notario  Publico  no. 96 de  la  ciudad de Monterrey  NL.  Lic. Everardo Alanis Guerra,  Inscrita  en  el  registro  publico  de  la");
				ContextoImp.MoveTo(20, filatex-166);
				ContextoImp.Show("propiedad  y  del comercio  del Estado de Nuevo León,  folio numero 11421  de fecha 18 de noviembre del 2002,  la  cual  fue  modificada  en su razon  y  objeto  social,   mediante   escritura");
				ContextoImp.MoveTo(20, filatex-174);
				ContextoImp.Show("9508  ante  el  Notario publico 113  Lic. Gonzalo Trevino Sada  en fecha  de 08 de Julio del 2005,  registrada  en el registro publico  de la  propiedad  y  del comercio  mediante folio  mercantil");
				ContextoImp.MoveTo(20, filatex-182);
				ContextoImp.Show("electronico No. 82769*1 el 11 de julio del 2005.");
				//DECLARACIONES
				ContextoImp.MoveTo(20, filatex-198);
				ContextoImp.Show("I.2 Que su registro federal de contribuyentes es HSC-021105D5A");
				ContextoImp.MoveTo(20, filatex-206);
				ContextoImp.Show("I.3 Que su domicilio fiscal se encuentra ubicado en Galeana 200 Ote. Monterrey centro entre J. Treviño e Isaac garza, colonia Centro Monterrey N.L.; Codigo postal 64000.");
				ContextoImp.MoveTo(20, filatex-214);
			    ContextoImp.Show("I.4 Que dentro de sus principales actividades se encuentre el porporcionar servicios para la atención de enfermos que se internen para su diagnostico, tratamiento o rehabilitación");
				ContextoImp.MoveTo(19.5, filatex-230);
			    ContextoImp.Show("II. DECLARA ''EL USUARIO O DEMANDANTE DEL SERVICIO:");			
				ContextoImp.MoveTo(20, filatex-230);
			    ContextoImp.Show("II. DECLARA ''EL USUARIO O DEMANDANTE DEL SERVICIO");
				ContextoImp.MoveTo(20, filatex-238);
			    ContextoImp.Show("II.1 Que cuenta con la capacidad legal para celebrar el  presente contrato y  es su deseo  contratar los servicios  de ''EL HOSPITAL'' en los terminos  y condiciones que adelante se  establecen.");
			    ContextoImp.MoveTo(20, filatex-246);
			    ContextoImp.Show("II.2 Ser lo suficientemente  solvente(s)  economicamente  y  con la liquidez  necesaria para  pagar a  ''EL HOSPITAL'' todos  y   cada uno  de  los  servicios  hospitalarios materia del  presente");
			    ContextoImp.MoveTo(20, filatex-254);
			    ContextoImp.Show("contrato.");
			    //CLAUSULAS
			    ContextoImp.MoveTo(279.5, filatex-266);
			    ContextoImp.Show("C L A U S U L A S");
			    ContextoImp.MoveTo(280, filatex-266);
			    ContextoImp.Show("C L A U S U L A S");
				ContextoImp.MoveTo(19.5, filatex-276);
			    ContextoImp.Show("PRIMERA:  OBJETO.-  ''EL HOSPITAL''");
				ContextoImp.MoveTo(20, filatex-276);
			    ContextoImp.Show("PRIMERA:  OBJETO.-  ''EL HOSPITAL''  prestara  los  servicios   de   atención   medica,   hospitalaria,   quirurgica,   de   urgencias   y   servicios   clinicos   internos,   extetrnos   y   auxiliares   de");
			    ContextoImp.MoveTo(20, filatex-284);
			    ContextoImp.Show("diagnostico  y  tratamiento  en sus instalaciones  de  acuerdo  con  sus normas  y  reglamentos  y  con el equipo,  personal, materiales y medicamentos de  que  sea  posible disponer, y que");
			    ContextoImp.MoveTo(20, filatex-292);
			    ContextoImp.Show("sean  indicados  por  su  medico  tratante  y/o  aquellos  que  sean  necesarios   en  caso  de  urgencia,  de  acuerdo a la  capacidad resolutiva  e  instalada de  ''EL HOSPITAL'' conforme a los");
			    ContextoImp.MoveTo(20, filatex-300);
			    ContextoImp.Show("procedimientos y normatividad legal aplicable en el ramo de salud.");
			    ContextoImp.MoveTo(19.5, filatex-316);
			    ContextoImp.Show("SEGUNDA:  LOS  SERVICIOS.- ");
				ContextoImp.MoveTo(20, filatex-316);
			    ContextoImp.Show("SEGUNDA:  LOS  SERVICIOS.-  Podran  consistir enunciativa  y  no  limitativamente  en  uso  de  equipos  medicos  e  infraestructura   hospitalaria,  exámenes  de  diagnostico  y  tratamiento,");
			    ContextoImp.MoveTo(20, filatex-324);
			    ContextoImp.Show("servicios   de   enfermeria  y  personal  paramedico,  suministro   de   medicamentos  e  insumos   para  las  curaciones,  tratamientos  medicos  y  terapeuticos,   interveciones   quirurgicas  y");
			 	ContextoImp.MoveTo(20, filatex-332);
			    ContextoImp.Show("procedimientos  medicos  que  se  requieran  con  motivo  del  padecimiento  del  usuario  y  de  las  posibles  eventualidades  que   se  pudieran  presentarse  durante  los  tratamientos   y/o");
			 	ContextoImp.MoveTo(20, filatex-340);
			    ContextoImp.Show("intervenciones    quirurgicas.   Por   su   parte    ''EL  USUARIO   Y/O   DEMANDANTE   DEL   SERVICIO''   se   obliga(n)   a    pagar   a   ''EL HOSPITAL''  el   importe  de  los   servicios  que   sean");
			 	ContextoImp.MoveTo(20, filatex-348);
			    ContextoImp.Show("proporcionados por motivos del presente contrato.");
			 	ContextoImp.MoveTo(19.5, filatex-364);
			    ContextoImp.Show("TERCERA: PAQUETE  DE  SERVICIOS.- ");
			    ContextoImp.MoveTo(20, filatex-364);
			    ContextoImp.Show("TERCERA: PAQUETE  DE  SERVICIOS.-  Cuando  se  contrate  un paquete  de  servicios,  se  especificará  en  forma  anexa  sus precios  y sus servicios que  lo integran y  dicho  anexo  una vez");
			    ContextoImp.MoveTo(20, filatex-372);
			    ContextoImp.Show("firmado por  ''EL USUARIO  Y/O  DEMANDANTE  DEL  SERVICIO''  formara  parte  integrante  de este  contrato.  Todos  los servicios   que   no  se  encuentren  incluidos   en  la  descripcion  del");
			    ContextoImp.MoveTo(20, filatex-380);
			    ContextoImp.Show("paquete  repectivo,  o  que  excedan  de  los  limites de dicho paquete deberán de  ser pagados conforme  a los precios  de lista autorizados  de ''EL HOSPITAL'' y que se encuentren vigentes.");
			    ContextoImp.MoveTo(19.5, filatex-396);
			    ContextoImp.Show("CUARTA: LUGAR  DE  LA  PRESTACION  DE LOS  SERVICIOS.-");
			    ContextoImp.MoveTo(20, filatex-396);
			    ContextoImp.Show("CUARTA: LUGAR  DE  LA  PRESTACION  DE LOS  SERVICIOS.-  Los servicios  serán  proporcionados  en  las instalaciones  de ''EL HOSPITAL'',  las cuales  se ubican  en  Isaac  Garza  200  oriente,");
			    ContextoImp.MoveTo(20, filatex-404);
			    ContextoImp.Show("Colonia Centro, entre Galeana y Guerrero en Monterrey N.L. Mexico. C.P. 64000");
			    ContextoImp.MoveTo(19.5, filatex-420);
			    ContextoImp.Show("QUINTA:  INSUMOS   Y   MEDICAMENTOS.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO''");
			    ContextoImp.MoveTo(20, filatex-420);
			    ContextoImp.Show("QUINTA:  INSUMOS   Y   MEDICAMENTOS.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO''    reconoce(n)     y     manifiesta(n)   su    conformidad    en    que   ''EL   HOSPITAL''    proveera    los");
			    ContextoImp.MoveTo(20, filatex-428);
			    ContextoImp.Show("insumos  y  medicamentos  de  uso  común  que  se  requieran  para  la  atención  medica, durante la estancia  hospitalaria,  conforme  a  la  dosis,  cantidades  y  terminos que ordene(n) por");
			    ContextoImp.MoveTo(20, filatex-436);
			    ContextoImp.Show("escrito el (los) medicos tratantes(s)");
			    ContextoImp.MoveTo(19.5, filatex-452);
			    ContextoImp.Show("SEXTA:   REQUISITOS   PARA   LA  ADMISION   HOSPITALARIA.-  ''EL USUARIO  Y/O  DEMANDANTE  DEL SERVICIO''");
			    ContextoImp.MoveTo(20, filatex-452);
			    ContextoImp.Show("SEXTA:   REQUISITOS   PARA   LA  ADMISION   HOSPITALARIA.-  ''EL USUARIO  Y/O  DEMANDANTE  DEL SERVICIO'' debera(n)  registrarse  en el  departamento  de  Admision  de  ''EL HOSPITAL'',");
			    ContextoImp.MoveTo(20, filatex-460);
			    ContextoImp.Show("proporcionando    todos    los    datos    generales   e   información   necesaria   para   su   registro,   asi   mismo   deberán  otorgar   el   anticipo   y/o   deposito   en   garantia de  pago   que");
			    ContextoImp.MoveTo(20, filatex-468);
			    ContextoImp.Show("establezca el catálogo de precios vigente de ''EL HOSPITAL'', de acuerdo con el motivo de ingreso  y/o  servicio hospitalario al que ingresara ''EL  USUARIO'', recabando el  comprobante  que");
			    ContextoImp.MoveTo(20, filatex-476);
			    ContextoImp.Show("al efecto expida ''EL HOSPITAL''.");
			    ContextoImp.MoveTo(20, filatex-492);
			    ContextoImp.Show("En  caso  de  que  ''EL USUARIO'' cuente  con  una  poliza  de  seguro de accidente  y/o  enfermedad en los ramos de accidentes personales,  gastos  medicos y/o salud, con una institución de ");
			    ContextoImp.MoveTo(20, filatex-500);
			    ContextoImp.Show("seguros legalmente constituida o sea derechohabiente de algúna institución publica a privada la cual tenga convenio celebrado con ''EL HOSPITAL'' para pago directo de los servicios materia");
			    ContextoImp.MoveTo(20, filatex-508);
			    ContextoImp.Show("del  presente  contrato, ''EL USUARIO Y/O DEMANDANTE  DEL  SERVICIO''  tendrá(n)   la   obligacion   de   comúnicarlo   al   personal   de   Admision  de  ''EL HOSPITAL''   al   momento   de  su");
			    ContextoImp.MoveTo(20, filatex-516);
			    ContextoImp.Show("admision   hospitalaria,   a   fin  de  que  se  lleven  a  cabo   los   procedimientos   convenidos   entre   la   aseguradora  o  institución  correspondiente  y  ''EL  HOSPITAL''.  En  caso de que la");
			    ContextoImp.MoveTo(20, filatex-524);
			    ContextoImp.Show("aseguradora   o   institución   no  asuma  la  obligacion  de pago de  los  servicios  ''EL USUARIO  Y/O  DEMANDANTE  DEL  SERVICIO'' tendrá(n)  la obligacion de  pagar  a  ''EL  HOSPITAL'',  en");
			    ContextoImp.MoveTo(20, filatex-532);
			    ContextoImp.Show("los terminos señalados en la clausula decima primera, todos los servicios prestados con motivo del presente contrato.");
			    ContextoImp.MoveTo(20, filatex-548);
			    ContextoImp.Show("Asi mismo ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' se obligan al pago de todos los cargos de los servicios no incluidos, no cubiertos, por la aseguradora o institución correspondiente,");
			    ContextoImp.MoveTo(20, filatex-556);
			    ContextoImp.Show("asi como el pago de coaseguro, deducibles o cualquier otra cantidad establecida por su aseguradora o institución.");
			    ContextoImp.MoveTo(19.5, filatex-572);
			    ContextoImp.Show("SEPTIMA: INGRESO DE OBJETOS PERSONALES.- ''EL HOSPITAL''");
			    ContextoImp.MoveTo(20, filatex-572);
		    	ContextoImp.Show("SEPTIMA: INGRESO DE OBJETOS PERSONALES.- ''EL HOSPITAL'' no se hace responsable por la perdida, daño o robo de los objetos o valores de ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO''");
			    ContextoImp.MoveTo(20, filatex-580);
			    ContextoImp.Show("que se introduzcan a ''EL HOSPITAL''. En los casos de pacientes programados, no presentarse con objetos de valor a ''EL HOSPITAL''.");
			    //Pie de pagina
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(490, filatex-620);
			    ContextoImp.Show("FO-DMH-08/REV.02/04-OCT-06");
			    Gnome.Print.Setfont (ContextoImp, fuente1);
			    ContextoImp.MoveTo(20, filatex-640);
   	   			ContextoImp.Show("____________________________");
			    
			    
			    if ((bool) lector.Read())
   		   		{
      				Gnome.Print.Setfont (ContextoImp, fuente2);
      				ContextoImp.MoveTo(20, filatex-10);
					ContextoImp.Show("Nombre: "+(string) lector["nombre1_paciente"]+" "+(string) lector["nombre2_paciente"]+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"]);
					ContextoImp.MoveTo(50, filatex-10);
					ContextoImp.MoveTo(20, filatex-20);
					ContextoImp.Show("Domicilio: "+(string) lector["direccion_paciente"]+" "+(string) lector["numero_casa_paciente"]+" "+(string) lector["numero_departamento_paciente"]+" "+(string) lector["colonia_paciente"]+" "+(string) lector["codigo_postal_paciente"]+" "+(string) lector["municipio_paciente"]+" "+(string) lector["estado_paciente"]);
					ContextoImp.MoveTo(20, filatex-30);
					ContextoImp.Show("Telefono: "+(string) lector["telefono_particular1_paciente"]+" ");
					ContextoImp.MoveTo(20, filatex-60);
					ContextoImp.Show("Nombre: "+(string) lector["responsable_cuenta"]+" ");
					ContextoImp.MoveTo(20, filatex-70);
					ContextoImp.Show("Domicilio: "+(string) lector["direccion_responsable_cuenta"]+" "); 
		 			ContextoImp.MoveTo(20, filatex-80);
					ContextoImp.Show("Telefono: "+(string) lector["telefono1_responsable_cuenta"]+" ");
					ContextoImp.MoveTo(20, filatex-90);
					ContextoImp.Show("Nombre del medico tratante del usuario: "+medico.ToString()); //(string) lector["nombre_medico"]+" ");
					ContextoImp.MoveTo(20, filatex-100);
					ContextoImp.Show("Fecha y hora de ingreso: "+(string) lector["fecha_ing"]+"      "+(string) lector["hora_ing"]);
				}
		    	lector.Close ();
				ContextoImp.ShowPage();
		    	//PAGINA 2
		    	ContextoImp.BeginPage("Pagina 2");
		    	//Encabezado de pagina
				Gnome.Print.Setfont (ContextoImp, fuente2);
				ContextoImp.MoveTo(19.5, 750);
		    	ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
		    	ContextoImp.MoveTo(20, 750);
		    	ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(19.5, 740); 
			    ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
		    	ContextoImp.MoveTo(20, 740);
		    	ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
				ContextoImp.MoveTo(19.5, 730);
			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			    ContextoImp.MoveTo(20, 730);
		    	ContextoImp.Show("Conmutador:(81) 81-25-56-10");
				Gnome.Print.Setfont (ContextoImp, fuente);
				ContextoImp.MoveTo(89.5, 708);
		    	ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
				ContextoImp.MoveTo(90, 708);
		    	ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
				Gnome.Print.Setfont (ContextoImp, fuente1);
		    	ContextoImp.MoveTo(20, 708);
      			ContextoImp.Show("____________________________");
			    //CONTINUACION CLAUSULAS
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(19.5, filatex);
			    ContextoImp.Show("OCTAVA: CONSENTIMIENTO INFORMADO.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO''");
			    ContextoImp.MoveTo(20, filatex);
			    ContextoImp.Show("OCTAVA: CONSENTIMIENTO INFORMADO.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' manifiesta(n) que ha(n)  sido  inforado(s)  de  los  riesgos  que  implican cualquier  atención  medica,");
			    ContextoImp.MoveTo(20, filatex-8);
			    ContextoImp.Show("asi como de sus beneficios, de los cuales esta(n) conciente(s), por lo que reconoce(n) que pueden presentarse eventualidades que varien o modifiquen el diagnostico y/o tratamiento medico ");
		    	ContextoImp.MoveTo(20, filatex-16);	
		   		ContextoImp.Show("inicial. Sin embargo autorizan expresamente a ''EL HOSPITAL'' y a su(s) medico(s) tratante(s) para que presenen y practiquen al usuario los procedimientos medicos, quirurgicos, hospitalarios,");
		    	ContextoImp.MoveTo(20, filatex-24);
			    ContextoImp.Show("exámenes  y  curaciones  que  sean necesarios  de  acuerdo  al  padecimiento  y  a  las  posibles eventualidades  que  pudieran presentarse durante los mismos. De igual forma, otorga(n) su");
			    ContextoImp.MoveTo(20, filatex-32);
			    ContextoImp.Show("consentimiento  para  que  el/los  medico(s)  tratante(s)  de  ''EL USUARIO''  ordene(n)  la  practica  de  exámenes,  curaciones,  tratamientos e intervenciones de otros medicos,  asi  como  la");
		    	ContextoImp.MoveTo(20, filatex-40);
		    	ContextoImp.Show("administracion de anestesicos, sangre y/o medicamentos que consideren oportunos.");
		    	ContextoImp.MoveTo(19.5, filatex-56);
		    	ContextoImp.Show("NOVENA: REGLAMENTO PARA VISITANTES.-");
		    	ContextoImp.MoveTo(20, filatex-56);
		    	ContextoImp.Show("NOVENA: REGLAMENTO PARA VISITANTES.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' se compromete(n) a cumplir el reglamento patra visitantes de ''EL HOSPITAL'' el cual manifiesta(n)");
		    	ContextoImp.MoveTo(20, filatex-62);
			    ContextoImp.Show("conocer por haberlo leido antes de la firma del presente contrato. Un ejemplar del dicho reglamento queda a dispocision de ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' para su consulta ");
			    ContextoImp.MoveTo(20, filatex-70);
			    ContextoImp.Show("en el departamento de admision de ''EL HOSPITAL''.");
			    ContextoImp.MoveTo(19.5, filatex-86);
			    ContextoImp.Show("DECIMA: PRECIO.-");
			    ContextoImp.MoveTo(20, filatex-86);
			    ContextoImp.Show("DECIMA: PRECIO.-Las partes contratantes convienen en que el precio de los servicios que sean proporcionados con motivo del presente contrato, sera  el que resulte  de aplicar los precios y ");
			    ContextoImp.MoveTo(20, filatex-94);
			    ContextoImp.Show("tarifas que ''EL HOSPITAL'' tenga vigentes en la fecha de contratacion de los servicios, por lo que ''EL HOSPITAL'' deja a dispocicion  de  ''EL  USUARIO  Y/O  DEMANDANTE  DEL  SERVICIO''  el ");
			    ContextoImp.MoveTo(20, filatex-102);
			    ContextoImp.Show("catálogo o lista de precios y tarifas vigentes.");
			    ContextoImp.MoveTo(20, filatex-110);
			    ContextoImp.Show("FO-DMH-08/REV.02/04-OCT-06");
			    ContextoImp.MoveTo(19.5, filatex-126);
			    ContextoImp.Show("DECIMA PRIMERA: PROCEDIMIENTO DE PAGO.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO''");
			    ContextoImp.MoveTo(20, filatex-126);
			    ContextoImp.Show("DECIMA PRIMERA: PROCEDIMIENTO DE PAGO.- ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' se obliga(n)  a  pagar diariamente a  ''EL HOSPITAL''  los servicios proporcionados, conforme  al ");
			    ContextoImp.MoveTo(20, filatex-134);
			    ContextoImp.Show("estado de cuenta correspondiente, el cual ''EL HOSPITAL'' pondra a su dispocision en forma diaria, de tal manera que  al  final  de  cada dia hospitalario  se  encuentren cubiertos los servicios ");
			    ContextoImp.MoveTo(20, filatex-142);
			    ContextoImp.Show("otorgados al usuario.");
			    ContextoImp.MoveTo(20, filatex-150);
			    ContextoImp.Show("Tratandose  de  paquetes  de  servicios  el  precio  y  forma  de pago  de  los  mismos serán  establecidos de forma anexa al presente contrato, en el entendido que todos los servicios que no");
			    ContextoImp.MoveTo(20, filatex-158);
			    ContextoImp.Show("se  encuentren  incluidos  en  la  descripcion  del  paquete  respectivo,  o  que  excedan  los  limites  de  dichos  paquetes  deberán   ser   pagados  en  los   terminos   del   presente  contrato.");
			    ContextoImp.MoveTo(19.5, filatex-174);
			    ContextoImp.Show("DECIMA SEGUNDA: OBLIGACION DEL PAGO.- ");
			    ContextoImp.MoveTo(20, filatex-174);
			    ContextoImp.Show("DECIMA SEGUNDA: OBLIGACION DEL PAGO.- Las partes contratantes convienen en que la responsabilidad  por  el  pago  de  los  servicios, conforme al catálogo  de precios  y  tarifas vigente");
			    ContextoImp.MoveTo(20, filatex-182);
			    ContextoImp.Show("de ''EL HOSPITAL'', recaera solidariamente sobre ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'', de conformidad  con los  dispuestos en los  articulos 181,  1883, 1892  y  1896  del  Codigo ");
			    ContextoImp.MoveTo(20, filatex-190);
			    ContextoImp.Show("Civil vigente en el Estado de Nuevo León.");
			    ContextoImp.MoveTo(20, filatex-198);
			    ContextoImp.Show("Las partes contratantes convienen en que si el monto llegara a rebasar la capacidad economica y/o liquidez de ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'', este(os)  debera(n)  informar");
			    ContextoImp.MoveTo(20, filatex-206);
			    ContextoImp.Show("a ''EL HOSPITAL'' y trasladar de inmediato al usuario a otra institución hospitalaria. Con el fin de evitar incurrir en mora o falta de pago de los servicios.");
			    ContextoImp.MoveTo(20, filatex-222);
			    ContextoImp.Show("En caso que no se cumpla lo establecido  en  el  párrafo  anterior,  ''EL HOSPITAL''  podrá  realizar  el  traslado  del  usuarios  a  una institución hospitalaria pública, quedando  ''EL HOSPITAL''");
			    ContextoImp.MoveTo(20, filatex-230);
			    ContextoImp.Show("liberado   del   incumplimiento  de  las  obligaciones  que  corran  a  su  cargo  con  motivo  del  presente  contrato  y  contando  desde  ahora  con  el  consentimiento  de  ''EL  USUARIO   Y/O");
			    ContextoImp.MoveTo(20, filatex-238);
			    ContextoImp.Show("DEMANDANTE DEL  SERVICIO''  para  ese  efecto,  en  el  entendido  que  el  tras  lado  del  usuario no extinguirá la obligación  de pago  de los servicios  proporcionados  y  no pagados  antes");
			    ContextoImp.MoveTo(20, filatex-246);
			    ContextoImp.Show("de realizarse  el  traslado.  Antes  de  realizar  el  traslado,  ''EL HOSPITAL'' se  obliga  a dar  aviso  fehacientemente  al  usuario  y/o  demandante  del  servicio  de   que   se   efectuará  dicho");
			    ContextoImp.MoveTo(20, filatex-254);
			    ContextoImp.Show("traslado, especificando los datos de la Unidad Médica receptora.");
			    ContextoImp.MoveTo(20, filatex-270);
			    ContextoImp.Show("Las  partes  contratantes  convienen  en que  sobre  las  cantidades vencidas  y  no pagadas,  ''EL HOSPITAL''  podrá cobrar intereses moratorios aplicando el TIEE  por  tres  anual,  desde  el");
			    ContextoImp.MoveTo(20, filatex-278);
			    ContextoImp.Show("día siguiente en que debió haberse realizado el pago y hasta el momento en que se liquide el adeudo.");
			    ContextoImp.MoveTo(20, filatex-294);
			    ContextoImp.Show("Para efectos del presente contrato, TIEE  significa  Tasa  de  Interés  Interbancaria  de  Equilibrio  a   28   días,  publicada  por  el  Banco  de  México  al  momento   de  ser  aplicable  la  pena");
			    ContextoImp.MoveTo(20, filatex-302);
			    ContextoImp.Show("convencional que establece este contrato.");
			    ContextoImp.MoveTo(19.5, filatex-318);
			    ContextoImp.Show("DECIMA TERCERA: PROCEDIMIENTO PARA EL ALTA DEL USUARIO.- ");
			    ContextoImp.MoveTo(20, filatex-318);
			    ContextoImp.Show("DECIMA TERCERA: PROCEDIMIENTO PARA EL ALTA DEL USUARIO.- El egreso (alta) del usuario de ''EL HOSPITAL'' se verificara cuando el médico tratante  del  usuario haga constar  por  escrito");
			    ContextoImp.MoveTo(20, filatex-326);
			    ContextoImp.Show("en el expediente clínico del usuario la orden de alta respectiva o cuando  se  solicite  el  egreso voluntario  del  usuario  y  se  firme  el  formato  que  para  tal  efecto elabore  un  médico  de");
			    ContextoImp.MoveTo(20, filatex-334);
			    ContextoImp.Show("''EL HOSPITAL''. Previo  a  su  retiro, ''EL USUARIO Y/O DEMANDANTE DEL SERVICIO'' deberá(n) liquidar  en  la  caja  de  pago  de  ''EL  HOSPITAL'',  el   total   de   la  cuenta  de  los  servicios ");
			    ContextoImp.MoveTo(20, filatex-342);
			    ContextoImp.Show("proporcionados, debiendo recabar en dicha caja la papelería que contenga el pase de salida correspondiente,  mismo  que  deberá  ser  entregado  por  ''EL USUARIO  Y/O DEMANDANTE DEL");
			    ContextoImp.MoveTo(20, filatex-350);
			    ContextoImp.Show("SERVICIO'' a la Jefa de Enfermeras de ''EL HOSPITAL'' al momento de su egreso.");
			    ContextoImp.MoveTo(20, filatex-366);
			    ContextoImp.Show("El anticipo y/o depósito en garantía será aplicado a la cuenta al momento  de  liquidar el  importe total  de los  servicios proporcionados, es decir, el  día  en  que  el usuario sea dado de alta");
			    ContextoImp.MoveTo(20, filatex-374);
			    ContextoImp.Show("y previamente a que abandone las instalaciones de ''EL HOSPITAL''; en el entendido que cualquier excedente será devuelto de inmediato por ''EL HOSPITAL''");
			    ContextoImp.MoveTo(19.5, filatex-390);
			    ContextoImp.Show("DECIMA CUARTA: LÍMITES PARA LA CONTRATACIÓN DE SERVICIOS CON TERCEROS.- ");
			    ContextoImp.MoveTo(20, filatex-390);
			    ContextoImp.Show("DECIMA CUARTA: LÍMITES PARA LA CONTRATACIÓN DE SERVICIOS CON TERCEROS.- Las partes contratantes convienen expresamente que  ''EL HOSPITAL'' no asume  ninguna responsabilidad");
			    ContextoImp.MoveTo(20, filatex-398);
			    ContextoImp.Show("sobre  la  actuación  profesional  de  los  médicos  particulares (médicos  tratantes)   del   usuario  y/o   de  terceras  personas  que  proporcionen  servicio  de  atención  medica   al   usuario,");
			    ContextoImp.MoveTo(20, filatex-406);
			    ContextoImp.Show("cuando tales profesionistas y/o servicios sean contratados directamente por 'EL USUARIO Y/O DEMANDANTE DEL SERVICIO'', por lo que reconocen que el pago de estos servicios");
			    ContextoImp.MoveTo(20, filatex-414);
			    ContextoImp.Show("será liquidado de manera independiente a la cuenta de los servicios materia del presente contrato.");
			    ContextoImp.MoveTo(19.5, filatex-430);
			    ContextoImp.Show("DECIMA QUINTA: CONFIDENCIALIDAD.- ");
				ContextoImp.MoveTo(20, filatex-430);
			    ContextoImp.Show("DECIMA QUINTA: CONFIDENCIALIDAD.-  Salvo  en  los  casos  que  ''EL  HOSPITAL''  debe  proporcionar  información  del  usuario  a  la  autoridad  competente,  ''EL HOSPITAL''  se  obliga  a");
			    ContextoImp.MoveTo(20, filatex-438);
			    ContextoImp.Show("dar trato confidencial a la  información  contenida  en  el  expediente  del  usuario,  comprometiéndose  a  no  revelar  a  terceros  sin  autorización  escrita  de  éste. Desde este momento ''EL");
			    ContextoImp.MoveTo(20, filatex-446);
			    ContextoImp.Show("USUARIO  Y/O  DEMANDANTE  DEL  SERVICIO''  autoriza(n)  a  ''EL HOSPITAL''  para  que  proporcione  toda  la  información,  y  en  su  caso  documentación  referente a la  hospitalización  del");
			    ContextoImp.MoveTo(20, filatex-454);
			    ContextoImp.Show("usuario, la compañía de seguros con quien tenga(n) contratada una póliza de seguro de accidentes personales, gastos médicos y/o salud, o la institución de la que sea(n) derechohabiente(s).");
			    ContextoImp.MoveTo(20, filatex-462);
			    ContextoImp.Show("");
			    ContextoImp.MoveTo(19.5, filatex-478);
			    ContextoImp.Show("DECIMA SEXTA: PROCEDIMIENTO PARA PRESENTAR SUGERENCIA, RECLAMACIONES O QUEJAS.-”EL USUARIO Y/O DEMANDANTE DEL SERVICIO”");
			    ContextoImp.MoveTo(20, filatex-478);
			    ContextoImp.Show("DECIMA SEXTA: PROCEDIMIENTO PARA PRESENTAR SUGERENCIA, RECLAMACIONES O QUEJAS.-”EL USUARIO Y/O DEMANDANTE DEL SERVICIO”  podrá(n) presentar sugerencia,  reclamaciones");
			    ContextoImp.MoveTo(20, filatex-486);
			    ContextoImp.Show("o quejas relativas a los servicios materia del  presente contrato en el  departamento jurídico de  “EL HOSPITAL”, mismas que deberán ser atendidas en  un  tiempo  máximo  de 15-quince  días");
			    ContextoImp.MoveTo(20, filatex-494);
			    ContextoImp.Show("hábiles.");
			    ContextoImp.MoveTo(20, filatex-510);
			    ContextoImp.Show("En caso que algúna de las partes contratantes incurra en una o varias de las causales de incumplimiento adelante señaladas, deberán pagar a la otra parte, como pena convencional, el 10% ");
			    ContextoImp.MoveTo(20, filatex-518);
			    ContextoImp.Show("del monto total de los servicios proporcionados  y  facturados  por  ''EL  HOSPITAL'' con motivo  del  presente  contrato.  Sin  que ello libere al  usuario  y/o  demandante  del  servicio  de  sus");
			    ContextoImp.MoveTo(20, filatex-526);
			    ContextoImp.Show("obligaciones de pago de dichos servicios.");
			    ContextoImp.MoveTo(19.5, filatex-542);
			    ContextoImp.Show("DECIMA SEPTIMA: CAUSAS DE INCLUMPLIMIENTO DE “EL HOSPITAL”.-");
			    ContextoImp.MoveTo(20, filatex-542);
			    ContextoImp.Show("DECIMA SEPTIMA: CAUSAS DE INCLUMPLIMIENTO DE “EL HOSPITAL”.-");
			    ContextoImp.MoveTo(50, filatex-550);
			    ContextoImp.Show("a)No proporcionar los servicios hospitalarios contratados conforme a su capacidad resolutiva e instalada.");
			    ContextoImp.MoveTo(50, filatex-558);
			    ContextoImp.Show("b)No respetar el precio de los servicios en los términos contratados.");
			    ContextoImp.MoveTo(50, filatex-566);
			    ContextoImp.Show("c)No proporcionar los insumos y medicamentos de uso común que soliciten los médicos tratantes e ínter consultantes durante la estancia hospitalaria del usuario.");
			    //PIE DE PAGINA
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(490, filatex-620);
			    ContextoImp.Show("FO-DMH-08/REV.02/04-OCT-06");
			    Gnome.Print.Setfont (ContextoImp, fuente1);
			    ContextoImp.MoveTo(20, filatex-640);
   	 	  		ContextoImp.Show("____________________________");
			 	ContextoImp.ShowPage();
			    //PAGINA TRES
			    ContextoImp.BeginPage("Pagina 3");
			    //Encabezado de pagina
				Gnome.Print.Setfont (ContextoImp, fuente2);
				ContextoImp.MoveTo(19.5, 750);
			    ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
			    ContextoImp.MoveTo(20, 750);
			    ContextoImp.Show("Hospital Santa Cecilia de Monterrey");
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(19.5, 740); 
			    ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
			    ContextoImp.MoveTo(20, 740);
			    ContextoImp.Show("Dirección: Isaac Garza #200 Ote. Centro Monterrey, NL.");
				ContextoImp.MoveTo(19.5, 730);
			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			    ContextoImp.MoveTo(20, 730);	
			    ContextoImp.Show("Conmutador:(81) 81-25-56-10");
			    Gnome.Print.Setfont (ContextoImp, fuente);
				ContextoImp.MoveTo(89.5, 708);
			    ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
				ContextoImp.MoveTo(90, 708);
			    ContextoImp.Show("CONTRATO DE PRESTACIONES DE SERVICIOS MEDICOS HOSPITALARIOS");
			    Gnome.Print.Setfont (ContextoImp, fuente1);
			    ContextoImp.MoveTo(20, 708);
	   	   		ContextoImp.Show("____________________________");
			    //CONTINUACION CLAUSULAS
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(20, filatex);
			    ContextoImp.Show("Leído que fue el presente contrato y enteradas  las  partes contratantes del contenido y alcance legal, lo firman de conformidad  en  la  Ciudad  de  Monterrey  N.L.,  en  la  fecha  de  ingreso");
			    ContextoImp.MoveTo(20, filatex-8);
		    	ContextoImp.Show("hospitalario  que  aparece  en  la  carátula  de  éste   documento,  manifestando desde este momento que para el caso  de  controversia sobre  su  interpretación, cumplimiento ejecución  se");
		    	ContextoImp.MoveTo(20, filatex-16);
		    	ContextoImp.Show("someterán  a las  leyes aplicables  y  a  la  competencia  de  los  tribunales de la Ciudad de Monterrey N.L. renunciando expresamente a cualquier otro fuero que pudiera corresponderles por");
			    ContextoImp.MoveTo(20, filatex-24);
			    ContextoImp.Show("razón  de  su  domicilio presente o  futuro  o  cualquier  otra  causa,  en  el  entendido  que  cualquier  reclamación  de  ''EL  USUARIO   Y/O   DEMANDANTE   DEL   SERVICIO''  deberá   agotar");
			    ContextoImp.MoveTo(20, filatex-32);
			    ContextoImp.Show("previamente  el  procedimiento conciliatorio de la  Procuraduría Federal del Consumidor.  En la  Ciudad de Monterrey,  Nuevo León siendo  el día "+DateTime.Now.ToString("dd")+ "  del  "+ DateTime.Now.ToString("MM")+"  del año  "+DateTime.Now.ToString("yyyy"));
			    ContextoImp.MoveTo(30, filatex-190);
			    ContextoImp.Show("_______________________________________________________");
			    ContextoImp.MoveTo(45, filatex-190);
			    ContextoImp.Show((string) lector["nombre1_paciente"]+" "+(string) lector["nombre2_paciente"]+" "+(string) lector["apellido_paterno_paciente"]+" "+(string) lector["apellido_materno_paciente"]);
			    ContextoImp.MoveTo(420, filatex-190);
			    ContextoImp.Show("______________________________________");
			    ContextoImp.MoveTo(89.5, filatex-200);
			    ContextoImp.Show("EL USUARIO");
			    ContextoImp.MoveTo(90, filatex-200);
			    ContextoImp.Show("EL USUARIO");
			    ContextoImp.MoveTo(459.5, filatex-200);
			    ContextoImp.Show("EL HOSPITAL");
			    ContextoImp.MoveTo(460, filatex-200);
			    ContextoImp.Show("EL HOSPITAL");
			    ContextoImp.MoveTo(220, filatex-400);
			    ContextoImp.Show("________________________________________________");
			    ContextoImp.MoveTo(249.5, filatex-410);
			    ContextoImp.Show("EL DEMANDANTE DEL SERVICIO");
			    ContextoImp.MoveTo(250, filatex-410);
			    ContextoImp.Show("EL DEMANDANTE DEL SERVICIO");
				    //Pie de pagina
			    Gnome.Print.Setfont (ContextoImp, fuente3);
			    ContextoImp.MoveTo(490, filatex-620);
			    ContextoImp.Show("FO-DMH-08/REV.02/04-OCT-06");
			    Gnome.Print.Setfont (ContextoImp, fuente1);
			    ContextoImp.MoveTo(20, filatex-640);
      			ContextoImp.Show("____________________________");
	      			/*int filas3=700;
      			for (int i1=0; i1 < 35; i1++)
				{
				filas3-=20;
				ContextoImp.MoveTo(588, filas3);
      			ContextoImp.Show("|");
				}
    			lector.Close ();*/
				conexion.Close ();
				//TERMINAN de funcionar la conexion con postgresql 
				ContextoImp.ShowPage();
				}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
		}
	}
}