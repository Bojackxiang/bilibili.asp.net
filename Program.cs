var builder = WebApplication.CreateBuilder(args);

// Swagger COnfig
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1- controllers injection
builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// 2- controllers Mapping
app.MapControllers();
app.Run();
