var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next(context);
    // your code that runs after the following middleware
});

app.MapGet("/patientVisits", () =>
{

    PatientVisit[] visits = {
        new("1","Test 1","1/1/2024"),
        new("2","Test 2","1/1/2024"),
        new("3","Test 3","2/1/2024"),
        new("4","Test 4","3/1/2024"),
    };

    return visits;
})
.WithName("GetPatientVisits")
.WithOpenApi();

app.Run();

record PatientVisit(string id, string name, string Date)
{
}