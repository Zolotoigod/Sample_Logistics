using DataModels.DTO;
using LogisticApi.DTO;

namespace Servises
{
    public interface ILogisticService
    {
        Task<Guid> AddDocument(DocumentRequest document);
        Task<DocumentResponse> ReadDocumentWithArticle(Guid id);
        Task<IEnumerable<ArticleResponse>> ShowArtikleForStorage(string market);
        IAsyncEnumerable<DocumentResponse> ReadDocuments();
    }
}