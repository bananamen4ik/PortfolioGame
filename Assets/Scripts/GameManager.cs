using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerData playerData;

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

    private PlayerController playerController;

    private bool isQuestDialogOpened = false;

    public void ChangeCamera(
        GameObject cameraFrom, GameObject cameraTo, float timeoutBack = 0)
    {
        cameraTo.SetActive(true);
        cameraFrom.SetActive(false);

        if (cameraFrom.CompareTag("CharacterCamera"))
        {
            playerController.FreezeMove();
            playerController.FreezeCamera();
        }
        else if(cameraTo.CompareTag("CharacterCamera"))
        {
            playerController.UnFreezeMove();
            playerController.UnFreezeCamera();
        }

        if (timeoutBack != 0)
        {
            StartCoroutine(
                ChangeCameraBackTimeout(cameraTo, cameraFrom, timeoutBack));
        }
    }

    public void ShowQuestDialog(GameObject questDialog)
    {
        isQuestDialogOpened = true;
        questDialog.SetActive(true);
        PauseGame();
    }

    public void HideQuestDialog(GameObject questDialog)
    {
        isQuestDialogOpened = false;
        questDialog.SetActive(false);
        ContinueGame();
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public PlayerController GetPlayerController()
    {
        return GetPlayer().GetComponent<PlayerController>();
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

    private IEnumerator ChangeCameraBackTimeout(
    GameObject cameraFrom, GameObject cameraTo, float timeout)
    {
        yield return new WaitForSeconds(timeout);

        ChangeCamera(cameraFrom, cameraTo);
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
        InitPlayerData();
    }

    private void InitPlayerData()
    {
        playerData = new(0, 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        playerController = GetPlayerController();
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
        if (isQuestDialogOpened) return;
        if ((playerController.isFreezeMove || playerController.isFreezeCamera)
            && !isPaused) return;

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

public class PlayerData
{
    public int mission;
    public int quest;

    public PlayerData(int mission, int quest)
    {
        this.mission = mission;
        this.quest = quest;
    }
}