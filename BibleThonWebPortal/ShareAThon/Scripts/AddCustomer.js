$(function () {
    // ------- Error Handlings

    function showErrorInfo(message) {
        var element = $('.Success,.errorinfo');
        element.html(message).show().removeClass('Success').addClass('errorinfo');
        element.parent().show();
    }

    function hideErrorInfo() {
        $('.errorinfo').html('').hide();
        $('.errorinfo').parent().show();
    }

    // allow only numbers for specified fields
    $("[id$=TxtQuantity],[id$=txtCreditCardNo],[id$=txtExpirationDate],[id$=txtCVN],[id$=txtCurDonor],[id$=txtDonationAmt]").keydown(function (event) {
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    // validation for save starts here
    $('[id$=BtnAddCustomer]').click(function () {
        // validation on billing address tab
        if (!validateAddCustomerDetails()) {
            return false;
        }
        return true;
    });

    // validation method for Add Customer Details.
    function validateAddCustomerDetails() {
        var element = $('[id$=txtCustomerName]');
        if ($.trim(element.val()) == '') {
            showErrorInfo("Please input customer name.");
            element.focus();
            return false;
        }

        element = $('[id$=txtCustomerNumber]');
        if ($.trim(element.val()) == '') {
            showErrorInfo("Please input customer number.");
            element.focus();
            return false;
        }
        element = $('[id$=txtAddressCode]');
        if ($.trim(element.val()) == '') {
            showErrorInfo("Please input address code.");
            element.focus();
            return false;
        }
        element = $('[id$=txtAddress1]');
        if ($.trim(element.val()) == '') {
            showErrorInfo("Please input address.");
            element.focus();
            return false;
        }
        element = $('[id$=txtAddress2]');
        if ($.trim(element.val()) == '') {
            showErrorInfo("Please input address1.");
            element.focus();
            return false;
        }

        /*element = $('[id$=txtCreditCardNo]');
        if($.trim(element.val()) == '') {
        showErrorInfo("Please input credit card number.");
        element.focus();
        return false;
        }
        element = $('[id$=txtExpirationDate]');
        if($.trim(element.val()) == '') {
        showErrorInfo("Please input credit card expire date.");
        element.focus();
        return false;
        }*/
        $(this).attr('disabled', 'disabled');
        showErrorInfo("Please wait....");
        return true;
    }
});