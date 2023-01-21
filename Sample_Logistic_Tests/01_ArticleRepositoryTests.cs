using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class ArticleRepositoryTests
    {
        private readonly Article testArticle = new Article
        {
            Id = Guid.Parse("7558f472-5968-49af-92c3-75f0343ffcab"),
            DocumentNumber = Guid.Parse("bdac52bb-e01b-4321-9f4f-e00af0c90549"),
            ActionType = true,
            PositionName = "Fish",
            PositionCount = 10,
            Unit = "box",
            PriceNetto = 1000,
            Vat = 100,
            PriceBrutto = 1100,
            Storage = "WarshawMarket",
            IsDeleted = false,
        };
        private DbContextOptions<SampleLogisticContext>? contextOptions;
        private readonly ILogger<ArticlesRepository> docLogger = new Mock<ILogger<ArticlesRepository>>().Object;
        private readonly ArticleRequest testRequest = new ArticleRequest()
        {
            PositionName = "Apple",
            PositionCount = 10,
            Unit = "box",
            PriceBrutto = 550,
            Vat = 50,
            PriceNetto = 500,
        };

        [SetUp]
        public void SetUp()
        {
            string dbName = "LogisticDb" + Guid.NewGuid().ToString();
            contextOptions = new DbContextOptionsBuilder<SampleLogisticContext>()
                .UseInMemoryDatabase(dbName).Options;
        }

        [Test(Description = "Read all articles, success")]
        public async Task ReadArticles()
        {
            //Arrange 
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            var articls = await articleRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(articls.Length == 4);
            Assert.True(articls.Any(a => a.Id == Guid.Parse("a284246e-3e9d-4d95-ad71-0f0cda7d4501")));
            Assert.True(articls.Any(a => a.Id == Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9")));
            Assert.True(articls.Any(a => a.Id == Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43")));
            Assert.True(articls.Any(a => a.Id == Guid.Parse("61797a05-60aa-4ec6-bd13-cdbd09b57ea0")));

        }

        [Test(Description = "Read article by id, success")]
        [TestCase("a284246e-3e9d-4d95-ad71-0f0cda7d4501", ExpectedResult = "Fish")]
        [TestCase("cec1debb-1904-4d61-8634-df3c546632e9", ExpectedResult = "Fish")]
        [TestCase("6f82bb83-44af-4cbd-bffd-1d31d86f6b43", ExpectedResult = "Meat")]
        [TestCase("61797a05-60aa-4ec6-bd13-cdbd09b57ea0", ExpectedResult = "Meat")]
        public async Task<string> ReadArticleById(string id)
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            var doc = await articleRepository.ReadById(Guid.Parse(id));

            //Assert
            Assert.False(doc is null);
            return doc!.PositionName;
        }

        [Test(Description = "Read article by id, faild")]
        public async Task ReadArticleByIdFaild()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var ArticleRepository
                = new ArticlesRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await ArticleRepository.ReadById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f")));
        }

        [Test(Description = "Read Articles by storage, success")]
        public async Task ReadArticlesByStorage()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var ArticleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            var articls = await ArticleRepository.ReadCollectionByStorage("GdanskMarket").ToArrayAsync();

            //Assert
            Assert.True(articls.Length == 2);
            Assert.True(articls.Any(a => a.Id == Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9")));
            Assert.True(articls.Any(a => a.Id == Guid.Parse("61797a05-60aa-4ec6-bd13-cdbd09b57ea0")));
        }

        [Test(Description = "Create Article, success")]
        public async Task CreateArticle()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            await articleRepository.Create(testArticle);
            var articls = await articleRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(articls.Length == 5);
            Assert.True(articls.Any(d => d.Id == Guid.Parse("7558f472-5968-49af-92c3-75f0343ffcab")));
        }

        [Test(Description = "Update Article, success")]
        public async Task UpdateArticle()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            await articleRepository.UpdateById(Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9"), testRequest);
            var doc = await articleRepository.ReadById(Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9"));

            //Assert
            Assert.True(doc.PositionName == testRequest.PositionName);
            Assert.True(doc.PositionCount == testRequest.PositionCount);
            Assert.True(doc.Unit == testRequest.Unit);
            Assert.True(doc.PriceBrutto == testRequest.PriceBrutto);
            Assert.True(doc.Vat == testRequest.Vat);
            Assert.True(doc.PriceNetto == testRequest.PriceNetto);
        }

        [Test(Description = "Update Article, faild")]
        public async Task UpdateArticleFaild()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var ArticleRepository
                = new ArticlesRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await ArticleRepository.UpdateById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f"), testRequest));
        }

        [Test(Description = "Delete Article, success")]
        public async Task DeleteArticle()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act
            await articleRepository.DeleteById(Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43"));
            var articls = await articleRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(articls.Length == 3);
            Assert.False(articls.Any(d => d.Id == Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43")));
        }

        [Test(Description = "Delete Article, faild")]
        public async Task DeleteArticleFaild()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articleRepository
                = new ArticlesRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>( async () =>
                await articleRepository.DeleteById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f")));
        }
    }
}
