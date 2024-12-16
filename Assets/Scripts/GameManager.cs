using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public bool isPaused = false;
    public bool isOpenedQuestDialog = false;

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

    public void ShowQuestDialog(GameObject questDialog)
    {
        questDialog.SetActive(true);
        isOpenedQuestDialog = true;
        PauseGame();
    }

    public void HideQuestDialog(GameObject questDialog)
    {
        questDialog.SetActive(false);
        isOpenedQuestDialog = false;
        ContinueGame();
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public void PauseGame(bool withUIPauseMenu = false)
    {
        playerController.FreezeCamera();
        playerController.FreezeMove();

        ShowCursor();
        isPaused = true;

        if (withUIPauseMenu)
        {
            ShowUIPauseMenu();
        }
    }

    public void ContinueGame(bool withUIPauseMenu = false)
    {
        playerController.UnFreezeCamera();
        playerController.UnFreezeMove();

        HideCursor();
        isPaused = false;

        if (withUIPauseMenu)
        {
            HideUIPauseMenu();
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
            ContinueGame(true);
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

    public void ShowUIPauseMenu()
    {
        UIPauseMenu.SetActive(true);
    }

    public void HideUIPauseMenu()
    {
        UIPauseMenu.SetActive(false);
    }

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
        playerController = GetPlayer().GetComponent<PlayerController>();
    }

    private void InitInputSystem()
    {
        InputAction ESCAction = inputActions.FindActionMap("UI").FindAction("ESC");
        ESCAction.Enable();
        ESCAction.performed += (InputAction.CallbackContext ctx) => OnESCKeyPressed();
    }

    private void OnESCKeyPressed()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        if (isOpenedQuestDialog) return;

        if (!isPaused)
        {
            PauseGame(true);
        }
        else
        {
            ContinueGame(true);
        }
    }
}
