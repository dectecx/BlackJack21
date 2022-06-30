namespace BlackJack21.Extensions;

public static class IntExtension
{
    /// <summary>
    /// 文字置中
    /// </summary>
    /// <param name="input">字串</param>
    /// <param name="width">寬度</param>
    /// <param name="paddingChar">補齊字元</param>
    /// <returns></returns>
    public static string ToAlignCenter(this int? input, int width, char paddingChar = ' ')
    {
        string inputStr = input?.ToString() ?? "";
        string result = inputStr.ToAlignCenter(width, paddingChar);
        return result;
    }

    /// <summary>
    /// 文字置中
    /// </summary>
    /// <param name="input">字串</param>
    /// <param name="width">寬度</param>
    /// <param name="paddingChar">補齊字元</param>
    /// <returns></returns>
    public static string ToAlignCenter(this int input, int width, char paddingChar = ' ')
    {
        string inputStr = input.ToString();
        string result = inputStr.ToAlignCenter(width, paddingChar);
        return result;
    }
}
