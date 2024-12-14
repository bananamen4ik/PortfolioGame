using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIMenu;

    [SerializeField]
    private GameObject mainCharacter;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        gm.ChangeCursorUIButtons(UIMenu);
        mainCharacter.GetComponent<Animator>().SetBool("IsDancing", true);
    }
}
