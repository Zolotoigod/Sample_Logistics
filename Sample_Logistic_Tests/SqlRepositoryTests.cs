using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class SqlRepositoryTests
    {
        private DbContextOptions<SampleLogisticContext>? contextOptions;
        private readonly ILogger<DocumentRepository> docLogger = new Mock<ILogger<DocumentRepository>>().Object;
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
        private readonly UpdateDocumentRequest testRequest = new UpdateDocumentRequest()
        {
            ActionType = false,
            ContragentName = "SomeMarket",
            Storage = "GdanskMarket",
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
        };

        [SetUp]
        public void SetUp()
        {
            string dbName = "LogisticDb" + Guid.NewGuid().ToString();
            contextOptions = new DbContextOptionsBuilder<SampleLogisticContext>()
                .UseInMemoryDatabase(dbName).Options;
        }

        
        [Test(Description = "Read all documents, success")]
        public async Task ReadDocuments()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            var docs = await documentRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(docs.Length == 2);
            Assert.True(docs.Any(d => d.Number == Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22")));
            Assert.True(docs.Any(d => d.Number == Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497")));
        }

        [Test(Description = "Read document by id, success")]
        public async Task ReadDocumentById()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            var doc1 = await documentRepository.ReadById(Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"));
            var doc2 = await documentRepository.ReadById(Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"));

            //Assert
            Assert.False(doc1 is null);
            Assert.False(doc2 is null);
            Assert.True(doc1!.Storage == "WarshawMarket");
            Assert.True(doc2!.Storage == "GdanskMarket");
        }

        [Test(Description = "Read document by id, faild")]
        public async Task ReadDocumentByIdFaild()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() => 
            documentRepository.ReadById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f"))
            .GetAwaiter()
            .GetResult());
        }

        [Test(Description = "Read documents by storage, success")]
        public async Task ReadDocumentsByStorage()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            var docs = await documentRepository.ReadCollectionByStorage("GdanskMarket").ToArrayAsync();

            //Assert
            Assert.True(docs.Length == 1);
            Assert.True(docs.First().Number == Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"));
        }

        [Test(Description = "Create document, success")]
        public async Task CreateDocument()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            await documentRepository.Create(testDocument);
            var docs = await documentRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(docs.Length == 3);
            Assert.True(docs.Any(d => d.Number == Guid.Parse("bdac52bb-e01b-4321-9f4f-e00af0c90549")));
        }

        [Test(Description = "Update document, success")]
        public async Task UpdateDocument()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            await documentRepository.UpdateById(Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"), testRequest);
            var doc = await documentRepository.ReadById(Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"));

            //Assert
            Assert.True(doc.Storage == testRequest.Storage);
            Assert.True(doc.UpdateDate != doc.CreateDate);
            Assert.True(doc.ContragentName == testRequest.ContragentName);
        }

        [Test(Description = "Update document, faild")]
        public async Task UpdateDocumentFaild()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() =>
            documentRepository.UpdateById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f"), testRequest)
                .GetAwaiter()
                .GetResult());
        }

        [Test(Description = "Delete document, success")]
        public async Task DeleteDocument()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            await documentRepository.DeleteById(Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"));
            var docs = await documentRepository.ReadAllItems().ToArrayAsync();

            //Assert
            Assert.True(docs.Length == 1);
            Assert.False(docs.Any(d => d.Number == Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497")));
        }

        [Test(Description = "Delete document, faild")]
        public async Task DeleteDocumentFaild()
        {
            //Arrange
            var context = await InitialiseContext();
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() =>
                documentRepository.DeleteById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f"))
                .GetAwaiter()
                .GetResult());
        }

        private async Task SetTestDataAsync(SampleLogisticContext context)
        {
            var documents = new List<Document>()
            {
                new Document()
                {
                    Number = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    ContragentName = "FishMeatMarket",
                    Storage = "WarshawMarket",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    PriceNetto = 1800,
                    Vat = 180,
                    PriceBrutto = 1980,
                },
                new Document()
                {
                    Number = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    ContragentName = "FishMeatMarket",
                    Storage = "GdanskMarket",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    PriceNetto = 1700,
                    Vat = 170,
                    PriceBrutto = 1870,
                },
            };

            var articles = new List<Article>()
            {
                new Article()
                {
                    Id = Guid.Parse("a284246e-3e9d-4d95-ad71-0f0cda7d4501"),
                    DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    PositionName = "Fish",
                    PositionCount = 10,
                    Unit = "box",
                    PriceNetto = 1000,
                    Vat = 100,
                    PriceBrutto = 1100,
                    Storage = "WarshawMarket",
                },
                new Article()
                {
                    Id = Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9"),
                    DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    PositionName = "Fish",
                    PositionCount = 5,
                    Unit = "box",
                    PriceNetto = 500,
                    Vat = 50,
                    PriceBrutto = 550,
                    Storage = "WarshawMarket",
                },
                new Article()
                {
                    Id = Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43"),
                    DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    PositionName = "Meat",
                    PositionCount = 4,
                    Unit = "box",
                    PriceNetto = 800,
                    Vat = 80,
                    PriceBrutto = 880,
                    Storage = "WarshawMarket",
                },
                new Article()
                {
                    Id = Guid.Parse("61797a05-60aa-4ec6-bd13-cdbd09b57ea0"),
                    DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    PositionName = "Meat",
                    PositionCount = 6,
                    Unit = "box",
                    PriceNetto = 1200,
                    Vat = 120,
                    PriceBrutto = 1320,
                    Storage = "WarshawMarket",
                },
            };

            context.Documents.AddRange(documents);
            await context.SaveChangesAsync();
            context.Articles.AddRange(articles);
            await context.SaveChangesAsync();
        }

        private async Task<SampleLogisticContext> InitialiseContext()
        {
            var context = new SampleLogisticContext(contextOptions!);
            await SetTestDataAsync(context);
            return context;
        }
    }
}