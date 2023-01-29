using DataModels.DTO;
using LogisticApi.Controllers;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;
using Servises;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class LogisticApiTests
    {
        private static readonly List<ArticleRequest> articlesRequest = new()
        {
            new ArticleRequest
            {
                PositionName = "Orange",
                PositionCount = 100,
                Unit = "box",
                PriceBrutto = 1100,
                Vat = 100,
                PriceNetto = 1000,
            },
            new ArticleRequest
            {
                PositionName = "Apple",
                PositionCount = 10,
                Unit = "box",
                PriceBrutto = 550,
                Vat = 50,
                PriceNetto = 500,
            },
            new ArticleRequest
            {
                PositionName = "Pineapple",
                PositionCount = 20,
                Unit = "box",
                PriceBrutto = 2200,
                Vat = 200,
                PriceNetto = 2000,
            },
        };
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
        private readonly DocumentRequest documentRequest = new DocumentRequest()
        {
            ActionType = true,
            ContragentName = "FruitMarket",
            Storage = "GdanskMarket",
            Articles = articlesRequest,
        };
        private LogisticController controller;
        private Guid addedNumber;

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

        [Test(Description = "Get all documents, ok")]
        [Order(0)]
        public async Task GetData()
        {
            //Arrange
            //Act
            var docs = await controller.ReadDocuments().ToListAsync();

            //Assert
            Assert.NotNull(docs);
            Assert.True(docs.Count == 2);
            Assert.IsTrue(docs.Any(d => d.Number == Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22")));
        }

        [Test(Description = "Get article by storage, ok")]
        [Order(1)]
        [TestCase("WarshawMarket", "a284246e-3e9d-4d95-ad71-0f0cda7d4501")]
        [TestCase("GdanskMarket", "cec1debb-1904-4d61-8634-df3c546632e9")]
        public async Task GetArticlesByStorage(string storage, string id)
        {
            //Arrange
            //Act
            var result = await controller.ShowMarketState(storage) as ObjectResult;
            var articles = (IEnumerable<ArticleResponse>)result!.Value!;

            //Assert
            Assert.NotNull(articles);
            Assert.True(articles.Count() == 2);
            var test = articles.FirstOrDefault(a => a.Id == Guid.Parse(id));
            Assert.NotNull(test);
        }

        [Test(Description = "Add document, not throw")]
        [Order(2)]
        public void AddDocument()
        {
            //Arrange
            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => addedNumber = await TestDelegate());
        }

        [Test(Description = "Show added document, ok")]
        [Order(3)]
        public async Task ShowAdded()
        {
            //Arrange
            //Act
            var docs = await controller.ReadDocuments().ToListAsync();

            //Assert
            Assert.NotNull(docs);
            Assert.True(docs.Count == 3);
            Assert.True(docs.Any(d => d.ContragentName == documentRequest.ContragentName));
        }

        [Test(Description = "Read document with articles, ok")]
        [Order(4)]
        public async Task ShowDocumentWithArticles()
        {
            //Arrange
            //Act
            var result = await controller.ReadDocument(addedNumber) as ObjectResult;
            var doc = (DocumentResponse)result!.Value!;

            //Assert
            Assert.NotNull(doc);
            Assert.True(doc.ContragentName == "FruitMarket");
            Assert.True(doc.Articles!.Count == 3);
            Assert.True(doc.Articles.Any(a => a.PositionName == "Apple"));
        }

        private async Task<Guid> TestDelegate()
        {
            var objectResult = await controller.AddDocument(documentRequest) as ObjectResult;
            return Guid.Parse(objectResult!.Value!.ToString()!);
        }
    }
}
