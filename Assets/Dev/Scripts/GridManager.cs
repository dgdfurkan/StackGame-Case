using UnityEngine;
using System.Collections.Generic;
using System;
//#if UNITY_EDITOR
using UnityEditor;
//# endif

namespace GunduzDev
{
    [System.Serializable]
    public class GemType
    {
        public string GemName;
        public int StartingPrice;
        public Sprite GemIcon;
        public GameObject GemModel;
    }

    public class GridManager : MonoBehaviour
	{
        public static GridManager Instance;

        public int GridRowCount = 3;
        public int GridColumnCount = 3;
        public Vector3 Padding = Vector3.zero;
        //public int GemTypeID; // To create same gems
        private int _gemValue;
        [Space(10)]
        public List<GemType> GemTypes = new List<GemType>();

        private void Awake()
        {
            Instance = this;
        }

        public void GenerateGrid()
        {
            for (int row = 0; row < GridRowCount; row++)
            {
                for (int col = 0; col < GridColumnCount; col++)
                {
                    Vector3 position = new Vector3(Padding.x * col, 0, Padding.z * row);
                    GameObject tile = Instantiate(GemTypes[RandomGemNumberGenerator()].GemModel, position, Quaternion.identity);
                    tile.GetComponent<Gem>().InitilizeGem(GemTypes[_gemValue]);
                    tile.transform.parent = transform;
                }
            }
        }

        public void GenerateAfterCollect(Vector3 vector)
        {
            Vector3 position = new Vector3(vector.x, 0, vector.z);
            GameObject tile = Instantiate(GemTypes[RandomGemNumberGenerator()].GemModel, position, Quaternion.identity);
            tile.GetComponent<Gem>().InitilizeGem(GemTypes[_gemValue]);
            tile.transform.parent = transform;
        }

        int RandomGemNumberGenerator()
        {
            int random = UnityEngine.Random.Range(0, GemTypes.Count);
            _gemValue = random;
            return random;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(GridManager))]
    public class GridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This helps you to make grids.", MessageType.Info);

            DrawDefaultInspector();

            GridManager gridManager = (GridManager)target;

            if (GUILayout.Button("Generate Grids"))
            {
                //try
                //{
                //    GemType gtype = gridManager.GemTypes[gridManager.GemTypeID];
                //    gridManager.GenerateGrid();
                //}
                //catch (Exception)
                //{
                //    ErrorMessageBox.ShowError("Something wrong about 'GemTypeID', please handle it!");
                //}

                if(gridManager.GridRowCount <= 0 || gridManager.GridColumnCount <= 0)
                {
                    ErrorMessageBox.ShowError("Something wrong about 'GridRowCount' or 'GridColumnCount', please handle it!");
                }

                gridManager.GenerateGrid();
            }
        }
    }
#endif

    public class ErrorMessageBox : EditorWindow
    {
        private string errorMessage;

        public static void ShowError(string errorMessage)
        {
            ErrorMessageBox window = GetWindow<ErrorMessageBox>(true, "Error Message");            
            window.errorMessage = errorMessage;

            Vector2 windowSize = new Vector2(300, 200);
            Vector2 windowPosition = GUIUtility.GUIToScreenPoint(new Vector2(Screen.width, Screen.height) * 0.5f) - windowSize * 0.5f;
            windowPosition.y -= 50;
            window.position = new Rect(windowPosition, windowSize);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(errorMessage, EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("OK"))
            {
                Close();
            }
        }
    }
}
