using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    [SerializeField] Image occupiedImage;

    private void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    public void DeactivateShape()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivateShape()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        occupiedImage.gameObject.SetActive(true);
    }

    public void UnSetOccupied()
    {
        occupiedImage.gameObject.SetActive(false);
    }
}
