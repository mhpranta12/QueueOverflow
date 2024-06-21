using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Model;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using QueueOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Authorization;

namespace QueueOverflow.Web.Areas.user.Controllers
{
	[Area("user")]
	public class ProfilePicController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private ILogger<ProfilePicController> _logger;	
		public ProfilePicController(UserManager<ApplicationUser> userManager,
			ILogger<ProfilePicController> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View();
		}
        [Authorize]
        public IActionResult UploadPic()
		{
			return View();
		}
        [Authorize]
        [HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> UploadPic(IFormFile file)
		{
			
			if((file.ContentType =="image/jpeg" 
				||file.ContentType == "image/png"
				|| file.ContentType == "image/jpg")
				&& (file.Length < 2000000))
            {
				try
				{
					string accessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
					string secretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
					if (accessKeyId is null && secretAccessKey is null)
					{
						_logger.LogInformation("AWS Credentials was not found. Set it on env variables of system to run.");
						throw new NullReferenceException("AWS Credentials was not found. Set it on env variables of system to run.");
					}
					var s3Client = new AmazonS3Client(accessKeyId, secretAccessKey);
					using (var memoryStream = new MemoryStream())
					{
						file.CopyTo(memoryStream);
						Guid userId = GetCurrentUserId();
						var request = new TransferUtilityUploadRequest()
						{
							InputStream = memoryStream,
							Key = $"pp_{userId}",
							BucketName = "mhpranta-bucket-b9",
							ContentType = file.ContentType
						};
						var transferUtility = new TransferUtility(s3Client);
						await transferUtility.UploadAsync(request);
					}
					ViewBag.SuccessMessage = "Profile Picture updated !";
					return View();
				}
				catch (Exception ex)
				{
					_logger.LogError("An error occured, regarding : " + ex.ToString());
					ViewBag.Message = "An internal error occured";
				}
			}
			else
			{
				ViewBag.Message = "invalid file format/ size is greater than 2 MB";
				return View();
			}
			return View();
		}
		public async Task<GetObjectResponse> GetPicAsync(Guid userId)
		{
			string accessKeyId = Environment.GetEnvironmentVariable("aws_access_key_id");
			string secretAccessKey = Environment.GetEnvironmentVariable("aws_secret_access_key");
			if(accessKeyId  == null || secretAccessKey == null)
			{
				throw new NullReferenceException("Can't read AWS credentials");
			}
			var s3Client = new AmazonS3Client(accessKeyId, secretAccessKey);
			var getObjectRequest = new GetObjectRequest()
			{
				BucketName = "mhpranta-bucket-b9",
				Key = $"pp_{userId}",
			};
			return await s3Client.GetObjectAsync(getObjectRequest);
		}
        public async Task<IActionResult> GetIamge(Guid Id)
		{
			var response = await GetPicAsync(Id);
			return File(response.ResponseStream, response.Headers.ContentType);
		}
		public Guid GetCurrentUserId()
		{
			return Guid.Parse(_userManager.GetUserId(HttpContext.User));
		}
	}	
}
