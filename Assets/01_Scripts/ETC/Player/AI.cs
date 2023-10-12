using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;

public class AI : Unit, IDamageable
{
    //차징구현_랜덤을 어디에 어떻게 적용시켜야 차징중에는 아무것도 안하고 차징이 멈춰야 다른 걸 할까
    public UnityEvent HitSoundPlay; 
    public UnityEvent BlockSoundPlay; 

    public GameObject visual;

    private Animator _animator;

    [SerializeField]
    private Transform _handL;
    [SerializeField]
    private Transform _handR;
    [SerializeField]
    private Transform _footL;
    [SerializeField]
    private Transform _footR;
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Transform _body;
    [SerializeField]
    private Transform _leg;

    private bool _attack = false;
    private bool _low = false;
    private bool _useUltimate = false;
    private bool _win = false;
    private bool _charging = false;

    private float _currentTime = 0;
    [SerializeField]
    private float _coolTime = 1f;
    [SerializeField]
    private float _aiHealth = 100;
    [SerializeField]
    private float _maxHealth = 100;
    [SerializeField]
    private float _aiUltimate = 0;
    [SerializeField]
    private float _maxUltimate = 100;

    private RaycastHit hit;

    private readonly int _hashMoveForward = Animator.StringToHash("MoveForward");
    private readonly int _hashMoveBackward = Animator.StringToHash("MoveBackward");
    private readonly int _hashLPunch = Animator.StringToHash("LPunch");                  
    private readonly int _hashRPunch = Animator.StringToHash("RPunch");                  
    private readonly int _hashLKick = Animator.StringToHash("LKick");                    
    private readonly int _hashRKick = Animator.StringToHash("RKick");
    private readonly int _hashHit = Animator.StringToHash("Hit");
    private readonly int _hashLowHit = Animator.StringToHash("LowHit");                  
    private readonly int _hashBlock = Animator.StringToHash("Block");
    private readonly int _hashKnockDown = Animator.StringToHash("KnockDown");            
    private readonly int _hashGetUp = Animator.StringToHash("GetUp");                    
    private readonly int _hashDie = Animator.StringToHash("Die");                        
    private readonly int _hashWin = Animator.StringToHash("Win");                        

    private Vector3 _punchVec;
    private Vector3 _kickVecR;
    private Vector3 _kickVecL;
    private Vector3 _checkVec;
    private Vector3 _ultimateVec;

    private PlayerSetting _ps;

    private Image _hp;
    private Image _ultimate;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _ps = GetComponent<PlayerSetting>();
    }

    protected override void Start()
    {
        _aiHealth = _maxHealth;
        _aiUltimate = 0;
        base.Start();
        if (_ps.state == PlayerSpawnState.left)
        {
            _punchVec = Vector3.right;
            _checkVec = Vector3.right;
            _ultimateVec = Vector3.right;
            _kickVecR = new Vector3(1, 1, 1);
            _kickVecL = new Vector3(1, 1, 0);
            _hp = GameObject.Find("FrontHealthBar1").GetComponent<Image>();
            _ultimate = GameObject.Find("FrontUltimateBar1").GetComponent<Image>();
            _hp.fillAmount = 1;
            _ultimate.fillAmount = 0;
        }
        else if (_ps.state == PlayerSpawnState.right)
        {
            _punchVec = Vector3.left;
            _checkVec = Vector3.left;
            _ultimateVec = Vector3.left;
            _kickVecR = new Vector3(-1, 1, 1);
            _kickVecL = new Vector3(-1, 1, 0);
            _hp = GameObject.Find("FrontHealthBar2").GetComponent<Image>();
            _ultimate = GameObject.Find("FrontUltimateBar2").GetComponent<Image>();
            _hp.fillAmount = 1;
            _ultimate.fillAmount = 0;
        }
    }

    protected override void Update()
    {
        _hp.fillAmount = _aiHealth / _maxHealth;
        _ultimate.fillAmount = _aiUltimate / _maxUltimate;
        _handLVector = new Vector3(_handL.position.x, _handL.position.y + 0.05f, _handL.position.z - 0.03f);
        _handRVector = new Vector3(_handR.position.x, _handR.position.y + 0.05f, _handR.position.z - 0.03f);
        _footLVector = new Vector3(_footL.position.x, _footL.position.y - 0.05f, _footL.position.z - 0.07f);
        _footRVector = new Vector3(_footR.position.x, _footR.position.y - 0.05f, _footR.position.z - 0.07f);
        _ultimateVector = _body.position;
        if (!_die && !_win && _aiHealth > 0)
        {
            Charging();
            Move();
            Attack();
            Ultimate();
            RayCheck();
            Win();
        }
    }

    private void RayCheck()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, -0.6f, transform.position.z), _checkVec);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            _attack = true;
        }
        else
        {
            _attack = false;
        }
    }

    protected override void Move()
    {
        int rand;
        if (_aiHealth <= 1)
        {
            rand = Random.Range(1, 4);
            if (rand > 2)
            {
                if (!_attack && !_useUltimate)
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
                if (_attack == false && !_useUltimate)
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
            if (_attack == false && !_useUltimate)
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
        if (_currentTime + _coolTime < Time.time)
        {
            LowLevelAi();
            _currentTime = Time.time;
        }
    }

    protected override void Die()
    {
        _die = true;
        if(_ps.state == PlayerSpawnState.left)
        {
            GameManager.Instance.PlayerWin2 = true;
        }
        else if(_ps.state == PlayerSpawnState.right)
        {
            GameManager.Instance.PlayerWin1 = true;
        }
        _animator.SetTrigger(_hashKnockDown);
        _animator.SetTrigger(_hashDie);
    }


    private void LowLevelAi()
    {
        if (_attack)
        {
            int attack = Random.Range(1, 5);
            Attack(attack);
        }
    }

    protected override void Charging()
    {
        if(_charging)
        {
            if(_aiUltimate == _maxUltimate)
            {
                _charging = false;
                return;
            }
            // bool로 차징 애니메이션 나오기 
            _aiUltimate += 0.01f;
        }
    }

    protected override void Ultimate()
    {
        if (_aiUltimate == _maxUltimate)
        {
            // Trigger로 궁극기 애니메이션 나오기 
            if (Physics.Raycast(_ultimateVector, _ultimateVec, out hit, 1f))
            {
                _useUltimate = true;
                _aiUltimate = 0;
                OnHitEnemy(hit, _low, _useUltimate);
            }
            else
            {
                _aiUltimate = 0;
            }
        }
    }

    private void Attack(int attack)
    {
        _useUltimate = false;
        if (_die)
        {
            Die();
        }
        switch (attack)
        {
            case 1:
                {
                    _animator.SetTrigger(_hashLPunch);
                    if (Physics.SphereCast(_handLVector, _punchRadius, _punchVec, out hit, _maxDistance))
                    {
                        _low = false;
                        OnHitEnemy(hit, _low, _useUltimate);
                    }
                    break;
                }
            case 2:
                {
                    _animator.SetTrigger(_hashRPunch);
                    if (Physics.SphereCast(_handRVector, _punchRadius, _punchVec, out hit, _maxDistance))
                    {
                        _low = false;
                        OnHitEnemy(hit, _low, _useUltimate);
                    }
                    break;
                }
            case 3:
                {
                    _animator.SetTrigger(_hashLKick);
                    if (Physics.SphereCast(_footLVector, _kickRadius, _kickVecL, out hit, _maxDistance))
                    {
                        _low = true;
                        OnHitEnemy(hit, _low, _useUltimate);
                    }
                    break;
                }
            case 4:
                {
                    _animator.SetTrigger(_hashRKick);
                    if (Physics.SphereCast(_footRVector, _kickRadius + 1, _kickVecR, out hit, _maxDistance))
                    {
                        _low = true;
                        OnHitEnemy(hit, _low, _useUltimate);
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void TakeDamage(float damage, RaycastHit hit, bool low, bool ultimate)
    {
        int block = Random.Range(1, 11);
        if (!_die)
        {
            _aiUltimate += 0.5f;
            _charging = false;
            if (block < 8)
            {
                if (ultimate)
                    _aiHealth -= damage * 5;
                else
                    _aiHealth -= damage;
                Hit(low, ultimate);
            }
            else
            {
                if (ultimate)
                    _aiHealth -= damage;
                else
                    _aiHealth -= damage / 2;
                Block(low, ultimate);
            }
        }
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

    protected override void Hit(bool low, bool ultimate)
    {
        HitSound();
        if (low)
        {
            _animator.SetTrigger(_hashLowHit);
            Invoke("LegHitEffect", 0.3f);
        }
        else if(ultimate)
        {
            _animator.SetTrigger(_hashKnockDown);
            Invoke("GetUp", 0.5f);
        }
        else
        {
            _animator.SetTrigger(_hashHit);
            Invoke("HeadHitEffect", 0.3f);
        }
    }

    protected override void OnHitEnemy(RaycastHit hit, bool low, bool ultimate)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage, hit, low, ultimate);
        }
    }

    protected override void Block(bool low, bool ultimate)
    {
        BlockSound();
        if (low)
        {
            Invoke("LegHitEffect", 0.3f);
        }
        else if(ultimate)
        {
            Invoke("LegHitEffect", 0.3f);
            Invoke("HeadHitEffect", 0.3f);
        }
        else
        {
            Invoke("HeadHitEffect", 0.3f);
        }
    }

    private void GetUp()
    {
        _animator.SetTrigger(_hashGetUp);
    }

    private void HitSound()
    {
        HitSoundPlay.Invoke();
    }

    private void BlockSound()
    {
        BlockSoundPlay.Invoke();
    }

    private void HeadHitEffect()
    {
        Instantiate(visual, _head);
        Destroy(visual, 0.5f);
    }

    private void LegHitEffect()
    {
        Instantiate(visual, _leg);
        Destroy(visual, 0.5f);
    }
}
