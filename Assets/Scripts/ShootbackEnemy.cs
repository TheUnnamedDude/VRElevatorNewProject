using UnityEngine;
using Zenject;

public class ShootbackEnemy : Enemy
{
    public float Timer;
    public GameObject Projectile;
    public AudioClip TargetLockSound;
    public GameObject Missileport;
    private GameObject _target;
    [Inject]
    private ScoreManager _scoreManager;

    public override void Awake()
    {
        base.Awake();
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Update()
    {
        base.Update();
        if (Alive && !_scoreManager.GameOver)
        {
            TargetPlayer();
        }
    }

    private void LockOnTargetSound()
    {
        MainAudioSource.PlayOneShot(TargetLockSound);
    }

    private void TargetPlayer()
    {
        Timer += Time.deltaTime;
        if(Timer >= 5f)
        {
            ShootProjectile();
            Timer = 0;
            LockOnTargetSound();
        }
    }
    private void ShootProjectile()
    {
        var projectile = (GameObject) Instantiate(Projectile, Missileport.transform.position, gameObject.transform.rotation, transform);
        projectile.transform.LookAt(_target.transform.position);
        projectile.GetComponent<ProjectileFromShootbackTarget>().ShootAt(_target.transform.position);
    }
}