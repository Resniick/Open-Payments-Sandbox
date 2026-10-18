using OpenPayments.Sandbox.Shared;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres(ResourceNames.Postgres)
	.WithDataVolume()
	.WithImageTag("18")
	.WithHostPort(5432)
	.WithLifetime(ContainerLifetime.Persistent)
	.WithPgAdmin(x => x
		.WithLifetime(ContainerLifetime.Persistent)
	);

var db = postgres.AddDatabase(ResourceNames.Database);

var api = builder.AddProject<Projects.OpenPayments_Sandbox_API>(ResourceNames.API)
	.WithReference(db)
	.WaitFor(db)
	.WithHttpHealthCheck("/health");

builder.Build().Run();
