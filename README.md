# Alap
A full-stack, production-ready chatbot platform built using **ASP.NET Core MVC**, **SignalR**, **Entity Framework Core**, **ASP.NET Identity**, **SQL Server**, and **Tavily AI**.
- Real-time messaging with SignalR
- AI-powered responses from Tavily
- Chat history with infinite scroll
- Edit, Delete, Approve messages (moderation)
- User authentication (ASP.NET Identity)
- Clean architecture using Repository + Unit of Work
- Deploy in Docker

- ## 🛠️ Tech Stack

| Layer       | Technology                          |
|-------------|--------------------------------------|
| Backend     | ASP.NET Core 8 (MVC + Web API)       |
| Realtime    | SignalR                              |
| Frontend    | Razor Views + JS + Bootstrap         |
| AI Service  | Tavily Search API                    |
| Auth        | ASP.NET Identity                     |
| DB          | SQL Server / LocalDb + EF Core       |
| Arch        | Repository + Unit of Work Pattern    |
| Deploy      | Docker                               |

"ConnectionStrings": {
  "DefaultConnection": "Server=host.docker.internal,1433;Database=AlapDb;User Id=saymon;Password=Saymon@005;MultipleActiveResultSets=true;TrustServerCertificate=True;"
},

"Tavily": {
  "ApiKey": "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"
},
