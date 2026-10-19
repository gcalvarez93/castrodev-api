# CastroDev API

The shared REST backend behind the CastroDev mobile portfolio. One .NET 10 service, organised in feature modules, serves six apps on Flutter, iOS and Android.

---

## ✨ What it does

| Module | Route prefix | Used by |
|---|---|---|
| Common (users) | `/api/common/users` | all apps |
| Finance | `/api/finance` (transactions, categories, budgets, balance, PDF/Excel export, receipt scan) | finance-tracker |
| Habits | `/api/habits` | habit-tracker |
| Tasks | `/api/tasks`, `/api/tasks/boards` | task-manager |
| Budget Scanner | `/api/budgetscanner` (budgets, transactions, analytics, receipt scan) | budget-scanner |
| Recipes | `/api/recipes` (recipes, meal plan, shopping list, external import) | recipe-manager |
| Workout | `/api/workout` (routines, sessions, body weight, analytics, external import) | workout-tracker |

`GET /health` is anonymous. Swagger UI is enabled. Every other route requires a Firebase ID token.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language / runtime | C# on .NET 10, Minimal APIs |
| Application layer | MediatR (one handler per use case) |
| Error handling | ErrorOr (`Result` pattern) |
| Authentication | Firebase Auth ID tokens, validated as JWT bearer |
| Database | Cloud Firestore |
| OCR | Google Cloud Vision |
| Documents | iText (PDF), ClosedXML (Excel) |
| Tests | xUnit, Moq, FluentAssertions |
| Hosting | Google Cloud Run (listens on `PORT`) |

---

## 🏗️ Architecture

```
src/
├── Common/                  # Users, shared domain and endpoints
└── Modules/
    ├── Finance/
    ├── Habits/
    ├── Tasks/
    ├── BudgetScanner/
    ├── RecipeManager/
    └── WorkoutTracker/
        ├── Domain/          # Entities and repository interfaces
        ├── Application/     # Commands, queries, handlers, DTOs
        ├── Infrastructure/  # Firestore repositories and services
        └── Presentation/    # Minimal API endpoints
tests/
└── Api.Tests/               # Handler tests with mocked repositories
```

---

## 🧭 Technical Decisions

- **Feature modules with Clean Architecture.** Each module has its own `Domain / Application / Infrastructure / Presentation`. The domain layer has no dependency on Firestore, so handlers are tested with mocked repositories.
- **One handler per use case, through MediatR.** Endpoints only read the user, send a command or query and map the result to HTTP. The trade-off is many small files per module.
- **Expected failures are values, not exceptions.** Handlers return `ErrorOr<T>` and the endpoint maps it (`NotFound` to `404`, and so on). The application layer does not throw for "not found", which used to surface as a `500`.
- **Authentication with Firebase ID tokens.** The API validates issuer, audience and lifetime of the JWT, and reads the `user_id` claim.
- **Every repository call is scoped by user.** Lookups take `(id, userId)`, so one user cannot read or change another user's data by guessing an id.
- **No credentials in the repository.** The Firebase Admin SDK and Firestore use Application Default Credentials, supplied by Cloud Run or by `GOOGLE_APPLICATION_CREDENTIALS` locally. Only the public Firebase project id is in `appsettings.json`.
- **External data behind named `HttpClient`s.** TheMealDB and Wger are imported through the API, so the apps depend on one contract instead of several third-party ones.
- **One backend for six apps.** Business rules live on the server, so the Flutter, iOS and Android clients stay thin and behave the same.
- **Tests with hand-written scenarios, not a mocking-heavy suite.** xUnit, Moq and FluentAssertions cover the not-found and happy paths of representative handlers; CI runs them on every push.

### Other projects in this portfolio

| Project | Platform | Language |
|---|---|---|
| [finance-tracker](https://github.com/gcalvarez93/finance-tracker) | Flutter (iOS + Android) | Dart |
| [habit-tracker](https://github.com/gcalvarez93/habit-tracker) | Flutter (iOS + Android) | Dart |
| [task-manager](https://github.com/gcalvarez93/task-manager) | iOS | Swift |
| [budget-scanner](https://github.com/gcalvarez93/budget-scanner) | iOS | Swift |
| [recipe-manager](https://github.com/gcalvarez93/recipe-manager) | Android | Kotlin |
| [workout-tracker](https://github.com/gcalvarez93/workout-tracker) | Android | Kotlin |
| [castrodev-api](https://github.com/gcalvarez93/castrodev-api) | Shared backend | C# (.NET 10) |

---

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- A Firebase project with Firestore and Authentication enabled
- Application Default Credentials: `gcloud auth application-default login`, or `GOOGLE_APPLICATION_CREDENTIALS` pointing to a service-account key kept **outside** the repository

### Run

```bash
git clone https://github.com/gcalvarez93/castrodev-api.git
cd castrodev-api

# Use your own Firebase project id
dotnet user-secrets init --project api.csproj
dotnet user-secrets set "Firebase:ProjectId" "<your-project-id>" --project api.csproj

dotnet run --project api.csproj
```

The API listens on `PORT` (default `8080`). Swagger UI is at `/swagger`.

### Running Tests

```bash
dotnet test CastroDev.Api.slnx
```

---

## 🔗 Related

- [Finance Tracker](https://github.com/gcalvarez93/finance-tracker) — Personal finance app (Flutter)
- [Habit Tracker](https://github.com/gcalvarez93/habit-tracker) — Daily habit tracking app (Flutter)
- [Task Manager](https://github.com/gcalvarez93/task-manager) — Board-based task management (iOS)
- [Budget Scanner](https://github.com/gcalvarez93/budget-scanner) — Budget and expense tracking (iOS)
- [Recipe Manager](https://github.com/gcalvarez93/recipe-manager) — Recipes and meal planning (Android)
- [Workout Tracker](https://github.com/gcalvarez93/workout-tracker) — Routines and sessions (Android)

---

## 📄 License

MIT © Gabriel Castro

---

---

# CastroDev API

El backend REST compartido del portfolio móvil de CastroDev. Un único servicio .NET 10, organizado en módulos por feature, da servicio a seis apps en Flutter, iOS y Android.

---

## ✨ Qué hace

| Módulo | Prefijo de ruta | Lo usa |
|---|---|---|
| Common (usuarios) | `/api/common/users` | todas las apps |
| Finance | `/api/finance` (transacciones, categorías, presupuestos, balance, exportación PDF/Excel, escaneo de recibos) | finance-tracker |
| Habits | `/api/habits` | habit-tracker |
| Tasks | `/api/tasks`, `/api/tasks/boards` | task-manager |
| Budget Scanner | `/api/budgetscanner` (presupuestos, transacciones, análisis, escaneo de recibos) | budget-scanner |
| Recipes | `/api/recipes` (recetas, plan semanal, lista de la compra, importación externa) | recipe-manager |
| Workout | `/api/workout` (rutinas, sesiones, peso corporal, análisis, importación externa) | workout-tracker |

`GET /health` es anónimo. Swagger UI está activado. El resto de rutas exigen un ID token de Firebase.

---

## 🛠️ Stack Tecnológico

| Capa | Tecnología |
|---|---|
| Lenguaje / runtime | C# sobre .NET 10, Minimal APIs |
| Capa de aplicación | MediatR (un handler por caso de uso) |
| Gestión de errores | ErrorOr (patrón `Result`) |
| Autenticación | ID tokens de Firebase Auth, validados como JWT bearer |
| Base de datos | Cloud Firestore |
| OCR | Google Cloud Vision |
| Documentos | iText (PDF), ClosedXML (Excel) |
| Tests | xUnit, Moq, FluentAssertions |
| Hosting | Google Cloud Run (escucha en `PORT`) |

---

## 🧭 Decisiones Técnicas

- **Módulos por feature con Clean Architecture.** Cada módulo tiene su propio `Domain / Application / Infrastructure / Presentation`. El dominio no depende de Firestore, así que los handlers se prueban con repositorios simulados.
- **Un handler por caso de uso, con MediatR.** Los endpoints solo leen el usuario, envían un comando o consulta y convierten el resultado a HTTP. El inconveniente son muchos archivos pequeños por módulo.
- **Los fallos esperados son valores, no excepciones.** Los handlers devuelven `ErrorOr<T>` y el endpoint lo traduce (`NotFound` a `404`, etc.). La capa de aplicación no lanza excepciones por "no encontrado", que antes acababa en un `500`.
- **Autenticación con ID tokens de Firebase.** La API valida emisor, audiencia y caducidad del JWT y lee el claim `user_id`.
- **Toda llamada a un repositorio está acotada al usuario.** Las búsquedas reciben `(id, userId)`, así que un usuario no puede leer ni modificar datos de otro adivinando un id.
- **Sin credenciales en el repositorio.** El Firebase Admin SDK y Firestore usan Application Default Credentials, aportadas por Cloud Run o por `GOOGLE_APPLICATION_CREDENTIALS` en local. En `appsettings.json` solo está el id público del proyecto Firebase.
- **Datos externos tras `HttpClient` con nombre.** TheMealDB y Wger se importan a través de la API, así que las apps dependen de un único contrato en lugar de varios de terceros.
- **Un solo backend para seis apps.** Las reglas de negocio viven en el servidor, por lo que los clientes Flutter, iOS y Android se mantienen finos y se comportan igual.
- **Tests con escenarios escritos a mano, no una suite basada en mocks.** xUnit, Moq y FluentAssertions cubren el caso "no encontrado" y el caso correcto de handlers representativos; la CI los ejecuta en cada push.

### Otros proyectos del portfolio

| Proyecto | Plataforma | Lenguaje |
|---|---|---|
| [finance-tracker](https://github.com/gcalvarez93/finance-tracker) | Flutter (iOS + Android) | Dart |
| [habit-tracker](https://github.com/gcalvarez93/habit-tracker) | Flutter (iOS + Android) | Dart |
| [task-manager](https://github.com/gcalvarez93/task-manager) | iOS | Swift |
| [budget-scanner](https://github.com/gcalvarez93/budget-scanner) | iOS | Swift |
| [recipe-manager](https://github.com/gcalvarez93/recipe-manager) | Android | Kotlin |
| [workout-tracker](https://github.com/gcalvarez93/workout-tracker) | Android | Kotlin |
| [castrodev-api](https://github.com/gcalvarez93/castrodev-api) | Backend compartido | C# (.NET 10) |

---

## 🚀 Instalación

### Requisitos previos

- SDK de .NET 10
- Un proyecto Firebase con Firestore y Authentication activados
- Application Default Credentials: `gcloud auth application-default login`, o `GOOGLE_APPLICATION_CREDENTIALS` apuntando a una clave de cuenta de servicio guardada **fuera** del repositorio

### Ejecutar

```bash
git clone https://github.com/gcalvarez93/castrodev-api.git
cd castrodev-api

# Usa el id de tu propio proyecto Firebase
dotnet user-secrets init --project api.csproj
dotnet user-secrets set "Firebase:ProjectId" "<tu-project-id>" --project api.csproj

dotnet run --project api.csproj
```

La API escucha en `PORT` (por defecto `8080`). Swagger UI está en `/swagger`.

### Tests

```bash
dotnet test CastroDev.Api.slnx
```

---

## 🔗 Relacionado

- [Finance Tracker](https://github.com/gcalvarez93/finance-tracker) — App de finanzas personales (Flutter)
- [Habit Tracker](https://github.com/gcalvarez93/habit-tracker) — App de seguimiento de hábitos (Flutter)
- [Task Manager](https://github.com/gcalvarez93/task-manager) — Gestión de tareas por tableros (iOS)
- [Budget Scanner](https://github.com/gcalvarez93/budget-scanner) — Presupuestos y gastos (iOS)
- [Recipe Manager](https://github.com/gcalvarez93/recipe-manager) — Recetas y planificación de comidas (Android)
- [Workout Tracker](https://github.com/gcalvarez93/workout-tracker) — Rutinas y sesiones (Android)

---

## 📄 Licencia

MIT © Gabriel Castro
# CastroDev API

El backend REST compartido del portfolio móvil de [CastroDev](https://castrodev.com). Un único servicio .NET 10, organizado en módulos por feature, da servicio a seis apps en Flutter, iOS y Android.

---

## ✨ Qué hace

| Módulo | Prefijo de ruta | Lo usa |
|---|---|---|
| Common (usuarios) | `/api/common/users` | todas las apps |
| Finance | `/api/finance` (transacciones, categorías, presupuestos, balance, exportación PDF/Excel, escaneo de recibos) | finance-tracker |
| Habits | `/api/habits` | habit-tracker |
| Tasks | `/api/tasks`, `/api/tasks/boards` | task-manager |
| Budget Scanner | `/api/budgetscanner` (presupuestos, transacciones, análisis, escaneo de recibos) | budget-scanner |
| Recipes | `/api/recipes` (recetas, plan semanal, lista de la compra, importación externa) | recipe-manager |
| Workout | `/api/workout` (rutinas, sesiones, peso corporal, análisis, importación externa) | workout-tracker |

`GET /health` es anónimo. Swagger UI está activado. El resto de rutas exigen un ID token de Firebase.

---

## 🛠️ Stack Tecnológico

| Capa | Tecnología |
|---|---|
| Lenguaje / runtime | C# sobre .NET 10, Minimal APIs |
| Capa de aplicación | MediatR (un handler por caso de uso) |
| Gestión de errores | ErrorOr (patrón `Result`) |
| Autenticación | ID tokens de Firebase Auth, validados como JWT bearer |
| Base de datos | Cloud Firestore |
| OCR | Google Cloud Vision |
| Documentos | iText (PDF), ClosedXML (Excel) |
| Tests | xUnit, Moq, FluentAssertions |
| Hosting | Google Cloud Run (escucha en `PORT`) |

---

## 🧭 Decisiones Técnicas

- **Módulos por feature con Clean Architecture.** Cada módulo tiene su propio `Domain / Application / Infrastructure / Presentation`. El dominio no depende de Firestore, así que los handlers se prueban con repositorios simulados.
- **Un handler por caso de uso, con MediatR.** Los endpoints solo leen el usuario, envían un comando o consulta y convierten el resultado a HTTP. El inconveniente son muchos archivos pequeños por módulo.
- **Los fallos esperados son valores, no excepciones.** Los handlers devuelven `ErrorOr<T>` y el endpoint lo traduce (`NotFound` a `404`, etc.). La capa de aplicación no lanza excepciones por "no encontrado", que antes acababa en un `500`.
- **Autenticación con ID tokens de Firebase.** La API valida emisor, audiencia y caducidad del JWT y lee el claim `user_id`.
- **Toda llamada a un repositorio está acotada al usuario.** Las búsquedas reciben `(id, userId)`, así que un usuario no puede leer ni modificar datos de otro adivinando un id.
- **Sin credenciales en el repositorio.** El Firebase Admin SDK y Firestore usan Application Default Credentials, aportadas por Cloud Run o por `GOOGLE_APPLICATION_CREDENTIALS` en local. En `appsettings.json` solo está el id público del proyecto Firebase.
- **Datos externos tras `HttpClient` con nombre.** TheMealDB y Wger se importan a través de la API, así que las apps dependen de un único contrato en lugar de varios de terceros.
- **Un solo backend para seis apps.** Las reglas de negocio viven en el servidor, por lo que los clientes Flutter, iOS y Android se mantienen finos y se comportan igual.
- **Tests con escenarios escritos a mano, no una suite basada en mocks.** xUnit, Moq y FluentAssertions cubren el caso "no encontrado" y el caso correcto de handlers representativos; la CI los ejecuta en cada push.

### Otros proyectos del portfolio

| Proyecto | Plataforma | Lenguaje |
|---|---|---|
| [finance-tracker](https://github.com/gcalvarez93/finance-tracker) | Flutter (iOS + Android) | Dart |
| [habit-tracker](https://github.com/gcalvarez93/habit-tracker) | Flutter (iOS + Android) | Dart |
| [task-manager](https://github.com/gcalvarez93/task-manager) | iOS | Swift |
| [budget-scanner](https://github.com/gcalvarez93/budget-scanner) | iOS | Swift |
| [recipe-manager](https://github.com/gcalvarez93/recipe-manager) | Android | Kotlin |
| [workout-tracker](https://github.com/gcalvarez93/workout-tracker) | Android | Kotlin |
| [castrodev-api](https://github.com/gcalvarez93/castrodev-api) | Backend compartido | C# (.NET 10) |

---

## 🚀 Instalación

### Requisitos previos

- SDK de .NET 10
- Un proyecto Firebase con Firestore y Authentication activados
- Application Default Credentials: `gcloud auth application-default login`, o `GOOGLE_APPLICATION_CREDENTIALS` apuntando a una clave de cuenta de servicio guardada **fuera** del repositorio

### Ejecutar

```bash
git clone https://github.com/gcalvarez93/castrodev-api.git
cd castrodev-api

# Usa el id de tu propio proyecto Firebase
dotnet user-secrets init --project api.csproj
dotnet user-secrets set "Firebase:ProjectId" "<tu-project-id>" --project api.csproj

dotnet run --project api.csproj
```

La API escucha en `PORT` (por defecto `8080`). Swagger UI está en `/swagger`.

### Tests

```bash
dotnet test CastroDev.Api.slnx
```

---

## 🔗 Relacionado

- [Finance Tracker](https://github.com/gcalvarez93/finance-tracker) — App de finanzas personales (Flutter)
- [Habit Tracker](https://github.com/gcalvarez93/habit-tracker) — App de seguimiento de hábitos (Flutter)
- [Task Manager](https://github.com/gcalvarez93/task-manager) — Gestión de tareas por tableros (iOS)
- [Budget Scanner](https://github.com/gcalvarez93/budget-scanner) — Presupuestos y gastos (iOS)
- [Recipe Manager](https://github.com/gcalvarez93/recipe-manager) — Recetas y planificación de comidas (Android)
- [Workout Tracker](https://github.com/gcalvarez93/workout-tracker) — Rutinas y sesiones (Android)
- [castrodev.com](https://castrodev.com) — Portfolio
