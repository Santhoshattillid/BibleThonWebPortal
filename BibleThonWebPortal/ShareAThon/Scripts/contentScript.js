$(function () {
    // ------- Global Variables
    var isBillingAddressSelected = false;
    var isItCreditCardTransaction = true;
    // ------- Accordations

    // accordation code here
    $('.accordion div').hide();
    $('.accordion h3:first').addClass('current').next().slideDown('fast');
    $('.accordion h3').click(function () {
        if ($(this).next().is(':hidden')) {
            $('.accordion h3').removeClass('current').next().slideUp();
            $(this).toggleClass('current').next().slideDown();
        }
    });

    // setting the default accordian tab based on selected index
    var activeIndex = parseInt($('input[id$=hidAccordionIndex]').val());
    $('.accordion h3').eq(activeIndex).trigger('click');
    //$('.accordion h3').eq(4).trigger('click'); // temporarly on last tab

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

    // ------- Popup Screen Methods

    $('.CustomerNoLink').live('click', function () {
        var row = $(this).parents('tr:first');
        var customerDetails = {
            'no': $(this).html(),
            'name': row.find('td').eq(1).text(),
            'address1': row.find('td').eq(2).text(),
            'address2': row.find('td').eq(3).text(),
            //'address3': row.find('td').eq(4).text(),
            'city': row.find('td').eq(4).text(),
            'state': row.find('td').eq(5).text(),
            'country': row.find('td').eq(6).text(),
            'zipCode': row.find('td').eq(7).text(),
            'telephone1': row.find('td').eq(8).text(),
            //'telephone2': row.find('td').eq(11).text(),
            'email': $.trim(row.find('td').eq(9).text()),
            'cardno': $.trim($('[id$=HdnCreditCardNumber]').val()),
            'expireDate': $.trim($('[id$=HdnExpireDate]').val()),
        };
        getRadWindow().close(customerDetails);
    });

    function getRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    $('#BtnBack').click(function() {
        window.history.back();
    });

    // ------- Validations for all the tabs

    // billing address validation here [On Billing address continue button click event]
    $('input[id$=btnBillContinue]').click(function () {
        //validation starts here
        if(validateBillingAddress())
            $('.DonationAccordion').trigger('click');
        return false;
    });

    // validation method for billing address.
    function validateBillingAddress() {
        var value = $('[id$=txtCustName]').val();
        if (!isBillingAddressSelected || (value != null && $.trim(value) == "")) {
            showErrorInfo("Please select customer and then continue.");
            return false;
        } else {
            hideErrorInfo();
            return true;
        }
    }
    // validation method for donation details.
    function validateDonationDetails() {
        var element = $('[id$=drpFrequency]');
        if($.trim(element.val()) == '') {
            showErrorInfo("Please select donation frequency.");
            element.focus();
            return false;
        }

        element = $('[id$=txtInitialCharge]');
        if($.trim(element.val()) == '') {
            showErrorInfo("Please input Initial Charge on for donation.");
            element.focus();
            return false;
        }
        element = $('[id$=txtDonationAmt]');
        if($.trim(element.val()) == '') {
            showErrorInfo("Please input donation amount.");
            element.focus();
            return false;
        }
        return true;
    }

    // confirm order validation here address validation here
    $('[id$=BtnDonationContinue]').click(function () {
        if(validateDonationDetails()) {
            if(isItCreditCardTransaction) {
                $('.creditCardAccordion').trigger('click');
                setCcPaymentInformation();
            }
            else
                $('.orderConfirmationAccordion').trigger('click');
            hideErrorInfo();
        }
        return false; // put always false since everything handling client side itself
    });

    // validation method for credit card details.
    function validateCreditCardDetails() {
        if(isItCreditCardTransaction) {
            var element = $('[id$=txtExpirationDate]');
            if ($.trim(element.val()) == '') {
                showErrorInfo("Please input credit card expire date.");
                element.focus();
                return false;
            }
            element = $('[id$=txtCreditCardNo]');
            if ($.trim(element.val()) == '') {
                showErrorInfo("Please input credit card number.");
                element.focus();
                return false;
            }
            element = $('[id$=txtCVN]');
            if ($.trim(element.val()) == '') {
                showErrorInfo("Please input credit card CVN number.");
                element.focus();
                return false;
            }
        }
        return true;
    }

    // payment continue button handling here
    $('[id$=BtnCreditCardProcess]').click(function () {
        if(validateCreditCardDetails()) {
            $('.orderConfirmationAccordion').trigger('click');
            hideErrorInfo();
        }
        return false; // put always false since everything handling client side itself
    });

    // custom search for billing address
    $('#imgSearch').click(function () {
        var name = $('input[id$=txtCustName]').val();
        var telephone = $('input[id$=txtPhone]').val();
        var wnd = window.radopen("BillingAddress.aspx?name=" + name + "&telephone=" + telephone);
        wnd.setSize(950, 400);
        //wnd.autoSize(true);
        //wnd.set_autoSize(true);
        wnd.add_close(onClientClose);
        wnd.center();
        return false;
        //}
    });

    // getting customer information from rad window for billing address
    function onClientClose(oWnd, args) {
        var arg = args.get_argument();
        if (arg != null) {
            $('[id$=HdnCustomerNo]').val(arg.no);
            $('input[id$=txtCustName]').val(arg.name);
            $('[id$=lblAddress1]').html(arg.address1);
            $('[id$=lblAddress2]').html(arg.address2);
            $('[id$=lblAddress3]').html(arg.address3);
            $('[id$=lblCity]').html(arg.city);
            $('[id$=lblState]').html(arg.state);
            $('[id$=lblZipCode]').html(arg.zipCode);
            $('[id$=lblCountry]').html(arg.country);
            $('[id$=txtBEmail]').val(arg.email);
            $('[id$=txtPhone]').val(arg.telephone1);

            // updating Credit Card Information tab
            $('[id$=txtCreditCardNo]').val(arg.cardno);
            $('[id$=txtExpirationDate]').val(arg.expireDate);

            // updating Donation Info
            //$('[id$=txtDonationAmt]').val(arg.cardno);
            $('[id$=drpFrequency]').val(arg.expireDate);

            // updating left hand side panel credit card info
            if ($.trim(arg.cardno) != "")
                $('[id$=LblCreditCardNo]').html("xxxxxxxxxxxx" + arg.cardno.substr(arg.cardno.length - 4));
            else
                $('[id$=LblCreditCardNo]').html("-");
            $('[id$=LblCreditExpired]').html(arg.expireDate);

            // updating confirmation tab labels
            $('[id$=TxtOCCustomerNO]').val(arg.no);
            $('[id$=TxtOCCustomerName]').val(arg.name);

            // updating left hand side panel
            $('[id$=txtCustName],[id$=txtPhone],[id$=txtBEmail]').each(function () {
                var ids = $(this).attr('for').split(",");
                for (var index = 0; index < ids.length; index++) {
                    $('[id$=' + $.trim(ids[index]) + ']').html($(this).val());
                }
            });

            $('[id$=lblAddress1],[id$=lblAddress2],[id$=lblAddress3]').each(function () {
                var id = $(this).attr('for');
                $('[id$=' + id + ']').html($(this).text());
            });

            $('[id$=LblBCityStateZip]').html($('[id$=lblCity]').text() + ", " + $('[id$=lblState]').text() + ", " + $('[id$=lblZipCode]').text());

            isBillingAddressSelected = true;
        }
    }

    // -------- Back Buttons

    // back button handling here
    $('[id$=BtnOrderConfirmationBack]').click(function () {
        if(isItCreditCardTransaction)
            $('.creditCardAccordion').trigger('click');
        else
            $('.DonationAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnCreditCardBack]').click(function () {
        $('.DonationAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnOfferLineBack]').click(function () {
        $('.DonationAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnDonationBack]').click(function () {
        $('.BillingAddressAccordion').trigger('click');
        return false;
    });

    $('#RadGridOfferLines').show();

    $('.NextPrevAndNumeric *').show();

    function formatToMoney(theNumber, theCurrency, theDecimal) {
        var theDecimalDigits = Math.round((theNumber * 100) - (Math.floor(theNumber) * 100));
        theDecimalDigits = "" + (theDecimalDigits + "0").substring(0, 2);
        theNumber = "" + Math.floor(theNumber);
        var theOutput = theCurrency;
        for (var x = 0; x < theNumber.length; x++) {
            theOutput += theNumber.substring(x, x + 1);
        };
        theOutput += theDecimal + theDecimalDigits;
        return theOutput;
    }

    $('[id$=txtDonationAmt]').each(function() {
        var value = $.trim($(this).val());
        if(value != '') {
            $(this).val(formatToMoney(value, "$", "."));
        }
    });

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

    // updating billing address at left hand side panel
    $('[id$=txtCustName],[id$=txtPhone],[id$=txtBEmail]').keyup(function () {
        var ids = $(this).attr('for').split(",");
        for (var index = 0; index < ids.length; index++) {
            $('[id$=' + $.trim(ids[index]) + ']').html($(this).val());
        }
    });

    $('[id$=txtCustName],[id$=txtPhone],[id$=txtBEmail]').change(function () {
        var ids = $(this).attr('for').split(",");
        for (var index = 0; index < ids.length; index++) {
            $('[id$=' + $.trim(ids[index]) + ']').html($(this).val());
        }
    });

    $('[id$=lblAddress1],[id$=lblAddress2],[id$=lblAddress3]').change(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).text());
    });

    $('[id$=lblCity],[id$=lblState],[id$=lblZipCode]').change(function () {
        $('[id$=LblBCityStateZip]').html($('[id$=lblCity]').text() + ", " + $('[id$=lblState]').text() + ", " + $('[id$=lblZipCode]').text());
    });

    // updating credit card information at left hand side panel
    $('[id$=txtExpirationDate]').keyup(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=txtExpirationDate]').change(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=txtCreditCardNo]').blur(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html("xxxxxxxxxxxx" + $(this).val().toString().substr($(this).val().toString().length - 4));
    });

    // updating method of donation
    $('[id$=drpMethdDonation]').change(function() {
        var value = $(this).find(':selected').text();
        if(value == "Credit Card") {
            $('div.creditCardAccordion').show();
            $('a.creditCardAccordion').parent().show();
            $('#CCInformation').show();
            isItCreditCardTransaction = true;
        }
        else {
            $('div.creditCardAccordion').hide();
            $('a.creditCardAccordion').parent().hide();
            $('#CCInformation').hide();
            isItCreditCardTransaction = false;
        }
            $('[id$=LblMethodOfDonation]').text(value);
    });

    $('[id$=LblMethodOfDonation]').text($('[id$=drpMethdDonation]').find(':selected').text());

    // updating donation details at left hand side panel
    function setDonationDetails() {
        var value = $.trim($('[id$=txtDonationAmt]').val());
        if(value != '') {
             var donationType = $('[id$=drpFrequency] :selected').text();
               if(donationType == "Monthly") {
                   $('[id$=LblDonationInfo]').text("$ " + value + " Monthly, " + "every " + $('[id$=drpMonths] :selected').text() + " month (s)");
               }
            else {
                   $('[id$=LblDonationInfo]').text("$ " + value);
               }
        }
    }

    $('[id$=txtDonationAmt]').keyup(function() {
        setDonationDetails();
    });

    $('[id$=drpMonths]').change(function() {
        setDonationDetails();
    });

    $('[id$=drpFrequency]').change(function() {
        var x = $(this).find(':selected').text();
        if (x == "Monthly") {
            $('#drpMonths').prop("disabled", false);
            $('#drpchargeMontly').prop("disabled", false);
        } else {
            $('#drpMonths').prop("disabled", true);
            $('#drpchargeMontly').prop("disabled", true);
        }

        if (x == "Other") {
            $('#DropDownList1').prop("disabled", false);
        } else {
            $('#DropDownList1').prop("disabled", true);
        }
        setDonationDetails();
        setCcPaymentInformation();
    });

    // set donation amount to increasing to field
$('input[id$=txtDonationAmt]').keyup(function () {
    $('[id$=txtIncreasing]').val($('[id$=txtDonationAmt]').val());
    $('[id$=TxtTotalOrder]').val($('[id$=txtDonationAmt]').val());
});

    // updating credit card information at left hand side panel
    function setCcPaymentInformation() {
        var type = $('[id$=ddlCreditCardType] :selected').text();
        var donationType = $('[id$=drpFrequency] :selected').text();
               if(donationType == "Monthly") {
                   $('[id$=LblCreditCardType]').text(type + ", Recurring");
                   $('[id$=LblInitialChargeOn]').text("Initial charge on " + $('[id$=txtInitialCharge]').val());
                   $('[id$=LblRecurrence]').text("Charge on every " + $('[id$=drpMonths] :selected').text() + " of month.");
               } else
                       $('[id$=LblCreditCardType]').text(type);
    }

    $('[id$=ddlCreditCardType]').change(function() {
        setCcPaymentInformation();
    });

        // validation for save starts here
        $('[id$=btnSaveOrder]').click(function () {
            // validation on billing address tab
            if (!validateBillingAddress()) {
                $('.BillingAddressAccordion').trigger('click');
                return false;
            }
            // validation on donation tab
            if (!validateDonationDetails()) {
                $('.DonationAccordion').trigger('click');
                return false;
            }
            // validation on credit card tab
            if (!validateCreditCardDetails()) {
                $('.creditCardAccordion').trigger('click');
                return false;
            }
            return true;
        });
});