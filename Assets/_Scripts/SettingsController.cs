using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    public Toggle tog;

    void Start()
    {
        if (PlayerPrefs.GetInt("show_gore") == 1)
        {
            tog.isOn = true;
        }
        else if (PlayerPrefs.GetInt("show_gore") == 0)
        {
            tog.isOn = false;
        }

    }

    public void ToggleGore()
    {
        if (!tog.isOn) { PlayerPrefs.SetInt("show_gore", 0); }
        else PlayerPrefs.SetInt("show_gore", 1);
        Debug.Log("Toggle state: " + PlayerPrefs.GetInt("show_gore"));
    }
}
