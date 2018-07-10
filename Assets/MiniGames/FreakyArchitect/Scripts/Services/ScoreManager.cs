using UnityEngine;
using System;
using System.Collections;

namespace FreakyArchitect
{
    public class ScoreManager : SharedInstance<ScoreManager> {
        public int Score { get; private set; }

        public int HighScore { get; private set; }

        public bool HasNewHighScore { get; private set; }

        public int Height { get; private set; }

        public int BestHeight { get; private set; }

        public bool HasNewBestHeight { get; private set; }

        public static event Action<int> ScoreUpdated = delegate {};
        public static event Action<int> HighscoreUpdated = delegate {};
        public static event Action<int> HeightUpdated = delegate{};
        public static event Action<int> BestHeightUpdated = delegate{};

        const string HIGH_SCORE = "HIGHSCORE";
        // key name to store high score in PlayerPrefs
        const string BEST_HEIGHT = "BESTHEIGHT";
        // key name to store best height in PlayerPrefs

        public void Reset()
        {
            // Initialize scores
            Score = 0;
            Height = 0;

            // Initialize highscores
            HighScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
            HasNewHighScore = false;

            BestHeight = PlayerPrefs.GetInt(BEST_HEIGHT, 0);
            HasNewBestHeight = false;
        }

        public void AddScore(int amount)
        {
            SetScore(Score + amount);
        }

        public void SetScore(int newScore)
        {
            Score = newScore;

            // Fire event
            ScoreUpdated(Score);

            // Update highscore if player has made a new one
            UpdateHighScore();
        }

        public void AddHeight(int amount)
        {
            SetHeight(Height + amount);
        }

        public void SetHeight(int newHeight)
        {
            Height = newHeight;

            // Fire event
            HeightUpdated(Height);

            // Update best height
            UpdateBestHeight();
        }

        void UpdateHighScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt(HIGH_SCORE, HighScore);
                HasNewHighScore = true;
                HighscoreUpdated(HighScore);
            }
        }

        void UpdateBestHeight()
        {
            if (Height > BestHeight)
            {
                BestHeight = Height;
                PlayerPrefs.SetInt(BEST_HEIGHT, BestHeight);
                HasNewBestHeight = true;
                BestHeightUpdated(BestHeight);
            }
        }
    }
}
