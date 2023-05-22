using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NewSchoolSystem
{
    [Serializable]
    public class School
    {
        private Hashtable members {get; set;} 

        
        public School()
        {
            members = new Hashtable();
        }

        public void DisplayAll<T>() where T : SchoolMember
        {

            if (members.Values.OfType<T>().Count() == 0)
            {
                Console.WriteLine("There are no Members to Display.");
            }
            else
            {
                Console.Clear();

                if (typeof(T) == typeof(Staff))
                {
                    Console.WriteLine("STAFF\n" + new string('=', 20));
                } 
                else if(typeof(T) == typeof(Student))
                {
                    Console.WriteLine("STUDENTS\n" + new string('=', 20));
                }
                       
                foreach (T user in members.Values.OfType<T>())
                {
                    Console.WriteLine(new string('-', 20));
                    user.Display(); 
                    Console.WriteLine(new string ('-', 20));
                }
            }

            
        }

        public void LookupMember<T>() where T :SchoolMember
        {
            List<string> lookupOptions = new List<string> (){
                "ID",
                "Name",
                "Gender",
                "Age"
            };
            if (typeof(T) == typeof(Staff))
            {
                lookupOptions.Add("Role");
            }


            int lookupOption = Program.DisplayMenu(lookupOptions.ToArray(), "LOOKUP OPTIONS");

            List<T> filteredList = new();

            switch (lookupOption)
            {

                case 1:

                    int id = Program.GetUserInput<int>(input => input < 0, "Input ID to Search: ", "Invalid ID");

                    filteredList = members.Values.OfType<T>().Where(e => e.ID == id).ToList();
                    break;
                        
                case 2:
                    Tuple<string, string> name = GetNameTuple(); 

                    filteredList = members.Values.OfType<T>().Where(e => e.Name.Item1 == name.Item1 && e.Name.Item2 == name.Item2).ToList();
                    break;
                
                case 3:
                    GenderType gender = Program.GetUserInput<GenderType>
                    (
                        input => (int)input < 0 || (int)input >= Enum.GetNames(typeof(GenderType)).Length,
                        "Input Gender (Male, Female, Undefined): ",
                        "Invalid Gender."
                    );

                    filteredList = members.Values.OfType<T>().Where(e => e.Gender == gender).ToList();
                    break;

                case 4:
                    int age = Program.GetUserInput<int>(input => input < 0, "Input Age: ", "Invalid Age.");
                    filteredList = members.Values.OfType<T>().Where(e => e.Age == age).ToList();
                    break;
                case 5:

                    Staff.RoleType role = GetStaffRole();
                    filteredList = new List<T>(members.Values.OfType<Staff>().Where(e => e.Role == role).Cast<T>()); 

                    break;
                
                                    

            }

            filteredList.OrderBy(e => e.ID);

            Console.Clear();
            Console.WriteLine("FILTERED RESULTS" + "\n" + new string('=', 20));
            foreach (T user in filteredList)
            {
                Console.WriteLine(new string('-', 10));
                user.Display();
                Console.WriteLine(new string('-', 10));
            } 

        }

        private static Tuple<string, string> GetNameTuple()
        {
            string firstName = Program.GetUserInput<string>(input => input == "", "Input First Name: ", "Invalid First Name.");
            string lastName = Program.GetUserInput<string>(input => input == "", "Input Last Name: ", "Invalid Last Name.");

            return Tuple.Create(firstName, lastName);
 
        }

        private static Staff.RoleType GetStaffRole()
        {
            string[] roles = Enum.GetNames(typeof(Staff.RoleType));

            for (int i = 0; i < roles.Length; i++)
            {
                Console.WriteLine($"{i}. {roles[i]}"); 
            }

            return Program.GetUserInput<Staff.RoleType>(
                input => (int)input < 0 || (int)input >= roles.Length,
                "Input Role: ",
                "Invalid role."
            ); 
        }


        public void AddMember<T>() where T : SchoolMember
        {
            Console.Clear();
            int id = Program.GetUserInput<int>(input => input < 0 || members.ContainsKey(input), "Input ID: ", "Invalid ID.");

            Tuple<string, string> name = GetNameTuple();

            GenderType gender = Program.GetUserInput<GenderType>(input => (int)input < 0 || (int)input >= (int)Enum.GetNames(typeof(GenderType)).Length,
                "Input Gender (Male, Female, Undefined): ", "Invalid Gender");

            DateTime dob = Program.GetUserInput<DateTime>(input => input.Date >= DateTime.Today.Date, "Input Date of Birth (xx/xx/xx): ", "Invalid Date of Birth");

            if (typeof(T) == typeof(Student))
            {
                members.Add(id, new Student(id, name, gender, dob));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[+] Student Added.");
            }
            else if (typeof(T) == typeof(Staff))
            {
                string[] roles = Enum.GetNames(typeof(Staff.RoleType));

                Console.Write("\n");
                for (int i = 0; i < roles.Length; i++)
                {
                    Console.WriteLine($"{i}. {roles[i]}"); 
                } 


                Staff.RoleType role = Program.GetUserInput<Staff.RoleType>(
                    input => (int)input < 0 || (int)input > (int)Enum.GetNames(typeof(Staff.RoleType)).Length,
                    "\nInput Role: ",
                    "Invalid Role");

                members.Add(id, new Staff(id, name, gender, dob, role));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[+] {role} Added.");
            }

            Console.ForegroundColor = ConsoleColor.White;
         
            
        }
    
        public void RemoveMember<T>() 
        {
            while (true)
            {
                int removeIndex = Program.GetUserInput<int>(input => input < -1 , "Input ID to Remove (-1 to cancel): ", "Invalid ID");

                if (removeIndex == -1)
                {
                    Console.WriteLine("Returning to Menu...");
                    return; 
                }
                else if (!members.ContainsKey(removeIndex))
                {
                    Console.WriteLine("There is no member using that ID.");
                    continue;
                }

                if (typeof(T) != members[removeIndex].GetType())
                {
                    Console.WriteLine("Wrong Member type.");
                    continue; 
                }


                members.Remove(removeIndex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[-] Member Removed.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
   
            }
            
        }

        public T GetMember<T>() where T : SchoolMember
        {
            int id = Program.GetUserInput<int>
            (
                input => input < 0 || !members.ContainsKey(input),
                "Input ID: ",
                "Invalid ID."
            );

            return (T)members[id];
        }

        public void TakeAttendance() 
        {
            char registerEntry;

            Console.WriteLine("REGISTER (a - abscent, / - present)");

            foreach (Student student in members.Values.OfType<Student>())
            {
                registerEntry = Program.GetUserInput<char>(input => !(input == 'a' || input == '/'), $"{student.Name.Item1} {student.Name.Item2}: ", "Invalid entry.");

                student.Attendance = (registerEntry == '/') ? Student.Register.Present : Student.Register.Absent;
            }
        }

        public void SaveRegister()
        {
            StreamWriter file = new StreamWriter("Register.txt");

            file.WriteLine(DateTime.Now);
            file.WriteLine(new string('=', 10));
            foreach (Student student in members.Values.OfType<Student>())
            {
                file.WriteLine($"{student.Name.Item1} {student.Name.Item2}: {student.Attendance}");
                student.Attendance = Student.Register.Unkown;
            }

            file.Close();
        }
        public void Save()
        {
            
            IFormatter formatter = new BinaryFormatter();
            Stream file = new FileStream("School.bin", FileMode.Create, FileAccess.Write, FileShare.None);

            formatter.Serialize(file, this);
            file.Close();
        }

        public static School Load(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            School school = new School();

            if (File.Exists(fileName))
            {
                Stream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read); 

                try
                {
                    school = (School)formatter.Deserialize(file);
                    file.Close();
                }
                catch 
                {
                    file.Close();
                }
            }

            return school;
        }

    }
}
