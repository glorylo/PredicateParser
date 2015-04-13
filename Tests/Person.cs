using System.Diagnostics;

namespace Tests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public string PostalCode { get; set; }

        public void Inspect()
        {
            Trace.WriteLine("First Name:  " + FirstName);
            Trace.WriteLine("Last Name:  " + LastName);
            Trace.WriteLine("Age:  " + Age);
            Trace.WriteLine("Salary:  " + Salary);
            Trace.WriteLine("Postal Code:  " + PostalCode);
        }

    }
}