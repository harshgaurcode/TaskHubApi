using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Net.Mail;
using System.Net;
using Taskhub.Models.utility;
using Microsoft.Extensions.Hosting.Internal;

namespace Taskhub.Common.Exceptionhandling
{
    public class Exceptionhandling:IExceptionhandling
    {
        private readonly IConfiguration _emailConfigurations;
        

        public Exceptionhandling(IConfiguration emailConfiguration)
        {
            _emailConfigurations = emailConfiguration;
         
        }
        public async Task<object> SendEmail(EmailRequestModel emailBody)
        {
            string from = _emailConfigurations.GetValue<string>("EmailSetting:FromEmail");
            string to = _emailConfigurations.GetValue<string>("EmailSetting:To");
            string cc = _emailConfigurations.GetValue<string>("EmailSetting:CC");
            string bcc = _emailConfigurations.GetValue<string>("EmailSetting:BCC");
            string _host = _emailConfigurations.GetValue<string>("EmailSetting:Host");
            int _port = _emailConfigurations.GetValue<int>("EmailSetting:Port");
            string _username = _emailConfigurations.GetValue<string>("EmailSetting:UserName");
            string _password = _emailConfigurations.GetValue<string>("EmailSetting:Password");



            try
            {
                //var name = fullname;
                using (MailMessage mm = new MailMessage(from, to))
                {

                    mm.Subject = "Exception occured";
                    mm.Body = emailBody.EmailBody;
                    if (!string.IsNullOrEmpty(cc) && cc.Split(",").Length == 0)
                    {
                        mm.CC.Add(new MailAddress(cc));
                    }
                    else if (!string.IsNullOrEmpty(cc) && cc.Split(",").Length > 0)
                    {
                        foreach (var item in cc.Split(","))
                            mm.CC.Add(new MailAddress(item));
                    }
                    if (!string.IsNullOrEmpty(bcc) && bcc.Split(",").Length == 0)
                    {
                        mm.Bcc.Add(new MailAddress(bcc));
                    }
                    else if (!string.IsNullOrEmpty(bcc) && bcc.Split(",").Length > 0)
                    {
                        foreach (var item in bcc.Split(","))
                            mm.Bcc.Add(new MailAddress(item));
                    }
                    #region
                    //if (!string.IsNullOrEmpty(attachmentFiles) && attachmentFiles.Split(",").Length == 0)
                    //{
                    //    Attachment data = new Attachment(attachmentFiles);
                    //    ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = System.IO.File.GetCreationTime(attachmentFiles);
                    //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachmentFiles);
                    //    disposition.ReadDate = System.IO.File.GetLastAccessTime(attachmentFiles);
                    //    disposition.FileName = Path.GetFileName(attachmentFiles);
                    //    disposition.Size = new FileInfo(attachmentFiles).Length;
                    //    disposition.DispositionType = DispositionTypeNames.Attachment;
                    //    mm.Attachments.Add(data);
                    //}
                    //else if (!string.IsNullOrEmpty(attachmentFiles) && attachmentFiles.Split(",").Length > 0)
                    //{
                    //    foreach (var item in attachmentFiles.Split(","))
                    //    {
                    //        Attachment data = new Attachment(item);
                    //        ContentDisposition disposition = data.ContentDisposition;
                    //        disposition.CreationDate = System.IO.File.GetCreationTime(item);
                    //        disposition.ModificationDate = System.IO.File.GetLastWriteTime(item);
                    //        disposition.ReadDate = System.IO.File.GetLastAccessTime(item);
                    //        disposition.FileName = Path.GetFileName(item);
                    //        disposition.Size = new FileInfo(item).Length;
                    //        disposition.DispositionType = DispositionTypeNames.Attachment;
                    //        mm.Attachments.Add(data);
                    //    }
                    //}
                    #endregion
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(_host, _port)
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_username, _password),
                        EnableSsl = true,
                    };
                    //smtp.UseDefaultCredentials = false;
                    //  NetworkCredential networkCred = new NetworkCredential(_username, _password);
                    // smtp.Credentials = networkCred;
                    //smtp.Port = 25;
                    //  smtp.Port = _port;
                    smtp.Send(mm);
                    return "Success";

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        //public async void LogWrite(string Base, string RequestId, string RequestPath, string Method, string InputData, string Result, string CreatedBy, string CreatedById, string dateTime)
        //{
        //    try
        //    {
        //        if (root != null)
        //        {
        //            ///string ip = new HttpContextAccessor().HttpContext.Connection.RemoteIpAddress.ToString();
        //            StringBuilder content = new StringBuilder();
        //            content.AppendLine("{");
        //            content.AppendLine("Base : " + Base + ",");
        //            content.AppendLine("RequestId : " + RequestId + ",");
        //            content.AppendLine("RequestPath : " + RequestPath + ",");
        //            content.AppendLine("Method : " + Method + ",");
        //            content.AppendLine("Input Data : " + InputData + ",");
        //            content.AppendLine("Result : " + Result + ",");
        //            content.AppendLine("CreatedBy : " + CreatedBy + ",");
        //            content.AppendLine("CreatedById : " + CreatedById + ",");
        //            content.AppendLine("DateTime : " + dateTime + ",");
        //            //content.AppendLine("IpAddress : " + ip + ",");
        //            content.AppendLine("},");
        //            var path = root.ContentRootPath + "/CustomLogs/" + DateTime.UtcNow.ToString("MMddyyyy") + "/";
        //            string filedate = DateTime.UtcNow.ToString("MMddyyyy-HH");
        //            var fileName = Path.Combine(path, "FlightLog-" + filedate + ".txt");
        //            if (!Directory.Exists(Path.Combine(path)))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        //            {
        //                StreamWriter sw = new StreamWriter(fs);
        //                sw.BaseStream.Seek(0, SeekOrigin.End);
        //                sw.WriteLine(content);
        //                sw.Flush();
        //                sw.Close();
        //                sw.Dispose();
        //                fs.Close();
        //                fs.Dispose();
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return;
        //    }
        //}
    }
}
