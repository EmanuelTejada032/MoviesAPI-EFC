using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI_EFC.DTOs.Security;
using MoviesAPI_EFC.Services.Implementation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly HashService _hashService;
        private readonly IDataProtector _dataProtector;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration,
            IDataProtectionProvider dataProtectionProvider, HashService hashService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._hashService = hashService;
            _dataProtector = dataProtectionProvider.CreateProtector("Unique_And_Posible_Secret_Value");
        }

        [HttpGet("hash/{plainText}")]
        public ActionResult Hash(string plainText)
        {
            var hash1 = _hashService.GetHash(plainText);
            var hash2 = _hashService.GetHash(plainText);
            return Ok(new { PlainText = plainText, Hash1 = hash1, Hash2 = hash2 });
        }

        [HttpGet("Encrypt")]
        public ActionResult Encrypt()
        {
            var plainText = "Emanuel Tejada";
            var encryptionText = _dataProtector.Protect(plainText);
            var decodedText = _dataProtector.Unprotect(encryptionText);

            return Ok(new
            {
                plainText = plainText,
                encryptionText = encryptionText,
                decodedText = decodedText
            });
        }

        [HttpGet("TimedEncryption")]
        public ActionResult TimedEncryption()
        {
            var timedDataProtector = _dataProtector.ToTimeLimitedDataProtector();
            var plainText = "Emanuel Tejada";
            var encryptionText = timedDataProtector.Protect(plainText, TimeSpan.FromSeconds(8));
            Thread.Sleep(6000);
            var decodedText = timedDataProtector.Unprotect(encryptionText);

            return Ok(new
            {
                plainText = plainText,
                encryptionText = encryptionText,
                decodedText = decodedText
            });
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AuthResponse>> SignUp([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);
            if (result.Succeeded) return await GetAuthResponse(userCredentials);
            else { return BadRequest(result.Errors); }
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<AuthResponse>> SignIn([FromBody] UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded) return await GetAuthResponse(userCredentials);
            else { return BadRequest("Incorrect credentials"); }
        }

        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthResponse>> RenewToken()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "email");
            if (email == default) return BadRequest();

            var userCredentials = new UserCredentials()
            {
                Email = email.Value,
            };
            return await GetAuthResponse(userCredentials);
        }


        private async Task<AuthResponse> GetAuthResponse(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimsFromDB = await _userManager.GetClaimsAsync(user);
            claims.AddRange(claimsFromDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtkey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationDate = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expirationDate, signingCredentials: credentials);

            return new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expires = expirationDate
            };
        }

        [HttpPost("SetAdminRole")]
        public async Task<ActionResult> SetAdminRole(AdminSetRoleDTO adminSetRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(adminSetRoleDTO.Email);
            if (user == null) return NotFound();
            await _userManager.AddClaimAsync(user, new Claim("isAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdminRole")]
        public async Task<ActionResult> RemoveAdminRole(AdminSetRoleDTO adminSetRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(adminSetRoleDTO.Email);
            await _userManager.RemoveClaimAsync(user, new Claim("isAdmin", "1"));
            return NoContent();
        }
    }
}
