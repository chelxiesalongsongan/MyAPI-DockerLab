
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disabled for the HTTP-only Docker lab.
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Health endpoint for Docker lab
app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy"
    });
});

// Get items from SQL Server
app.MapGet("/api/items", async (IConfiguration configuration) =>
{
    string connectionString =
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection is missing.");

    List<object> items = new List<object>();

    await using SqlConnection connection =
        new SqlConnection(connectionString);

    await connection.OpenAsync();

    string sql = "SELECT Id, Name FROM Items";

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    await using SqlDataReader reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        items.Add(new
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1)
        });
    }

    return Results.Ok(items);
});

app.Run();
