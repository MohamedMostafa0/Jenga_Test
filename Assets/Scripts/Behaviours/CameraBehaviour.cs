using JengaTest.Helpers;
using JengaTest.Managers;
using JengaTest.Utils;
using UnityEngine;

namespace JengaTest.Behaviours
{
    public class CameraBehaviour : BaseSingleton<CameraBehaviour>
    {
        private Transform target;
        [SerializeField] private float speed;

        private Vector3 camOffset;

        protected override void OnAwake()
        {
            GameSceneManager.Instance.ChangeGenerationEventHandler += OnChangeGeneration;
            OnChangeGeneration(GenerationEnum.Gen7);
        }
        private void OnChangeGeneration(GenerationEnum generation)
        {
            target = GameSceneManager.Instance.generationPositions[(int)generation];
            Vector3 newPos = new(target.position.x, 3, -10);
            transform.position = newPos;
            camOffset = transform.position - target.position;
            transform.LookAt(target);
        }
        private void Update()
        {
            if (Input.GetMouseButton(1))
                Rotate();
        }
        private void Rotate()
        {
            float axisX = Input.GetAxis("Mouse X");
            if (axisX != 0)
            {
                float horizontalInput = axisX * speed * Time.deltaTime;
                Quaternion camTurnAngle = Quaternion.AngleAxis(horizontalInput, Vector3.up);
                camOffset = camTurnAngle * camOffset;
                Vector3 pos = target.position + camOffset;
                transform.position = Vector3.Slerp(transform.position, pos, 0.1f);
                transform.LookAt(target);
            }
        }
    }
}
