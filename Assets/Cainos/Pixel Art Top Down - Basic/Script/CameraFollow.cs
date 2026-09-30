using UnityEngine;
using Zenject;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : ITickable, IInitializable
    {
        public Transform Target;
        public float LerpSpeed = 1.0f;

        private Transform _cameraTransorm;
        private Vector3 _offset;
        private Vector3 _targetPos;

        public CameraFollow(Transform target)
        {
            Target = target;
        }

        public void Initialize()
        {
            if (Target == null) return;

            _offset = _cameraTransorm.position - Target.position;
        }

        public void Tick()
        {
            if (Target == null) return;

            _targetPos = Target.position + _offset;
            _cameraTransorm.position = Vector3.Lerp(_cameraTransorm.position, _targetPos, LerpSpeed * Time.deltaTime);
        }
    }
}
