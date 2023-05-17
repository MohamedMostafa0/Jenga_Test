using UnityEngine;

namespace JengaTest.Behaviours
{
    public class RotationBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        private void FixedUpdate()
        {
            transform.Rotate(0, 0, -speed * Time.deltaTime);
        }
    }
}
