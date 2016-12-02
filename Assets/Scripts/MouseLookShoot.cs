using System.Collections;
using UnityEngine;

public class MouseLookShoot : BasePlayer
{
    public float Sens = 2f;

    private float _xRot;
    private float _yRot;

    private GunDisplay _gunDisplay;

    void Start()
    {
        base.Start();
        _gunDisplay = GetComponentInChildren<GunDisplay>();
    }

    void Update()
    {
        UpdateRecoilTime();
        _xRot -= Input.GetAxis("Mouse Y") * Sens;
        _yRot += Input.GetAxis("Mouse X") * Sens;
        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

        _gunDisplay.changeMenuAction();
        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if (currentEnergy < maxEnergy && !IsFiring)
        {
            currentEnergy += ChargeSpeed * Time.deltaTime;
        }
        

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
                StartCoroutine(Auto());
            }
            if (Input.GetMouseButtonUp(0))
            {
                IsFiring = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if(_gunDisplay.GunMode)
            {
                FiringMode--;
            } else
            {
                _gunDisplay.MenuAction--;
            }
            

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(_gunDisplay.GunMode)
            {
                FiringMode++;
            }
            else
            {
                _gunDisplay.MenuAction++;
            }
            
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            _gunDisplay.GunMode = !_gunDisplay.GunMode;
        }
        if(!_gunDisplay.GunMode && Input.GetKeyDown(KeyCode.F))
        {
            _gunDisplay.HandleAction();
        }
    }
}
