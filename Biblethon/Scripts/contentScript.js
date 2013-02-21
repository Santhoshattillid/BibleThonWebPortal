
$(function () {

    // accordation script section 
    $('#accordion .divAccordian').hide();
    $('#accordion h3:first').addClass('active').next().slideDown('slow');
    $('#accordion h3').click(function () {
        if ($(this).next().is(':hidden')) {
            $('#accordion h3').removeClass('active').next().slideUp('slow');
            $(this).toggleClass('active').next().slideDown('slow');
        }
    });


    // billind address validation here
    $('input[id$=btnBillContinue]').click(function () {
        //validation starts here
        if ($('form').validate().element($('input[id$=txtCustName]')) && $('form').validate().element($('input[id$=txtPhone]')))
            return true;
        else
            return false;
    });

    // custom search for billing address
    $('#imgSearch').click(function () {
        //if ($('form').validate().element($('input[id$=txtCustName]')) && $('form').validate().element($('input[id$=txtPhone]'))) {
        var name = $('input[id$=txtCustName]').val();
        var telephone = $('input[id$=txtPhone]').val();
        var wnd = window.radopen("BillingAddress.aspx?name=" + name + "&telephone=" + telephone);
        wnd.setSize(950, 500);
        wnd.add_close(onClientClose);
        wnd.center();
        return false;
        //}
    });


    // getting customer information from rad window

    function onClientClose(oWnd, args) {
        var arg = args.get_argument();
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
    }


    // shipping address validation here
    $('.BtnshippingContinue').click(function () {

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

        return true;
    });


    // accordation code here
    var activeIndex = parseInt($('input[id$=hidAccordionIndex]').val());

    $("#accordion").accordion({
        event: "click",
        autoHeight: false,
        active: activeIndex
    });


    // calculating prise on selected quantity on grid
    $('input[id$=TXTQty]').keyup(function () {
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
    $('[id$=gdvOfferLine]').find('tr').each(function () {
        calculateTotals($(this));
    });


    function calculateTotals(row) {
        if (row.find('input[id$=TXTQty]').size() > 0) {
            var val = row.find('input[id$=TXTQty]').val();
            var totalElement = row.find('td:last').find('input[type=text]');
            val = validateNumber(val);
            var price = row.find('td').eq(3).text().replace("$", "");
            var sub = (parseFloat(price) * parseFloat(val));
            if (isNaN(sub)) {
                totalElement.val("0.00");
            } else {
                totalElement.val(formatToMoney(sub, "$", "."));
            }
            calculatSubTotal();
        }
    }

    function calculatSubTotal() {
        var total = 0;
        $('input[id$=LBLSubTotal]').each(function () {
            total += parseFloat($(this).val().replace("$", ""));
        });
        $('input[id$=lblTotal]').val(formatToMoney(total, "$", "."));
        calculatGrandTotal();
    }

    $('input[id$=txtShipping],input[id$=txtADonation]').keyup(function () {
        calculatGrandTotal();
    });

    function calculatGrandTotal() {
        var total = parseFloat($('input[id$=lblTotal]').val().replace("$", ""));
        if ($('input[id$=txtShipping]').val() != "")
            total += parseFloat($('input[id$=txtShipping]').val().replace("$", ""));
        if ($('input[id$=txtADonation]').val() != "")
            total += parseFloat($('input[id$=txtADonation]').val().replace("$", ""));
        $('input[id$=lblGrandTotal]').val(formatToMoney(total, "$", "."));
    }
});











