using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public class FindingObject : MonoBehaviour
    {
        [SerializeField]
        private Transform[] borders;

        public FindingObjectState State;

        // Use this for initialization
        void Awake()
        {


        }
        private void OnEnable()
        {
            State = FindingObjectState.NoFound;
        }
        private void OnDisable()
        {
            State = FindingObjectState.Hacking;
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void ResetGame()
        {
            gameObject.SetActive(false);
            State = FindingObjectState.NoFound;
        }

        public bool CheckFinding(float radius, Vector3 center)
        {
            if (State == FindingObjectState.Hacking)
            {
                return false;
            }
            if (State == FindingObjectState.Found)
            {
                return true;
            }

            if (borders.Length > 0)
            {
                foreach (Transform tr in borders)
                {
                    if (Mathf.Pow(tr.position.x - center.x, 2) + Mathf.Pow(tr.position.y - center.y, 2) + Mathf.Pow(tr.position.z - center.z, 2) < Mathf.Pow(radius, 2))
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (Mathf.Pow(transform.position.x - center.x, 2) + Mathf.Pow(transform.position.y - center.y, 2) + Mathf.Pow(transform.position.z - center.z, 2) < Mathf.Pow(radius, 2))
                {

                }
                else
                {
                    return false;
                }
            }

            State = FindingObjectState.Found;
            return true;
        }
    }
    public enum FindingObjectState
    {
        /// <summary>
        /// Не найден
        /// </summary>
        NoFound,
        /// <summary>
        /// Найден
        /// </summary>
        Found,
        /// <summary>
        /// Взломан
        /// </summary>
        Hacking
    }
}
