namespace WebTest.VWModels.Description
{
    public class MedicineInfo
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; } = null!;
        public string Factory { get; set; }
        public decimal Dose { get; set; }
        public int InStock { get; set; }
        public string TradeName { get; set; } = null!;
    }
}
