using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api.Application.Resolver
{
    public class LogWorkIgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly IEnumerable<String> ignoreProps;

        public LogWorkIgnorePropertiesResolver(IEnumerable<String> ignoreProps)
        {
            this.ignoreProps = ignoreProps;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            #pragma warning disable RCS1163 // Unused parameter
            property.ShouldSerialize = (x) => { return !ignoreProps.Contains(property.PropertyName); };
            #pragma warning restore RCS1163 // Unused parameter
            return property;
        }
    }
}