# Projet dal_postgres

## Présentation

Ce projet représente la DAL contenant le contexte de notre base de données applicative appdb (nom du service postgre dans docker-compose)

Pour pouvoir utiliser les commande `dotnet ef`, ajouter `--startup-project ../dalinit/` à la fin de chaque commande

Exemple :

`dotnet ef dbcontext info --startup-project ../dalinit/`

## Création de la migration initiale

`dotnet ef migrations add Initialisation --startup-project ../dalinit/`

## Environnement de debug

Lancer un container : cela va créé un user `webapi` et une base de données du même nom

`docker run -d -e POSTGRES_USER=webapi -e POSTGRES_PASSWORD=mypostgrespassword! --name appdb -p 5432:5432 postgres`

Et lancer le site en debug

## Lancer des commandes SQL dans le container

`docker exec -it appdb psql -U webapi -W moneyboard`


