using UnityEngine;

public class MissionManager_1 : MonoBehaviour
{
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        gm.HideCursor();
    }
}
