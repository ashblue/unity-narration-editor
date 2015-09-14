using UnityEngine;
using System.Collections.Generic;

namespace Adnc.Narration {
	public class NarrationDatabase : ScriptableObject {
		public string title = "Untitled";

		[TextArea(3, 5)]
		public string description;

		[HideInInspector] public List<NarrationBase> narrations = new List<NarrationBase>();

		[EditNarrationDatabase]
		public string editDatabase = "Edit Narration Database";
	}
}
