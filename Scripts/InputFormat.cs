using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

namespace AeLa.Utilities.Input
{
	[PublicAPI]
	public static class InputFormat
	{
		public static bool Initialized { get; private set; }

		/// <summary>
		/// Call this to make sure all resources are loaded and initialized before using
		/// </summary>
		public static async Task Initialize()
		{
			if (Initialized) return;

			await InputGlyphMap.Initialize();
			Initialized = true;
		}

		/// <summary>
		/// Replaces any input path format strings (denoted with handlebars {{}}) in <paramref name="str"/> with the appropriate values.
		/// </summary>
		/// <param name="str">The string to be formatted</param>
		/// <param name="playerInput">The <see cref="PlayerInput"/> to use for input layouts and active devices</param>
		/// <param name="glyphs">Whether to include TMP SpriteAsset glyph tags</param>
		public static string Format(string str, PlayerInput playerInput, bool glyphs = true)
		{
			const string pattern = "{{(?<id>.*?)/(?<prop>.*?)}}";
			var formattedText = str;
			foreach (Match match in Regex.Matches(str, pattern))
			{
				var id = match.Groups["id"].Value;
				var prop = match.Groups["prop"].Value;

				var action = playerInput.actions.FindAction(id);

				if (action == null) continue;

				var controlScheme = playerInput.currentControlScheme;

				formattedText = formattedText.Replace(match.Value, GetPropertyText(action, prop, controlScheme, glyphs));
			}

			return formattedText;
		}

		private static string GetPropertyText(InputAction action, string prop, string controlScheme, bool glyphs)
		{
			return prop switch
			{
				"bindingGlyph" when !glyphs => string.Empty,
				"bindingGlyph" => action.GetBindingGlyphTag(controlScheme),
				"binding" => action.GetBindingDisplayString(),
				"name" => action.name,
				_ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
			};
		}
	}
}