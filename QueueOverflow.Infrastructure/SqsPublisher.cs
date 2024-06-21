using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
	public class SqsPublisher
	{
		private readonly IAmazonSQS _sqs;
		public SqsPublisher(IAmazonSQS sqs)
		{
			_sqs = sqs;
		}
		public async Task PublishAsync(string queueName, EmailInfo emailInfo)
		{
			var queueUrl = await _sqs.GetQueueUrlAsync(queueName);
			var request = new SendMessageRequest
			{
				QueueUrl = queueUrl.QueueUrl,
				MessageBody = JsonSerializer.Serialize(emailInfo)
			};
			try
			{
				await _sqs.SendMessageAsync(request);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
