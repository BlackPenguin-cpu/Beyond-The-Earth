using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Normalize
{
    public class Move : MonoBehaviour
    {
        public float speed = 2f;
        Transform target;

        void Update()
        {
            if (target == null) return;

            Vector2 nor = (target.position - transform.position).normalized;
            transform.Translate(nor * Time.deltaTime * speed);
        }

        public void SetTarget(Transform _target)
        {
            target = _target;
        }
    }
}
