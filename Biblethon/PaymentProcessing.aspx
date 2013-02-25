<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentProcessing.aspx.cs"
    Inherits="Biblethon_PaymentProcessing" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="AuthorizeNet" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('form').submit();
        });
    </script>
</head>
<body>
    <form action='<%= Utilities.AuthorizeNetUrl %>' method='post'>
    <% var seq = Crypto.GenerateSequence();
       var timeStamp = Crypto.GenerateTimestamp().ToString(CultureInfo.InvariantCulture);
               
    %>
    <input type='hidden' name='x_fp_hash' value='<%= Utilities.GenerateFingerprint(Convert.ToDecimal(Request.QueryString["Amount"]),seq,timeStamp) %>' />
    <input type='hidden' name='x_fp_sequence' value='<%= seq %>' />
    <input type='hidden' name='x_fp_timestamp' value='<%= timeStamp %>' />
    <input type='hidden' name='x_login' value='<%= Utilities.ApiLoginID %>' />
    <input type='hidden' name='x_amount' value='<%= Request.QueryString["Amount"] %>' />
    <input type='hidden' name='x_relay_url' value='<%= Utilities.SiteUrl + "/BibleThonWebPortal/Biblethon/PaymentReceiving.aspx" %>' />
    <input type='hidden' name='x_relay_response' value='TRUE' />
    <input type='hidden' size='28' name='x_card_num' value='<%= Request.QueryString["CardNo"] %>' id='x_card_num' />
    <input type='hidden' size='5' maxlength='5' name='x_exp_date' value='<%= Request.QueryString["ExpDate"]  %>' id='x_exp_date' />
    <input type='hidden' size='5' maxlength='5' name='x_card_code' id='x_card_code' value='<%= Request.QueryString["CVV"] %>' />
    <div>
        <label>
            Please wait while your payment is processing.
        </label>
    </div>
    <div class="ui-helper-hidden">
        <label>
            This page will be submitted automatically within few seconds. Otherwise click the submit button to continue.
        </label>
        <input type="submit" value="Submit"/>
    </div>
    </form>
</body>
</html>
