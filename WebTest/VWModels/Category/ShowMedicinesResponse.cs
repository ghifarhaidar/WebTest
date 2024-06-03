namespace WebTest.VWModels.Category
{
    public class ShowMedicinesResponse
    {
        public int FactoryId { get; set; }
        public String Name { get; set; } = null;
        public List<MedicineInfo> Medicines { get; set; } = new();
    }
}
