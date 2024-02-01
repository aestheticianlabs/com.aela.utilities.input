using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace AeLa.Utilities.Input
{
	public class InputFormattedTMPText : MonoBehaviour
	{
		[FormerlySerializedAs("PlayerInput")] [SerializeField] private PlayerInput playerInput;
		private PlayerInput currentPlayerInput;
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

			SetPlayerInput(playerInput);
		}

		public void SetPlayerInput(PlayerInput playerInput)
		{
			if (currentPlayerInput == playerInput) return;

			if (currentPlayerInput)
			{
				currentPlayerInput.onControlsChanged -= OnControlsChanged;
			}

			currentPlayerInput = playerInput;
			if (!playerInput) return;

			playerInput.onControlsChanged += OnControlsChanged;
			UpdateText();
		}

		private void OnDisable()
		{
			if (currentPlayerInput)
			{
				currentPlayerInput.onControlsChanged -= OnControlsChanged;
			}
		}

		private void OnControlsChanged(PlayerInput obj)
		{
			UpdateText();
		}

		private void UpdateText()
		{
			tmp.text = InputFormat.Format(formatString, playerInput);
		}
	}
}