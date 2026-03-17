using Spectre.Console;

namespace App.Screens.Books;

internal class BookDetailsMainTableDescriptor
{
    public static TableColumn[] Columns() => [
        new TableColumn("ID"),
        new TableColumn("Character Name"),
        new TableColumn("Description"),
        ];
}
