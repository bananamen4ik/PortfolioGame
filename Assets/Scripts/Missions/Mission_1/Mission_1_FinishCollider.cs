using UnityEngine;

public class Mission_1_FinishCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("FINISH! :)");
    }
}
