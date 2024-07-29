using Godot;
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
            List<string> list = new List<string>();

            for (int i = 0; i < leaderboardLines.Length; i++)
            {
                GD.Print(leaderboardLines[i].IndexOf(": ") + 2);
                GD.Print(leaderboardLines[i]);
                float time = float.Parse(leaderboardLines[i].Substring(leaderboardLines[i].IndexOf(": ") + 2));

                bool paste = true;
                if (list.Count != 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        float time2 = float.Parse(list[j].Substring(list[j].IndexOf(": ") + 2));
                        if (time > time2) { paste = false; list.Insert(j, leaderboardLines[i]); break; }
                    }
                }
                if (paste) { list.Add(leaderboardLines[i]); }
            }

            return list.ToArray();
        }

        public static void ListTimes(Control parent)
        {
            GD.Print(GetTimes().Length);
            foreach (string line in GetTimes())
            {
                Label label = new Label();
                parent.AddChild(label);
                label.Text = line;
            }
        }
    }
}
