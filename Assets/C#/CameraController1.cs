using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float acceleration = 2f;
    public float minSpeed = 2f;
    public float maxSpeed = 50f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    [Range(0, 90)] public float verticalAngleLimit = 80f;

    [Header("Zoom Settings")]
    public float scrollSensitivity = 10f;

    private Vector3 _currentRotation;
    private float _currentMoveSpeed;

    void Start()
    {
        _currentRotation = transform.eulerAngles;
        _currentMoveSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
        HandleScrollInput();
    }

    private void HandleMouseInput()
    {
        // 鼠标右键旋转
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            _currentRotation.y += mouseX;
            _currentRotation.x -= mouseY;
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -verticalAngleLimit, verticalAngleLimit);

            transform.rotation = Quaternion.Euler(_currentRotation);
        }
    }

    private void HandleKeyboardInput()
    {
        Vector3 moveDirection = Vector3.zero;

        // 基础移动方向（WASD）
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;

        // 升降控制（Q/E）
        if (Input.GetKey(KeyCode.E)) moveDirection += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) moveDirection += Vector3.down;

        // 应用移动
        if (moveDirection != Vector3.zero)
        {
            _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, moveSpeed, acceleration * Time.deltaTime);
            transform.position += moveDirection.normalized * _currentMoveSpeed * Time.deltaTime;
        }
    }

    private void HandleScrollInput()
    {
        // 滚轮调整速度
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            moveSpeed += scroll * scrollSensitivity;
            moveSpeed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);
        }
    }
}