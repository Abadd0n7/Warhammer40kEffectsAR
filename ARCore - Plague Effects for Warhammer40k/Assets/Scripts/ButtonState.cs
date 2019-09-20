using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonState : MonoBehaviour
{
    public Action<ButtonValue> OnButtonClick;
    private Button ButtonToClick;

    [SerializeField]
    ButtonValue B_Value;

    // Start is called before the first frame update
    void Start()
    {
        ButtonToClick = GetComponent<Button>();
        ButtonToClick.onClick.AddListener(OnClickButton);
    }
    void OnClickButton()
    {
        OnButtonClick?.Invoke(B_Value);
    }
    void OnDestroy()
    {
        ButtonToClick.onClick.RemoveListener(OnClickButton);
    }
}

