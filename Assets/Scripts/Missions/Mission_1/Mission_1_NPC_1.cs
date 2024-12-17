using UnityEngine;

public class Mission_1_NPC_1 : MonoBehaviour
{
    [SerializeField]
    private Mission_1_Manager mm;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || gm.playerData.quest == 1)
        {
            return;
        }

        mm.ShowQuest_1_Dialog();
    }
}
