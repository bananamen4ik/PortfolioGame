using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    private CharacterController characterController;
    private Animator characterAnimator;
    private Vector2 moving = new();
    private readonly float speedMove = 2;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = new(moving.x, 0, moving.y);
        if (move == Vector3.zero) return;

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 direction = move.x * cameraRight + move.z * cameraForward;

        transform.forward = direction;
        characterController.Move(speedMove * Time.deltaTime * direction);
    }

    public void OnMove(InputValue value)
    {
        moving = value.Get<Vector2>();

        if (moving != Vector2.zero)
        {
            characterAnimator.SetBool("IsWalking", true);
        }
        else
        {
            characterAnimator.SetBool("IsWalking", false);
        }
    }
}
