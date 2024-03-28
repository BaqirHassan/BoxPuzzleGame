using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    [SerializeField] Image hoverImage;
    [SerializeField] Image activeImage;
    [SerializeField] Image normalImage;
    [SerializeField] List<Sprite> normalImages;

    private Config.SquareColor currentSquareColor = Config.SquareColor.None;

    public Config.SquareColor GetCurrentColor()
    {
        return currentSquareColor;
    }

    public bool Selected { set; get; }
    public int SquareIndex{set; get; }
    public bool SquareOccupied { set; get; }

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    //Temp Function. Remove it
    public bool CanWePlaceHere()
    {
        return hoverImage.gameObject.activeSelf;
    }


    public void PlaceShapeOnBoard(Config.SquareColor color)
    {
        currentSquareColor = color;
        ActivateSquare();
    }

    public void ActivateSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        currentSquareColor = Config.SquareColor.None;
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        currentSquareColor = Config.SquareColor.None;
        Selected = false;
        SquareOccupied = false;
    }

    public void SetImage(bool SetFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = SetFirstImage ? normalImages[1] : normalImages[0];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }
        else
        {
            collision.GetComponent<ShapeSquare>()?.SetOccupied();
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;

        if(!SquareOccupied)
        {
            hoverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = false;
            hoverImage.gameObject.SetActive(false);
        }
        else
        {
            collision.GetComponent<ShapeSquare>()?.UnSetOccupied();
        }
    }
}
