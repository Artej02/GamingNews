namespace GamingWeb.Custom.Models.Product
{
    public class ProductParentViewModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public int CEFTA { get; set; }
        public int MSA { get; set; }
        public int TRMTL { get; set; }
        public double TVSH { get; set; }
        public string Excise { get; set; }
        public int? ParentId { get; set; }
    }
}
