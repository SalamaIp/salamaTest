using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Quiz.Data;
using Quiz.Models;
using System.Drawing;
using Microsoft.AspNetCore.Cors;
using Quiz.ViewModels;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class QuizController : ControllerBase
    {
        private readonly QuizContext db;

        public QuizController(QuizContext context)
        {
            db = context;
        }


        [HttpGet("Test")]

        public IActionResult Get()
        {
            var D = db.Objects.ToList();

            return Ok(D);
        }

        [HttpPost("Result")]

        public IActionResult PostResult([FromBody] FinalResult dp)

        {
            var res = new FinalResult
            {
                score = dp.score,
                pass = dp.pass,

            };
            db.Add(res);
            db.SaveChanges();
            return Ok("Done");

        }



        [HttpGet("getResult")]

        public IActionResult GetRes()
        {
            var returnRes = db.FinalResults.ToList();
            return Ok(returnRes);
        }


        [HttpGet("QuizColorShape")]

        public IActionResult GetColor()
        {
            var result = from obj in db.Objects
                         join color in db.Colors on obj.ColorId equals color.Id
                         join shape in db.Shapes on obj.ShapeId equals shape.Id
                         select new { obj.Name, ColorSrc = color.ColorSrc, ShapeSrc = shape.ShapeSrc };

            foreach (var item in result)
            {
                Console.WriteLine($"Object Name: {item.Name}, Color Source: {item.ColorSrc}, Shape Source: {item.ShapeSrc}");
            }


            return Ok(result);
        }


        [HttpPost("PostAPI")]

        public IActionResult Post([FromBody] Objects dp)

        {
            //var color = db.Colors.Where(c => c.Id == 1).FirstOrDefault();
            //var Shape = db.Shapes.Where(s => s.Id == 2).FirstOrDefault();


            //var color = new Colors
            //{
            //    ColorName = dp.ColorName,
            //    ColorSrc = dp.ColorSrc
            //};

            //var shape = new Shapes
            //{
            //    ShapeName = dp.ShapeName,
            //    ShapeSrc = dp.ShapeSrc
            //};

            var obj = new Objects
            {
                Name = dp.Name,
                ColorId = dp.ColorId,
                ShapeId = dp.ShapeId
            };

            db.Add(obj);
            db.SaveChanges();
            return Ok(obj);
        }

        [HttpPost("PostShape/{name}")]
        public IActionResult PostShape(string name)
        {
            var shape = db.Shapes.FirstOrDefault(c => c.ShapeName == name);

            if (shape == null)
            {
                return NotFound("Shape not found");
            }

            return Ok(shape.Id);
        }


        [HttpPost("PostColor/{name}")]
        public IActionResult PostColor(string name)
        {
            var color = db.Colors.FirstOrDefault(c => c.ColorName == name);

            if (color == null)
            {
                return NotFound("Color not found");
            }

            return Ok(color.Id);
        }


        [HttpPost("update/{id}")]

        public IActionResult Update(int id, [FromBody] Objects dp)
        {
            var existingObject = db.Objects.FirstOrDefault(O => O.Id == id);

            if (existingObject == null)
            {
                return NotFound("Object not found");
            }
            else
            {
                existingObject.Name = dp.Name;
                existingObject.ColorId = dp.ColorId;
                existingObject.ShapeId = dp.ShapeId;
                db.SaveChanges();
                return Ok(existingObject);
            }
        }

        [HttpPost("delete/{id}")]

        public IActionResult Delete(int id)
        {
            var existingObject = db.Objects.FirstOrDefault(O => O.Id == id);

            if (existingObject == null)
            {
                return NotFound("Object not found");
            }
            else
            {
                db.Objects.Remove(existingObject);
                db.SaveChanges();
                return Ok("Object deleted successfully");
            }
        }
    }
}
