var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddCors();


PatientVisit[] visits = {
        new("1","Patient 1","1/1/2024"),
        new("2","Patient 2","1/1/2024"),
        new("3","Patient 3","2/1/2024"),
        new("4","Patient 4","4/1/2024"),
        new("5","Patient 5","4/1/2024"),
        new("6","Patient 6","4/1/2024"),
        new("7","Patient 7","5/1/2024"),
        new("8","Patient 8","5/1/2024"),
        new("9","Patient 9","5/1/2024"),
        new("10","Patient 10","6/1/2024"),
        new("1","Patient 1","3/1/2024"),
        new("2","Patient 2","3/1/2024"),
        new("3","Patient 3","6/1/2024"),
        new("4","Patient 4","6/1/2024"),
        new("1","Patient 1","8/1/2024"),
        new("2","Patient 2","8/1/2024"),
        new("3","Patient 3","8/1/2024"),
        new("4","Patient 4","8/1/2024"),
    };


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

app.MapGet("/patientVisits", (string searchType, string searchValue) =>
{
    List<PatientVisit> visitsToReturn = new List<PatientVisit>();

    foreach(PatientVisit c in visits)
    {
        if(searchType.Equals("name") && c.name.Equals(searchValue)) {
            visitsToReturn.Add(c);
        } else  if(searchType.Equals("id") && c.id.Equals(searchValue)) {
            visitsToReturn.Add(c);
        }else  if(searchType.Equals("date") && c.Date.Equals(searchValue)) {
            visitsToReturn.Add(c);
        }
    }

    return visitsToReturn.ToArray();    
})
.WithName("GetPatientVisits")
.WithOpenApi();

app.Run();

record PatientVisit(string id, string name, string Date)
{
}