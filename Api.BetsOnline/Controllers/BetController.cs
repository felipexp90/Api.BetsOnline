using Api.BetsOnline.Logic;
using Business.Interfaces.Base;
using Business.Interfaces.General;
using Entities;
using Entities.Enums;
using Entities.Utils;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.BetsOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IValidator<Bet> _entityValidator;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IGames _gameService;
        private readonly IBets _betsService;
        private readonly BetProcess _betProcess;

        public BetController(IValidator<Bet> entityValidator,
                             IResponseService responseService,
                             IExceptionHandler exceptionHandler,
                             IGames gameService,
                             IBets betsService)
        {
            _entityValidator = entityValidator;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _gameService = gameService;
            _betsService = betsService;
            _betProcess = new BetProcess(_responseService, _exceptionHandler, _gameService, _betsService);
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            Singleton.Instance.Audit = false;
            var response = await _betsService.List();
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Singleton.Instance.Audit = false;
            var response = await _betsService.Get(id);
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bet param)
        {
            var validation = _entityValidator.Validate(param);
            if (!validation.IsValid)
            {
                GetDetailInputsValidation(validation);
                return BadRequest(_responseService);
            }
            else
            {
                if (param != null)
                {
                    var response = await _betProcess.AddBet(GetObjectValidateByBet(param));
                    if (response.Meta.Status) return Ok(response);
                    ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                    return BadRequest(ModelState);
                }
                return BadRequest();
            }
        }

        [HttpPost("Close")]
        public async Task<IActionResult> Post(int idGame)
        {
            var response = await _betProcess.CloseBet(idGame);
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Bet param)
        {
            var validation = _entityValidator.Validate(param);
            if (!validation.IsValid)
            {
                GetDetailInputsValidation(validation);
                return BadRequest(_responseService);
            }
            else
            {
                if (param != null)
                {
                    var response = await _betProcess.UpdateBet(await GetObjectForUpdate(param));
                    if (response.Meta.Status) return Ok(response);
                    ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                    return BadRequest(ModelState);
                }
                return BadRequest();
            }
        }

        private void GetDetailInputsValidation(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
            {
                _responseService.Meta.Errors.Add(error.ErrorMessage);
            }
            _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
        }

        private async Task<Bet> GetObjectForUpdate(Bet param)
        {
            var getGame = await _betsService.Get(param.Id);
            object elementObject = GenericUtil.GetElementsResultService(getGame.Result);
            Bet response = (Bet)elementObject;
            response.Number = param.Number;
            response.Color = param.Color;
            response.MoneyBet = param.MoneyBet;
            return response;
        }

        private Bet GetObjectValidateByBet(Bet param)
        {
            if (param.Number == 0 && (param.Color == "rojo" || param.Color == "negro"))
                return param;
            else if (param.Number > 0 && string.IsNullOrEmpty(param.Color))
                return param;
            else
                throw new ArgumentException(MessagesEnum.InvalidColor);
        }
    }
}
