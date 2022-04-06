using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;

namespace MyLab.WebErrors
{
    class ErrorToResponseMap : Collection<ErrorToResponseBinding>
    {
        public static ErrorToResponseMap LoadFromMethod(MethodInfo actionMethod)
        {
            var map = new ErrorToResponseMap();

            var bindingAttrs = actionMethod.GetCustomAttributes<ErrorToResponseAttribute>();

            foreach (var bindingAttr in bindingAttrs)
            {
                map.Add(new ErrorToResponseBinding(bindingAttr));
            }

            return map;
        }

        public static bool GetFromActionProperties(IDictionary<object, object> actionProperties, out ErrorToResponseMap map)
        {
            var factor = actionProperties.TryGetValue(typeof(ErrorToResponseMap), out var mapObject);
            map = mapObject as ErrorToResponseMap;

            return factor;
        }

        public static bool IsActionPropertiesContainsMap(IDictionary<object, object> actionProperties)
        {
            return actionProperties.ContainsKey(typeof(ErrorToResponseMap));
        }

        public void AddToActionProperties(IDictionary<object, object> actionProperties)
        {
            actionProperties.Add(typeof(ErrorToResponseMap), this);
        }

        public bool TryGetBinding(Type exceptionType, out ErrorToResponseBinding binding)
        {
            binding = this.FirstOrDefault(b => b.ExceptionType == exceptionType);
            return binding != null;
        }
    }

    class ErrorToResponseBinding
    {
        public Type ExceptionType { get; }

        public HttpStatusCode ResponseCode { get; }

        public string Message { get; }

        public ErrorToResponseBinding()
        {
            
        }

        public ErrorToResponseBinding(ErrorToResponseAttribute bindingAttribute)
        {
            ExceptionType = bindingAttribute.ExceptionType;
            Message = bindingAttribute.Message;
            ResponseCode = bindingAttribute.ResponseCode;
        }
    }
}