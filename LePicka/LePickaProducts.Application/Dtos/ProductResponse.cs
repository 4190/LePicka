namespace LePickaProducts.Application.Dtos
{
    public class ProductResponse
    {
        public ProductDto Product { get; set; }
        public bool IsSucceeded { get; set; }
        public List<string> Errors { get; set; }
    }
}
