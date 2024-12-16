using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCharacter;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        gm.ShowCursor();
        mainCharacter.GetComponent<Animator>().SetBool("IsDancing", true);
    }
}
