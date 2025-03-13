# RealTimeMonitoring.WPF 
(+RealTimeMonitoring.API)

Real-Time Production Monitoring System
Overview
The Real-Time Production Monitoring System is designed to track and display real-time production data from manufacturing equipment. It utilizes a full-stack architecture with modern web and desktop technologies to ensure seamless data visualization and interaction.
Technologies Used
Backend (API)
•	ASP.NET Core Web API: Provides RESTful services and SignalR real-time communication.
•	Entity Framework Core: Handles database operations with MSSQL.
•	SignalR: Enables real-time data transmission between the server and clients.
•	Dapper (Optional): Lightweight Micro-ORM for optimized database queries.
•	#JWT Authentication: Secures API endpoints for authorized access.
•	Swagger (Swashbuckle): Provides API documentation and testing interface.
Database
•	Microsoft SQL Server: Stores production data with structured tables.
•	T-SQL (Transact-SQL): Defines database schema and query operations.
Frontend (Desktop & Web)
WPF Client
•	XAML (Windows Presentation Foundation): Builds an interactive desktop GUI.
•	HttpClient: Handles API calls to fetch production data.
•	SignalR Client: Listens for real-time data updates.
•	ObservableCollection: Manages dynamic UI data binding.
React.js Client (Optional)
•	React.js: Builds a dynamic, modern web interface.
•	Axios: Manages API requests.
•	SignalR Client (ASP.NET Core): Enables real-time data streaming.
•	Recharts: Displays production analytics in graphical form.
DevOps & Version Control
•	Azure DevOps & Git: Manages source control and CI/CD pipelines.
•	Docker (Optional): Containerizes the application for easy deployment.
System Architecture
1.	Production Equipment → Sends data to API (ASP.NET Core).
2.	ASP.NET Core API → Stores data in MSSQL Database.
3.	SignalR Hub → Broadcasts updates to connected clients.
4.	WPF/React Client → Receives & visualizes data.
Implementation Steps
Step 1: Setting Up Backend (ASP.NET Core API)
•	Create an ASP.NET Core Web API project in Visual Studio.
•	Install dependencies: Entity Framework Core, SignalR, Swagger, Microsoft SQL Server.
•	Define the ProductionData model:
public class ProductionData {
    public int Id { get; set; }
    public string MachineName { get; set; }
    public double Efficiency { get; set; }
    public string Status { get; set; } // Running, Idle, Error
    public int ProductionCount { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string ErrorLog { get; set; }
    public DateTime Timestamp { get; set; }
}
•	Configure Entity Framework Core and database connection.
•	Implement ProductionHub using SignalR:
public class ProductionHub : Hub {
    public async Task SendProductionData(ProductionData data) {
        await Clients.All.SendAsync("ReceiveProductionData", data);
    }
}
•	Expose API Endpoints for data retrieval and submission.
Step 2: Setting Up Database
•	Run dotnet ef migrations add InitialCreate to generate migration files.
•	Execute dotnet ef database update to create the database.
Step 3: Implementing WPF Client
•	Create a WPF application in Visual Studio.
•	Add a DataGrid to display production records.
•	Implement SignalR Client for real-time updates.
•	Use HttpClient to fetch and send data to the API.
Step 4: Testing & Deployment
•	Run swagger (https://localhost:7053/swagger) to verify API endpoints.
•	Test WPF client for real-time updates and database interactions.
•	Deploy using Azure DevOps Pipelines or Docker.
Conclusion
This system efficiently monitors real-time production metrics, ensuring a robust and scalable solution for manufacturing environments. The integration of ASP.NET Core, SignalR, MSSQL, WPF, and React.js enables a dynamic and interactive user experience.
________________________________________

