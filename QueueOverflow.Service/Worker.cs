using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using QueueOverflow.Application.Utilities;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace QueueOverflow.Service
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private IEmailService _emailService;

		public Worker(ILogger<Worker> logger, IEmailService emailService)
		{
			_logger = logger;
			_emailService = emailService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			//Getting required key from system env
			string accessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
			string secretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
			//If by chance null then, throw exception
			if(accessKeyId is null && secretAccessKey is null)
			{
				_logger.LogInformation("AWS Credentials was not found. Set it on env variables of system to run.");
				throw new NullReferenceException("AWS Credentials was not found. Set it on env variables of system to run.");
			}

			//building basic credentials
			var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);

			var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);
			//getting url of the sqs
			var queueUrl = await sqsClient.GetQueueUrlAsync("pranta_sqs");
			var recieveRequest = new ReceiveMessageRequest()
			{
				QueueUrl = queueUrl.QueueUrl,
				MessageAttributeNames = new List<string>() { "All" },
				AttributeNames = new List<string>() { "All" },
			};
			//while service will run, send the confirmation mail to the email 
			while (!stoppingToken.IsCancellationRequested)
			{
				//getting the response from sqs end
				var response = await sqsClient.ReceiveMessageAsync(recieveRequest,stoppingToken);
				//if status code is OK then, execute 
				if(response.HttpStatusCode == HttpStatusCode.OK)
				{
                    foreach (var message in response.Messages)
                    {
						dynamic msgData = JsonConvert.DeserializeObject(message.Body);
						string userId = msgData.Id;
						string email = msgData.EmailOfUser;
						//Confirmation URL Email Sending Part
						var confirmationUrl = $"https://localhost:7037/Account/ConfirmEmail/{userId}";

						try
						{
							_emailService.SendSingleEmail("QueueOverflow User", email, "Confirm your email",
													   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationUrl)}'>clicking here</a> (Mail was sent through worker service from sqs)");
						}
						catch (Exception ex)
						{
							_logger.LogError("A problem occured in worker service regarding sending mail :" + ex);
						}
					}
                }
				//task will be run after each 1 minute gaps
				await Task.Delay(60000, stoppingToken);
			}
		}
	}
}
