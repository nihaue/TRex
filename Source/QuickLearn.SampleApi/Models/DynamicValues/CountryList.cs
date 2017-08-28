namespace QuickLearn.SampleApi.Models
{
    public class CountryList
    {

        public CountryItem[] Countries { get; set; }

    }

    public class CountryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}