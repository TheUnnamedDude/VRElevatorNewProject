using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;

public class GunDisplay : MonoBehaviour {

    private BasePlayer _basePlayer;


    Slider energyBar;
    public Image energyBarFill;
    public Text ammovalue;
    public Text Single;
    public Text Auto;
    public Text Burst;
    public Text Explosive;
    
    int shotsLeft;

	Color GUIColor;
    

	// Use this for initialization
	void Start () {
        energyBar = GetComponentInChildren<Slider>();
        _basePlayer = GetComponentInParent<BasePlayer>();
        
    }
	
	// Update is called once per frame
	void Update () {
		changeGuiColor();
        shotsLeft = (int) (_basePlayer.currentEnergy / _basePlayer.energyDecrease);
        UpdateGunDisplay();
	}

    public void UpdateGunDisplay()
    {
        energyBar.value = _basePlayer.currentEnergy;
        ammovalue.text = shotsLeft.ToString();

        switch(_basePlayer.FiringMode)
        {
            case 0:
				Single.enabled = true;
                Auto.enabled = false;
                Burst.enabled = false;
                Explosive.enabled = false;
                break;
            case 1:
				Single.enabled = false;
				Auto.enabled = true;
				Burst.enabled = false;
				Explosive.enabled = false;
				break;
            case 2:
				Single.enabled = false;
				Auto.enabled = false;
				Burst.enabled = true;
				Explosive.enabled = false;
				break;
            case 3:
				Single.enabled = false;
				Auto.enabled = false;
				Burst.enabled = false;
				Explosive.enabled = true;
				break;
        }
        
    }
	void changeGuiColor() {
        GUIColor = Color.Lerp(Color.red, Color.green, (_basePlayer.currentEnergy / 100));
        Single.color = GUIColor;
        Auto.color = GUIColor;
        Burst.color = GUIColor;
        Explosive.color = GUIColor;
        ammovalue.color = GUIColor;
        energyBarFill.color = GUIColor;
	}
}
