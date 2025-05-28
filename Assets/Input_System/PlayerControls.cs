using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCotrols : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    private Vector2 moveInputValue;

    public void OnMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
        Debug.Log(moveInputValue);
    }

    public void MoveLogicMethod()
    {
        Vector2 result = moveInputValue * speed * Time.fixedDeltaTime;
        rb.velocity = result;
    }

    private void FixedUpdate()
    {
        MoveLogicMethod();
    }
}
