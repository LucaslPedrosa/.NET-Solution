using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
namespace Application.Tests;

public class JokeServiceTests
{
    [Fact]
    public async Task GetByExternalIdAsync_ReturnsJoke_WhenExists()
    {
        // Arrange
        var externalId = 123;
        var joke = new Joke
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Setup = "Test setup",
            Punchline = "Test punchline",
            Type = "general",
            CreatedAt = DateTime.UtcNow
        };

        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.GetByExternalIdAsync(externalId))
                .ReturnsAsync(joke);

        var service = new JokeService(repoMock.Object);

        // Act
        var result = await service.GetByExternalIdAsync(externalId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(joke.ExternalId, result.ExternalId);
        Assert.Equal(joke.Setup, result.Setup);
    }

    [Fact]
    public async Task GetByExternalIdAsync_ThrowsNotFound_WhenNotExists()
    {
        // Arrange
        var externalId = 999;
        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.GetByExternalIdAsync(externalId))
                .ReturnsAsync((Joke?)null);

        var service = new JokeService(repoMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetByExternalIdAsync(externalId));

        Assert.Contains(externalId.ToString(), ex.Message);
    }

    [Fact]
    public async Task GetByExternalIdAsync_ShouldReturnCorrectJoke_WhenExists()
    {
        // Arrange
        var externalId = 101;
        var joke = new Joke
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Type = "tech",
            Setup = "Why don’t programmers like nature?",
            Punchline = "Too many bugs.",
            CreatedAt = DateTime.UtcNow
        };

        var mockRepo = new Mock<IJokeRepository>();
        mockRepo.Setup(repo => repo.GetByExternalIdAsync(externalId)).ReturnsAsync(joke);
        var service = new JokeService(mockRepo.Object);

        // Act
        var result = await service.GetByExternalIdAsync(externalId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(joke.ExternalId, result.ExternalId);
        Assert.Equal(joke.Setup, result.Setup);
    }
    [Fact]
    public async Task GetByIdAsync_ReturnsJoke_WhenExists()
    {
        // Arrange
        var jokeId = Guid.NewGuid();
        var joke = new Joke
        {
            Id = jokeId,
            ExternalId = 42,
            Setup = "Setup text",
            Punchline = "Punchline text",
            Type = "general",
            CreatedAt = DateTime.UtcNow
        };

        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.GetByIdAsync(jokeId))
                .ReturnsAsync(joke);

        var service = new JokeService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(jokeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jokeId, result.Id);
        Assert.Equal(joke.Setup, result.Setup);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenNotExists()
    {
        // Arrange
        var jokeId = Guid.NewGuid();
        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.GetByIdAsync(jokeId))
                .ReturnsAsync((Joke?)null);

        var service = new JokeService(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetByIdAsync(jokeId));
    }

    [Fact]
    public async Task UpsertAsync_AddsJoke_WhenNotExists()
    {
        // Arrange
        var newJoke = new Joke
        {
            Id = Guid.NewGuid(),
            ExternalId = 999,
            Setup = "New setup",
            Punchline = "New punchline",
            Type = "new-type",
            CreatedAt = DateTime.UtcNow
        };

        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.UpsertAsync(It.IsAny<Joke>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

        var service = new JokeService(repoMock.Object);

        // Act
        await service.UpsertAsync(newJoke.ToDto());

        // Assert
        repoMock.Verify(r => r.UpsertAsync(It.Is<Joke>(j =>
            j.Setup == newJoke.Setup &&
            j.Punchline == newJoke.Punchline &&
            j.Type == newJoke.Type)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeletesJoke_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var joke = new Joke
        {
            Id = id,
            Setup = "This is a valid setup",
            Punchline = "This is a valid punchline",
            Type = "general",
            CreatedAt = DateTime.UtcNow
        };

        var repoMock = new Mock<IJokeRepository>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(joke);
        repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();

        var service = new JokeService(repoMock.Object);

        // Act
        await service.DeleteAsync(id);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}
