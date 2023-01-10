using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using LogisticMapper;
using Microsoft.AspNetCore.Mvc;

namespace FullApi.Controllers
{
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]ArticleRequest model)
        {
            try
            {
                return Ok(await articleRepository.Create(model.ToEntity(Guid.NewGuid())));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("readById/{id}")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReadById([FromRoute]Guid id)
        {
            try
            {
                var resp = await articleRepository.ReadById(id);
                return Ok(resp ?? throw new InvalidOperationException($"Article #{id} not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("read/{offset}/{limit}")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Article> ReadCollection([FromRoute] int offset, [FromRoute] int limit)
        {
            var collection = articleRepository.ReadCollection(offset, limit);
            await foreach (var article in collection)
            {
                yield return article;
            }
        }

        [HttpGet("read/market/{market}")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Article> ReadByMarket([FromRoute] string market)
        {
            var collection = articleRepository.ReadCollectionByStorage(market);
            await foreach (var article in collection)
            {
                yield return article;
            }
        }

        [HttpGet("read/all")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Article> ReadAll()
        {
            var collection = articleRepository.ReadAllItems();
            await foreach (var article in collection)
            {
                yield return article;
            }
        }

        [HttpPatch("updateById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ArticleRequest model)
        {
            try
            {
                await articleRepository.UpdateById(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await articleRepository.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
