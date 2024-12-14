using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorTexture;

    [SerializeField]
    private Texture2D handCursorTexture;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetDefaultCursor();
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
