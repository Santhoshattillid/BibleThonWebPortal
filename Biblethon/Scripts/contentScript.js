
$(function () {
    var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

    $("#accordion").accordion({
        event: "click",
        autoHeight: false,
        active: activeIndex
    });
    if ($('#MainContent_lblZipCode').text() != '') {
        getBillingAddress();
    }
    if ($('#MainContent_txtZipCode').val() != '') {
        getShippingAddress();
    }
    if ($('#MainContent_lblGrandTotal').text() != '') {
        GetOfferLines();
    }
    CalculateTotals();
});

function AccordianHid(val1) {
    //debugger;
    var i = parseInt(val1.id);
    $("#<%=hidAccordionIndex.ClientID %>").val(i);
}
function popModel() {
    $("#popUpShow").css('display', 'block');
    $("#dialog-confirm").dialog({
        resizable: true,
        height: 600,
        width: 800,
        modal: true,
        buttons: {
            "Delete all items": function () {
                $(this).dialog("close");
            },
            Cancel: function () {
                $("#popUpShow").css('display', 'none');
                $(this).dialog("close");
            }
        }
    });
}

function GetSelectedRow(lnk) {
    // debugger;
    var row = lnk.parentNode.parentNode;
    var rowIndex = row.rowIndex - 1;
    var id = document.getElementById('MainContent_gvCustomers_customerId_' + rowIndex).value;
    $("#<%=hidCustId.ClientID %>").val(id);
    //alert($("#<%=hidCustId.ClientID %>").val())
    return false;
}
function IsValidate() {
    var findByName = document.getElementById('txtName').value;
    var findByAddr = document.getElementById('txtAddAndPh').value;
    if (findByName == '') {
        alert("Find is Required");
        return false;
    }
    else if (findByAddr == '') {
        alert("Refine Search By is Required");
        return false;
    }
    else {
        return true;
    }
}

function IsValidateFor() {
    var name = $('#MainContent_txtCustName').val();
    var phone = $('#MainContent_txtPhone').val();
    if (name == '') {
        alert("Name is Required");
        return false;
    }
    else if (phone == '') {
        alert("Telephone Number is Required");
        return false;
    }
    else {
        return true;
    }
}

function GetBack(anchor) {
    //debugger;
    var i = parseInt($("#<%=hidAccordionIndex.ClientID %>").val()) - 1;
    $("#<%=hidAccordionIndex.ClientID %>").val(i);
}

function getBillingAddress() {
    //debugger;
    var name = $('#MainContent_txtCustName').val();
    var add1 = $('#MainContent_lblAddress1').text();
    var add2 = $('#MainContent_lblAddress2').text();
    var add3 = $('#MainContent_lblAddress3').text();
    var city = $('#MainContent_lblCity').text();
    var state = $('#MainContent_lblState').text();
    var zip = $('#MainContent_lblZipCode').text();
    var country = $('#MainContent_lblCountry').text();
    var phone = $('#MainContent_txtPhone').val();
    var email = $('#MainContent_txtBEmail').val();
    $("#divBilling").html('<p>' + name + '<br/>' + add1 + '<br/>' + city + ',' + state + ',' + zip + '<br/>' + country + '<br/>T:' + phone + '<br/>E:' + email + '</p>');
    return true;
}

function getShippingAddress() {
    //debugger;
    var name = $('#MainContent_txtCustomerName').val();
    var add1 = $('#MainContent_txtAddress1').val();
    var add2 = $('#MainContent_txtAddress2').val();
    var add3 = $('#MainContent_txtAddress3').val();
    var city = $('#MainContent_txtCity').val();
    var state = $('#MainContent_txtState').val();
    var zip = $('#MainContent_txtZipCode').val();
    var country = $('#MainContent_txtCountry').val();
    var phone = $('#MainContent_txtTelephone').val();
    var email = $('#MainContent_txtEmail').val();
    $("#divShipping").html('<p>' + name + '<br/>' + add1 + '<br/>' + city + ',' + state + ',' + zip + '<br/>' + country + '<br/>T:' + phone + '<br/>E:' + email + '</p>');
    return true;
}

function GetOfferLines() {
    var sub = $('#MainContent_lblTotal').text().substring(1);
    var subtotal = parseFloat(sub);
    var shipping, donation;
    if ($("#MainContent_txtShipping").val() == '')
        shipping = 0;
    else
        shipping = parseFloat($("#MainContent_txtShipping").val());
    if ($("#MainContent_txtADonation").val() == '')
        donation = 0;
    else
        donation = parseFloat($("#MainContent_txtADonation").val());

    var grandtotal = subtotal + shipping + donation;
    $("#MainContent_lblGrandTotal").text(grandtotal);
    $("#divOfferLines").html('<p>Shipping Total: ' + shipping + '<br/>Order Subtotal: ' + subtotal + '<br/>Additional Donation: ' + donation + '<br/>Total: ' + grandtotal + '</p>');
    return true;
}

//mani

function CalculateTotals() {
    var gv = document.getElementById("<%= gdvOfferLine.ClientID %>");
    var tb = gv.getElementsByTagName("input");
    var lb = gv.getElementsByTagName("span");

    var sub = 0;
    var total = 0;
    var indexQ = 1;
    var indexP = 0;
    var price = 0;
    var qty = 0;
    var totalQty = 0;

    for (var i = 0; i < tb.length; i++) {
        if (tb[i].type == "text") {
            ValidateNumber(tb[i]);

            price = lb[indexP].innerHTML.replace("$", "").replace(",", "");
            sub = parseFloat(price) * parseFloat(tb[i].value);
            if (isNaN(sub)) {
                lb[i + indexQ].innerHTML = "0.00";
                sub = 0;
            }
            else {
                lb[i + indexQ].innerHTML = FormatToMoney(sub, "$", ",", "."); ;
            }

            indexQ++;
            indexP = indexP + 2;

            if (isNaN(tb[i].value) || tb[i].value == "") {
                qty = 0;
            }
            else {
                qty = tb[i].value;
            }

            totalQty += parseInt(qty);
            total += parseFloat(sub);
        }
    }

    //document.getElementById('MainContent_LBLTotal1').value = FormatToMoney(total, "$", ",", ".");
    $('#MainContent_lblTotal').text(FormatToMoney(total, "$", "."));
    calculatGranTotal();
}

function ValidateNumber(o) {
    if (o.value.length > 0) {
        o.value = o.value.replace(/[^\d]+/g, ''); //Allow only whole numbers
    }
}
function isThousands(position) {
    if (Math.floor(position / 3) * 3 == position) return true;
    return false;
};

function FormatToMoney(theNumber, theCurrency, theDecimal) {
    var theDecimalDigits = Math.round((theNumber * 100) - (Math.floor(theNumber) * 100));
    theDecimalDigits = "" + (theDecimalDigits + "0").substring(0, 2);
    theNumber = "" + Math.floor(theNumber);
    var theOutput = theCurrency;
    for (x = 0; x < theNumber.length; x++) {
        theOutput += theNumber.substring(x, x + 1);
        //                if (isThousands(theNumber.length - x - 1) && (theNumber.length - x - 1 != 0)) {
        //                    theOutput += theThousands;
        //                };
    };
    theOutput += theDecimal + theDecimalDigits;
    return theOutput;
}

function calculatGranTotal() {

    var sub = $('#MainContent_lblTotal').text().substring(1);
    var subtotal = parseFloat(sub);
    var shipping, donation;
    if ($("#MainContent_txtShipping").val() == '')
        shipping = 0;
    else
        shipping = parseFloat($("#MainContent_txtShipping").val());
    if ($("#MainContent_txtADonation").val() == '')
        donation = 0;
    else
        donation = parseFloat($("#MainContent_txtADonation").val());

    var grandtotal = subtotal + shipping + donation;
    $("#MainContent_lblGrandTotal").text(grandtotal);
}
        
