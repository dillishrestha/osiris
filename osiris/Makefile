# **************************************************************************
# Fichero makefile.
# --------------------------------------------------------------------------
# Licencia GPL. Juan Gonzalez Gomez
# --------------------------------------------------------------------------
#
#***************************************************************************
#-- Compilador de C#
MCS = mcs

#-------- Nombres y dependencias de los programas a construir
NAME1= cube-virtual-mono.exe
DEP1= cube-virtual.cs Angulo.cs Articulacion.cs Funcion.cs GFuncion.cs \
      GGusano.cs Gusano.cs Vector.cs

all: $(NAME1)

#--------------------------
#  Reglas
#--------------------------
$(NAME1): $(DEP1) 
	 $(MCS) $(DEP1) -pkg:gtk-sharp -pkg:glade-sharp -out:$(NAME1)
   
clean::
	  rm -f *.bak $(NAME1)     
    
   
clean::
	  rm -f $(NAME6).exe       
	  