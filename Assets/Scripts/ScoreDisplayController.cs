using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScoreDisplayController : MonoBehaviour {
    public Text ScoreText;
    public Text TimeleftText;
    public Text LevelText;
    public Image Help;

    public string ScoreFormat;
    public string TimeLeftFormat;
    public string LevelTextFormat;

    public bool helpMode = false;

    [Inject]
    private ScoreManager _scoreManager;
    [Inject]
    private GameController _gameController;

    void Update() {
        UpdateScoreboardText();
        Debug.Log(_gameController.IsRunning);
    }

    public void UpdateScoreboardText() {
        ScoreText.text = string.Format(ScoreFormat, _scoreManager.Score);

        if (_scoreManager.GameOver)
        {
            TimeleftText.text = string.Format(ScoreFormat, _scoreManager.Score);
            ScoreText.text = "Press start or";
            LevelText.text = "restart.";
            TimeleftText.alignment = TextAnchor.MiddleCenter;
            ScoreText.alignment = TextAnchor.MiddleCenter;
            LevelText.alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            var seconds = (int)_scoreManager.TimeLeft;
            var milliseconds = (int)(_scoreManager.TimeLeft % 1 * 100);
            TimeleftText.text = string.Format(TimeLeftFormat, seconds, milliseconds);
            LevelText.text = string.Format(LevelTextFormat, _scoreManager.Level);
        }
        
    }

   
}

    