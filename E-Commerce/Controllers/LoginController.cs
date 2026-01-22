using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.Repositories;
using E_Commerce.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private ApiResponse _apiResponse;
        private readonly IConfiguration _configuration;

        public LoginController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _apiResponse = new ApiResponse();
            _configuration = configuration;
        }

        [HttpPost("signup-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateUserAsync(SignUpDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = "Invalid Input";

                    return BadRequest(_apiResponse);
                }

                User? existingUser = await _userRepository.GetAsync(u => u.Email == dto.Email);
                
                if (existingUser != null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = "Email already taken!";

                    return BadRequest(_apiResponse);
                }

                User user = _mapper.Map<User>(dto);
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    var passwordHashWithSalt = CreatePasswordHashWithSalt(dto.Password);
                    user.Password = passwordHashWithSalt.PasswordHash;
                    user.PasswordSalt = passwordHashWithSalt.Salt;
                }

                if (user.Email == "pratyush@gmail.com")
                {
                    user.Role = "Admin";
                }
                else
                {
                    user.Role = "User";
                }

                await _userRepository.CreateAsync(user);

                byte[] key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("LocalKey"));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, dto.Username),
                        new Claim(ClaimTypes.Email, dto.Email)
                    }),
                    Expires = DateTime.Now.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var responseToken = tokenHandler.WriteToken(token);

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = _mapper.Map<UserDTO>(user);
                _apiResponse.Token = responseToken;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("signin-user")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> SignInUserAsync(SignInDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = "Invalid Input";

                    return BadRequest(_apiResponse);
                }

                User? existingUser = await _userRepository.GetAsync(u => u.Email == dto.Email);

                if (existingUser == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                    _apiResponse.Errors = "Incorrect Username";

                    return Unauthorized(_apiResponse);
                }

                var storedSaltBytes = Convert.FromBase64String(existingUser.PasswordSalt);

                var incomingPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: dto.Password,
                    salt: storedSaltBytes,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8
                ));

                if (incomingPasswordHash != existingUser.Password)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                    _apiResponse.Errors = "Incorrect Password";

                    return Unauthorized(_apiResponse);
                }

                string claimRole = existingUser.Role == "User" ? "User" : "Admin";

                byte[] key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("LocalKey"));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, dto.Email),
                        new Claim(ClaimTypes.Role, claimRole)
                    }),
                    Expires = DateTime.Now.AddMinutes(500),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var responseToken = tokenHandler.WriteToken(token);

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = _mapper.Map<UserDTO>(existingUser);
                _apiResponse.Token = responseToken;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetUserById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = "Id was invalid";

                    return BadRequest(_apiResponse);
                }

                User? user = await _userRepository.GetAsync(user => user.Id == id);

                if (user == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = $"User with ID: {id} was not found in the database";

                    return NotFound(_apiResponse);
                }


                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = user.Id;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors = ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
            }
        }

        private (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password)
        {
            // Create the Salt

            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Create the Hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            return (hash, Convert.ToBase64String(salt));
        }

    }
}
