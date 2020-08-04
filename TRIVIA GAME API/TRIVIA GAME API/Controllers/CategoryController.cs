using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRIVIA_GAME_API.Services;

namespace TRIVIA_GAME_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IRepositoryWrapper _triviaRepo;
        private IModelCreatorService _model;

        public CategoryController(
            IRepositoryWrapper repositoryWrapper,
            IModelCreatorService model
            )
        {
            _triviaRepo = repositoryWrapper;
            _model = model;
        }

        [HttpGet]
        //[Route("~/")]
        public IActionResult GetAllCategories ()
        {
            var categories = _triviaRepo.Category.FindAll().Include(c => c.Questions).ThenInclude(q => q.Answers);
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById (Guid id)
        {
            var category = _triviaRepo.Category.FindByCondition(x => x.Id == id).Include(c => c.Questions).ThenInclude(q => q.Answers).FirstOrDefault();
            
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DELETE(Guid id)
        {
            var category = _triviaRepo.Category.FindByCondition(x => x.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            _triviaRepo.Category.Delete(category);
            _triviaRepo.Save(); // TODO: make it on service
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PUT(Guid id, [FromBody] Category category)
        {
            var oldCagegory = _triviaRepo.Category.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (oldCagegory == null)
            {
                return NotFound($"Category with id: {id} does not exist");
            }
            else
            {
                category.Id = id;
                var updatedCategory = await _model.CreateOrUpdateCategoryAsync(category);
                if (updatedCategory != null)
                    return Ok(updatedCategory);
                else return BadRequest();
            }
        }
        
        
        [HttpPost]
        public async Task<IActionResult> POST([FromBody] Category category)
        {
            var updatedCategory = await _model.CreateOrUpdateCategoryAsync(category);

            if (updatedCategory != null)
            {
                return Ok(updatedCategory);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
