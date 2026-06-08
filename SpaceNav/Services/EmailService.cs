using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SpaceNav.Services;

/// <summary>
/// This class defines the emailing service, which is used to send the mission report to mission control
/// via email.
/// </summary>
public class EmailService
{
    private static readonly IConfiguration _config;

    static EmailService()
    {
        _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json",optional: false, reloadOnChange: true)
            .Build();
    }
    /// <summary>
    /// Sends the report via email
    /// </summary>
    /// <param name="fromEmail"> Email of sender</param>
    /// <param name="senderPassword"> Password of sender</param>
    /// <param name="toEmail"> Email of recipient</param>
    /// <param name="reportBody"> The mission report to send</param>
    public static void SendMailReport(
        string fromEmail,
        string senderPassword,
        string toEmail,
        string reportBody)
    {
        string smtpHost = _config["SmtpSettings:Host"] ?? "smtp.gmail.com";
        int smtpPort = int.Parse(_config["SmtpSettings:Port"] ?? "587");
        
        using MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(new MailAddress(toEmail));
        mail.Subject = $"SpaceNav reporting to mission control at - {DateTime.Now:yyyy-MM-dd HH-mm} -";
        mail.Body = reportBody;
        
        using SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
        smtpClient.Credentials = new NetworkCredential(fromEmail, senderPassword );
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 10000;
        smtpClient.Send(mail);
    }
}