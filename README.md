# QueryLab

QueryLab est un éditeur SQL multi-SGBD, cross-platform et modulaire, écrit en C# avec Avalonia UI et une architecture MVVM extensible.

L'objectif est d'offrir un outil simple, moderne et scalable pour manipuler différentes bases de données SQL via une interface unique et portable.

## 🚀 Fonctionnalités actuelles

✅ Éditeur SQL multi-onglet avec exécution de requêtes SQL

✅ Affichage dynamique des résultats via un ResultGridControl personnalisé

✅ Gestion d’annulation des requêtes en cours d’exécution (CancellationToken)

✅ Connexion PostgreSQL locale pour le développement

✅ Architecture modulaire (Core, Domain, App, DataProviders, Infrastructure)

✅ UI cross-platform avec Avalonia 11

## 🔧 Stack technique

.NET 8 / 9

Avalonia UI 11

Microsoft.Extensions.Hosting (DI + Configuration)

CommunityToolkit.MVVM (Binding et Commandes)

ADO.NET via DbProviderFactories

PostgreSQL (via Npgsql)

Docker pour l'environnement de développement PostgreSQL

## 🔧 Pré-requis

.NET SDK 9.0

Docker (pour la base PostgreSQL locale)

## 📦 Lancer PostgreSQL en local via Docker

Créer un fichier docker-compose.yml à la racine du projet :
```yml 
services:
  postgres:
    image: postgres:16
    container_name: querylab-postgres
    restart: unless-stopped
    ports:
      - "5432:5432"
    environment:
      POSTGRES_USER: querylab
      POSTGRES_PASSWORD: querylab
      POSTGRES_DB: querylabdb
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

Lancer le conteneur PostgreSQL :

```sh 
docker-compose up -d
```

## 🔢 Exemple de requêtes de test pour QueryLab
``` sql
CREATE TABLE IF NOT EXISTS produits (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL
);
```

```sql 
INSERT INTO produits (nom, prix, stock) VALUES
('Clavier mécanique', 129.99, 50),
('Ecran 27 pouces', 349.90, 20),
('Souris gamer', 59.90, 80),
('Webcam 4K', 99.99, 35);
```
```sql 
SELECT * FROM produits;
```

🔧 Configuration de la connexion dans l'application

Utiliser DatabaseConnectionInfo dans le code :

var connectionInfo = new DatabaseConnectionInfo
{
    Name = "PostgreSQL Local",
    Provider = "Npgsql",
    ConnectionString = "Host=localhost;Port=5432;Username=querylab;Password=querylab;Database=querylabdb"
};

## 📄 Structure du projet
```txt
QueryLab/
├── QueryLab.sln
├── src/
│   ├── QueryLab.App/            (UI Avalonia)
│   ├── QueryLab.Core/           (Logique métier SQL)
│   ├── QueryLab.Domain/         (Contrats & modèles)
│   ├── QueryLab.DataProviders/  (Futurs providers DB spécifiques)
│   └── QueryLab.Infrastructure/ (Persistance future)
└── tests/
    └── QueryLab.Tests/
```

## 🔧 Build et lancement

dotnet build
dotnet run --project src/QueryLab.App

## 🛠️ Prochaines évolutions prévues

Gestion multi-connexions via ConnectionManager

Persistance des workspaces et onglets

Historique des requêtes

Support multi-SGBD (SQL Server, Oracle, SQLite...)

Export des résultats (CSV, Excel)

Syntax Highlighting SQL

Auto-complétion SQL

Parsing de schéma dynamique

Tests unitaires et end-to-end

## 🚀 Statut du projet

🔸 Phase de fondation validée. Prêt à monter les briques avancées.
