using UnityEngine;
using Zenject;

public class ShootbackEnemy : Enemy
{
    public float Timer;
    public GameObject Projectile;
    private AudioSource _targetLock;
    public GameObject Missileport;
    private GameObject _target;

    void Awake()
    {
        base.Awake();
        _targetLock = GetComponent<AudioSource>();
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        LockOnTargetSound();
        
    }

    void Update()
    {
        base.Update();
        if (Alive)
        {
            TargetPlayer();
        }
    }

    private void LockOnTargetSound()
    {
        _targetLock.Play();
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
        GameObject projectile = (GameObject) Instantiate(Projectile, Missileport.transform.position, gameObject.transform.rotation, transform);
        Debug.Log("Rockets AWAAAAAAY!");
        projectile.transform.LookAt(_target.transform.position);
        projectile.GetComponent<ProjectileFromShootbackTarget>().ShootAt(_target.transform.position);
    }
}