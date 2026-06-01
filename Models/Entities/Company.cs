using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PresupuestoMVC.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Street { get; set; }     

        public int StreetNumber { get; set; }

        public string? FloorOrApartment { get; set; }

        public string Locality { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public int CP {  get; set; }

        public string Phone { get; set; }

        public string CUIT {  get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
