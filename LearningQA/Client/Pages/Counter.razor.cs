using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using LearningQA.Client;
using LearningQA.Client.Shared;
using LearningQA.Shared.Entities;
using LearningQA.Client.ViewModel;
using LearningQA.Client.Components;
using YLBlazor;

namespace LearningQA.Client.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
        private IEnumerable<Person<int>> People;
        protected override void OnInitialized()
        {
            People = new Person<int>[]{new Person<int>{Id = 1, Name = "Yosef", IdNumber = "05982888", Email = "yos@gmail"}, new Person<int>{Id = 2, Name = "Yosef2", IdNumber = "05982888-2", Email = "yos2@gmail"}};
        }

        private void IncrementCount()
        {
            currentCount++;
        }
    }
}