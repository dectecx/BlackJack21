/*
設計可以讓兩個玩家或一個人跟電腦玩的撲克牌「BlackJack 21 點」遊戲。
⚫ 假設電腦與玩家的規則完全相同，沒有莊家。
    程式從一組 52 張的撲克牌中輪流發牌給兩位玩家，發出的牌面花色點數，以及目前累計點數都需顯示在螢幕上。
    在詢問玩家是否要再拿牌後由程式再跑亂數發牌。擁有最高點數的玩家獲勝，其點數必須等於或低於 21 點；超過 21 點的玩家稱為爆牌（Bust）。
    2 點至 10 點的牌以牌面的點數計算，J、Q、K 每張為 10 點。
    A 可記為 1 點或 11 點，而 2-10 則按牌面點數算，若玩家會因 A 而爆牌則 A 可算為 1 點 (規則說明部分取自 Wiki)。
⚫ 請在詢問玩家是否要再拿牌前，計算在當下拿牌會發生爆牌的機率(P1)，以及當下拿牌可以得到 21 點(含)或以下的機率(P2)，以提供玩家參考。
    若只有一人玩，電腦擔任二號玩家的部分就以 P2>P1 作為其是否加牌的決策。
⚫ 以迴圈詢問玩家，在一局結束後是否要再重開一局。可加入賭金的設計，讓遊戲更接近現實玩法。
*/

using System.Reflection;

// 牌庫
List<Poker> pokers = null!;
// 玩家數
int playerCnt = 2;
// 玩家清單
List<Player> players = null!;
// 池底
int totalBet = 0;
// 命令暫存
string cmd = "i";

/// <summary>
/// 初始化
/// </summary>
void Initial()
{
    pokers = new();
    foreach (FieldInfo cardValueProp in typeof(CardValue).GetFields())
    {
        object cardValue = cardValueProp.GetValue(null)!;
        foreach (CardSuit suit in typeof(CardSuit).GetEnumValues())
        {
            pokers.Add(new Poker { Card = cardValueProp.Name, Suit = suit, Point = (int)cardValue, IsUse = false });
        }
    }

    players = new();
    for (int i = 0; i < playerCnt; i++)
    {
        players.Add(new() { Id = i + 1, Chips = 100, Bet = 0, HandCards = new(), IsPass = false });
    }
}

/// <summary>
/// 重開一局
/// </summary>
void Restart()
{
    foreach (Poker item in pokers)
    {
        item.IsUse = false;
    }
    foreach (Player item in players)
    {
        item.HandCards = new();
        item.Bet = 0;
        item.IsPass = item.Chips <= 0;
    }
}

/// <summary>
/// 印出總覽
/// </summary>
void PrintSummary()
{
    string msg = $"=====Summary=====\n" +
        $"\t池底\t|\t{totalBet}\n" +
        $"\t回合\t|\t玩家\t|\t點數\t|\t押注\t|\t爆牌機率\n";
    foreach (Player player in players)
    {
         msg += $"\t{(player != null ? ">" : null)}\t|\t" +
            $"{player.Id}\t|\t" +
            $"{player.TotalPoint}\t|\t" +
            $"{player.Bet}\t|\t" +
            $"{0.0}\n";
    }
    Console.WriteLine(msg);
}

/// <summary>
/// 印出提示訊息
/// </summary>
void PrintHint(List<Poker> handCards)
{
    double p1 = CalProbability(handCards);
    string msg = $"爆牌的機率(P1):{p1}%\t不爆牌的機率(P2):{100-p1}%\t數字:押注籌碼 p:Pass";
    Console.WriteLine(msg);
}

/// <summary>
/// 計算機率
/// </summary>
double CalProbability(List<Poker> handCards)
{
    // TODO: 待完成功能
    pokers.Where(x => !x.IsUse);
    return 33.5;
}

while (cmd.ToLower() != "q")
{
    // 重新開始
    if (cmd.ToLower() == "i")
    {
        Initial();
        cmd = "";
    }
    // 重開一局
    if (cmd.ToLower() == "r")
    {
        Restart();
        cmd = "";
    }
    // 檢查是否剩下一位
    if (players.Where(x => !x.IsPass).Count() == 1)
    {
        Player player = players.Where(x => !x.IsPass).Single();
        Console.WriteLine($"回合結束，勝者:玩家{player.Id}，獲得籌碼{totalBet}");
        player.Chips += totalBet;
        player.Bet = 0;
        totalBet = 0;
        
        Console.WriteLine("i:重新開始\tr:重開一局\tq:結束遊戲");
        cmd = Console.ReadLine()!;
        continue;
    }

    for (int i = 0; i < players.Count; i++)
    {
        Player player = players[i];
        // 略過已pass玩家
        if (player.IsPass)
        {
            continue;
        }

        PrintSummary();
        PrintHint(player.HandCards);
        while (true)
        {
            cmd = Console.ReadLine()!;
            if (int.TryParse(cmd, out int bet) && bet <= player.Chips)
            {
                player.Chips -= bet;
                player.Bet += bet;
                totalBet += bet;

                player.Gacha(pokers);
                if (player.TotalPoint > 21)
                {
                    player.IsPass = true;
                    Console.WriteLine($"點數共{player.TotalPoint}，超過21點，您已出局");
                }
                break;
            }
            else if (cmd.ToLower() == "p")
            {
                player.IsPass = true;
                break;
            }
            else
            {
                if (bet > player.Chips)
                {
                    Console.WriteLine($"籌碼不足(餘額:{player.Chips})，請重新輸入");
                }
                else
                {
                    Console.WriteLine("輸入格式錯誤，請重新輸入");
                }
                PrintHint(player.HandCards);
            }
        }
    }
}

/// <summary>
/// 牌
/// </summary>
static class CardValue
{
    public const int A = 11;
    public const int _2 = 2;
    public const int _3 = 3;
    public const int _4 = 4;
    public const int _5 = 5;
    public const int _6 = 6;
    public const int _7 = 7;
    public const int _8 = 8;
    public const int _9 = 9;
    public const int _10 = 10;
    public const int J = 10;
    public const int Q = 10;
    public const int K = 10;
}

/// <summary>
/// 花色
/// </summary>
enum CardSuit
{
    /// <summary>
    /// 梅花
    /// </summary>
    club,

    /// <summary>
    /// 方塊
    /// </summary>
    diamond,

    /// <summary>
    /// 紅心
    /// </summary>
    heart,

    /// <summary>
    /// 黑桃
    /// </summary>
    spade
}

/// <summary>
/// 撲克牌
/// </summary>
class Poker
{
    /// <summary>
    /// 牌
    /// </summary>
    public string Card { get; set; } = null!;

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

/// <summary>
/// 玩家
/// </summary>
class Player
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public int Id { get; set; }

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
    public int PointWithoutAce => HandCards?.Where(x => x.Card != nameof(CardValue.A)).Select(x => x.Point).Sum() ?? 0;

    /// <summary>
    /// Ace牌數量
    /// </summary>
    public int CountOfAce => HandCards?.Where(x => x.Card == nameof(CardValue.A)).Count() ?? 0;

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
        while (true)
        {
            int index = rand.Next(0, 51);
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