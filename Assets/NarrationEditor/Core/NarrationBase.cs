using UnityEngine;
using System.Collections;

namespace Adnc.Narration {
	[System.Serializable]
	public class NarrationBase {
		public string displayName = "Untitled";
		public string id = "";
		public string subtitles = "";
		public AudioClip filename;

		// Editor only values
		public string notes = "";
		public bool expanded = true;
	}
}
