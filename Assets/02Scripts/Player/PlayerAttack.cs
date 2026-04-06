using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator m_animator;
    [SerializeField] InputAction mouseAttack;
    [SerializeField] int AttackCount = 0;

    [SerializeField] float defaultDamage = 0;
    [SerializeField] float currentDamage = 0;
    [SerializeField] float equipDamage = 0;
    public float Dmaage => currentDamage;
    [SerializeField] WeaponCollider weapon;
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        currentDamage = defaultDamage;
        m_animator = GetComponent<Animator>();
        mouseAttack.Enable();
    }
    public void EquipWeapon(float amount)
    {
        equipDamage = amount;
    }

    private void Update()
    {
        if (mouseAttack.WasPerformedThisFrame())
        {
            AttackRotation();
            DefalutAttack();
        }
    }
    void AttackRotation()
    {
        Transform carmeraTransform = Camera.main.transform;
        Vector3 forward = carmeraTransform.forward;
        forward.y = 0f;

        forward.Normalize();
        Vector3 moveDir = forward;
        Quaternion rot = Quaternion.LookRotation(moveDir);

        transform.rotation = rot;
    }
    void DefalutAttack()
    {
        m_animator.SetTrigger("Attack");
        m_animator.SetInteger("AttackCount",AttackCount);
    }
    public void LastAttack()
    {
        currentDamage = (defaultDamage + equipDamage) * 1.3f;
    }
    public void StatAttack()
    {
        weapon.StartAttack();
    }
    public void EndAttack()
    {
        weapon.EndAttack();
        currentDamage = (defaultDamage + equipDamage);
    }

}
