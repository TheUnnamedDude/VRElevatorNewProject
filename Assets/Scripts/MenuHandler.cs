using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;
using System;

public class MenuHandler : MonoBehaviour {

	public MenuAction Action;
	public Text text;
    public GameObject helpCanvas;
	
    [Inject]
    private GameController _gameController;
    private LevelGenerator _levelGenerator;



    public void HandleAction() {
        if (_gameController.IsRunning)
            return;
        switch (Action)
        {
            case MenuAction.Start:
                _gameController.StartGame();
                return;
            case MenuAction.Quit:
                Application.Quit();
                return;
            case MenuAction.Restart:
                _gameController.ResetGame();
                return;
            case MenuAction.Help:
                helpCanvas.SetActive(!helpCanvas.activeInHierarchy);
                return;
        }
    }

    public enum MenuAction {
		Quit,
		Restart,
		Start,
		Help
	}
}
