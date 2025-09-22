var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger COnfig
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// 自动将所有 HTTP 请求重定向到 HTTPS，提升安全性。
app.UseHttpsRedirection();



app.Run();
