using System.Dynamic;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace TRex.Metadata
{
    /// <summary>
    /// This class can be used as a base class for any operations that require
    /// a completely dynamic schema as either an input or return type. It can be
    /// used as a dynamic or implicitly converted to JToken at any time.
    /// </summary>
    public class DynamicModelBase : DynamicObject
    {
        JToken data;

        /// <summary>
        /// Initializes a new instance of the DynamicModelBase class
        /// </summary>
        public DynamicModelBase()
        {
            data = new JObject();
        }

        /// <summary>
        /// Initializes a new instance of the DynamicModelBase class based on a given object
        /// </summary>
        /// <param name="source">Source object to use for the dynamic model</param>
        public DynamicModelBase(object source)
        {
            data = JToken.FromObject(source);
        }
        
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var dObj = (data as JObject);

            if (null != dObj)
            {
                if (dObj.Properties() != null && dObj.Properties().Any())
                {
                    return dObj.Properties().Select(p => p.Name).Union(getPublicMemberNames());
                }
            }

            return base.GetDynamicMemberNames();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            if (binder == null) return false;
            
            try
            {
                if (getPublicMemberNames().Contains(binder.Name))
                {
                    PropertyInfo propInfo = null;
                    FieldInfo fieldInfo = null;

                    if (null != (propInfo = this.GetType().GetProperty(binder.Name)))
                    {
                        data[binder.Name] = JToken.FromObject(propInfo.GetValue(this));
                    }
                    else if (null != (fieldInfo = this.GetType().GetField(binder.Name)))
                    {
                        data[binder.Name] = JToken.FromObject(fieldInfo.GetValue(this));
                    }
                    else { }
                }

                result = data[binder.Name];
                return true;
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null) return false;

            try
            {
                if (getPublicMemberNames().Contains(binder.Name))
                {
                    PropertyInfo propInfo = null;
                    FieldInfo fieldInfo = null;

                    if (null != (propInfo = this.GetType().GetProperty(binder.Name)))
                    {
                        propInfo.SetValue(this, value);
                    }
                    else if (null != (fieldInfo = this.GetType().GetField(binder.Name)))
                    {
                        fieldInfo.SetValue(this, value);
                    }
                    else { }
                }

                data[binder.Name] = JToken.FromObject(value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IEnumerable<string> getPublicMemberNames()
        {
            return this.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Select(p => p.Name).Union(
                        this.GetType()
                            .GetFields(BindingFlags.Instance | BindingFlags.Public)
                            .Select(p => p.Name)).OrderBy(p => p);
        }

        public static JToken ToJToken(DynamicModelBase source)
        {
            return source;
        }

        public static DynamicModelBase FromJToken(JToken source)
        {
            return source;
        }

        public static implicit operator JToken(DynamicModelBase source)
        {
            if (source == null) return null;
            return source.data;
        }

        public static implicit operator DynamicModelBase(JToken source)
        {
            if (source == null) return null;
            return new DynamicModelBase() { data = source };
        }

    }
}
