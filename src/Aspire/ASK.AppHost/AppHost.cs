var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis");

var db = builder.AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("ASK");

var api = builder.AddProject<Projects.ASK_Api>("api");
//.WaitFor(db);
//.WithHttpHealthCheck("/health");

//builder.AddProject<Projects.ASK_BlazorWebApp>("blazor")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(cache)
//    .WaitFor(cache)
//    .WithReference(api)
//    .WaitFor(api);

using var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
