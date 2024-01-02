using Discount.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CoreApiResponse;
using Discount.API.Models;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : BaseController
    {
        ICouponRepository _couponRepository;
        public DiscountController(ICouponRepository couponRepository) {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string productId) {
            try
            {
                var coupon = await _couponRepository.GetDiscount(productId);
                return CustomResult(coupon);

            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody]Coupon coupon)
        {
            try
            {
                var isSaved = await _couponRepository.CreateDiscount(coupon);
                if(isSaved)
                {
                    return CustomResult("Coupon has been created.", coupon);
                }
                return CustomResult("Coupon saved failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var isSaved = await _couponRepository.UpdateDiscount(coupon);
                if (isSaved)
                {
                    return CustomResult("Coupon updated successfully", coupon);
                }
                return CustomResult("Coupon update failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productId)
        {
            try
            {
                var isDeleted = await _couponRepository.DeleteDiscount(productId);
                if (isDeleted)
                {
                    return CustomResult("Coupon deleted successfully");
                }
                return CustomResult("Coupon delete failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
