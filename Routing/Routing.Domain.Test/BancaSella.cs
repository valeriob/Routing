using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Routing.Domain.Test
{
    [TestClass]
    public class BancaSella_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new BancaSella.WSCryptDecryptSoapClient();

            string shopLogin = "GESPAY54144";
            string uicCode = "242";
            string amount = "123";
            string shopTransactionId = "123";

            string cardNumber = "12345678901234567890";
            string expiryMonth = "01";
            string expiryYear = "2015";
            string buyerName = "name";
            string buyerEmail = "email@google.com";
            string languageId = "2";
            string cvv = "123";


            cardNumber = "";
            expiryMonth = "";
            expiryYear = "";
            buyerName = "";
            buyerEmail = "";
            languageId = "";
            cvv = "";



            string customInfo = "";
            string CryptedString = "CryptedString";


            var encripted = service.Encrypt(shopLogin, uicCode, amount, shopTransactionId, cardNumber, expiryMonth, expiryYear, buyerName, buyerEmail, languageId, cvv, customInfo);
            var decripted = service.Decrypt(shopLogin, CryptedString);

            var url = string.Format("https://testecomm.sella.it/gestpay/pagam.asp?a={0}&b={1}", shopLogin, encripted.InnerText);
        }
    }
}
