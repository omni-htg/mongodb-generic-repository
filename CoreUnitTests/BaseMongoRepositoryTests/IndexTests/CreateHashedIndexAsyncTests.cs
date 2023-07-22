using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture;
using CoreUnitTests.Infrastructure.Model;
using MongoDbGenericRepository.DataAccess.Index;
using MongoDbGenericRepository.Models;
using Moq;
using Xunit;
using CancellationToken = System.Threading.CancellationToken;

namespace CoreUnitTests.BaseMongoRepositoryTests.IndexTests;

public class CreateHashedIndexAsyncTests : BaseIndexTests
{
    private readonly Expression<Func<TestDocument, object>> fieldExpression = t => t.SomeContent2;
    private readonly Expression<Func<TestDocumentWithKey<int>, object>> keyedFieldExpression = t => t.SomeContent2;

    [Fact]
    public async Task WithFieldExpression_CreatesIndex()
    {
        // Arrange
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(fieldExpression, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task WithFieldExpressionAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(fieldExpression, null, null, token));
    }

    [Fact]
    public async Task WithFieldExpressionAndOptions_CreatesIndex()
    {
        // Arrange
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, options);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, options, null, CancellationToken.None));
    }

    [Fact]
    public async Task WithFieldExpressionAndOptionsAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, options, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, options, null, token));
    }

    [Fact]
    public async Task WithFieldExpressionAndPartitionKey_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, partitionKey);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, null, partitionKey, CancellationToken.None));
    }

    [Fact]
    public async Task WithFieldExpressionAndPartitionKeyAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, partitionKey, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, null, partitionKey, token));
    }

    [Fact]
    public async Task WithFieldExpressionAndOptionsAndPartitionKey_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };

        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, options, partitionKey);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, options, partitionKey, CancellationToken.None));
    }

    [Fact]
    public async Task WithFieldExpressionAndOptionsAndPartitionKeyAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var token = new CancellationToken(true);
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };

        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync(fieldExpression, options, partitionKey, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocument, Guid>(
                fieldExpression, options, partitionKey, token));
    }

    #region Keyed

    [Fact]
    public async Task Keyed_WithKeyedFieldExpression_CreatesIndex()
    {
        // Arrange
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, null, null, token));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndOptions_CreatesIndex()
    {
        // Arrange
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, options);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, options, null, CancellationToken.None));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndOptionsAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, options, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, options, null, token));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndPartitionKey_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, partitionKey);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, null, partitionKey, CancellationToken.None));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndPartitionKeyAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var token = new CancellationToken(true);
        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, partitionKey, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, null, partitionKey, token));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndOptionsAndPartitionKey_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };

        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, options, partitionKey);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, options, partitionKey, CancellationToken.None));
    }

    [Fact]
    public async Task Keyed_WithKeyedFieldExpressionAndOptionsAndPartitionKeyAndCancellationToken_CreatesIndex()
    {
        // Arrange
        var partitionKey = Fixture.Create<string>();
        var token = new CancellationToken(true);
        var indexName = Fixture.Create<string>();
        var options = new IndexCreationOptions { Name = indexName };

        IndexHandler = new Mock<IMongoDbIndexHandler>();

        // Act
        await Sut.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(keyedFieldExpression, options, partitionKey, token);

        // Assert
        IndexHandler.Verify(
            x => x.CreateHashedIndexAsync<TestDocumentWithKey<int>, int>(
                keyedFieldExpression, options, partitionKey, token));
    }

    #endregion
}
