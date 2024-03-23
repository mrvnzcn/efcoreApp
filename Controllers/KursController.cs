using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace efcoreApp.Controllers
{
    
    public class KursController : Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.KursKayitlari.ToListAsync();
            return View(kurslar);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kurs model)
        {
            _context.Kurslar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            { 
                return NotFound(); 
            }

            var kurslar = await _context.KursKayitlari.FindAsync(id);
            // var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);

            if(kurslar == null)
            { 
                return NotFound(); 
            }
            
            return View(kurslar);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursKayit model)
        {
            if(id != model.KayitId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!_context.KursKayitlari.Any(o => o.KursId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {throw;}
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            { 
                return NotFound(); 
            }

            var kurslar = await _context.KursKayitlari.FindAsync(id);

            if(kurslar == null)
            { 
                return NotFound(); 
            }

            return View(kurslar);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var kurslar = await _context.KursKayitlari.FindAsync(id);
            if(kurslar == null) 
            { 
                return NotFound(); 
            }
            _context.KursKayitlari.Remove(kurslar);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}