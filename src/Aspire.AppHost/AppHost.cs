using Aspire.Hosting.Azure;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<AzureSqlDatabaseResource> azureSql = builder
    .AddAzureSqlServer("azure-sql")
    .RunAsContainer(resourceBuilder => resourceBuilder.WithDataVolume("azure-sql-data"))
    .AddDatabase("clean-architecture-db");

builder
    .AddProject<Projects.Web_Api>("web-api")
    .WithReference(azureSql)
    .WaitFor(azureSql);

#pragma warning disable S6966
builder.Build().Run();
#pragma warning restore S6966
