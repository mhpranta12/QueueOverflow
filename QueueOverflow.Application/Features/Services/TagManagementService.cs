using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
    public class TagManagementService : ITagManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public TagManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task CreateTagAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
