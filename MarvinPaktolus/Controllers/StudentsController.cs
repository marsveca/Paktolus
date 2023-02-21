using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarvinPaktolus.Models;
using MarvinPaktolus.Dto;

namespace MarvinPaktolus.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MarvinVelasquezContext _context;

        public StudentsController(MarvinVelasquezContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.Include(x=>x.StudentHobbies).ThenInclude(x=>x.Hobbie).ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            StudentViewModel model = new StudentViewModel();
            var hobbies = _context.Hobbies.ToList();
            model.Hobbies = new List<HobbiesCheck>();
            foreach (var item in hobbies)
            {
                HobbiesCheck hob = new HobbiesCheck();
                hob.IsChecked = false;
                hob.Value = item.Id;
                hob.Text = item.Name;
                model.Hobbies.Add(hob);
            }
            return View(model);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel students)
        {
            Students student = new Students();
            if (ModelState.IsValid)
            {
                student.Email = students.Email;
                student.Name = students.Name;
                student.Phone = students.Phone;
                student.Zip = students.Zip.ToString();
                foreach (var hobbie in (students.Hobbies.Where(x => x.IsChecked == true).ToList()))
                {
                    StudentHobbies sh = new StudentHobbies();
                    
                    sh.HobbieId = hobbie.Value;
                    student.StudentHobbies.Add(sh);
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            

            var students = await _context.Students.FindAsync(id);
            

            StudentViewModel student = new StudentViewModel();
            student.Id = students.Id;
            student.Email = students.Email;
            student.Name = students.Name;
            student.Phone = students.Phone;
            student.Zip = int.Parse(students.Zip);

            List<int> sh = _context.StudentHobbies.Where(x=>x.StudentId==id).Select(x=>x.HobbieId).ToList();


            student.Hobbies = new List<HobbiesCheck>();
            var hobbies = _context.Hobbies.ToList();
            foreach (var item in hobbies)
            {
                HobbiesCheck hob = new HobbiesCheck();
                hob.IsChecked = sh.Contains(item.Id);
                hob.Value = item.Id;
                hob.Text = item.Name;
                student.Hobbies.Add(hob);
            }
            if (students == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  StudentViewModel students)
        {
            if (id != students.Id)
            {
                return NotFound();
            }
            Students student = new Students();
            if (ModelState.IsValid)
            {
                try
                {
                    student.Email = students.Email;
                    student.Name = students.Name;
                    student.Phone = students.Phone;
                    student.Zip = students.Zip.ToString();
                    student.Id = students.Id;

                    List<StudentHobbies> shd = _context.StudentHobbies.Where(x => x.StudentId == id).ToList();
                    _context.StudentHobbies.RemoveRange(shd);

                    foreach (var hobbie in (students.Hobbies.Where(x => x.IsChecked == true).ToList()))
                    {
                        StudentHobbies sh = new StudentHobbies();
                        sh.HobbieId = hobbie.Value;
                        student.StudentHobbies.Add(sh);
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.Id))
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
            return View(students);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _context.Students.FindAsync(id);
            List<StudentHobbies> shd = _context.StudentHobbies.Where(x => x.StudentId == id).ToList();
            _context.StudentHobbies.RemoveRange(shd);
            _context.Students.Remove(students);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
