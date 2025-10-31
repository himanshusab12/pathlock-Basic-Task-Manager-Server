# Task Manager RESTful API

A high-performance backend service built with ASP.NET Core, providing robust task management capabilities through a clean RESTful interface with in-memory data persistence.

---

## 🌐 Live Service

**Production API:** [https://pathlock-basic-task-manager-server.onrender.com](https://pathlock-project-1-server.onrender.com)

**API Documentation:** [https://pathlock-basic-task-manager-server.onrender.com/swagger](https://pathlock-project-1-server.onrender.com/swagger)

---

## 📖 About This Service

This backend API delivers a streamlined task management solution designed for modern web applications. Built using ASP.NET Core's minimal API approach, it offers lightning-fast response times and thread-safe operations through in-memory storage, making it perfect for rapid development and testing scenarios.

### Technology Stack

**Backend Framework:**
- ASP.NET Core (Minimal APIs)
- .NET 8.0 SDK
- ConcurrentDictionary for thread-safe storage
- Swagger/OpenAPI for documentation
- Docker containerization support

---

## 🎯 Core Features

### API Capabilities
- 📝 Create tasks with custom descriptions
- 📋 Retrieve complete task lists with smart sorting
- ✏️ Modify task details and completion states
- 🗑️ Remove tasks permanently
- ✅ Built-in request validation
- 🔒 Thread-safe concurrent operations

### Developer Experience
- 🌍 CORS enabled for cross-origin access
- 📚 Interactive Swagger UI documentation
- 🐳 Docker-ready configuration
- ⚡ Lightning-fast in-memory operations
- 🔧 Configurable port binding for cloud deployment
- 🎨 Clean, minimal API design

---

## 📁 Project Organization

```
backend-root/
│
├── Program.cs                    # Main entry point & API configuration
├── appsettings.json             # Environment settings
├── Server.csproj      # Project dependencies
├── Dockerfile                    # Container build instructions
└── .dockerignore                # Docker exclusion rules
```

---

## 🔌 API Reference

### Retrieve All Tasks

**Endpoint:** `GET /api/tasks`

Returns task collection sorted by completion status, then alphabetically by description.

**Success Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "description": "First Task",
    "isCompleted": false
  },
  {
    "id": "7cb92d48-8923-4561-a4ec-5d847b98bfa2",
    "description": "Second Task",
    "isCompleted": true
  }
]
```

---

### Create New Task

**Endpoint:** `POST /api/tasks`

Adds a new task to the collection with auto-generated ID.

**Request Payload:**
```json
{
  "description": "Implement user authentication"
}
```

**Success Response:** `201 Created`
```json
{
  "id": "9ef83a56-1234-5678-9abc-def012345678",
  "description": "Implement user authentication",
  "isCompleted": false
}
```

**Validation:**
- Description is required
- Description cannot be empty or whitespace

---

### Modify Existing Task

**Endpoint:** `PUT /api/tasks/{id}`

Updates task description and/or completion status.

**Path Parameter:** `id` (GUID) - Task identifier

**Request Payload:**
```json
{
  "description": "Updated task description",
  "isCompleted": true
}
```

**Success Response:** `200 OK`
```json
{
  "id": "9ef83a56-1234-5678-9abc-def012345678",
  "description": "Updated task description",
  "isCompleted": true
}
```

**Error Response:** `404 Not Found` - Task ID doesn't exist

---

### Remove Task

**Endpoint:** `DELETE /api/tasks/{id}`

Permanently deletes a task from the collection.

**Path Parameter:** `id` (GUID) - Task identifier

**Success Response:** `204 No Content`

**Error Response:** `404 Not Found` - Task ID doesn't exist

---

## 🚀 Getting Started

### Prerequisites

Ensure you have the following installed:
- **.NET 8 SDK** ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Visual Studio 2022** / VS Code / JetBrains Rider
- **Git** for version control
- **Docker Desktop** (optional, for containerization)

### Installation Steps

**Step 1 - Clone Repository:**
```bash
git clone <your-repository-url>
cd <backend-directory>
```

**Step 2 - Restore Dependencies:**
```bash
dotnet restore
```

**Step 3 - Build Project:**
```bash
dotnet build
```

**Step 4 - Verify Installation:**
```bash
dotnet run
```

Navigate to `http://localhost:8080/swagger` to access the API documentation.

---

## 💻 Running the Service

### Local Development

**Standard Mode:**
```bash
dotnet run
```

Access points:
- **API Base:** `http://localhost:8080`
- **Swagger UI:** `http://localhost:8080/swagger`
- **API Docs:** `http://localhost:8080/swagger/v1/swagger.json`

**Production Configuration:**
```bash
dotnet run --configuration Release
```

**With Environment Variables:**
```bash
dotnet run --environment Production
```

---

### IDE Execution

**Visual Studio:**
1. Open solution file `Server.sln`
2. Press `F5` for debug mode
3. Swagger UI launches automatically

**VS Code:**
1. Open project folder
2. Press `F5` (with C# extension installed)
3. Select `.NET Core Launch (web)` configuration

**Rider:**
1. Open project
2. Click run configuration dropdown
3. Select project and click play button

---

### Docker Deployment

**Build Container Image:**
```bash
docker build -t task-manager-api .
```

**Run Container:**
```bash
docker run -d -p 8080:8080 --name task-api task-manager-api
```

**Access API:**
- Local: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

**Container Management:**
```bash
# View logs
docker logs task-api

# Stop container
docker stop task-api

# Remove container
docker rm task-api
```

---

## 🔧 Configuration

### Port Configuration

Default port: `8080`

**Change port via environment variable:**
```bash
# Windows
set PORT=5000
dotnet run

# Linux/Mac
export PORT=5000
dotnet run
```

**Change in appsettings.json:**
```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      }
    }
  }
}
```

---

## 📊 Data Models

### Task Entity

```csharp
public class Task
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
```

### Request Models

**CreateTaskRequest:**
```csharp
public record CreateTaskRequest(string Description);
```

**UpdateTaskRequest:**
```csharp
public record UpdateTaskRequest(string Description, bool IsCompleted);
```

---

## 🔒 CORS Configuration

The API accepts requests from any origin by default for development purposes.

**Current Policy:**
- All origins allowed
- All methods allowed (GET, POST, PUT, DELETE)
- All headers allowed

**Production Recommendation:** Configure specific origins in `Program.cs`

---

## 🧪 Testing the API

### Using Swagger UI
1. Navigate to `http://localhost:8080/swagger`
2. Select an endpoint
3. Click "Try it out"
4. Enter parameters/body
5. Click "Execute"

### Using cURL

**Create Task:**
```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"description":"Test task"}'
```

**Get All Tasks:**
```bash
curl http://localhost:8080/api/tasks
```

**Update Task:**
```bash
curl -X PUT http://localhost:8080/api/tasks/{id} \
  -H "Content-Type: application/json" \
  -d '{"description":"Updated","isCompleted":true}'
```

**Delete Task:**
```bash
curl -X DELETE http://localhost:8080/api/tasks/{id}
```

---

## 👨‍💻 Developer

**Himanshu Sabharwal**

📧 2003hims@gmail.com  
🐙 [@himanshusab12](https://github.com/himanshusab12)

---

## 📝 License

This project is available for educational and personal use.

---

## 📌 Important Notes

- **Data Persistence:** Tasks are stored in-memory and will be lost on application restart
- **Scalability:** For production use, consider implementing database storage
- **Security:** Add authentication/authorization for production deployments
- **Logging:** Consider adding structured logging for monitoring