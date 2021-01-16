using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MongoDB.Bson.Serialization.Attributes;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	public class Person<Tdb> : IPerson<Tdb>
	{
		
		public string IdNumber { get; set; }
		public string Name { get ; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public Tdb? Id { get; set; }
		public virtual ICollection<Test<QUestionSql,Tdb>> Tests { get; set; } 
		public void SetNull()
		{
			IdNumber = null;
			Name = null;
			Email = null;
			Phone = null;
			Address = null;

		}
		public void Reset()
		{
			IdNumber = "";
			Name = "";
			Email = "";
			Phone = "";
			Address = "";
			
		}
	}
}
