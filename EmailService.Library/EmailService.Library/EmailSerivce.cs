using System.ComponentModel;
using System.Data;
using System.Net.Mail;
using System.Net;
using DatabaseManager;
using System.Collections;

namespace EmailService.Library
{
    public class EmailSerivce: IEmailSerivce
    {
        MiscDataSetting _miscDataSetting = new MiscDataSetting();
        static bool mailSent = false;
        //public async Task QueueEmail(EmailConfiguration emailConfiguration,string toEmailIds, string subject, string body, string token, int isHTML = 1)
        public async Task QueueEmail(EmailConfiguration emailConfiguration, EmailDetails emailDetails)
        {
            emailDetails.body = emailDetails.isHTML == 1 ? WebUtility.HtmlEncode(emailDetails.body) : emailDetails.body;
            string str = $"INSERT INTO EmailNotification(Token,ToEmailIds,EmailSubject,EmailBody,HasAttachment,IsSent,CreatedDate,Status,IsHTML) VALUES('{emailDetails.token}','{emailDetails.ToEmailIds}','{emailDetails.subject}','{emailDetails.body}',0,0,GETDATE(),'A',{emailDetails.isHTML})";
            await _miscDataSetting.ExecuteNonQuery(str);
            await ClearEmailQueue(emailConfiguration);
        }
        public async Task ClearEmailQueue(EmailConfiguration emailConfiguration)
        {
            string? toEmailIds, subject, body, token = "";
            Boolean isHTML = false;
            string str = "Select ToEmailIds,EmailSubject,EmailBody,Token,IsHTML FROM EmailNotification WHERE IsSent = 0 AND Status='A' AND Len(Isnull(ToEmailIds,'')) > 0";
            DataSet ds = await _miscDataSetting.GetDataSet(str);
            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    toEmailIds = row["ToEmailIds"].ToString();
                    subject = row["EmailSubject"].ToString();
                    body = row["EmailBody"].ToString();
                    token = row["Token"].ToString();
                    isHTML = Convert.ToBoolean(row["IsHTML"].ToString());
                    body = isHTML ? WebUtility.HtmlDecode(body) : body;
                     await SendEmail(emailConfiguration,toEmailIds, subject, body, token);
                }
            }
        }
        public async Task SendEmail(EmailConfiguration emailConfiguration, string toEmailIds, string subject, string body, string token = "")
        {
            //if (UMSResources.configuration.GetSection("ByPass:sendMail").Value == "Y")
            if (emailConfiguration.ByPass)
            {
                //string smtpServerAddress = emailConfiguration.SmtpServerAddress;
                //int smtpServerPort = emailConfiguration.SmtpServerPort;
                //string smtpServerUserId = emailConfiguration.SmtpServerUserId;
                //string smtpServerPassword = emailConfiguration.SmtpServerPassword;
                //string emailFromAddress = emailConfiguration.EmailFromAddress;
                //string displayName = emailConfiguration.DisplayName;
                try
                {
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(emailConfiguration.EmailFromAddress, emailConfiguration.DisplayName)
                    };
                    if (toEmailIds.Contains(";"))
                    {
                        foreach (string item in toEmailIds.Split(';'))
                        {
                            mail.To.Add(item);
                        }
                    }
                    else
                    {
                        mail.To.Add(toEmailIds);
                    }

                    mail.Subject = $"{subject} for Date:{DateTime.Now.ToShortDateString()}";
                    //mail.Body = $"Hi <br>This is just a Test Email from .net.<br>No further action required.<br>{Environment.NewLine}<br>gNxt Systems, Thanks";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(emailConfiguration.SmtpServerAddress, emailConfiguration.SmtpServerPort);
                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailConfiguration.SmtpServerUserId, emailConfiguration.SmtpServerPassword);
                    string userState = token;
                    smtp.SendAsync(mail, token);
                    //Console.WriteLine($"\tEmail notification process finished at {DateTime.UtcNow}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;
            string status = "";
            int isSent = 0;
            if (e.Cancelled)
            {
                isSent = 1;
                status = "C";
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                isSent = 1;
                status = "F";
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                isSent = 1;
                status = "S";
                Console.WriteLine("Message sent.");
            }
            string str = $"UPDATE EmailNotification SET IsSent = {isSent},SentDate=GETUTCDATE(),Status='{status}' WHERE Token = '{token}'";
            MiscDataSetting miscDataSetting = new MiscDataSetting();
            miscDataSetting.ExecuteNonQueryNonAsync(str);
            mailSent = true;
        }

        //public async Task<EmailTemplate> GetEmailTemplate(EmailType emailType)
        //{
        //    try
        //    {
        //        ArrayList arrList = new ArrayList();
        //        DAL.spArgumentsCollection(arrList, "@MobileOrEmail", req.MobileOrEmail, "VARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
        //        DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");

        //        var res = DAL.RunStoredProcedureRetError("sp_SetOTP", arrList);


                
        //    }
        //    catch (Exception ex) 
        //    { 
            

        //    }
            
        //}
    }
}
