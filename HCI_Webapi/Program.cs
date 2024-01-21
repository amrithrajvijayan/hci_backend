using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Default")!);

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


app.MapGet("/patientVisits", async ([Microsoft.AspNetCore.Mvc.FromServices] MySqlDataSource database, string searchType, string searchValue) =>
{
    using var connection = await database.OpenConnectionAsync();
    using var command = connection.CreateCommand();

    if (searchType.Equals("name"))
    {

        command.CommandText = @"select patient_id, name, visit_date from Patient, Patient_Visits where Patient.id = Patient_Visits.patient_id and name = @name";
        command.Parameters.AddWithValue("@name", searchValue);

    }
    else if (searchType.Equals("id"))
    {

        command.CommandText = @"select patient_id, name, visit_date from Patient, Patient_Visits where Patient.id = Patient_Visits.patient_id and patient_id = @id";
        command.Parameters.AddWithValue("@id", searchValue);

    }


    var visits = await ReadAllAsyncPatientVisits(await command.ExecuteReaderAsync());


    return visits.ToArray();
})
.WithName("GetPatientVisits")
.WithOpenApi();


app.Run();


async Task<IReadOnlyList<PatientVisit>> ReadAllAsyncPatientVisits(System.Data.Common.DbDataReader reader)
{
    var visits = new List<PatientVisit>();
    using (reader)
    {
        while (await reader.ReadAsync())
        {
            var visit = new PatientVisit
            {
                patient_id = reader.GetInt32(0),
                name = reader.GetString(1),
                date = reader.GetDateTime(2),
            };
            visits.Add(visit);
        }
    }
    return visits;
}


public class PatientVisit
{
    public int patient_id { get; set; }
    public string? name { get; set; }

    public DateTime? date { get; set; }
}
