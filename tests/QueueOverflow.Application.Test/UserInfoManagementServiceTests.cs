using Autofac.Extras.Moq;
using Moq;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Exceptions;
using QueueOverflow.Domain.Repositories;
using Shouldly;

namespace QueueOverflow.Application.Test
{
	public class UserInfoManagementServiceTests
	{
		private AutoMock _mock;
		private Mock<IUserInfoRepository> _userInfoRepositoryMock;
		private UserInfo _userInfo;
		private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
		private UserInfoManagementService _userManagementService;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			_mock = AutoMock.GetLoose();
		}

		[SetUp]
		public void Setup()
		{
			_userInfoRepositoryMock = _mock.Mock<IUserInfoRepository>();
			_unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
			_userManagementService = _mock.Create<UserInfoManagementService>();
			_userInfo = _mock.Create<UserInfo>();
		}

		[TearDown]
		public void TearDown()
		{
			_userInfoRepositoryMock?.Reset();
			_unitOfWorkMock?.Reset();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_mock?.Dispose();
		}

		[Test]
		public async Task CreateUserInfoAsync_CreatesUser()
		{
			// Arrange
			Guid userId = Guid.NewGuid();
			string? name = "pranta";
			string? designation = "jse";
			string? about = "adfaf";
			string? portfolioLink = "www";
			string? address = "munshignaj";
			string? gitHubLink = "sasa";
			string? badge = "dsd";


			_unitOfWorkMock.SetupGet(x => x.UserInfoRepository).Returns(_userInfoRepositoryMock.Object);

			_userInfoRepositoryMock.Setup(x => x.AddAsync(It.IsAny<UserInfo>()))	
				.Returns(Task.CompletedTask);
			_unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

			// Act
			await _userManagementService.CreateUserInfoAsync(userId,name,designation,about,portfolioLink,
				address);

			// Assert
			this.ShouldSatisfyAllConditions(
					() => _userInfoRepositoryMock.VerifyAll(),
					() => _unitOfWorkMock.VerifyAll()
					);
		}public async Task EditUserInfoAsync_UniqueName_EditUser()
		{
			// Arrange
			Guid userId = Guid.NewGuid();
			string name = "rahat";
			string? newName = "pranta";
			string? designation = "jse";
			string? about = "adfaf";
			string? portfolioLink = "www";
			string? address = "munshignaj";
			string? gitHubLink = "sasa";
			string? badge = "dsd";


			_unitOfWorkMock.SetupGet(x => x.UserInfoRepository).Returns(_userInfoRepositoryMock.Object);
			_userInfoRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new UserInfo { Id = userId });
			_userInfoRepositoryMock.Setup(x => x.IsUserNameDuplicateAsync(newName, userId)).ReturnsAsync(false);
			_userInfoRepositoryMock.Setup(x => x.EditAsync(It.IsAny<UserInfo>()))	
				.Returns(Task.CompletedTask);
			_unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

			// Act
			await _userManagementService.EditUserInfoAsync(userId,name,designation,about,portfolioLink,
				address);

			// Assert
			this.ShouldSatisfyAllConditions(
					() => _userInfoRepositoryMock.VerifyAll(),
					() => _unitOfWorkMock.VerifyAll()
					);
		}
	}
}