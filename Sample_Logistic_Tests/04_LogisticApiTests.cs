using LogisticApi.Controllers;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;
using Servises;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class LogisticApiTests
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
        private readonly Document testDocument = new Document()
        {
            Number = Guid.Parse("bdac52bb-e01b-4321-9f4f-e00af0c90549"),
            ActionType = true,
            ContragentName = "FishMeatMarket",
            Storage = "WarshawMarket",
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            IsDeleted = false,
            PriceNetto = 1000,
            Vat = 100,
            PriceBrutto = 1100,
        };
        private readonly ArticleRequest testArticleRequest = new ArticleRequest()
        {
            PositionName = "Apple",
            PositionCount = 10,
            Unit = "box",
            PriceBrutto = 550,
            Vat = 50,
            PriceNetto = 500,
        };
        private readonly UpdateDocumentRequest testDocumentRequest = new UpdateDocumentRequest()
        {
            ActionType = false,
            ContragentName = "SomeMarket",
            Storage = "GdanskMarket",
        };
        private LogisticController controller;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            string dbName = "LogisticDb" + Guid.NewGuid().ToString();
            var contextOptions = new DbContextOptionsBuilder<SampleLogisticContext>()
                .UseInMemoryDatabase(dbName).Options;

            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var articles = new ArticlesRepository(context, new Mock<ILogger<ArticlesRepository>>().Object);
            var documents = new DocumentRepository(context, new Mock<ILogger<DocumentRepository>>().Object);
            var service = new LogisticService(new Mock<ILogger<LogisticService>>().Object, articles, documents);
            controller = new LogisticController(service);
        }

        [Test]
        public async Task GetData()
        {
            //Arrange
            //Act
            var docs = await controller.ReadDocuments().ToListAsync();

            //Assert
            Assert.NotNull(docs);
        }
    }
}
