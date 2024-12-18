using UnityEngine;

public class Mission_1_Fireflies : MonoBehaviour
{
    private readonly float speed = 90;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime * Vector3.up);
    }
}
