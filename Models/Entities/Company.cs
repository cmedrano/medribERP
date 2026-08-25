using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PresupuestoMVC.Models.Entities
{
    [Table("company")]
    public class Company
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; }

        [Column("street")]
        public string Street { get; set; }

        [Column("street_number")]
        public int StreetNumber { get; set; }

        [Column("floor_or_apartment")]
        public string? FloorOrApartment { get; set; }

        [Column("locality")]
        public string Locality { get; set; }

        [Column("province")]
        public string Province { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("zip_code")]
        public int CP {  get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("cuit")]
        public string CUIT {  get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
