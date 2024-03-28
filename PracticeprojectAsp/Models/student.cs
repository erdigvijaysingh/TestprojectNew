using System.ComponentModel.DataAnnotations;

namespace PracticeprojectAsp.Models
{
    public class student
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string standard { get; set; }
        public string Address { get; set; }

    }
}
