using System.Text;

namespace BlackJack21.Extensions;

public static class StringExtension
{
    /// <summary>
    /// 文字置中
    /// </summary>
    /// <param name="input">字串</param>
    /// <param name="width">寬度</param>
    /// <param name="paddingChar">補齊字元</param>
    /// <returns></returns>
    public static string ToAlignCenter(this string? input, int width, char paddingChar = ' ')
    {
        StringBuilder result = new();
        if (input == null)
        {
            result.Append(paddingChar, width);
        }
        else
        {
            result.Append(paddingChar, (width - input.Length) / 2);
            result.Append(input);
            result.Append(paddingChar, width - result.Length);
        }
        return result.ToString();
    }
}
