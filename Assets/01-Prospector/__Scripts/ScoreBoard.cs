using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard S;

    [Header("Set in Inspector")]
<<<<<<< Updated upstream
    public GameObject prefabFloatingScore;

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
=======
    public GameObject       prefabFloatingScore;

    [Header("Set Dynamically")]
    [SerializeField] private int    _score = 0;
    [SerializeField] private string _scoreString;

    private Transform        canvasTrans;

    public int score 
    {
        get
        {
            return(_score);
        }
        set 
>>>>>>> Stashed changes
        {
            _score = value;
            scoreString = _score.ToString("N0");
        }
    }
    public string scoreString
    {
<<<<<<< Updated upstream
        get
        {
            return (_scoreString);
=======
        get 
        {
            return(_scoreString);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        else
=======
        else 
>>>>>>> Stashed changes
        {
            Debug.LogError("ERROR: Scoreboard.Awake(): S is already set!");
        }
        canvasTrans = transform.parent;
    }
    public void FSCallbacl(FloatingScore fs)
    {
        score += fs.score;
    }
    public FloatingScore CreateFloatingScore(int amt, List<Vector2> pts)
    {
<<<<<<< Updated upstream
        GameObject go = Instantiate<GameObject>(prefabFloatingScore);
        go.transform.SetParent(canvasTrans);
=======
        GameObject go = Instantiate <GameObject> (prefabFloatingScore);
        go.transform.SetParent (canvasTrans);
>>>>>>> Stashed changes
        FloatingScore fs = go.GetComponent<FloatingScore>();
        fs.score = amt;
        fs.reportFinishTo = this.gameObject;
        fs.Init(pts);
<<<<<<< Updated upstream
        return (fs);
    }
}
=======
        return(fs);
    }
}
>>>>>>> Stashed changes
