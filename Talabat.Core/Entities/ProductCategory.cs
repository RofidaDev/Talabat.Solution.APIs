

namespace Talabat.Core.Entities
{
    public class ProductCategory:BaseEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]  //no identity
        //public int Id { get; set; }
        public string Name { get; set; }
    }
}
