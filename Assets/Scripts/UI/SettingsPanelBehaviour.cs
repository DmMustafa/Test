using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPanelBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    
    private UIDocument _settingsPanelRoot;
    private Button _openButton;
    private Button _closeButton;
    private IStyle _containerStyle;
    private StyleLength _containerMarginValue;
    private ScrollView _scrollView;
    private VisualElement _toggleContainer;
    
    void Start()
    {
        _settingsPanelRoot = GetComponent<UIDocument>();
        _containerStyle = _settingsPanelRoot.rootVisualElement.
            Q<VisualElement>("SettingsPanelContainer").style;
        _containerMarginValue = _containerStyle.marginLeft;
        _scrollView = _settingsPanelRoot.rootVisualElement.Q<ScrollView>();
        _toggleContainer = _settingsPanelRoot.rootVisualElement.Q<VisualElement>("ToggleContainer");

        ButtonInitialize();
        ScrollViewSetup();
        SettingsPanelInitialize();
    }

    private void ScrollViewSetup()
    {
        _scrollView.elasticity = 0;
        _scrollView.scrollDecelerationRate = 0;
        _scrollView.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
    }

    private void SettingsPanelInitialize()
    {
        for (var i = 0; i < prefabs.Length; i++)
        {
            var container = new VisualElement();
            
            var innerContainer = new VisualElement
            {
                name = prefabs[i].name + i
            };
            
            var toggle = new Toggle
            {
                text = prefabs[i].name,
                value = false
            };

            GameObject objectInstance = null;

            var i1 = i;
            toggle.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue)
                {
                    objectInstance = Instantiate(prefabs[i1]);
                    objectInstance.name = prefabs[i1].name+i1;
                }
                else
                {
                    if (objectInstance != null)
                        Destroy(objectInstance);
                }
            });
            
            container.Add(toggle);
            container.Add(innerContainer);
            _toggleContainer.Add(container);
        }
    }
    
    private void ButtonInitialize()
    {
        _openButton = _settingsPanelRoot.rootVisualElement.Q<Button>("SettingsOpenButton");
        _openButton.clickable.clicked += OpenButtonBehaviour;
        
        _closeButton = _settingsPanelRoot.rootVisualElement.Q<Button>("SettingsCloseButton");
        _closeButton.clickable.clicked += CloseButtonBehaviour;
    }
    
    private void UpdateMargin()
    {
        _containerStyle.marginLeft = _containerMarginValue;  
    }

    private void OpenButtonBehaviour()
    {
        _containerMarginValue.value = Length.Percent(0);
        UpdateMargin();
    }

    private void CloseButtonBehaviour()
    {
        _containerMarginValue.value = Length.Percent(20);
        UpdateMargin();
    }
}
