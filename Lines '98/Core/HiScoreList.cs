using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace Lines98.Core
{
    public class HiScoreList : IEnumerable
    {
        public class PlayerScore : IComparable
        {
            private readonly string _name;
            private readonly int _score;
            private readonly int _steps;
            private readonly string _time;

            public PlayerScore(string name, int score, int steps, string time)
            {
                _name = name;
                _score = score;
                _steps = steps;
                _time = time;
            }

            public string Name
            {
                get { return _name; }
            }

            public int Score
            {
                get { return _score; }
            }

            public int Steps
            {
                get { return _steps; }
            }

            public string Time
            {
                get { return _time; }
            }

            public int CompareTo(object obj)
            {
                var playerScore = (PlayerScore)obj;

                if (Score > playerScore.Score)
                    return 1;
                if (Score != playerScore.Score) 
                    return -1;
                if (Steps < playerScore.Steps)
                    return 1;
                if (Steps != playerScore.Steps)
                    return -1;
                if (Convert.ToInt32(Time.Split(':')[0]) < Convert.ToInt32(playerScore.Time.Split(':')[0]))
                    return 1;
                if (Convert.ToInt32(Time.Split(':')[0]) != Convert.ToInt32(playerScore.Time.Split(':')[0]))
                    return -1;
                if (Convert.ToInt32(Time.Split(':')[1]) < Convert.ToInt32(playerScore.Time.Split(':')[1]))
                    return 1;
                if (Convert.ToInt32(Time.Split(':')[1]) == Convert.ToInt32(playerScore.Time.Split(':')[1]))
                    return 0;
                return -1;
            }

            public static bool operator <(PlayerScore ps1, PlayerScore ps2)
            {
                return (ps1.CompareTo(ps2) < 0);
            }

            public static bool operator >(PlayerScore ps1, PlayerScore ps2)
            {
                return (ps1.CompareTo(ps2) > 0);
            }

            public static bool operator <=(PlayerScore ps1, PlayerScore ps2)
            {
                return (ps1.CompareTo(ps2) <= 0);
            }

            public static bool operator >=(PlayerScore ps1, PlayerScore ps2)
            {
                return (ps1.CompareTo(ps2) >= 0);
            }
        }

        private readonly PlayerScore[] _topList;
        private readonly int _maxLength;

        public HiScoreList()
            : this(10)
        {
        }

        public HiScoreList(int maxLength)
        {
            _maxLength = maxLength;
            _topList = new PlayerScore[10];
        }

        public void Load(string filename)
        {
            if (!File.Exists(filename)) return;
            var doc = new XmlDocument();
            doc.Load(filename);
            XmlNode root = doc.DocumentElement;
            if (root != null)
            {
                var node = root.FirstChild;
                var count = 0;
                while ((node != null) && (count < _maxLength))
                {
                    if (node.Attributes != null)
                        _topList[count] = new PlayerScore(node.Attributes["name"].Value,
                            int.Parse(node.Attributes["score"].Value), int.Parse(node.Attributes["steps"].Value), node.Attributes["time"].Value);

                    count++;
                    node = node.NextSibling;
                }
            }

            // Just to be sure that nobody tricked
            Array.Sort(_topList);
            Array.Reverse(_topList, 0, _topList.Length);
        }

        public void Save(string filename)
        {
            var doc = new XmlDocument();
            var root = doc.CreateNode(XmlNodeType.Element, "HiScore", "");
            doc.AppendChild(root);

            for (var i = 0; i < _maxLength; i++)
            {
                var playerScore = _topList[i];
                if (playerScore == null)
                    break;
                var node = doc.CreateNode(XmlNodeType.Element, "Player", "");
                var attribute = doc.CreateAttribute("name");
                attribute.Value = playerScore.Name;
                if (node.Attributes != null)
                {
                    node.Attributes.Append(attribute);
                    attribute = doc.CreateAttribute("score");
                    attribute.Value = playerScore.Score.ToString();
                    node.Attributes.Append(attribute);
                    attribute = doc.CreateAttribute("steps");
                    attribute.Value = playerScore.Steps.ToString();
                    node.Attributes.Append(attribute);
                    attribute = doc.CreateAttribute("time");
                    attribute.Value = playerScore.Time;
                    node.Attributes.Append(attribute);
                }
                root.AppendChild(node);
            }

            doc.Save(filename);
        }

        public bool IsRecord(int score, int steps, string time)
        {
            var playerScore = _topList[_maxLength - 1];

            if (playerScore == null)
                return true;
            if (score > playerScore.Score)
                return true;
            if ((score == playerScore.Score) && (steps < playerScore.Steps))
                return true;
            if ((score == playerScore.Score) && (steps == playerScore.Steps) &&
                (Convert.ToInt32(time.Split(':')[0]) < Convert.ToInt32(playerScore.Time.Split(':')[0])))
                return true;
            return (score == playerScore.Score) && (steps == playerScore.Steps) &&
                   (Convert.ToInt32(time.Split(':')[0]) == Convert.ToInt32(playerScore.Time.Split(':')[0])) &&
                   (Convert.ToInt32(time.Split(':')[1]) < Convert.ToInt32(playerScore.Time.Split(':')[1]));
        }

        public int AddRecord(string name, int score, int steps, string time)
        {
            if (!IsRecord(score, steps, time))
                return -1;

            var newPlayerScore = new PlayerScore(name, score, steps, time);
            for (var i = _maxLength; i > 0; i--)
            {
                var playerScore = _topList[i - 1];
                if (playerScore == null) continue;
                if (playerScore > newPlayerScore)
                {
                    _topList[i] = newPlayerScore;
                    return i;
                }
                if (i < _maxLength)
                {
                    _topList[i] = playerScore;
                }
            }
            _topList[0] = newPlayerScore;

            return 0;
        }

        public IEnumerator GetEnumerator()
        {
            return _topList.GetEnumerator();
        }
    }
}
