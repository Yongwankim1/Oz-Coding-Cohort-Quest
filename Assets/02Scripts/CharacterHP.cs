using UnityEngine;

public class CharacterHP : MonoBehaviour, IDamageable
{
    [SerializeField] int currentHP;
    [SerializeField] int maxHP;
    [SerializeField] Animator m_animator;
    public bool IsDead => currentHP <= 0;
    void Awake()
    {
        Init();
    }
    void Init()
    {
        currentHP = this.maxHP;
        m_animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if(IsDead) return;
        currentHP -= damage;
        Debug.Log($"{gameObject.name}이 데미지 받음{damage} || 남은체력 {currentHP}");
        m_animator.SetTrigger("Hit");
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if(currentHP == 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (IsDead) return;
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
    }

    void Die()
    {

    }
}
