using Spectre.Console;

namespace App.Renderings;

public class MySelectionPrompt
{
    private static Color _defaultBorderColor = Color.Grey;
    private static string _defaultSelectedColor = "bold green";

    private string _title { get; set; } = string.Empty;

    private Color _borderColor { get; set; }

    private string _selectedColor { get; set; }

    private string _instructions { get; set; }

    private Choice[] _choices { get; set; } = [];

    public MySelectionPrompt()
    {
        _borderColor = _defaultBorderColor;
        _selectedColor = _defaultSelectedColor;
        _instructions = $"[{_borderColor}]Use ↑↓ to navigate, Enter to select[/]";
    }


    private string Title => _title;

    private Color BorderColor => _borderColor;

    private string SelectedColor => _selectedColor;

    private string Instructions => _instructions;

    private Choice[] Choices => _choices;

    public MySelectionPrompt WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public MySelectionPrompt WithChoices(string[] choices)
    {
        _choices = choices.Select((c, i) => new Choice(c, i)).ToArray();
        return this;
    }

    public async Task<string> LiveRender()
    {
        var cursor = new Cursor(false, 0);
        await AnsiConsole.Live(Update(cursor))
            .StartAsync(async ctx =>
            {
                while (Continue(cursor))
                {
                    ctx.UpdateTarget(Update(cursor));
                    cursor = UpdateCursor(cursor);
                }
            });
        return Choices[cursor.Position].DisplayText;
    }

    private Panel Update(Cursor cursor)
    {
        var choicesRendered = new Rows(Choices.Select(c => FormatRow(c, cursor)));
        var rows = new Rows(choicesRendered,
            new Markup(string.Empty),
            new Markup(Instructions));

        return new Panel(rows)
            .Header(Title)
            .BorderColor(BorderColor)
            .Padding(2, 2)
            .Expand();
    }

    private Markup FormatRow(Choice choice, Cursor cursor)
    {
        if (choice.Position == cursor.Position)
        {
            return new Markup($"[{SelectedColor}]{Markup.Escape(choice.DisplayText)}[/]");
        }
        else
        {
            return new Markup(Markup.Escape(choice.DisplayText));
        }
    }

    private Cursor UpdateCursor(Cursor cursor)
    {
        int maxCursor = _choices.Length - 1;
        var key = Console.ReadKey(intercept: true);

        if (key.Key == ConsoleKey.UpArrow)
        {
            return cursor with
            {
                Position = Math.Max(0, cursor.Position - 1)
            };
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            return cursor with
            {
                Position = Math.Min(maxCursor, cursor.Position + 1)
            };
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            return cursor with
            {
                Done = true
            };
        }
        else
        {
            return cursor;
        }
    }

    private bool Continue(Cursor cursor)
    {
        return !cursor.Done
            && cursor.Position >= 0 
            && cursor.Position < _choices.Length;
    }

    private record struct Choice(string DisplayText, int Position);

    private record struct Cursor(bool Done, int Position);
}
