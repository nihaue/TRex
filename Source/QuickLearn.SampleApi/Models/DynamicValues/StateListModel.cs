namespace QuickLearn.SampleApi.Models
{
    public class StateListModel
    {

        public StateItem[] States { get; set; }

    }

    public class StateItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}