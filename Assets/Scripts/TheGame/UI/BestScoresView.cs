using UnityEngine;

// ========================
// Revision 2020.11.13
// ========================

namespace TheGame.UI
{
    public class BestScoresView : MonoBehaviour
    {
        // ========================================================================================
        public bool SearchCurrent = true;
        public GameObject CheerText;
        public BestScoreValueView[] BestScoreViews;

        private bool _currentWasFound;


        // ========================================================================================
        public void RefreshView()
        {
            var scores = PersistentDataStorage.Instance.BestScores;
            for (var i = 0; i < scores.Count; i++)
            {
                var scoreRec = scores[i];
                if (SearchCurrent && !_currentWasFound && scoreRec.Score == GameField.Instance.CurrentScore )
                {
                    _currentWasFound = true;
                    CheerText.transform.SetSiblingIndex(i + 1);
                    CheerText.SetActive(true);
                    BestScoreViews[i].SetValue(scoreRec.Score, true);
                }
                else
                {
                    BestScoreViews[i].SetValue(scoreRec.Score, false);
                }
            }
        }

        protected void OnEnable()
        {
            CheerText.SetActive(false);
            _currentWasFound = false;

            RefreshView();
        }
    }
}