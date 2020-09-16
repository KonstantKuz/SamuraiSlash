using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicator_Slider : MonoBehaviour
{    
    private Slider slider;
    private bool toIncrease;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        //Observer.Instance.OnDirectionRollResult += SetValue;
        //Observer.Instance.OnChangeRollDirection += ChangeDirection;
    }

    private void SetValue(int value)
    {
        if(toIncrease)
        {
            slider.value = value;
        }
        else
        {
            slider.value = -value;
        }
    }

    private void ChangeDirection()
    {
        toIncrease = !toIncrease;
    }
}
