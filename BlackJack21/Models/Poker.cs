using BlackJack21.Enums;

namespace BlackJack21.Models;

/// <summary>
/// 撲克牌
/// </summary>
class Poker
{
    /// <summary>
    /// 牌
    /// </summary>
    public string CardValue { get; set; } = null!;

    /// <summary>
    /// 花色
    /// </summary>
    public CardSuit Suit { get; set; }

    /// <summary>
    /// 點數
    /// </summary>
    public int Point { get; set; }

    /// <summary>
    /// 是否已被抽走
    /// </summary>
    public bool IsUse { get; set; }
}