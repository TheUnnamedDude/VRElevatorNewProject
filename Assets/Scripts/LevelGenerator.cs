using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;
using UnityObject = UnityEngine.Object;

public class LevelGenerator : ITickable
{
    private readonly ElevatorDirection[] ALL_DIRECTIONS = {ElevatorDirection.North, ElevatorDirection.East,
            ElevatorDirection.South, ElevatorDirection.West};

    [Inject]
    private ScoreManager _scoreManager;

    public double MinDistance = 40;
    public double MaxDistance = 130;

    private System.Random _rng;
    private readonly Dictionary<ElevatorDirection, List<Enemy>> _enemies = new Dictionary<ElevatorDirection, List<Enemy>>();
    private List<SpawnEvent> _spawnTime = new List<SpawnEvent>();
    private List<Door> doors = new List<Door>();
    private int _seed;

    public int Seed
    {
        get
        {
            if (_seed == 0)
            {
                _seed = new System.Random().Next();
            }
            return _seed;
        }
        set { _seed = value; }
    }

    public int NumberOfTargetsAlive { get { return NumberOfVisibleTargets + _spawnTime.Count; } }

    public int NumberOfVisibleTargets
    {
        get
        {
            var count = 0;
            foreach (var enemies in _enemies.Values)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.Alive)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }

    public void InitializeGame()
    {
        foreach (var direction in ALL_DIRECTIONS)
        {
            Debug.Log(direction);
            _enemies[direction] = new List<Enemy>();
        }
        foreach (var enemy in UnityObject.FindObjectsOfType<Enemy>())
        {
            if (!_enemies.ContainsKey(enemy.Direction)) {
                Debug.LogWarning("Unmapped direction on enemy " + enemy);
                continue;
            }
            _enemies[enemy.Direction].Add(enemy);
        }
        doors.AddRange(UnityObject.FindObjectsOfType<Door>());
        Reset();
        FinishLevel();
    }

    public void Tick()
    {
        if (_scoreManager.Freezed || _scoreManager.TimeElapsedForLevel > 0 || _spawnTime.IsEmpty())
            return;
        _spawnTime
            .Where(e => !e.Enemy.Alive && e.SpawnTime <= _scoreManager.TimeElapsedForLevel)
            .ForEach(e => e.Execute());

        if (NumberOfVisibleTargets <= 0)
        {
            _spawnTime.First(e => !e.Enemy.Alive).Execute();
        }

        _spawnTime.RemoveAll(e => e.Executed);
    }

    public void Reset()
    {
        _rng = new System.Random(Seed);
        foreach (var door in doors) {
            if (door.Open)
            {
                door.Open = false;
            }
        }
    }

    public void FinishLevel()
    {
        foreach (var enemies in _enemies.Values)
        {
            enemies.ForEach(enemy => enemy.ResetEnemy());
        }
        var numberOfSpawns = GetTargetSpawnsForLevel(_scoreManager.Level + 1);
        _scoreManager.NextLevel(numberOfSpawns);

        var directions = GetElevatorSidesForLevel(_scoreManager.Level);

        var availableDirections = new List<ElevatorDirection>(ALL_DIRECTIONS);
        var spawnableEnemies = new List<Enemy>();
        bool doorsOpen = false;
        doors.Where(d => d.Open)
            .ForEach(door =>
            {
                door.ScheduleClose();
                doorsOpen = false;
            });
        for (var i = 0; i < directions; i++)
        {
            var directionIndex = _rng.Next(availableDirections.Count);
            var direction = availableDirections[directionIndex];
            var door = GetDoorByDirection(direction);
            door.ScheduleOpen(!doorsOpen);
            availableDirections.RemoveAt(directionIndex);

            foreach (var enemy in _enemies[direction])
            {
                if (_scoreManager.Level >= enemy.MinSpawnLevel)
                {
                    spawnableEnemies.Add(enemy);
                }
            }
        }
        _spawnTime.Clear();
        var northDoor = GetDoorByDirection(ElevatorDirection.North);
        if (doorsOpen)
        {
            _scoreManager.FreezeTime(northDoor.OpeningTimeOffset + northDoor.AnimationTime);
        }
        else
        {
            _scoreManager.FreezeTime(northDoor.OpeningTimeOffset);
        }
        for (var i = 0; i < Math.Min(numberOfSpawns, spawnableEnemies.Count); i++)
        {
            var enemy = spawnableEnemies[_rng.Next(_rng.Next(spawnableEnemies.Count))];
            _spawnTime.Add(new SpawnEvent(enemy, (float) (_rng.NextDouble() * (_scoreManager.ExpectedLevelTime / 2))));
            spawnableEnemies.Remove(enemy);
        }
        _spawnTime.Sort();
    }

    private Door GetDoorByDirection(ElevatorDirection direction)
    {
        foreach (var door in doors)
        {
            if (door.Direction == direction)
            {
                return door;
            }
        }
        return null;
    }

    private int GetTargetSpawnsForLevel(int level)
    {
        if (level <= 3)
        {
            return level + 1;
        }
        if (level < 10)
        {
            return _rng.Next(3, 5);
        }
        if (level < 20)
        {
            return _rng.Next(3, 10);
        }
        if (level < 25)
        {
            return _rng.Next(10, 20);
        }
        if (level < 30)
        {
            return _rng.Next(15, 30);
        }
        return level < 40 ? _rng.Next(25, 40) : 40;
    }

    private int GetElevatorSidesForLevel(int level)
    {
        if (level < 3)
        {
            return 1;
        }
        if (level == 3)
        {
            return 2;
        }
        if (level < 10)
        {
            return _rng.Next(1, 3);
        }
        if (level < 15)
        {
            return _rng.Next(1, 4);
        }
        if (level < 20)
        {
            return _rng.Next(2, 5);
        }
        return level < 25 ? _rng.Next(3, 5) : 4;
    }

    public ElevatorDirection GetRandomDirection(List<ElevatorDirection> directions)
    {
        return directions[_rng.Next(directions.Count)];
    }

    public enum ElevatorDirection
    {
        North = 5,
        South = 95,
        West = 185,
        East = 275
    }
}