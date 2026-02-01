using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class OrderDTO
    {
        
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
       
        public AddressDTO ShippingAddress { get; set; }
    }
}
