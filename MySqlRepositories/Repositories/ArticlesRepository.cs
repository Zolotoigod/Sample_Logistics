using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlReposytories;

namespace MySqlRepositories.Repositories
{
    public class ArticlesRepository : IArticleRepository
    {
        private readonly SampleLogisticContext context;
        private readonly ILogger<ArticlesRepository> logger;

        public ArticlesRepository(SampleLogisticContext context, ILogger<ArticlesRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Guid> Create(Article model)
        {
            try
            {
                await context.Articles.AddAsync(model);
                await context.SaveChangesAsync();
                logger.LogInformation(string.Format(RepositoryMessages.CreateArticl, model.Id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            return model.Id;
        }

        public async Task<Article> ReadById(Guid id)
        {
            try
            {
                logger.LogInformation(string.Format(RepositoryMessages.ReadArticl, id));
                return await context.Articles
                        .Where(a => a.IsDeleted == false && a.Id == id)
                        .FirstOrDefaultAsync() 
                        ?? throw new InvalidOperationException($"Article #{id} not found"); ;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }


        public async IAsyncEnumerable<Article> ReadCollection(int offset, int limit)
        {
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionArticl));
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
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionArticl));
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
            logger.LogInformation(string.Format(RepositoryMessages.ReadCollectionArticl));
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
            try
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
                logger.LogInformation(string.Format(RepositoryMessages.UpdateArticl, id));
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
                var entity = await context.Articles
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException(nameof(id));

                entity.IsDeleted = true;
                await context.SaveChangesAsync();
                logger.LogInformation(string.Format(RepositoryMessages.DeleteArticl, id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task AddRange(IEnumerable<Article> collection)
        {
            try
            {
                await context.Articles.AddRangeAsync(collection);
                await context.SaveChangesAsync();
                logger.LogInformation(RepositoryMessages.AddCollectionArticl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Article>> ReadByDocumentNumber(Guid docNumber) =>
            await context.Articles.Where(a => a.DocumentNumber == docNumber).ToListAsync();
    }
}
