using UnityEngine;

public class Collectables : MonoBehaviour
{

    public static int RemainingCount;

    private void Awake()
    {
        RemainingCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        RemainingCount--;
        Destroy(gameObject);
        Debug.Log($"Collectable collected. {RemainingCount} still to go.");
    }

}
