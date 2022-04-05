using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;

namespace api.Infrastructure.Clients
{
    public abstract class ZohoServiceClient
    {
#pragma warning disable SA1401 // Fields must be private
        protected readonly HttpClient client;

        protected readonly string teamId;

        protected readonly IServiceProvider svcProvider;

        protected readonly IConfiguration configuration;

        protected string path;
#pragma warning restore SA1401 // Fields must be private

        protected ZohoServiceClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
        {
            this.teamId = configuration.GetValue<string>("Zoho:TeamId");
            this.client = client;
            this.svcProvider = svcProvider;
            this.configuration = configuration;
        }

        protected IEnumerable<T> ConvertJsonResponseToClass<T>(JToken? properties, JToken? items)
        {
            var result = new List<T>();

            if (items != null)
            {
                #pragma warning disable S3217 // Either change the type to iterate on a generic collection of type
                foreach (JProperty item in items)
                {
                    var isNumber = long.TryParse(item.Name, out var temp);
                    if (!isNumber)
                    {
                        continue;
                    }

                    var newJObjFormat = new JObject();
                    var values = item.Value;
                    newJObjFormat.Add("id", item.Name);

                    foreach (JProperty property in properties)
                    {
                        var index = property.Value.ToObject<int>();
                        var propertyVal = values[index];
                        newJObjFormat.Add(property.Name, propertyVal);
                    }

                    var resultItem = newJObjFormat.ToObject<T>();
                    result.Add(resultItem);
                }
                #pragma warning restore S3217
            }

            return result;
        }
        protected IEnumerable<User> ConvertUserDisplay(JToken items)
        {
            var result = new List<User>();

            if (items != null)
            {
                #pragma warning disable S3217 // Either change the type to iterate on a generic collection of type
                foreach (JProperty item in items)
                {
                    var newJObjFormat = new JObject();
                    newJObjFormat.Add("id", item.Name);
                    newJObjFormat.Add("displayName", item.Value.ToObject<string>());

                    var resultItem = newJObjFormat.ToObject<User>();
                    result.Add(resultItem);
                }
                #pragma warning restore S3217
            }

            return result;
        }

        protected FormUrlEncodedContent SetAndEncodeParameter<T>(T parameter)
        {
            var type = parameter.GetType();
            var properties = type.GetProperties().Where(prop => prop.GetCustomAttribute(typeof(JsonPropertyAttribute)) != null);
            var urlParameter = new List<KeyValuePair<string, string>>();

            foreach (var property in properties)
            {
                var value = property.GetValue(parameter);
                var jsonAttr = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (value != null)
                {
                    var stringValue = value.ToString();
                    if (property.PropertyType.Equals(typeof(DateTime?)) ||
                        property.PropertyType.Equals(typeof(DateTime))) {
                        var date = (DateTime) value;
                        stringValue = date.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                    }
                    var keyValue = new KeyValuePair<string, string>(jsonAttr.PropertyName, stringValue);
                    urlParameter.Add(keyValue);
                }
            }

            return new FormUrlEncodedContent(urlParameter);
        }
    }
}