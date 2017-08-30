using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Program
    {
        static void Main()
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                bool exit = false;
                while (exit != true)
                {
                    Console.Write(":>");
                    string line = Console.ReadLine();
                    string[] input = line.Split((' '));
                    string optionSelected = input[0].ToUpperInvariant();
                    string[] list = input.Skip(1).ToArray();
                    switch (optionSelected)
                    {
                        case "ADD":
                            checkInput(list);
                            break;
                        case "REMOVE":
                            removeOption(list);
                            break;
                        case "LIST":
                            listOption(input);
                            break;
                        case "ASSIGN":
                            assignOption(list);
                            break;
                        case "UNASSIGN":
                            unassignOption(list);
                            break;
                        case "EXIT":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid command: the valid commands are ADD, REMOVE, LIST, ASSIGN, UNASSIGN and EXIT");
                            break;
                    }
                }
            }
        }

        public static void checkInput(string[] list)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                if (list.Length >= 2) //checks for the name and surname
                {
                    int ID_index = Array.FindIndex(list, id => id.Equals("id", StringComparison.InvariantCultureIgnoreCase));
                    if (ID_index > -1) //checks for the "ID" keyword 
                    {
                        try
                        {
                            string officerID = list[ID_index + 1];
                            int n;
                            bool isNumeric = int.TryParse(officerID, out n);
                            if (isNumeric == true)
                            {
                                if (officerID.Length == 6)
                                {
                                    var exist = db.StaffMember.Any(i => i.officer_ID == officerID);
                                    if (exist == false) //checks if officer ID exist in the 
                                    {
                                        int AS_index = Array.FindIndex(list, a => a.Equals("as", StringComparison.InvariantCultureIgnoreCase));
                                        if (AS_index > -1) // checks for the AS keyword 
                                        {
                                            string skill = list[AS_index + 1].ToLowerInvariant(); //accepts any capitalisation
                                            if (skill == "basic" || skill == "intermediate" || skill == "advanced")
                                            {
                                                int end = ID_index - 1;
                                                string first_names = list[0];
                                                for (int i = 1; i < end; i++)
                                                {
                                                    first_names = first_names + " " + list[i];

                                                }    
                                                string[] input = { list[ID_index + 1], first_names, list[ID_index - 1], list[AS_index + 1], "None" };
                                                //Console.WriteLine("officerID: {0}, first names: {1}, surname: {2}, skill level: {3}, ambulanceID: {4}", input[0], input[1], input[2], input[3], input[4]);
                                                addToDataBase(input);  
                                            }
                                            else
                                            {
                                                Console.WriteLine("The skill level for the officer must be one of basic, intermediate, or advanced");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("An officer must have a skill level (basic, intermediate, or advanced)");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("The officer ID entered already exist. Please try again.");

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("An officer must have a six digit ID number.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("The officer ID must be a six digit number");

                            }
                        }
                        catch(IndexOutOfRangeException)
                        {
                            Console.WriteLine("An officer must have a six digit ID number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("An officer must have a six digit ID number");

                    }

                }
                else
                {
                    Console.WriteLine("An officer must have a surname and at least one given name");

                }
            }
        }
        public static void addToDataBase(string[] input)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                string officer_ID = input[0];
                string first_names = input[1];
                string surname = input[2];
                string skill_level = input[3];
                StaffMember staff = new StaffMember { officer_ID = officer_ID, first_names = first_names, surname = surname, skill_level = skill_level, ambulance_ID = null };
                db.StaffMember.Add(staff);
                db.SaveChanges();
                var added = db.StaffMember.Any(i => i.officer_ID == officer_ID);
                if (added == true)
                {
                    Console.WriteLine("The ambulance officer has been added");
                }
                else
                {
                    Console.WriteLine("The ambulance officer has not been added successfully. Please try again.");
                }
                
            }
        }

        public static void removeOption(string[] list)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                try
                {
                    string officer_id = list[0];
                    int n;
                    bool isNumeric = int.TryParse(officer_id, out n);
                    if (isNumeric == true & officer_id.Length == 6)
                    {
                        var found = db.StaffMember.Any(i => i.officer_ID == officer_id);
                        if (found == true)
                        {
                            var staff = db.StaffMember.Where(s => s.officer_ID == officer_id);
                            foreach (StaffMember s in staff)
                            {
                                db.StaffMember.Remove(s);
                                db.SaveChanges();
                                var removed = db.StaffMember.Any(i => i.officer_ID == officer_id);
                                if (removed != true)
                                {
                                    Console.WriteLine("Officer {0} {1} ({2}) has been removed", s.first_names, s.surname, s.skill_level);
                                }
                                else
                                {
                                    Console.WriteLine("The officer {0} has not been removed successfully. Please try again.", officer_id);
                                    Main();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Officer ID not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a six digit officer ID number.");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Please enter a six digit officer ID number.");
                }
            }
        }

        public static void listOption(string[] input)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                if (input.Length == 1 & input[0].ToUpperInvariant() == "LIST")
                {
                    Console.WriteLine("Ambulance officer list as of {0} at {1}", DateTime.Now.ToString("dd MMM yyyy"), DateTime.Now.ToString("HH:mmtt"));

                    Console.WriteLine("Surname. " + "First Name. " + "Officer ID. " + "Skill Level. " + "Assigned Ambulance");
                    var x = db.Database.SqlQuery<StaffMember>("select * from StaffMember");
                    foreach (StaffMember s in x)
                    {
                        if (s.ambulance_ID == null)
                        {
                            s.ambulance_ID = "None";
                            Console.WriteLine(String.Format("{0}, " + "{1}, " + "{2}, " + "{3}, " + "{4}", s.surname, s.first_names, s.officer_ID, s.skill_level, s.ambulance_ID));
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}, " + "{1}, " + "{2}, " + "{3}, " + "{4}", s.surname, s.first_names, s.officer_ID, s.skill_level, s.ambulance_ID));

                        }
                    }
                    int totalOfficers = db.StaffMember.Count();
                    Console.WriteLine(string.Format("Listed {0} officers", totalOfficers));
                }
                else
                {
                    if (input[1].Length != 0)
                    {
                        string surname = input[1];
                        var found = db.StaffMember.Any(i => i.surname == surname);
                        if (found == true)
                        {
                            var result = db.StaffMember.Where(s => s.surname == surname);

                            foreach (StaffMember s in result)
                            {
                                if (s.ambulance_ID == null)
                                {
                                    s.ambulance_ID = "None";
                                    Console.WriteLine(String.Format("{0}, " + "{1}, " + "{2}, " + "{3}, " + "{4}", s.surname, s.first_names, s.officer_ID, s.skill_level, s.ambulance_ID));
                                }
                                else
                                {
                                    Console.WriteLine(String.Format("{0}, " + "{1}, " + "{2}, " + "{3}, " + "{4}", s.surname, s.first_names, s.officer_ID, s.skill_level, s.ambulance_ID));

                                }
                            }
                            int totalOfficers = result.Count();
                            Console.WriteLine(string.Format("Listed {0} officers", totalOfficers));
                        }
                        else
                        {
                            Console.WriteLine("Officer with {0} surname not found. Please try again.", surname);
                        }
                    }
                    else
                    {
                        input = new string[] { input[0] };
                        listOption(input);
                    }

                }
            }
        }

        public static void assignOption(string[] input)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                try
                {
                    string officer_id = input[0];
                    int n;
                    bool isNumeric = int.TryParse(officer_id, out n);
                    if (isNumeric == true & officer_id.Length == 6)
                    {
                        int to_index = Array.FindIndex(input, to => to.Equals("to", StringComparison.InvariantCultureIgnoreCase));
                        var found = db.StaffMember.Any(i => i.officer_ID == officer_id);
                        if (found == true)
                        {
                            if (input.Length == 3 & to_index == 1)
                            {
                                string ambulance_id = input[2];
                                var exist = db.Ambulance.Any(i => i.ambulance_ID == ambulance_id);
                                if (exist == true)
                                {
                                    foreach (Ambulance a in db.Ambulance.Where(i => i.ambulance_ID == ambulance_id))
                                    {
                                        var staff = db.StaffMember.Where(s => s.officer_ID == officer_id);
                                        foreach (StaffMember s in staff)
                                        {
                                            s.ambulance_ID = ambulance_id.ToUpperInvariant();
                                            db.SaveChanges();
                                            Console.WriteLine("The ambulance officer has been assigned to {0} at {1}", a.ambulance_ID, a.station);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Ambulance ID is missing or not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ambulance ID is missing or not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Officer ID not found");
                        }
                    }

                    else
                    {
                        Console.WriteLine("An officer must have a six digit ID number");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Please enter a six digit officer ID number."); //if user has only typed in the command and no other values, input is empty
                }
            }
        }

        public static void unassignOption(string[] input)
        {
            using (AmbulanceStaffContext db = new AmbulanceStaffContext())
            {
                try
                {
                    string officer_id = input[0];
                    int n;
                    bool isNumeric = int.TryParse(officer_id, out n);
                    if ((isNumeric == true & officer_id.Length == 6) & input.Length == 1)
                    {
                        var found = db.StaffMember.Any(i => i.officer_ID == officer_id);
                        if (found == true)
                        {
                            var staff = db.StaffMember.Where(s => s.officer_ID == officer_id);
                            foreach (StaffMember s in staff)
                            {
                                if (s.ambulance_ID != null)
                                {
                                    var station = db.Ambulance.Where(a => a.ambulance_ID == s.ambulance_ID);
                                    foreach (Ambulance a in station)
                                    {
                                        string ambulance_id = s.ambulance_ID; //need to be printed so save in a variable
                                        s.ambulance_ID = null;
                                        Console.WriteLine("The ambulance officer has been removed from {0} at {1}", ambulance_id, a.station);
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Officer is not assigned to an ambulance");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Officer ID not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("An officer must have a six digit ID number");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Please enter a six digit officer ID number."); //if user has only typed in the command and no other values, input is empty
                }
            }
            
        }
        
    }
}
                    
                
//how to capitalise input
