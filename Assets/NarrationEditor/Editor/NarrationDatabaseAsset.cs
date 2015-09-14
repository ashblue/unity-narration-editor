using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Adnc.Narration {
	public class NarrationDatabaseAsset {  
		[MenuItem("Assets/Create/Narration Database")]
		public static void CreateDecisionDatabase () {
			NarrationDatabase asset = ScriptableObject.CreateInstance<NarrationDatabase>();

			AssetDatabase.CreateAsset(asset, GetPath() + "/NarrationDatabase.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;        
		}

		public static string GetPath () {
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
				}
				break;
			}

			return path;
		}
	}
}
