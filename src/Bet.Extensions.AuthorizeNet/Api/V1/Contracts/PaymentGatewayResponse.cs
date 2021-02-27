using System;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Contracts
{
    /// <summary>
    /// Mapping for this class can be found at:
    /// https://www.authorize.net/content/dam/anet-redesign/documents/AIM_guide.pdf.
    /// https://community.developer.authorize.net/t5/Integration-and-Testing/Definitive-guide-key-to-directResponse-fields/td-p/20447
    /// Fields in the Payment Gateway Response.
    /// </summary>
    public class PaymentGatewayResponse
    {
        public PaymentGatewayResponse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(input);
            }

            var parsed = input.Split(',');
            ResponseCode = parsed[0];
            ResponseReasonCode = parsed[2];
            ResponseReasonText = parsed[3];
            AuthorizationCode = parsed[4];
            AVSResponse = parsed[5];
            TransactionId = parsed[6];
            InvoiceNumber = parsed[7];
            Description = parsed[8];
            Amount = parsed[9];
            Method = parsed[10];
            TransactionType = parsed[11];
            CustomerId = parsed[12];
            ZipCode = parsed[19];
            CardCodeResponse = parsed[38];
            CardholderAuthenticationVerificationResponse = parsed[39];

            AccountNumber = parsed[50];
            CardType = parsed[51];
        }

        /// <summary>
        /// 1 Response Code Value: The overall status of the transaction.
        /// 1 = Approved
        /// 2 = Declined
        /// 3 = Error
        /// 4 = Held for review.
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// 3 Response
        /// Reason Code
        /// Value: A code that represents more details about the result of the transaction.
        /// 1 - This transaction has been approved.
        /// 2 - This transaction has been declined.
        /// 3 - There has been an error processing this transaction.
        /// 4 - This transaction is being held for review.
        /// </summary>
        public string ResponseReasonCode { get; set; }

        /// <summary>
        /// 4 Response Reason Text
        /// Value: A brief description of the result that corresponds with the response reason code.
        /// </summary>
        public string ResponseReasonText { get; set; }

        /// <summary>
        /// 5 Authorization  Code
        /// Value: The authorization or approval code
        /// Format: 6 characters.
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
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
        public string AVSResponse { get; set; }

        /// <summary>
        /// 7 Transaction ID Value: The payment gateway-assigned identification number for the transaction.
        /// Format: When x_test_request is set to a positive response, or
        ///         when Test Mode is enabled on the payment gateway, this value is 0.
        /// Notes: This value must be used for any follow-on transactions
        /// such as a CREDIT, PRIOR_AUTH_CAPTURE, or VOID.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 8 Invoice Number Value: The merchant-assigned invoice number for the transaction.
        /// Format: 20-character maximum(no symbols).
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// 9 Description Value: The transaction description.
        /// Format: 255-character maximum(no symbols).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 10 Amount Value: The amount of the transaction.
        /// Format: 15-digit maximum.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 11 Method Value: The payment method.
        /// CC or ECHECK.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 12 Transaction Type Value: The type of credit card transaction
        /// Format: AUTH_CAPTURE, AUTH_ONLY, CAPTURE_ONLY, CREDIT, PRIOR_AUTH_CAPTURE, VOID.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 13 Customer ID Value: The merchant-assigned customer ID.
        /// Format: 20-character maximum(no symbols).
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 20 ZIP Code Value: The ZIP code of the customer’s billing address.
        /// Format: 20-character maximum(no symbols.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 39 Card Code Response
        /// Value: The card code verification(CCV) response code
        /// Format:
        ///  M = Match
        ///  N = No Match
        ///  P = Not Processed
        ///  S = Should have been present
        ///  U = Issuer unable to process request
        /// Notes: Indicates the result of the CCV filter.
        /// </summary>
        public string CardCodeResponse { get; set; }

        /// <summary>
        /// 40 Cardholder
        ///         Authentication
        ///         Verification
        /// Response
        /// Value: The cardholder authentication verification response code
        /// Format: Blank or not present = CAVV not validated
        ///  0—CAVV not validated because erroneous data was
        /// submitted
        ///  1—CAVV failed validation
        ///  2—CAVV passed validation
        ///  3—CAVV validation could not be performed; issuer attempt
        /// incomplete
        ///  4—CAVV validation could not be performed; issuer system
        /// error
        ///  5—Reserved for future use
        ///  6—Reserved for future use
        ///  7—CAVV attempt—failed validation—issuer available(U.S.-
        /// issued card/non-U.S acquirer)
        ///  8—CAVV attempt—passed validation—issuer available(U.S.-
        /// issued card/non-U.S.acquirer)
        ///  9—CAVV attempt—failed validation—issuer unavailable
        /// (U.S.-issued card/non-U.S.acquirer)
        ///  A—CAVV attempt—passed validation—issuer unavailable
        /// (U.S.-issued card/non-U.S.acquirer)
        ///  B—CAVV passed validation, information only, no liability shift.
        /// </summary>
        public string CardholderAuthenticationVerificationResponse { get; set; }

        /// <summary>
        /// 51 Account Number Value: Last 4 digits of the card provided
        /// Format: Alphanumeric(XXXX6835)
        /// Notes: This field is returned with all transactions.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// 52 Card Type Value: Visa, MasterCard, American Express, Discover, Diners Club, JCB.
        /// Format: Text.
        /// </summary>
        public string CardType { get; set; }
    }
}
