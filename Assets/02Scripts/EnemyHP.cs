using System.Collections;
using UnityEngine;

public class EnemyHP : CharacterHP
{
    Coroutine activeHPBar;
    void LateUpdate()
    {
        if (hpBar != null && isHit)
        {
            hpBar.transform.forward = Camera.main.transform.forward;
        }
    }
    public override void Init()
    {
        if (hpBar != null) hpBar.gameObject.SetActive(false);
        base.Init();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        isHit = true;
        if (activeHPBar == null)
        {
            activeHPBar = StartCoroutine(ActiveHPBar());
        }
        else
        {
            StopCoroutine(activeHPBar);
            activeHPBar = StartCoroutine(ActiveHPBar());
        }
    }

    IEnumerator ActiveHPBar()
    {
        yield return new WaitForSeconds(5);
        hpBar.gameObject.SetActive(false);
        isHit = false;
        activeHPBar = null;
    }
}
