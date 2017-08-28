using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class FavoriteFoodModel
    {

        [Metadata("Favorite Food", "Food most favored in the selected region", VisibilityType.Important)]
        public string FavoriteFood { get; set; }
    }
}