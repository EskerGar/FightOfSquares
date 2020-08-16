using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class PlayerStatistic
    {
        public static string WinsString => "Wins";
        public static string LosesString => "Loses";
        public static string DrawsString => "Draws";

        public static string NickNameString => "NickName";

        public int WinsAmount { get; private set; }
        public int LosesAmount { get; private set; }
        public int DrawsAmount { get; private set; }
        public string NickName { get; private set; }

        public PlayerStatistic()
        {
            WinsAmount = PlayerPrefs.GetInt(WinsString);
            LosesAmount = PlayerPrefs.GetInt(LosesString);
            DrawsAmount = PlayerPrefs.GetInt(DrawsString);
            NickName = PlayerPrefs.GetString(NickNameString);
        }

        public static void AddDraw() => PlayerPrefs.SetInt(DrawsString, PlayerPrefs.GetInt(DrawsString) + 1);
        public static void AddWin() => PlayerPrefs.SetInt(WinsString, PlayerPrefs.GetInt(WinsString) + 1);
        public static void AddLose() => PlayerPrefs.SetInt(LosesString, PlayerPrefs.GetInt(LosesString) + 1);
    }
}
