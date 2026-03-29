using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHP : MonoBehaviour, IDamageable
{
    [SerializeField] protected int currentHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected Slider hpBar;
    [SerializeField] protected bool isHit;

    public int MaxHP => maxHP;
    public bool IsDead => currentHP <= 0;
    void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        isHit = false;
        currentHP = maxHP;
        m_animator = GetComponent<Animator>();
        HPBarUpdate();
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (IsDead) return;
        hpBar.gameObject.SetActive(true);
        currentHP -= damage;
        HPBarUpdate();
        m_animator.SetTrigger("Hit");
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (IsDead)
        {
            Die();
        }
        Debug.Log($"{gameObject.name}이 데미지 받음{damage} || 남은체력 {currentHP}");
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
