using UnityEngine;
using Zenject;

public class ProjectileFromShootbackTarget : MonoBehaviour
{
    private bool _shot;
    public float speed = 5;
    public GameObject _target;
    public AudioSource LoopingProjectileSound;

    void Start()
    {
        //LoopingProjectileSound.Play();
    }

    void Update()
    {
        if (_target != null)
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _target.transform.position, step);
            if (gameObject.transform.position == _target.transform.position) //TODO: Add logic for if hit targetPath
            {
                //reduce time left by 5 sec on stage
            }
        }
    }

    public void ShootAt(GameObject target)
    {
        _target = target;
    }
}
