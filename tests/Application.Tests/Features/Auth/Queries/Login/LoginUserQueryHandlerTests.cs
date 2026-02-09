using Application.Abstractions.Model.DTO;
using Application.Abstractions.Repository;
using Application.Features.Auth.Login;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using FluentAssertions;
using Infrastructure.Abstractions.Interfaces.Auth;
using Moq;

namespace Application.Tests.Features.Auth.Login;

public class LoginUserQueryHandlerTests
{
    private readonly Mock<IBaseRepository<User>> _userRepoMock = new();
    private readonly Mock<IJwtGenerator> _jwtMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

    private LoginUserQueryHandler CreateSut() =>
        new(
            _userRepoMock.Object,
            _jwtMock.Object,
            _passwordHasherMock.Object
        );

    [Fact]
    public async Task Handle_UserNotFound_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var query = new LoginUserQuery("nonexistent", "any");

        _userRepoMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.InvalidUsernameOrPassword);
    }

    [Fact]
    public async Task Handle_PasswordIncorrect_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var query = new LoginUserQuery("testuser", "wrongpass");

        var user = CreateValidUser();

        _userRepoMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.Verify("wrongpass", user.PasswordHash))
            .Returns(Result.Failed(ErrorCode.InvalidUsernameOrPassword));

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.InvalidUsernameOrPassword);
    }

    [Fact]
    public async Task Handle_JwtGenerationFailed_ReturnsTokenGenerationError()
    {
        // Arrange
        var query = new LoginUserQuery("testuser", "correct");

        var user = CreateValidUser();

        _userRepoMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.Verify("correct", user.PasswordHash))
            .Returns(Result.Success());

        _jwtMock
            .Setup(j => j.GenerateToken(It.IsAny<GenerateTokenDTO>()))
            .Returns(string.Empty);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.TokenGenerationError);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccessWithToken()
    {
        // Arrange
        var query = new LoginUserQuery("testuser", "correct");
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

        var user = CreateValidUser();

        _userRepoMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.Verify("correct", user.PasswordHash))
            .Returns(Result.Success());

        _jwtMock
            .Setup(j => j.GenerateToken(It.Is<GenerateTokenDTO>(dto =>
                dto.Id == user.Id &&
                dto.Username == user.Username)))
            .Returns(expectedToken);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.JwtToken.Should().Be(expectedToken);
    }

    // Helpers
    private static User CreateValidUser()
    {
        return new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
    }
}