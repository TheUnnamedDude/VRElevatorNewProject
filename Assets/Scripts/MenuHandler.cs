using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;
using System;

public class MenuHandler : MonoBehaviour, Shootable {

	public MenuAction Action;
	public Text text;
	
    [Inject]
    private GameController _gameController;

	bool isPaused = false;


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
            case MenuAction.Pause:
                if (!isPaused)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
                return;
            case MenuAction.Help:
                //Spawn help canvas (and pause?)
                return;
        }
    }

    public enum MenuAction {
		Quit,
		Pause,
		Start,
		Help
	}
}
