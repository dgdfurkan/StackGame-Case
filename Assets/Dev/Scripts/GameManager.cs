using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace GunduzDev
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

        private float totalGold;

        public Dictionary<string, int> CollectedDict = new Dictionary<string, int>();

        private void Awake()
        {
            Instance = this;
            StartGame();
            GetCollectedGems();
        }

        void StartGame()
        {
            totalGold = PlayerPrefs.GetFloat("Gold", 0);
            Debug.Log("Total Gold is: " + totalGold);
            UIManager.Instance.UpdateGoldText(totalGold);
        }

        public void AddGold(float value)
        {
            totalGold += value;
            PlayerPrefs.SetFloat("Gold", totalGold);
            Debug.Log("Total Gold is: " + totalGold);
            UIManager.Instance.UpdateGoldText(totalGold);
        }

        public void GetCollectedGems()
        {
            foreach (var item in GridManager.Instance.GemTypes)
            {
                if (PlayerPrefs.HasKey(item.GemName))
                {
                    CollectedDict.Add(item.GemName, PlayerPrefs.GetInt(item.GemName));
                    Debug.Log("GemName and total: " + PlayerPrefs.GetInt(item.GemName));
                }
            }
            UIManager.Instance.UpdateGemInformation();
        }

        public void SaveCollectedGems()
        {
            foreach (var kvp in CollectedDict)
            {
                string key = kvp.Key;
                int value = kvp.Value;

                PlayerPrefs.SetInt(key, value);
            }
            PlayerPrefs.Save();
            UIManager.Instance.UpdateGemInformation();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log(CollectedDict.Values);
                Debug.Log(CollectedDict.Keys);

                
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                foreach (var kvp in CollectedDict)
                {
                    string key = kvp.Key;
                    int value = kvp.Value;

                    // Anahtar ve deðeri kullanarak iþlemler yapabilirsiniz
                    Debug.Log("Key: " + key + ", Value: " + value);
                }


            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                int random = UnityEngine.Random.Range(0, 3);
                string namee = "";


                switch (random)
                {                           
                    case 0:
                        namee = "Green";
                        break;
                    case 1:
                        namee = "Pink";
                        break;
                    case 2:
                        namee = "Yellow";
                        break;
                }

                CollectedDict.Add(namee, +1);

            }
        }
    }
}
