using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;
    public SquareTextureData squareTextureData;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;

    private Config.SquareColor currentActiveSquareColor = Config.SquareColor.None;

    private List<Config.SquareColor> colorsInTheGrid = new List<Config.SquareColor>();
    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].squareColor; 
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpadateSquareColor;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpadateSquareColor;
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void OnUpadateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllSquareColorsInTheGrid()
    {        
        var colors = new List<Config.SquareColor>();
        foreach (var square in _gridSquares)
            {
            var gridSqaure = square.GetComponent<GridSquare>();
            if (gridSqaure.SquareOccupied)
            {
                var color = gridSqaure.GetCurrentColor();
                if (colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }

    private void SpawnGridSquares()
    {
        //0, 1, 2, 3, 4,
        //5, 6, 7, 8, 9
        int square_index = 0;
        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }


    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;
        var square_rect = _gridSquares[0].GetComponent<RectTransform>();


        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;


        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                //go to the next column
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if(gridSquare.Selected  && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }

        var currentSelecShape = shapeStorage.GetCurrentSelectedShape();
        if(currentSelecShape == null) { return; } // No currently Selcted Shape

        if(currentSelecShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
            }

            int shapeLeft = 0;

            foreach (var shape in shapeStorage.shapeList)
            {
                if(shape.isOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if(shapeLeft == 0)
            {
                GameEvents.RequestNewShapes?.Invoke();
            }
            else
            {
                GameEvents.SetShapeInactive?.Invoke();
            }

            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }        
    }


    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        //columns
        foreach (var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetverticalLine(column));
        }

        //rows
        for (int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);

            for (int index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }


        // Squares
        for (int square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for (int index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.square_data[square, index]);
            }

            lines.Add(data.ToArray());
        }

        /// This function needs to be called before <see cref="CheckIfSquaresAreCompleted(List{int[]})">CheckIfSquaresAreCompleted</see>.
        colorsInTheGrid = GetAllSquareColorsInTheGrid();

        var completedLines = CheckIfSquaresAreCompleted(lines);

        if(completedLines > 1)
        {
            GameEvents.ShowCongratulationsWritings?.Invoke();
        }

        // Add Scores. 
        var totalScore = GameConstants.Instance.ScorePerLineComplete * completedLines;
        var bonusScore = ShouldPlayColorBonusAnimation();
        GameEvents.AddScores?.Invoke(totalScore + bonusScore);
        CheckIfPlayerLost();
    }


    private int ShouldPlayColorBonusAnimation()
    {
        var colorsInTheGridAfterLineRemoved = GetAllSquareColorsInTheGrid(); 
        Config.SquareColor colorToPlayBonusFor = Config.SquareColor.None;

        foreach (var squareColor in colorsInTheGrid)
        {
            if (colorsInTheGridAfterLineRemoved.Contains(squareColor) == false)
            {
                colorToPlayBonusFor = squareColor;
            }
        }


        if (colorToPlayBonusFor == Config.SquareColor.None)
        {
            Debug.Log("Cannot find Color for bonus"); 
            return 0;
        }

        //Should never play bonus for the current color.
        if (colorToPlayBonusFor == currentActiveSquareColor)
        {
            return 0;
        }

        GameEvents.ShowBonusScreen?.Invoke(colorToPlayBonusFor);

        return 50;
    }



    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        int linesCompleted = 0;

        foreach (var line in data)
        {
            bool lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();

                if(comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }

            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            bool completed = false;

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
                comp.ClearOccupied();
            }

            if(completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;

        for (int index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();

            if (CheckIfShapeCanBePlaceonGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }

        if(validShapes == 0)
        {
            // GameOver
            GameEvents.GameOver?.Invoke(false);

            Debug.Log("Game Over");
        }
    }

    private bool CheckIfShapeCanBePlaceonGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.Columns;
        var shapeRows = currentShapeData.rows;

        // All indexes of filled up shapes.
        List<int> originalShapeFilledSquares = new List<int>();
        var squareIndex = 0;

        for (int rowindex = 0; rowindex < shapeRows; rowindex++)
        {
            for (int columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowindex].column[columnIndex])
                {
                    originalShapeFilledSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        if(currentShape.TotalSquareNumber != originalShapeFilledSquares.Count)
        {
            Debug.LogError("Number of filled up squares are not the same as the original shape have.");
        }

        var squareList = GetAllSquaresCombinations(shapeColumns, shapeRows);

        bool canBePlaced = false;

        foreach (var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;

            foreach (var squareIndexToCheck in originalShapeFilledSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();

                if(comp.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if(shapeCanBePlacedOnTheBoard)
                canBePlaced = true;
        }

        return canBePlaced;
    }

    private List<int[]> GetAllSquaresCombinations(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (rows -1) < 9)
        {
            var rowData = new List<int>();

            for (int row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (int column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if(lastColumnIndex + (columns - 1) >= 9)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }

            safeIndex++;

            if(safeIndex > 100)
            {
                Debug.LogError("While Loop is running endlessly", gameObject);
                break;
            }
        }

        return  squareList;
    }
}