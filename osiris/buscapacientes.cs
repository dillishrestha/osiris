///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
// 				  Ing. Hector Vargas (Diseño de Pantallas Glade)
// Licencia		: GLP
//////////////////////////////////////////////////////////
// Programa		: hscmty.cs
// Proposito	: Realiza un lista de los que necesita buscar
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Gnome;
using System.Collections;
using GtkSharp;

	
	public class busca_pacientes
	{
		[Widget] Gtk.Window busca_paciente;
		
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Entry entry_app_nom_exp;
				
		private TreeStore treeViewEngine;
		
		public busca_pacientes(string _tipo_, string LoginEmpleado, string NomEmpleado, string AppEmpleado, string ApmEmpleado) 
		{
		    //string []userName = {"Admin","Root","Guest","Nobody"};
   			//string []passWord= {"test","blank","Guest","password"};
   			
   			        
	        Console.WriteLine("registro_admision.glade");
	       	Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_paciente", null);
	        gxml.Autoconnect (this);
	        
	        busca_paciente.Show();
	        	    
	        //TreeView ListStore = new TreeView();
            treeViewEngine = new TreeStore(typeof (bool),typeof (string),typeof (string), typeof (string) );
			lista_de_Pacientes.Model = treeViewEngine;
			//treeViewEngine.SetSortFunc (0, StoreSortFunc);
		    treeViewEngine.SetSortColumnId (1, Gtk.SortType.Ascending);
					
			
			for (int i=0; i < 10; i++)
          	{
               TreeIter iter = treeViewEngine.AppendValues (null,"Demo " + i, "Data " + i, "01/01/200"+i);
          	}
			            
            //lista_de_Pacientes.Selection.Changed += onItemSelected;  // cambio de fila
            lista_de_Pacientes.RowActivated += OnRowActivated; // Doble click selecciono paciente
      		
            TreeViewColumn col_Seleccion = new TreeViewColumn();
            CellRendererToggle cellrtg = new CellRendererToggle();
            col_Seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
            col_Seleccion.PackStart(cellrtg, true);
            col_Seleccion.SetCellDataFunc (cellrtg, new TreeCellDataFunc (BoolCellDataFunc));
            col_Seleccion.AddAttribute (cellrtg,  "active", 0);    // la siguiente columna será 1 en vez de 0
            
            cellrtg.Activatable = true;
            cellrtg.Toggled += crt_toogled;
                  
	        TreeViewColumn col_PidPaciente = new TreeViewColumn();
            CellRendererText cellrt = new CellRendererText();
            col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
            col_PidPaciente.PackStart(cellrt, true);
            col_PidPaciente.AddAttribute (cellrt, "text", 1);    // la siguiente columna será 1 en vez de 0
            cellrt.Editable = true; 
            
            TreeViewColumn col_NombrePaciente = new TreeViewColumn();
            CellRendererText cellrt1 = new CellRendererText();
            col_NombrePaciente.Title = "Nombre de Paciente";
            col_NombrePaciente.PackStart(cellrt1, true);
            col_NombrePaciente.AddAttribute (cellrt1, "text", 2); // la siguiente columna será 2 en vez de 1
      
            TreeViewColumn col_fechaCrea = new TreeViewColumn();
            col_fechaCrea.Title = "Fecha";
            col_fechaCrea.PackStart(cellrt1, true);
            col_fechaCrea.AddAttribute (cellrt1, "text", 3);     // la siguiente columna será 3 en vez de 2
                              		
      		lista_de_Pacientes.AppendColumn(col_Seleccion);
            lista_de_Pacientes.AppendColumn(col_PidPaciente);
            lista_de_Pacientes.AppendColumn(col_NombrePaciente);
            lista_de_Pacientes.AppendColumn(col_fechaCrea);
         	
   		}
		
		// Al realizar el doble click soble la lista toma el valor y lo coloca en el
		// texto que va buscar
		public void OnRowActivated(object o, Gtk.RowActivatedArgs args) 
		{
    		TreeModel model;
    		TreeIter iterSelected;
        
    		if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) {
        		string seleccion_lista = (string) model.GetValue(iterSelected, 1);
        		entry_app_nom_exp.Text = seleccion_lista;
    		}
     	}
		
				
		// Marca y desmarca el toggle en la lista (el checkbox en lista)
		public void crt_toogled(object o, ToggledArgs args) 
		{
     		TreePath path = new TreePath (args.Path);
		    TreeIter iter;
		    if (lista_de_Pacientes.Model.GetIter (out iter, path))
		    {
		    	bool old = (bool) lista_de_Pacientes.Model.GetValue(iter,0);
		    	lista_de_Pacientes.Model.SetValue(iter,0,!old);
 		    }
		}
		
		// funcion para cuando se activa el checkbox en la lista (toggle)
		private void BoolCellDataFunc (TreeViewColumn col, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CellRendererToggle t = (CellRendererToggle) cell;
			//Channel c = (Channel) model.GetValue (iter, 0);
			//t.Active = c.Subscribed;
		}
				
	}
	

