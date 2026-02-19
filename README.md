<p align="center">
  <img src="https://www.especial.gr/wp-content/uploads/2019/03/panepisthmio-dut-attikhs.png" alt="UNIWA" width="150"/>
</p>

<p align="center">
  <strong>UNIVERSITY OF WEST ATTICA</strong><br>
  SCHOOL OF ENGINEERING<br>
  DEPARTMENT OF COMPUTER ENGINEERING AND INFORMATICS
</p>

<p align="center">
  <a href="https://www.uniwa.gr" target="_blank">University of West Attica</a> ·
  <a href="https://ice.uniwa.gr" target="_blank">Department of Computer Engineering and Informatics</a>
</p>

---

<p align="center">
  <strong>Special Topics in Software Engineering</strong>
</p>

<h1 align="center">
  Preze Cinemas Web<br>
  Back-end 
</h1>

<p align="center">
  <strong>Vasileios Evangelos Athanasiou</strong><br>
  Student ID: 19390005
</p>

<p align="center">
  <a href="https://github.com/Ath21" target="_blank">GitHub</a> ·
  <a href="https://www.linkedin.com/in/vasilis-athanasiou-7036b53a4/" target="_blank">LinkedIn</a>
</p>

<p align="center">
  <strong>Spyros Dellaportas</strong><br>
  Student ID: 20390054
</p>

<p align="center">
  <a href="https://github.com/Doubleshot243" target="_blank">GitHub</a>
</p>

<hr/>

<p align="center">
  <strong>Supervision</strong>
</p>

<p align="center">
  Supervisor: Georgios Prezerakos, Professor
</p>
<p align="center">
  <a href="https://ice.uniwa.gr/en/emd_person/george-prezerakos/" target="_blank">UNIWA Profile</a> ·
  <a href="https://www.linkedin.com/in/georgenprezerakos/" target="_blank">LinkedIn</a>
</p>

</hr>

---

<p align="center">
  Athens, February 2024
</p>

---

<p align="center">
  <img src="https://raygun.com/blog/images/net-6-features/feature.png" width="250"/>
</p>

---

# README

## Preze Cinemas Web - Back-end

**Preze Cinemas Web** is a full-stack cinema booking system. This repository contains the **back-end API** layer and data management services.

The back-end is implemented in **C# (.NET 6)** and exposes **RESTful APIs** for:

- User authentication and management
- Movie and hall information
- Ticket reservation and confirmation
- Payment handling
- Integration with front-end UI

The project follows **clean architecture** principles with separate layers for **data**, **business logic**, **API controllers**, and **integration tests**.

> Front-end development is managed separately: [Preze Cinemas Front-End](https://github.com/Preze-Cinemas-Web/Front-end.git)

---

## Repository Structure

```bash
Back-end/
├── CinemaData/ # Data layer (Entities, DbContext, Migrations)
├── CinemaStore/ # API Layer (Controllers, Business Logic, Models)
├── CinemaStoreIntegrationTests/ # Integration tests for API
├── assign/ # Assignment instructions and reference materials
├── walkthrough/ # Demo walkthrough video
├── .github/ # CI/CD workflows
├── CinemaWebapp.sln # Visual Studio solution
└── README.md
└── INSTALL.md
```

---

## 1. Key Folders

| Folder                         | Description                                                                                                                               |
| ------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------- |
| `CinemaData/`                  | Data layer implementing **Entity Framework Core** ORM, migrations, and entity models (`Movie.cs`, `Hall.cs`, `User.cs`, `Reservation.cs`) |
| `CinemaStore/`                 | API layer exposing **REST endpoints** with controllers, DTOs, and business logic services. Uses **Dependency Injection** for modularity.  |
| `CinemaStoreIntegrationTests/` | Integration tests for API endpoints using **xUnit** and **Microsoft.AspNetCore.Mvc.Testing**                                              |
| `assign/`                      | Course assignment instructions and guidelines                                                                                             |
| `walkthrough/`                 | Demo video showcasing system functionality                                                                                                |

---

## 2. Project Layers & Dependencies

### 2.1 CinemaData (Data Layer)

- **Target Framework:** .NET 6.0
- **Packages:**
  - AutoMapper 12.0.1
  - Entity Framework Core 7.0 (SQL Server provider)
  - EF Core Tools and Design packages
  - AutoMapper Dependency Injection & Expression Mapping

**Responsibilities:**

- Defining entity models and relationships
- Database migrations
- ORM-based CRUD operations

### 2.2 CinemaStore (API Layer)

- **Target Framework:** .NET 6.0 Web API
- **Packages:**
  - AutoMapper
  - MailKit (email notifications)
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore
  - JWT Authentication
  - EF Core (SQL Server)
  - Swashbuckle (Swagger/OpenAPI)

**Responsibilities:**

- Exposing API endpoints for the front-end
- Business logic services for Users, Movies, Reservations, Halls
- DTO mapping using AutoMapper
- Payment & email integration

### 2.3 CinemaStoreIntegrationTests

- **Target Framework:** .NET 6.0
- **Packages:**
  - xUnit
  - Microsoft.NET.Test.Sdk
  - Microsoft.AspNetCore.Mvc.Testing
  - Coverlet for code coverage

**Responsibilities:**

- End-to-end integration testing of API controllers
- Validates the interaction between API layer and database

---

## 3. Development Practices

- **Dependency Injection:** Used throughout all business services and controllers
- **ORM Usage:** Entity Framework Core (DbContext, Migrations, Fluent API)
- **CRUD & REST:** Full CRUD operations implemented for all main entities
- **Integration Testing:** Automated tests to verify controller endpoints and business logic
- **Project Management:** Organized in **sprints** with **user stories** on Trello: [Project Board](https://trello.com/b/r4rFhJmz/cinema-webpage)

---

## 4. Team

| Role      | Members      |
| --------- | ------------ |
| Back-End  | 2 developers |
| Front-End | 2 developers |

---

## 5. Related Repositories

[Preze Cinemas Front-End](https://github.com/Preze-Cinemas-Web/Front-end.git)
