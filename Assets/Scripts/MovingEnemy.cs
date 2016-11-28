using UnityEngine;

public class MovingEnemy : Enemy
{
    public float speed = 25;
    private Vector3 _path;
    public bool OnTheMove;
    public Transform EnemyModel;
    public GameObject DestinationPath;
    private Vector3 _startPosition;
    
    void Start()
    {
        _startPosition = gameObject.transform.position;
        _path = DestinationPath.transform.position;
    }

    void Update()
    {
        base.Update();
        if (Alive) {
            Movement();
        }
    }

    private void Movement()
    {
        var step = speed * Time.deltaTime;
        if (!OnTheMove)
        {
            EnemyModel.position = Vector3.MoveTowards(EnemyModel.position, _startPosition, step);
            if (EnemyModel.position == _startPosition)
            {
                OnTheMove = true;
            }
        }
        else
        {
            EnemyModel.position = Vector3.MoveTowards(EnemyModel.position, _path, step);
            if (EnemyModel.position == _path)
            {
                OnTheMove = false;
            }
        }
    }
}