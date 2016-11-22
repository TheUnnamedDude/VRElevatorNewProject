using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

	public MenuAction Action;
	public Text text;
	
	[Inject]
	private LevelGenerator _levelGenerator;

	bool isPaused = false;


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Controller")
		{
			switch (Action)
			{
				case MenuAction.Start:
					_levelGenerator.InitializeGame();
					return;
				case MenuAction.Quit:
					Application.Quit();
					return;
				case MenuAction.Pause:
					if(!isPaused) {
						Time.timeScale = 0f;
					} else {
						Time.timeScale = 1f;
					}
					return;
				case MenuAction.Help:
					//Spawn help canvas (and pause?)
					return;
			}
		}
	}

	public enum MenuAction {
		Quit,
		Pause,
		Start,
		Help
	}
}
