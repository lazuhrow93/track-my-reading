namespace App.FirstScreen;

public static class MenuOptions
{
    public const string ViewCatalog = "View Catalog";

    public const string CharacterTree = "Character Tree";

    public static string[] All => new[]
    {
        ViewCatalog,
        CharacterTree
    };
}
