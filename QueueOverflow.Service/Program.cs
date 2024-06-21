using QueueOverflow.Service;
using QueueOverflow.Infrastructure.Email;
using QueueOverflow.Application.Utilities;


try
{
	IHost host = Host.CreateDefaultBuilder(args)
	.UseWindowsService()
	.ConfigureServices(services =>
	{
		var config = services.BuildServiceProvider().
			GetRequiredService<IConfiguration>().GetSection("Smtp");

		services.Configure<Smtp>(config);
		services.AddSingleton<IEmailService, HtmlEmailService>();
		services.AddHostedService<Worker>();

	})
	.Build();

	host.Run();
}
catch (Exception ex)
{

}

