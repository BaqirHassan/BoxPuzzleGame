using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]

public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if(ShapeDataInstance.board != null && ShapeDataInstance.Columns > 0 && ShapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if(GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    public void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    public void DrawColumnsInputFields()
    {
        var columnsTemp = ShapeDataInstance.Columns;
        var rowsTemp = ShapeDataInstance.rows;

        ShapeDataInstance.Columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.Columns);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);

        if((ShapeDataInstance.Columns != columnsTemp || ShapeDataInstance.rows != rowsTemp) &&
                ShapeDataInstance.Columns > 0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    public void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;


        for (int row = 0; row < ShapeDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for (int column = 0; column < ShapeDataInstance.Columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].column[column], dataFieldStyle);
                ShapeDataInstance.board[row].column[column] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
