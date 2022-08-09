using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Impl.View;
using Service.Device.Class;
using Service.Device.Enums;
using UnityEngine;

namespace Impl.Controller
{
    public class UnityDeviceController : Device
    {
        private readonly UnityDeviceView _view;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _animation;

        public UnityDeviceController(UnityDeviceView view)
        {
            _view = view;
        }

        protected override void Operation()
        {
            _animation = _view.Object.transform.
                DOMove(state + Vector3.one, type == StateChanging.Analog ? 2f : 0f).
                SetEase(Ease.InOutCubic).
                OnComplete(StopAnimation);
        }

        private void StopAnimation()
        {
            state = _view.Object.transform.position;
            EndOperation();
        }

        protected override void AbortOperation()
        {
            _animation.Kill();
            state = _view.Object.transform.position;
            base.AbortOperation();
        }

        public static UnityDeviceController CreateUnityDeviceControllerFromDevice(Device device, UnityDeviceView view)
        {
            return new UnityDeviceController(view)
            {
                configuration = device.configuration,
                state = device.state,
                type = device.type
            };
        }
    }
}