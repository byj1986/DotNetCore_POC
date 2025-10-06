using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zenBeat.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;

        // Foreign key
        [ForeignKey("Name")]
        public virtual Language Language { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<ProjectStepsMapping> ProjectStepsMappings { get; set; } = new List<ProjectStepsMapping>();
    }
}