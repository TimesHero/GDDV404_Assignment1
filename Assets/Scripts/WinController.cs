using UnityEngine;

public class WinController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private NpcPatrol npcPatrol;
    private bool hasWon;

    private void Start()
    {
        Time.timeScale = 1f;
        winPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (hasWon)
        {
            return;
        }

        if (Collectables.RemainingCount <= 0 && npcPatrol != null && !npcPatrol.HasCaughtPlayer)

        {
            hasWon = true;
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
