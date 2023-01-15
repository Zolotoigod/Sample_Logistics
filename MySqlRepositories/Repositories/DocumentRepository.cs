using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlReposytories;

namespace MySqlRepositories.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly SampleLogisticContext context;
        private readonly ILogger<DocumentRepository> logger;
        public DocumentRepository(SampleLogisticContext context, ILogger<DocumentRepository> logger) 
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Guid> Create(Document document)
        {
            try
            {
                await context.Documents.AddAsync(document);
                await context.SaveChangesAsync();
                logger.LogInformation(string.Format(RepositoryMessages.CreateDocument, document.Number));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            
            return document.Number;
        }

        public async Task<Document> ReadById(Guid id)
        {
            try
            {
                logger.LogInformation(string.Format(RepositoryMessages.ReadDocument, id));
                return await context.Documents
                        .Where(d => d.Number == id && !d.IsDeleted!.Value)
                        .FirstOrDefaultAsync()
                        ?? throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async IAsyncEnumerable<Document> ReadCollection(int offset, int limit)
        {
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionDocument));
            var collection = context.Documents
                .Where(a => !a.IsDeleted!.Value)
                .Skip(offset)
                .Take(limit)
                .AsAsyncEnumerable();

            await foreach (var document in collection)
            {
                yield return document;
            }
        }

        public async IAsyncEnumerable<Document> ReadCollectionByStorage(string market)
        {
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionDocument));
            var colection = context.Documents
                .Where(a => !a.IsDeleted!.Value && a.Storage == market)
                .AsAsyncEnumerable();

            await foreach (var document in colection)
            {
                yield return document;
            }
        }

        public async IAsyncEnumerable<Document> ReadAllItems()
        {
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionDocument));
            var colection = context.Documents
                .Where(a => !a.IsDeleted!.Value)
                .AsAsyncEnumerable();

            await foreach (var document in colection)
            {
                yield return document;
            }
        }

        public int TotalItems() => context.Documents
                .Where(a => !a.IsDeleted!.Value)
                .Count();

        public async Task UpdateById(Guid id, UpdateDocumentRequest request)
        {
            try
            {
                logger.LogInformation(string.Format(RepositoryMessages.UpdateDocument, id));
                var entity = await context.Documents
                .Where(a => a.Number == id && !a.IsDeleted!.Value)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

                entity.ContragentName = request.ContragentName is null ? entity.ContragentName : request.ContragentName;
                entity.Storage = request.Storage is null ? entity.Storage : request.Storage;
                entity.UpdateDate = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                logger.LogInformation(string.Format(RepositoryMessages.DeleteDocument, id));
                var entity = await context.Documents
                    .Where(a => a.Number == id && !a.IsDeleted!.Value)
                    .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

                entity.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
