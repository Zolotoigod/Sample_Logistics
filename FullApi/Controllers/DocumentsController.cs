using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using LogisticApi.DTO;
using LogisticMapper;
using Microsoft.AspNetCore.Mvc;
using MySqlRepositories.Repositories;

namespace FullApi.Controllers
{
    [Route("documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository documentRepository;

        public DocumentsController(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

		[HttpPost("create")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] DocumentRequest request)
		{
			try
			{
				return Ok(await documentRepository.Create(request.ToEntity()));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("readById/{id}")]
        [ProducesResponseType(typeof(Document), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReadById([FromRoute] Guid id)
        {
            try
            {
                var resp = await documentRepository.ReadById(id);
                return Ok(resp ?? throw new InvalidOperationException($"Article #{id} not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("read/{offset}/{limit}")]
        [ProducesResponseType(typeof(Document), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Document> NewMethod([FromRoute] int offset, [FromRoute] int limit)
        {
            var collection = documentRepository.ReadCollection(offset, limit);
            await foreach (var doc in collection)
            {
                yield return doc;
            }
        }

        [HttpGet("read/market/{market}")]
        [ProducesResponseType(typeof(Document), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Document> ReadByMarket([FromRoute] string market)
        {
            var collection = documentRepository.ReadCollectionByStprage(market);
            await foreach (var doc in collection)
            {
                yield return doc;
            }
        }

        [HttpGet("read/all")]
        [ProducesResponseType(typeof(Document), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async IAsyncEnumerable<Document> ReadAll()
        {
            var collection = documentRepository.ReadAllItems();
            await foreach (var doc in collection)
            {
                yield return doc;
            }
        }

        [HttpPatch("updateById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DocumentRequest model)
        {
            try
            {
                await documentRepository.UpdateById(id, model);
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
                await documentRepository.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
