using BlackJack21.Constant;

namespace BlackJack21.Models;

/// <summary>
/// 玩家
/// </summary>
class Player
{
    /// <summary>
    /// Index
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// 玩家名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 籌碼
    /// </summary>
    public int Chips { get; set; }

    /// <summary>
    /// 押注籌碼
    /// </summary>
    public int Bet { get; set; }

    /// <summary>
    /// 手牌
    /// </summary>
    public List<Poker> HandCards { get; set; } = null!;

    /// <summary>
    /// 總點數
    /// </summary>
    public int TotalPoint
    {
        get
        {
            int total = PointWithoutAce;
            for (int i = 0; i < CountOfAce; i++)
            {
                if (total + (int)CardValue.A <= 21)
                {
                    total += (int)CardValue.A;
                }
                else
                {
                    total += 1;
                }
            }
            return total;
        }
    }

    /// <summary>
    /// 不含Ace總點數
    /// </summary>
    public int PointWithoutAce => HandCards?.Where(x => x.CardValue != nameof(CardValue.A)).Select(x => x.Point).Sum() ?? 0;

    /// <summary>
    /// Ace牌數量
    /// </summary>
    public int CountOfAce => HandCards?.Where(x => x.CardValue == nameof(CardValue.A)).Count() ?? 0;

    /// <summary>
    /// 是否停手
    /// </summary>
    public bool IsPass { get; set; }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="pokers">牌庫</param>
    public void Gacha(IEnumerable<Poker> pokers)
    {
        Random rand = new();
        while (pokers.Any(x => !x.IsUse))
        {
            int index = rand.Next(0, 52);
            Poker gacha = pokers.ToArray()[index];
            if (!gacha.IsUse)
            {
                gacha.IsUse = true;
                this.HandCards.Add(gacha);
                break;
            }
        }
    }
}
