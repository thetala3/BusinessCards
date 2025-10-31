using BusinessCards.Application.Dtos;
using BusinessCards.Application.Interfaces;
using BusinessCards.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCards.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuisnessCardsController : ControllerBase
    {
        private readonly ILogger<BuisnessCardsController> _logger;
        private readonly IBusinessCardsService _businessCardsService;
        public BuisnessCardsController(ILogger<BuisnessCardsController> logger, IBusinessCardsService businessCardsService)
        {
            _logger = logger;
            _businessCardsService = businessCardsService;
        }

        [HttpGet]
        [Route("GetAllCards")]
        public async Task<ActionResult<List<BusinessCardRequestDto>>> GetAllBuisnessCards()
        {
            var cards = await _businessCardsService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpGet]
        [Route("GetCard")]
        public async Task<ActionResult<BusinessCardRequestDto>> GetCard(Guid guid) {
            var card = await _businessCardsService.GetCard(guid);

            if (card is null)
                return NotFound(new { message = "Business card not found." });

            return Ok(card);
        }

        [HttpDelete]
        [Route("DeleteCard")]
        public async Task<IActionResult> DeleteCard(Guid guid, CancellationToken ct)
        {
            if (guid == Guid.Empty) return BadRequest(new { message = "Invalid id." });
            var deleted = await _businessCardsService.DeleteCardAsync(guid, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost]
        [Route("PostCard")]
        public async Task<IActionResult> AddCard(BusinessCardRequestDto card) {

            if (card is null)
                return BadRequest(new { message = "No cards provided." });

            await _businessCardsService.CreateCard(card);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
