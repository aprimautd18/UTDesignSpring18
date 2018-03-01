using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ImprovedSchedulingSystemApi.Models.CustomModelBinders
{
    public class ObjectIdModelBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (values.Length == 0)
            {
                return Task.CompletedTask;
            }

            ObjectId id;
            if (ObjectId.TryParse(values.FirstValue, out id))
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, id, values.FirstValue); //Update the given attempt parse to the newly parsed item
                bindingContext.Result = ModelBindingResult.Success(id);
            }
            else
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, ObjectId.Empty, values.FirstValue); //Update the given attempt parse to the newly parsed item
                bindingContext.Result = ModelBindingResult.Success(ObjectId.Empty);
            }
            return Task.CompletedTask;
        }
    }

    public class ObjectIdBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(ObjectId) ? new ObjectIdModelBinder() : null; // If the item is an ObjectId type run the model binder on it, otherwise do nothing
        }
    }


    public class ObjectIDJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = reader.Value?.ToString();
            ObjectId id;
            if (ObjectId.TryParse(stringValue, out id))
            {
                return id;
            }

            return ObjectId.Empty;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }


}
