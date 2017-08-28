using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class FavoriteFoodVoteModel
    {
        [DynamicValueLookup("GetStates", "country={country}", "States", "Id", "Name")]
        public string State { get; set; }

        [Metadata("Favorite Food", "Food most favored in the selected region", VisibilityType.Important)]
        public string FavoriteFood { get; set; }
    }
}