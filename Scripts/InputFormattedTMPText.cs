using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AeLa.Utilities.Input
{
	public class InputFormattedTMPText : MonoBehaviour
	{
		public PlayerInput PlayerInput;
		private TMP_Text tmp;

		private string formatString;

		private async void OnEnable()
		{
			if (!InputFormat.Initialized)
			{
				await InputFormat.Initialize();
			}

			if (!tmp)
			{
				tmp = GetComponent<TMP_Text>();
				formatString = tmp.text;
			}

			PlayerInput.onControlsChanged += OnControlsChanged;
			UpdateText();
		}

		private void OnDisable()
		{
			PlayerInput.onControlsChanged -= OnControlsChanged;
		}

		private void OnControlsChanged(PlayerInput obj)
		{
			UpdateText();
		}

		private void UpdateText()
		{
			tmp.text = InputFormat.Format(formatString, PlayerInput);
		}
	}
}