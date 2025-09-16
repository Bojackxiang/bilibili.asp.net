var builder = WebApplication.CreateBuilder(args);

// Swagger COnfig
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// 下面全部都是 routing
app.UseHttpsRedirection();


// GET: /query?id=123
app.MapGet("/query", (HttpRequest req) => $"Query id: {req.Query["id"]}");

// GET: /user/123
app.MapGet("/user/{id}", (int id) => $"Path id: {id}");

// POST: /user  (with JSON body)
app.MapPost("/user", async (HttpRequest req) =>
{
  var user = await req.ReadFromJsonAsync<UserDto>();
  return user is not null ? $"Received user: {user.Name}, {user.Age}" : "Invalid body";
});

app.MapGet("/", () => "Hello world");

app.MapFallback(() => Results.NotFound("Custom 404: Endpoint not found"));


app.Run();

public record UserDto(string Name, int Age);
