using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InputAction interactAction;
    [SerializeField] IInteractable curInteractable;

    private void Awake()
    {
        interactAction.Enable();
    }
    private void Update()
    {
        if(interactAction.WasPerformedThisFrame())
        {
            Interact();
        }
    }
    void Interact()
    {
        if(curInteractable == null)
        {
            Debug.Log("상호작용 대상 없음");
            return;
        }
        curInteractable.Interact(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        curInteractable = other.GetComponent<IInteractable>();
    }
    private void OnTriggerExit(Collider other)
    {
        curInteractable = null;
    }
}
