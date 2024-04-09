using UnityEngine;

namespace Golf
{
    public enum eScoreEvent
    {
        draw,
        mine, // Renamed for clarity, consider 'tableauCleared' or similar if more appropriate
        gameWin,
        gameLoss,
        holeComplete // New event for completing a hole
    }

    public class ScoreManagerGolf : MonoBehaviour
    {
        static private ScoreManagerGolf S;
        public int score = 0; // Represents the current score for the hole
        public int totalScore = 0; // Total score across all holes

        [Header("Game Settings")]
        public int holes = 9; // Number of holes (rounds) in a game
        public int par = 45; // Par score for the game

        void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.LogError("ERROR: ScoreManagerGolf.Awake(): S is already set!");
            }
        }

        public static void EVENT(eScoreEvent evt, int cardsLeft = 0)
        { // cardsLeft parameter added
            if (S == null)
            {
                Debug.LogError("ScoreManagerGolf.EVENT() called with no instance.");
                return;
            }
            S.HandleEvent(evt, cardsLeft);
        }

        void HandleEvent(eScoreEvent evt, int cardsLeft)
        {
            if (evt == eScoreEvent.holeComplete)
            {
                if (cardsLeft > 0)
                {
                    // Score one point for each card remaining in the tableau
                    score += cardsLeft;
                }
                else
                {
                    // If the tableau is cleared, score a negative point for every card left in the stock
                    //score -= S.deck.cards.Count; // Assuming `S.deck.cards.Count` gives the remaining cards in the stock
                    /*******************************/
                }
                totalScore += score;
                CheckGameEnd();
            }
            else if (evt == eScoreEvent.gameWin)
            {
                // Logic for handling game win, potentially resetting for a new hole
            }
            else if (evt == eScoreEvent.gameLoss)
            {
                // Logic for handling game loss
            }
            // Additional event handling as needed
        }

        void CheckGameEnd()
        {
            if (totalScore <= par)
            {
                Debug.Log("Game over. Score: " + totalScore + " (Par or better!)");
            }
            else
            {
                Debug.Log("Game over. Score: " + totalScore);
            }
            // Implement logic for game end, e.g., showing results, resetting for a new game, etc.
        }

        public static int SCORE => S.score;
        public static int HIGH_SCORE => S.totalScore;
    }
}
