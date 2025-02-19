using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] GameObject squareShapeImage;

    [Tooltip("The Scale for the shape that is currently selected.")]
    /// The Scale for the shape that is currently selected.
    [SerializeField] Vector3 SelectedShapeScale;

    [Tooltip("The amount we want currently selected shape to offset from the finger position.")]
    /// The amount we want currently selected shape to offset from the finger position.
    [SerializeField] Vector2 offSet = new Vector2(0, 700);

    [HideInInspector]
    public ShapeData CurrentShapeData;

    public int TotalSquareNumber { get; set; }

    private List<GameObject> _currentShapes = new List<GameObject>();
    private Vector3 _shapeStartingScale;
    private RectTransform _transform;
    private bool _isDragable = true;
    private Canvas _canvas;
    private Vector3 StartPosition;
    private bool _shapeActive = true;

    private void Awake()
    {
        _shapeStartingScale = GetComponent<RectTransform>().localScale;
        _transform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        StartPosition = _transform.localPosition;
        _shapeActive = true;
    }

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;
    }

    public bool isOnStartPosition()
    {
        return _transform.localPosition == StartPosition;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        foreach (var square in _currentShapes)
        {
            if(square.gameObject.activeSelf)
                return true;
        }

        return false;
    }

    public void DeactivateShape()
    {
        if(_shapeActive)
        {
            foreach (var square in _currentShapes)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
        }

        _shapeActive = false;
    }

    private void SetShapeInactive()
    {
        if(!isOnStartPosition() && IsAnyOfShapeSquareActive())
        {
            foreach (var square in _currentShapes)
            {
                square.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShapes)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }

        _shapeActive = true;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = StartPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);

        while( _currentShapes.Count < TotalSquareNumber)
        {
            _currentShapes.Add(Instantiate(squareShapeImage, transform) as GameObject );
        }

        foreach (var square in _currentShapes)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var SquareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(SquareRect.rect.width * SquareRect.localScale.x,
              SquareRect.rect.height * SquareRect.localScale.y);

        //Debug.LogError(moveDistance);

        int currentIndexInList = 0;

        // Set Position to from final Shape

        for (int row = 0; row < shapeData.rows; row++)
        {
            for (int column = 0; column < shapeData.Columns; column++)
            {
                if (shapeData.board[row].column[column]) 
                {
                    _currentShapes[currentIndexInList].SetActive(true);
                        Vector2 newPosition = new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), 
                        GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    _currentShapes[currentIndexInList].GetComponent<RectTransform>().localPosition = newPosition;

                    //Debug.LogError("Position = row = " + row + "  Colunm = " + column + "Position = " + newPosition, _currentShapes[currentIndexInList]);

                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 MoveDistance)
    {
        float shiftOnY = 0;

        if (shapeData.rows > 1) // Vertical Position Calculation
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;

                if (row < middleSquareIndex)            // Move it on negative
                {
                    shiftOnY = MoveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex)     // Move it on Plus
                {
                    shiftOnY = MoveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : shapeData.rows / 2;
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : shapeData.rows - 1;  // May be Need to Change it to 2
                var multiplier = shapeData.rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        shiftOnY = MoveDistance.y / 2 * -1;
                    if (row == middleSquareIndex1)
                        shiftOnY = (MoveDistance.y / 2) * 1;
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2)      //Move it on negative
                {
                    shiftOnY = MoveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2)    // Move it to plus
                {
                    shiftOnY = MoveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
        }

        return shiftOnY;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 MoveDistance)
    {
        float shiftOnX = 0;

        if(shapeData.Columns > 1) // Vertical Position Calculation
        {
            if(shapeData.Columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.Columns - 1) / 2;
                var multiplier = (shapeData.Columns - 1) / 2;

                if(column < middleSquareIndex)
                {
                    shiftOnX = MoveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column > middleSquareIndex)     // Move it on Plus
                {
                    shiftOnX = MoveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.Columns == 2) ? 1 : shapeData.Columns / 2;
                var middleSquareIndex1 = (shapeData.Columns == 2) ? 0 : shapeData.Columns - 1;
                var multiplier = shapeData.Columns / 2;

                if(column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                        shiftOnX = MoveDistance.x / 2;
                    if(column == middleSquareIndex1)
                        shiftOnX = (MoveDistance.x / 2 ) * -1;

                    //Debug.Log("Equal shiftOnX = " + shiftOnX);
                }

                if (column < middleSquareIndex1 && column < middleSquareIndex2)      //Move it on negative
                {
                    shiftOnX = MoveDistance.x * -1;
                    shiftOnX *= multiplier;
                    //Debug.Log("Less shiftOnX = " + shiftOnX);
                }
                else if(column > middleSquareIndex1 && column  > middleSquareIndex2)    // Move it to plus
                {
                    shiftOnX = MoveDistance.x * 1;
                    shiftOnX *= multiplier;
                    //Debug.Log("Right shiftOnX = " + shiftOnX);
                }
            }
        }

        return shiftOnX;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;

        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if(active)
                    number++;
            }
        }

        return number;
    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = StartPosition;
    }


    #region InterFace implimentation

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _transform.localScale = SelectedShapeScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = Vector2.zero;
        _transform.anchorMax = Vector2.zero;
        _transform.pivot = Vector2.zero;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);

        _transform.localPosition = pos + offSet;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _transform.localScale = _shapeStartingScale;
        GameEvents.CheckIfShapeCanBePlaced?.Invoke();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }


    #endregion
}
