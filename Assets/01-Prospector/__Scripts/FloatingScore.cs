using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eFSState
{
    idle,
    pre,
    active,
    post
}


public class FloatingScore : MonoBehaviour
{
    [Header("Set Dynamically")]
<<<<<<< Updated upstream
    public eFSState state = eFSState.idle;

    [SerializeField]
    protected int _score = 0;
    public string scoreString;

    public int score
    {
        get
        {
            return (_score);
=======
    public eFSState         state = eFSState.idle;

    [SerializeField]
    protected int           _score = 0;
    public string           scoreString;

    public int score 
    {
        get
        {
            return(_score);
>>>>>>> Stashed changes
        }
        set
        {
            _score = value;
            scoreString = _score.ToString("N0");
            GetComponent<Text>().text = scoreString;
        }
    }
<<<<<<< Updated upstream
    public List<Vector2> bezierPts;
    public List<float> fontSizes;
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public string easingCurve = Easing.InOut;

    public GameObject reportFinishTo = null;

    private RectTransform rectTrans;
    private Text txt;
=======
    public List<Vector2>    bezierPts;
    public List<float>      fontSizes;
    public float            timeStart = -1f;
    public float            timeDuration = 1f;
    public string           easingCurve = Easing.InOut;

    public GameObject       reportFinishTo = null;

    private RectTransform   rectTrans;
    private Text            txt;
>>>>>>> Stashed changes

    public void Init(List<Vector2> ePts, float eTimeS = 0, float eTimeD = 1)
    {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.anchoredPosition = Vector2.zero;

        txt = GetComponent<Text>();

        bezierPts = new List<Vector2>(ePts);
        if (ePts.Count == 1)
        {
            transform.position = ePts[0];
            return;
        }
        if (eTimeS == 0) eTimeS = Time.time;
        timeStart = eTimeS;
        timeDuration = eTimeD;

        state = eFSState.pre;
    }
    public void FSCallback(FloatingScore fs)
    {
        score += fs.score;
    }
    void Update()
    {
        if (state == eFSState.idle) return;

<<<<<<< Updated upstream
        float u = (Time.time - timeStart) / timeDuration;
        float uC = Easing.Ease(u, easingCurve);
        if (u < 0)
        {
            state = eFSState.pre;
            txt.enabled = false;
        }
        else
        {
            if (u >= 1)
=======
        float u = (Time.time - timeStart)/timeDuration;
        float uC = Easing.Ease (u, easingCurve);
        if (u<0) 
        {
            state = eFSState.pre;
            txt.enabled= false;
        }
        else
        {
            if (u>=1)
>>>>>>> Stashed changes
            {
                uC = 1;
                state = eFSState.post;
                if (reportFinishTo != null)
                {
                    reportFinishTo.SendMessage("FSCallback", this);
<<<<<<< Updated upstream
                    Destroy(gameObject);
=======
                    Destroy (gameObject);
>>>>>>> Stashed changes
                }
                else
                {
                    state = eFSState.idle;
                }
            }
            else
            {
                state = eFSState.active;
                txt.enabled = true;
            }
            Vector2 pos = Utils.Bezier(uC, bezierPts);
            rectTrans.anchorMin = rectTrans.anchorMax = pos;
<<<<<<< Updated upstream
            if (fontSizes != null && fontSizes.Count > 0)
=======
            if (fontSizes != null && fontSizes.Count>0)
>>>>>>> Stashed changes
            {
                int size = Mathf.RoundToInt(Utils.Bezier(uC, fontSizes));
                GetComponent<Text>().fontSize = size;
            }
        }
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
