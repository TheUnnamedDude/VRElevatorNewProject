using System;

public class SpawnEvent : IComparable<SpawnEvent>
{
    public Enemy Enemy { get; private set; }
    public float SpawnTime { get; private set; }
    public bool Executed { get; private set; }

    public SpawnEvent(Enemy enemy, float spawnTime)
    {
        Enemy = enemy;
        SpawnTime = spawnTime;
    }

    public void Execute()
    {
        Enemy.Show();
        Executed = true;
    }

    public int CompareTo(SpawnEvent other)
    {
        return other.SpawnTime.CompareTo(SpawnTime);
        // Reverse order so we can reverse it and remove from list without fucking up the list
    }
}