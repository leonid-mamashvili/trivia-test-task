using System;
using System.Collections.Generic;
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
    public class QuestionsController : ControllerBase
    {
        private IRepositoryWrapper _triviaRepo;
        private IModelCreatorService _model;
        private ICategoryService _categoryService;

        public QuestionsController (
            IRepositoryWrapper repo,
            IModelCreatorService model,
            ICategoryService categoryService
            )
        {
            _triviaRepo = repo;
            _model = model;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("By_Category/{categoryId}")]
        public async Task<IActionResult> GetRandomQuestionByCategoryId (Guid categoryId)
        {
            var category = _triviaRepo.Category.FindByCondition(x => x.Id == categoryId).FirstOrDefault();

            if (category == null)
            {
                return NotFound($"Category with id: {categoryId} not found");
            }

            var question = await _categoryService.GetRandomQuestionFromCategoryAsync(category);

            return Ok(question);
        }


        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            var Questions = _triviaRepo.Question.FindAll().Include(c => c.Answers);
            return Ok(Questions);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var question = _triviaRepo.Question.FindByCondition(x => x.Id == id).Include(c => c.Answers).FirstOrDefault();

            if (question == null)
            {
                return NotFound($"Question with id: {id} does not exist");
            }

            return Ok(question);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DELETE(Guid id)
        {
            var question = _triviaRepo.Question.FindByCondition(x => x.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound();
            }

            _triviaRepo.Question.Delete(question);
            _triviaRepo.Save(); // TODO: make it on service
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PUT(Guid id, [FromBody] Question question)
        {
            var oldQuestion = _triviaRepo.Question.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (oldQuestion == null)
            {
                return NotFound($"Question with id: {id} does not exist");
            }
            else
            {
                question.Id = id;
                var updatedQuestion = await _model.CreateOrUpdateQuestionAsync(question);
                if (updatedQuestion != null)
                    return Ok(updatedQuestion);
                else return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> POST([FromBody] Question question)
        {
            var createdQuestion = await _model.CreateOrUpdateQuestionAsync(question);

            if (createdQuestion != null)
            {
                return Ok(createdQuestion);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
