using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class Product
    {
        public static IDictionary<string, object> Widget = new Dictionary<string, object>()
        {
            { "Id", "1234567" },
            { "Description", "The best widget for your household.  Works wonders" },
            { "Price", 34.99 },
            { "Inventory Status", "In Stock"},
            { "Quantity", 30 },
            { "Best Seller", true },
            { "Sale Item", false }
        };
  
        public static dynamic Car = new ExpandoObject();

        public static dynamic CreateCar()
        {
            Car.Make = "Honda";
            Car.Model = "Civic";
            Car.NumberOfDoors = 4;
            Car.Description = "This is an awesome economy car to own and save on gas";
            Car.Hybrid = false;
            Car.Price = 15000.00;
            Car.HasMp3 = true;
            Car.Year = 2002;
            dynamic dealer = new ExpandoObject();
            dealer.Name = "Joe's Honda and Import Cars";
            dealer.Distance = "0.5 kilometers";
            dealer.TopSeller = false;
            dealer.HighestSale = 51000.50;
            dealer.Rank = 15;
            dealer.PhoneNumber = "6041234567";
            Car.Dealer = dealer;
            return Car;

        }

        public static void InspectCar()
        {
            var car = Car as IDictionary<string, object>;
            Trace.WriteLine("Make: " +  car["Make"]);
            Trace.WriteLine("Model: " + car["Model"]);
            Trace.WriteLine("Doors: " + car["NumberOfDoors"]);
            Trace.WriteLine("Description: " + car["Description"]);
            Trace.WriteLine("Hybrid: " + car["Hybrid"]);
            Trace.WriteLine("Price: " + car["Price"]);
            Trace.WriteLine("HasMp3: " + car["HasMp3"]);
            Trace.WriteLine("Year: " + car["Year"]);
            var dealer = car["Dealer"] as IDictionary<string,object>;
            Trace.WriteLine("Dealer: " + dealer["Name"]);
            Trace.WriteLine("Dealer Distance: " + dealer["Distance"]);
            Trace.WriteLine("Dealer Top Seller: " + dealer["TopSeller"]);
            Trace.WriteLine("Dealer Highest Sale: " + dealer["HighestSale"]);
            Trace.WriteLine("Dealer Rank: " + dealer["Rank"]);
            Trace.WriteLine("Dealer Phone Number: " + dealer["PhoneNumber"]);
          
        }

    }
}
