using System.Collections;
using UnityEngine;
using Zenject;

public class BasePlayer : MonoBehaviour
{

    public float Speed = 10;
    public Transform BarrelOpening;
    public Transform Bullet;
    public Transform ExplosionSphere;

    //firing mode variables
    private float _lastShot;
    public float RecoilTime;
    public int FiringMode;
    public bool FullAuto;
    public int FiringCycle;
    public float Damage;

    //Energy management variables
    public float maxEnergy = 100f;
    public float ChargeSpeed = 20f;
    public float currentEnergy;
    public float energyDecrease;
    public bool isFiring = false;

    public LineRenderer Lazer;
    Ray ray;

    public void Start()
    {
        currentEnergy = maxEnergy;
        Lazer = GetComponent<LineRenderer>();
        Lazer.SetWidth(0.01f, 0.01f);
        Lazer.enabled = false;
    }


    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
    }

    public void ShootBullet()
    {
        if (currentEnergy < energyDecrease)
            return;

        currentEnergy -= energyDecrease;
        ray = new Ray(BarrelOpening.position, BarrelOpening.forward);
        Lazer.SetPosition(0, ray.origin);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Lazer.SetPosition(1, hit.point);
            var shootable = hit.collider.gameObject.GetComponentInParent<Shootable>();
            if (shootable == null) {
                shootable = hit.collider.gameObject.GetComponent<Shootable>();
            }
            if (FiringMode != 3)
            {
                if (shootable != null)
                {
                    shootable.OnHit(Damage);
                }
            }
            else if (FiringMode == 3)
            {
                Instantiate(ExplosionSphere, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
            }
        }
        else {
            Lazer.SetPosition(1, ray.GetPoint(100));
        }
    }


    public void SetFiringMode()
    {
        if(FiringMode > 3)
        {
            FiringMode = 0;
        }
        if(FiringMode < 0)
        {
            FiringMode = 3;
        }
    }

    public void SetValuesByFiringMode(int firingMode)
    {
        if (firingMode == 0)
        {
            energyDecrease = 16.66f;
            RecoilTime = 0.1f;
            FullAuto = false;
            FiringCycle = 1;
            Damage = 2f;
        }
        else if (firingMode == 1)
        {
            energyDecrease = 3f;
            RecoilTime = 0.1f;
            FullAuto = true;
            Damage = 1f;
        }
        else if (firingMode == 2)
        {
            energyDecrease = 6f;
            RecoilTime = 0.05f;
            FiringCycle = 3;
            FullAuto = false;
            Damage = 1.5f;
        }
        if (FiringMode == 3)
        {
            energyDecrease = 50f;
            RecoilTime = 0.5f;
            FiringCycle = 1;
            FullAuto = false;
            Damage = 3f;
        }
    }
    public IEnumerator Burst()
    {
        if (!FullAuto)
        {
            if (energyDecrease < currentEnergy)
            {
                isFiring = true;
                for (int i = 0; i < FiringCycle; i++)
                {
                    //Lazer.enabled = true;
                    ShootBullet();
                    yield return new WaitForSeconds(RecoilTime);
                }
                //Lazer.enabled = false;
                isFiring = false;
            }
        }
    }
    public IEnumerator Auto()
    {
        //Lazer.enabled = true;
        while (energyDecrease < currentEnergy && isFiring)
        {
            ShootBullet();
            yield return new WaitForSeconds(RecoilTime);
        }
        //Lazer.enabled = false;
     }
}