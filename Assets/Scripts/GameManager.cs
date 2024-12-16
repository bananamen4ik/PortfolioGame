using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused = false;

    [SerializeField]
    private Texture2D cursorTexture;

    [SerializeField]
    private Texture2D handCursorTexture;

    [SerializeField]
    private InputActionAsset inputActions;

    [SerializeField]
    private GameObject eventSystem;

    [SerializeField]
    private GameObject UIPauseMenu;

    private GameObject lastActiveVirtualCamera;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        eventSystem.SetActive(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetDefaultCursor();
        InitInputSystem();
        ChangeCursorUIButtons(UIPauseMenu);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.buildIndex == 0)
        {
            GameObject mainMenu = GameObject.FindWithTag("UIMainMenu");
            Button[] buttons = mainMenu.GetComponentsInChildren<Button>();
            ChangeCursorUIButtons(mainMenu);

            foreach (Button button in buttons)
            {
                switch (button.name)
                {
                    case "StartButton":
                        button.onClick.AddListener(StartGame);
                        break;

                    case "SettingsButton":
                        break;

                    case "CreditsButton":
                        break;

                    case "ExitButton":
                        button.onClick.AddListener(ExitGame);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void InitInputSystem()
    {
        InputAction ESCAction = inputActions.FindActionMap("UI").FindAction("ESC");
        ESCAction.Enable();
        ESCAction.performed += (InputAction.CallbackContext ctx) => Pause();
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        UIPauseMenu.SetActive(!UIPauseMenu.activeSelf);
        isPaused = !isPaused;

        CinemachineBrain mainCameraBrain =
            GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();

        if (isPaused)
        {
            lastActiveVirtualCamera =
                mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject;

            lastActiveVirtualCamera.SetActive(false);

            ShowCursor();
        }
        else
        {
            lastActiveVirtualCamera.SetActive(true);

            HideCursor();
        }
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(20, 14), CursorMode.Auto);
    }

    public void SetHandCursor()
    {
        Cursor.SetCursor(handCursorTexture, new Vector2(20, 14), CursorMode.Auto);
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        SetDefaultCursor();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeCursorUIButtons(GameObject UIWrapper)
    {
        Button[] buttons = UIWrapper.GetComponentsInChildren<Button>();

        EventTrigger.Entry cursorEnter = new()
        {
            eventID = EventTriggerType.PointerEnter
        };

        EventTrigger.Entry cursorExit = new()
        {
            eventID = EventTriggerType.PointerExit
        };

        cursorEnter.callback.AddListener(
            (eventData) => SetHandCursor()
        );

        cursorExit.callback.AddListener(
            (eventData) => SetDefaultCursor()
        );

        foreach (Button button in buttons)
        {
            EventTrigger eventTrigger =
                button.gameObject.AddComponent<EventTrigger>();

            eventTrigger.triggers.Add(cursorEnter);
            eventTrigger.triggers.Add(cursorExit);
        }
    }

    public void MainMenu(bool fromPauseMenu = false)
    {
        if (fromPauseMenu)
        {
            Pause();
        }

        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
