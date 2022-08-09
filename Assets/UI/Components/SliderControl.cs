using UnityEditor.UIElements;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SliderControl : VisualElement
{
    private readonly Label _label;
    private readonly Slider _slider;
    private readonly FloatField _floatField;

    private float _value;
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _slider.value = value;
            _floatField.value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    private string _labelText;
    public string LabelText
    {
        get => _labelText;
        set
        {
            _labelText = value;
            _label.text = value;
        }
    }

    private float _min;
    public float Min
    {
        get => _min;
        set
        {
            _min = value;
            _slider.lowValue = value;
        }
    }

    private float _max;
    public float Max
    {
        get => _max;
        set
        {
            _max = value;
            _slider.highValue = value;
        }
    }

    public UnityAction<float> OnValueChanged;
    
    public new class UxmlFactory : UxmlFactory<SliderControl, UxmlTraits> {}
    
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private readonly UxmlFloatAttributeDescription _minValueAttribute = new()
        {
            name = "min-value",
            defaultValue = 0.0f,
        };
    
        private readonly UxmlFloatAttributeDescription _maxValueAttribute = new()
        {
            name = "max-value",
            defaultValue = 1.0f,
        };
    
        private readonly UxmlFloatAttributeDescription _progressAttribute = new()
        {
            name = "progress",
            defaultValue = 0.5f,
        };

        private readonly UxmlStringAttributeDescription _labelTextAttribute = new()
        {
            name = "label-text",
            defaultValue = "Slider"
        };
    
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            ((SliderControl)ve).Min = _minValueAttribute.GetValueFromBag(bag, cc);
            ((SliderControl)ve).Max = _maxValueAttribute.GetValueFromBag(bag, cc);
            ((SliderControl)ve).Value = _progressAttribute.GetValueFromBag(bag, cc);
            ((SliderControl)ve).LabelText = _labelTextAttribute.GetValueFromBag(bag, cc);
        }
    }

    public SliderControl()
    {
        _label = new Label();
        _slider = new Slider();
        _floatField = new FloatField();
    
        Add(_label);
        Add(_slider);
        Add(_floatField);
    
        _slider.RegisterValueChangedCallback(SliderValueChangeHandler);
        _floatField.RegisterValueChangedCallback(FloatFieldValueChangeHandler);
    }

    private void SliderValueChangeHandler(ChangeEvent<float> changeEvent)
    {
        Value = changeEvent.newValue;
    }
    
    private void FloatFieldValueChangeHandler(ChangeEvent<float> changeEvent)
    {
        if (changeEvent.newValue > Max)
        {
            Value = Max;
        } 
        else if (changeEvent.newValue < Min)
        {
            Value = Min;
        }
        else
        {
            Value = changeEvent.newValue;
        }
    }
}
