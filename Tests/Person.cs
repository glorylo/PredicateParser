using System.Diagnostics;

namespace Tests
{
    public class Address
    {
        public string Street  { get; set; }
        public int SuiteNumber {get ; set; }
        public string City {get; set;}
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public string PostalCode { get; set; }
        public bool HasSiblings { get; set; }
        public bool HasCar { get; set; }
        public Address Address { get; set; }

        public void Inspect()
        {
            Trace.WriteLine("First Name:  " + FirstName);
            Trace.WriteLine("Last Name:  " + LastName);
            Trace.WriteLine("Age:  " + Age);
            Trace.WriteLine("Salary:  " + Salary);
            Trace.WriteLine("Postal Code:  " + PostalCode);
            Trace.WriteLine("Has Sibling:  " + HasSiblings);
            Trace.WriteLine("Has Car:  " + HasCar);
            Trace.WriteLine("Address Street Name: " + Address.Street);
            Trace.WriteLine("Address Suite Number: " + Address.SuiteNumber);
            Trace.WriteLine("Address City:  " + Address.City);
        }
    }

    public static class People
    {
        static public Person John = new Person
        {
            FirstName = "John",
            LastName = "Smith",
            Age = 60,
            Salary = 2300.50,
            PostalCode = "V5H0A7",
            HasCar = false,
            HasSiblings = true,
            Address = new Address
            {
                Street = "123 Robson Street",
                City = "Vancouver"
            }
        };

    } 
}