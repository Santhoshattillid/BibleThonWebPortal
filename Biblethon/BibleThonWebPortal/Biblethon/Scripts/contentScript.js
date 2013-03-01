
$(function () {

    // accordation code here
    $('.accordion div').hide();
    $('.accordion h3:first').addClass('current').next().slideDown('fast');

    $('.accordion h3').click(function () {
        if ($(this).next().is(':hidden')) {
            $('.accordion h3').removeClass('current').next().slideUp();
            $(this).toggleClass('current').next().slideDown();


        }
    });

    // billing address validation here
    $('input[id$=btnBillContinue]').click(function () {
        //validation starts here
        //if ($('form').validate().element($('input[id$=txtCustName]'))) // && $('form').validate().element($('input[id$=txtPhone]')))
        var value = $('[id$=txtCustName]').val();
        if (value != null && $.trim(value) == "") {
            $('.errorinfo').html("Please select customer and then continue.");
            $('.errorinfo').show();
            $('.errorinfo').parent().show();
            return false;
        } else
            return true;
    });


    // shipping address validation here
    $('[id$=btnShipContinue]').click(function () {

        /*
        var result = $('form').validate().element($('input[id$=txtCustomerName]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtAddress1]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtCity]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtState]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtZipCode]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtZipCode]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtCountry]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtTelephone]'));
        if (!result)
        return false;

        result = $('form').validate().element($('input[id$=txtEmail]'));
        if (!result)
        return false; */

        $('.OffLinesAccordion').trigger('click');
        //resizeGrid();
        return false; // put always false since everything handling client side itself
    });


    // confirm order validation here address validation here
    $('[id$=btnConfirmOffer]').click(function () {
        $('.creditCardAccordion').trigger('click');
        return false; // put always false since everything handling client side itself
    });

    // payment continue button handling here
    $('[id$=BtnCreditCardProcess]').click(function () {
        $('.orderConfirmationAccordion').trigger('click');
        return false; // put always false since everything handling client side itself
    });

    $('[id$=btnBillContinue]').click(function () {
        $('.ShippingAddressAccordion').trigger('click');
        return false; // put always false since everything handling client side itself
    });

    function resizeGrid() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        oWindow.autoSize(true);
        oWindow.set_autoSize(true);
    }

    // custom search for billing address
    $('#imgSearch').click(function () {
        //if ($('form').validate().element($('input[id$=txtCustName]')) && $('form').validate().element($('input[id$=txtPhone]'))) {
        var name = $('input[id$=txtCustName]').val();
        var telephone = $('input[id$=txtPhone]').val();
        var wnd = window.radopen("BillingAddress.aspx?name=" + name + "&telephone=" + telephone);
        //wnd.setSize(1050, 550);
        wnd.autoSize(true);
        wnd.set_autoSize(true);
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


            // updating left hand side panel
            $('[id$=txtCustName],[id$=txtPhone],[id$=txtBEmail]').each(function () {
                var id = $(this).attr('for');
                $('[id$=' + id + ']').html($(this).val());
            });

            $('[id$=lblAddress1],[id$=lblAddress2],[id$=lblAddress3]').each(function () {
                var id = $(this).attr('for');
                $('[id$=' + id + ']').html($(this).text());
            });

            $('[id$=LblBCityStateZip]').html($('[id$=lblCity]').text() + ", " + $('[id$=lblState]').text() + ", " + $('[id$=lblZipCode]').text());
        }
    }


    // custom search for shipping address
    $('#ImgShippingAddressModify').click(function () {
        var wnd = window.radopen("ShippingAddress.aspx?CustomerNo=" + $('[id$=HdnCustomerNo]').val());
        //wnd.setSize(1050, 550);
        wnd.autoSize(true);
        wnd.set_autoSize(true);
        wnd.add_close(onClientCloseForShippingAddress);
        wnd.center();
        return false;
        //}
    });


    function onClientCloseForShippingAddress(oWnd, args) {
        var arg = args.get_argument();
        if (arg != null) {
            $('input[id$=txtCustomerName]').val($('input[id$=txtCustName]').val());
            $('[id$=txtAddress1]').val(arg.address1);
            $('[id$=txtAddress2]').val(arg.address2);
            $('[id$=txtAddress3]').val(arg.address3);
            $('[id$=txtCity]').val(arg.city);
            $('[id$=txtState]').val(arg.state);
            $('[id$=txtZipCode]').val(arg.zipCode);
            $('[id$=txtCountry]').val(arg.country);
            $('[id$=txtTelephone]').val(arg.telephone1);
            $('[id$=txtEmail]').val(arg.email);

            // updating left hand side panel for shipping address labels
            updateLeftPanelShippingAddress();
        }
    }


    // updating shipping address values from billing address if it is same 
    $('[id$=cbShipping]').click(function () {
        if ($(this).is(':checked')) {
            $('[id$=txtCustomerName]').val($('[id$=txtCustName]').val());
            $('[id$=txtAddress1]').val($('[id$=lblAddress1]').text());
            $('[id$=txtAddress2]').val($('[id$=lblAddress2]').text());
            $('[id$=txtAddress3]').val($('[id$=lblAddress3]').text());
            $('[id$=txtTelephone]').val($('[id$=txtPhone]').val());
            $('[id$=txtCity]').val($('[id$=lblCity]').text());
            $('[id$=txtState]').val($('[id$=lblState]').text());
            $('[id$=txtZipCode]').val($('[id$=lblZipCode]').text());
            $('[id$=txtCountry]').val($('[id$=lblCountry]').text());
            $('[id$=txtEmail]').val($('[id$=txtBEmail]').val());
        } else {
            $('[id$=txtCustomerName]').empty();
            $('[id$=txtAddress1]').empty();
            $('[id$=txtAddress2]').empty();
            $('[id$=txtAddress3]').empty();
            $('[id$=txtTelephone]').empty();
            $('[id$=txtCity]').empty();
            $('[id$=txtState]').empty();
            $('[id$=txtZipCode]').empty();
            $('[id$=txtCountry]').empty();
            $('[id$=txtEmail]').empty();
        }
        // updating left hand side panel for shipping address labels
        updateLeftPanelShippingAddress();
    });

    function updateLeftPanelShippingAddress() {
        // updating left hand side panel for shipping address labels
        $('[id$=txtCustomerName],[id$=txtAddress1],[id$=txtAddress2],[id$=txtAddress3],[id$=txtCountry],[id$=txtTelephone],[id$=txtEmail]').each(function () {
            var id = $(this).attr('for');
            $('[id$=' + id + ']').html($(this).val());
        });

        $('[id$=LblSACityStateZip]').html($('[id$=txtCity]').val() + ", " + $('[id$=txtState]').val() + ", " + $('[id$=txtZipCode]').val());
    }


    // credit card confirm validation here
    $('[id$=BtnCreditCardProcess]').click(function () {
        /*
        var result = $('form').validate().element($('[id$=ddlCreditCardType]'));
        if (!result)
        return false;

        result = $('form').validate().element($('[id$=txtExpirationDate]'));

        if (!result)
        return false;
        
        result = $('form').validate().element($('[id$=txtCreditCardNo]'));
        if (!result)
        return false;

        result = $('form').validate().element($('[id$=txtCVN]'));
        if (!result)
        return false;

        return true;*/
    });


    // validation process order
    $('[id$=btnProcessOrder]').click(function () {
        var result = $('form').validate().element($('[id$=ddlCreditCardType]'));
        if (!result) {
            $(".creditCardAccordion").trigger('click');
            return false;
        }
        result = $('form').validate().element($('[id$=txtExpirationDate]'));
        if (!result) {
            $(".creditCardAccordion").trigger('click');
            return false;
        }
        result = $('form').validate().element($('[id$=txtCreditCardNo]'));
        if (!result) {
            $(".creditCardAccordion").trigger('click');
            return false;
        }
        result = $('form').validate().element($('[id$=txtCVN]'));
        if (!result) {
            $(".creditCardAccordion").trigger('click');
            return false;
        }
        return true;
    });


    // back button handling here
    $('[id$=BtnOrderConfirmationBack]').click(function () {
        $('.creditCardAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnCreditCardBack]').click(function () {
        $('.OffLinesAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnOfferLineBack]').click(function () {
        $('.ShippingAddressAccordion').trigger('click');
        return false;
    });

    $('[id$=BtnShippingBack]').click(function () {
        $('.BillingAddressAccordion').trigger('click');
        return false;
    });


    $('#RadGridOfferLines').show();

    $('.NextPrevAndNumeric *').show();

    // accordation code here
    var activeIndex = parseInt($('input[id$=hidAccordionIndex]').val());
    $('.accordion h3').eq(activeIndex).trigger('click');
    //$('.accordion h3').eq(2).trigger('click'); // temporarly on grid tab

    // calculating prise on selected quantity on grid
    $('[id$=RadGridOfferLines] .currencyfield').keyup(function () {
        var row = $(this).parents('tr').eq(0);
        calculateTotals(row);
    });

    function validateNumber(o) {
        if (o > 0) {
            o = o.replace(/[^\d]+/g, ''); //Allow only whole numbers
        }
        return o;
    }
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

    // calculating on initial loading of the page
    $('[id$=RadGridOfferLines]').find('tr').each(function () {
        calculateTotals($(this));
    });


    function calculateTotals(row) {
        if (row.find('.currencyfield').size() > 0) {
            var val = row.find('.currencyfield').val();
            var totalElement = row.find('td:last');
            val = validateNumber(val);
            var price = row.find('td').eq(3).text().replace("$", "");
            var sub = (parseFloat(price) * parseFloat(val));
            if (isNaN(sub)) {
                totalElement.html("0.00");
            } else {
                totalElement.html(formatToMoney(sub, "$", "."));
            }
            calculatSubTotal();
        }
    }

    function calculatSubTotal() {
        var total = 0;
        $('[id$=RadGridOfferLines] tbody tr').each(function () {
            var lastTd = $(this).find('td:last');
            if (lastTd.size() > 0) {
                var value = parseFloat(lastTd.text().replace("$", ""));
                if (value != undefined && value != NaN && !isNaN(value))
                    total += value;
            }
        });
        $('input[id$=lblTotal]').val(formatToMoney(total, "$", "."));
        $('[id$=LblOrderSubtotal]').html(formatToMoney(total, "$", "."));
        calculatGrandTotal();
    }

    $('input[id$=txtShipping],input[id$=txtADonation]').keyup(function () {
        calculatGrandTotal();
    });

    function calculatGrandTotal() {
        var total = parseFloat($('input[id$=lblTotal]').val().replace("$", ""));
        if ($('input[id$=txtShipping]').val() != "") {
            var shippingAmount = parseFloat($('input[id$=txtShipping]').val().replace("$", ""));
            $('[id$=LblShippingTotal]').html(formatToMoney(shippingAmount, "$", "."));
            total += shippingAmount;
        }
        if ($('input[id$=txtADonation]').val() != "") {
            var donationTotal = parseFloat($('input[id$=txtADonation]').val().replace("$", ""));
            $('[id$=LblAdditionalDonation]').html(formatToMoney(donationTotal, "$", "."));
            total += donationTotal;
        }
        $('input[id$=lblGrandTotal]').val(formatToMoney(total, "$", "."));
        $('input[id$=TxtTotalOrder]').val(formatToMoney(total, "$", "."));
        $('[id$=LblTotalAmount]').html(formatToMoney(total, "$", "."));
    }



    // allow only numbers for specified fields
    $("[id$=TxtQuantity],[id$=txtCreditCardNo],[id$=txtExpirationDate],[id$=txtCVN]").keydown(function (event) {
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
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=txtCustName],[id$=txtPhone],[id$=txtBEmail]').change(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=lblAddress1],[id$=lblAddress2],[id$=lblAddress3]').change(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).text());
    });

    $('[id$=lblCity],[id$=lblState],[id$=lblZipCode]').change(function () {
        $('[id$=LblBCityStateZip]').html($('[id$=lblCity]').text() + ", " + $('[id$=lblState]').text() + ", " + $('[id$=lblZipCode]').text());
    });


    // updating shipping address at left hand side panel
    $('[id$=txtCustomerName],[id$=txtAddress1],[id$=txtAddress2],[id$=txtAddress3],[id$=txtCountry],[id$=txtTelephone],[id$=txtEmail]').keyup(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=txtCustomerName],[id$=txtAddress1],[id$=txtAddress2],[id$=txtAddress3],[id$=txtCountry],[id$=txtTelephone],[id$=txtEmail]').change(function () {
        var id = $(this).attr('for');
        $('[id$=' + id + ']').html($(this).val());
    });

    $('[id$=txtCity],[id$=txtState],[id$=txtZipCode]').keyup(function () {
        $('[id$=LblSACityStateZip]').html($('[id$=txtCity]').val() + ", " + $('[id$=txtState]').val() + ", " + $('[id$=txtZipCode]').val());
    });

    $('[id$=txtCity],[id$=txtState],[id$=txtZipCode]').change(function () {
        $('[id$=LblSACityStateZip]').html($('[id$=txtCity]').val() + ", " + $('[id$=txtState]').val() + ", " + $('[id$=txtZipCode]').val());
    });

});











