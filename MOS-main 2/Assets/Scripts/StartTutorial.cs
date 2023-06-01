using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class StartTutorial : MonoBehaviour
{
	public TMP_InputField prolificIDInput;
	public UnityEngine.UI.Button startButton;

	private void Awake()
	{
		startButton.interactable = false;
		prolificIDInput.onValueChanged.AddListener((x) =>
		{
			startButton.interactable = true;
		});
	}

	public void PlayTutorial()
	{
		PlayerPrefs.SetString("ProlificID", prolificIDInput.text);
		SceneManager.LoadScene("MOSTutorial");		
	}
}
