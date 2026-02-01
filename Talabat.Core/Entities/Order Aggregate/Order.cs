

namespace Talabat.Core.Entities.Order_Aggregate
{
    //Complex object
    public class Order:BaseEntity
    { //Accessable Empty Parameterless Constructor must be Exist
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            PaymentIntentId = paymentIntentId;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress  { get; set; } //not NP as Address not a table
        //Order-Address is (1 to 1) => 1 table
        //Address will mapped here (Order)
        public int? DeliveryMethodId { get; set; }  //FK  //or
        //public DeliveryMethod? DeliveryMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }  //NP [one]
        //it seems [ 1 to 1] but it is really [one to many]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();  //NP [many]
        public decimal SubTotal { get; set; }  //Price(Products)*quantity
        //Deliverd attiebute:(3 ways)
        //[NotMapped]
        //public decimal Total => SubTotal + DeliveryMethod.Cost; //1
        //[NotMapped]
        //public decimal Total { get { return SubTotal + DeliveryMethod.Cost; } //2
        public decimal GetTotal() //3
        {   //total prop in DTO will read its value from this method
            return SubTotal + DeliveryMethod.Cost;
        }
        public string PaymentIntentId { get; set; }


        //Note: All relationships mapped to [one to many]
        //if I want [1 to 1] add unique constrain

    }
}
