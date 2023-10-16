using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public event Action<SoundType> HitSoundPlay;
    public event Action<SoundType> BlockSoundPlay;
    public event Action<SoundType> AttackSoundPlay;
    public event Action<SoundType> WinSoundPlay;

    protected float _punchRadius = 0.07f;
    protected float _kickRadius = 0.5f;
    protected float _ultimateRadius = 1f;
    protected float _maxDistance = 0.5f;
    protected float _damage = 5f;

    protected bool _die = false;
    public bool Able { get; set; } = false;

    protected Vector3 _handLVector;
    protected Vector3 _handRVector;
    protected Vector3 _footLVector;
    protected Vector3 _footRVector;
    protected Vector3 _boxSize = new Vector3(0.1f, 0.1f, 0.1f);
    protected Vector3 _ultimateVector;

    protected virtual void Start()
    {
        HitSoundPlay += SoundManager.Instance.PlaySound;
        BlockSoundPlay += SoundManager.Instance.PlaySound;
        AttackSoundPlay += SoundManager.Instance.PlaySound;
        WinSoundPlay += SoundManager.Instance.PlaySound; 
    }

    protected virtual void OnDestroy()
    {
        HitSoundPlay -= SoundManager.Instance.PlaySound;
        BlockSoundPlay -= SoundManager.Instance.PlaySound;
        AttackSoundPlay -= SoundManager.Instance.PlaySound;
        WinSoundPlay -= SoundManager.Instance.PlaySound;
    }

    protected virtual void Update()
    {
        
    }

    protected void HitSound()
    {
        HitSoundPlay?.Invoke(SoundType.Hit);
    }
    protected void BlockSound()
    {
        BlockSoundPlay.Invoke(SoundType.Block);
    }
    protected void AttackSound()
    {
        AttackSoundPlay?.Invoke(SoundType.Attack);
    }
    protected void WinSound()
    {
        WinSoundPlay?.Invoke(SoundType.Win);
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
