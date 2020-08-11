using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NBPChess.UI
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        public Text username;
        public Text time;
        public Text rank;
        private float timeInSeconds;
        public bool shouldUpdate;

        public void Update()
        {
            if (shouldUpdate)
            {
                timeInSeconds -= Time.deltaTime;
                WriteTime();
            }
        }

        public void Setup(string username, int timeInSeconds, int rank, bool shouldUpdate)
        {
            this.username.text = username;
            this.timeInSeconds = timeInSeconds;
            this.rank.text = rank.ToString();
            this.shouldUpdate = shouldUpdate;
            WriteTime();

        }
        public void UpdateTime(int timeInSeconds, bool shouldUpdate)
        {
            this.timeInSeconds = timeInSeconds;
            this.shouldUpdate = shouldUpdate;
            WriteTime();
        }
        private void WriteTime()
        {
            int hours = (int)timeInSeconds / 3600;
            int minutes = (int)timeInSeconds / 60 - hours * 60;
            int seconds = (int)timeInSeconds % 60;
            if (hours > 0)
            {
                time.text = $"{hours}:{minutes}:{seconds}";
            } else
            {
                time.text = $"{minutes}:{seconds}";
            }

        }
    }

}
