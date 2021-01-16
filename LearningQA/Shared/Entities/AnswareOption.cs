using LearningQA.Shared.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Entities
{
	public class AnswareOption<Tdb> : IQuestionOption<Tdb>
	{
		public Tdb Id { get; set; } 
		public string TenantId { get; set; }
		public string Content { get; set; }
		public bool IsTrue { get; set; }
		public bool IsSelected { get; set; } = false;

	}
	public static class EntityExtensions
	{
		public static AnswareOption<Tdb> ToAnswareOption<Tdb>(this QuestionOption<Tdb> source)
		{
			var destination = Activator.CreateInstance<AnswareOption<Tdb>>();
			destination.Content = source.Content;
			destination.TenantId = source.TenantId;
			return destination;
		}
	}
	
}
