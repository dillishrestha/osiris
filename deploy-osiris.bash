#!/bin/bash

REMOTE="root@192.168.1.10"
NOMBD=`grep "Database=osiris_produccion" /home/dolivares/Projects/systemsosirisho/osiris/class_conexion.cs | cut -c30-55`

if test $NOMBD = "Database=osiris_produccion;"
then 
	echo "Instalando Sistema Osiris en Servidor de Produccion..."

	scp /opt/osiris/bin/Debug/* $REMOTE:/opt/osiris/bin/
	scp /opt/osiris/img/* $REMOTE:/opt/osiris/img/
	scp /opt/osiris/osiris/* $REMOTE:/opt/osiris/osiris/
	scp /opt/osiris/osiris.mds $REMOTE:/opt/osiris/

	echo "Fin de la instalacion"
else
	echo "Error no esta en la base de datos correcta"
	echo "Cambiando permisos"
	ssh $REMOTE "chown -R root /opt/osiris"
fi
