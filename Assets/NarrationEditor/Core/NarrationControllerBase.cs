using UnityEngine;
using System.Collections.Generic;

namespace Adnc.Narration {
	public abstract class NarrationControllerBase : MonoBehaviour {
		[SerializeField] NarrationDatabase database;
		public Dictionary<string, NarrationBase> narrationDef = new Dictionary<string, NarrationBase>(); // Defenitions used in retrieving decision data

		public virtual void Awake () {
			Debug.Assert(database != null, "You must include a narration database for the NarrationController to fully load");
			foreach (NarrationBase d in database.narrations) {
				narrationDef[d.id] = d;
			}
		}

		public virtual NarrationBase GetNarration (string id) {
			NarrationBase result;
			if (narrationDef.TryGetValue(id, out result)) {
				return result;
			} else {
				Debug.LogErrorFormat("Narration ID {0} does not exist. Please fix.", id);
				return null;
			}
		}
	}
}
