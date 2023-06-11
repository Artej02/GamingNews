

namespace GamingWeb.Custom.Models.Kendo.DropDownTree
{
    public class ItemViewModel
    {
        public int Value { get; set; }

        public string Text { get; set; }

        public int? ParentId { get; set; }

        public bool HasChildren { get; set; }
    }
}
