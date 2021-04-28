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
		Text = 1,
		ImageFileName = 2,
		ImageBase64String = 4,
		TextExplain = 65, // 64 + 2
		ImageFileNameExplain = 66, // 64 + 2
		ImageBase64StringExplain = 68, //64 + 4
	}

	public class Supplement<Tdb> : ISupplement<Tdb>
	{
		public Tdb Id { get; set; }
		public string Title { get; set; } = "";
		public string TenantId { get; set; } = "";
		/// <summary>
		/// The Conntent to present
		/// </summary>
		public string Content { get; set; } = "";
		/// <summary>
		/// Rotate content in Degree
		/// </summary>
		public int RotateContent { get; set; } = 0;
		/// <summary>
		/// The Original content
		/// </summary>
		public string OriginalContent { get; set; } = "";
		public ContentType OriginalcontentType { get; set; }
		/// <summary>
		/// Content type like text, image , image as string 
		/// </summary>
		public ContentType ContentType {get;set;}
	}
}
