using System;
using System.Collections.Generic;
using NUnit.Framework;
using Service.Database.Impl;
using Service.Database.Interface;

namespace Service.Editor.Tests
{
    public class DatabaseTest
    {
        private IDatabase _database;

        [SetUp]
        public void SetUp()
        {
            _database = new DatabaseService();
        }
        
        [Test]
        public void When_LoadDataFromNewDatabaseInstance_Expect_DataArray()
        {
            var devicesArray = _database.LoadData();
            Assert.Greater(devicesArray.devices.Count, 0);
        }
        
        [Test]
        public void When_WriteEmptyDataArrayToDatabaseInstance_Expect_Exception()
        {
            var devicesArray = new Devices
            {
                devices = new List<Device.Class.Device>()
            };
            Assert.Throws<Exception>(() =>
            {
                _database.SaveData(devicesArray);
            });
        }
    }
}