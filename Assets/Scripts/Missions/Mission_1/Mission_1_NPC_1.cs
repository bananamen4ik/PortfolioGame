using UnityEngine;

public class Mission_1_NPC_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;


    }
}
