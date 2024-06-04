namespace WebTest.VWModels.Patient
{
    public class ShowDescriptionsResponse
    {
        public int PatientId { get; set; }
        public String Name { get; set; } = null;
        public List<DescriptionInfo> Descriptions { get; set; } = new();
    }
}
