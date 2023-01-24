using DataModels.DTO;
using LogisticApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Servises;

namespace LogisticApi.Controllers
{
    [Route("logistic")]
    public class LogisticController : ControllerBase
    {
		private readonly ILogisticService service;

        public LogisticController(ILogisticService service)
        {
            this.service = service;
        }

        [HttpPost("document")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddDocument([FromBody]DocumentRequest request)
		{
			try
			{
				return Ok(await service.AddDocument(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("document/{id}")]
        [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReadDocument([FromRoute]Guid id)
        {
            try
            {
                return Ok(await service.ReadDocumentWithArticle(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("documents")]
        [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<DocumentResponse> ReadDocuments()
        {
            var collection = service.ReadDocuments();
            await foreach (var item in collection)
            {
                yield return item;
            }
        }

        [HttpGet("market/state/{name}")]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ShowMarketState([FromRoute] string name)
        {
            var collection = await service.ShowArticleForStorage(name);
            return Ok(collection);
        }
    }
}
