using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] protected float maxHp = 1000;
    [SerializeField] protected bool isDeath;

    public virtual void TakeDamage(float damage)
    {
        if (isDeath)
            return;

        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;
        if (maxHp < 0)
            Die();
    }

    private void Die()
    {
        isDeath = true;
        Debug.Log("Entity Die");
    }
}
