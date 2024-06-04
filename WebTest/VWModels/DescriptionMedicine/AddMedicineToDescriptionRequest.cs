namespace WebTest.VWModels.DescriptionMedicine
{
    public class AddMedicineToDescriptionRequest
    {
        public int DescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int Count { get; set; }
    }
}
