using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPlatLib.Contracts;
using XPlatLib.MVC.Contracts;

namespace XPlatXampLib.MVC
{
    public static class MVCMaster
    {
        private static Dictionary<Type, Type> _viewToControllerMap = new Dictionary<Type, Type>();
        private static Dictionary<Type, IController> _instantiatedControllers = new Dictionary<Type, IController>();

        private static ILogger _logger;
        private static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = IoC.Container.Resolve<ILogger>();
                }
                return _logger;
            }
        }

        /// <summary>
        /// Registers a controller that's waiting for a view to attach.
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="controllerType"></param>
        public static void RegisterForView(Type viewType, Type controllerType)
        {
            Logger.Info("MVCMaster.RegisterForView - " + viewType.FullName);
            if (_viewToControllerMap.Keys.Contains(viewType))
            {
                _viewToControllerMap.Remove(viewType);
            }
            _viewToControllerMap.Add(viewType, controllerType);
        }

        /// <summary>
        /// Called by a view to get its associated controller.
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public static IController GetController(Type viewType)
        {
            Logger.Info("MVCMaster.GetController - " + viewType.FullName);
            if (!_viewToControllerMap.Keys.Contains(viewType))
            {
                return null;
            }
            if (_instantiatedControllers.ContainsKey(_viewToControllerMap[viewType]))
            {
                return _instantiatedControllers[_viewToControllerMap[viewType]];
            }
            IController controller = IoC.Container.Resolve(_viewToControllerMap[viewType]) as IController;
            _instantiatedControllers.Add(_viewToControllerMap[viewType], controller);
            return controller;
        }

    }
}
