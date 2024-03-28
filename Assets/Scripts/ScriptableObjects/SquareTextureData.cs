using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu, System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }

    public int tresholdVal = 10;
    private const int StartTresholdVal = 10;

    public List<TextureData> activeSquareTextures = new List<TextureData>();

    [SerializeField] public Config.SquareColor currentColor;
    private Config.SquareColor nextColor;

    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;

        for (int index = 0; index < activeSquareTextures.Count; index++)
        {
            if (activeSquareTextures[index].squareColor == currentColor)
            {
                currentIndex = index;
            }
        }

        return currentIndex;
    }


    public void UpdateColors(int Current_Score)
    {
        currentColor = nextColor;
        var CurrentColorIndex = GetCurrentColorIndex();

        nextColor = activeSquareTextures[ (CurrentColorIndex + 1) % activeSquareTextures.Count].squareColor;

        tresholdVal = StartTresholdVal + Current_Score;
    }

    public void SetStartColor()
    {
        tresholdVal = StartTresholdVal;
        currentColor = activeSquareTextures[0].squareColor;
        nextColor = activeSquareTextures[1].squareColor;
    }

    private void Awake()
    {
        SetStartColor();
    }

    private void OnEnable()
    {
        SetStartColor();
    }
}
