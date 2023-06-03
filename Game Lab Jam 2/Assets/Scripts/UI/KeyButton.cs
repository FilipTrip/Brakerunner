using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Start()
    {
        text.text = gameObject.name;
    }

    public void OnClick()
    {
        string output = text.text;

        if (output.Length == 1)
        {
            GetComponentInParent<Keyboard>().Add(output);
        }

        else if (output == "SPACE")
        {
            GetComponentInParent<Keyboard>().Add(" ");
        }

        else if (output == "DONE")
        {
            GetComponentInParent<Keyboard>().Done();
        }

        else if (output == "DEL")
        {
            GetComponentInParent<Keyboard>().Backspace();
        }

        else
        {
            Debug.Log("KeyButton: " + gameObject.name + " gave no output.");
        }
    }
}
