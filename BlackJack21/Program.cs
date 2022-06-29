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

using BlackJack21;
using BlackJack21.Constant;
using BlackJack21.Enums;
using BlackJack21.Helper;
using BlackJack21.Models;
using System.Reflection;

// 玩家數
int playerCnt = 2;
// 起始籌碼
int defaultChips = 100;

/// <summary>
/// 初始化
/// </summary>
void Initial()
{
    SystemInfo.Pokers = new();
    foreach (FieldInfo cardValueProp in typeof(CardValue).GetFields())
    {
        object cardValue = cardValueProp.GetValue(null)!;
        foreach (CardSuit suit in typeof(CardSuit).GetEnumValues())
        {
            SystemInfo.Pokers.Add(new Poker { CardValue = cardValueProp.Name, Suit = suit, Point = (int)cardValue, IsUse = false });
        }
    }

    SystemInfo.Players = new();
    for (int i = 0; i < playerCnt; i++)
    {
        SystemInfo.Players.Add(new() { Id = i + 1, Chips = defaultChips, Bet = 0, HandCards = new(), IsPass = false });
    }

    SystemInfo.CurrentId = SystemInfo.Players.First().Id;
    SystemInfo.TotalBet = 0;
}

/// <summary>
/// 重開一局
/// </summary>
void Restart()
{
    // 從勝者開始
    SystemInfo.CurrentId = SystemInfo.Players.Where(x => !x.IsPass).Single().Id;
    // 歸零池底
    SystemInfo.TotalBet = 0;
    // 賦歸牌庫狀態
    foreach (Poker item in SystemInfo.Pokers)
    {
        item.IsUse = false;
    }
    // 賦歸玩家狀態
    foreach (Player item in SystemInfo.Players)
    {
        item.HandCards = new();
        item.Bet = 0;
        // 沒籌碼的人不能再玩
        item.IsPass = item.Chips <= 0;
    }
}

/// <summary>
/// 印出總覽
/// </summary>
void PrintSummary()
{
    string msg = $"=====Summary=====\n" +
        $"\t池底\t|\t{SystemInfo.TotalBet}\n" +
        $"\t回合\t|\t玩家\t|\t點數\t|\t押注\t|\t爆牌機率\n";
    foreach (Player player in SystemInfo.Players)
    {
         msg += $"\t{(player.Id == SystemInfo.CurrentId ? ">" : null)}\t|\t" +
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
    SystemInfo.Pokers.Where(x => !x.IsUse);
    return 33.5;
}

/// <summary>
/// 遊戲中
/// </summary>
void Gaming()
{
    for (int i = 0; i < SystemInfo.Players.Count; i++)
    {
        Player player = SystemInfo.Players[i];
        // 略過已pass玩家
        if (player.IsPass)
        {
            continue;
        }
        // 目前回合玩家
        SystemInfo.CurrentId = player.Id;
        // 印出總覽
        PrintSummary();
        // 印出提示訊息
        PrintHint(player.HandCards);
        // 輸入押注籌碼
        Console.WriteLine($"籌碼餘額:{player.Chips}，請輸入「押注籌碼(最大值:{player.Chips})」或輸入「p」pass回合");
        SystemInfo.Cmd = CmdHelper.GetExpectedInput(
            expectedInputs: (input) => input.ToLower() == "p" || (int.TryParse(input, out int bet) && bet <= player.Chips),
            errorMsg: $"籌碼餘額:{player.Chips}，請輸入「押注籌碼(最大值:{player.Chips})」或輸入「p」pass回合"
        );
        if (SystemInfo.Cmd.ToLower() == "p")
        {
            player.IsPass = true;
        }
        else
        {
            // 預期一定是數字,故不做安全轉換,若會報錯,表示程式有瑕疵
            int bet = Convert.ToInt32(SystemInfo.Cmd);
            player.Chips -= bet;
            player.Bet += bet;
            SystemInfo.TotalBet += bet;

            player.Gacha(SystemInfo.Pokers);
            if (player.TotalPoint > 21)
            {
                player.IsPass = true;
                Console.WriteLine($"點數共{player.TotalPoint}，超過21點，您已出局");
            }
        }

        // 檢查是否只剩下一位玩家尚未pass
        bool isRoundOver = SystemInfo.Players.Where(x => !x.IsPass).Count() == 1;
        if (isRoundOver)
        {
            // 發放池底
            Player winer = SystemInfo.Players.Where(x => !x.IsPass).Single();
            winer.Chips += SystemInfo.TotalBet;
            Console.WriteLine($"回合結束，勝者:玩家{winer.Id}，獲得籌碼{SystemInfo.TotalBet}");

            // 決定接下來流程
            // 檢查是否只剩一位有籌碼,若只剩一位有籌碼,則限制只能「重新開始(i)」or「結束遊戲(q)」
            bool isOnlyOneHasChips = SystemInfo.Players.Where(x => x.Chips > 0).Count() == 1;
            if (isOnlyOneHasChips)
            {
                Console.WriteLine("i:重新開始\tq:結束遊戲");
                SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                    expectedInputs: (input) => new[] { "i", "q" }.Contains(input.ToLower()),
                    appendErrorMsg: "i:重新開始\tq:結束遊戲"
                );
            }
            else
            {
                Console.WriteLine("i:重新開始\tr:重開一局\tq:結束遊戲");
                SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                    expectedInputs: (input) => new[] { "i", "r", "q" }.Contains(input.ToLower()),
                    appendErrorMsg: "i:重新開始\tr:重開一局\tq:結束遊戲"
                );
            }
            // 返回狀態機跑流程
            return;
        }
        else
        {
            // 繼續遊戲
            SystemInfo.Cmd = "g";
        }
    }
}

/* ============================== *
 * ==========主程式入口========== *
 * ============================== */
SystemInfo.Cmd = "i";
while (SystemInfo.Cmd.ToLower() != "q")
{
    Action action = SystemInfo.Cmd.ToLower() switch
    {
        // 重新開始
        "i" => new Action(() => { Initial(); SystemInfo.Cmd = "g"; }),
        // 重開一局
        "r" => new Action(() => { Restart(); SystemInfo.Cmd = "g"; }),
        // 遊戲中
        "g" => new Action(() => { Gaming(); }),
        // 指令錯誤
        _ => new Action(() =>
        {
            Console.WriteLine("輸入格式錯誤，請重新輸入");
            SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                expectedInputs: (input) => new[] { "i", "r" }.Contains(input.ToLower())
            );
        })
    };
    action.Invoke();
}
Console.WriteLine("遊戲結束");