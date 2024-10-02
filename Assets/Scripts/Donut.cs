using UnityEngine;

public enum Colors
{
    RED,
    GREEN, 
    BLUE,
    YELLOW,
    ORANGE,
    PURPLE,
    CYAN,
    PINK,
    LIGHT_PURPLE,

}
public class Donut : MonoBehaviour
{
    public bool IsColorHide => _hideColor;
    public Colors DonutColor => _color;
    [SerializeField] private Colors _color;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private bool _hideColor;

    private Color _materialColor;

    public void OpenColor()
    {
        _hideColor = false;
        UpdateColor();
    }

    private void Awake()
    {
        _materialColor = _renderer.material.color;
        UpdateColor();
    }

   

    private void UpdateColor()
    {
        if (_hideColor)
        {
            _renderer.material.color = Color.gray;
            return;
        }
        _renderer.material.color = _materialColor;

    }
}
