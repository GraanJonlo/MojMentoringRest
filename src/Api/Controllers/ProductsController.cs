﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductsController : Controller
    {
        private static readonly Dictionary<string, Product> Products;

        #region NoddySetup

        private static readonly object Lock = new object();

        static ProductsController()
        {
            var widget = new Product("1", "Widget");
            widget.Reviews.Add(new Review(1, "Does it for me"));
            widget.Reviews.Add(new Review(2, "It sucks"));
            var bobbin = new Product("2", "Bobbin");
            bobbin.Reviews.Add(new Review(1, "It mjade me LOL"));
            Products = new Dictionary<string, Product>
            {
                {"1", widget},
                {"2", bobbin}
            };
        }

        #endregion

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            var plainText = "This is my shared, not so secret, secret!";
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainText));
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    "http://my.website.com",
                    "http://my.otherwebsite.com"
                },
                ValidIssuers = new string[]
                {
                    "http://my.tokenissuer.com",
                    "http://my.othertokenissuer.com"
                },
                IssuerSigningKey = key
            };
            
            lock (Lock)
            {
                return Ok(Products.Values);
            }
        }

        [HttpGet("{sku}")]
        public IActionResult Get(string sku)
        {
            lock (Lock)
            {
                if (Products.TryGetValue(sku, out var product))
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        #region Reviews

        [HttpGet("{sku}/reviews")]
        public IActionResult GetReviews(string sku)
        {
            lock (Lock)
            {
                if (Products.TryGetValue(sku, out var product))
                {
                    return Ok(product.Reviews);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("{sku}/reviews/{id}")]
        public IActionResult GetReview(string sku, int id)
        {
            lock (Lock)
            {
                if (Products.TryGetValue(sku, out var product))
                {
                    return Ok(product.Reviews.Where(x => x.Id == id));
                }
                else
                {
                    return NotFound();
                }
            }
        }

        #endregion

        [HttpPut("{sku}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Put(string sku, [FromBody] Product product)
        {
            lock (Lock)
            {
                if (Products.ContainsKey(sku))
                {
                    Products[sku] = product;
                    return Ok(product);
                }
                else
                {
                    Products.Add(sku, product);
                    return Created("/api/v1/products/" + sku, product);
                }
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Post([FromBody] Product product)
        {
            lock (Lock)
            {
                product.Sku = Guid.NewGuid().ToString();
                Products.Add(product.Sku, product);
                return Created("/api/v1/products/" + product.Sku, product);
            }
        }

        [HttpDelete("{sku}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(string sku)
        {
            lock (Lock)
            {
                Products.Remove(sku);
            }

            return NoContent();
        }
    }
}