using System.Collections;
using UnityEngine;

public class MouseLookShoot : BasePlayer
{
    public float Sens = 2f;

    private float _xRot;
    private float _yRot;



    void Update()
    {
        UpdateRecoilTime();
        _xRot -= Input.GetAxis("Mouse Y") * Sens;
        _yRot += Input.GetAxis("Mouse X") * Sens;
        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if (currentEnergy < maxEnergy && !isFiring)
        {
            currentEnergy += ChargeSpeed * Time.deltaTime;
        }

        Lazer.enabled = isFiring;
        

        if (!FullAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Burst());
            }
        }
        if (FullAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isFiring = true;
                StartCoroutine(Auto());
            }
            if (Input.GetMouseButtonUp(0))
            {
                isFiring = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            FiringMode--;

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            FiringMode++;
        }
    }


}
