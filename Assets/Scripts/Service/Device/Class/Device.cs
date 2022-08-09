using System;
using Service.Device.Enums;
using Service.Device.Interface;
using UnityEngine;

namespace Service.Device.Class
{
    [Serializable]
    public class Device : IDeviceControlSystem
    {
        public Vector3 state = Vector3.zero;
        public StateChanging type = StateChanging.Analog;
        public CollisionConfiguration configuration = CollisionConfiguration.Continue;

        private bool _isDeviceBusy;
        private int _repeatCount = 0;
        public Action OnOperationAbort;
        
        private void StartOperation()
        {
            _isDeviceBusy = true;
            Operation();
        }

        protected virtual void Operation() { }

        protected virtual void EndOperation()
        {
            _isDeviceBusy = false;
            if (configuration == CollisionConfiguration.Continue)
                CheckRepeat();
        }

        protected virtual void AbortOperation()
        {
            OnOperationAbort?.Invoke();
            EndOperation();
        }

        private void CheckRepeat()
        {
            if (_repeatCount == 0) return;
            _repeatCount--;
            StartOperation();
        }

        public void PerformAction()
        {
            if (_isDeviceBusy)
            {
                switch (configuration)
                {
                    case CollisionConfiguration.Exception : 
                        throw new Exception($"[{nameof(Device)}] Device busy!");
                    case CollisionConfiguration.Abort:
                        AbortOperation();
                        StartOperation();
                        break;
                    case CollisionConfiguration.Continue:
                        _repeatCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                StartOperation();
            }
        }
    }
}