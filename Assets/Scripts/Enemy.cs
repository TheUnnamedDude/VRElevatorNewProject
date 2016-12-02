using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, Shootable
{
    [Inject]
    private GameController _gameController;
    private Renderer[] _renderers;
    public float MaxHealth;
    public float AnimationTime;
    public int MinSpawnLevel;
    public AudioClip SpawnSound;

    private float _animationElapsed;
    private int _defaultLayer;
    private int _ignoreRaycast;
    private bool _animationStarted;
    private AudioSource[] _audioSources;
    private int _audioSourceIndex;

    public AudioSource MainAudioSource { private set; get; }

    private float _health;

    public LevelGenerator.ElevatorDirection Direction;

    public bool Alive { get; private set; }

    public virtual void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _defaultLayer = gameObject.layer;
        _ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        _audioSources = GetComponents<AudioSource>();
        MainAudioSource = NextAudioSource();
        ResetEnemy();
    }

    public virtual void Update()
    {
        if (!_animationStarted)
            return;

        _animationElapsed += Time.deltaTime;

        if (_animationElapsed <= AnimationTime)
            return;

        _animationStarted = false;
        _animationElapsed = 0;

        if (Alive)
            return;
        ResetEnemy();
        _gameController.OnTargetDestroy(20f);
    }


    public virtual void OnHit(float damage)
    {
        if (!Alive)
            return;
        _health -= damage;
        if (_health > 0)
            return;
        _animationStarted = true;
        // Run animation then reset
        Alive = false;
        GetComponentInChildren<Animator>().SetBool("Dead", true);
    }

    public AudioSource NextAudioSource()
    {
        return _audioSourceIndex >= _audioSources.Length ?
            null : _audioSources[_audioSourceIndex++];
    }

    public virtual void ResetEnemy()
    {
        Alive = false;
        _health = MaxHealth;
        foreach (var rendr in _renderers)
        {
            rendr.enabled = false;
        }
        gameObject.layer = _ignoreRaycast;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = _ignoreRaycast;
        }
    }
    public virtual void Show()
    {
        foreach (var rendr in _renderers)
        {
            rendr.enabled = true;
        }
        Alive = true;
        gameObject.layer = _defaultLayer;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = _defaultLayer;
        }
        GetComponentInChildren<Animator>().SetBool("Dead", false);
        if (MainAudioSource != null && SpawnSound != null)
        {
            MainAudioSource.PlayOneShot(SpawnSound);
        }
    }

    public bool IsAnimationRunning()
    {
        return _animationStarted;
    }
}