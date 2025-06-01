using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private GameObject _visual;

    [Header("Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private float _hp;

    private float _acceleration = 5f;
    private float _accelerationRemainingTime = 0f;

    public float Speed => _speed + ((_accelerationRemainingTime > 0) ? _acceleration : 0);

    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                Death();
                return;
            }
            _hpText.text = $"{(int)_hp}/{(int)_maxHp}";
        }
    }

    private void Start()
    {
        Hp = _maxHp;
    }

    private void Update()
    {
        if (_accelerationRemainingTime > 0)
            _accelerationRemainingTime -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
    }

    public void AddAccelearation(float acceleration, float duration)
    {
        _acceleration = acceleration;
        _accelerationRemainingTime = duration;
    }

    public void Death()
    {
        _gameOverMenu.SetActive(true);
        _speed = 0;
        Destroy(_visual);
    }
}
