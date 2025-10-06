using System.ComponentModel.DataAnnotations;

namespace zenBeat.Models.History
{
    public class StepHistory
    {
        [Key]
        public int Id { get; set; }

        public int StepId { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(200)]
        public string FromValue { get; set; } = string.Empty;

        [StringLength(200)]
        public string ToValue { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        [StringLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
    }
}