# User Service

## Descrizione

Il **User Service** è il microservizio responsabile della gestione degli utenti e dell'autenticazione.

Fornisce funzionalità di registrazione, autenticazione tramite JWT, gestione del profilo utente e cambio password.

## Architettura

Il servizio fa parte di un'architettura a microservizi:

* Espone API REST
* Gestisce autenticazione e autorizzazione tramite JWT
* Archivia le password in forma cifrata mediante BCrypt
* Fornisce i token JWT utilizzati dagli altri microservizi del sistema

## Avvio del servizio

Per avviare il servizio in locale:

```bash
# esempio
dotnet run
```

Oppure con Docker:

```bash
docker-compose up
```

Il servizio sarà disponibile su:

```
http://localhost:7802
```

## API

Documentazione Swagger disponibile qui:

```
http://localhost:7802/swagger/index.html
```

## Autenticazione e autorizzazione

Il servizio utilizza **JWT (JSON Web Token)** per autenticare gli utenti e autorizzare l'accesso alle operazioni protette.

### Accesso pubblico

Le seguenti operazioni sono accessibili senza autenticazione:

* Registrazione di un nuovo utente
* Login
* Visualizzazione di tutti gli utenti
* Visualizzazione di un utente specifico

### Accesso autenticato

Le seguenti operazioni richiedono un token JWT valido:

* Modifica del proprio profilo
* Eliminazione del proprio account
* Cambio password

Il client deve includere il token nell'header HTTP:

```http
Authorization: Bearer <token>
```

### Regole di autorizzazione

#### Registrazione

Un nuovo utente può registrarsi fornendo i dati richiesti dal sistema.

Durante la registrazione:

* lo username deve essere univoco
* la password viene cifrata tramite BCrypt prima del salvataggio

#### Login

L'autenticazione avviene tramite username e password.

Se le credenziali sono valide, il sistema restituisce un token JWT contenente:

* ID dell'utente
* username
* ruolo dell'utente

#### Modifica del profilo

Un utente può modificare esclusivamente il proprio profilo.

Non è consentito modificare i dati di altri utenti.

#### Eliminazione dell'account

Un utente può eliminare esclusivamente il proprio account.

Non è consentito eliminare altri utenti.

#### Cambio password

Un utente può modificare esclusivamente la propria password.

Per effettuare il cambio password:

* deve fornire la password attuale corretta
* la nuova password deve essere diversa da quella precedente

## Sicurezza

### Gestione delle password

Le password non vengono mai salvate in chiaro.

Il sistema utilizza **BCrypt** per:

* hashing delle password durante la registrazione
* verifica delle credenziali durante il login
* aggiornamento sicuro delle password

### JWT

I token JWT contengono le seguenti informazioni:

| Claim          | Descrizione       |
| -------------- | ----------------- |
| NameIdentifier | ID dell'utente    |
| Name           | Username          |
| Role           | Ruolo dell'utente |

Caratteristiche del token:

* firma tramite HMAC SHA-256
* scadenza dopo 1 ora
* utilizzabile dagli altri microservizi per l'autorizzazione delle richieste

## Endpoints principali

| Metodo | Endpoint                       | Autenticazione | Descrizione                            |
| ------ | ------------------------------ | -------------- | -------------------------------------- |
| GET    | /api/User                      | ❌ No           | Recupera tutti gli utenti              |
| GET    | /api/User/{id}                 | ❌ No           | Recupera un utente per ID              |
| POST   | /api/User                      | ❌ No           | Registra un nuovo utente               |
| POST   | /api/User/login                | ❌ No           | Effettua il login e restituisce un JWT |
| PUT    | /api/User/{id}                 | ✅ Sì           | Aggiorna il proprio profilo            |
| PATCH  | /api/User/change-password/{id} | ✅ Sì           | Modifica la propria password           |
| DELETE | /api/User/{id}                 | ✅ Sì           | Elimina il proprio account             |

## Controlli automatici

* Verifica dell'unicità dello username durante la registrazione.
* Cifratura automatica delle password tramite BCrypt.
* Aggiornamento automatico del campo `LastModified` durante la modifica del profilo.
* Aggiornamento automatico del campo `LastModified` durante il cambio password.
* Verifica della password precedente prima dell'aggiornamento.
* Generazione automatica del token JWT dopo un login effettuato con successo.

## Integrazioni

### Sistema di autenticazione centralizzato

Il User Service rappresenta il punto centrale per l'autenticazione dell'intera piattaforma.

Gli altri microservizi utilizzano i claim contenuti nel JWT per:

* identificare l'utente autenticato
* verificare la proprietà delle risorse
* applicare le regole di autorizzazione

### Informazioni propagate nel token

Ogni token JWT contiene:

* ID dell'utente
* username
* ruolo dell'utente

Queste informazioni consentono agli altri microservizi di implementare controlli di sicurezza distribuiti senza dover interrogare continuamente il User Service.
