using CrudRestApi.Models;
using CrudRestApi.Repositories;

namespace CrudRestApi.Tests;

public class InMemoryRepositoryTests
{
    // Helper to create a fresh repository for each test
    private InMemoryRepository<User> CreateRepository() =>
        new InMemoryRepository<User>(u => u.Id, (u, id) => u.Id = id);

    [Fact]
    public void Add_ShouldAssignIdAndStoreEntity()
    {
        // Arrange
        var repo = CreateRepository();
        var user = new User { Name = "Mario", Email = "mario@mail.com" };

        // Act
        var result = repo.Add(user);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Mario", result.Name);
    }

    [Fact]
    public void Add_MultipleEntities_ShouldIncrementIds()
    {
        // Arrange
        var repo = CreateRepository();

        // Act
        var first = repo.Add(new User { Name = "Mario", Email = "mario@mail.com" });
        var second = repo.Add(new User { Name = "Luigi", Email = "luigi@mail.com" });

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectEntity()
    {
        // Arrange
        var repo = CreateRepository();
        repo.Add(new User { Name = "Mario", Email = "mario@mail.com" });

        // Act
        var result = repo.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mario", result.Name);
    }

    [Fact]
    public void GetById_WhenNotFound_ShouldReturnNull()
    {
        // Arrange
        var repo = CreateRepository();

        // Act
        var result = repo.GetById(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var repo = CreateRepository();
        repo.Add(new User { Name = "Mario", Email = "mario@mail.com" });
        repo.Add(new User { Name = "Luigi", Email = "luigi@mail.com" });

        // Act
        var result = repo.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Update_ShouldModifyExistingEntity()
    {
        // Arrange
        var repo = CreateRepository();
        repo.Add(new User { Name = "Mario", Email = "mario@mail.com" });

        // Act
        var updated = repo.Update(1, new User { Name = "Mario Rossi", Email = "mario@mail.com" });

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Mario Rossi", updated.Name);
        Assert.Equal(1, updated.Id);
    }

    [Fact]
    public void Update_WhenNotFound_ShouldReturnNull()
    {
        // Arrange
        var repo = CreateRepository();

        // Act
        var result = repo.Update(99, new User { Name = "Mario", Email = "mario@mail.com" });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveEntity()
    {
        // Arrange
        var repo = CreateRepository();
        repo.Add(new User { Name = "Mario", Email = "mario@mail.com" });

        // Act
        var deleted = repo.Delete(1);

        // Assert
        Assert.True(deleted);
        Assert.Null(repo.GetById(1));
    }

    [Fact]
    public void Delete_WhenNotFound_ShouldReturnFalse()
    {
        // Arrange
        var repo = CreateRepository();

        // Act
        var result = repo.Delete(99);

        // Assert
        Assert.False(result);
    }
}