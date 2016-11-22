using UnityEngine;
using Zenject;

public class ScoreManager : ITickable
{
    public float InitialStartTime = 15f;
    private float _timeScore;
    private float _targetScore;
    private float _timeElapsed;
    private float _timeLimit;
    private float _timeFreezed;
    private float _freezeTime;
    public float TimePointsLevelModifier = 0.2f;
    public float TimePointsPerSecond = 10.0f;
    public float TargetPointsLevelModifier = 0.2f;

    public int Score { get { return (int) (_timeScore + _targetScore); } }
    public int Level { get; private set; }

    public float TimeLeft
    {
        get { return _timeLimit - _timeElapsed; }
        set { _timeElapsed = TimeLeft - value; }
    }

    public bool GameOver { get { return TimeLeft <= 0; }}

    public float TimeElapsedForLevel { get; private set; }

    public float ExpectedLevelTime { get; private set; }
    public bool Freezed { get { return _freezeTime > 0; } }

    public void AddTargetScore(float score)
    {
        _targetScore += score * TargetPointsLevelModifier;
    }

    public void NextLevel()
    {
        Level++;
        _timeScore += CalculateLevelTimeScore();
        TimeElapsedForLevel = 0;
        ExpectedLevelTime = GetTimeForLevel();
        _timeLimit += GetTimeForLevel();
    }

    public bool IsTestLevel()
    {
        return false;
    }

    public void Tick()
    {
        if (_freezeTime > 0)
        {
            _timeFreezed += Time.deltaTime;
            if (_timeFreezed >= _freezeTime)
            {
                _freezeTime = 0;
                _timeFreezed = 0;
            }
        }
        else
		{
			if (GameOver)
				return;
			_timeElapsed += Time.deltaTime;
            TimeElapsedForLevel += Time.deltaTime;
        }
    }

    public void FreezeTime(float freezeTime)
    {
        _freezeTime = freezeTime;
    }

    private float CalculateLevelTimeScore()
    {
        var spareTime = TimeElapsedForLevel - ExpectedLevelTime;
        if (spareTime > 0)
        {
            return spareTime * TimePointsPerSecond * Level * TimePointsLevelModifier;
        }
        return 0f;
    }

    /// <summary>
    /// Calculate the time you gain for finishing the current level
    /// </summary>
    /// <returns>A float representing the time you gained in seconds</returns>
    private float GetTimeForLevel()
    {
        return 14.0f;
    }

    public void Reset()
    {
        _timeElapsed = 0;
        TimeElapsedForLevel = 0;
        _targetScore = 0;
        _timeScore = 0;
    }
}