using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolSystem
{
    [Serializable]
    public class Student : SchoolMember 
    {
        public enum Register
        {
            Present,
            Absent,
            Unkown
        }

        public Register Attendance {get; set; }


        public Student(int id, Tuple<string, string> name, GenderType gender, DateTime dob)
        {

            ID = id;
            Name = name;                         
            Gender = gender;
            DOB = dob;
            Age = GetAgeFromDOB(dob);
            Attendance = Register.Unkown;
        }


        public override void Display()
        {
            base.Display();
            Console.WriteLine("Role: Student");
            Console.WriteLine($"Attendance: {Attendance}\n");
        }
    }
}
