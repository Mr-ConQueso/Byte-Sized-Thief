using UnityEngine;

public class TutorialArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("A Player just Hit Me!!");
        }
    }
}
