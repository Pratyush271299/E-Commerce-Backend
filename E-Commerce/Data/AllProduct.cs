using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce.Data
{
    public class AllProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<CartProduct>? CartProducts { get; set; }
    }
}
