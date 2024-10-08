using UnityEngine;

public class TutorialArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("A thing just Hit Me!!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("A Player just Hit Me!!");
        }
    }
}
