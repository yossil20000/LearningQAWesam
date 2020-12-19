using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
    public class Category<Tdb> : ICategory<Tdb>
    {
        public Tdb Id { get; set; }

        public string Name { get; set; }
		
	}
}
