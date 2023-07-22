using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using CoreUnitTests.Infrastructure.Model;
using FluentAssertions;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Moq;
using Xunit;

namespace CoreUnitTests.DataAccessTests.MongoDbReaderTests;

public class ProjectManyAsyncTests : BaseReaderTests
{
    private readonly Expression<Func<TestDocument, bool>> filter = t => string.IsNullOrWhiteSpace(t.SomeContent2);

    private readonly Expression<Func<TestDocument, TestProjection>>
        projectionExpression = t => new TestProjection {TestDocumentId = t.Id, NestedData = t.Nested.SomeDate};

    [Fact]
    public async Task WithFilterAndProjection_Projects()
    {
        // Arrange
        var collection = MockOf<IMongoCollection<TestDocument>>();
        var projections = Fixture.CreateMany<TestProjection>().ToList();
        var (context, cursor) = SetupAsyncProjection(projections, collection);

        // Act
        var result = await Sut.ProjectManyAsync<TestDocument, TestProjection, Guid>(filter, projectionExpression);

        // Assert
        context.Verify(x => x.GetCollection<TestDocument>(null), Times.Once);
        cursor.Verify(x => x.MoveNextAsync(CancellationToken.None), Times.AtLeast(1));
        result.Should().NotBeNull();
        result.Should().OnlyContain(x => projections.Contains(x));
    }

    [Fact]
    public async Task WithFilterAndProjectionAndCancellationToken_Projects()
    {
        // Arrange
        var collection = MockOf<IMongoCollection<TestDocument>>();
        var projections = Fixture.CreateMany<TestProjection>().ToList();
        var token = new CancellationToken(false);
        var (context, cursor) = SetupAsyncProjection(projections, collection);

        // Act
        var result = await Sut.ProjectManyAsync<TestDocument, TestProjection, Guid>(filter, projectionExpression, cancellationToken: token);

        // Assert
        context.Verify(x => x.GetCollection<TestDocument>(null), Times.Once);
        cursor.Verify(x => x.MoveNextAsync(token), Times.AtLeast(1));
        result.Should().NotBeNull();
        result.Should().OnlyContain(x => projections.Contains(x));
    }

    [Fact]
    public async Task WithFilterAndProjectionAndPartitionKey_Projects()
    {
        // Arrange
        var collection = MockOf<IMongoCollection<TestDocument>>();
        var projections = Fixture.CreateMany<TestProjection>().ToList();
        var partitionKey = Fixture.Create<string>();
        var (context, cursor) = SetupAsyncProjection(projections, collection, partitionKey);

        // Act
        var result = await Sut.ProjectManyAsync<TestDocument, TestProjection, Guid>(filter, projectionExpression, partitionKey);

        // Assert
        context.Verify(x => x.GetCollection<TestDocument>(partitionKey), Times.Once);
        cursor.Verify(x => x.MoveNextAsync(CancellationToken.None), Times.AtLeast(1));
        result.Should().NotBeNull();
        result.Should().OnlyContain(x => projections.Contains(x));
    }

    [Fact]
    public async Task WithFilterAndProjectionAndPartitionKeyAndCancellationToken_Projects()
    {
        // Arrange
        var collection = MockOf<IMongoCollection<TestDocument>>();
        var projections = Fixture.CreateMany<TestProjection>().ToList();
        var partitionKey = Fixture.Create<string>();
        var token = new CancellationToken(false);
        var (context, cursor) = SetupAsyncProjection(projections, collection, partitionKey);

        // Act
        var result = await Sut.ProjectManyAsync<TestDocument, TestProjection, Guid>(filter, projectionExpression, partitionKey, token);

        // Assert
        context.Verify(x => x.GetCollection<TestDocument>(partitionKey), Times.Once);
        cursor.Verify(x => x.MoveNextAsync(token), Times.AtLeast(1));
        result.Should().NotBeNull();
        result.Should().OnlyContain(x => projections.Contains(x));
    }

    private (Mock<IMongoDbContext>, Mock<IAsyncCursor<TProjection>>) SetupAsyncProjection<TDocument, TProjection>(
        List<TProjection> projections,
        Mock<IMongoCollection<TDocument>> collection,
        string partitionKey = null)
    {
        var asyncCursor = SetupAsyncCursor(projections);

        collection
            .Setup(
                x => x.FindAsync(
                    It.IsAny<FilterDefinition<TDocument>>(),
                    It.IsAny<FindOptions<TDocument, TProjection>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        var context = MockOf<IMongoDbContext>();

        context
            .Setup(x => x.GetCollection<TDocument>(partitionKey))
            .Returns(collection.Object);

        return (context, asyncCursor);
    }
}
