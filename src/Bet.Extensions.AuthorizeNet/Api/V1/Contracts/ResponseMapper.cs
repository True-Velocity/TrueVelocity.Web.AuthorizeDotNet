namespace Bet.Extensions.AuthorizeNet.Api.V1.Contracts
{
    public sealed class ResponseMapper
    {
        /// <summary>
        /// Maps string ResponseCode to enumeration.
        /// The overall status of the transaction.
        /// 1 = Approved
        /// 2 = Declined
        /// 3 = Error
        /// 4 = Held for review.
        /// </summary>
        /// <param name="responseCode">The value for response code as string.</param>
        /// <returns></returns>
        public static ResponseCodeEnum GetResponseCode(string responseCode)
        {
            return responseCode switch
            {
                "1" => ResponseCodeEnum.Approved,
                "2" => ResponseCodeEnum.Declined,
                "3" => ResponseCodeEnum.Error,
                "4" => ResponseCodeEnum.HeldForReview,
                _ => ResponseCodeEnum.Unknown,
            };
        }

        /// <summary>
        /// Maps string ResponseCode to Text.
        /// Reason Code
        /// Value: A code that represents more details about the result of the transaction.
        /// 1 - This transaction has been approved.
        /// 2 - This transaction has been declined.
        /// 3 - There has been an error processing this transaction.
        /// 4 - This transaction is being held for review.
        /// </summary>
        /// <param name="responseCode"></param>
        /// <returns></returns>
        public static string GetResponseReasonCode(string responseCode)
        {
            return responseCode switch
            {
                "1" => "This transaction has been approved.",
                "2" => "This transaction has been declined.",
                "3" => "There has been an error processing this transaction.",
                "4" => "This transaction is being held for review.",
                _ => "This transaction is an uknown state.",
            };
        }

        /// <summary>
        /// Maps AVS ResponseCode String to text.
        /// 6 AVS Response Value: The Address Verification Service (AVS) response code.
        /// Format:
        ///  A = Address(Street) matches, ZIP does not
        ///  B = Address information not provided for AVS check
        ///  E = AVS error
        ///  G = Non-U.S.Card Issuing Bank
        ///  N = No Match on Address(Street) or ZIP
        ///  P = AVS not applicable for this transaction
        ///  R = Retry—System unavailable or timed out
        ///  S = Service not supported by issuer
        ///  U = Address information is unavailable
        ///  W = Nine digit ZIP matches, Address (Street) does not
        ///  X = Address(Street) and nine digit ZIP match
        ///  Y = Address(Street) and five digit ZIP match
        ///  Z = Five digit ZIP matches, Address (Street) does not
        /// Notes: Indicates the result of the AVS filter.
        /// </summary>
        /// <param name="avsResponseCode"></param>
        /// <returns></returns>
        public static string GetAVSResponseText(string avsResponseCode)
        {
            return avsResponseCode switch
            {
                "A" => "Address(Street) matches, ZIP does not",
                "B" => "Address information not provided for AVS check",
                "E" => "AVS error",
                "G" => "Non-U.S.Card Issuing Bank",
                "N" => "No Match on Address(Street) or ZIP",
                "P" => "AVS not applicable for this transaction",
                "R" => "Retry—System unavailable or timed out",
                "S" => "Service not supported by issuer",
                "U" => "Address information is unavailable",
                "W" => "Nine digit ZIP matches, Address (Street) does not",
                "X" => "Address(Street) and nine digit ZIP match",
                "Y" => "Address(Street) and five digit ZIP match",
                "Z" => "Five digit ZIP matches, Address (Street) does not",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Value: The card code verification(CCV) response code
        /// Format:
        ///  M = Match
        ///  N = No Match
        ///  P = Not Processed
        ///  S = Should have been present
        ///  U = Issuer unable to process request
        /// Notes: Indicates the result of the CCV filter.
        /// </summary>
        /// <param name="cardCodeResponse"></param>
        /// <returns></returns>
        public static string GetCardCodeResponseText(string cardCodeResponse)
        {
            return cardCodeResponse switch
            {
                "M" => "CCV card code - was matched",
                "N" => "CCV card code - didn't match",
                "P" => "CCV card code - was not processed",
                "S" => "CCV card code - should have been present",
                "U" => "CCV card code - Issuer unable to process request",
                _ => "CCV card code - Unknown"
            };
        }
    }
}
