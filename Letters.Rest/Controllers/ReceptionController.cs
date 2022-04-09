﻿using Letters.Core.Models;
using Letters.Rest.Extensions;
using Letters.Rest.Input;
using Letters.Service.Exceptions;
using Letters.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Letters.Rest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ICaptchaService _captchaService;

        public ReceptionController(IFormService formService, ICaptchaService captchaService)
        {
            _formService = formService;
            _captchaService = captchaService;
        }

        /// <summary>
        /// Метод получения формы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FormModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Form([FromQuery] FormInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var form = await _formService.GetForm(model.ReceptionId);

            return Ok(form);
        }

        /// <summary>
        /// Метод создания обращения
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Letter([FromForm] LetterInputModel model)
        {
            var bytes = await model.File.GetBytes();

            await _formService.AddLetters(model.Text, model.Email, model.Address, model.Recipient, model.Phone,
                                          model.SocialStatus, model.FirstName, model.LastName, model.MiddleName, bytes);

            return Ok();
        }

        /// <summary>
        /// Метод обновления капчи
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CaptchaModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCaptcha([FromQuery] UpdateCaptchaInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var captcha = _captchaService.UpdateCaptcha(model.Key);

                return Ok(captcha);
            }
            catch (OwnException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> test()
        {
            var captcha = _captchaService.Test();

            return File(captcha, "image/png");
        }
    }
}