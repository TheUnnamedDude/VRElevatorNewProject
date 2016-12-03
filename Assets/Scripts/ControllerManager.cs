using UnityEngine;

public class ControllerManager : BasePlayer
{

    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device device;
    private GunDisplay _gunDisplay;

    // Use this for initialization
    public override void Start()
    {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
        _gunDisplay = GetComponentInChildren<GunDisplay>();
        device = SteamVR_Controller.Input((int)_trackedObject.index);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Lazer.enabled = IsFiring;
        UpdateRecoilTime();

        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if (currentEnergy < maxEnergy && !IsFiring)
        {
            currentEnergy += ChargeSpeed * Time.deltaTime;
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (FullAuto)
            {
                StartCoroutine(Auto());
                device.TriggerHapticPulse(3999);
            }
            else
            {
                StartCoroutine(Burst());
                device.TriggerHapticPulse(3999);
            }
        }
        else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            IsFiring = false;
        }

        CheckTouchpad();
    }

    void CheckTouchpad()
    {
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchPoint = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            Debug.Log(touchPoint);
            if (touchPoint.y < -0.5f)
            {
                _gunDisplay.GunMode = !_gunDisplay.GunMode;
                return;
            }
            if (touchPoint.x < 0.5f && touchPoint.x > -0.5f)
            {
                _gunDisplay.HandleAction();
                return;
            }

            if (_gunDisplay.GunMode)
            {
                FiringMode += (int)Mathf.Round(touchPoint.x);
            }
            else
            {
                _gunDisplay.MenuAction += (int)Mathf.Round(touchPoint.x);
            }

        }
    }

}
