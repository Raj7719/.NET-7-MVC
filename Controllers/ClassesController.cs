using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementApp.MVC.Data;
using SchoolManagementApp.MVC.Models;

namespace SchoolManagementApp.MVC.Controllers;
public class ClassesController : Controller
{
	private readonly SchoolManagementDbContext _context;

	public ClassesController(SchoolManagementDbContext context)
	{
		_context = context;
	}

	// GET: Classes
	public async Task<IActionResult> Index()
	{
		var schoolManagementDbContext = _context.Classes.Include(q => q.Courses).Include(q => q.Lecturer);
		return View(await schoolManagementDbContext.ToListAsync());
	}

	// GET: Classes/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var classes = await _context.Classes
			.Include(q => q.Courses)
			.Include(q => q.Lecturer)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (classes == null)
		{
			return NotFound();
		}

		return View(classes);
	}

	// GET: Classes/Create
	public IActionResult Create()
	{
		CreateSelectLists();
		return View();
	}

	// POST: Classes/Create
	// To protect from over-posting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([Bind("Id,LecturerId,CoursesId,Time")] Class classes)
	{
		if (ModelState.IsValid)
		{
			_context.Add(classes);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		CreateSelectLists();
		return View(classes);
	}

	// GET: Classes/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var classes = await _context.Classes.FindAsync(id);
		if (classes == null)
		{
			return NotFound();
		}
		CreateSelectLists();
		return View(classes);
	}

	// POST: Classes/Edit/5
	// To protect from over-posting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CoursesId,Time")] Class classes)
	{
		if (id != classes.Id)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(classes);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ClassExists(classes.Id))
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
		CreateSelectLists();
		return View(classes);
	}

	// GET: Classes/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var classes = await _context.Classes
			.Include(q => q.Courses)
			.Include(q => q.Lecturer)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (classes == null)
		{
			return NotFound();
		}

		return View(classes);
	}

	// POST: Classes/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var classes = await _context.Classes.FindAsync(id);
		if (classes != null)
		{
			_context.Classes.Remove(classes);
		}

		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<ActionResult> ManageEnrollments(int classId)
	{
		var @class = await _context.Classes
			.Include(q => q.Courses)
			.Include(q => q.Lecturer)
			.Include(q => q.Enrollments)
			.ThenInclude(q => q.Student)
			.FirstOrDefaultAsync(m => m.Id == classId);

		var students = await _context.Students.ToListAsync();

		var model = new ClassEnrollmentViewModel
		{
			Class = new ClassViewModel{
				Id = @class!.Id,
				CourseName = $"{@class.Courses!.Code} - {@class.Courses.Name}",
				LecturerName = $"{@class.Lecturer!.FirstName} {@class.Lecturer.LastName}",
				Time = @class.Time.ToString()
			}
		};

		foreach (var student in students)
		{
			model.Enrollment.Add(new StudentEnrollmentViewModel{
				Id = student.Id,
				FirstName = student.FirstName,
				LastName = student.LastName,
				IsEnrolled = (@class?.Enrollments?.Any(q => q.StudentId == student.Id)).GetValueOrDefault()
			});
		}

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> EnrollStudent(int classId,int studentId, bool shouldEnroll){
		var enrollment = new Enrollment();
		if (shouldEnroll)
		{
			enrollment.ClassId = classId;
			enrollment.StudentId = studentId;
			await _context.AddAsync(enrollment);
		}
		else
		{
			enrollment = await _context.Enrollments.FirstOrDefaultAsync(
				q => q.ClassId == classId && q.StudentId == studentId
			);

			if (enrollment != null)
			{
				_context.Remove(enrollment);
			}
		}

		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(ManageEnrollments), new{classId});
	}

	private bool ClassExists(int id)
	{
		return (_context.Classes.Any(e => e.Id == id));
	}

	private void CreateSelectLists()
	{
		var courses = _context.Courses.Select(q => new 
		{
			CourseName = $"{q.Code} - {q.Name} ({q.Credits} Credits)",
			q.Id
		});
		ViewData["CoursesId"] = new SelectList(courses, "Id", "CourseName");
		
		var lecturers = _context.Lecturers.Select(q => new 
		{
			FullName = $"{q.FirstName} {q.LastName}",
			q.Id
		});
		ViewData["LecturerId"] = new SelectList(lecturers, "Id", "FullName");
	}
}
