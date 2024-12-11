using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 moving = new();
    private float speedMove = 1;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = new(moving.x, 0, moving.y);

        if (move == Vector3.zero) return;

        characterController.Move(speedMove * Time.deltaTime * move);
        transform.forward = move;
    }

    public void OnMove(InputValue value)
    {
        moving = value.Get<Vector2>();
    }
}
