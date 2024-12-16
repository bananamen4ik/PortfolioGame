using UnityEngine;

public class Mission_1_NPC_1 : MonoBehaviour
{
    [SerializeField]
    private Mission_1_Manager mm;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        mm.ShowQuest_1_Dialog();
    }
}
