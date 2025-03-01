using UnityEngine;

namespace HotFix.avatar {
	[RequireComponent(typeof(CharacterController), typeof(Animator))]
	public class AvatarController : MonoBehaviour {
		[Header("移动设置"), SerializeField]
		private float moveSpeed = 5f;

		[SerializeField]
		private float rotationSpeed = 10f;

		[SerializeField]
		private float jumpForce = 5f;

		[SerializeField]
		private float gravity = -9.81f;

		[Header("动画参数名称"), SerializeField]
		private string speedParameterName = "Speed";

		[SerializeField]
		private string jumpParameterName = "IsJumping";

		private CharacterController characterController;
		private Animator animator;
		private Vector3 velocity;
		private bool isJumping;

		private void Awake() {
			characterController = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();
		}

		private void Update() {
			HandleMovement();
			HandleJump();
			ApplyGravity();
			UpdateAnimations();
		}

		private void HandleMovement() {
			// 获取输入
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");

			// 计算移动方向
			var movement = new Vector3(horizontal, 0f, vertical).normalized;

			if (movement.magnitude >= 0.1f) {
				// 计算目标旋转角度
				var targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

				// 平滑旋转
				var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 1f / rotationSpeed);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);

				// 移动角色
				var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
				characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
			}
		}

		private float turnSmoothVelocity;

		private void HandleJump() {
			if (characterController.isGrounded) {
				if (Input.GetButtonDown("Jump")) {
					velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
					isJumping = true;
				}
				else {
					isJumping = false;
				}
			}
		}

		private void ApplyGravity() {
			if (!characterController.isGrounded)
				velocity.y += gravity * Time.deltaTime;
			else if (velocity.y < 0) velocity.y = -2f; // 小的向下力，确保角色贴地

			characterController.Move(velocity * Time.deltaTime);
		}

		private void UpdateAnimations() {
			// 计算移动速度
			var horizontalVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
			var speed = horizontalVelocity.magnitude / moveSpeed;

			// 更新动画参数
			animator.SetFloat(speedParameterName, speed);
			animator.SetBool(jumpParameterName, isJumping);
		}

		// 提供公共方法用于外部控制
		public void SetMovementSpeed(float newSpeed) {
			moveSpeed = newSpeed;
		}

		public void SetJumpForce(float newJumpForce) {
			jumpForce = newJumpForce;
		}
	}
}