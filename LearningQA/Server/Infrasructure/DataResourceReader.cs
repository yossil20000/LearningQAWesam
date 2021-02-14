﻿using LearningQA.Client.Pages;

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
            if(string.IsNullOrEmpty(file))
			{
                file = "TestItems.TestItem.json";
            }
			else
			{
                file = $@"TestItems\{file}"; 
			}
            var thisAssembly = Assembly.GetExecutingAssembly();
            try
			{
                string filename = $@"{System.IO.Directory.GetCurrentDirectory()}\DataResource\{file}";
                var json = System.IO.File.ReadAllText(filename);
                //using (StreamReader r = new StreamReader(thisAssembly.GetManifestResourceStream($"LearningQA.Server.DataResource.{file}")))
                {
                    //var json = r.ReadToEnd();
                    JsonSerializerOptions option = new JsonSerializerOptions();
                    option.IncludeFields = true;
                    option.PropertyNameCaseInsensitive = true;
                    var items = JsonSerializer.Deserialize<List<T>>(json,option).ToList();
                    return items;
                }
            }
            catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
			}
            return null;
        }

        public static  string LoadImageForDisplay(string file)
        {
            string filename = $@"{System.IO.Directory.GetCurrentDirectory()}\{file}";
            byte[] fileContent = null;
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
            long byteLength = new System.IO.FileInfo(filename).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            var x = Convert.ToBase64String(fileContent);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();
            return x;
        }
    }
}
