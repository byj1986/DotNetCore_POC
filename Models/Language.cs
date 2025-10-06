using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zenBeat.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("en-US")]
        [StringLength(200)]
        public string EnUS { get; set; } = string.Empty;

        [Column("zh-CN")]
        [StringLength(200)]
        public string ZhCN { get; set; } = string.Empty;

        [Column("zh-TW")]
        [StringLength(200)]
        public string ZhTW { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Step> Steps { get; set; } = new List<Step>();
    }
}