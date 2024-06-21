using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Utilities
{
    public interface IEmailService
    {
        void SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body);
    }
}
