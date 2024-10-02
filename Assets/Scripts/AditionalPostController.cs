using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AditionalPostController : MonoBehaviour
{
    public Button Yes => _yesBtn;
    public Button No => _noBtn;
    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;    
}
