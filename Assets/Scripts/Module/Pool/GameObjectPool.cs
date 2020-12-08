using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameObjectPool
{
    private List<GameObject> _acticveList;
    private List<GameObject> _inactiveList;
    private bool _isDestroying;
    private int _minNum;

    private GameObject _prefab;
    private GameObject _selfGO;
    private DateTime _spawnTime;

    private readonly int _useFrequency = 5;

    public void Init(GameObject prefab, Transform parent, int preloadCount, bool autoDestroy)
    {
        _isDestroying = false;
        _minNum = preloadCount;
        _acticveList = new List<GameObject>(preloadCount);
        _inactiveList = new List<GameObject>(preloadCount);
        _prefab = prefab;
        _selfGO = new GameObject(_prefab.name + "Pool");
        _selfGO.transform.SetParent(parent);
        Preload(preloadCount);

        if (autoDestroy)
            AutoDstroy();
    }

    private async void AutoDstroy()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var spendTime = (DateTime.Now - _spawnTime ).Seconds;
            if (spendTime >= _useFrequency && !_isDestroying)
            {
                _isDestroying = true;
                StartDestroy();
            }
        }
    }

    private async void StartDestroy()
    {
        GameObject go = null;
        while (_inactiveList.Count > _minNum)
        {
            await Task.Delay(100);
            go = _inactiveList[0];
            _inactiveList.Remove(go);
            Object.Destroy(go);
        }

        _isDestroying = false;
    }

    private void Preload(int count)
    {
        GameObject temp = null;
        for (var i = 0; i < count; i++)
        {
            temp = SpawnNew();
            _inactiveList.Add(temp);
            temp.SetActive(false);
        }
    }

    public GameObject Spawn()
    {
        _spawnTime = DateTime.Now;
        GameObject temp = null;
        if (_inactiveList.Count > 0)
        {
            temp = _inactiveList[0];
            _inactiveList.Remove(temp);
            temp.SetActive(true);
        }
        else
        {
            temp = SpawnNew();
        }

        _acticveList.Add(temp);
        return temp;
    }

    private GameObject SpawnNew()
    {
        return Object.Instantiate(_prefab, _selfGO.transform);
    }

    public void Despawn(GameObject go)
    {
        if (_acticveList.Contains(go))
        {
            _acticveList.Remove(go);
            _inactiveList.Add(go);
            go.SetActive(false);
        }
    }
    
    public int GetActiveNum()
    {
        return _acticveList.Count;
    }
}
