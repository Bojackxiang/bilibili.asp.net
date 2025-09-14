var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

// middleware-1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
  /**
  /* !! 当访问 localhost：3000, 会产产生两个 "request" ，"/favicon.ico"  and "/"
  **/
  Console.WriteLine(context.Request.Path);
  Console.WriteLine("1");
  await context.Response.WriteAsync("hello1");
  await next(context);
});

// middleware-2
app.Use(async (HttpContext context, RequestDelegate next) =>
{
  Console.WriteLine(context.Request.Path);
  Console.WriteLine("2");
  await context.Response.WriteAsync("hello2");
});

app.Run();
