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
            UIManager.Instance.UpdateGoldText(totalGold);
        }

        public void AddGold(float value)
        {
            totalGold += value;
            PlayerPrefs.SetFloat("Gold", totalGold);
            UIManager.Instance.UpdateGoldText(totalGold);
        }

        public void GetCollectedGems()
        {
            foreach (var item in GridManager.Instance.GemTypes)
            {
                if (PlayerPrefs.HasKey(item.GemName))
                {
                    CollectedDict.Add(item.GemName, PlayerPrefs.GetInt(item.GemName));
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
    }
}
