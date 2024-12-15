using UnityEngine;

public class Mission_1_Manager : MonoBehaviour
{
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        gm.HideCursor();
    }
}
