using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Golf
{
    public class ScoreBoard : MonoBehaviour
    {
        public static ScoreBoard S;

        [Header("Set Dynamically")]
        [SerializeField] private int _score = 0;
        [SerializeField] private string _scoreString;

        private Transform canvasTrans;

        public int score
        {
            get
            {
                return (_score);
            }
            set
            {
                _score = value;
                scoreString = _score.ToString("N0");
            }
        }
        public string scoreString
        {
            get
            {
                return (_scoreString);
            }
            set
            {
                _scoreString = value;
                GetComponent<Text>().text = _scoreString;
            }
        }
        void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.LogError("ERROR: Scoreboard.Awake(): S is already set!");
            }
            canvasTrans = transform.parent;
        }
        public void FSCallback(FloatingScore fs)
        {
            score += fs.score;
        }
        
    }
}
