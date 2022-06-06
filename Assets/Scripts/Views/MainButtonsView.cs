using UnityEngine;
using UnityEngine.UI;

public class MainButtonsView : MonoBehaviour {

	[SerializeField] private Button startButton;
	[SerializeField] private Button pauseButton;
	[SerializeField] private Button exitButton;

	[SerializeField] private Text hitsText;
	[SerializeField] private Text failsText;

	public event System.Action OnStartClicked;
	public event System.Action OnPauseClicked;
	public event System.Action OnExitClicked;

	public Text HitsText {
		get { return hitsText; }
	}

	public Text FailsText {
		get { return failsText; }
	}

	public void StartClicked() {

		if (OnStartClicked != null) {

			OnStartClicked();
		}
	}
	public void PauseClicked() {

		if (OnPauseClicked != null) {

			OnPauseClicked();
		}
	}

	public void ExitClicked() {

		if (OnExitClicked != null) {

			OnExitClicked();
		}

		UnityEditor.EditorApplication.isPlaying = false;
	}

	public void ShowStartButton() {

		startButton.gameObject.SetActive(true);
	}

	public void HideStartButton() {

		startButton.gameObject.SetActive(false);
	}

	public void ShowPauseButton() {

		pauseButton.gameObject.SetActive(true);
	}
	public void HidePauseButton() {

		pauseButton.gameObject.SetActive(true);
	}

	public void ShowExitButton() {

		exitButton.gameObject.SetActive(true);
	}

	public void HideExitButton() {

		exitButton.gameObject.SetActive(false);
	}
}
