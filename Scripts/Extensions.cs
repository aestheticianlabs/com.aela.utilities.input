using UnityEngine.InputSystem;

namespace AeLa.Utilities.Input
{
	public static class Extensions
	{
		/// <summary>
		/// Returns the TMP sprite glyph tag for the provided input action and control scheme
		/// </summary>
		public static string GetBindingGlyphTag(this InputAction action, string controlScheme)
		{
			// TODO: better default bad value?
			return $"<sprite={InputGlyphMap.GetGlyphID(action, controlScheme)}>";
		}
	}
}