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
    /// <param name="errorMsg">輸入非預期input時顯示的錯誤訊息</param>
    /// <returns>預期的input</returns>
    public static string GetExpectedInput(
        Func<string, bool> expectedInputsDelegate,
        string showMsg,
        string errorMsg = "輸入錯誤！")
    {
        ConsoleColor.Gray.WriteLine(showMsg);
        string input = Console.ReadLine()!;
        while (!expectedInputsDelegate.Invoke(input))
        {
            ConsoleColor.DarkRed.WriteLine(errorMsg);
            ConsoleColor.Gray.WriteLine(showMsg);
            input = Console.ReadLine()!;
        }
        return input;
    }
}
