var builder = DistributedApplication.CreateBuilder(args);

//var redis = builder.AddRedis("redis");
var sql = builder.AddSqlServer("SqlServer")
    .PublishAsConnectionString()
    .AddDatabase("AspireOrleansDb");

var sqlConnectionoString = builder.AddParameter("SqlConnectionStrings");
//var messaging = builder.AddRabbitMQ("messaging");

var secret = builder.AddParameter("JwtSecret");
var iss = builder.AddParameter("JwtIss");
var aud = builder.AddParameter("JwtAud");

//var serveEmail = builder.AddParameter("ServeEmail");
//var smtpServer = builder.AddParameter("SMTPServer");
//var smtpPort = builder.AddParameter("SMTPPort");
//var authorizationCode = builder.AddParameter("AuthorizationCode");

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var clusteringTable = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grain-state");
//var orleans = builder.AddOrleans("default")
//                     .WithDevelopmentClustering()
//                     .WithMemoryGrainStorage("Default");

var orleans = builder.AddOrleans("my-app")
                     .WithClustering(clusteringTable)
                     .WithGrainStorage("Default", grainStorage);

//var orleans = builder.AddOrleans("default")    
//    .WithClustering(redis)
//    .WithGrainStorage("users", redis)
//    .WithGrainStorage("products", redis)
//    .WithGrainStorage("email", redis)
//    .WithGrainStorage("order", redis)
//    .WithGrainStorage("store", redis);

builder.AddProject<Projects.Usr_Api>("usr-api")
    .WithEnvironment("SqlConnectionStrings", sqlConnectionoString)
    .WithEnvironment("JwtSecret", secret)
    .WithEnvironment("JwtIss", iss)
    .WithEnvironment("JwtAud", aud)
    .WithReference(sql)
    .WithReference(orleans)
    .WithReplicas(3);

builder.AddProject<Projects.Email_Api>("email-api")
    //.WithReference(messaging) // 消息队列
    .WithReference(orleans)
    .WithReplicas(2);

builder.AddProject<Projects.Prdt_Api>("prdt-api")
    .WithEnvironment("SqlConnectionStrings", sqlConnectionoString)
    .WithEnvironment("JwtSecret", secret)
    .WithEnvironment("JwtIss", iss)
    .WithEnvironment("JwtAud", aud)
    .WithReference(sql)
    .WithReference(orleans)
    .WithReplicas(3);

builder.AddProject<Projects.Odr_Api>("odr-api")
    .WithEnvironment("SqlConnectionStrings", sqlConnectionoString)
    .WithEnvironment("JwtSecret", secret)
    .WithEnvironment("JwtIss", iss)
    .WithEnvironment("JwtAud", aud)
    .WithReference(sql)
    .WithReference(orleans)
    .WithReplicas(3);

builder.AddProject<Projects.Sto_Api>("sto-api")
    .WithEnvironment("SqlConnectionStrings", sqlConnectionoString)
    .WithEnvironment("JwtSecret", secret)
    .WithEnvironment("JwtIss", iss)
    .WithEnvironment("JwtAud", aud)
    .WithReference(sql)
    .WithReference(orleans)
    .WithReplicas(3);

builder.AddProject<Projects.Gateway>("gateway")
    .WithEnvironment("JwtSecret", secret)
    .WithEnvironment("JwtIss", iss)
    .WithEnvironment("JwtAud", aud)
    .WithReference(orleans.AsClient());

builder.Build().Run();
