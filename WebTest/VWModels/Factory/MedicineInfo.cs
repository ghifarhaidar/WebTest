namespace WebTest.VWModels.Factory
{
    public class MedicineInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Category { get; set; }
        public decimal Dose { get; set; }
        public string ActiveSubstance { get; set; }
        public int InStock { get; set; }
        public string TradeName { get; set; } = null!;
    }
}
