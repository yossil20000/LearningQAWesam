using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LearningQA.Client.Services
{
	public class TestItemClient
	{
		public HttpClient Client { get; }
		public TestItemClient(HttpClient httpClient)
		{
			Client = httpClient;
			Client.Timeout = new TimeSpan(0, 0, 30);
			Client.DefaultRequestHeaders.Clear();
		}
	}
}
