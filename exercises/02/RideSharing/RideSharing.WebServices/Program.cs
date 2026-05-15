using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.Queue.Common.Configuration;
using RideSharing.Contracts;
using RideSharing.Data;
using RideSharing.WebServices;
using RideSharing.WebServices.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddQueueTransport();

builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddTransient<RideBookingService>();

var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RideSharingDbContext>();
    db.Database.EnsureCreated();
}

app.UseServiceModel(serviceBuilder =>
{
    // Existing HTTP service
    serviceBuilder.AddService<Service>();
    serviceBuilder.AddServiceEndpoint<Service, IService>(
        new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");

    // RabbitMQ ride booking service
    serviceBuilder.AddService<RideBookingService>();
    serviceBuilder.AddServiceEndpoint<RideBookingService, IRideBookingService>(
        new RabbitMqBinding(),
        new Uri("net.amqp://localhost/ride_booking_exchange/ride_booking_queue"));

    // HTTP endpoint for request-reply ride status queries
    serviceBuilder.AddServiceEndpoint<RideBookingService, IRideStatusService>(
        new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/RideStatusService.svc");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
