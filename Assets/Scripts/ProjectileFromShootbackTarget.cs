﻿using System;
using UnityEngine;
using Zenject;

public class ProjectileFromShootbackTarget : MonoBehaviour, Shootable
{
    private bool _shot;
    public float speed = 8;
    public Vector3 _target;
    public AudioSource LoopingProjectileSound;
    public Transform Explosion;

    void Start()
    {
        //LoopingProjectileSound.Play();
    }

    void Update()
    {
        if (_target != null)
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _target, step);
            if (gameObject.transform.position == _target) //TODO: Add logic for if hit targetPath
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShootAt(Vector3 target)
    {
        _target = target;
    }

    public void OnHit(float damage)
    {
        Debug.Log("Rocket shot");
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
