using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Painter : MonoBehaviour
{
    private VisualElement _rootElement;
    
    private IPControl _ipControl;
    
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
        IPControlSetup();
        UiSetup();
    }

    private void IPControlSetup()
    {
        _ipControl = new IPControl();
        _ipControl.OnColorChange += ColorChangedHandler;
    }
    
    private void UiSetup()
    {
        _rootElement = FindObjectOfType<UIDocument>().rootVisualElement.
            Q<VisualElement>(gameObject.name);
        _rootElement.Add(_ipControl);
    }

    private void ColorChangedHandler(Color value)
    {
        _meshRenderer.material.SetColor("_Color", value);
    }

    private void OnDestroy()
    {
        _rootElement.Remove(_ipControl);
        
        _ipControl.OnColorChange = null;
    }
}
