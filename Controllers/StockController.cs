using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dotnet.Data;
using api_dotnet.Dtos.Stock;
using api_dotnet.Helpers;
using api_dotnet.Interfaces;
using api_dotnet.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IStockRepository stockRepository;

        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            this.context = context;
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = await stockRepository.GetAllAsync(queryObject);

            var stocksDto = stocks.Select(item => item.ToStockDto());

            return Ok(stocksDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound("Stock not found");
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto createStockRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var stockModel = createStockRequestDto.ToStockFromCreateDto();

            await stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var stockModel = await stockRepository.UpdateAysnc(id, updateStockRequestDto);

                return Ok(stockModel.ToStockDto());
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
                throw;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stockModel = await stockRepository.DeleteAsync(id);

                return Ok("Stock deleted successfully");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
                throw;
            }
        }
    }
}