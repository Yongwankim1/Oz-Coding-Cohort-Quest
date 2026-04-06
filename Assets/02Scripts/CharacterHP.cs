using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHP : MonoBehaviour, IDamageable
{
    [SerializeField] protected int currentHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int defaultMaxHP;
    [SerializeField] protected int equipHP;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected Slider hpBar;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isDebug;
    public int MaxHP => maxHP;
    public bool IsDead => currentHP <= 0;
    void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        isHit = false;
        maxHP = defaultMaxHP + equipHP;
        currentHP = maxHP;
        m_animator = GetComponent<Animator>();
        HPBarUpdate();
    }
    protected void changeHP(int amount)
    {
        equipHP = amount;
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
        currentHP = Mathf.Clamp(currentHP, 0, maxHP + equipHP);
        if (IsDead)
        {
            Die();
        }
        if(isDebug)
            Debug.Log($"{gameObject.name}이 데미지 받음{damage} || 남은체력 {currentHP}");
    }


    void HPBarUpdate()
    {
        if (hpBar != null)
        {
            hpBar.value = (float) currentHP / (maxHP + equipHP);
        }
    }
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (IsDead) return;
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP + equipHP);
        HPBarUpdate();
        if (isDebug)
            Debug.Log($"{gameObject.name}이 {amount}만큼 회복 | 현재 체력 {currentHP}");
    }

    void Die()
    {
        Destroy(gameObject,3f);
    }
}
