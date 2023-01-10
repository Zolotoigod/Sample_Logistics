using LogisticApi.DBContext;
using LogisticApi.DTO;

namespace LogisticApi.Abstraction.Repositories
{
    public interface IArticleRepository
    {
        Task<Guid> Create(Article model);
        Task DeleteById(Guid id);
        IAsyncEnumerable<Article> ReadAllItems();
        Task<Article> ReadById(Guid id);
        IAsyncEnumerable<Article> ReadCollection(int offset, int limit);
        IAsyncEnumerable<Article> ReadCollectionByStorage(string market);
        int TotalItems();
        Task UpdateById(Guid id, ArticleRequest request);
        Task AddRange(IEnumerable<Article> collection);
        Task<IEnumerable<Article>> ReadByDocumentNumber(Guid docNumber);
    }
}