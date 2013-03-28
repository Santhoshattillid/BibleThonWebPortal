using System;

namespace AlbaDL
{
    public class CardDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CardDetails()
        {
        }

        /// <summary>
        /// Constructor for Card Details
        /// </summary>
        /// <param name="cardName"></param>
        /// <param name="cardNo"></param>
        /// <param name="expireDate"></param>
        /// <param name="authorizeCode"></param>
        public CardDetails(string cardName, string cardNo, DateTime expireDate, string authorizeCode)
        {
            CardName = cardName;
            CardNo = cardNo;
            ExpireDate = expireDate;
            AuthorizeCode = authorizeCode;
        }

        /// <summary>
        /// Credit Card name
        /// </summary>
        public string CardName { get; private set; }

        /// <summary>
        /// Credit Card Number
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// Credit Card Expire Date
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// For Transaction Authorize Code
        /// </summary>
        public string AuthorizeCode { get; set; }

        /// <summary>
        /// Credit Card CVN number
        /// </summary>
        public string CardCode { get; set; }
    }
}