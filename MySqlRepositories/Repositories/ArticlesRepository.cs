using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace MySqlRepositories.Repositories
{
    public class ArticlesRepository : IArticleRepository
    {
        private readonly SampleLogisticContext context;

        public ArticlesRepository(SampleLogisticContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Create(Article model)
        {
            await context.Articles.AddAsync(model);
            await context.SaveChangesAsync();
            return model.Id;
        }

#pragma warning disable CS8603 // Possible null reference return.
        public async Task<Article> ReadById(Guid id) => await context.Articles
                .Where(a => a.IsDeleted == false && a.Id == id)
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.

        public async IAsyncEnumerable<Article> ReadCollection(int offset, int limit)
        {
            var colection = context.Articles
                .Where(a => a.IsDeleted == false)
                .Skip(offset)
                .Take(limit)
                .AsAsyncEnumerable();

            await foreach (var article in colection)
            {
                yield return article;
            }
        }

        public async IAsyncEnumerable<Article> ReadCollectionByStorage(string storage)
        {
            var colection = context.Articles
                .Where(a => a.IsDeleted == false && a.Storage == storage)
                .AsAsyncEnumerable();

            await foreach (var article in colection)
            {
                yield return article;
            }
        }

        public async IAsyncEnumerable<Article> ReadAllItems()
        {
            var colection = context.Articles
                .Where(a => a.IsDeleted == false)
                .AsAsyncEnumerable();

            await foreach (var article in colection)
            {
                yield return article;
            }
        }

        public int TotalItems() => context.Articles
                .Where(a => a.IsDeleted == false)
                .Count();

        public async Task UpdateById(Guid id, ArticleRequest request)
        {
            var entity = await context.Articles
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

            entity.PositionCount = request.PositionCount is null ? entity.PositionCount : request.PositionCount;
            entity.PositionName = request.PositionName is null ? entity.PositionName : request.PositionName;
            entity.Unit = request.Unit is null ? entity.Unit : request.Unit;
            entity.PriceNetto = request.PriceNetto is null ? entity.PriceNetto : request.PriceNetto;
            entity.Vat = request.Vat is null ? entity.Vat : request.Vat;
            entity.PriceBrutto = request.PriceBrutto is null ? entity.PriceBrutto : request.PriceBrutto;
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var entity = await context.Articles
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

            entity.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<Article> collection)
        {
            await context.Articles.AddRangeAsync(collection);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Article>> ReadByDocumentNumber(Guid docNumber) =>
            await context.Articles.Where(a => a.DocumentNumber == docNumber).ToListAsync();
    }
}
