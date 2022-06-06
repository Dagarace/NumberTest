using UnityEngine;
using System.Collections.Generic;

public class MainFlow : MonoBehaviour {

	GameStates gameState;

	MainUIController mainUIController;
	GameplayController gameplayController;

	private List<KeyValuePair<string, int>> numbers = new List<KeyValuePair<string, int>>();

	private int numberOfHits;
	private int numberOfFails;

	void Awake() {

		InitNumbersList();

		mainUIController = GetComponent<MainUIController>();
		gameplayController = GetComponent<GameplayController>();
		mainUIController.CreateMainUI();
		mainUIController.InitMainUI();

		gameState = GameStates.MainMenu;
		mainUIController.OnGameFlowStarted += OnGameFlowStarted;
		mainUIController.OnGameFlowReStarted += OnGameFlowReStarted;
		mainUIController.OnGameFlowPaused += OnGameFlowPaused;
		gameplayController.OnAttemptFinished += OnAttemptFinished;
	}

	private void OnGameFlowStarted() {

		gameState = GameStates.Playing;

		gameplayController.SelectRandomOptionsFromList(numbers);
		gameplayController.CreateGameplayView();

		RunGameFlow();
	}

	private void OnGameFlowReStarted() {

		gameState = GameStates.Playing;
		gameplayController.EnableButtonInput();
		Time.timeScale = 1;
	}

	private void OnGameFlowPaused() {

		gameState = GameStates.Paused;
		gameplayController.DisableButtonInput();
		Time.timeScale = 0;
	}

	private void OnAttemptFinished(bool isSuccessfulAttempt) {

		UpdateCounters(isSuccessfulAttempt);

		gameplayController.SelectRandomOptionsFromList(numbers);
		RunGameFlow();
	}

	private void InitNumbersList() {

		numbers.Add(new KeyValuePair<string, int>("Uno", 1));
		numbers.Add(new KeyValuePair<string, int>("Dos", 2));
		numbers.Add(new KeyValuePair<string, int>("Tres", 3));
		numbers.Add(new KeyValuePair<string, int>("Cuatro", 4));
		numbers.Add(new KeyValuePair<string, int>("Cinco", 5));
		numbers.Add(new KeyValuePair<string, int>("Seis", 6));
		numbers.Add(new KeyValuePair<string, int>("Siete", 7));
		numbers.Add(new KeyValuePair<string, int>("Ocho", 8));
		numbers.Add(new KeyValuePair<string, int>("Nueve", 9));
		numbers.Add(new KeyValuePair<string, int>("Diez", 10));

	}

	private void RunGameFlow() {

		if (gameState == GameStates.Playing && !gameplayController.isRunningAttempt) {

			StartCoroutine(gameplayController.Run());
		}
	}


	private void UpdateCounters(bool isCorrect) {

		if (isCorrect) {

			numberOfHits++;
		} else {

			numberOfFails++;
		}

		UpdateCountersVisuals();
	}

	private void UpdateCountersVisuals() {

		mainUIController.SetHitsText("Aciertos: "+numberOfHits.ToString());
		mainUIController.SetFailsText("Fallos: "+numberOfFails.ToString());
	}
}

public enum GameStates { MainMenu, Playing, Paused }
