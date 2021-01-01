using LearningQA.Client.Pages;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace LearningQA.Server.Infrasructure
{
	public static class DataResourceReader
	{
        public static List<T> LoadJson<T>(string file = "TestItems.TestItem.json")
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            try
			{
                using (StreamReader r = new StreamReader(thisAssembly.GetManifestResourceStream($"LearningQA.Server.DataResource.{file}")))
                {
                    var json = r.ReadToEnd();
                    JsonSerializerOptions option = new JsonSerializerOptions();
                    option.IncludeFields = true;
                    option.PropertyNameCaseInsensitive = true;
                    var items = JsonSerializer.Deserialize<List<T>>(json,option).ToList();


                    return items;
                }
            }
            catch(Exception ex)
			{

			}
            return null;
        }
    }
}
