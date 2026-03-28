using System.Collections;
using UnityEngine;

public class HealingItem : MonoBehaviour, IInteractable
{
    enum HealType
    {
        None,
        Heal,
        HealBuff    // 지속형
    }
    [SerializeField] private HealType type;
    [SerializeField] int value;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] GameObject myMesh;
    [SerializeField] SphereCollider buffZone;
    [SerializeField] float deactiveRadius = 0.98f;
    [SerializeField] float activeRadius = 4.35f;
    [SerializeField] Coroutine healCoroutine;
    [SerializeField] bool isInteract;
    [SerializeField] ParticleSystem effect;
    void Start()
    {
        buffZone = GetComponent<SphereCollider>();
    }
    public void Interact(PlayerInteract player)
    {
        HealBuff(player);
    }
    void Heal()
    {

    }
    [ContextMenu("HealBuff")]
    void HealBuff(PlayerInteract player)
    {
        buffZone.radius = activeRadius;
        RaycastHit hit;
        Vector3 position;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,5))
        {
            position = hit.point;
        }
        else
        {
            position=transform.position;
        }
        if(effect == null)
        {
            GameObject go = Instantiate(effectPrefab, position, Quaternion.identity, transform);
            effect = go.GetComponent<ParticleSystem>();
            healCoroutine = StartCoroutine(HealStart(player));
        }
        else
        {
            Debug.Log("이미 힐링 중");
        }
    }
    IEnumerator HealStart(PlayerInteract player)
    {
        CharacterHP characterHP = player.GetComponent<CharacterHP>();
        if (characterHP == null) yield break;
        float amount = characterHP.MaxHP * value / 100;

        while (true)
        {
            characterHP.Heal((int)amount);
            yield return new WaitForSeconds(3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
            buffZone.radius = deactiveRadius;
            effect.Stop();
            effect = null;
        }
    }
}
