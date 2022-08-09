using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using Service.Device.Enums;
using UnityEngine;
using UnityEngine.TestTools;

namespace Service.Editor.Tests
{
    public class TestDeviceController : Device.Class.Device
    {
        public int CompleteOperationCounter;
        protected override async void Operation()
        {
            await Task.Delay(TimeSpan.FromSeconds(1f));
            CompleteOperationCounter++;
            EndOperation();
        }
    }
    
    public class DeviceTest
    {
        private TestDeviceController _device;

        [Test]
        public void When_ConfigurationExceptionAndDoubleActionPerformed_Expect_Exception()
        {
            _device = new TestDeviceController()
            {
                configuration = CollisionConfiguration.Exception
            };
            Assert.Throws<Exception>(() =>
            {
                _device.PerformAction();
                _device.PerformAction();
            });
        }
        [Test]
        public void When_ConfigurationAbortAndDoubleActionPerformed_Expect_ActionAbort()
        {
            _device = new TestDeviceController()
            {
                configuration = CollisionConfiguration.Abort
            };
            _device.OnOperationAbort += Assert.Pass;
            _device.PerformAction();
            _device.PerformAction();
        }
        [UnityTest]
        public IEnumerator When_ConfigurationAbortAndDoubleActionPerformed_Expect_ActionContinue()
        {
            _device = new TestDeviceController()
            {
                configuration = CollisionConfiguration.Continue
            };
            _device.PerformAction();
            _device.PerformAction();
            _device.PerformAction();
            yield return new WaitForSeconds(4f);
            Assert.AreEqual(_device.CompleteOperationCounter, 3);
        }
    }
}