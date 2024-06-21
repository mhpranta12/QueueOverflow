using Autofac.Extras.Moq;
using Moq;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Exceptions;
using QueueOverflow.Domain.Repositories;
using Shouldly;

namespace QueueOverflow.Application.Test
{
	public class ReplyManagementServiceTests
	{
		private AutoMock _mock;
		private Mock<IReplyRepository> _replyRepositoryMock;
		private Mock<IUserInfoRepository> _userInfoRepositoryMock;
		private UserInfo _userInfo;
		private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
		private ReplyManagementService _replyManagementService;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			_mock = AutoMock.GetLoose();
		}

		[SetUp]
		public void Setup()
		{
			_replyRepositoryMock = _mock.Mock<IReplyRepository>();
			_userInfoRepositoryMock = _mock.Mock<IUserInfoRepository>();
			_unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
			_replyManagementService = _mock.Create<ReplyManagementService>();
			_userInfo = _mock.Create<UserInfo>();
		}

		[TearDown]
		public void TearDown()
		{
			_replyRepositoryMock?.Reset();
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
			Guid Id = Guid.NewGuid();
			const string text = "This is a reply";
			const string userName = "MH Pranta";
			Guid userId = Guid.NewGuid();
			Guid questionId = Guid.NewGuid();

			_unitOfWorkMock.SetupGet(x => x.ReplyRepository).Returns(_replyRepositoryMock.Object);
			_unitOfWorkMock.SetupGet(x => x.UserInfoRepository).Returns(_userInfoRepositoryMock.Object);

			_userInfoRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(_userInfo);
			_replyRepositoryMock.Setup(x => x.AddAsync(It.Is<Reply>(
				c => c.Text == text && c.UserName == userName 
				&& c.QuestionId == questionId && c.UserId == userId)))
				.Returns(Task.CompletedTask);
			_unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

			// Act
			await _replyManagementService.CreateReplyAsync(text, userName,  questionId, userId);

			// Assert
			this.ShouldSatisfyAllConditions(
					() => _replyRepositoryMock.VerifyAll(),
					() => _unitOfWorkMock.VerifyAll()
					);
		}
	}
}