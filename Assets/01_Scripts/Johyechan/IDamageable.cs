using UnityEngine;

public interface IDamageable 
{
    void TakeDamage(float damage, RaycastHit hit, bool low, bool middle);
}
