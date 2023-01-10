using DataModels.DTO;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using Microsoft.IdentityModel.Tokens;

namespace LogisticMapper
{
    public static class Mapper
    {
        public static Document ToEntity(this DocumentRequest request)
        {
            return new Document()
            {
                Number = Guid.NewGuid(),
                ContragentName = request.ContragentName,
                ActionType = request.ActionType!.Value,
                Storage = request.Storage,
                CreateDate = DateTime.Now,
                PriceBrutto = request.Articles.Sum(a => a.PriceBrutto),
                PriceNetto = request.Articles.Sum(a => a.PriceNetto),
                Vat = request.Articles.Sum(a => a.Vat),
            };
        }

        public static Article ToEntity(this ArticleRequest request, Document document)
        {
            return new Article()
            {
                Id = Guid.NewGuid(),
                PositionName = request.PositionName,
                PositionCount = request.PositionCount,
                PriceBrutto = request.PriceBrutto,
                PriceNetto = request.PriceNetto,
                Vat = request.Vat,
                DocumentNumber = document.Number,
                Storage = document.Storage,
                ActionType = document.ActionType,
                Unit = request.Unit,
            };
        }

        public static Article ToEntity(this ArticleRequest request,
             Guid number, string storage = "newStorage", bool actiontype = true)
        {
            return new Article()
            {
                Id = Guid.NewGuid(),
                PositionName = request.PositionName,
                PositionCount = request.PositionCount,
                PriceBrutto = request.PriceBrutto,
                PriceNetto = request.PriceNetto,
                Vat = request.Vat,
                DocumentNumber = number,
                Storage = storage,
                ActionType = actiontype,
                Unit = request.Unit,
            };
        }

        public static ArticleResponse ToResponse(this Article article) =>
            new ArticleResponse(
                article.Id,
                article.PositionName,
                article.PositionCount,
                article.Unit,
                article.PriceNetto,
                article.Vat,
                article.PriceBrutto);

        public static DocumentResponse ToResponse(this Document document, List<ArticleResponse> collection = null!)
        {
            var articles = document.Articles
                .Select(a => a.ToResponse())
                .ToList();
            if (articles.IsNullOrEmpty())
            {
                articles = collection;
            }

            return new DocumentResponse(
                document.Number,
                document.ActionType,
                document.CreateDate,
                document.UpdateDate,
                document.ContragentName,
                document.Storage,
                document.PriceNetto,
                document.Vat,
                document.PriceBrutto,
                articles);
        }   
    }
}