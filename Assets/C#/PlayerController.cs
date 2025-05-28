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
    private Vector2 lastInput;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null) return;

        Vector2 input = gamepad.leftStick.ReadValue();

        if (input.magnitude < deadZone)
        {
            input = Vector2.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - deadZone) / (1 - deadZone));
        }

        if (input == Vector2.zero) return;

        float currentSpeed = input.magnitude >= runThreshold ? runSpeed : walkSpeed;

        // ������ȥ���ҷ���Ĵ�ֱ����
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0; // ������ȥ��Y�����
        right.Normalize(); // ��������һ��

        Vector3 direction = forward * input.y + right * input.x;
        Vector3 movement = direction * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}