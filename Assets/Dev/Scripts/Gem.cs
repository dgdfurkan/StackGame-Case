using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunduzDev
{
	public class Gem : MonoBehaviour
	{
        public GemType MyGemType;
        [SerializeField] private CapsuleCollider _capsuleCollider;
        private bool _isGrowth = false;
        public bool isStack = false;

        private float _targetScale = .25f;
        private float _currentScale = 0f;
        private float _scaleSpeed = 1f;

        private void Awake()
        {
            StartCoroutine(Growing(UnityEngine.Random.Range(2, 5)));
        }

        public void InitilizeGem(GemType GType)
        {
            gameObject.transform.localScale = Vector3.zero;
            _isGrowth = false;
            _capsuleCollider.enabled = false;
            MyGemType = GType;
        }

        IEnumerator Growing(float value)
        {
            yield return new WaitForSeconds(value);

            while (_currentScale <= 1 && !isStack)
            {
                _currentScale = Mathf.Lerp(_currentScale, 1, _scaleSpeed * Time.deltaTime);
                transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);

                if(_currentScale >= _targetScale) MakeCollectable();

                yield return null;
            }
        }

        private void MakeCollectable()
        {
            _isGrowth = true;
            _capsuleCollider.enabled = true;
            
        }
    }
}
