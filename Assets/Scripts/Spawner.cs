using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private GameObject _spawnPrefab;

    private Stack<GameObject> _spawningPool = new(10);

    private void Start()
    {
        StartCoroutine(AutoSpawner());
    }

    public void ReturnToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _spawningPool.Push(gameObject);
    }

    private IEnumerator AutoSpawner()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(5);
        }
    }

    public GameObject TakeFromPool()
    {
        if (_spawningPool.Count <= 0)
        {
            var obj = Instantiate(_spawnPrefab, Vector3.zero, Quaternion.identity);
            _spawningPool.Push(obj);
        }
        return _spawningPool.Pop();
    }

    public void SpawnObject() 
    {
        var planeLenght = _planeTransform.localScale.x * 10;
        var planeWidth = _planeTransform.localScale.z * 10;

        var posForSpawn = _planeTransform.transform.position 
            + Vector3.right * Random.Range(-planeLenght / 2, planeLenght / 2)
            + Vector3.up * 0.5f
            + Vector3.forward * Random.Range(-planeWidth / 2, planeWidth / 2);

        var obj = TakeFromPool();
        obj.transform.position = posForSpawn;
        obj.SetActive(true);
        obj.GetComponent<PickableObjectLogic>().Type = (PickableObjectLogic.ObjectType) Random.Range(0, 3);
    }
}
