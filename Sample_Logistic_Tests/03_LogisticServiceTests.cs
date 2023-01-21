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

        [Test(Description = "Add document, success")]
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

        [Test(Description = "Read all documents, success")]
        public async Task ReadDocuments()
        {
            //Arrange
            documentRepositoryMoq
            .Setup(r => r.ReadAllItems())
            .Returns(new List<Document>()
                {
                    TestsConfigurator.documents[0],
                    TestsConfigurator.documents[1]
                }
            .ToAsyncEnumerable());

            var service =
                new LogisticService(logger, articleRepositoryMoq.Object, documentRepositoryMoq.Object);

            //Act
            var response = await service.ReadDocuments().ToListAsync();

            //Assert
            Assert.NotNull(response);
            Assert.That(response.Count == 2);
            Assert.That(response.Any(d => d.Number == TestsConfigurator.documents[0].Number));
        }


        [Test(Description = "Read docments with articles, success")]
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

        [Test(Description = "Read docments with articles, faild")]
        public void ReadDocWithArticleTestFaild()
        {
            //Arrange
            documentRepositoryMoq
            .Setup(r => r.ReadById(It.Is<Guid>(g => g != Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"))))
            .ThrowsAsync(new InvalidOperationException());

            articleRepositoryMoq
            .Setup(a => a.ReadByDocumentNumber(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Article>() { TestsConfigurator.articles[0], TestsConfigurator.articles[2] });

            var service = new LogisticService(logger, articleRepositoryMoq.Object, documentRepositoryMoq.Object);

            //Act
              
            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async() => await service.ReadDocumentWithArticle(Guid.NewGuid()));
        }


        [Test(Description = "Read article by storage, success")]
        public async Task ReadArticlesByStorage()
        {
            //Arrange
            articleRepositoryMoq
                 .Setup(a => a.ReadCollectionByStorage("WarshawMarket"))
                 .Returns(new List<Article>()
                 {
                     TestsConfigurator.articles[0],
                     TestsConfigurator.articles[2]
                 }
                 .ToAsyncEnumerable());

            var service = new LogisticService(logger, articleRepositoryMoq.Object, documentRepositoryMoq.Object);

            //Act
            var response = await service.ShowArticleForStorage("WarshawMarket");


            //Assert
            Assert.NotNull(response);
            Assert.True(response.Any(a => a.Id == TestsConfigurator.articles[0].Id));
            Assert.False(response.Any(a => a.Id == TestsConfigurator.articles[3].Id));
        }
    }
}
