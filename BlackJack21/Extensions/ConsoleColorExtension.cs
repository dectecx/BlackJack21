namespace BlackJack21.Extensions;

public static class ConsoleColorExtensions
{
    /// <summary>
    /// Writes the specified string value to the standard output stream.
    /// </summary>
    /// <example>
    /// ConsoleColor.Green.Write("The task executed successfully");
    /// </example>
    /// <param name="foregroundColor">Foreground color.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="cursorPosition">The row position of the cursor.</param>
    public static void Write(this ConsoleColor foregroundColor, string value, int? cursorPosition = null)
    {
        if (cursorPosition != null)
        {
            Console.SetCursorPosition(0, cursorPosition.Value);
        }

        ConsoleColor originalForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.Write(value);
        Console.ForegroundColor = originalForegroundColor;
    }

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <example>
    /// ConsoleColor.Green.WriteLine("The task executed successfully");
    /// </example>
    /// <param name="foregroundColor">Foreground color.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="cursorPosition">The row position of the cursor.</param>
    public static void WriteLine(this ConsoleColor foregroundColor, string value, int? cursorPosition = null)
    {
        if (cursorPosition != null)
        {
            Console.SetCursorPosition(0, cursorPosition.Value);
        }

        ConsoleColor originalForegroundColor = Console.ForegroundColor;        
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(value);
        Console.ForegroundColor = originalForegroundColor;
    }
}
