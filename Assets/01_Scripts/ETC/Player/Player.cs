using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : Unit, IDamageable
{
    // 움직이거나 공격 또는 맞았을 경우 차징 멈추기 추가
    public UnityEvent HitSoundPlay;
    public UnityEvent BlockSoundPlay;

    public GameObject visual;

    private CharacterController _player;

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

    private Animator _animator;

    [SerializeField]
    private GameManager _gameManager;

    private readonly int _hashMoveForward   = Animator.StringToHash("MoveForward");
    private readonly int _hashMoveBackward  = Animator.StringToHash("MoveBackward");
    private readonly int _hashLPunch        = Animator.StringToHash("LPunch");        
    private readonly int _hashRPunch        = Animator.StringToHash("RPunch");          
    private readonly int _hashLLowKick      = Animator.StringToHash("LKick");        
    private readonly int _hashRLowKick      = Animator.StringToHash("RKick");       
    private readonly int _hashHit           = Animator.StringToHash("Hit");  
    private readonly int _hashLowHit        = Animator.StringToHash("LowHit");      
    private readonly int _hashBlock         = Animator.StringToHash("Block");       
    private readonly int _hashKnockDown     = Animator.StringToHash("KnockDown");   
    private readonly int _hashGetUp         = Animator.StringToHash("GetUp");       
    private readonly int _hashDie           = Animator.StringToHash("Die");         
    private readonly int _hashWin           = Animator.StringToHash("Win");

    [SerializeField]
    private float _playerHealth = 100;
    [SerializeField]
    private float _maxHealth = 100;
    [SerializeField]
    private float _playerUltimate = 0;
    [SerializeField]
    private float _maxUltimate = 100;

    private bool _low = false;
    private bool _useUltimate = false;
    private bool _win = false;
    private bool _isBlock = false;
    private bool _charging = false;

    private RaycastHit hit;

    private Vector3 _punchVec;
    private Vector3 _kickVec;
    private Vector3 _ultimateVec;

    private PlayerSetting _ps;

    private Image _hp;
    private Image _ultimate;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _ps = GetComponent<PlayerSetting>();
        _player = GetComponent<CharacterController>();
    }

    protected override void Start()
    {
        _playerHealth = _maxHealth;
        _playerUltimate = 0;
        base.Start();
        if (_ps.state == PlayerSpawnState.left)
        {
            _punchVec = Vector3.right;
            _ultimateVec = Vector3.right;
            _kickVec = new Vector3(1, 1, 0);
            _hp = GameObject.Find("FrontHealthBar1").GetComponent<Image>();
            _ultimate = GameObject.Find("FrontUltimateBar1").GetComponent<Image>();
            _hp.fillAmount = 1;
            _ultimate.fillAmount = 0;
        }
        else if(_ps.state == PlayerSpawnState.right)
        {
            _punchVec = Vector3.left;
            _ultimateVec = Vector3.left;
            _kickVec = new Vector3(-1, 1, 0);
            _hp = GameObject.Find("FrontHealthBar2").GetComponent<Image>();
            _ultimate = GameObject.Find("FrontUltimateBar2").GetComponent<Image>();
            _hp.fillAmount = 1;
            _ultimate.fillAmount = 0;
        }
    }

    protected override void Update()
    {
        _hp.fillAmount = _playerHealth / _maxHealth;
        _ultimate.fillAmount = _playerUltimate / _maxUltimate;
        _handLVector = new Vector3(_handL.position.x, _handL.position.y + 0.05f, _handL.position.z + 0.03f);
        _handRVector = new Vector3(_handR.position.x, _handR.position.y + 0.05f, _handR.position.z + 0.03f);
        _footLVector = new Vector3(_footL.position.x, _footL.position.y - 0.05f, _footL.position.z);
        _footRVector = new Vector3(_footR.position.x, _footR.position.y - 0.05f, _footR.position.z);
        _ultimateVector = _body.position;
        if(!_die && !_win && _playerHealth > 0)
        {
            if(!_charging)
            {
                Attack();
                Move();
                Ultimate();
            }
            Win();
            Charging();
        }
        Debug.Log(_playerUltimate);
    }

    protected override void Move()
    {
        if (_ps.state == PlayerSpawnState.left)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _animator.SetTrigger(_hashMoveBackward);
                Invoke("MoveL", 0.3f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _animator.SetTrigger(_hashMoveForward);
                Invoke("MoveR", 0.3f);
            }
        }
        else if(_ps.state == PlayerSpawnState.right)
        {
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                _animator.SetTrigger(_hashMoveBackward);
                Invoke("MoveR", 0.3f);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger(_hashMoveForward);
                Invoke("MoveL", 0.3f);
            }
        }
    }

    protected override void Charging()
    {
        
        if(_ps.state == PlayerSpawnState.left)
        {
            if(Input.GetKey(KeyCode.E) && _playerUltimate < _maxUltimate)
            {
                _charging = true;
                _animator.SetBool(_hashMoveForward, false);
                _animator.SetBool(_hashMoveBackward, false);
                // bool로 차징 애니메이션 나오기 
                _playerUltimate += 0.05f;
            }
            if(Input.GetKeyUp(KeyCode.E))
            {
                _charging = false;
                // 차징 애니메이션 끝내기
            }
        }
        else if(_ps.state == PlayerSpawnState.right)
        {
            if(Input.GetKey(KeyCode.P) && _playerUltimate < _maxUltimate)
            {
                _charging = true;
                _animator.SetBool(_hashMoveForward, false);
                _animator.SetBool(_hashMoveBackward, false);
                // bool로 차징 애니메이션 나오기
                _playerUltimate += 0.05f;
            }
            if (Input.GetKeyUp(KeyCode.P))
            {
                _charging = false;
                // 차징 애니메이션 끝내기
            }
        }
    }

    protected override void Ultimate()
    {
        if (_ps.state == PlayerSpawnState.left)
        {
            if (_playerUltimate >= _maxUltimate && Input.GetKeyDown(KeyCode.Q))
            {
                // Trigger로 궁극기 애니메이션 나오기 
                if (Physics.Raycast(_ultimateVector, _ultimateVec, out hit, 1f))
                {
                    _useUltimate = true;
                    _playerUltimate = 0;
                    OnHitEnemy(hit, _low, _useUltimate);
                }
                else
                {
                    _playerUltimate = 0;
                }
            }
        }
        else if (_ps.state == PlayerSpawnState.right)
        {
            if (_playerUltimate >= _maxUltimate && Input.GetKeyDown(KeyCode.I))
            {
                // Trigger로 궁극기 애니메이션 나오기
                if (Physics.Raycast(_ultimateVector, _ultimateVec, out hit, 1f))
                {
                    _useUltimate = true;
                    _playerUltimate = 0;
                    OnHitEnemy(hit, _low, _useUltimate);
                }
                else
                {
                    _playerUltimate = 0;
                }
            }
        }
    }

    protected override void Attack()
    {
        if (_ps.state == PlayerSpawnState.left)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _useUltimate = false;
                _low = false;
                RandomPunch();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _useUltimate = false;
                _low = true;
                RandomKick();
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _animator.SetBool(_hashBlock, true);
                _isBlock = true;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                _animator.SetBool(_hashBlock, false);
                _isBlock = false;
            }
        }
        else if (_ps.state == PlayerSpawnState.right)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _useUltimate = false;
                _low = false;
                RandomPunch();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                _useUltimate = false;
                _low = true;
                RandomKick();
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                _animator.SetBool(_hashBlock, true);
                _isBlock = true;
            }
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                _animator.SetBool(_hashBlock, false);
                _isBlock = false;
            }
        }
    }

    private void RandomPunch()
    {
        int rand = Random.Range(1, 11);
        if (rand > 5)
        {
            _animator.SetTrigger(_hashLPunch);
            if (Physics.SphereCast(_handLVector, _punchRadius, _punchVec, out hit, _maxDistance))
            {
                OnHitEnemy(hit, _low, _useUltimate);
            }
        }
        else
        {
            _animator.SetTrigger(_hashRPunch);
            if (Physics.SphereCast(_handRVector, _punchRadius, _punchVec, out hit, _maxDistance))
            {
                OnHitEnemy(hit, _low, _useUltimate);
            }
        }
    }

    private void RandomKick()
    {
        int rand = Random.Range(1, 11);
        if (rand > 5)
        {
            _animator.SetTrigger(_hashLLowKick);
            if (Physics.BoxCast(_footLVector, _boxSize / 2, _kickVec.normalized, out hit))
            {
                OnHitEnemy(hit, _low, _useUltimate);
            }
        }
        else
        {
            _animator.SetTrigger(_hashRLowKick);
            if (Physics.BoxCast(_footRVector, _boxSize / 2, _kickVec.normalized, out hit))
            {
                OnHitEnemy(hit, _low, _useUltimate);
            }
        }
    }

    protected override void Die()
    {
        _die = true;
        GameManager.Instance.AiWin = true;
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

    protected override void OnHitEnemy(RaycastHit hit, bool low, bool ultimate)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage, hit, low, ultimate);
        }
    }

    public void TakeDamage(float damage, RaycastHit hit, bool low, bool ultimate)
    {
        _charging = false;
        if (!_die)
        {
            _playerUltimate += 5f;
            if(_isBlock)
            {
                if (ultimate)
                    _playerHealth -= damage;
                else
                    _playerHealth -= damage / 2;
                Block(low, ultimate);
            }
            else
            {
                if (ultimate)
                    _playerHealth -= damage * 5;
                else
                    _playerHealth -= damage;
                Hit(low, ultimate);
            }
        }
        if (_playerHealth <= 0 && !_die)
        {
            Die();
        }
    }

    protected override void Win()
    {
        if(_ps.state == PlayerSpawnState.left)
        {
            if (GameManager.Instance.PlayerWin1 && !_win)
            {
                _animator.SetTrigger(_hashWin);
                _win = true;
            }
        }
        else if (_ps.state == PlayerSpawnState.right)
        {
            if (GameManager.Instance.PlayerWin2 && !_win)
            {
                _animator.SetTrigger(_hashWin);
                _win = true;
            }
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
        else if (ultimate)
        {
            _animator.SetTrigger(_hashKnockDown);
            if (_playerHealth > 0)
                Invoke("GetUp", 0.5f);
            else
                Die();
        }
        else
        {
            _animator.SetTrigger(_hashHit);
            Invoke("HeadHitEffect", 0.3f);
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

    private void MoveL()
    {
        _player.Move(Vector3.left / 3);
    }

    private void MoveR()
    {
        _player.Move(Vector3.right / 3);
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
