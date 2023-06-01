using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfidenceViewController : MonoBehaviour
{
	public ToggleGroup toggleGroup;
	public Toggle[] toggles;
	public GameObject button;
	private void Awake()
	{
		button.SetActive(false);
		SetupToggles();
	}

	private void SetupToggles()
	{
		toggleGroup.SetAllTogglesOff();
		for (int i = 0; i < toggles.Length; i++)
		{
			int index = i;
			toggles[index].onValueChanged.AddListener((x) =>
			{
				if (x)
					OnConfidenceChanged(index);
			});
		}
	}

	private void OnConfidenceChanged(int confidence)
	{
		button.SetActive(true);
		DataManager.RecordingTrial.ConfidenceRating = confidence + 1;
	}
}
