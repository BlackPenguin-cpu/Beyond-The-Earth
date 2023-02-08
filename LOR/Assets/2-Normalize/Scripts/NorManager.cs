using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Normalize
{
    public class NorManager : MonoBehaviour
    {
        [Header("Objects")]
        public Move move;
        public Transform[] targets;

        [Header("UI")]
        public Transform buttonTr;
        public Button buttonPrefab;

        void Start()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Button temp = Instantiate(buttonPrefab, buttonTr);

                int index = i;
                temp.onClick.AddListener(() => SetTarget(index));
                temp.GetComponentInChildren<Text>().text = targets[i].gameObject.name;
            }

            buttonPrefab.gameObject.SetActive(false);
        }

        void SetTarget(int index)
        {
            move.SetTarget(targets[index]);
        }
    }
}
