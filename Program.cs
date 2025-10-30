using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TaskStorage>();

// Configure JSON serialization to use camelCase
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

var tasks = app.MapGroup("/api/tasks");

// GET /api/tasks
tasks.MapGet("/", (TaskStorage storage) =>
{
    return Results.Ok(storage.GetAll());
});

// POST /api/tasks
tasks.MapPost("/", ([FromBody] CreateTaskRequest request, TaskStorage storage) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new { error = "Title is required" });
    }

    var task = new TaskItem
    {
        Id = Guid.NewGuid(),
        Title = request.Title.Trim(),
        Description = request.Description?.Trim() ?? string.Empty,
        IsCompleted = false
    };

    storage.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

// PUT /api/tasks/{id}
tasks.MapPut("/{id:guid}", (Guid id, [FromBody] UpdateTaskRequest request, TaskStorage storage) =>
{
    var task = storage.Get(id);
    if (task == null)
    {
        return Results.NotFound(new { error = "Task not found" });
    }

    if (request.Title != null)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest(new { error = "Title cannot be empty" });
        }
        task.Title = request.Title.Trim();
    }

    if (request.Description != null)
    {
        task.Description = request.Description.Trim();
    }

    if (request.IsCompleted.HasValue)
    {
        task.IsCompleted = request.IsCompleted.Value;
    }

    return Results.Ok(task);
});

// DELETE /api/tasks/{id}
tasks.MapDelete("/{id:guid}", (Guid id, TaskStorage storage) =>
{
    var deleted = storage.Delete(id);
    if (!deleted)
    {
        return Results.NotFound(new { error = "Task not found" });
    }

    return Results.NoContent();
});

app.Run();

// Models
public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

public record CreateTaskRequest(string Title, string? Description);
public record UpdateTaskRequest(string? Title, string? Description, bool? IsCompleted);

// In-memory storage
public class TaskStorage
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

    public IEnumerable<TaskItem> GetAll() => _tasks.Values.OrderBy(t => t.IsCompleted).ThenBy(t => t.Title);

    public TaskItem? Get(Guid id) => _tasks.TryGetValue(id, out var task) ? task : null;

    public void Add(TaskItem task) => _tasks.TryAdd(task.Id, task);

    public bool Delete(Guid id) => _tasks.TryRemove(id, out _);
}