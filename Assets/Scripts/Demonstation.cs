using Impl.Controller;
using Impl.View;
using Service.Database.Impl;
using Service.Database.Interface;
using Service.Device.Class;
using Service.Device.Enums;
using Service.Device.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Demonstation : MonoBehaviour
{
    [SerializeField] private Button createDeviceButton;
    [SerializeField] private Button removeDeviceButton;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject container;

    [SerializeField] private Devices deviceArray;
    private readonly IDatabase _database = new DatabaseService();

    private void Start()
    {
        CreateDevicesFromDatabase();
        createDeviceButton.onClick.AddListener(CreateDeviceButtonBehaviour);
        removeDeviceButton.onClick.AddListener(RemoveDeviceButtonBehaviour);
    }

    private void CreateDevicesFromDatabase()
    {
        LoadDataFromDatabase();
        foreach (var device in deviceArray.devices) 
            CreateDeviceVisualization(device);
    }

    private void RemoveDeviceButtonBehaviour()
    {
        if (deviceArray.devices.Count >= 1)
        {
            deviceArray.devices.RemoveAt(deviceArray.devices.Count-1);
            SaveDataToDatabase();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogWarning($"[{nameof(Demonstation)}] At least one devices must be");
        }
    }

    private void LoadDataFromDatabase()
    {
        deviceArray = _database.LoadData();
    }
    
    private void SaveDataToDatabase()
    {
        _database.SaveData(deviceArray);
    }

    private void CreateDeviceButtonBehaviour()
    {
        var device = CreateRandomDevice();
        deviceArray.devices.Add(device);
        SaveDataToDatabase();
        CreateDeviceVisualization(device);
    }

    private Device CreateRandomDevice()
    {
        return new Device()
        {
            type = (StateChanging) Random.Range(0,2),
            configuration = (CollisionConfiguration) Random.Range(0,3),
            state = new Vector3(Random.Range(0,10), Random.Range(0,10), Random.Range(0,10))
        };
    }

    private void CreateDeviceVisualization(Device device)
    {
        var deviceVisualization = Instantiate(prefab, device.state, Quaternion.identity);
        var deviceView = deviceVisualization.GetComponent<UnityDeviceView>();
        IDeviceControlSystem deviceController = 
            UnityDeviceController.CreateUnityDeviceControllerFromDevice(device, deviceView);
        CreateButton(deviceController);
    }

    private void CreateButton(IDeviceControlSystem deviceControlSystem)
    {
        var buttonInstance = Instantiate(button, container.transform);
        buttonInstance.GetComponent<Button>().onClick.AddListener(deviceControlSystem.PerformAction);
        buttonInstance.GetComponentInChildren<Text>().text = 
            "Perform Action #" + container.transform.childCount;
    }
}
