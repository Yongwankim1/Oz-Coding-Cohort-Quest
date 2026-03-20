using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
}
}
