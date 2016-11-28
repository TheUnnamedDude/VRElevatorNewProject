using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;
using System;

public class MenuHandler : MonoBehaviour, Shootable {

	public MenuAction Action;
	public Text text;
    public GameObject helpCanvas;
	
    [Inject]
    private GameController _gameController;
    private LevelGenerator _levelGenerator;
	
    

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Controller")
		{
            HandleAction();
		}
	}

    public void OnHit(float damage)
    {
        HandleAction();
    }

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
                _levelGenerator.Reset();
                _gameController.StartGame();
                return;
            case MenuAction.Help:
                if(!helpCanvas.activeInHierarchy)
                {
                    helpCanvas.SetActive(true);
                } else
                {
                    helpCanvas.SetActive(false);
                }
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
