var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");

var api = builder.AddProject<Projects.GemNote_API>("gemnote-api")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.GemNote_Web>("gemnote-web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
