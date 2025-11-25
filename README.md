# GestionBiblioteca

Este repositorio esta hecho como ejemplo de una api funcional (aun no dockerizada)

El comando para ejecutarlo es 

docker run -e MYSQL_ROOT_PASSWORD="MySup3rP4ssw0rd!" -e MYSQL_DATABASE="BiblioDB" -dp 1488:3306 mysql


docker run -e MYSQL_ROOT_PASSWORD="MySup3rP4ssw0rd!" -e MYSQL_DATABASE="BiblioDB" --mount type=bind,source=./bbddScripts,target=/docker-entrypoint-initdb.d/ -dp 1488:3306 mysql