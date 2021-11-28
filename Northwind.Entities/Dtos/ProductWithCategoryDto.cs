using Core.Entities;

namespace Northwind.Entities.Dtos
{
    public class ProductWithCategoryDto : IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }   
    }
}