using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayController : MonoBehaviour
{
	const string MAIN_GAMEPLAY_VIEW_PATH = "Prefabs/GameplayView";
	private GameplayView gameplayView;

	public bool isRunningAttempt = false;
	public event System.Action<bool> OnAttemptFinished;

	private int attemptNumber;
	private bool isOptionChoosed;
	private PlayerInput playerInput;

	private List<KeyValuePair<string, int>> attemptOptions = new List<KeyValuePair<string, int>>();
	private int correctOption;



	public void CreateGameplayView() {

		GameObject view = Instantiate(Resources.Load(MAIN_GAMEPLAY_VIEW_PATH)) as GameObject;
		view.transform.parent = this.transform;
		gameplayView = view.GetComponent<GameplayView>();
		gameplayView.OnOptionClicked += OnOptionClicked;
	}

	public IEnumerator Run() {

		ResetGameplayParameters();
		InitAttemptData();

		yield return StartCoroutine(InitGameplayView());

		playerInput = new PlayerInput();

		yield return StartCoroutine(WaitForUserInput());

		CheckAnswerCorrect(playerInput);

		yield return StartCoroutine(ResolveVisuals(playerInput));

		yield return StartCoroutine(FinishAttempt());
	}

	public void SelectRandomOptionsFromList(List<KeyValuePair<string, int>> numbers) {

		attemptOptions.Clear();
		List<int> positions = new List<int>();

		while (attemptOptions.Count != 3) {
			
			int position = Random.Range(0, numbers.Count);

			if (!positions.Contains(position)) {

				positions.Add(position);
				attemptOptions.Add(numbers[position]);
			}
		}

		correctOption = Random.Range(0, 3);
	}

	private void InitAttemptData() {

		gameplayView.SetStatementText(attemptOptions, correctOption);
		gameplayView.SetButtonsText(attemptOptions);
	}

	private IEnumerator InitGameplayView() {

		gameplayView.DisableButtonsClick();

		yield return StartCoroutine(gameplayView.ShowStatement());
		yield return StartCoroutine(gameplayView.ShowOptions());
	}

	private IEnumerator WaitForUserInput() {

		gameplayView.EnableButtonsClick();
		while (!isOptionChoosed) {

			yield return null;
		}

		isOptionChoosed = false;
		gameplayView.DisableButtonsClick();
		yield break;
	}

	public void OnOptionClicked(int option) {

		gameplayView.DisableButtonsClick();
		playerInput.OptionChoosed = option;
		isOptionChoosed = true;
		attemptNumber++;
	}

	private bool CheckAnswerCorrect(PlayerInput input) {

		if(input.OptionChoosed == correctOption) {

			input.isCorrect = true;

		} else if (attemptNumber == 1) {

			input.isFirstAttempt = true;
			input.isCorrect = false;
		}else {

			input.isFirstAttempt = false;
			input.isCorrect = false;
		}
		playerInput.isFirstAttempt = input.isFirstAttempt;
		playerInput.isCorrect = input.isCorrect;

		return input.isCorrect;
	}

	private IEnumerator ResolveVisuals(PlayerInput input) {

		if (input.isCorrect) {

			yield return StartCoroutine(gameplayView.RunCorrectAnswerVisuals(input.OptionChoosed));

		} else if(input.isFirstAttempt) {

			yield return StartCoroutine(gameplayView.RunFirstWrongAnswerVisuals(input.OptionChoosed));

		} else {

			yield return StartCoroutine(gameplayView.RunSecondWrongAnswerVisuals(input.OptionChoosed, correctOption));
		}
	}

	private IEnumerator FinishAttempt() {

		gameplayView.SetButtonsDefaultBackgroundColor();
		if (playerInput.isCorrect) {

			isRunningAttempt = false;
			OnAttemptFinished(true);

		} else if (playerInput.isFirstAttempt) {

			yield return StartCoroutine(WaitForUserInput());
			CheckAnswerCorrect(playerInput);
			yield return StartCoroutine(ResolveVisuals(playerInput));
			yield return StartCoroutine(FinishAttempt());
		} else {

			isRunningAttempt = false;
			OnAttemptFinished(false);
		}
	}

	private void ResetGameplayParameters() {

		isRunningAttempt = true;
		attemptNumber = 0;
		isOptionChoosed = false;
	}
	public void EnableButtonInput() {

		gameplayView.EnableButtonsClick();
	}
	public void DisableButtonInput() {

		gameplayView.DisableButtonsClick();
	}
}

struct PlayerInput {

	public int OptionChoosed;
	public bool isCorrect;
	public bool isFirstAttempt;
}
