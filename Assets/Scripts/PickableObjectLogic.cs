using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickableObjectLogic : MonoBehaviour
{
    [SerializeField] private List<TypesMaterials> materials = new List<TypesMaterials>();

    [SerializeField] private bool DestroyOnTouch = true;
    [SerializeField] private ObjectType _type;
    [SerializeField] private string _letter;

    private MeshRenderer _meshRenderer;

    public ObjectType Type
    {
        get => _type;
        set
        {
            _type = value;
            _meshRenderer.material = materials.Where(p => p.Type == _type).First().Material;
        }
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var player = other.GetComponent<Player>();
        switch (_type)
        {
            case ObjectType.Bonus:
                player.AddAccelearation(10, Random.Range(3, 5));
                break;

            case ObjectType.Bomb:
                player.TakeDamage(25);
                Destroy(gameObject);
                break;

            case ObjectType.Letter:
                player.ShowLetter(_letter);
                break;
        }
        player.GotObjectLogic(this);
        if (DestroyOnTouch)
            Destroy(gameObject);
    }

    public enum ObjectType
    {
        Bonus,
        Bomb,
        Letter,
    }

    [Serializable]
    public struct TypesMaterials
    {
        public ObjectType Type;
        public Material Material;
    }
}

