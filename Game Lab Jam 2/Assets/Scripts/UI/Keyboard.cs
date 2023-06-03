using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;

    [SerializeField] private UnityEvent<string> onDone;

    public void SetInputField(TextMeshProUGUI inputField)
    {
        this.inputField = inputField;
    }

    public void Add(string text)
    {
        inputField.text += text;
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
    }

    public void Done()
    {
        Debug.Log("Closing keyboard with output: " + inputField.text);
        onDone.Invoke(inputField.text);
    }
}
