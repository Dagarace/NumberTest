using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameplayView : MonoBehaviour
{
	[SerializeField] private Text statementText;

	[SerializeField] private Button option1Button;
	[SerializeField] private Button option2Button;
	[SerializeField] private Button option3Button;

	[SerializeField] private Text option1Text;
	[SerializeField] private Text option2Text;
	[SerializeField] private Text option3Text;

	[SerializeField] private Image option1Background;
	[SerializeField] private Image option2Background;
	[SerializeField] private Image option3Background;


	public event System.Action<int> OnOptionClicked;

	public GameObject Statement {
		get { return statementText.gameObject; }
	}

	public void SetStatementText(List<KeyValuePair<string, int>> attemptOptions, int correctOption) {

		statementText.text = attemptOptions[correctOption].Key;
	}

	public void SetButtonsText(List<KeyValuePair<string, int>> attemptOptions) {

		option1Text.text = attemptOptions[0].Value.ToString();
		option2Text.text = attemptOptions[1].Value.ToString();
		option3Text.text = attemptOptions[2].Value.ToString();
	}

	public void SetCorrectButtonBackgroundColor(int option) {

		switch (option) {

			case 0:
				option1Background.color = Color.green;
				break;
			case 1:
				option2Background.color = Color.green;
				break;
			case 2:
				option3Background.color = Color.green;
				break;
		}
	}

	public void SetWrongButtonBackgroundColor(int option) {

		switch (option) {

			case 0:
				option1Background.color = Color.red;
				break;
			case 1:
				option2Background.color = Color.red;
				break;
			case 2:
				option3Background.color = Color.red;
				break;
		}
	}

	public void SetButtonsDefaultBackgroundColor() {

		option1Background.color = Color.white;
		option2Background.color = Color.white;
		option3Background.color = Color.white;
	}

	public IEnumerator ShowStatement() {

		yield return StartCoroutine(FadeInFadeOut(Statement.transform));
	}

	public IEnumerator ShowOptions() {

		Coroutine opt1 = StartCoroutine(FadeIn(option1Button.transform));
		Coroutine opt2 = StartCoroutine(FadeIn(option2Button.transform));
		Coroutine opt3 = StartCoroutine(FadeIn(option3Button.transform));

		yield return opt1;
		yield return opt2;
		yield return opt3;
	}

	public IEnumerator RunCorrectAnswerVisuals(int option) {

		Coroutine opt1 = StartCoroutine(FadeOut(option1Button.transform));
		Coroutine opt2 = StartCoroutine(FadeOut(option2Button.transform));
		Coroutine opt3 = StartCoroutine(FadeOut(option3Button.transform));

		SetCorrectButtonBackgroundColor(option);

		yield return opt1;
		yield return opt2;
		yield return opt3;
	}

	public IEnumerator RunFirstWrongAnswerVisuals(int buttonToDisable) {

		SetWrongButtonBackgroundColor(buttonToDisable);
		switch (buttonToDisable) {

			case 0:
				yield return StartCoroutine(FadeOut(option1Button.transform));
				break;
			case 1:
				yield return StartCoroutine(FadeOut(option2Button.transform));
				break;
			case 2:
				yield return StartCoroutine(FadeOut(option3Button.transform));
				break;
		}
	}

	public IEnumerator RunSecondWrongAnswerVisuals(int wrongOption, int correctOption) {

		SetWrongButtonBackgroundColor(wrongOption);
		SetCorrectButtonBackgroundColor(correctOption);

		Coroutine opt1 = StartCoroutine(FadeOut(option1Button.transform));
		Coroutine opt2 = StartCoroutine(FadeOut(option2Button.transform));
		Coroutine opt3 = StartCoroutine(FadeOut(option3Button.transform));

		yield return opt1;
		yield return opt2;
		yield return opt3;
	}

	public void EnableButtonsClick() {

		option1Button.enabled = true;
		option2Button.enabled = true;
		option3Button.enabled = true;
	}

	public void DisableButtonsClick() {

		option1Button.enabled = false;
		option2Button.enabled = false;
		option3Button.enabled = false;
	}

	private IEnumerator FadeIn(Transform transform) {

		transform.gameObject.SetActive(true);
		yield return StartCoroutine(ScaleUp(transform, new Vector3(1, 1, 1), 2f));
	}
	private IEnumerator FadeOut(Transform transform) {

		yield return StartCoroutine(ScaleDown(transform, new Vector3(0, 0, 1), 2f));
		transform.gameObject.SetActive(false);
	}

	private IEnumerator FadeInFadeOut(Transform transform) {

		transform.gameObject.SetActive(true);

		yield return StartCoroutine(ScaleUp(transform, new Vector3(1, 1, 1), 2f));
		yield return StartCoroutine(Wait(2));
		yield return StartCoroutine(ScaleDown(transform, new Vector3(0, 0, 1), 2f));

		transform.gameObject.SetActive(false);
	}

	private IEnumerator ScaleUp(Transform transform, Vector3 upScale, float duration) {

		transform.localScale = new Vector3(0, 0, 1);
		Vector3 initialScale = transform.localScale;
		float time = 0;
		while (transform.localScale != upScale) {
			time += Time.deltaTime;
			float progress = time / duration;
			transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
			yield return null;
		}
	}

	private IEnumerator ScaleDown(Transform transform, Vector3 downScale, float duration) {

		Vector3 initialScale = transform.localScale;
		float time = 0;
		while (transform.localScale != downScale) {
			time += Time.deltaTime;
			float progress = time / duration;
			transform.localScale = Vector3.Lerp(initialScale, downScale, progress);
			yield return null;
		}
	}

	private IEnumerator Wait(float seconds) {

		yield return new WaitForSeconds(2);
	}

	public void OptionClicked(int option) {

		if (OnOptionClicked != null) {

			OnOptionClicked(option);
		}
	}
}
