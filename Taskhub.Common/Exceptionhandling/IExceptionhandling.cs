using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Models.utility;

namespace Taskhub.Common.Exceptionhandling
{
    public interface IExceptionhandling
    {
        Task<object> SendEmail(EmailRequestModel emailBody);

        //void LogWrite(string Base, string RequestId, string RequestPath, string Method, string InputData, string Result, string CreatedBy, string CreatedById, string dateTime);


    }
}
