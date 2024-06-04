namespace WebTest.VWModels.DescriptionMedicine
{
    public class ShowMedicinesResponse
    {
        public int DescriptionId { get; set; }
        public List<VWModels.Description.MedicineInfo> Medicines { get; set; } = new();
    }
}
