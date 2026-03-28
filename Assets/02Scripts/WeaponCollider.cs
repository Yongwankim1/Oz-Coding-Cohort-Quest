using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [SerializeField] Collider weaponCollider;
    [SerializeField] HashSet<IDamageable> targetsName = new HashSet<IDamageable>();
    [SerializeField] int damage = 0;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] LayerMask targetLayer;
    private void Awake()
    {
        Init();
    }
    void Init()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }
    public void StartAttack()
    {
        weaponCollider.enabled = true;
    }
    public void EndAttack()
    {
        weaponCollider.enabled = false;
        targetsName.Clear();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        IDamageable damageable = null;
        other.TryGetComponent<IDamageable>(out damageable);
        if (damageable == null || targetsName.Contains(damageable)) return;
        targetsName.Add(damageable);
        damage = (int)playerAttack.Dmaage;
        damageable.TakeDamage(damage);
    }
}
