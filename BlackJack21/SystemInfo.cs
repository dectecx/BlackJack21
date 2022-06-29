using BlackJack21.Models;

namespace BlackJack21;

/// <summary>
/// 系統資訊
/// </summary>
internal static class SystemInfo
{
    /// <summary>
    /// 牌庫
    /// </summary>
    public static List<Poker> Pokers = null!;

    /// <summary>
    /// 玩家清單
    /// </summary>
    public static List<Player> Players = null!;

    /// <summary>
    /// 目前回合玩家
    /// </summary>
    public static int CurrentId;

    /// <summary>
    /// 池底
    /// </summary>
    public static int TotalBet = 0;

    /// <summary>
    /// 命令暫存
    /// </summary>
    public static string Cmd = "i";
}
