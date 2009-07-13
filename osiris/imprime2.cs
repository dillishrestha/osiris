using System;
using Gtk;
using Gnome;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;
using GtkSharp;

public class rptAdmision_fecha 
{
	public string connectionString = "Server=172.16.1.148;" +
            	                         "Port=5432;" +
                	                     "User ID=admin1;" +
                    	                 "Password=1qaz2wsx;"+
                        	             "Database=hscmty;";            
	public rptAdmision_fecha ()
	{
		PrintJob    trabajo   = new PrintJob (PrintConfig.Default ());
        PrintDialog dialogo   = new PrintDialog (trabajo, "Prueba", 0);
        int         respuesta = dialogo.Run ();
        
        Console.WriteLine ("Respuesta: " + respuesta);
              
		if (respuesta == (int) PrintButtons.Cancel) 
		{
			Console.WriteLine("Impresión cancelada");
			dialogo.Hide (); 
			dialogo.Dispose (); 
			return;
		}

        PrintContext ctx = trabajo.Context;
        ComponerPagina(ctx, trabajo); 

        trabajo.Close();
             
        switch (respuesta)
        {
                  case (int) PrintButtons.Print:   
                  		trabajo.Print (); 
                  		break;
                  case (int) PrintButtons.Preview:
                  Console.WriteLine ("vista previa");
                      	new PrintJobPreview(trabajo, "Prueba").Show();
                        break;
        }

		dialogo.Hide (); dialogo.Dispose ();
        
	}
      
	void ComponerPagina (PrintContext ContextoImp, PrintJob trabajoImpresion)
	{
		NpgsqlConnection conexion; 
        conexion = new NpgsqlConnection (connectionString);
            
        // Verifica que la base de datos este conectada
        try
        {
        	conexion.Open ();
        	NpgsqlCommand comando; 
        	comando = conexion.CreateCommand (); 
             
			comando.CommandText ="SELECT hscmty_erp_movcargos.id_tipo_admisiones,hscmty_his_tipo_admisiones.descripcion_admisiones,folio_de_servicio,folio_de_servicio_dep, to_char(fechahora_admision_registro,'dd-MM-yyyy') AS fech_reg_adm,hscmty_erp_movcargos.id_tipo_paciente, hscmty_erp_movcargos.pid_paciente,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,grupo_sanguineo_paciente,direccion_paciente,numero_casa_paciente,codigo_postal_paciente,estado_civil_paciente, "+
                                 "colonia_paciente,numero_departamento_paciente,ocupacion_paciente,sexo_paciente,to_char(fecha_nacimiento_paciente,'dd-MM-yyyy') AS fech_nacimiento, id_empresa, descripcion_tipo_paciente FROM hscmty_erp_movcargos, hscmty_his_paciente, hscmty_his_tipo_pacientes, hscmty_his_tipo_admisiones WHERE hscmty_erp_movcargos.pid_paciente = hscmty_his_paciente.pid_paciente AND  hscmty_erp_movcargos.id_tipo_paciente = hscmty_his_tipo_pacientes.id_tipo_paciente AND hscmty_erp_movcargos.id_tipo_admisiones = hscmty_his_tipo_admisiones.id_tipo_admisiones;"; 
 
			NpgsqlDataReader lector = comando.ExecuteReader ();
		
			ContextoImp.BeginPage("Demostración");
			//NUEVO
			// Crear una fuente de tipo Impact
			Gnome.Font fuente = Gnome.Font.FindClosest
			("Bitstream Vera Sans", 12);
			Gnome.Font fuente2 = Gnome.Font.FindClosest
			("Bitstream Vera Sans", 36);
			Gnome.Font fuente3 = Gnome.Font.FindClosest
			("Bitstream Vera Sans", 9);
			
			// Cambiar la fuente
			Print.Setfont (ContextoImp, fuente);
						
			ContextoImp.MoveTo(190, 765);
		    ContextoImp.Show("REPORTE DE ADMISIONES Y REGISTRO ");
		    
		    Print.Setfont (ContextoImp, fuente2);
		    ContextoImp.MoveTo(20, 765);
      		ContextoImp.Show("________________________________");
		    
		    Print.Setfont (ContextoImp, fuente3);
		    ContextoImp.MoveTo(20, 745);
			ContextoImp.Show("PID");
				
			ContextoImp.MoveTo(60, 745);
			ContextoImp.Show("FOLIO");
			ContextoImp.MoveTo(56, 735);
			ContextoImp.Show("SERVICIO");
		        				 
			ContextoImp.MoveTo(100, 745);
		    ContextoImp.Show("FECHA");
		    ContextoImp.MoveTo(100, 735);
		    ContextoImp.Show("ADMISION");
		        
		    ContextoImp.MoveTo(150,745);
		    ContextoImp.Show("NOMBRE DEL PACIENTE");
				                
			ContextoImp.MoveTo(320, 745);                
			ContextoImp.Show("TIPO PACIENTE");
				
			ContextoImp.MoveTo(465, 745);
			ContextoImp.Show("TIPOS DE ADMISIONES");
		    			
			
			//TERMINACION
        	int filas=720;
        	//int columnas=20;
		
			while (lector.Read())
			{
				Gnome.Font fuente1 = Gnome.Font.FindClosest
				("Bitstream Vera Sans", 7);
				Print.Setfont (ContextoImp, fuente1);
			
				int pidpaciente = (int) lector["pid_paciente"];//se transforma el pid de int a string para poder ser leido
				int folioregist = (int) lector["folio_de_servicio"];
				
				ContextoImp.MoveTo(20, filas);
				ContextoImp.Show(pidpaciente.ToString());
				
				ContextoImp.MoveTo(60, filas);
				ContextoImp.Show(folioregist.ToString());
		       				 
				ContextoImp.MoveTo(100, filas);
		        ContextoImp.Show((string) lector["fech_reg_adm"]);
		        
		        ContextoImp.MoveTo(150,filas);
		        ContextoImp.Show((string) lector["nombre1_paciente"]+" "+ 
				                 (string) lector["nombre2_paciente"]+" "+
				                 (string) lector["apellido_paterno_paciente"]+" "+
				                 (string) lector["apellido_materno_paciente"]);
				                
				ContextoImp.MoveTo(320, filas);                
				ContextoImp.Show((string) lector["descripcion_tipo_paciente"]);
				
				ContextoImp.MoveTo(465, filas);
				ContextoImp.Show((string) lector["descripcion_admisiones"]);
				
				
				filas-= 12;
        	}
        	
			lector.Close (); 
			conexion.Close ();
			
			//ContextoImp.SetLineWidth(10);
			ContextoImp.ShowPage();
			
		}
		catch (NpgsqlException ex)
		{
			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			return; 
		}
		
		
		
		
	}
}