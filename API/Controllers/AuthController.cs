using Business.Abstract;
using Core.Entities.DTOs;
using Core.Utilities.CodeGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            return Json(_authService.Login(userForLoginDto));
        }

        [Authorize]
        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {            
            return Json(_authService.Register(userForRegisterDto));
        }

        [Authorize]
        [HttpPost("code-generator")]
        public IActionResult CodeGenerator(string tableName)
        {
            DB_Model dB_Model = new DB_Model();
            dB_Model.TableName = tableName;
            dB_Model.HasAddDto = true;

            dB_Model.Generate(dB_Model);

            return Json(new { Success = true });
        }
    }
}
