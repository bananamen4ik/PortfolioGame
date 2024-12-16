using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    private CharacterController characterController;
    private Animator characterAnimator;
    private Vector2 moving = new();
    private readonly float speedMove = 2;
    private GameObject lastActiveVirtualCamera;

    public void FreezeCamera()
    {
        CinemachineBrain mainCameraBrain =
            mainCamera.GetComponent<CinemachineBrain>();

        lastActiveVirtualCamera =
            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject;

        lastActiveVirtualCamera.GetComponent<CinemachineInputProvider>()
            .enabled = false;
    }

    public void UnFreezeCamera()
    {
        lastActiveVirtualCamera.GetComponent<CinemachineInputProvider>()
            .enabled = true;
    }

    public void FreezeMove()
    {
        GetComponent<PlayerInput>().enabled = false;
    }

    public void UnFreezeMove()
    {
        GetComponent<PlayerInput>().enabled = true;
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
