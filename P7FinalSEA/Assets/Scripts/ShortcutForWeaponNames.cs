using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShortcutForWeaponNames : MonoBehaviour
{
    public PlayerControls playerScript;
    public string[] weaponNames;
    public TMP_Text ownText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int textToDisplay = playerScript.actualScroll;

        ownText.text = weaponNames[textToDisplay];
    }
}
