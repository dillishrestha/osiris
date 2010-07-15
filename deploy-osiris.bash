#!/bin/bash

REMOTE="dolivares@172.16.1.11"
NOMBD=`grep "Database=osiris_produccion" /opt/osiris/osiris/hscmty.cs | cut -c29-44`

if test $NOMBD = "Database=hscmty;"
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
	ssh $REMOTE "chown -R dolivares /opt/osiris"
fi
