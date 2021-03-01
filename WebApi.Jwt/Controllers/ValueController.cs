using System.Web.Http;
using WebApi.Jwt.Filters;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Jwt.Controllers
{
    public class ValueController : ApiController
    {
        [JwtAuthentication]
        [HttpGet]
        [Route ("testGET")]
        public string Getvalues(string Username)
        {

            return "ยินดีต้อนรับ";
        }

        [AllowAnonymous]
        //  [JwtAuthentication]
        [Route("api/test1")]
        public  string Type(int size)
        {
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            var charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var chars = charSet.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

    }

}