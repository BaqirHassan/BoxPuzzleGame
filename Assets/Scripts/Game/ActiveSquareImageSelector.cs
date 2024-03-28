using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
    [SerializeField] private SquareTextureData squareTextureData;
    [SerializeField] private bool updateImageOnReachTreshold = false;

    private void OnEnable()
    {
        UpdateSquareColorBaseOnCurrentPoints();

        if(updateImageOnReachTreshold)
        {
            GameEvents.UpdateSquareColor += UpdateSquareColor;
        }
    }

    private void OnDisable()
    {
        if (updateImageOnReachTreshold)
        {
            GameEvents.UpdateSquareColor -= UpdateSquareColor;
        }
    } 

    private void UpdateSquareColorBaseOnCurrentPoints()
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if(squareTextureData.currentColor == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }

    private void UpdateSquareColor(Config.SquareColor color)
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if (color == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }
}
