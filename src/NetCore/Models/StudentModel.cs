using System.ComponentModel.DataAnnotations;

namespace NetCore.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student Name Required")]
        [Display(Name = "Student Name")]
        [MaxLength(80, ErrorMessage = "Enter Valid Name !")]
        public string Name { get; set; }
    }
}