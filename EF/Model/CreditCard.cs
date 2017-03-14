using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Model
{
    [Table("CreditCards")]
    public sealed class CreditCard
    {
        [Key]
        [Column("CreditCardID")]
        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string CardHolderName { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Column("EmployeeID")]
        public int? EmployeeId { get; set; }
    }
}
