using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace SendMail.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(Name = "FirstName", SupportsGet = false)]
        public string FirstName { get; set; }

        [BindProperty(Name = "LastName", SupportsGet = false)]
        public string LastName { get; set; }

        [BindProperty(Name = "MiddleName", SupportsGet = false)]
        public string MiddleName { get; set; }

        [BindProperty(Name = "NumberGroup", SupportsGet = false)]
        public string NumberGroup { get; set; }

        [BindProperty(Name = "Upload", SupportsGet = false)]
        public IFormFile Upload { get; set; }

        [BindProperty(Name = "Email", SupportsGet = false)]
        public string Email { get; set; }



        public void OnPost()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("f1lzz0@yandex.ru");
            mail.To.Add(new MailAddress(Email));
            mail.Subject = "Группа " + NumberGroup; //zagolovok
            mail.Body = "Студент " + FirstName + " " + LastName + " " + MiddleName; //textsoobcshenia
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.yandex.ru";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("f1lzz0@yandex.ru", "mlvpkfrqzyuimsqe");
            var file = Path.Combine("C:\\Users\\drop-\\OneDrive\\Рабочий стол\\filefote", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                Upload.CopyTo(fileStream);
            }
            using (ZipArchive archive = ZipFile.Open(Path.ChangeExtension(file, ".zip"), ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(file, Path.GetFileName(file));
            }
            string path = Path.ChangeExtension(file, ".zip");
            try
            {
                if (System.IO.File.Exists(path)) // Проверка есть ли файл на диске?!
                {
                    var SF = new Attachment(path);
                    mail.Attachments.Add(SF); // прикрепляем какой-нибудь файл.
                }
            }
            catch
            { // тут лови исключения
            }

            client.Send(mail);
            Response.Redirect("./Privacy");
        }
    }
}