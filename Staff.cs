using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolSystem
{
    [Serializable]
    public class Staff : SchoolMember
    {
        public enum RoleType
        {
            Principle,
            Deputy,
            Teacher,
            Assistant
        }

        private RoleType Role {get; set;}

        public Staff(int id, Tuple<string, string> name, GenderType gender, DateTime dob, RoleType role)
        {
            ID = id; 
            Name = name;
            Gender = gender;
            DOB = dob;
            Age = GetAgeFromDOB(dob);
            Role = role;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Role: {Role}");
        } 

        public void EditRole()
        {
            string[] roles = Enum.GetNames(typeof(RoleType));
            RoleType newRole = (RoleType)(Program.DisplayMenu(roles, "ROLES") - 1);
            Role = newRole;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[.] Role Updated");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
    }
}
