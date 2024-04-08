using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Golf
{
    public class Golf : MonoBehaviour
    {
        static public Golf S;

        [Header("Set in Inspector")]
        public TextAsset deckXML;
        public TextAsset layoutXML;
        public float xOffset = 3;
        public float yOffset = -2.5f;
        public Vector3 layoutCenter;
        public Vector2 fsPosMid = new Vector2(0.5f, 0.90f);
        public Vector2 fsPosRun = new Vector2(0.5f, 0.75f);
        public Vector2 fsPosMid2 = new Vector2(0.4f, 1.0f);
        public Vector2 fsPosEnd = new Vector2(0.5f, 0.95f);
        public float reloadDelay = 7f;
        public Text gameOverText, roundResultTest, highScoreText;


        [Header("Set Dynamically")]
        public Deck deck;
        public Layout layout;
        public List<CardGolf> drawPile;
        public Transform layoutAnchor;
        public CardGolf target;
        public List<CardGolf> tableau;
        public List<CardGolf> discardPile;
        public FloatingScore fsRun;

        void Awake()
        {
            S = this;
            SetUpUITexts();
        }

        void SetUpUITexts()
        {
            GameObject go = GameObject.Find("HighScore");
            if (go != null)
            {
                highScoreText = go.GetComponent<Text>();
            }
            int highScore = ScoreManagerGolf.HIGH_SCORE;
            string hScore = "High Score: " + Utils.AddCommasToNumber(highScore);
            go.GetComponent<Text>().text = hScore;

            go = GameObject.Find("GameOver");
            if (go != null)
            {
                gameOverText = go.GetComponent<Text>();
            }
            go = GameObject.Find("RoundResult");
            if (go != null)
            {
                roundResultTest = go.GetComponent<Text>();
            }
            ShowResultsUI(false);
        }

        void ShowResultsUI(bool show)
        {
            gameOverText.gameObject.SetActive(show);
            roundResultTest.gameObject.SetActive(show);
        }

        void Start()
        {
            ScoreBoard.S.score = ScoreManagerGolf.SCORE;
            deck = GetComponent<Deck>();
            deck.InitDeck(deckXML.text);
            Deck.Shuffle(ref deck.cards);

            Card c;
            for (int cNum = 0; cNum < deck.cards.Count; cNum++)
            {
                c = deck.cards[cNum];
                c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
            }
            layout = GetComponent<Layout>();
            layout.ReadLayout(layoutXML.text);
            drawPile = ConvertListCardsToListCardGolfs(deck.cards);
            LayoutGame();
        }

        List<CardGolf> ConvertListCardsToListCardGolfs(List<Card> lCD)
        {
            List<CardGolf> lCG = new List<CardGolf>();
            CardGolf tCG;
            foreach (Card tCD in lCD)
            {
                tCG = tCD as CardGolf;
                lCG.Add(tCG);
            }
            return lCG;
        }

        CardGolf Draw()
        {
            CardGolf cg = drawPile[0];
            drawPile.RemoveAt(0);
            return cg;
        }

        void LayoutGame()
        {
            if (layoutAnchor == null)
            {
                GameObject tGO = new GameObject("_LayoutAnchor");
                layoutAnchor = tGO.transform;
                layoutAnchor.transform.position = layoutCenter;
            }

            CardGolf cg;
            foreach (SlotDef tSD in layout.slotDefs)
            {
                cg = Draw();
                cg.faceUp = tSD.faceUp;
                cg.transform.parent = layoutAnchor;
                cg.transform.localPosition = new Vector3(
                    layout.multiplier.x * tSD.x,
                    layout.multiplier.y * tSD.y,
                    -tSD.layerID);
                cg.layoutID = tSD.id;
                cg.slotDef = tSD;
                cg.state = eCardState.tableau;
                cg.SetSortingLayerName(tSD.layerName);
                tableau.Add(cg);
            }

            foreach (CardGolf tCG in tableau)
            {
                foreach (int hid in tCG.slotDef.hiddenBy)
                {
                    cg = FindCardByLayoutID(hid);
                    tCG.hiddenBy.Add(cg);
                }
            }

            MoveToTarget(Draw());
            UpdateDrawPile();
        }

        CardGolf FindCardByLayoutID(int layoutID)
        {
            foreach (CardGolf tCG in tableau)
            {
                if (tCG.layoutID == layoutID)
                {
                    return tCG;
                }
            }
            return null;
        }

        void SetTableauFaces()
        {
            foreach (CardGolf cg in tableau)
            {
                bool faceUp = true;
                foreach (CardGolf cover in cg.hiddenBy)
                {
                    if (cover.state == eCardState.tableau)
                    {
                        faceUp = false;
                    }
                }
                cg.faceUp = faceUp;
            }
        }

        void MoveToDiscard(CardGolf cg)
        {
            cg.state = eCardState.discard;
            discardPile.Add(cg);
            cg.transform.parent = layoutAnchor;

            cg.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.discardPile.x,
                layout.multiplier.y * layout.discardPile.y,
                -layout.discardPile.layerID + 0.5f);
            cg.faceUp = true;
            cg.SetSortingLayerName(layout.discardPile.layerName);
            cg.SetSortOrder(-100 + discardPile.Count);
        }

        void MoveToTarget(CardGolf cg)
        {
            if (target != null) MoveToDiscard(target);
            target = cg;
            cg.state = eCardState.target;
            cg.transform.parent = layoutAnchor;
            cg.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.discardPile.x,
                layout.multiplier.y * layout.discardPile.y,
                -layout.discardPile.layerID);
            cg.faceUp = true;
            cg.SetSortingLayerName(layout.discardPile.layerName);
            cg.SetSortOrder(0);
        }

        void UpdateDrawPile()
        {
            CardGolf cg;
            for (int i = 0; i < drawPile.Count; i++)
            {
                cg = drawPile[i];
                cg.transform.parent = layoutAnchor;

                Vector2 dpStagger = layout.drawPile.stagger;
                cg.transform.localPosition = new Vector3(
                    layout.multiplier.x * (layout.drawPile.x + i * dpStagger.x),
                    layout.multiplier.y * (layout.drawPile.y + i * dpStagger.y),
                    -layout.drawPile.layerID + 0.1f * 1);

                cg.faceUp = false;
                cg.state = eCardState.drawpile;
                cg.SetSortingLayerName(layout.drawPile.layerName);
                cg.SetSortOrder(-10 * i);
            }
        }

        public void CardClicked(CardGolf cg)
        {
            switch (cg.state)
            {
                case eCardState.target:
                    break;
                case eCardState.drawpile:
                    MoveToDiscard(target);
                    MoveToTarget(Draw());
                    UpdateDrawPile();
                    ScoreManagerGolf.EVENT(eScoreEvent.draw);
                    FloatingScoreHandler(eScoreEvent.draw);
                    break;
                case eCardState.tableau:
                    bool validMatch = true;
                    if (!cg.faceUp)
                    {
                        validMatch = false;
                    }
                    if (!AdjacentRank(cg, target))
                    {
                        validMatch = false;
                    }
                    if (!validMatch) return;
                    tableau.Remove(cg);
                    MoveToTarget(cg);
                    SetTableauFaces();
                    ScoreManagerGolf.EVENT(eScoreEvent.mine);
                    FloatingScoreHandler(eScoreEvent.mine);
                    break;
            }
            CheckForGameOver();
        }

        void CheckForGameOver()
        {
            if (tableau.Count == 0)
            {
                GameOver(true);
                return;
            }
            if (drawPile.Count > 0)
            {
                return;
            }
            foreach (CardGolf cg in tableau)
            {
                if (AdjacentRank(cg, target))
                {
                    return;
                }
            }
            GameOver(false);
        }

        void GameOver(bool won)
        {
            int score = ScoreManagerGolf.SCORE;
            if (fsRun != null) score += fsRun.score;
            if (won)
            {
                gameOverText.text = "Round Over";
                roundResultTest.text = "You won this round!\nRound Score: " + score;
                ShowResultsUI(true);
                ScoreManagerGolf.EVENT(eScoreEvent.gameWin);
                FloatingScoreHandler(eScoreEvent.gameWin);
            }
            else
            {
                gameOverText.text = "Game Over";
                if (ScoreManagerGolf.HIGH_SCORE <= score)
                {
                    string str = "You got the high score!\nHigh score: " + score;
                    roundResultTest.text = str;
                }
                else
                {
                    roundResultTest.text = "Your final score was: " + score;
                }
                ShowResultsUI(true);
                ScoreManagerGolf.EVENT(eScoreEvent.gameLoss);
                FloatingScoreHandler(eScoreEvent.gameLoss);
            }
            Invoke("ReloadLevel", reloadDelay);
        }

        void ReloadLevel()
        {
            SceneManager.LoadScene("Golf");
        }

        public bool AdjacentRank(CardGolf c0, CardGolf c1)
        {
            if (!c0.faceUp || !c1.faceUp) return false;
            if (Mathf.Abs(c0.rank - c1.rank) == 1)
            {
                return true;
            }
            if((c0.rank == 1 && c1.rank == 13) || (c0.rank == 13 && c1.rank == 1))
    {
                return false;
            }
            return false;
        }

        void FloatingScoreHandler(eScoreEvent evt)
        {
            List<Vector2> fsPts;
            switch (evt)
            {
                case eScoreEvent.draw:
                case eScoreEvent.gameWin:
                case eScoreEvent.gameLoss:
                    if (fsRun != null)
                    {
                        fsPts = new List<Vector2>();
                        fsPts.Add(fsPosRun);
                        fsPts.Add(fsPosMid2);
                        fsPts.Add(fsPosEnd);
                        fsRun.reportFinishTo = ScoreBoard.S.gameObject;
                        fsRun.Init(fsPts, 0, 1);
                        fsRun.fontSizes = new List<float>(new float[] { 28, 36, 4 });
                        fsRun = null;
                    }
                    break;
                case eScoreEvent.mine:
                    FloatingScore fs;
                    Vector2 p0 = Input.mousePosition;
                    p0.x /= Screen.width;
                    p0.y /= Screen.height;
                    fsPts = new List<Vector2>();
                    fsPts.Add(p0);
                    fsPts.Add(fsPosMid);
                    fsPts.Add(fsPosRun);
                    fs = ScoreBoard.S.CreateFloatingScore(ScoreManagerGolf.CHAIN, fsPts);
                    fs.fontSizes = new List<float>(new float[] { 4, 50, 28 });
                    if (fsRun == null)
                    {
                        fsRun = fs;
                        fsRun.reportFinishTo = null;
                    }
                    else
                    {
                        fs.reportFinishTo = fsRun.gameObject;
                    }
                    break;
            }
        }
    }
}
