﻿using Microsoft.AspNetCore.Mvc;
using SV21T1020105.DomainModels;

namespace SV21T1020105.Shop.Controllers
{
    public class CartController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        public IActionResult Index()
        {
            return View(GetShoppingCart());
        }

        public IActionResult ShoppingCart()
        {
            if (GetShoppingCart() == null || GetShoppingCart().Count() == 0)
            {
                return PartialView("_EmptyCart");
            }
            return View(GetShoppingCart());
        }

        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }

        public IActionResult AddToCart(CartItem item)
        {
            if (item.Quantity <= 0)
                return Json("Số lượng không hợp lệ");

            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }
            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult GetCartCount()
        {
            var shoppingCart = GetShoppingCart();
            return Content(shoppingCart.Count.ToString());
        }

        public IActionResult UpdateQuantity(int productID, int quantity)
        {
            if (quantity <= 0)
                return Json("Số lượng không hợp lệ");
            var shoppingCart = GetShoppingCart();

            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == productID);
            if (existsProduct != null)
            {
                existsProduct.Quantity = quantity;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
    }
}
