using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunduzDev
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class CharacterController : MonoBehaviour
	{
        public static CharacterController Instance;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Animator _animator;

        [SerializeField] private float _moveSpeed;
        private float _totalJoystickValue = 0;

        [SerializeField] private Transform _stackTransform;
        public List<GameObject> _stackedGems = new List<GameObject>();

        bool isInside = false;

        private void Awake()
        {
            Instance = this;
        }

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
            int stackedCount = _stackedGems.Count;

            if (GameManager.Instance.CollectedDict.ContainsKey(gameObject.GetComponent<Gem>().MyGemType.GemName))
            {
                GameManager.Instance.CollectedDict[gameObject.GetComponent<Gem>().MyGemType.GemName]++;
            }
            else
            {
                GameManager.Instance.CollectedDict[gameObject.GetComponent<Gem>().MyGemType.GemName] = 1;
            }

            GameManager.Instance.SaveCollectedGems();

            gameObject.transform.SetParent(_stackTransform);
            gameObject.transform.localPosition = new Vector3(0, stackedCount, 0);

            _stackedGems.Add(gameObject);
        }

        public void SellGem(float price)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Gem")
            {
                GameObject myGem = other.gameObject;

                //Destroy(myGem.GetComponent<CapsuleCollider>());
                
                //myGem.GetComponent<Gem>().isStack = true;

                GridManager.Instance.GenerateAfterCollect(myGem.transform.position);
                
                StackGem(myGem.transform.parent.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Finish")
            {
                isInside = true;
                StartCoroutine(SellGemsQueue());
            }
            else
            {
                isInside = false;
            }
        }
        // Delete
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Finish")
            {
                isInside = false;
                StopCoroutine(SellGemsQueue());
            }
        }

        private IEnumerator SellGemsQueue()
        {
            while (_stackedGems.Count > 0 && isInside)
            {
                GameObject topGem = _stackedGems[_stackedGems.Count - 1];
                GameManager.Instance.AddGold(topGem.GetComponent<Gem>().MyGemType.StartingPrice + (topGem.transform.localScale.x * 100));
                _stackedGems.Remove(topGem);
                Destroy(topGem);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
