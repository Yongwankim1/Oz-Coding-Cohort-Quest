using UnityEngine;

public class PlayerHP : CharacterHP
{
    [ContextMenu("TestTakeDamage")]
    public void TestDamage()
    {
        float damage = (float)currentHP / 2;
        TakeDamage((int)damage);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
