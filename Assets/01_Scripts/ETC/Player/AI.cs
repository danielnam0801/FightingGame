using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Unit, IDamageable
{
    public LayerMask Player;

    private Animator _animator;

    [SerializeField]
    private Transform _handL;
    [SerializeField]
    private Transform _handR;
    [SerializeField]
    private Transform _footL;
    [SerializeField]
    private Transform _footR;

    private bool _attack = false;
    private bool _low = false;
    private bool _middle = false;
    private bool _win = false;

    private float _currentTime = 0;
    [SerializeField]
    private float _coolTime = 1f;
    [SerializeField]
    private float _aiHealth = 3;

    private RaycastHit hit;

    private readonly int _hashMoveForward = Animator.StringToHash("MoveForward");
    private readonly int _hashMoveBackward = Animator.StringToHash("MoveBackward");      // 초급 ai에서는 체력이 일정 이하로 내려가면 일정확률로 뒤로 갈 예정 아마도
    private readonly int _hashLPunch = Animator.StringToHash("LPunch");                  // 상단 왼쪽 주먹 공격
    private readonly int _hashRPunch = Animator.StringToHash("RPunch");                  // 상단 오른쪽 주먹 공격
    private readonly int _hashLKick = Animator.StringToHash("LKick");                    // 상단 왼쪽 발차기 공격
    private readonly int _hashRKick = Animator.StringToHash("RKick");                    // 상단 오른쪽 발차기 공격
    private readonly int _hashLMiddlePunch = Animator.StringToHash("LMiddlePunch");      // 중단 왼쪽 주먹 공격
    private readonly int _hashRMiddlePunch = Animator.StringToHash("RMiddlePunch");      // 중단 오른쪽 주먹 공격
    private readonly int _hashLMiddleKick = Animator.StringToHash("LMiddleKick");        // 중단 왼쪽 발차기 공격
    private readonly int _hashRMiddleKick = Animator.StringToHash("RMiddleKick");        // 중단 오른쪽 발차기 공격
    private readonly int _hashLLowKick = Animator.StringToHash("LLowKick");              // 하단 왼쪽 발차기 공격
    private readonly int _hashRLowKick = Animator.StringToHash("RLowKick");              // 하단 오른쪽 발차기 공격
    private readonly int _hashHit = Animator.StringToHash("Hit");                        // 상단을 맞음
    private readonly int _hashMiddleHit = Animator.StringToHash("MiddleHit");            // 중단을 맞음
    private readonly int _hashLowHit = Animator.StringToHash("LowHit");                  // 하단을 맞음
    private readonly int _hashBlock = Animator.StringToHash("Block");                    // 상단을 막음
    private readonly int _hashMiddleBlock = Animator.StringToHash("MiddleBlock");        // 중단을 막음
    private readonly int _hashLowBlock = Animator.StringToHash("LowBlock");              // 하단을 막음
    private readonly int _hashKnockDown = Animator.StringToHash("KnockDown");            // 쓰러짐
    private readonly int _hashGetUp = Animator.StringToHash("GetUp");                    // 일어남
    private readonly int _hashDie = Animator.StringToHash("Die");                        // 쓰러진 상태로 지속
    private readonly int _hashWin = Animator.StringToHash("Win");                        // 승리 모션

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        _handLVector = new Vector3(_handL.position.x, _handL.position.y + 0.05f, _handL.position.z - 0.03f);
        _handRVector = new Vector3(_handR.position.x, _handR.position.y + 0.05f, _handR.position.z - 0.03f);
        _footLVector = new Vector3(_footL.position.x, _footL.position.y - 0.05f, _footL.position.z - 0.07f);
        _footRVector = new Vector3(_footR.position.x, _footR.position.y - 0.05f, _footR.position.z - 0.07f);
        if (!_die || !_win)
        {
            RayCheck(); 
            Move();     
            Attack();
            Win();
        }
    }

    private void RayCheck()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, 1, transform.position.z), Vector3.back);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            _attack = true;
        }
        else
        {
            _attack = false;
        }
        Debug.DrawRay(new Vector3(transform.position.x, 1, transform.position.z), Vector3.back, Color.cyan, 1f);
    }

    protected override void Move()
    {
        int rand;
        if (_aiHealth <= 1)
        {
            rand = Random.Range(1, 4);
            if(rand > 2)
            {
                if(!_attack)
                {
                    _animator.SetBool(_hashMoveForward, false);
                    _animator.SetBool(_hashMoveBackward, true);
                }
                else
                {
                    _animator.SetBool(_hashMoveBackward, false);
                }
            }
            else
            {
                if (_attack == false)
                {
                    _animator.SetBool(_hashMoveBackward, false);
                    _animator.SetBool(_hashMoveForward, true);
                }
                else
                {
                    _animator.SetBool(_hashMoveForward, false);
                }
            }
        }
        else
        {
            if (_attack == false)
            {
                _animator.SetBool(_hashMoveForward, true);
            }
            else
            {
                _animator.SetBool(_hashMoveForward, false);
            }
        }
    }

    protected override void Attack()
    {
        if(_currentTime + _coolTime < Time.time)
        {
            LowLevelAi();
            _currentTime = Time.time;
        }
    }

    protected override void Die()
    {
        _die = true;
        GameManager.Instance.PlayerWin = true;
        _animator.SetTrigger(_hashKnockDown);
        _animator.SetTrigger(_hashDie);
    }


    private void LowLevelAi()
    {
        if (_attack)
        {
            int hittingPosition = Random.Range(1, 4);
            int attack = 0;
            switch (hittingPosition)
            {
                case 1:
                    {
                        attack = Random.Range(1, 5);
                        HighAttack(attack);
                        break;
                    }
                case 2:
                    {
                        attack = Random.Range(1, 5);
                        MiddleAttack(attack);
                        break;
                    }
                case 3:
                    {
                        attack = Random.Range(1, 3);
                        LowAttack(attack);
                        break;
                    }
                default:
                    break;
            }

        }
    }

    private void HighAttack(int attack)
    {
        if (_die)
        {
            Die();
        }
        switch (attack)
        {
            case 1:
                {
                    _animator.SetTrigger(_hashLPunch);
                    if (Physics.SphereCast(_handLVector, _punchRadius, Vector3.back, out hit, _maxDistance, Player))
                    {
                        _low = false;
                        _middle = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 2:
                {
                    _animator.SetTrigger(_hashRPunch);
                    if (Physics.SphereCast(_handRVector, _punchRadius, Vector3.back, out hit, _maxDistance, Player))
                    {
                        _low = false;
                        _middle = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 3:
                {
                    _animator.SetTrigger(_hashLKick);
                    if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, -1), out hit, _maxDistance, Player))
                    {
                        _low = false;
                        _middle = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 4:
                {
                    _animator.SetTrigger(_hashRKick);
                    if (Physics.SphereCast(_footRVector, _kickRadius + 1, new Vector3(1, 1, -1), out hit, _maxDistance, Player))
                    {
                        _low = false;
                        _middle = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            default:
                break;
        }
    }

    private void MiddleAttack(int attack)
    {
        if (_die)
        {
            Die();
        }
        switch (attack)
        {
            case 1:
                {
                    _animator.SetTrigger(_hashLMiddlePunch);
                    if (Physics.SphereCast(_handLVector, _punchRadius, Vector3.back, out hit, _maxDistance, Player))
                    {
                        _middle = _low = false;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 2:
                {
                    _animator.SetTrigger(_hashRMiddlePunch);
                    if (Physics.SphereCast(_handRVector, _punchRadius, Vector3.back, out hit, _maxDistance, Player))
                    {
                        _middle = _low = false;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 3:
                {
                    _animator.SetTrigger(_hashLMiddleKick);
                    if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, -1), out hit, _maxDistance, Player))
                    {
                        _middle = _low = false;
                        OnHitEnemy(hit, _low, _middle);
                    }

                    break;
                }
            case 4:
                {
                    _animator.SetTrigger(_hashRMiddleKick);
                    if (Physics.SphereCast(_footRVector, _kickRadius + 1, new Vector3(1, 1, -1), out hit, _maxDistance, Player))
                    {
                        _middle = _low = false;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            default:
                break;
        }
    }

    private void LowAttack(int attack)
    {
        if(_die)
        {
            Die();
        }
        switch (attack)
        {
            case 1:
                {
                    _animator.SetTrigger(_hashLLowKick);
                    if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, -1), out hit, _maxDistance, Player))
                    {
                        _middle = false;
                        _low = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            case 2:
                {
                    _animator.SetTrigger(_hashRLowKick);
                    if (Physics.SphereCast(_footRVector, _kickRadius + 1, new Vector3(1, 1, -1), out hit, _maxDistance, Player))
                    {
                        _middle = false;
                        _low = true;
                        OnHitEnemy(hit, _low, _middle);
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void TakeDamage(float damage, RaycastHit hit, bool low, bool middle)
    {
        int block = Random.Range(1, 11);
        if(!_die)
        {
            if(block < 8)
            {
                Hit(low, middle);
                _aiHealth -= damage;
            }
            else
            {
                Block(low, middle);
                _aiHealth -= damage / 4;
            }
        }
        Debug.Log(_aiHealth);
        if (_aiHealth <= 0 && !_die)
        {
            Die();
        }
    }

    protected override void Win()
    {
        if (GameManager.Instance.AiWin && !_win)
        {
            _animator.SetTrigger(_hashWin);
            _win = true;
        }
    }

    protected override void Hit(bool low, bool middle)
    {
        if (middle)
        {
            _animator.SetTrigger(_hashMiddleHit);
        }
        else if(low)
        {
            _animator.SetTrigger(_hashLowHit);
        }
        else
        {
            _animator.SetTrigger(_hashHit);
        }
    }

    protected override void OnHitEnemy(RaycastHit hit, bool low, bool middle)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage, hit, low, middle);
        }
    }

    protected override void Block(bool low, bool middle)
    {
        if (middle)
        {
            _animator.SetTrigger(_hashMiddleBlock);
        }
        else if (low)
        {
            _animator.SetTrigger(_hashLowBlock);
        }
        else
        {
            _animator.SetTrigger(_hashBlock);
        }
    }
}
