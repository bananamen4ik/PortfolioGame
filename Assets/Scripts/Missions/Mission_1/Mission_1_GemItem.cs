using UnityEngine;

public class Mission_1_GemItem : MonoBehaviour
{
    private Mission_1_Manager mm;

    private readonly float speed = 30;

    private bool isPickUp = false;

    private void Start()
    {
        mm = GameObject.FindWithTag("MissionManager")
            .GetComponent<Mission_1_Manager>();
    }

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isPickUp) return;

        isPickUp = true;
        mm.PickUpGem();

        Invoke(nameof(DestroyInvoke), 3);
    }

    private void DestroyInvoke()
    {
        Destroy(gameObject);
    }
}
