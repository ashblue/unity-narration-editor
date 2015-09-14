using UnityEngine;
using UnityEditor;

namespace Adnc.Narration {
	[CustomPropertyDrawer(typeof(EditNarrationDatabaseAttribute))]
	public class EditDecisionDatabaseDrawer : PropertyDrawer {
		public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
			if (GUI.Button(position, prop.stringValue)) {
				NarrationWindow.SetDatabase(prop.serializedObject.targetObject as NarrationDatabase);
				NarrationWindow.ShowEditor();
			}
		}

	
	}
}
