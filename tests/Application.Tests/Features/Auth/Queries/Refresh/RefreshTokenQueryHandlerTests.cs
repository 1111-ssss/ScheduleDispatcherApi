using Application.Abstractions.Interfaces.Auth;
using Application.Abstractions.Model.DTO;
using Application.Features.Auth.Refresh;
using Domain.Abstractions.Result;
using FluentAssertions;
using Infrastructure.Abstractions.Interfaces.Auth;
using Moq;

namespace Application.Tests.Features.Auth.Queries.Refresh;

public class RefreshTokenQueryHandlerTests
{
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private RefreshTokenQueryHandler CreateSut() =>
        new RefreshTokenQueryHandler(
            _jwtGeneratorMock.Object,
            _currentUserServiceMock.Object);

    [Fact]
    public async Task Handle_NoJwtToken_ReturnsInvalidTokenError()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.JwtToken).Returns((string?)null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(new RefreshTokenQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.InvalidToken);
    }

    [Fact]
    public async Task Handle_TokenExpired_ReturnsTokenExpiredError()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.JwtToken).Returns("some.jwt.token");
        _currentUserServiceMock.Setup(x => x.ExpiresAt).Returns(DateTime.UtcNow.AddMinutes(-5));

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(new RefreshTokenQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.TokenExpired);
    }

    [Fact]
    public async Task Handle_TokenDtoIsNull_ReturnsInvalidTokenError()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.JwtToken).Returns("valid.jwt");
        _currentUserServiceMock.Setup(x => x.ExpiresAt).Returns(DateTime.UtcNow.AddHours(1));
        _currentUserServiceMock.Setup(x => x.TokenDTO).Returns((GenerateTokenDTO?)null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(new RefreshTokenQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.InvalidToken);
    }

    [Fact]
    public async Task Handle_GenerateTokenFailed_ReturnsTokenGenerationError()
    {
        // Arrange
        var tokenDto = new GenerateTokenDTO(123, "test", "Admin");

        _currentUserServiceMock.Setup(x => x.JwtToken).Returns("old.valid.token");
        _currentUserServiceMock.Setup(x => x.ExpiresAt).Returns(DateTime.UtcNow.AddHours(2));
        _currentUserServiceMock.Setup(x => x.TokenDTO).Returns(tokenDto);

        _jwtGeneratorMock.Setup(x => x.GenerateToken(tokenDto))
            .Returns(string.Empty);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(new RefreshTokenQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.TokenGenerationError);
    }

    [Fact]
    public async Task Handle_ValidCase_ReturnsNewJwtToken()
    {
        // Arrange
        const string NEW_TOKEN = "new.jwt.token";
        
        var tokenDto = new GenerateTokenDTO(123, "test", "Admin");

        _currentUserServiceMock.Setup(x => x.JwtToken).Returns("old.jwt.token");
        _currentUserServiceMock.Setup(x => x.ExpiresAt).Returns(DateTime.UtcNow.AddHours(3));
        _currentUserServiceMock.Setup(x => x.TokenDTO).Returns(tokenDto);

        _jwtGeneratorMock.Setup(x => x.GenerateToken(tokenDto))
            .Returns(NEW_TOKEN);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(new RefreshTokenQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.JwtToken.Should().Be(NEW_TOKEN);
    }
}