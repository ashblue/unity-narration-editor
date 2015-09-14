using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.Narration {
	public class NarrationWindow : EditorWindow {
		static NarrationDatabase database; // Current database we are editing
		static List<NarrationBase> narrationTmp; // Temporary dump of narrations for filter purspoes

		GUIStyle titleStyle; // Style used for title in upper left
		GUIStyle errorStyle; // Used for error styling
		GUIStyle errorFoldoutStyle;

		int paddingSize = 15; // Total padding wrapping the window
		GUIStyle containerPadding;

		string filter; // Current search target
		Vector2 scrollPos; // Scroll window details
		int deleteIndex = -1;

		[MenuItem("Window/Narration Editor")]
		public static void ShowEditor () {
			// Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow<NarrationWindow>("Narration Editor");
		}

		void OnEnable () {
			containerPadding = new GUIStyle();
			containerPadding.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);

			titleStyle = new GUIStyle();
			titleStyle.fontSize = 20;

			errorStyle = new GUIStyle();
			errorStyle.normal.textColor = Color.red;
		}

		NarrationBase narration;
		bool errorId;
		void OnGUI () {
			// We have to get the foldout error style in OnGUI or it will error on us
			if (errorFoldoutStyle == null) errorFoldoutStyle = GetFoldoutErrorStyle();

			EditorGUILayout.BeginVertical(containerPadding); // BEGIN Padding

			/***** BEGIN Header *****/
			if (database == null) {
				GUILayout.Label("Narration Database", titleStyle);
				GUILayout.Label("Please select a narration database from the assets and click the edit " +
					"button in the inspector pannel (or create one if you haven't).");
				return;
			}

			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck();

			GUILayout.Label(string.Format("Narration Database: {0}", database.title), titleStyle);

			GUI.SetNextControlName("Filter");
			if (GUI.GetNameOfFocusedControl() == "Filter") {
				filter = EditorGUILayout.TextField(filter);
			} else {
				EditorGUILayout.TextField("Filter");
			}

			if (EditorGUI.EndChangeCheck()) {
				FilterNarrations(filter);
			}

			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Add Narration")) AddNarration(true);
			/***** END Header *****/

			/***** BEGIN Body *****/
			EditorGUILayout.EndVertical(); // END Padding

			scrollPos = GUILayout.BeginScrollView(scrollPos);
			EditorGUILayout.BeginVertical(containerPadding); // BEGIN Padding

			EditorGUI.BeginChangeCheck();
			for (int i = 0, l = narrationTmp.Count; i < l; i++) {
				narration = narrationTmp[i];
			
				errorId = string.IsNullOrEmpty(narration.id);

				if (!errorId) {
					narration.expanded = EditorGUILayout.Foldout(narration.expanded, narration.displayName);
				} else {
					narration.expanded = EditorGUILayout.Foldout(narration.expanded, 
					                                            string.Format("{0}: ID cannot be left blank and must be unique", narration.displayName), 
					                                            errorFoldoutStyle);
				}

				if (narration.expanded) {
					BeginIndent(20f);
					
					narration.displayName = EditorGUILayout.TextField("Display Name", narration.displayName);
					narration.id = EditorGUILayout.TextField("ID", narration.id);
					narration.filename = (AudioClip)EditorGUILayout.ObjectField("Narration Audio", narration.filename, typeof(AudioClip), false);

					EditorGUILayout.LabelField("Subtitles");
					narration.subtitles = GUILayout.TextArea(narration.subtitles, GUILayout.MaxHeight(60f), GUILayout.Width(350f));

					EditorGUILayout.LabelField("Notes");
					narration.notes = GUILayout.TextArea(narration.notes, GUILayout.MaxHeight(60f), GUILayout.Width(300f));

					if (GUILayout.Button(string.Format("Remove '{0}'", narration.displayName))) {
						if (ConfirmDelete(narration.displayName)) {
							deleteIndex = i;
						}
					}

					EndIndent();
				}
			}

			if (EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(database);
			}

			EditorGUILayout.EndVertical(); // END Padding
			GUILayout.EndScrollView();
			/***** END Body *****/			

			if (deleteIndex != -1) {
				RemoveNarration(deleteIndex);
				deleteIndex = -1;
			}
		}

		bool ConfirmDelete (string itemName) {
			return EditorUtility.DisplayDialog("Delete Item", 
			                                   string.Format("Are you sure you want to delete '{0}'", itemName), 
			                                   string.Format("Delete '{0}'", itemName),
			                                   "Cancel"
			);
		}
		
		void FilterNarrations (string search) {
			if (string.IsNullOrEmpty(search)) {
				narrationTmp = database.narrations;
				return;
			}

			string[] searchBits = search.ToLower().Split(' ');
			List<NarrationBase> matches = database.narrations.Where(d => searchBits.All(n => d.displayName.ToLower().Contains(n))).ToList();
		
			narrationTmp = matches;
		}

		void AddNarration (bool placeAtTop) {
			database.narrations.Insert(0, new NarrationDefault());

			FilterNarrations(filter);
			EditorUtility.SetDirty(database);
		}

		void RemoveNarration (int index) {
			database.narrations.RemoveAt(index);
			FilterNarrations(filter);
			EditorUtility.SetDirty(database);
		}

		void BeginIndent (float indent) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(indent);
			EditorGUILayout.BeginVertical();
		}

		void EndIndent () {
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		static GUIStyle GetFoldoutErrorStyle () {
			GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
			Color myStyleColor = Color.red;
			myFoldoutStyle.normal.textColor = myStyleColor;
			myFoldoutStyle.onNormal.textColor = myStyleColor;
			myFoldoutStyle.hover.textColor = myStyleColor;
			myFoldoutStyle.onHover.textColor = myStyleColor;
			myFoldoutStyle.focused.textColor = myStyleColor;
			myFoldoutStyle.onFocused.textColor = myStyleColor;
			myFoldoutStyle.active.textColor = myStyleColor;
			myFoldoutStyle.onActive.textColor = myStyleColor;

			return myFoldoutStyle;
		}

		public static void SetDatabase (NarrationDatabase newDatabase) {
			database = newDatabase;
			narrationTmp = database.narrations;
		}
	}
}
