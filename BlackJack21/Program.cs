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
/*
Line    輸出範本
0       ====================            Summary         ====================
1               池底    |       0
2               回合    |       玩家    |       點數    |       押注    |       爆牌機率
2+n-1           >       |       1       |       0       |       0       |       33.5%
2+n                     |       2       |       0       |       0       |       60.0%
3+n     {{提示訊息}}
4+n     {{使用者輸入}}
5+n     {{錯誤訊息}}
*/

using BlackJack21;
using BlackJack21.Constant;
using BlackJack21.Enums;
using BlackJack21.Extensions;
using BlackJack21.Helper;
using BlackJack21.Models;
using System.Reflection;

#region 參數設定
// 玩家數
int playerCnt = 2;
// 起始籌碼
int defaultChips = 100;
// 總畫面大小
IEnumerable<int> printRange = Enumerable.Range(0, 2 + playerCnt + 4);
// 總覽範圍
Range summaryRange = 0..(2 + playerCnt);
// 提示訊息範圍
Range hintRange = (summaryRange.End.Value + 1)..(summaryRange.End.Value + 1);
// 使用者輸入範圍
Range inputRange = (hintRange.End.Value + 1)..(hintRange.End.Value + 1);
// 錯誤訊息範圍
Range errorRange = (inputRange.End.Value + 1)..(inputRange.End.Value + 1);
#endregion

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
    // 清空總覽區塊
    CmdHelper.ConsoleClearRange(summaryRange);

    ConsoleColor.Cyan.WriteLine("===============\tSummary\t===============", summaryRange.Start.Value);
    ConsoleColor.Cyan.WriteLine($"池底|{SystemInfo.TotalBet.ToAlignCenter(8)}");
    ConsoleColor.Cyan.WriteLine("回合|玩家|籌碼餘額|手牌點數|押注|爆牌機率");
    foreach (Player player in SystemInfo.Players)
    {
        string msg = $"{(player.Id == SystemInfo.CurrentId ? ">".ToAlignCenter(4) : "".ToAlignCenter(4))}|" +
           $"{player.Id.ToAlignCenter(4)}|" +
           $"{player.Chips.ToAlignCenter(8)}|" +
           $"{player.TotalPoint.ToAlignCenter(8)}|" +
           $"{player.Bet.ToAlignCenter(4)}|";
        double p1 = CalProbability(player.HandCards);
        ConsoleColor.Cyan.Write(msg);
        ConsoleColor.Yellow.WriteLine($"{p1}%".ToAlignCenter(8));
    }
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
        // 輸入押注籌碼
        SystemInfo.Cmd = CmdHelper.GetExpectedInput(
            expectedInputsDelegate: (input) => input.ToLower() == "p" || (int.TryParse(input, out int bet) && bet <= player.Chips),
            showMsg: $"籌碼餘額:{player.Chips}，請輸入「押注籌碼(最大值:{player.Chips})」或輸入「p」pass回合",
            showRange: hintRange,
            inputRange: inputRange,
            errorRange: errorRange
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
                CmdHelper.ConsoleClearRange(errorRange);
                ConsoleColor.DarkYellow.WriteLine($"點數共{player.TotalPoint}，超過21點，您已出局", errorRange.Start.Value);
            }
        }

        // 檢查是否只剩下一位玩家尚未pass
        bool isRoundOver = SystemInfo.Players.Where(x => !x.IsPass).Count() == 1;
        if (isRoundOver)
        {
            // 發放池底
            Player winer = SystemInfo.Players.Where(x => !x.IsPass).Single();
            winer.Chips += SystemInfo.TotalBet;
            CmdHelper.ConsoleClearRange(errorRange);
            ConsoleColor.DarkGreen.WriteLine($"回合結束，勝者:玩家{winer.Id}，獲得籌碼{SystemInfo.TotalBet}", errorRange.Start.Value);

            // 顯示總覽
            PrintSummary();

            // 決定接下來流程
            // 檢查是否只剩一位有籌碼,若只剩一位有籌碼,則限制只能「重新開始(i)」or「結束遊戲(q)」
            bool isOnlyOneHasChips = SystemInfo.Players.Where(x => x.Chips > 0).Count() == 1;
            if (isOnlyOneHasChips)
            {
                SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                    expectedInputsDelegate: (input) => new[] { "i", "q" }.Contains(input.ToLower()),
                    showMsg: "i:重新開始\tq:結束遊戲",
                    showRange: hintRange,
                    inputRange: inputRange,
                    errorRange: errorRange
                );
            }
            else
            {
                SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                    expectedInputsDelegate: (input) => new[] { "i", "r", "q" }.Contains(input.ToLower()),
                    showMsg: "i:重新開始\tr:重開一局\tq:結束遊戲",
                    showRange: hintRange,
                    inputRange: inputRange,
                    errorRange: errorRange
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
            SystemInfo.Cmd = CmdHelper.GetExpectedInput(
                expectedInputsDelegate: (input) => new[] { "i", "r" }.Contains(input.ToLower()),
                showMsg: "i:重新開始\tr:重開一局\tq:結束遊戲",
                showRange: hintRange,
                inputRange: inputRange,
                errorMsg: "輸入格式錯誤，請重新輸入",
                errorRange: errorRange
            );
        })
    };
    action.Invoke();
}
CmdHelper.ConsoleClearRange(errorRange);
ConsoleColor.Yellow.WriteLine("遊戲結束", errorRange.Start.Value);