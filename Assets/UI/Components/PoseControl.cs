using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PoseControl : VisualElement
{
    private readonly Vector3Field _vector3FieldPosition = new();
    private readonly Vector3Field _vector3FieldRotation = new();

    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set
        {
            _vector3FieldPosition.value = value;
            _position = value;
            OnPositionChanged?.Invoke(value);
        }
    }

    public UnityAction<Vector3> OnPositionChanged;

    private Vector3 _rotation;
    public Vector3 Rotation
    {
        get => _rotation;
        set
        {
            _vector3FieldRotation.value = value;
            _rotation = value;
            OnRotationChanged?.Invoke(value);
        }
    }
    
    public UnityAction<Vector3> OnRotationChanged;
    
    public new class UxmlFactory : UxmlFactory<PoseControl> {}
    
    public PoseControl()
    {
        Add(new Label
        {
            text = "Transform"
        });
        Add(_vector3FieldPosition);
        
        Add(new Label
        {
            text = "Rotation"
        });
        Add(_vector3FieldRotation);

        _vector3FieldPosition.RegisterValueChangedCallback(PositionValueChangeHandler);
        _vector3FieldRotation.RegisterValueChangedCallback(RotationValueChangeHandler);
    }
    
    private void PositionValueChangeHandler(ChangeEvent<Vector3> changeEvent)
    {
        Position = changeEvent.newValue;
    }
    
    private void RotationValueChangeHandler(ChangeEvent<Vector3> changeEvent)
    {
        Rotation = changeEvent.newValue;
    }
}
