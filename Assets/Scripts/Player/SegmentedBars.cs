using UnityEngine;
using UnityEngine.UI;

public class SegmentedBars : MonoBehaviour
{
    public Image healthBar;
    public Sprite[] bars;
    
    public float maxHp = 100f;

    public void UpdateHealth(float currentHp)
    {
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        if (currentHp > 75f)
        {
            healthBar.sprite = bars[0];
            healthBar.fillAmount = (currentHp - 75f) / 25f;
        }
        else if (currentHp > 50f)
        {
            healthBar.sprite = bars[1];
            healthBar.fillAmount = (currentHp - 50f) / 25f;
        }
        else if (currentHp > 0f)
        {
            healthBar.sprite = bars[2];
            healthBar.fillAmount = currentHp / 50f;
        }
        else
        {
            healthBar.sprite = bars[3];
            healthBar.fillAmount = 0f;
        }
    }
}

