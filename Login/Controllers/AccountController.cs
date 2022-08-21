using Login.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Login.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private string code = null;
        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue) 
            {
                return Redirect("/Home/Index");
            }
            return View();
        }
        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }
        public IActionResult ResetPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }
        public IActionResult SendCode(string email)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Email.Equals(email));
            if (user != null)
            {
                _dataContext.Add(new PasswordCode { UserId = user.Id, Code = getCode() });
                _dataContext.SaveChanges();
                
                //string text = "<h1>Sıfırlama kodunuz: </h1>" + getCode() + " ";
                //string subject = "Parola Sıfırlama Kodu";
                //MailMessage mailMessage = new MailMessage("wwww@wwwww.com", email, subject, text);
                //mailMessage.IsBodyHtml = true;
                //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                //smtpClient.UseDefaultCredentials = false;
                //NetworkCredential networkCredential = new NetworkCredential("wwww@wwww.com", "wwwww");
                //smtpClient.Credentials = networkCredential;
                //smtpClient.EnableSsl = true;
                //smtpClient.Send(mailMessage);
                return Redirect("ResetPassword");

            }
            return Redirect("Index");
        }
        public IActionResult ResetPass(string Code, string NewPassword)
        {
            var passwordcode = _dataContext.PasswordCodes.FirstOrDefault(x => x.Code.Equals(Code));
            if (passwordcode != null)
            {
                var user = _dataContext.Users.Find(passwordcode.UserId);
                user.Password = NewPassword;
                _dataContext.Update(user);
                _dataContext.Remove(passwordcode);
                _dataContext.SaveChanges();
                return Redirect("Index");
            }
            return Redirect("Index");

        }

        public IActionResult Login(string email, string pass)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(pass));
            if (user != null)
            {
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("fullname", user.Name + " " + user.Surname);
                return Redirect("/Home/Index");

            }
            return Redirect("/Account/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Account/Index");
        }

        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public async Task<IActionResult> Register(User usr)
        {
            await _dataContext.AddAsync(usr);
            await _dataContext.SaveChangesAsync();
            return Redirect("Index");
        }

        public string getCode() 
        {
            if (code == null)
            {
                Random random = new Random();
                code = "";
                for (int i = 0; i < 6; i++)
                {
                    char tmp = Convert.ToChar(random.Next(48, 58));
                    code += tmp;
                }
            }
            return code;
        }
    }
}
