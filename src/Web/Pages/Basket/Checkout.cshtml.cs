﻿using System.Text;
using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Interfaces;
using Newtonsoft.Json;
using Microsoft.eShopWeb.Web.Models;

namespace Microsoft.eShopWeb.Web.Pages.Basket;

[Authorize]
public class CheckoutModel : PageModel
{
    private readonly IBasketService _basketService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOrderService _orderService;
    private string? _username = null;
    private readonly IBasketViewModelService _basketViewModelService;
    private readonly IAppLogger<CheckoutModel> _logger;

    public CheckoutModel(IBasketService basketService,
        IBasketViewModelService basketViewModelService,
        SignInManager<ApplicationUser> signInManager,
        IOrderService orderService,
        IAppLogger<CheckoutModel> logger)
    {
        _basketService = basketService;
        _signInManager = signInManager;
        _orderService = orderService;
        _basketViewModelService = basketViewModelService;
        _logger = logger;
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
        await SetBasketModelAsync();
    }

    public async Task<IActionResult> OnPost(IEnumerable<BasketItemViewModel> items)
    {
        try
        {
            await SetBasketModelAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
            await _basketService.SetQuantities(BasketModel.Id, updateModel);
            await _orderService.CreateOrderAsync(BasketModel.Id, new Address("123 Main St.", "Kent", "OH", "United States", "44240"));

            #region order detail function
            //var order = new OrderDetailModel
            //{
            //    OrderId = BasketModel.Id,
            //    Items = items.Select(t => new OrderDetailItemModel
            //    {
            //        ItemId = t.Id,
            //        Quantity = t.Quantity
            //    }).ToList()
            //};
            //var json = JsonConvert.SerializeObject(order);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            //var url = "https://eshoponweb-funcapp.azurewebsites.net/api/order/reserve";
            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("x-functions-key", "qIDZ2-kl-tKD-XnORnhqYThMi8bhNsG1Dack5aMGqq2HAzFusvalzQ==");
            //    var result = await client.PostAsync(url, data);
            //} 
            #endregion

            var order = new OrderDetailModel
            {
                ShippingAddress = "123 Main St., Kent, OH, United States, 44240",
                Items = items.Select(t => new OrderDetailItemModel
                {
                    Id = t.Id,
                    UnitPrice = t.UnitPrice,
                    Quantity = t.Quantity
                }).ToList(),
                TotalPrice = items.Sum(t => t.UnitPrice * t.Quantity)
            };

            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://eshoponweb-funcapp.azurewebsites.net/api/order/process";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-functions-key", "87QK_jvkEMdAGixGfbu_hOxnKsjFyS24KwfJ1KB3gKjpAzFuG7RFIw==");
                var result = await client.PostAsync(url, data);
            }

            await _basketService.DeleteBasketAsync(BasketModel.Id);


        }
        catch (EmptyBasketOnCheckoutException emptyBasketOnCheckoutException)
        {
            //Redirect to Empty Basket page
            _logger.LogWarning(emptyBasketOnCheckoutException.Message);
            return RedirectToPage("/Basket/Index");
        }

        return RedirectToPage("Success");
    }

    private async Task SetBasketModelAsync()
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
        }
        else
        {
            GetOrSetBasketCookieAndUserName();
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(_username!);
        }
    }

    private void GetOrSetBasketCookieAndUserName()
    {
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            _username = Request.Cookies[Constants.BASKET_COOKIENAME];
        }
        if (_username != null) return;

        _username = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
    }
}
