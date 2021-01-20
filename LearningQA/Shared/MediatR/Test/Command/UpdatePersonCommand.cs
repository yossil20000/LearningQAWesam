using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace LearningQA.Shared.MediatR.Test.Command
{
	public class UpdatePersonCommand : IRequestWrapper<PersonInfoModel>
	{
		public PersonInfoModel Person { get; private set; }

		public UpdatePersonCommand(PersonInfoModel person)
		{
			Person = person;
		}
	}

	public class UpdatePersonHandler : BaseDBContextHandler, IHandlerWrapper<UpdatePersonCommand, PersonInfoModel>
	{
		public UpdatePersonHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<PersonInfoModel>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request.Person.Id == 0)
				{
					return new ServiceResult.NotFoundResult<PersonInfoModel>("Person index should nt be zero");
				}
				else
				{
					var person = dbContext.Person.Find(request.Person.Id);
					person.IdNumber = request.Person.IdNumber;
					person.Name = request.Person.Name;
					person.Phone = request.Person.Phone;
					person.Email = request.Person.Email;
					person.Address = request.Person.Address;
					 dbContext.Person.Update(person);
					var result = await dbContext.SaveChangesAsync();
					return new SuccessResult<PersonInfoModel>(request.Person);
				}
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<PersonInfoModel>(ex.Message);
			}
		}
	}
}
