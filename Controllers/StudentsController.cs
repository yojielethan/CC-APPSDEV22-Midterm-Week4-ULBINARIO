using Microsoft.AspNetCore.Mvc;
using StudentRosterApi.Models;

namespace StudentRosterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    // Simulated in-memory database
    private static List<Student> _students = new List<Student>
    {
        new Student { Id = 1, FirstName = "Juan", LastName = "Dela Cruz", Course = "BSCS", YearLevel = 3 },
        new Student { Id = 2, FirstName = "Maria", LastName = "Clara", Course = "BSIT", YearLevel = 2 }
    };

    // GET: api/students
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        return Ok(_students);
    }

    // GET: api/students/5
    [HttpGet("{id}")]
    public IActionResult GetStudentById(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound(new { message = $"Student with ID {id} not found." });
        }
        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public IActionResult CreateStudent([FromBody] Student newStudent)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(newStudent.FirstName) || string.IsNullOrWhiteSpace(newStudent.LastName))
        {
            return BadRequest(new { message = "First name and last name are required." });
        }

        // Auto-assign ID
        newStudent.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
        _students.Add(newStudent);

        // Returns 201 Created and includes the URI to the new resource
        return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
    }

    // PUT: api/students/5
    [HttpPut("{id}")]
    public IActionResult UpdateStudent(int id, [FromBody] Student updatedStudent)
    {
        if (id != updatedStudent.Id)
        {
            return BadRequest(new { message = "ID mismatch." });
        }

        var existingStudent = _students.FirstOrDefault(s => s.Id == id);
        if (existingStudent == null)
        {
            return NotFound();
        }

        // Update properties
        existingStudent.FirstName = updatedStudent.FirstName;
        existingStudent.LastName = updatedStudent.LastName;
        existingStudent.Course = updatedStudent.Course;
        existingStudent.YearLevel = updatedStudent.YearLevel;
        existingStudent.Email = updatedStudent.Email;

        return NoContent(); // 204 No Content
    }

    // DELETE: api/students/5
    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        _students.Remove(student);
        return NoContent();
    }
}