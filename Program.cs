using System.Reflection.Metadata.Ecma335;
using FirstProject.ConventionalMiddleware;

var builder = WebApplication.CreateBuilder(args);

// middlewares
builder.Services.AddTransient<CustomizedMiddlewaresOne>();

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
  await next(context);
});

// middleware-3
/**
* 直接使用 middleware
*/
app.UseMiddleware<CustomizedMiddlewaresOne>();

// middleware-4
// app.DoSomething();

// middleware-5
/**
  * explain： 通过 extension 使用 middleware
  * 先使用 extension
  * extension 中调用 middleware
  * 真实逻辑写在 middleware 中
*/
app.UseCustomizedMiddlewaresOneExtension();

// middleware-6 使用 conventional middleware
app.UseConventionalMiddlewareOne();

app.UseWhen(
  (context) => context.Request.Query.ContainsKey("gender"),
  app =>
  {
    app.Use(async (context, next) =>
    {
      await context.Response.WriteAsync($" {context.Request.Query["gender"]}");
      await next();
    });
  });

app.Run(async (context) =>
  {
    await context.Response.WriteAsync(" back to the main chain");
  });

app.Run();
