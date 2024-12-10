using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dotnet.Data;
using api_dotnet.Dtos.Stock;
using api_dotnet.Helpers;
using api_dotnet.Interfaces;
using api_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext context;

        public StockRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await context.Stocks.FirstOrDefaultAsync(item => item.Id == id);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }

            context.Stocks.Remove(stock);
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var stocks = context.Stocks.Include(item => item.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(item => item.CompanyName.ToLower().Contains(queryObject.CompanyName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(item => item.Symbol.ToLower().Contains(queryObject.Symbol.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.isDecsending ? stocks.OrderByDescending(item => item.Symbol) : stocks.OrderBy(item => item.Symbol);
                }
            }

            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await context.Stocks.Include(item => item.Comments).FirstOrDefaultAsync(item => item.Id == id);
        }

        public Task<bool> StockExists(int id)
        {
            return context.Stocks.AnyAsync(item => item.Id == id);
        }

        public async Task<Stock> UpdateAysnc(int id, UpdateStockRequestDto updateStockRequestDto)
        {
            var existingStock = await context.Stocks.FirstOrDefaultAsync(item => item.Id == id);

            if (existingStock == null)
            {
                throw new Exception("Stock not found!");
            }

            existingStock.Symbol = updateStockRequestDto.Symbol;
            existingStock.CompanyName = updateStockRequestDto.CompanyName;
            existingStock.Industry = updateStockRequestDto.Industry;
            existingStock.LastDiv = updateStockRequestDto.LastDiv;
            existingStock.Purchase = updateStockRequestDto.Purchase;
            existingStock.MarketCap = updateStockRequestDto.MarketCap;

            await context.SaveChangesAsync();

            return existingStock;
        }
    }
}