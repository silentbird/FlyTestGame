using UnityEngine;

namespace HotFix.module.ui.core {
	public class UIPanel {
		private GameObject gameObject;
		private int viewId;
		private string name;
		private IUIBehavior[] _behaviors;


		UIPanel(GameObject go) {
			this.gameObject = go;
		}

		private void init_behaviors() {
			foreach (var behavior in _behaviors) {
				behavior.Init();
			}
		}

		protected virtual void onOpen() {
			init_behaviors();
		}

		protected virtual void onClose() {
		}

		public void Show() {
			this.gameObject.SetActive(true);
			this.onOpen();
		}

		public void Hide() {
			this.gameObject.SetActive(false);
		}

		public void Destroy() {
		}

		public void AddBehavior(IUIBehavior behavior) {
		}

		public void RemoveBehavior() {
		}
	}
}