using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolClass
{
    public GameObject parentObj, activeGroupObj, inactiveGroupObj;
    public Queue<GameObject> objNotActiveQueue = new Queue<GameObject>();
    /// <summary>
    /// 해당 풀이 가질수 있는 활성화된 오브젝트의 최대 갯수. 값이 -1일경우 제한 없음
    /// </summary>
    public int maxObjCount = -1;

    public ObjectPoolClass(GameObject _parentObj, GameObject _activeGroupObj, GameObject _inactiveGroupObj, int _maxObjCount = -1)
    {
        parentObj = _parentObj;
        activeGroupObj = _activeGroupObj;
        inactiveGroupObj = _inactiveGroupObj;
        objNotActiveQueue = new Queue<GameObject>();
        maxObjCount = _maxObjCount;
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
            if (parentObjList[obj.name].maxObjCount != -1 && parentObjList[obj.name].maxObjCount < parentObjList[obj.name].activeGroupObj.transform.childCount)
            {
                returnObj(parentObjList[obj.name].activeGroupObj.transform.GetChild(0).gameObject);
                return createObj(obj);
            }
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
                poolObj.transform.parent = parentObjList[obj.name].activeGroupObj.transform;
                return poolObj;
            }
        }
        else
        {
            var parentObj = new GameObject($"{obj.name}_Parent");
            var activeObjPool = new GameObject($"{obj.name}__ActiveObjs");
            var inactiveObjPool = new GameObject($"{obj.name}_InactiveObjs");
            var objectPoolClass = new ObjectPoolClass(parentObj, activeObjPool, inactiveObjPool);

            activeObjPool.transform.parent = parentObj.transform;
            inactiveObjPool.transform.parent = parentObj.transform;

            activeObjPool.transform.parent = this.gameObject.transform;

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

    public void returnObj(GameObject obj)
    {
        if (parentObjList.ContainsKey(textCloneRemove(obj.name)))
        {
            string objOriginalName = textCloneRemove(obj.name);

            obj.SetActive(false);
            obj.transform.parent = parentObjList[objOriginalName].inactiveGroupObj.transform;
            parentObjList[objOriginalName].objNotActiveQueue.Enqueue(obj);
        }
        else
        {
            Debug.Log("경고. 오브젝트풀 오브젝트가 아닌 오브젝트를 오브젝트풀 형식으로 해제하려 하였습니다.");
            Destroy(obj);
        }
    }

    public void MaxObjCountChange(GameObject targetObj, int objCount)
    {
        parentObjList[textCloneRemove(targetObj.name)].maxObjCount = objCount;
    }

    private string textCloneRemove(string objName)
    {
        StringBuilder builder = new StringBuilder(objName);
        builder.Replace("(Clone)", "");
        return builder.ToString();
    }
}
