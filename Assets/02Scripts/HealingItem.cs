using UnityEngine;

public class HealingItem : MonoBehaviour, IInteractable
{
    enum HealType
    {
        None,
        Heal,
        HealBuff    // Áö¼ÓÇü
    }
    [SerializeField] private HealType type;
    [SerializeField] int value;
    [SerializeField] GameObject effect;
    [SerializeField] GameObject myMesh;

    public void Interact()
    {

    }
}
