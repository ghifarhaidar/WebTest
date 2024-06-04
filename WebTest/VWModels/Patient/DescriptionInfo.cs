namespace WebTest.VWModels.Patient
{
    public class DescriptionInfo
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description1 { get; set; }
    }
}
