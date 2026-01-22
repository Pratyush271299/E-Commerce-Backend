using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.Repositories;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "ShopperLogin")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private ApiResponse _apiResponse;

        public CartController(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _apiResponse = new ApiResponse();
        }


        [HttpPost("add-new-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddNewProductAsync(CartProduct cartProduct)
        {
            try
            {
                if (cartProduct == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = "Bad Request done by user";

                    return BadRequest(_apiResponse);
                }

                await _cartRepository.CreateAsync(cartProduct);

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = cartProduct;

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

        [HttpPut("update-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdateProductAsync(UpdateDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = "Input model was invalid";

                    return BadRequest(_apiResponse);
                }

                CartProduct? cartProductToUpdate = await _cartRepository.GetAsync(cp => cp.UserId == dto.UserId && cp.AllProductId == dto.AllProductId );

                if (cartProductToUpdate == null)
                {
                    _apiResponse.Success = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Errors = $"Cart Product not added yet";

                    return NotFound(_apiResponse);
                }

                _mapper.Map(dto, cartProductToUpdate);

                await _cartRepository.UpdateAsync();

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = dto;

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

        [HttpGet("all-cart-items/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetCartItemsByUserId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = "UserId was invalid";

                    return BadRequest(_apiResponse);
                }

                List<CartProduct> cartProducts = await _cartRepository.GetAllByFilter(cp => cp.UserId == id);

                if (cartProducts.IsNullOrEmpty())
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = $"User with ID: {id} was not found in the Cart Table";
                    _apiResponse.Data = new List<CartProduct>();
                    return NotFound(_apiResponse);
                }

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = cartProducts;
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

        [HttpGet("{productId:int}/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductByProductId(int productId, int userId)
        {
            try
            {
                if (userId <= 0 || productId == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = "Invalid Product";

                    return BadRequest(_apiResponse);
                }

                CartProduct? cartProduct = await _cartRepository.GetAsync(cp => cp.AllProductId == productId && cp.UserId == userId);

                if (cartProduct == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = $"CartProduct  was not found in the database";

                    return NotFound(_apiResponse);
                }

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = cartProduct;

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

        [HttpDelete("delete-product/{allproductId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteProductByProductId(int allProductId)
        {
            try
            {
                if (allProductId <= 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = "Invalid Product";

                    return BadRequest(_apiResponse);
                }

                CartProduct? cartProductToDelete = await _cartRepository.GetAsync(cp => cp.AllProductId == allProductId);

                if (cartProductToDelete == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;
                    _apiResponse.Errors = $"CartProduct  was not found in the database";

                    return NotFound(_apiResponse);
                }

                await _cartRepository.DeleteAsync(cartProductToDelete);

                _apiResponse.Success = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = $"CartProduct with AllProductId: {allProductId} was successfully removed";

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
    }
}
