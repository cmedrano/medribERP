namespace PresupuestoMVC.Models.ViewModels
{
    public class CreateCloneViewRequest
    {
        public int OrigenAnio { get; set; }
        public int OrigenMes { get; set; }
        public int DestinoAnio { get; set; }
        public int DestinoMes { get; set; }
    }
}
