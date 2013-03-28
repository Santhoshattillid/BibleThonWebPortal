$(function () {
        // ------- Popup Screen Methods
        var customerDetails = {
            'no': $('[id$=txtCustomerNumber]').val(),
            'name': $('[id$=txtCustomerName]').val(),
            'address1': $('[id$=txtAddress1]').val(),
            'address2': $('[id$=txtAddress2]').val(),
            'address3': $('[id$=txtAddress3]').val(),
            'city': $('[id$=txtCity]').val(),
            'state': $('[id$=txtState]').val(),
            'country': $('[id$=txtCountry]').val(),
            'zipCode': $('[id$=txtZipCode]').val(),
            'telephone1': $('[id$=txtTelephone]').val(),
            'email': $('[id$=txtEmail]').val(),
            'cardno': $('[id$=txtCreditCardNo]').val(),
            'expireDate': $.trim($('[id$=txtExpirationDate]').val()),
        };
        getRadWindow().close(customerDetails);

    function getRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
});