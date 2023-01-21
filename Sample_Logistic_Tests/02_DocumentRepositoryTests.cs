using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;

namespace Sample_Logistic_Tests
{
    [TestFixture]
    public class DocumentRepositoryTests
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
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
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
        [TestCase("7af64e5a-3010-413e-bfad-c105c3687e22", ExpectedResult = "WarshawMarket")]
        [TestCase("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497", ExpectedResult = "GdanskMarket")]
        public async Task<string> ReadDocumentById(string number)
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act
            var doc = await documentRepository.ReadById(Guid.Parse(number));

            //Assert
            Assert.False(doc is null);
            return doc!.Storage;
        }

        [Test(Description = "Read document by id, faild")]
        public async Task ReadDocumentByIdFaild()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await documentRepository.ReadById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f")));
        }

        [Test(Description = "Read documents by storage, success")]
        public async Task ReadDocumentsByStorage()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
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
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
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
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
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
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await documentRepository.UpdateById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f"), testRequest));
        }

        [Test(Description = "Delete document, success")]
        public async Task DeleteDocument()
        {
            //Arrange
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
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
            var context = await new TestsConfigurator().InitialiseContext(contextOptions);
            var documentRepository
                = new DocumentRepository(context, docLogger);

            //Act

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await documentRepository.DeleteById(Guid.Parse("a4642dd0-e12d-431b-9544-58303df1234f")));
        }
    }
}