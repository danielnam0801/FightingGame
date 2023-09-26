using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit, IDamageable
{
    //히트 범위는 내가 좀 못함 바꿔야됨 + AI 체력 낮아졌을 때 앞 뒤 움직이는거 좀 이상해서 바꾸긴 해야됨

    public LayerMask Enemy;

    [SerializeField]
    private Transform _handL;
    [SerializeField]
    private Transform _handR;
    [SerializeField]
    private Transform _footL;
    [SerializeField]
    private Transform _footR;

    private Animator _animator;

    [SerializeField]
    private GameManager _gameManager;

    #region AnimationHashs
    private readonly int _hashMoveForward   = Animator.StringToHash("MoveForward");
    private readonly int _hashMoveBackward  = Animator.StringToHash("MoveBackward");
    private readonly int _hashLPunch        = Animator.StringToHash("LPunch");          // 상단 왼쪽 주먹 공격
    private readonly int _hashRPunch        = Animator.StringToHash("RPunch");          // 상단 오른쪽 주먹 공격
    private readonly int _hashLKick         = Animator.StringToHash("LKick");           // 상단 왼쪽 발차기 공격
    private readonly int _hashRKick         = Animator.StringToHash("RKick");           // 상단 오른쪽 발차기 공격
    private readonly int _hashLMiddlePunch  = Animator.StringToHash("LMiddlePunch");    // 중단 왼쪽 주먹 공격
    private readonly int _hashRMiddlePunch  = Animator.StringToHash("RMiddlePunch");    // 중단 오른쪽 주먹 공격
    private readonly int _hashLMiddleKick   = Animator.StringToHash("LMiddleKick");     // 중단 왼쪽 발차기 공격
    private readonly int _hashRMiddleKick   = Animator.StringToHash("RMiddleKick");     // 중단 오른쪽 발차기 공격
    private readonly int _hashLLowKick      = Animator.StringToHash("LLowKick");        // 하단 왼쪽 발차기 공격
    private readonly int _hashRLowKick      = Animator.StringToHash("RLowKick");        // 하단 오른쪽 발차기 공격
    private readonly int _hashHit           = Animator.StringToHash("Hit");             // 상단을 맞음
    private readonly int _hashMiddleHit     = Animator.StringToHash("MiddleHit");       // 중단을 맞음
    private readonly int _hashLowHit        = Animator.StringToHash("LowHit");          // 하단을 맞음
    private readonly int _hashBlock         = Animator.StringToHash("Block");           // 상단을 막음
    private readonly int _hashMiddleBlock   = Animator.StringToHash("MiddleBlock");     // 중단을 막음
    private readonly int _hashLowBlock      = Animator.StringToHash("LowBlock");        // 하단을 막음
    private readonly int _hashKnockDown     = Animator.StringToHash("KnockDown");       // 쓰러짐
    private readonly int _hashGetUp         = Animator.StringToHash("GetUp");           // 일어남
    private readonly int _hashDie           = Animator.StringToHash("Die");             // 쓰러진 상태로 지속
    private readonly int _hashWin           = Animator.StringToHash("Win");             // 승리 모션
    #endregion

    [SerializeField]
    private float _playerHealth = 3;

    private bool _low = false;
    private bool _middle = false;
    private bool _win = false;

    private RaycastHit hit;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        _handLVector = new Vector3(_handL.position.x, _handL.position.y + 0.05f, _handL.position.z + 0.03f);
        _handRVector = new Vector3(_handR.position.x, _handR.position.y + 0.05f, _handR.position.z + 0.03f);
        _footLVector = new Vector3(_footL.position.x, _footL.position.y - 0.05f, _footL.position.z + 0.07f);
        _footRVector = new Vector3(_footR.position.x, _footR.position.y - 0.05f, _footR.position.z + 0.07f);
        if(!_die || !_win)
        {
            Attack();
            Move();
            Win();
        }
    }

    protected override void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _animator.SetBool(_hashMoveBackward, true);
        }
        if (Input.GetKey(KeyCode.E))
        {
            _animator.SetBool(_hashMoveForward, true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _animator.SetBool(_hashMoveBackward, false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            _animator.SetBool(_hashMoveForward, false);
        }
    }

    protected override void Attack()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                _animator.SetTrigger(_hashLMiddlePunch);
                if (Physics.SphereCast(_handLVector, _punchRadius, Vector3.forward, out hit, _maxDistance, Enemy))
                {
                    _low = false;
                    _middle = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _animator.SetTrigger(_hashRMiddlePunch);
                if (Physics.SphereCast(_handRVector, _punchRadius, Vector3.forward, out hit, _maxDistance, Enemy))
                {
                    _low = false;
                    _middle = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                _animator.SetTrigger(_hashLMiddleKick);
                if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, 1), out hit, _maxDistance, Enemy))
                {
                    _low = false;
                    _middle = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger(_hashRMiddleKick);
                if (Physics.SphereCast(_footRVector, _kickRadius + 1, Vector3.one, out hit, _maxDistance, Enemy))
                {
                    _low = false;
                    _middle = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
        }
        else if (Input.GetKey(KeyCode.X))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _animator.SetTrigger(_hashLLowKick);
                if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, 1), out hit, _maxDistance, Enemy))
                {
                    _middle = false;
                    _low = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger(_hashRLowKick);
                if (Physics.SphereCast(_footRVector, _kickRadius + 1, Vector3.one, out hit, _maxDistance, Enemy))
                {
                    _middle = false;
                    _low = true;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                _animator.SetTrigger(_hashLPunch);
                if (Physics.SphereCast(_handLVector, _punchRadius, Vector3.forward, out hit, _maxDistance, Enemy))
                {
                    _middle = _low = false;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _animator.SetTrigger(_hashRPunch);
                if (Physics.SphereCast(_handRVector, _punchRadius, Vector3.forward, out hit, _maxDistance, Enemy))
                {
                    _middle = _low = false;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                _animator.SetTrigger(_hashLKick);
                if (Physics.SphereCast(_footLVector, _kickRadius, new Vector3(0, 1, 1), out hit, _maxDistance, Enemy))
                {
                    _middle = _low = false;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger(_hashRKick);
                if (Physics.SphereCast(_footRVector, _kickRadius + 1, Vector3.one, out hit, _maxDistance, Enemy))
                {
                    _middle = _low = false;
                    OnHitEnemy(hit, _low, _middle);
                }
            }
        }
    }

    protected override void Die()
    {
        _die = true;
        GameManager.Instance.AiWin = true;
        _animator.SetTrigger(_hashKnockDown);
        _animator.SetTrigger(_hashDie);
    }

    protected override void OnHitEnemy(RaycastHit hit, bool low, bool middle)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage, hit, low, middle);
        }
    }

    public void TakeDamage(float damage, RaycastHit hit, bool low, bool middle)
    {
        if(!_die)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                Block(low, middle);
                _playerHealth -= damage / 4;
            }
            else
            {
                Hit(low, middle);
                _playerHealth -= damage;
            }
        }
        Debug.Log(_playerHealth);
        if (_playerHealth <= 0 && !_die)
        {
            Die();
        }
    }

    protected override void Win()
    {
        if(GameManager.Instance.PlayerWin && !_win)
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
        else if (low)
        {
            _animator.SetTrigger(_hashLowHit);
        }
        else
        {
            _animator.SetTrigger(_hashHit);
        }
    }

    protected override void Block(bool low, bool middle)
    {
        if(middle)
        {
            _animator.SetTrigger(_hashMiddleBlock);
        }
        else if(low)
        {
            _animator.SetTrigger(_hashLowBlock);
        }
        else
        {
            _animator.SetTrigger(_hashBlock);
        }
    }
}
