using Autofac.Extras.Moq;
using Moq;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Application;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Repositories;
using Shouldly;
namespace QueueOverflow.Application.Test
{
	public class CommentManagementServiceTests
	{
		private AutoMock _mock;
		private Mock<ICommentRepository> _commentRepositoryMock;
		private Mock<IUserInfoRepository> _userInfoRepositoryMock;
		private UserInfo _userInfo;
		private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
		private CommentManagementService _commentManagementService;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			_mock = AutoMock.GetLoose();
		}

		[SetUp]
		public void Setup()
		{
			_commentRepositoryMock = _mock.Mock<ICommentRepository>();
			_userInfoRepositoryMock = _mock.Mock<IUserInfoRepository>();
			_unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
			_commentManagementService = _mock.Create<CommentManagementService>();
			_userInfo = _mock.Create<UserInfo>();
		}

		[TearDown]
		public void TearDown()
		{
			_commentRepositoryMock?.Reset();
			_unitOfWorkMock?.Reset();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_mock?.Dispose();
		}

		[Test]
		public async Task CreateCommentAsync_UserInfoAndEnoughPoints_CreatesComment()
		{
			// Arrange
			const string text = "This is a comment";
			const string userName = "MH Pranta";
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();
			_userInfo.Reputations = 60; // Enough points

			_unitOfWorkMock.SetupGet(x => x.CommentRepository).Returns(_commentRepositoryMock.Object);
			_unitOfWorkMock.SetupGet(x => x.UserInfoRepository).Returns(_userInfoRepositoryMock.Object);

			_userInfoRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(_userInfo);
			_commentRepositoryMock.Setup(x => x.AddAsync(It.Is<Comment>(
				c => c.Text == text && c.UserName == userName && c.QuestionId == questionId)))
				.Returns(Task.CompletedTask);
			_unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

			// Act
			await _commentManagementService.CreateCommentAsync(text, userId, questionId, userName);

			// Assert
			this.ShouldSatisfyAllConditions(
					() => _commentRepositoryMock.VerifyAll(),
					() => _unitOfWorkMock.VerifyAll()
					);
		}

		[Test]
		public async Task CreateCommentAsync_UserInfoButNotEnoughPoints_DoesNotCreateComment()
		{
			// Arrange
			const string text = "This is a comment text";
			const string userName = "Pranta";
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();
			_userInfo.Reputations = 20; // Not enough points


			_unitOfWorkMock.SetupGet(x => x.CommentRepository).Returns(_commentRepositoryMock.Object).Verifiable();
			_unitOfWorkMock.SetupGet(x => x.UserInfoRepository).Returns(_userInfoRepositoryMock.Object);
			_userInfoRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(_userInfo);

			
			// Act
			await _commentManagementService.CreateCommentAsync(text, userId, questionId, userName);

			// Assert
			this.ShouldSatisfyAllConditions(
					() => _commentRepositoryMock.VerifyAll(),
					() => _unitOfWorkMock.VerifyAll()
					);
		}
	}
}