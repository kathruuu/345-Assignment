using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2
{
    public class AmbulanceStaffContext : DbContext
    {
        public AmbulanceStaffContext() : base("MySqlConnection") { }
        public DbSet<StaffMember> StaffMember { get; set; }
        public DbSet<Ambulance> Ambulance { get; set; }
    }
    [Table("StaffMember")]
    public class StaffMember
    {
        [Key]
        public string officer_ID { get; set; }
        public string first_names { get; set; }
        public string surname { get; set; }
        public string skill_level { get; set; }
        public string ambulance_ID { get; set; }
    }
    [Table("Ambulance")]
    public class Ambulance
    {
        [Key]
        public string ambulance_ID { get; set; }
        public string station { get; set; }
    }

}
