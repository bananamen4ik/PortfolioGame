using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCharacter;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        InitUI();
        gm.ShowCursor();
        mainCharacter.GetComponent<Animator>().SetBool("IsDancing", true);
    }

    private void InitUI()
    {
        GameObject mainMenu = GameObject.FindWithTag("UIMainMenu");
        Button[] buttons = mainMenu.GetComponentsInChildren<Button>();
        gm.ChangeCursorUIButtons(mainMenu);

        foreach (Button button in buttons)
        {
            switch (button.name)
            {
                case "StartButton":
                    button.onClick.AddListener(gm.StartGame);
                    break;

                case "SettingsButton":
                    break;

                case "CreditsButton":
                    break;

                case "ExitButton":
                    button.onClick.AddListener(gm.ExitGame);
                    break;

                default:
                    break;
            }
        }
    }
}
