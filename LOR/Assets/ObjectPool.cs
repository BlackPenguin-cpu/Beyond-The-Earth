using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolClass
{
    public GameObject parentObj;
    public Queue<GameObject> objNotActiveQueue = new Queue<GameObject>();
    public Queue<GameObject> objActiveQueue = new Queue<GameObject>();

    public ObjectPoolClass(GameObject _parentObj)
    {
        parentObj = _parentObj;
        objActiveQueue = new Queue<GameObject>();
        objNotActiveQueue = new Queue<GameObject>();
    }
}
public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, ObjectPoolClass> parentObjList;
    public GameObject createObj(GameObject obj)
    {
        if (obj.GetComponent<IObjectPoolObject>() == null) Debug.Log("경고 - 오브젝트풀 인터페이스를 상속받지 않은 오브젝트가 오브젝트풀을 사용중입니다.");

        if (parentObjList.ContainsKey(obj.name))
        {
            //미리 생성되어 비활성화된 오브젝트풀 오브젝트를 체크합니다
            if (parentObjList[obj.name].objNotActiveQueue.Count > 0)
            {
                var poolObj = parentObjList[obj.name].objNotActiveQueue.Dequeue();
                poolObj.SetActive(true);
                poolObj.TryGetComponent(out IObjectPoolObject objPoolinterface);
                objPoolinterface.onCreate();
                return poolObj;
            }
            else
            {
                var poolObj = Instantiate(obj);
                parentObjList[obj.name].objNotActiveQueue.Enqueue(poolObj);
                return poolObj;
            }
        }
        else
        {
            var parentObj = new GameObject($"{obj.name}_Parent");
            var objectPoolClass = new ObjectPoolClass(parentObj);

            parentObj.transform.parent = this.gameObject.transform;

            parentObjList.Add(obj.name, objectPoolClass);
            return createObj(obj);
        }

    }
    public GameObject createObj(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        var localObj = createObj(obj);
        localObj.transform.position = pos;
        localObj.transform.rotation = quaternion;
        return localObj;
    }
}
