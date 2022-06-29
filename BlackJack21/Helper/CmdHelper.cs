namespace BlackJack21.Helper;

/// <summary>
/// 命令列Helper
/// </summary>
public static class CmdHelper
{
    /// <summary>
    /// 讀取使用者輸入,直到輸入內容為預期的input為止
    /// </summary>
    /// <param name="expectedInputs">預期的input</param>
    /// <param name="errorMsg">輸入非預期input時顯示的錯誤訊息</param>
    /// <returns>預期的input</returns>
    public static string GetExpectedInput(
        Func<string, bool> expectedInputs,
        string errorMsg = "輸入格式錯誤，請重新輸入。",
        string appendErrorMsg = "")
    {
        string input = Console.ReadLine()!;
        while (!expectedInputs.Invoke(input))
        {
            Console.WriteLine(errorMsg + appendErrorMsg);
            input = Console.ReadLine()!;
        }
        return input;
    }
}
