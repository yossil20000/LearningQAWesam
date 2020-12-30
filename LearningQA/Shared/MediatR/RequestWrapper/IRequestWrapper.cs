using MediatR;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.RequestWrapper
{
	public interface IRequestWrapper<T> : IRequest<Result<T>>
	{
	}
	public interface IHandlerWrapper<Tin,Tout> : IRequestHandler<Tin, Result<Tout>> where Tin : IRequestWrapper<Tout> { }
}
