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
    private bool _isFiring;

    public bool IsFiring
    {
        get { return _isFiring; }
        set
        {
            _isFiring = value;
            Lazer.enabled = value;
        }
    }

    public LineRenderer Lazer;

    public void Start()
    {
        currentEnergy = maxEnergy;
        Lazer = GetComponent<LineRenderer>();
        Lazer.SetWidth(0.01f, 0.01f);
    }


    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
        if (IsFiring)
        {
            UpdateLaserPosition();
        }
    }

    public void UpdateLaserPosition()
    {
        var ray = new Ray(BarrelOpening.position, BarrelOpening.forward);
        Lazer.SetPosition(0, ray.origin);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Lazer.SetPosition(1, hit.point);
        }
        else
        {
            Lazer.SetPosition(1, ray.GetPoint(100));
        }
    }

    public void ShootBullet()
    {
        if (currentEnergy < energyDecrease)
            return;

        currentEnergy -= energyDecrease;
        RaycastHit hit;
        if (Physics.Raycast(BarrelOpening.position, BarrelOpening.TransformDirection(Vector3.forward), out hit))
        {
            var shootable = GetShootable(hit.collider.transform);
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
    }

    private Shootable GetShootable(Transform hitpoint, bool checkChildren=true)
    {
        var shootable = hitpoint.GetComponent<Shootable>();
        if (shootable != null)
            return shootable;
        if (checkChildren && hitpoint.childCount > 0)
        {
            var shootables = hitpoint.GetComponentsInChildren<Shootable>();
            if (shootables.Length > 0)
                return shootables[0];
        }
        return hitpoint.transform.parent == null ? null : GetShootable(hitpoint.parent, false);
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
                IsFiring = true;
                for (int i = 0; i < FiringCycle; i++)
                {
                    ShootBullet();
                    yield return new WaitForSeconds(RecoilTime);
                }
                IsFiring = false;
            }
        }
    }
    public IEnumerator Auto()
    {
        while (energyDecrease < currentEnergy && IsFiring)
        {
            ShootBullet();
            yield return new WaitForSeconds(RecoilTime);
        }
    }
}