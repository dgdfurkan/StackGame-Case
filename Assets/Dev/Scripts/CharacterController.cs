using System.Collections.Generic;
using UnityEngine;

namespace GunduzDev
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class CharacterController : MonoBehaviour
	{
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Animator _animator;

        [SerializeField] private float _moveSpeed;
        private float _totalJoystickValue = 0;

        private List<GameObject> stackedGems = new List<GameObject>();

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);

                _totalJoystickValue = Mathf.Sqrt(_joystick.Horizontal * _joystick.Horizontal + _joystick.Vertical * _joystick.Vertical);
                _animator.SetFloat("CharacterSpeed", _totalJoystickValue);
            }
            else
            {
                _animator.SetFloat("CharacterSpeed", 0);

                if (_totalJoystickValue <= 0)
                    return;

                _totalJoystickValue -= Time.fixedDeltaTime;
                _animator.SetFloat("CharacterSpeed", _totalJoystickValue);

            }
        }

        public void StackGem(GameObject gameObject)
        {
            
        }
    }
}
