using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class IPControl : VisualElement
{
    private readonly TextField _colorTextField = new();
    private readonly FloatField _alphaTextField = new();
    private readonly Button _acceptButton = new();

    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            OnColorChange?.Invoke(value);
            _color = value;
        }
    }

    public UnityAction<Color> OnColorChange;
    
    public new class UxmlFactory : UxmlFactory<IPControl> {}
    
    public IPControl()
    {
        _colorTextField.maxLength = 6;
        _colorTextField.value = "FF0000";
        _alphaTextField.value = 0.5f;

        _acceptButton.text = "Accept";
        _acceptButton.clickable.clicked += AcceptButtonHandler;
        
        Add(new Label("Color"));
        Add(CreateTextFields(CreateContainer(), _colorTextField, _alphaTextField));
        Add(_acceptButton);
    }
    
    private void AcceptButtonHandler()
    {
        if (ColorUtility.TryParseHtmlString("#"+_colorTextField.value, out var color))
        {
            Color = color;
        }
        var tempColor = Color;
        tempColor.a = _alphaTextField.value;
        Color = tempColor;
    }

    private static VisualElement CreateContainer()
    {
        var container = new VisualElement();
        
        var flexDirection = container.style.flexDirection;
        var justifyContent = container.style.justifyContent;
        
        flexDirection.value = FlexDirection.Row;
        justifyContent.value = Justify.SpaceBetween;
        
        container.style.flexDirection = flexDirection;
        container.style.justifyContent = justifyContent;
        
        return container;
    }
    
    private static VisualElement CreateTextFields(VisualElement container, VisualElement rgb, VisualElement alpha)
    {        
        StyleLength width;

        width = Length.Percent(50);
        rgb.style.width = width;
        container.Add(rgb);
        
        width.value = Length.Percent(30);
        alpha.style.width = width;
        container.Add(alpha);

        var label = new Label
        {
            text = ":"
        };
        width.value = Length.Percent(5);
        label.style.width = width;
        
        var align = label.style.unityTextAlign;
        align.value = TextAnchor.MiddleCenter;
        label.style.unityTextAlign = align;
        container.Add(label);

        return container;
    }
}
