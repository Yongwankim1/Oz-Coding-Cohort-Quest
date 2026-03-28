using UnityEngine;
using UnityEngine.UI;

public class CharacterHP : MonoBehaviour, IDamageable
{
    [SerializeField] int currentHP;
    [SerializeField] int maxHP;
    [SerializeField] Animator m_animator;
    [SerializeField] Slider hpBar;
    public int MaxHP => maxHP;
    public bool IsDead => currentHP <= 0;
    void Awake()
    {
        Init();
    }
    void Init()
    {
        currentHP = maxHP;
        m_animator = GetComponent<Animator>();
        HPBarUpdate();
    }
    void LateUpdate()
    {
        if (hpBar != null)
        {
            hpBar.transform.forward = Camera.main.transform.forward;
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (IsDead) return;
        currentHP -= damage;
        HPBarUpdate();
        Debug.Log($"{gameObject.name}이 데미지 받음{damage} || 남은체력 {currentHP}");
        m_animator.SetTrigger("Hit");
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (currentHP == 0)
        {
            Die();
        }
    }

    void HPBarUpdate()
    {
        if (hpBar != null)
        {
            hpBar.value = (float) currentHP / maxHP;
        }
    }
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (IsDead) return;
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
        HPBarUpdate();
    }

    void Die()
    {
        Destroy(gameObject,3f);
    }
}
