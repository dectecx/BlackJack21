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
    /// <returns></returns>
    public static string GetExpectedInput(
        IEnumerable<string> expectedInputs,
        string errorMsg = "輸入格式錯誤，請重新輸入。",
        string appendErrorMsg = "")
    {
        string tmp = Console.ReadLine()!;
        while (!expectedInputs.Contains(tmp.ToLower()))
        {
            Console.WriteLine(errorMsg + appendErrorMsg);
            tmp = Console.ReadLine()!;
        }
        return tmp;
    }
}
