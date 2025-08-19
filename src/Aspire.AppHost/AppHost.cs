IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Web_Api>("web-api");

await builder.Build().RunAsync();
