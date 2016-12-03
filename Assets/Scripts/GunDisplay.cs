using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GunDisplay : MonoBehaviour {

    [Inject]
    private GameController _gameController;


    private BasePlayer _basePlayer;
    public int MenuAction;
    public GameObject helpCanvas;

    Slider energyBar;
    public Image energyBarFill;
    public Text ammovalue;
    public Text Single;
    public Text Auto;
    public Text Burst;
    public Text Explosive;
    public Text RightArrow;
    public Text LeftArrow;
    public Text MenuIcon;
    public Text MenuText;
    int shotsLeft;

	Color GUIColor;

    public bool GunMode = true;
    private bool _restartMode;

	// Use this for initialization
	void Start () {
        MenuAction = 0;
        energyBar = GetComponentInChildren<Slider>();
        _basePlayer = GetComponentInParent<BasePlayer>();
    }
	
	void Update () {
		ChangeGuiColor();
        shotsLeft = (int) (_basePlayer.currentEnergy / _basePlayer.energyDecrease);
	}

    public void UpdateGunDisplay()
    {
        if (_restartMode)
        {
            Single.enabled = false;
            Auto.enabled = false;
            Burst.enabled = false;
            Explosive.enabled = false;
            energyBar.enabled = false;
            ammovalue.enabled = false;
            MenuText.enabled = true;
            MenuText.text = "Restart";
            return;
        }
        energyBar.value = _basePlayer.currentEnergy;
        ammovalue.text = shotsLeft.ToString();
        if(GunMode)
        {
            MenuText.enabled = false;
            Single.enabled = true;
            Auto.enabled = true;
            Burst.enabled = true;
            Explosive.enabled = true;
            energyBar.enabled = true;
            ammovalue.enabled = true;
            switch (_basePlayer.FiringMode)
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
        } else if(!GunMode)
        {
            Single.enabled = false;
            Auto.enabled = false;
            Burst.enabled = false;
            Explosive.enabled = false;
            energyBar.enabled = false;
            ammovalue.enabled = false;
            MenuText.enabled = true;
            switch(MenuAction)
            {
                case 0:
                    MenuText.text = "Start";
                    break;
                case 1:
                    MenuText.text = "Restart";
                    break;
                case 2:
                    MenuText.text = "Help";
                    break;
                case 3:
                    MenuText.text = "Quit";
                    break;
            }
        }
    }
	void ChangeGuiColor() {
        GUIColor = Color.Lerp(Color.red, Color.green, (_basePlayer.currentEnergy / 100));
        Single.color = GUIColor;
        Auto.color = GUIColor;
        Burst.color = GUIColor;
        Explosive.color = GUIColor;
        ammovalue.color = GUIColor;
        energyBarFill.color = GUIColor;
        RightArrow.color = GUIColor;
        LeftArrow.color = GUIColor;
        MenuIcon.color = GUIColor;
        MenuText.color = GUIColor;
	    UpdateGunDisplay();
    }

    public void ChangeMenuAction()
    {
        if(MenuAction > 3)
        {
            MenuAction = 0;
        }
        if(MenuAction < 0)
        {
            MenuAction = 3;
        }
        UpdateGunDisplay();
    }
    public void HandleAction()
    {
        if (_restartMode)
        {
            _gameController.Restart();
        }
        switch (MenuAction)
        {
            case 1:
                _gameController.Restart();
                return;
            case 3:
                Application.Quit();
                return;
        }
        if (!_gameController.IsRunning)
        {
            HandleNonRunningMenuActions();
        }
    }

    public void HandleNonRunningMenuActions()
    {

        switch (MenuAction)
        {
            case 0:
                _gameController.StartGame();
                return;
            case 2:

                /*helpCanvas.SetActive(!helpCanvas.activeSelf);*/
                return;
        }
    }

    public void RestartMode()
    {
        _restartMode = true;
        UpdateGunDisplay();
    }
}
