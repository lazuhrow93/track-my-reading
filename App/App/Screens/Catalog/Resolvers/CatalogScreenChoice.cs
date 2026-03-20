using Database.Entites;

namespace App.Screens.Catalog.Resolvers;

public record struct CatalogScreenChoice
{
    public Page TargetPage { get; set; }

    public IScreenInput? ScreenInput { get; set; }
}
