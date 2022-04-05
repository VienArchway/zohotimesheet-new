using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService service;

        public ItemController(IItemService service)
        {
            this.service = service;
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public async Task<IActionResult> SearchAsync([FromBody] ItemSearchParameter parameter)
        {
            var result = await service.SearchAsync(parameter).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("search-by-projectId/{projectId}")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public async Task<IActionResult> SearchByProjectIdAsync(string projectId, [FromBody] ItemSearchParameter parameter)
        {
            var result = await service.SearchByProjectIdAsync(projectId, parameter).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] UpdateItemStatusParameter parameter)
        {
            await service.UpdateStatusAsync(parameter).ConfigureAwait(false);
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] ItemSaveParameter parameter)
        {
            var result = await service.CreateAsync(parameter).ConfigureAwait(false);
            return new CreatedResult(string.Empty, result);
        }

        [HttpPost("create-sub-item")]
        public async Task<IActionResult> CreateSubItemAsync([FromBody] ItemSaveParameter parameter)
        {
            var result = await service.CreateSubItemAsync(parameter).ConfigureAwait(false);
            return new CreatedResult(string.Empty, result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] ItemSaveParameter parameter)
        {
            await service.UpdateAsync(parameter).ConfigureAwait(false);
            return NoContent();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteItemParameter parameter)
        {
            await service.DeleteAsync(parameter).ConfigureAwait(false);
            return NoContent();
        }
}