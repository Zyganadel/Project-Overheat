using System;
using System.Collections.Generic;
using System.IO;

namespace SDTesting.Assets.Script.Managers
{
    internal class LeaderboardManager
    {
        public static string playerName = "Guest";
        static string dataFolder = "leaderboards";
        static string fileName = $"leaderboards/level{GameManager.Instance.currentMapIndex}";
        public static void SetTime(double time)
        {
            Directory.CreateDirectory(dataFolder);
            if (!File.Exists(fileName)) { File.Create(fileName); }

            List<string> list = new List<string>(File.ReadAllLines(fileName));

            list.Add($"{playerName}: {time}");

            File.WriteAllLines(fileName, list.ToArray());
        }

        public static string[] GetTimes()
        {
            Directory.CreateDirectory(dataFolder);

            string[] leaderboardLines = new string[0];

            try { leaderboardLines = File.ReadAllLines(fileName); } catch (Exception e) { File.Create(fileName).Close(); }
            return leaderboardLines;
        }
    }
}
