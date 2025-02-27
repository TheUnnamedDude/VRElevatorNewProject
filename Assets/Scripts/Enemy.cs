﻿using UnityEngine;
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
    public AudioClip ImpactClip;

    private float _animationElapsed;
    private int _defaultLayer;
    private int _ignoreRaycast;
    private bool _animationStarted;
    private AudioSource[] _audioSources;
    private int _audioSourceIndex;
    private bool _deathAnimation;

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
    }

    public virtual void Update()
    {
        if (!_animationStarted)
            return;

        _animationElapsed -= Time.deltaTime;

        if (_animationElapsed >= 0)
            return;

        _animationStarted = false;
        _animationElapsed = 0;

        if (Alive)
            return;
        if (_deathAnimation)
        {
            MainAudioSource.PlayOneShot(ImpactClip);
            ResetEnemy();
            _gameController.OnTargetDestroy(20f);
            _deathAnimation = false;
        }
    }


    public virtual void OnHit(float damage)
    {
        if (!Alive)
            return;
        _health -= damage;
        if (_health > 0)
            return;
        OnKill();
    }

    public virtual void OnKill()
    {
        if (!Alive)
            return;
        _animationStarted = true;
        // Run animation then reset
        Alive = false;
        _deathAnimation = true;
        _animationElapsed = AnimationTime / 2f;
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
        gameObject.SetActive(false);
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
        Alive = true;
        GetComponentInChildren<Animator>().SetBool("Dead", false);
        _animationElapsed = AnimationTime;
        _animationStarted = true;
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