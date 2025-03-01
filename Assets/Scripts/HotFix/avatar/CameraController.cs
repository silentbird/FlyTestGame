using UnityEngine;

namespace HotFix.avatar {
	[ExecuteAlways]
	public class CameraController : MonoBehaviour {
		[SerializeField]
		private Transform target; // 要跟随的目标

		[SerializeField]
		private Vector3 offset = new(0, 5, -10); // 相对目标的偏移位置

		[SerializeField, Range(0.1f, 10f)]
		private float smoothSpeed = 0.1f; // 跟随平滑度

		[SerializeField]
		private bool lookAtTarget = true; // 是否始终看向目标

		private void Update() {
			if (target == null)
				return;

			// 计算目标位置
			var desiredPosition = target.localPosition + offset;

			// 平滑移动到目标位置
			var smoothedPosition = Vector3.Lerp(transform.localPosition, desiredPosition, smoothSpeed * Time.deltaTime);
			transform.localPosition = new Vector3(desiredPosition.x, transform.localPosition.y, desiredPosition.z);

			// 如果需要，让相机始终看向目标
			if (lookAtTarget) transform.LookAt(target);
		}

		public void SetTarget(Transform newTarget) {
			target = newTarget;
		}

		public void SetOffset(Vector3 newOffset) {
			offset = newOffset;
		}
	}
}