using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolSystem
{

    public enum GenderType
    {
        Male,
        Female,
        Undefined
    }



    [Serializable]
    public abstract class SchoolMember 
    {
        
        public int ID {get; protected set;}
        public Tuple<string, string> Name {get;  protected set;}

        public GenderType Gender {get; protected set;} 

        public int Age {get; protected set;}

        public DateTime DOB {get; protected set;}


        public virtual void Display()
        {
            Console.WriteLine($"ID: {ID}");
            Console.WriteLine($"Name: {Name.Item1} {Name.Item2}");
            Console.WriteLine($"Gender: {(GenderType)Gender}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"DOB: {DOB.ToShortDateString()}");
        }

        protected static int GetAgeFromDOB(DateTime dateOfBirth)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(dateOfBirth.ToString("yyyyMMdd"));
            return (now - dob) / 10000;
        }

         
    }
}
