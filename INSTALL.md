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

# INSTALL

## Preze Cinemas Web - Back-end

This guide explains how to set up, build, and run the project on your local machine.

---

## 1. Usage

1. Clone the repository:

```bash
git clone https://github.com/Preze-Cinemas-Web/Back-end.git
```

2. Open solution in Visual Studio 2022 and restore NuGet packages.
3. Update the `appsettings.json` with your SQL Server connection string.
4. Run migrations to create the database:

```bash
dotnet ef database update --project CinemaData/CinemaData.csproj
```

5. Start the API:

```bash
dotnet run --project CinemaStore/CinemaStore.csproj
```

Access API documentation (Swagger) at:

https://localhost:7236/swagger/index.html
