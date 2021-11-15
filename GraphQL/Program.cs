using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Attendees;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Sessions;
using ConferencePlanner.GraphQL.Tracks;
using ConferencePlanner.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"));
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"));
builder.Services
   .AddGraphQLServer()
   .AddQueryType(d => d.Name("Query"))
           .AddTypeExtension<AttendeeQueries>()
      .AddTypeExtension<SpeakerQueries>()
      .AddTypeExtension<SessionQueries>()
      .AddTypeExtension<TrackQueries>()
   .AddMutationType(d => d.Name("Mutation"))
           .AddTypeExtension<AttendeeMutations>()
      .AddTypeExtension<SessionMutations>()
      .AddTypeExtension<SpeakerMutations>()
      .AddTypeExtension<TrackMutations>()
           .AddSubscriptionType(d => d.Name("Subscription"))
           .AddTypeExtension<AttendeeSubscriptions>()
           .AddTypeExtension<SessionSubscriptions>()
   .AddType<AttendeeType>()
   .AddType<SessionType>()
   .AddType<SpeakerType>()
   .AddType<TrackType>()
   .EnableRelaySupport()
   .AddFiltering()
   .AddSorting()
   .AddInMemorySubscriptions()
   .AddDataLoader<SpeakerByIdDataLoader>()
   .AddDataLoader<SessionByIdDataLoader>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();


// app.MapControllers();
app.UseWebSockets();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
