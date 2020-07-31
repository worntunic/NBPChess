using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleNotification : MonoBehaviour
{
    public Text text;
    public Image background;
    public float fullColorDuration, activeDuration;
    private bool notificationActive = false;
    private float currentTime = 0;
    private Color textColor, textFadeColor;
    private Color bkgColor, bkgFadeColor;
    private void Awake()
    {
        textColor = text.color;
        textFadeColor = textColor;
        bkgColor = background.color;
        bkgFadeColor = bkgColor;

        bkgFadeColor.a = textFadeColor.a = 0;

    }

    private void Update()
    {
        UpdateNotificationStatus();
    }

    private void UpdateNotificationStatus()
    {
        if (notificationActive)
        {
            if (currentTime >= activeDuration)
            {
                gameObject.SetActive(false);
                notificationActive = false;
            }
            else if (currentTime >= fullColorDuration)
            {
                float t = (currentTime - fullColorDuration) / (activeDuration / fullColorDuration);
                text.color = Color.Lerp(textColor, textFadeColor, t);
                background.color = Color.Lerp(bkgColor, bkgFadeColor, t);
            }
            currentTime += Time.deltaTime;

        }
    }

    public void CreateNotification(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
        currentTime = 0;
        text.color = textColor;
        background.color = bkgColor;
        notificationActive = true;
    }
}
