using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable] 
    public class Row
    {
        public bool[] column;
        private int _size = 0;

        public Row()
        {

        }

        public Row(int size)
        {
            createRow(size);
        }

        public void createRow(int size)
        {
            _size = size;
            column = new bool[size];
            ClearRow();
        }


        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                column[i] = false;
            }
        }
    }

    public int Columns = 0;
    public int rows = 0;
    public Row[] board;

    public void Clear()
    {
        for (int i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }
    
    public void CreateNewBoard()
    {
        board = new Row[rows];

        for (int i = 0; i < rows; i++)
        {
            board[i] = new Row(Columns);
        }
    }
}
