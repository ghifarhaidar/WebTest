namespace WebTest.VWModels.Patient
{
    public class ShowDescriptionsResponse
    {
        public int PatientId { get; set; }
        public List<DescriptionInfo> Descriptions { get; set; } = new();
    }
}
