using UnityEngine;
using UnityEngine.UI;

public class Mission_1_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIQuest_1_dialog;

    private GameManager gm;

    public void ShowQuest_1_Dialog()
    {
        gm.ShowQuestDialog(UIQuest_1_dialog);
    }

    public void HideQuest_1_Dialog()
    {
        gm.HideQuestDialog(UIQuest_1_dialog);
    }

    private void Start()
    {
        gm = GameManager.instance;

        InitUI();
        gm.HideCursor();
    }

    private void InitUI()
    {
        InitUIQuest_1_dialog();
    }

    private void InitUIQuest_1_dialog()
    {
        Button[] buttons = UIQuest_1_dialog.GetComponentsInChildren<Button>();
        gm.ChangeCursorUIButtons(UIQuest_1_dialog);

        foreach (Button button in buttons)
        {
            switch (button.name)
            {
                case "AcceptButton":
                    button.onClick.AddListener(StartQuest_1);
                    break;

                case "CancelButton":
                    button.onClick.AddListener(HideQuest_1_Dialog);
                    break;

                default:
                    break;
            }
        }
    }

    private void StartQuest_1()
    {
        HideQuest_1_Dialog();
    }
}
