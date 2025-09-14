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

// 自动将所有 HTTP 请求重定向到 HTTPS，提升安全性。
app.UseHttpsRedirection();

app.MapGet("/", () => "Hello world");

app.MapGet("/{id}", (string id) => $"{id}");

app.MapGet("/query", (HttpRequest req) => $"{req.Query["id"]}");


app.Run();
