using UnityEngine;
using UnityEngine.UI;

public class ImageFitSize : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;

    public void SetImageSize()
    {
        if (image.sprite != null)
        {
            float imageHeight = image.sprite.rect.height;
            float originalHeight = rectTransform.rect.height;
            float multiplier = originalHeight / imageHeight;

            rectTransform.sizeDelta = new Vector2(image.sprite.rect.width * multiplier, originalHeight);

        }
    }


}
