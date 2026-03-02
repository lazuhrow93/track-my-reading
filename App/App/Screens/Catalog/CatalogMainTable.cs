using Spectre.Console;

namespace App.Screens.Catalog;

public static class CatalogMainTableDescriptor
{
    public static TableColumn[] Columns() => [
        new TableColumn("ID"),
        new TableColumn("Title"),
        new TableColumn("Author"),
        new TableColumn("Status")
        ];
}
