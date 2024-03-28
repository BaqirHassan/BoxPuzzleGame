using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    [Tooltip("All possible shapes.")]
    [SerializeField] List<ShapeData> shapeData;
    [Tooltip("The objects where we want to spawn Dragable shapes. The length of list will also determine the num of dragable shapes on screen.")]
    [SerializeField] public List<Shape> shapeList;


    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    void Start()
    {
        ShapeThatHasBenSelected.Clear();

        foreach (var shape in shapeList)
        {
            shape.CreateShape(shapeData[GetRandomUniqueShapeIndex()]);
        }
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if(!shape.isOnStartPosition() && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }

        Debug.LogError("There is no shape selected!");
        return null;
    }


    List<int> ShapeThatHasBenSelected = new List<int>();

    private int GetRandomUniqueShapeIndex()
    {
        if (shapeData.Count <= 0 || shapeList.Count <= 0)
        {
            Debug.LogError("Either there is no shapes available or there is no place to put it.", gameObject);
        }

        int shapeIndex = Random.Range(0, shapeData.Count);

        if (ShapeThatHasBenSelected.Contains(shapeIndex)) shapeIndex += shapeData.Count < 3 ? 1 : Random.Range(0, shapeData.Count - 1);

        shapeIndex %= shapeData.Count;

        ShapeThatHasBenSelected.Add(shapeIndex);

        return shapeIndex;

    }

    private void RequestNewShapes()
    {
        ShapeThatHasBenSelected.Clear();
        foreach (var shape in shapeList)
        {
            int temp = GetRandomUniqueShapeIndex();
            shape.RequestNewShape(shapeData[temp]);
        }
    }
}
