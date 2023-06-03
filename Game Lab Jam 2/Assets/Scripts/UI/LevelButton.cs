using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int number;

    private void Start()
    {
        number = transform.GetSiblingIndex() + 1;
        text.text = number.ToString();
    }

    public void OnClick()
    {
        SoundManager.Instance.Play("Confirm");
        SceneTransitioner.Instance.FadeToScene("Level " + number);
    }

}
