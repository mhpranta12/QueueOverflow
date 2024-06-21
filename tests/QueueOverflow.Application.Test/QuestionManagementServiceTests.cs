using Autofac.Extras.Moq;
using Moq;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Exceptions;
using QueueOverflow.Domain.Repositories;
using Shouldly;
using System.Diagnostics;

namespace QueueOverflow.Application.Test
{
	public class QuestionManagementServiceTests
	{
		private AutoMock _mock;
		private Mock<IQuestionRepository> _questionRepositoryMock;
		private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
		private QuestionManagementService _questionManagementService;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			_mock = AutoMock.GetLoose();
		}

		[SetUp]
		public void Setup()
		{
			_questionRepositoryMock = _mock.Mock<IQuestionRepository>();
			_unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
			_questionManagementService = _mock.Create<QuestionManagementService>();
		}

		[TearDown]
		public void TearDown()
		{
			_questionRepositoryMock?.Reset();
			_unitOfWorkMock?.Reset();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_mock?.Dispose();
		}

		[Test]
		public async Task CreateQuestionAsync_TitleUnique_CreatesQuestion()
		{
			//Arrange 
			const string title = "why it is like this?";
			const string description = "check";
			const string tags = "c c# c++";
			const string username = "pranta";
			uint like = 0;
			uint dislike = 0;
			DateTime creationTime = DateTime.Now;
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();

			List<Tag> TagsObjects = new List<Tag>();
			string[] splittedTags = tags.Split();
			foreach (var tag in splittedTags)
			{
				if (!string.IsNullOrWhiteSpace(tag))
				{
					Tag tagObject = new Tag()
					{
						Id = Guid.NewGuid(),
						Name = tag,
						QuestionId = questionId,
					};
					TagsObjects.Add(tagObject);
				}
			}

			var question = new Question()
			{
				Id = questionId,
				Title = title,
				Description = description,
				CreationTime = creationTime,
				Like = like,
				Dislike = dislike,
				Tags = TagsObjects,
				UserName = username,
				UserId = userId
			};

			_unitOfWorkMock.SetupGet(x => x.QuestionRepository).Returns(_questionRepositoryMock.Object).Verifiable();
			_questionRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(title, null)).ReturnsAsync(false).Verifiable();
			_questionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Question>()))
			.Returns(Task.CompletedTask).Verifiable();

			_unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

			//Act 
			await _questionManagementService.CreateQuestionAsync(title, description, tags,
				username, userId);

			//Assert
			this.ShouldSatisfyAllConditions(
				() => _questionRepositoryMock.VerifyAll(),
				() => _unitOfWorkMock.VerifyAll()
				);

		}
		[Test]
		public async Task CreateQuestionAsync_TitleDuplicate_ThrowException()
		{
			//Arrange 
			const string title = "why it is like this?";
			const string description = "check";
			const string tags = "c c# c++";
			const string username = "pranta";
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();

			List<Tag> TagsObjects = new List<Tag>();
			string[] splittedTags = tags.Split();
			foreach (var tag in splittedTags)
			{
				if (!string.IsNullOrWhiteSpace(tag))
				{
					Tag tagObject = new Tag()
					{
						Id = Guid.NewGuid(),
						Name = tag,
						QuestionId = questionId,
					};
					TagsObjects.Add(tagObject);
				}
			}
			var question = new Question()
			{
				Id = questionId,
				Title = title,
				Description = description,
				Tags = TagsObjects,
				UserName = username,
				UserId = userId
			};

			_unitOfWorkMock.SetupGet(x => x.QuestionRepository).Returns(_questionRepositoryMock.Object).Verifiable();
			_questionRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(title, null)).ReturnsAsync(true).Verifiable();

			//Act & Assert
			await Should.ThrowAsync<DuplicateQuestionTitleException>(async ()
				=> _questionManagementService.CreateQuestionAsync(title, description, tags, username, userId));
		}
		
		[Test]
		public async Task CreateQuestionAsync_TagExceeeds_ThrowException()
		{
			//Arrange 
			const string title = "why it is like this?";
			const string description = "check";
			const string tags = "c c# c++ php";
			const string username = "pranta";
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();

			List<Tag> TagsObjects = new List<Tag>();
			string[] splittedTags = tags.Split();
			foreach (var tag in splittedTags)
			{
				if (!string.IsNullOrWhiteSpace(tag))
				{
					Tag tagObject = new Tag()
					{
						Id = Guid.NewGuid(),
						Name = tag,
						QuestionId = questionId,
					};
					TagsObjects.Add(tagObject);
				}
			}
			var question = new Question()
			{
				Id = questionId,
				Title = title,
				Description = description,
				Tags = TagsObjects,
				UserName = username,
				UserId = userId
			};
			_unitOfWorkMock.SetupGet(x => x.QuestionRepository).Returns(_questionRepositoryMock.Object).Verifiable();

			//Act & Assert
			await Should.ThrowAsync<TagExceedsException>(async ()
				=> _questionManagementService.CreateQuestionAsync(title, description, tags, username, userId));
		}
	}
}