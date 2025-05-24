using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _openLogsActions;

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private GameObject _visual;

    [Header("UI")]
    [SerializeField] private GameObject _letterUI;
    [SerializeField] private TextMeshProUGUI _letterText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _logsText;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _logsMenu;

    [Header("Player Stats")]
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
            _hpText.text = $"{(int) _hp}/{(int) _maxHp}";
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

        if (_openLogsActions.action.WasPressedThisFrame())
            VisibilityLogs();

        var moveForward = _moveAction.action.ReadValue<Vector2>();
        var moveStep = Speed * Time.deltaTime * new Vector3(moveForward.x, 0, moveForward.y);
        InBoxMove(moveStep);
    }

    public void Death()
    {
        _gameOverMenu.SetActive(true);
        Destroy(this);
        Destroy(_visual);
    }

    private void InBoxMove(Vector3 moveStep)
    {
        var planeLenght = _planeTransform.localScale.x * 10;
        var planeWidth = _planeTransform.localScale.z * 10;
        var newPosition = transform.position + moveStep;

        var isInX = Math.Abs(newPosition.x - _planeTransform.position.x) < planeLenght / 2;
        var isInZ = Math.Abs(newPosition.z - _planeTransform.position.z) < planeWidth / 2;

        _characterController.Move(new Vector3(
            isInX ? moveStep.x : 0, 
            0,
            isInZ ? moveStep.z : 0));
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
    }

    public void ShowLetter(string letter)
    {
        _letterUI.SetActive(true);
        _letterText.text = letter;
    }

    public void AddAccelearation(float acceleration, float duration)
    {
        _acceleration = acceleration;
        _accelerationRemainingTime = duration;
    }

    public void GotObjectLogic(PickableObjectLogic pickableObject)
    {
        _logsText.text += $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} --- {pickableObject.Type}\n";
    }

    public void VisibilityLogs()
    {
        _logsMenu.SetActive(!_logsMenu.activeSelf);
    }

    private void OnApplicationQuit()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/logs.txt");
        bf.Serialize(file, _logsText.text);
        file.Close();
    }
}
