using System;
using System.Text;
using Service.Database.Interface;
using UnityEngine;
using UnityEngine.Windows;

namespace Service.Database.Impl
{
    public class DatabaseService : IDatabase
    {
        private string _rawJsonString;
        
        public Devices LoadData()
        {
            if (File.Exists("Devices.json"))
            {
                _rawJsonString = Encoding.ASCII.GetString(File.ReadAllBytes("Devices.json"));
            }
            else
            {
                _rawJsonString = Resources.Load<TextAsset>("Devices").text;
            }
            return JsonUtility.FromJson<Devices>(_rawJsonString);
        }

        public void SaveData(Devices devicesArray)
        {
            if (devicesArray.devices.Count == 0) throw new Exception("Can't write empty array");
            _rawJsonString = JsonUtility.ToJson(devicesArray);
            File.WriteAllBytes("Devices.json", Encoding.ASCII.GetBytes(_rawJsonString));
        }
    }
}