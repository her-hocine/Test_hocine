using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database_First.Models;

namespace Database_First.Controllers
{
    public class EmpruntersController : Controller
    {
        private readonly bdgplccContext _context;
          
        public EmpruntersController(bdgplccContext context)
        {
            _context = context;
        }

        // GET: Emprunters
        public async Task<IActionResult> Index()
        {
            var bdgplccContext = _context.Emprunters.Include(e => e.IdLNavigation).Include(e => e.IdMNavigation);
            return View(await bdgplccContext.ToListAsync());
        }
           
        // GET: Emprunters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunter = await _context.Emprunters
                .Include(e => e.IdLNavigation)
                .Include(e => e.IdMNavigation)
                .FirstOrDefaultAsync(m => m.IdE == id);
            if (emprunter == null)
            {
                return NotFound();
            }

            return View(emprunter);
        }

        // GET: Emprunters/Create
        public IActionResult Create()
        {
            ViewData["IdL"] = new SelectList(_context.Livres, "IdL", "IdL");
            ViewData["IdM"] = new SelectList(_context.Membres, "IdM", "IdM");
            return View();
        }

        // POST: Emprunters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdE,IdM,IdL")] Emprunter emprunter)
        {
            if (ModelState.IsValid)
            {

                int nbEmprunts = _context.Emprunters.Where(p => p.IdM  == emprunter.IdM  && p.DateRetour == null).Count();
                if (nbEmprunts >= 3)
                {
                    ViewBag.Message = "Vous avez atteint votre limite d'emprunts! Veuillez retourner les livres avant d'effectuer un autre emprunt";
                    return View("Erreur");
                }

                DateTime DateEmp = DateTime.Today;
                emprunter.DateEmp = DateEmp; 

                _context.Add(emprunter);



                var livre = _context.Find<Livre>(emprunter.IdL); 
                if (livre.NbExemlpaireTotal > 0 )
                {
                    livre.NbExemlpaireTotal--;
                   _context.Update<Livre>(livre);
                } else
                {
                    ViewBag.Message = "Ce livre n'est pas disponible pour le moment. *Merci pour votre compréhension* ";
                    return View("Erreur");
                }
                

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdL"] = new SelectList(_context.Livres, "IdL", "IdL", emprunter.IdL); 
            ViewData["IdM"] = new SelectList(_context.Membres, "IdM", "IdM", emprunter.IdM);
            return View(emprunter);
        }

        // GET: Emprunters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunter = await _context.Emprunters.FindAsync(id);
            if (emprunter == null)
            {
                return NotFound();
            }
            ViewData["IdL"] = new SelectList(_context.Livres, "IdL", "IdL", emprunter.IdL);
            ViewData["IdM"] = new SelectList(_context.Membres, "IdM", "IdM", emprunter.IdM);
            return View(emprunter);
        }

        // POST: Emprunters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdE,IdM,IdL,DateEmp,DateRetour")] Emprunter emprunter)
        {
            if (id != emprunter.IdE)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprunter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprunterExists(emprunter.IdE))
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
            ViewData["IdL"] = new SelectList(_context.Livres, "IdL", "IdL", emprunter.IdL);
            ViewData["IdM"] = new SelectList(_context.Membres, "IdM", "IdM", emprunter.IdM);
            return View(emprunter);
        }

        // GET: Emprunters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunter = await _context.Emprunters
                .Include(e => e.IdLNavigation)
                .Include(e => e.IdMNavigation)
                .FirstOrDefaultAsync(m => m.IdE == id);
            if (emprunter == null)
            {
                return NotFound();
            }

            return View(emprunter);
        }

        // POST: Emprunters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprunter = await _context.Emprunters.FindAsync(id);
            _context.Emprunters.Remove(emprunter);


            var livre = _context.Find<Livre>(emprunter.IdL);

            if(emprunter.DateRetour == null)
            {
                livre.NbExemlpaireTotal++;
                _context.Update<Livre>(livre);
            }

                      

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> EffectuerRetour(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var emprunter = await _context.Emprunters
                .Include(p => p.IdLNavigation)
                .Include(p => p.IdMNavigation)
                .FirstOrDefaultAsync(m => m.IdE  == id);


             DateTime dateRetour = DateTime.Today;

            TimeSpan  nbJours  = (TimeSpan)(dateRetour - emprunter.DateEmp);

             //var nbJours = 3; 

            //int nbJours = (int)(dateRetour - emprunter.DateEmp).Days; 


            if (nbJours.Days > 7)
            {

                ViewBag.Message = "Vous avez dépassé la période indiquée (7 jours), veuillez contacter l'administration ";
                return View("Erreur");
            }
            emprunter.DateRetour = dateRetour;
            _context.Update<Emprunter>(emprunter);


            var livre = _context.Find<Livre>(emprunter.IdL);
            livre.NbExemlpaireTotal++; 
            _context.Update<Livre>(livre);


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }










        private bool EmprunterExists(int id)
        {
            return _context.Emprunters.Any(e => e.IdE == id);
        }
    }
}
