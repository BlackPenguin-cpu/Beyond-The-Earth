using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HommingBullet : BaseBullet
{
    public float hommingPower;
    public float hommingDuration;
    public GameObject target;

    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if (hommingDuration > 0 && target != null)
        {
            hommingDuration -= Time.deltaTime;
            Vector3 vec = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(vec.x, vec.y, vec.z, 0), hommingPower * Time.deltaTime);
        }
        Move();
    }
    protected override void Move()
    {
        base.Move();
        transform.position += transform.forward * speed * Time.deltaTime;
    }

}
