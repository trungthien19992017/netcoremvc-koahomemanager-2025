using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KOAHome.Controllers
{
  public class DichVuController : Controller
  {
    // GET: DichVuController
    public ActionResult Index()
    {
      return View();
    }

    // GET: DichVuController/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: DichVuController/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: DichVuController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: DichVuController/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: DichVuController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: DichVuController/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: DichVuController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }
  }
}
