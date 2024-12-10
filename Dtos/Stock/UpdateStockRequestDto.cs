using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_dotnet.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Symbol must be 2 chaaracters")]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Company Name must be 2 chaaracters")]
        [MaxLength(280, ErrorMessage = "Company Name cannot be over 280 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Industry must be 5 chaaracters")]
        [MaxLength(20, ErrorMessage = "Industry cannot be over 20 characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}