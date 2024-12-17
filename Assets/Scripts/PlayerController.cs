using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public GameObject characterCamera;
    public bool isFreezeMove = false;
    public bool isFreezeCamera = false;

    [SerializeField]
    private GameObject mainCamera;

    private CharacterController characterController;
    private Animator characterAnimator;
    private Vector2 moving = new();

    private readonly float speedMove = 2;

    public void TakeAnimaion()
    {
        characterAnimator.SetTrigger("PickUp");
    }

    public void FreezeCamera()
    {
        characterCamera.GetComponent<CinemachineInputProvider>()
            .enabled = false;
        isFreezeCamera = true;
    }

    public void UnFreezeCamera()
    {
        characterCamera.GetComponent<CinemachineInputProvider>()
            .enabled = true;
        isFreezeCamera = false;
    }

    public void FreezeMove(float timeoutUnFreeze = 0)
    {
        GetComponent<PlayerInput>().enabled = false;
        isFreezeMove = true;

        if (timeoutUnFreeze != 0)
        {
            Invoke(nameof(UnFreezeMove), timeoutUnFreeze);
        }
    }

    public void UnFreezeMove()
    {
        GetComponent<PlayerInput>().enabled = true;
        isFreezeMove = false;
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

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
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
}
