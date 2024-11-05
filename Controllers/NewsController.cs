using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FgssrApi.Data;
using FgssrApi.Models;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FgssrApi.Dtos;

namespace FgssrApi.Controllers
{
    //[Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public NewsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        [Route("GetAllNews")]
        public async Task<IActionResult> GetAllNews()
        {

            var news = await _unitofwork.News.GetAllAsync();
            var nvm = news.Select(nn => new NewsDto
            {
                NewsDescription=nn.NewsDescription,
                NewsTitle= nn.NewsTitle,
                NId=nn.NewsId,
                NewsDate=nn.NewsDate,
                
            }).ToList();

            return Ok(nvm);
        }

        [HttpGet]
        [Route("GetNewsById/{id}")]
        public async Task<IActionResult> GetNewsById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            var nvm = new NewsDto()
            {
                NewsDate =news.NewsDate,
                NewsDescription=news.NewsDescription,
                NewsTitle=news.NewsTitle,
                NId=news.NewsId
            };

            return Ok(nvm);
        }

        [HttpDelete]
        [Route("DeleteNewsById/{id}")]
        public async Task<IActionResult> DeleteNews(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            _unitofwork.News.Delete(news);
            _unitofwork.SaveChanges();
            TempData["SuccessMessage"] = "تمت عمليه الحذف بنجاح";

            return Ok("Delete is successfull");
        }




        [HttpPost]
        [Route("CreateNewNews")]
        public async Task<IActionResult> CreateNews([FromBody] NewsDtoCU NVM)
        {

            var news =  await _unitofwork.News.CreateAsync(NVM);
            _unitofwork.SaveChanges();

            return CreatedAtAction(nameof(GetNewsById), new { id = news.NewsId }, NVM);
        }

      

        [HttpPut]
        [Route("UpdateNewsById/{id}")]
        public async Task<IActionResult> UpdateNews(int? id, [FromBody] NewsDtoCU nvm)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var news = await _unitofwork.News.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
           
            news.NewsDescription=nvm.NewsDescription;
            news.NewsTitle= nvm.NewsTitle;
            news.NewsDate = nvm.NewsDate;

            _unitofwork.SaveChanges();


            return Ok(news);
        }
    }
}
