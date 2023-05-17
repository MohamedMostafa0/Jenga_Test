using UnityEngine;
using JengaTest.Helpers;
using JengaTest.Managers;
using JengaTest.Models;

namespace JengaTest.Behaviours
{
    public class Block : MonoBehaviour
    {
        public ItemType itemType;
        public StackModel StackModel { get; private set; }
        private Rigidbody rb;
        public void Init(Vector3 pos, Quaternion rot, StackModel stackModel)
        {
            transform.SetPositionAndRotation(pos, rot);
            StackModel = stackModel;
        }
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void Test()
        {
            if(itemType == ItemType.Glass)
                gameObject.SetActive(false);
            else
                rb.isKinematic = false;
        }
    }
}
