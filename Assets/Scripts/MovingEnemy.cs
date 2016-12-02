using UnityEngine;

public class MovingEnemy : Enemy
{
    public AudioClip MovingSound;
    public float speed = 25;
    private Vector3 _path;
    public bool OnTheMove;
    public Transform EnemyModel;
    public GameObject DestinationPath;
    private Vector3 _startPosition;
    private AudioSource _movingAudioSource;

    public override void Awake()
    {
        base.Awake();
        _movingAudioSource = NextAudioSource();
        _movingAudioSource.clip = MovingSound;
        _movingAudioSource.loop = true;
    }
    
    void Start()
    {
        _startPosition = gameObject.transform.position;
        _path = DestinationPath.transform.position;
    }

    public override void Update()
    {
        base.Update();
        if (Alive && IsAnimationRunning()) {
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

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        if (_movingAudioSource != null)
        {
            _movingAudioSource.Stop();
        }
    }

    public override void Show()
    {
        base.Show();
        _movingAudioSource.Play();
    }
}