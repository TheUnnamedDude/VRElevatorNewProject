using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : ITickable
{
    [Inject]
    private ScoreManager _scoreManager;

    [Inject]
    private LevelGenerator _levelGenerator;

    [Inject(Id = "GameOverBoxes")]
    private GameObject[] _gameOverBoxes;

    private float _slowMotionTimeLeft;

    public bool IsRunning
    {
        get;
        private set;
    }

    public void Tick ()
    {
        if (_scoreManager.GameOver && IsRunning)
        {
            GameOver();
        }
        if (_slowMotionTimeLeft <= 0)
        {
            _slowMotionTimeLeft -= Time.deltaTime;
            Time.timeScale = 1f;
        }
    }

    public void GameOver()
    {
        foreach (var rocket in Object.FindObjectsOfType<ProjectileFromShootbackTarget>())
        {
            Object.Destroy(rocket.gameObject);
        }
        foreach (var enemy in Object.FindObjectsOfType<Enemy>())
        {
            enemy.OnKill();
        }
        foreach (var controller in Object.FindObjectsOfType<GunDisplay>())
        {
            controller.RestartMode();
        }
        _levelGenerator.CloseDoors();
        // TODO: Redo this
        IsRunning = false;
        foreach(var gameOverBox in _gameOverBoxes)
        {
            gameOverBox.SetActive(true);
        }
    }

    public void StartGame() {
        IsRunning = true;
        _levelGenerator.InitializeGame();
    }

    public void ResetGame()
    {
        IsRunning = false;
        _scoreManager.Reset();
        _levelGenerator.Reset();
    }

    public void SetSlowMotion(float time)
    {
        _slowMotionTimeLeft = time;
        Time.timeScale = 0.5f;
    }

    public void OnTargetDestroy(float points)
    {
        if (_scoreManager.GameOver)
        {
            return;
        }
        _scoreManager.AddTargetScore(points);
        if (_levelGenerator.NumberOfTargetsAlive <= 0)
        {
            _levelGenerator.FinishLevel();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart pressed");
    }
}
