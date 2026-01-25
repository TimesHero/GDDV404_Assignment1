using UnityEngine;
using UnityEngine.InputSystem;

public class TouchToMove : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public UnityEngine.AI.NavMeshAgent navAgent;
    private Vector3 spawnPosition;

    private void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        spawnPosition = transform.position;
    }
    public void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttack;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Vector2 screenPos = inputActions.Player.Point.ReadValue<Vector2>();

        Ray mouseRay = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
        {
            navAgent.SetDestination(hitInfo.point);
        }
    }

    public void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Attack.performed -= OnAttack;
    }

    public void Respawn()
    {
        if (navAgent == null)
        {
            return;
        }

        navAgent.Warp(spawnPosition);
        navAgent.ResetPath();
    }
    
}
