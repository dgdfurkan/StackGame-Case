using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace GunduzDev
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance;

		[SerializeField] TextMeshProUGUI _goldText;
        private const string _goldMainInfo = "Gold: ";

        [SerializeField] private Transform _gemInformationTransform;
        [SerializeField] private GameObject _gemInformation;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateGoldText(float value)
        {
            int whole = Convert.ToInt32(value);
            //_goldText.text = _goldMainInfo + whole;

            int firstDigit = (whole - (whole % 10)) / 1000;

            int lastDigit = ((whole - (whole % 10)) / 100) % 10;

            if (firstDigit > 0)
            {
                _goldText.text = _goldMainInfo + (firstDigit + "." + lastDigit + "k");
            }
            else
            {
                _goldText.text = _goldMainInfo + whole.ToString();
            }

            if (whole.ToString().Length >= 7)
            {
                firstDigit = (whole - (whole % 10)) / 1000000;

                lastDigit = ((whole - (whole % 10)) / 100000) % 10;

                _goldText.text = _goldMainInfo + (firstDigit + "." + lastDigit + "m");
            }
        }

        public void UpdateGemInformation()
        {
            if( _gemInformation != null )
            {
                foreach (Transform child in _gemInformationTransform)
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (var kvp in GameManager.Instance.CollectedDict)
            {
                string key = kvp.Key;
                int value = kvp.Value;

                GameObject createdGame = Instantiate(_gemInformation);
                createdGame.transform.SetParent(_gemInformationTransform);

                foreach (var item in GridManager.Instance.GemTypes)
                {
                    if(item.GemName == key)
                    {
                        createdGame.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.GemIcon;
                        createdGame.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.GemName + " Gem";
                        createdGame.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Collected: " + value;
                    }
                }
            }
        }
    }
}
