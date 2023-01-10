using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace MySqlRepositories.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly SampleLogisticContext context;
        public DocumentRepository(SampleLogisticContext context) 
        {
            this.context = context;
        }

        public async Task<Guid> Create(Document document)
        {
            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();
            return document.Number;
        }

        public async Task<Document> ReadById(Guid id) => await context.Documents
            .Where(d => d.Number == id && !d.IsDeleted!.Value)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();

        public async IAsyncEnumerable<Document> ReadCollection(int offset, int limit)
        {
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

        public async IAsyncEnumerable<Document> ReadCollectionByStprage(string market)
        {
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
            var colection = context.Documents
                .Where(a => !a.IsDeleted!.Value)
                .AsAsyncEnumerable();

            await foreach (var article in colection)
            {
                yield return article;
            }
        }

        public int TotalItems() => context.Documents
                .Where(a => !a.IsDeleted!.Value)
                .Count();

        public async Task UpdateById(Guid id, DocumentRequest request)
        {
            var entity = await context.Documents
                .Where(a => a.Number == id && !a.IsDeleted!.Value)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

            entity.ContragentName = request.ContragentName is null ? entity.ContragentName : request.ContragentName;
            entity.Storage = request.Storage is null ? entity.Storage : request.Storage;
            entity.UpdateDate = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var entity = await context.Documents
                .Where(a => a.Number == id && !a.IsDeleted!.Value)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

            entity.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}
