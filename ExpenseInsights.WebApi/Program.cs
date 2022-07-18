using ExpenseInsights.WebApi.DbContexts;
using ExpenseInsights.WebApi.Repositories;
using ExpenseInsights.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransactionDbContext>();
// Add services to the container.
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IStatementService, StatementService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
