namespace WebTest.VWModels
{
    public class EditMedicineRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Dose { get; set; }
        public int ActiveSubstanceId { get; set; }
        public int InStock { get; set; }
        public int FactoryId { get; set; }
        public string TradeName { get; set; } = null!;
    }
}
