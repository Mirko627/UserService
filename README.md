# User Service

## Descrizione

Il **User Service** è un microservizio responsabile della gestione degli utenti.

Fornisce operazioni CRUD sugli utenti e gestisce le funzionalità di autenticazione e aggiornamento delle credenziali.

## Architettura

Il servizio è standalone e non comunica con altri microservizi.

## Avvio del servizio

Per avviare il servizio in locale:

```bash id="1c8q2v"
# esempio
dotnet run
```

Oppure con Docker:

```bash id="b7z3ya"
docker-compose up
```

Il servizio sarà disponibile su:

```id="s9kx4n"
http://localhost:7802
```

## API

Documentazione Swagger disponibile qui:

```id="n2df6m"
http://localhost:7802/swagger/index.html
```

## Endpoints principali

### Utenti

| Metodo | Endpoint       | Descrizione               |
| ------ | -------------- | ------------------------- |
| GET    | /api/User      | Recupera tutti gli utenti |
| GET    | /api/User/{id} | Recupera un utente per ID |
| POST   | /api/User      | Crea un nuovo utente      |
| PUT    | /api/User/{id} | Aggiorna un utente        |
| DELETE | /api/User/{id} | Elimina un utente         |

### Autenticazione

| Metodo | Endpoint                       | Descrizione              |
| ------ | ------------------------------ | ------------------------ |
| POST   | /api/User/login                | Login utente             |
| PATCH  | /api/User/change-password/{id} | Modifica password utente |

