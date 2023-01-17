using LogisticApi.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Sample_Logistic_Tests
{
    public class TestsConfigurator
    {
        public static  readonly List<Document> documents = new List<Document>()
        {
            new Document()
            {
                Number = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                ActionType = true,
                ContragentName = "FishMeatMarket",
                Storage = "WarshawMarket",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDeleted = false,
                PriceNetto = 1800,
                Vat = 180,
                PriceBrutto = 1980,
            },
            new Document()
            {
                Number = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                ActionType = true,
                ContragentName = "FishMeatMarket",
                Storage = "GdanskMarket",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDeleted = false,
                PriceNetto = 1700,
                Vat = 170,
                PriceBrutto = 1870,

            },
        };
        public static readonly List<Article> articles = new List<Article>()
        {
            new Article()
            {
                Id = Guid.Parse("a284246e-3e9d-4d95-ad71-0f0cda7d4501"),
                DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                ActionType = true,
                PositionName = "Fish",
                PositionCount = 10,
                Unit = "box",
                PriceNetto = 1000,
                Vat = 100,
                PriceBrutto = 1100,
                Storage = "WarshawMarket",
                IsDeleted = false,
            },
            new Article()
            {
                Id = Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9"),
                DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                ActionType = true,
                PositionName = "Fish",
                PositionCount = 5,
                Unit = "box",
                PriceNetto = 500,
                Vat = 50,
                PriceBrutto = 550,
                Storage = "GdanskMarket",
                IsDeleted = false,
            },
            new Article()
            {
                Id = Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43"),
                DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                ActionType = true,
                PositionName = "Meat",
                PositionCount = 4,
                Unit = "box",
                PriceNetto = 800,
                Vat = 80,
                PriceBrutto = 880,
                Storage = "WarshawMarket",
                IsDeleted = false,
            },
            new Article()
            {
                Id = Guid.Parse("61797a05-60aa-4ec6-bd13-cdbd09b57ea0"),
                DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                ActionType = true,
                PositionName = "Meat",
                PositionCount = 6,
                Unit = "box",
                PriceNetto = 1200,
                Vat = 120,
                PriceBrutto = 1320,
                Storage = "GdanskMarket",
                IsDeleted = false,
            },
        };

        private async Task SetTestDataAsync(SampleLogisticContext context)
        {
            var documents = new List<Document>()
            {
                new Document()
                {
                    Number = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    ContragentName = "FishMeatMarket",
                    Storage = "WarshawMarket",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    PriceNetto = 1800,
                    Vat = 180,
                    PriceBrutto = 1980,
                },
                new Document()
                {
                    Number = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    ContragentName = "FishMeatMarket",
                    Storage = "GdanskMarket",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    PriceNetto = 1700,
                    Vat = 170,
                    PriceBrutto = 1870,
                },
            };
            context.Documents.AddRange(documents);
            await context.SaveChangesAsync();
            var articles = new List<Article>()
            {
                new Article()
                {
                    Id = Guid.Parse("a284246e-3e9d-4d95-ad71-0f0cda7d4501"),
                    DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    PositionName = "Fish",
                    PositionCount = 10,
                    Unit = "box",
                    PriceNetto = 1000,
                    Vat = 100,
                    PriceBrutto = 1100,
                    Storage = "WarshawMarket",
                    IsDeleted = false,
                },
                new Article()
                {
                    Id = Guid.Parse("cec1debb-1904-4d61-8634-df3c546632e9"),
                    DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    PositionName = "Fish",
                    PositionCount = 5,
                    Unit = "box",
                    PriceNetto = 500,
                    Vat = 50,
                    PriceBrutto = 550,
                    Storage = "GdanskMarket",
                    IsDeleted = false,
                },
                new Article()
                {
                    Id = Guid.Parse("6f82bb83-44af-4cbd-bffd-1d31d86f6b43"),
                    DocumentNumber = Guid.Parse("7af64e5a-3010-413e-bfad-c105c3687e22"),
                    ActionType = true,
                    PositionName = "Meat",
                    PositionCount = 4,
                    Unit = "box",
                    PriceNetto = 800,
                    Vat = 80,
                    PriceBrutto = 880,
                    Storage = "WarshawMarket",
                    IsDeleted = false,
                },
                new Article()
                {
                    Id = Guid.Parse("61797a05-60aa-4ec6-bd13-cdbd09b57ea0"),
                    DocumentNumber = Guid.Parse("117d2dd3-6d5f-4c86-a77f-43ab3e5a1497"),
                    ActionType = true,
                    PositionName = "Meat",
                    PositionCount = 6,
                    Unit = "box",
                    PriceNetto = 1200,
                    Vat = 120,
                    PriceBrutto = 1320,
                    Storage = "GdanskMarket",
                    IsDeleted = false,
                },
            };
            context.Articles.AddRange(articles);
            await context.SaveChangesAsync();
        }

        public async Task<SampleLogisticContext> InitialiseContext(DbContextOptions<SampleLogisticContext>? contextOptions)
        {
            var context = new SampleLogisticContext(contextOptions!);
            await SetTestDataAsync(context);
            return context;
        }
    }
}
