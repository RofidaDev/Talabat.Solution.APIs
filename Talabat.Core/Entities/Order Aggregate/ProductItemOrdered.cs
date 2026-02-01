

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered  //Not a table in DB
    {
        public ProductItemOrdered()
        {
            
        }
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        //if I have some properties specify one thing 
        //make encapsulation in specific type
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
