using UnityEngine;
using UnityEngine.UI;

public class CharacterUIBar : MonoBehaviour
{
    [Header("Components")]
    public Transform UIBarPoint;

    [Header("Prefab")]
    public GameObject characterUIBarPrefab;

    [Header("Character UI Bar Info")]
    public Transform characterUIBarTrans;
    public Image hpBarSlider;
    public Image skillGaugeBarSlider;

    [Header("UI Bar Name")]
    public string hpBarSliderName;          //FIXME: �ק令���Φr���J
    public string skillGaugeBarSliderName;


    private void Start()
    {
        UIBarInitialize();
    }

    private void LateUpdate()
    {
        if (characterUIBarTrans != null)
        {
            characterUIBarTrans.position = UIBarPoint.position;
        }
    }

    private void UIBarInitialize()
    {
        characterUIBarTrans = Instantiate(characterUIBarPrefab, BattlefieldUI.Instance.characterUIBarCanvas.transform).transform;
        hpBarSlider = characterUIBarTrans.Find(hpBarSliderName).GetComponent<Image>();
        hpBarSlider.fillAmount = 1f;
        //TODO: �K�[skill gauge bar
    }


    public void UpdateUIBar(float maxHp, float currentHp, float maxSkillGauge, float currentSkillGauge)
    {
        if (currentHp <= 0f)
        {
            characterUIBarTrans.gameObject.SetActive(false);
            
        }

        float sliderPercent = (float)(currentHp / maxHp);
        hpBarSlider.fillAmount = sliderPercent;
        //TODO: �K�[skill gauge bar�p��


    }

}
