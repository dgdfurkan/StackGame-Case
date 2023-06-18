using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunduzDev
{
	public class Gem : MonoBehaviour
	{
        GemType gemType;
        [SerializeField] private CapsuleCollider capsuleCollider;
        private bool isGrowth = false;

        private float scaleIncreaseRate = 0.2f;
        private float targetScale = .25f;
        private float currentScale = 0f;
        private float scaleSpeed = 1f;

        public void OnEnable()
        {
            
        }

        private void Awake()
        {
            StartCoroutine(Growing(UnityEngine.Random.Range(2, 5)));
        }

        public void InitilizeGem(GemType GType)
        {
            gameObject.transform.localScale = Vector3.zero;
            isGrowth = false;
            capsuleCollider.enabled = false;
            gemType = GType;
            Debug.Log(gemType.StartingPrice);
        }

        IEnumerator Growing(float value)
        {
            yield return new WaitForSeconds(value);

            while (currentScale <= 1)
            {
                currentScale = Mathf.Lerp(currentScale, 1, scaleSpeed * Time.deltaTime);
                transform.localScale = new Vector3(currentScale, currentScale, currentScale);

                if(currentScale >= targetScale) MakeCollectable();

                yield return null;
            }

            //MakeCollectable();
        }

        private void MakeCollectable()
        {
            isGrowth = true;
            capsuleCollider.enabled = true;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                GridManager.Instance.GenerateAfterCollect(transform.position);

                // Stack it
            }
        }
    }
}
