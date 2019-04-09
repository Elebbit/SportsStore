using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            var cart = session?.GetJson<SessionCart>("cart")
                       ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("cart");
        }
    }
}
