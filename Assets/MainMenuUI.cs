using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(() => Globals.Global.View.StartGame.Invoke());
    }

    
}
