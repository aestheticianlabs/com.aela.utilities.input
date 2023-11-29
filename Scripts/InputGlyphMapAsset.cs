using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AeLa.Utilities.Input
{
	/// <summary>
	/// Maps input names to TMP sprite glyph IDs for <see cref="InputGlyphMap"/>.
	/// Be sure to mark all input glyph map assets as addressable and give them the "InputGlyphMap" tag!
	/// </summary>
	[CreateAssetMenu(menuName = "Input/Input Glyph Map")]
	public class InputGlyphMapAsset : ScriptableObject
	{
		/// <summary>
		/// Regex pattern to used to match input devices to this map
		/// </summary>
		[Tooltip("Regex pattern to used to match input devices to this map")]
		public string InputDevicePattern;

		/// <summary>
		/// Impacts order in which maps are checked when selecting a glyph. Higher = earlier.
		/// </summary>
		[Tooltip("Impacts order in which maps are checked when selecting a glyph. Higher = earlier.")]
		public int Priority = 0;

		public List<GlyphBinding> Bindings;

		/// <summary>
		/// Returns whether the device with the given control path and scheme match this map
		/// </summary>
		/// <param name="controlPath">The <see cref="InputControl"/>'s path</param>
		/// <param name="controlScheme">The current control scheme</param>
		public bool IsDeviceMatch(string controlPath, string controlScheme) =>
			Regex.IsMatch(controlPath, InputDevicePattern) || Regex.IsMatch(controlScheme, InputDevicePattern);

		/// <summary>
		/// Tries to get the glyph id for the provided control path and scheme.
		/// </summary>
		/// <param name="controlPath">The <see cref="InputControl"/>'s path</param>
		/// <param name="controlScheme">The current control scheme</param>
		/// <param name="id">The TMP sprite glyph ID</param>
		/// <returns>Returns whether a glyph ID was found for this map</returns>
		public bool TryGetGlyphID(string controlPath, string controlScheme, out int id)
		{
			id = -1;
			if (!IsDeviceMatch(controlPath, controlScheme)) return false;

			foreach (var binding in Bindings)
			{
				if (!binding.IsMatch(controlPath)) continue;

				id = binding.GlyphID;
				return true;
			}

			return false;
		}
	}

	[Serializable]
	public struct GlyphBinding
	{
		/// <summary>
		/// Regex pattern used to match input control paths to this glyph
		/// </summary>
		[Tooltip("Regex pattern used to match input control paths to this glyph")]
		public string ControlPathPattern;

		// TODO: it would be cool to show the TMP glyph in the binding inspector or even allow visual selection
		/// <summary>
		/// The ID of the glyph in the TMP SpriteAsset
		/// </summary>
		[Tooltip("The ID of the glyph in the TMP SpriteAsset")]
		public int GlyphID;

		/// <summary>
		/// Returns whether the provided control path matches this binding
		/// </summary>
		public bool IsMatch(string controlPath) => Regex.IsMatch(controlPath, ControlPathPattern);
	}
}