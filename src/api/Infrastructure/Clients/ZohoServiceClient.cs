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

        protected readonly String teamId;

        protected readonly IServiceProvider svcProvider;

        protected readonly IConfiguration configuration;

        protected String path;
#pragma warning restore SA1401 // Fields must be private

        protected ZohoServiceClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
        {
            this.teamId = configuration.GetValue<String>("Zoho:TeamId");
            this.client = client;
            this.svcProvider = svcProvider;
            this.configuration = configuration;
        }

        protected IEnumerable<T> ConvertJsonResponseToClass<T>(JToken properties, JToken TaskItems)
        {
            var result = new List<T>();

            if (TaskItems != null)
            {
                #pragma warning disable S3217 // Either change the type to iterate on a generic collection of type
                foreach (JProperty TaskItem in TaskItems)
                {
                    var isNumber = long.TryParse(TaskItem.Name, out var temp);
                    if (!isNumber)
                    {
                        continue;
                    }

                    var newJObjFormat = new JObject();
                    var values = TaskItem.Value;
                    newJObjFormat.Add("id", TaskItem.Name);

                    foreach (JProperty property in properties)
                    {
                        var index = property.Value.ToObject<int>();
                        var propertyVal = values[index];
                        newJObjFormat.Add(property.Name, propertyVal);
                    }

                    var resultTaskItem = newJObjFormat.ToObject<T>();
                    result.Add(resultTaskItem);
                }
                #pragma warning restore S3217
            }

            return result;
        }
        protected IEnumerable<User> ConvertUserDisplay(JToken TaskItems)
        {
            var result = new List<User>();

            if (TaskItems != null)
            {
                #pragma warning disable S3217 // Either change the type to iterate on a generic collection of type
                foreach (JProperty TaskItem in TaskItems)
                {
                    var newJObjFormat = new JObject();
                    newJObjFormat.Add("id", TaskItem.Name);
                    newJObjFormat.Add("displayName", TaskItem.Value.ToObject<String>());

                    var resultTaskItem = newJObjFormat.ToObject<User>();
                    result.Add(resultTaskItem);
                }
                #pragma warning restore S3217
            }

            return result;
        }

        protected FormUrlEncodedContent SetAndEncodeParameter<T>(T parameter)
        {
            var type = parameter.GetType();
            var properties = type.GetProperties().Where(prop => prop.GetCustomAttribute(typeof(JsonPropertyAttribute)) != null);
            var urlParameter = new List<KeyValuePair<String, String>>();

            foreach (var property in properties)
            {
                var value = property.GetValue(parameter);
                var jsonAttr = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (value != null)
                {
                    var StringValue = value.ToString();
                    if (property.PropertyType.Equals(typeof(DateTime?)) ||
                        property.PropertyType.Equals(typeof(DateTime))) {
                        var date = (DateTime) value;
                        StringValue = date.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                    }
                    var keyValue = new KeyValuePair<String, String>(jsonAttr.PropertyName, StringValue);
                    urlParameter.Add(keyValue);
                }
            }

            return new FormUrlEncodedContent(urlParameter);
        }
    }
}