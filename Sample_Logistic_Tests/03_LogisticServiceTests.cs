using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Servises;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class LogisticServiceTests
    {
        private readonly ILogger<LogisticService> logger = new Mock<ILogger<LogisticService>>().Object;
        private readonly Mock<IArticleRepository> articleRepositoryMoq = new Mock<IArticleRepository>();
        private readonly Mock<IDocumentRepository> documentRepositoryMoq = new Mock<IDocumentRepository>();
        private readonly DocumentRequest testDocumentRequest = new DocumentRequest()
        {
            ActionType = true,
            ContragentName = "FishMeatMarket",
            Storage = "WarshawMarket",
            Articles = new List<ArticleRequest>()
            {
                new ArticleRequest()
                {
                    PositionName = "Fish",
                    PositionCount = 10,
                    Unit = "box",
                    PriceNetto = 1000,
                    Vat = 100,
                    PriceBrutto = 1100,
                },
                new ArticleRequest()
                {
                    PositionName = "Meat",
                    PositionCount = 4,
                    Unit = "box",
                    PriceNetto = 800,
                    Vat = 80,
                    PriceBrutto = 880,
                }
            },
        };

        [Test(Description = "Add document, sucsess")]
        public async Task AddDocumentTest()
        {
            //Arrange
            var service = new LogisticService(logger, articleRepositoryMoq.Object, documentRepositoryMoq.Object);

            //Act
            Assert.DoesNotThrowAsync(() => service.AddDocument(testDocumentRequest));
            var number = await service.AddDocument(testDocumentRequest);

            //Assert
            Assert.NotNull(number);
            Assert.IsInstanceOf(typeof(Guid), number);
        }


        [Test(Description = "Read docments with articles, sucsess")]
        public async Task ReadDocWithArticleTest()
        {
            //Arrange
                documentRepositoryMoq
                .Setup(r => r.ReadById(It.IsAny<Guid>()))
                .ReturnsAsync(TestsConfigurator.documents[0]);

                articleRepositoryMoq
                .Setup(a => a.ReadByDocumentNumber(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Article>() { TestsConfigurator.articles[0], TestsConfigurator.articles[2] });

            var service = new LogisticService(logger, articleRepositoryMoq.Object, documentRepositoryMoq.Object);
            //Act

            var response = await service.ReadDocumentWithArticle(Guid.NewGuid());
            //Assert

            Assert.NotNull(response);
            Assert.True(response.Number == TestsConfigurator.documents[0].Number);
            Assert.True(response.ContragentName == TestsConfigurator.documents[0].ContragentName);
            Assert.True(response.ActionType == TestsConfigurator.documents[0].ActionType);
            Assert.True(response.Articles!.Any(a => a.PositionName == TestsConfigurator.articles.ToArray()[0].PositionName));
        }
    }
}
