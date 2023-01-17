using DataModels.DTO;
using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using LogisticMapper;
using Microsoft.Extensions.Logging;

namespace Servises
{
    public class LogisticService : ILogisticService
    {
        private readonly ILogger<LogisticService> logger;
        private readonly IArticleRepository articleReposirory;
        private readonly IDocumentRepository documentReposirory;

        public LogisticService(ILogger<LogisticService> logger,
            IArticleRepository articleReposirory,
            IDocumentRepository documentReposirory)
        {
            this.logger = logger;
            this.articleReposirory = articleReposirory;
            this.documentReposirory = documentReposirory;
        }

        public async Task<Guid> AddDocument(DocumentRequest document)
        {
            var doc = document.ToEntity();
            await documentReposirory.Create(doc);
            await articleReposirory.AddRange(document.Articles.Select(a => a.ToEntity(doc)));

            return doc.Number;
        }

        public async Task<DocumentResponse> ReadDocumentWithArticle(Guid id)
        {
            var document = await documentReposirory.ReadById(id);
            var articles = (await articleReposirory.ReadByDocumentNumber(id))
                .Select(a => a.ToResponse())
                .ToList();
            return document.ToResponse(articles);
        }

        public async Task<IEnumerable<ArticleResponse>> ShowArticleForStorage(string storage)
        {
            var collection = articleReposirory.ReadCollectionByStorage(storage);
            Dictionary<string, Article> cache = new ();

            await foreach (var article in collection)
            {
                if (!cache.ContainsKey(article.PositionName + article.Storage))
                {
                    cache.Add(article.PositionName + article.Storage, article);
                }
                else
                {
                    if (cache[article.PositionName + article.Storage].ActionType)
                    {
                        cache[article.PositionName + article.Storage].PositionCount += article.PositionCount;
                        cache[article.PositionName + article.Storage].PriceBrutto += article.PriceBrutto;
                        cache[article.PositionName + article.Storage].PriceNetto += article.PriceNetto;
                        cache[article.PositionName + article.Storage].Vat += article.Vat;
                    }
                    else
                    {
                        cache[article.PositionName + article.Storage].PositionCount =
                            Math.Abs((cache[article.PositionName + article.Storage].PositionCount - article.PositionCount)!.Value);
                        cache[article.PositionName + article.Storage].PriceBrutto =
                            Math.Abs((cache[article.PositionName + article.Storage].PriceBrutto - article.PriceBrutto)!.Value);
                        cache[article.PositionName + article.Storage].PriceNetto =
                            Math.Abs((cache[article.PositionName + article.Storage].PriceNetto - article.PriceNetto)!.Value);
                        cache[article.PositionName + article.Storage].Vat =
                            Math.Abs((cache[article.PositionName + article.Storage].Vat - article.Vat)!.Value);
                    }                  
                }
            }

            return cache.Select(d => d.Value.ToResponse());
        }

        public async IAsyncEnumerable<DocumentResponse> ReadDocuments()
        {
            var collection = documentReposirory.ReadAllItems();

            await foreach (var doc in collection)
            {
                yield return doc.ToResponse();
            }
        }
    }
}
