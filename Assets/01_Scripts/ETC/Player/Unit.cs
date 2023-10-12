using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected float _punchRadius = 0.07f;
    protected float _kickRadius = 0.5f;
    protected float _ultimateRadius = 1f;
    protected float _maxDistance = 0.5f;
    protected float _damage = 5f;

    protected bool _die = false;

    protected Vector3 _handLVector;
    protected Vector3 _handRVector;
    protected Vector3 _footLVector;
    protected Vector3 _footRVector;
    protected Vector3 _ultimateVector;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    protected abstract void Move();

    protected abstract void Attack();

    protected abstract void Charging();

    protected abstract void Ultimate();

    protected abstract void Block(bool low, bool ultimate);

    protected abstract void Hit(bool low, bool ultimate);

    protected abstract void Die();

    protected abstract void Win();

    protected abstract void OnHitEnemy(RaycastHit hit, bool low, bool ultimate);
}
