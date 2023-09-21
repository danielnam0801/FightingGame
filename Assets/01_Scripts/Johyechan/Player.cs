using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit, IDamageable
{
    //��Ʈ ������ ���� �� ���� �ٲ�ߵ� + AI ü�� �������� �� �� �� �����̴°� �� �̻��ؼ� �ٲٱ� �ؾߵ�

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
    private readonly int _hashLPunch        = Animator.StringToHash("LPunch");          // ��� ���� �ָ� ����
    private readonly int _hashRPunch        = Animator.StringToHash("RPunch");          // ��� ������ �ָ� ����
    private readonly int _hashLKick         = Animator.StringToHash("LKick");           // ��� ���� ������ ����
    private readonly int _hashRKick         = Animator.StringToHash("RKick");           // ��� ������ ������ ����
    private readonly int _hashLMiddlePunch  = Animator.StringToHash("LMiddlePunch");    // �ߴ� ���� �ָ� ����
    private readonly int _hashRMiddlePunch  = Animator.StringToHash("RMiddlePunch");    // �ߴ� ������ �ָ� ����
    private readonly int _hashLMiddleKick   = Animator.StringToHash("LMiddleKick");     // �ߴ� ���� ������ ����
    private readonly int _hashRMiddleKick   = Animator.StringToHash("RMiddleKick");     // �ߴ� ������ ������ ����
    private readonly int _hashLLowKick      = Animator.StringToHash("LLowKick");        // �ϴ� ���� ������ ����
    private readonly int _hashRLowKick      = Animator.StringToHash("RLowKick");        // �ϴ� ������ ������ ����
    private readonly int _hashHit           = Animator.StringToHash("Hit");             // ����� ����
    private readonly int _hashMiddleHit     = Animator.StringToHash("MiddleHit");       // �ߴ��� ����
    private readonly int _hashLowHit        = Animator.StringToHash("LowHit");          // �ϴ��� ����
    private readonly int _hashBlock         = Animator.StringToHash("Block");           // ����� ����
    private readonly int _hashMiddleBlock   = Animator.StringToHash("MiddleBlock");     // �ߴ��� ����
    private readonly int _hashLowBlock      = Animator.StringToHash("LowBlock");        // �ϴ��� ����
    private readonly int _hashKnockDown     = Animator.StringToHash("KnockDown");       // ������
    private readonly int _hashGetUp         = Animator.StringToHash("GetUp");           // �Ͼ
    private readonly int _hashDie           = Animator.StringToHash("Die");             // ������ ���·� ����
    private readonly int _hashWin           = Animator.StringToHash("Win");             // �¸� ���
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
