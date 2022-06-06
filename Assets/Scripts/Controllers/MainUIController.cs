using UnityEngine;

public class MainUIController : MonoBehaviour {

	const string MAIN_BUTTONS_PATH = "Prefabs/MainButtonsView";

	public event System.Action OnGameFlowStarted;
	public event System.Action OnGameFlowReStarted;
	public event System.Action OnGameFlowPaused;

	private MainButtonsView mainButtonsView;

	private bool isPaused;

	public void CreateMainUI() {

		GameObject view = Instantiate(Resources.Load(MAIN_BUTTONS_PATH)) as GameObject;
		view.transform.parent = this.transform;
		mainButtonsView = view.GetComponent<MainButtonsView>();
	}

	public void InitMainUI() {

		isPaused = false;
		mainButtonsView.OnStartClicked += OnStartClicked;
		mainButtonsView.OnPauseClicked += OnPauseClicked;
	}

	public void SetHitsText(string text) {

		mainButtonsView.HitsText.text = text;
	}

	public void SetFailsText(string text) {

		mainButtonsView.FailsText.text = text;
	}

	private void OnStartClicked() {

		mainButtonsView.HideStartButton();
		mainButtonsView.ShowPauseButton();
		OnGameFlowStarted();
	}

	private void OnPauseClicked() {

		if(isPaused) {

			mainButtonsView.HideExitButton();
			isPaused = false;
			OnGameFlowReStarted();
		} else {

			mainButtonsView.ShowExitButton();
			isPaused = true;
			OnGameFlowPaused();
		}
	}
}
