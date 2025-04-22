using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using L05.Models;

namespace L05.Controllers;

public class HomeController : Controller
{
  public IActionResult Index()
  {
    ViewData["Title"] = "Home";
    return View();
  }

  [HttpPost]
  public IActionResult SetNickname([FromForm] UserDto dto)
  {
    HttpContext.Session.SetString("Nickname", dto.Nickname);
    return RedirectToAction("Index", "Thread");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  public record UserDto(string Nickname) { }
}
