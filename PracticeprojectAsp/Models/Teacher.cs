using System.ComponentModel.DataAnnotations;

namespace PracticeprojectAsp.Models
{
    public class Teacher
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string qualification { get; set; }
        public string Address { get; set; }
    }
}
