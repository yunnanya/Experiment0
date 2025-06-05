using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float walkSpeed = 1.6f;
    [SerializeField] private float runSpeed = 2.1f;
    [SerializeField][Range(0, 0.9f)] private float deadZone = 0.2f;
    [SerializeField][Range(0.5f, 1f)] private float runThreshold = 0.95f;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null) return;

        Vector2 input = gamepad.leftStick.ReadValue();

        // 应用死区处理
        if (input.magnitude < deadZone)
        {
            input = Vector2.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - deadZone) / (1 - deadZone));
        }

        // 仅当摇杆向前推（Y轴正方向）时移动
        if (input.y <= 0) return;

        // 根据Y轴输入值判断是否奔跑
        float currentSpeed = input.y >= runThreshold ? runSpeed : walkSpeed;

        // 获取相机正前方向量（水平方向）
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        // 仅使用相机前方方向移动
        Vector3 movement = forward * (input.y * currentSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + movement);
    }
}