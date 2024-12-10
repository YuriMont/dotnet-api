using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dotnet.Dtos.Comment;
using api_dotnet.Interfaces;
using api_dotnet.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api_dotnet.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            this.commentRepository = commentRepository;
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var comments = await commentRepository.GetAllAsync();

            var commentDto = comments.Select(item => item.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var comment = await commentRepository.GetByIdAsync(id);

                return Ok(comment.ToCommentDto());
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await stockRepository.StockExists(stockId))
            {
                return BadRequest("Socks does not exist");
            }

            var commentModel = createCommentDto.ToCommentFromCreate(stockId);
            await commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var comment = await commentRepository.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdate());

                return Ok(comment.ToCommentDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var comment = await commentRepository.DeleteAsync(id);

                return Ok(comment.ToCommentDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}