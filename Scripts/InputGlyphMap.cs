using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

namespace AeLa.Utilities.Input
{
	[PublicAPI]
	public static class InputGlyphMap
	{
		private static IEnumerable<InputGlyphMapAsset> maps;

		public static bool Initialized { get; private set; }

		/// <summary>
		/// Call this to make sure all resources are loaded and initialized before using
		/// </summary>
		public static async Task Initialize()
		{
			if (Initialized) return;

			// load input glyph map assets from addressables
			maps = await Addressables.LoadAssetsAsync<InputGlyphMapAsset>("InputGlyphMap", null).Task;

			// order by priority
			maps = maps.OrderByDescending(map => map.Priority);

			Initialized = true;
		}

		// TODO: handle multiple bindings (return list/array?)

		/// <summary>
		/// Returns the TMP sprite glyph ID for the provided <see cref="InputAction"/>
		/// </summary>
		/// <param name="inputAction">The input action</param>
		/// <param name="controlScheme">The current control scheme</param>
		/// <returns>-1 if no glyph found</returns>
		public static int GetGlyphID(InputAction inputAction, string controlScheme)
		{
			if (inputAction.controls.Count == 0) return -1;

			return GetGlyphID(inputAction.controls[0].path, controlScheme);
		}

		/// <summary>
		/// Returns the TMP sprite glyph ID for the provided control path
		/// </summary>
		/// <param name="controlPath">The <see cref="InputControl"/>'s path</param>
		/// <param name="controlScheme">The current control scheme</param>
		/// <returns>-1 if no glyph found</returns>
		public static int GetGlyphID(string controlPath, string controlScheme)
		{
			foreach (var map in maps)
			{
				if (!map.TryGetGlyphID(controlPath, controlScheme, out var id)) continue;

				return id;
			}


			return -1;
		}
	}
}