using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    [Header("Inputs")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _openLogsActions;

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerLogger _playerLogger;

    [Header("UI")]
    [SerializeField] private GameObject _letterUI;
    [SerializeField] private TextMeshProUGUI _letterText;



    void Update()
    {
        if (_openLogsActions.action.WasPressedThisFrame())
            _playerLogger.VisibilityLogs();

        var moveForward = _moveAction.action.ReadValue<Vector2>();
        var moveStep = _playerStats.Speed * Time.deltaTime * new Vector3(moveForward.x, 0, moveForward.y);
        InBoxMove(moveStep);
    }

    public void ShowLetter(string letter)
    {
        _letterUI.SetActive(true);
        _letterText.text = letter;
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
}
