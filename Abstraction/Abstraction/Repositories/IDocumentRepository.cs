using LogisticApi.DBContext;
using LogisticApi.DTO;

namespace LogisticApi.Abstraction.Repositories
{
    public interface IDocumentRepository
    {
        Task<Guid> Create(Document model);
        Task DeleteById(Guid id);
        IAsyncEnumerable<Document> ReadAllItems();
        Task<Document> ReadById(Guid id);
        IAsyncEnumerable<Document> ReadCollection(int offset, int limit);
        IAsyncEnumerable<Document> ReadCollectionByStorage(string storage);
        int TotalItems();
        Task UpdateById(Guid id, UpdateDocumentRequest request);
    }
}