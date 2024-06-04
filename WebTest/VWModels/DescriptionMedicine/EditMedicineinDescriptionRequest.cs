namespace WebTest.VWModels.DescriptionMedicine
{
    public class EditMedicineinDescriptionRequest
    {
        public int DescriptionMedicineId { get; set; }
        public int DescriptionId { get; set; }
        public int MedicineId { get; set; }
        public MedicineInfo Medicine { get; set; }
    }
}
