﻿namespace WebTest.VWModels.Description
{
    public class ShowMedicinesResponse
    {
        public int DescriptionId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description1 { get; set; }
        public List<MedicineInfo> Medicines { get; set; } = new();
    }
}
