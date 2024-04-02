using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name{ get; set; }
        public DateTime CreatedDate{ get; set; }
    }
}
