﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Services;
using Project.Models.Auth;
using Project.Models.EntityModels;
using Project.Models.ViewModel;

namespace Project.Server.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/category")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Category is null.");
            }

            var addedCategory = await _categoryService.AddAsync(new Category { CategoryId = 0, Name = model.Name, Description = model.Description });

            return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.CategoryId }, addedCategory);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryViewModel model)
        {
            var category = new Category
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description
            };

            var updatedCategory = await _categoryService.UpdateAsync(category);

            return Ok(updatedCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(category);
        }


        [HttpGet("GetAll")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("GetAll")]
        [MapToApiVersion("2.0")]
        [ApiExplorerSettings(GroupName = "v2")]
        public async Task<IActionResult> GetAllV2()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
    }
}
