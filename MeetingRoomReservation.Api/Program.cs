using FluentValidation;
using FluentValidation.AspNetCore;
using MeetingRoomReservation.Api.Common;
using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Middlewares;
using MeetingRoomReservation.Api.Services;
using MeetingRoomReservation.Api.Services.Interfaces;
using MeetingRoomReservation.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

#region SERILOG

var columnOptions = new ColumnOptions();


columnOptions.Store.Remove(StandardColumn.Properties);

// JSON formatlý log saklamak için LogEvent kolonunu ekliyoruz
columnOptions.Store.Add(StandardColumn.LogEvent);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: columnOptions)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region SERVICES

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPublicHolidayService, PublicHolidayService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();


builder.Services.AddValidatorsFromAssemblyContaining<CreateRoomDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        var response = ApiResponse.Error("Validation hatasý", errors);

        return new BadRequestObjectResult(response);
    };
});

#endregion

var app = builder.Build();

#region PIPELINE

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

#endregion

app.Run();
