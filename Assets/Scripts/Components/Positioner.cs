using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Positioner : MonoBehaviour
{
    private VisualElement _rootElement;
    private GameObject _gameObject;
    
    private SliderControl _sliderControl;
    private PoseControl _poseControl;
    
    private Vector3 _scale;

    private void Start()
    {
        _gameObject = gameObject;
        
        PoseSetup();
        SliderSetup();
        UiSetup();
    }

    private void UiSetup()
    {
        _rootElement = FindObjectOfType<UIDocument>().rootVisualElement.
            Q<VisualElement>(gameObject.name);
        _rootElement.Add(_sliderControl);
        _rootElement.Add(_poseControl);
    }

    private void PoseSetup()
    {
        _poseControl = new PoseControl();
        _poseControl.OnPositionChanged += PositionChangedHandler;
        _poseControl.OnRotationChanged += RotationChangedHandler;
    }

    private void PositionChangedHandler(Vector3 value)
    {
        gameObject.transform.position = value;
    }
    
    private void RotationChangedHandler(Vector3 value)
    {
        gameObject.transform.rotation = Quaternion.Euler(value);
    }
    
    private void SliderSetup()
    {
        _sliderControl = new SliderControl()
        {
            Max = 10,
            Min = 0,
            Value = 1,
            LabelText = "Scale"
        };
        _sliderControl.OnValueChanged += SliderChangedHandler;
    }

    private void SliderChangedHandler(float value)
    {
        _scale.x = value;
        _scale.y = value;
        _scale.z = value;
        gameObject.transform.localScale = _scale;
    }
    
    private void Update()
    {
        if (!transform.hasChanged) return;
        _poseControl.Position = _gameObject.transform.position;
        _poseControl.Rotation = _gameObject.transform.rotation.eulerAngles;
        _sliderControl.Value = _gameObject.transform.localScale.x;
        transform.hasChanged = false;
    }

    private void OnDestroy()
    {
        _rootElement.Remove(_sliderControl);
        _rootElement.Remove(_poseControl);
        
        _poseControl.OnPositionChanged = null;
        _poseControl.OnRotationChanged = null;
        _sliderControl.OnValueChanged = null;
    }
}
