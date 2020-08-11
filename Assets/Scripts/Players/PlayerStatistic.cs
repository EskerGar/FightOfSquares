using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class PlayerStatistic
    {
        public static string Wins => "Wins";
        public static string Loses => "Loses";
        public static string Draws => "Draws";

        public static string NickNames => "NickName";

        public int WinsAmount { get; private set; }
        public int LosesAmount { get; private set; }
        public int DrawsAmount { get; private set; }
        public string NickName { get; private set; }

        public PlayerStatistic()
        {
            WinsAmount = PlayerPrefs.GetInt(Wins);
            LosesAmount = PlayerPrefs.GetInt(Loses);
            DrawsAmount = PlayerPrefs.GetInt(Draws);
            NickName = PlayerPrefs.GetString(NickNames);
        }
        
    }
}
