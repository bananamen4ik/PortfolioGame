using UnityEngine;
using UnityEngine.UI;

public class Mission_1_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIQuest_1_Dialog;

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private GameObject door_1_Camera;

    [SerializeField]
    private GameObject door_3_Camera;

    [SerializeField]
    private GameObject door_1;

    [SerializeField]
    private GameObject door_2;

    [SerializeField]
    private GameObject door_3;

    [SerializeField]
    private GameObject gemPrefab;

    private GameManager gm;
    private PlayerController playerController;

    private readonly int needGems = 6;
    private int gems = 0;

    public void PickUpGem()
    {
        gems++;
        SpawnGem();

        if (gems == 1)
        {
            CloseDoor_1();
            CloseDoor_2();
        }

        if (gems != needGems)
        {
            playerController.FreezeMove(6);
        }
        else
        {
            gm.ChangeCamera(playerController.characterCamera, door_3_Camera, 5);
            OpenDoor_3();
        }

        playerController.TakeAnimaion();
    }

    public void SpawnGem()
    {
        Vector3 pos = new(0, 1.7f, 0);
        
        switch (gems)
        {
            case 0:
                pos.x = -3.221566f;
                pos.z = -26.799f;
                break;

            case 1:
                pos.x = 7.37799978f;
                pos.z = -20.1989994f;
                break;

            case 2:
                pos.x = 9.95199966f;
                pos.z = -10.0369997f;
                break;

            case 3:
                pos.x = 6.93900013f;
                pos.z = -11.8509998f;
                break;

            case 4:
                pos.x = 7.54400015f;
                pos.z = -27.7070007f;
                break;

            case 5:
                pos.x = 13.9809999f;
                pos.z = -23.4969997f;
                break;
        }

        GameObject gem = Instantiate(gemPrefab, pos, Quaternion.identity);
        gem.AddComponent<Mission_1_GemItem>();
    }

    public void ShowQuest_1_Dialog()
    {
        gm.ShowQuestDialog(UIQuest_1_Dialog);
    }

    public void HideQuest_1_Dialog()
    {
        gm.HideQuestDialog(UIQuest_1_Dialog);
    }

    private void Start()
    {
        gm = GameManager.instance;
        playerController = gm.GetPlayerController();

        gm.playerData.mission = 1;
        gm.playerData.quest = 0;

        InitUI();
        gm.HideCursor();
    }

    private void InitUI()
    {
        InitUIQuest_1_dialog();
    }

    private void InitUIQuest_1_dialog()
    {
        Button[] buttons = UIQuest_1_Dialog.GetComponentsInChildren<Button>();
        gm.ChangeCursorUIButtons(UIQuest_1_Dialog);

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
            }
        }
    }

    private void StartQuest_1()
    {
        HideQuest_1_Dialog();

        GameObject characterCamera = playerController.characterCamera;

        gm.ChangeCamera(characterCamera, door_1_Camera, 5);
        gm.playerData.quest = 1;

        OpenDoor_1();
        Invoke(nameof(OpenDoor_2), 1);

        SpawnGem();
    }

    private void OpenDoor_1()
    {
        door_1.GetComponent<Animator>().SetBool("IsOpen", true);
    }

    private void CloseDoor_1()
    {
        door_1.GetComponent<Animator>().SetBool("IsOpen", false);
    }

    private void OpenDoor_2()
    {
        door_2.GetComponent<Animator>().SetBool("IsOpen", true);
    }

    private void CloseDoor_2()
    {
        door_2.GetComponent<Animator>().SetBool("IsOpen", false);
    }

    private void OpenDoor_3()
    {
        door_3.GetComponent<Animator>().SetBool("IsOpen", true);
        door_3.GetComponent<BoxCollider>().enabled = false;
    }
}