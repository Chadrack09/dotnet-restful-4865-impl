using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using warp_assessment.Data;
using warp_assessment.Models;

namespace warp_assessment.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserContext _context;
        private IConfiguration _config;
        public UsersController(UserContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Password,Country,FavoriteColour,BirthDay,CellPhoneNumber,Comments")] User user)
        {
            if (ModelState.IsValid)
            {
                int UserId = 1;
                string UserName = user.Name;
                
                _context.Add(user);
                await _context.SaveChangesAsync();

                /*string SenderEmail = "bbenolych@outlook.com";*/
                string SenderEmail =
                    _config.GetValue<string>("MailSettings:SenderEmail");
                string SenderPassword =
                    _config.GetValue<string>("MailSettings:SenderPassword");
                string RecipientEmail = 
                    _config.GetValue<string>("MailSettings:RecipientEmail");

                string subject = "New user added to databse";
                string emailBody = "<p>Hello Admin, <br />A new user named " +UserName+" :" +
                    " was added to the database.<br/>Please click the link to " +
                    $"<a href='https://localhost:5001/Users/Index/Details/{UserId}'> " +
                    "See details</a><br/><br/>With Regards</p>";

                MailMessage mailMessage = new MailMessage(
                                SenderEmail, RecipientEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                smtp.EnableSsl = true;
                smtp.Timeout = 100000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);

                
                smtp.Send(mailMessage);
                ViewBag.message = "Email sent";


                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
        
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email,Password,Country,FavoriteColour,BirthDay,CellPhoneNumber,Comments")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
