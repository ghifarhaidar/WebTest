namespace WebTest.VWModels.Description
{
    public class ShowMedicinesResponse
    {
        public int DescriptionId { get; set; }
        public String Name { get; set; } = null;
        public List<MedicineInfo> Medicines { get; set; } = new();
    }
}
