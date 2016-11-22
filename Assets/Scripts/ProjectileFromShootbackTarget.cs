using UnityEngine;
using Zenject;

public class ProjectileFromShootbackTarget : Enemy
{
    public float speed = 5;
    private Vector3 TargetPath;
    public AudioSource LoopingProjectileSound;

    void Start()
    {
        LoopingProjectileSound.Play();
        TargetPath = GameObject.FindGameObjectWithTag("Player").transform.position;
        Time.timeScale = 0.50f;
    }

    void Update()
    {
        base.Update();
        Shoot();
    }

    private void Shoot()
    {
        gameObject.transform.LookAt(TargetPath);
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TargetPath, step);
        if (gameObject.transform.position == TargetPath) //TODO: Add logic for if hit targetPath
        {
            Time.timeScale = 1f;
            //Damage player or reduce time left on stage
        }
    }
}
