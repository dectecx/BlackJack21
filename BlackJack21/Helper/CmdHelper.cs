using BlackJack21.Extension;

namespace BlackJack21.Helper;

/// <summary>
/// 命令列Helper
/// </summary>
public static class CmdHelper
{
    /// <summary>
    /// 讀取使用者輸入,直到輸入內容為預期的input為止
    /// </summary>
    /// <param name="expectedInputsDelegate">預期的input委派</param>
    /// <param name="showMsg">輸入前的提示訊息</param>
    /// <param name="showRange">提示訊息範圍</param>
    /// <param name="errorMsg">輸入非預期input時顯示的錯誤訊息</param>
    /// <param name="errorRange">錯誤訊息範圍</param>
    /// <returns>預期的input</returns>
    public static string GetExpectedInput(
        Func<string, bool> expectedInputsDelegate,
        string showMsg,
        Range showRange,
        string inputMsg = "> ",
        Range? inputRange = null,
        string errorMsg = "輸入錯誤！",
        Range? errorRange = null)
    {
        inputRange ??= new Range(showRange.End.Value + 1, 1);
        errorRange ??= new Range(showRange.End.Value + 2, 1);

        ConsoleClearRange(showRange);
        ConsoleColor.Gray.WriteLine(showMsg, showRange.Start.Value);
        ConsoleClearRange(inputRange.Value);
        ConsoleColor.Gray.Write(inputMsg, inputRange.Value.Start.Value);
        string input = Console.ReadLine()!;
        while (!expectedInputsDelegate.Invoke(input))
        {
            ConsoleClearRange(errorRange.Value);
            ConsoleColor.DarkRed.WriteLine(errorMsg, errorRange.Value.Start.Value);

            ConsoleClearRange(showRange);
            ConsoleColor.Gray.WriteLine(showMsg, showRange.Start.Value);
            ConsoleClearRange(inputRange.Value);
            ConsoleColor.Gray.Write(inputMsg, inputRange.Value.Start.Value);
            input = Console.ReadLine()!;
        }
        ConsoleClearRange(errorRange.Value);
        return input;
    }

    /// <summary>
    /// 清空指定範圍
    /// </summary>
    public static void ConsoleClearRange(Range range)
    {
        (int _, int originalTop) = Console.GetCursorPosition();
        // 清空指定範圍
        Console.SetCursorPosition(0, range.Start.Value);
        for (int i = 0; i < range.End.Value - range.Start.Value + 1; i++)
        {
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, originalTop);
    }
}