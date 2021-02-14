using LearningQA.Shared.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Entities
{
	public enum ContentType
	{
		Text = 0,
		ImageBase64String = 1,
		ImageFileName = 2
	}

	public class Supplement<Tdb> : ISupplement<Tdb>
	{
		public Tdb Id { get; set; }
		public string TenantId { get; set; }
		public string Content { get; set; }
		public string OriginalContent {get;set;}
		public ContentType OriginalcontentType { get; set; }
		public ContentType ContentType {get;set;}
	}
}
