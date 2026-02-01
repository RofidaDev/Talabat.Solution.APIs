
using System.Runtime.Serialization;


namespace Talabat.Core.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]  //to save this value in DB not as int
        Pending,
        [EnumMember(Value ="PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value ="'PaymentFailed")]
        PaymentFailed
       

    }
}
