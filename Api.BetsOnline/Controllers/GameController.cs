using Business.Interfaces.General;
using Entities;
using Entities.Enums;
using Entities.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.BetsOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGames _service;
        public GameController(IGames service) => _service = service;

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            Singleton.Instance.Audit = false;
            var response = await _service.List();
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Singleton.Instance.Audit = false;
            var response = await _service.Get(id);
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Game param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _service.Add(param);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Game param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _service.Update(await GetObjectForUpdate(param));
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        private async Task<Game> GetObjectForUpdate(Game param)
        {
            var getGame = await _service.Get(param.Id);
            object elementObject = GenericUtil.GetElementsResultService(getGame.Result);
            Game response = (Game)elementObject;
            response.Enabled = param.Enabled;
            return response;
        }
    }
}
