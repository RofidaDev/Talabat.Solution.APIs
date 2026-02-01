

namespace Talabat.Core.Entities.Order_Aggregate
{
     public class Address  //not table in DB
        //this address can be modified but user address not
    {  //default address is user's address
        public Address()  //so it can create obj from this class without send attributes to add migration
        {
            
        }
        public Address(string fName, string lName, string street, string city, string country)
        {
            FirstName = fName;
            LastName = lName;
            Street = street;
            City = city;
            Country = country;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
