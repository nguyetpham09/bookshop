var cart = {
    
    init: function () {
        cart.loadData();
        cart.registerEvent();
        rates = [];
    },
    registerEvent: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });
        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {

                var amount = quantity * price;

                $('#amount_' + productid).text(numeral(amount).format('0,0'));
            }
            else {
                $('#amount_' + productid).text(0);
            }

            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));


            cart.updateAll();

        });
        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
        });
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }

        });

        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
            $("#tblRate").html("");
            $("#sltWard").val("---");
        });
        $("#sltCity").on("change", function () {
            $("#tblRate").html("");
            var value = $(this).val();
            $.ajax({
                url: 'api/delivery_partner/districts/'+value,
                type: 'GET',
                dataType: 'json',
                /*headers: {
                    Token: '89ab5325-7280-11ec-ac64-422c37c6de1b'
                },*/
                success: function (response) {

                        var districts = response;
                    var html = "<option value ='---'>Quận/ Huyện</option>"
                    for (var index in districts) {
                            html += "<option value ='" + districts[index].id + "'>" + districts[index].name + "</option>";
                        }
                        $("#sltDistrict").html(html);
                    
                }
            });
        });
        $("#sltDistrict").on("change", function () {
            $("#tblRate").html("");

            var value = $(this).val();
            $.ajax({
                url: 'api/delivery_partner/wards/'+value,
                type: 'GET',
                //dataType: 'json',
                /*headers: {
                    Token: '89ab5325-7280-11ec-ac64-422c37c6de1b'
                },*/
                success: function (response) {

                        var districts = response;
                    var html = "<option value ='---'>Xã/ Phường</option>"
                        for (var index in districts) {
                            html += "<option value ='" + districts[index].id + "'>" + districts[index].name + "</option>";
                        }
                    $("#sltWard").html(html);
                    
                }
            });
        });
        $("#sltWard").on("change", function () {
            $("#tblRate").html("");
            var requestData = new Object();
            requestData.address_to = new Object();
            requestData.address_to.district = $("#sltDistrict").val();
            requestData.address_to.city = $("#sltCity").val();
            requestData.address_to.ward = $(this).val();
            requestData.parcel = new Object();
            requestData.parcel.weight = 300;
            if ($('input[name="paymentMethod"]:checked').val() == 'CASH') {
                requestData.parcel.cod = cart.getTotalOrder();
            }

            $.ajax({
                url: 'api/delivery_partner/rates',
                type: 'POST',
                dataType: 'json',
                data: requestData,
                /*headers: {
                    Token: '89ab5325-7280-11ec-ac64-422c37c6de1b'
                },*/
                success: function (response) {
                    this.rates = response;
                    var html = "";
                    var rateLayout = "<tr id='{id}'> <td> {carrier_name}</td>";
                    rateLayout += "<td class ='service'>{service}</td>";
                    rateLayout += "<td class ='expected'>{expected}</td>";
                    rateLayout += "<td class ='total_amount cod_fee' value ='{total_amount_value}' cod_fee='{cod_fee}'>{total_amount}</td> ";
                    rateLayout += "<td><input type = 'radio' value ='{id}' rate_id ='{rate_id}' name='selectedService'></td> </tr>";


                    for (var index in this.rates) {

                        var rateItem = rateLayout.split("{carrier_logo}").join(this.rates[index].carrier_logo);
                        rateItem = rateItem.split("{carrier_name}").join( this.rates[index].carrier_name);
                        rateItem = rateItem.split("{service}").join(this.rates[index].service);
                        rateItem = rateItem.split("{expected}").join(this.rates[index].expected);
                        rateItem = rateItem.split("{total_amount}").join(this.rates[index].total_amount.toLocaleString());
                        rateItem = rateItem.split("{total_amount_value}").join(this.rates[index].total_amount);
                        rateItem = rateItem.split("{cod_fee}").join(this.rates[index].cod_fee);
                        rateItem = rateItem.split("{rate_id}").join(this.rates[index].id);
                        rateItem = rateItem.split("{id}").join(index);
                            html += rateItem;
                        }
                    $("#tblRate").html(html);

                   // cart.initRadioEvent();
                    $('input:radio[name="selectedService"]').change(
                        function () {
                            if ($(this).is(':checked')) {
                                // append goes here
                                var value = $(this).val();
                                var total_amount = $("#" + value + " .total_amount").attr("value");
                                var expected = $("#" + value + " .expected").text();
                                var cod_fee = $("#" + value + " .cod_fee").attr("cod_fee");
                                $('#lblTotalOrder').text(cart.getTotalOrder().toLocaleString());
                                $('#lblTotalOrder').attr("value" , cart.getTotalOrder());
                                $("#lblDelivery").text( expected );
                                $("#lblCodFee").text(parseInt(cod_fee).toLocaleString());
                                $('#lblCodFee').attr("value", cod_fee);
                                $("#lblPaymentAmount").text((cart.getTotalOrder() + parseInt(total_amount)).toLocaleString());
                                $("#lblPaymentAmount").attr("value", cart.getTotalOrder() + parseInt(total_amount));
                            }
                        });
                }
            });
        });

        
    },
    
    initRadioEvent: function() {
        
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    createOrder: function () {

        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[groupname="bankcode"]:checked').prop('id'),
            Status: false,
            CustomerAddressDistrict : $("#sltDistrict").val(),
            CustomerAddressCity : $("#sltCity").val(),
            CustomerAddressWard : $("#sltWard").val(),
            Weight: 300,
            OrderAmount : parseInt($('#lblTotalOrder').attr("value")),
            CodFee: parseInt($('#lblCodFee').attr("value")),
            Total: parseInt($("#lblPaymentAmount").attr("value")),
            RateId: $('input:radio[name="selectedService"]:checked').attr("rate_id")
        }
        if (order.RateId == undefined || order.RateId == "") {
            alert("Vui lòng chọn dịch vụ vận chuyển");
            return;
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {
                        
                        window.location.href = response.urlCheckout;
                    }
                    else {
                        console.log('create order ok');
                        $('#divCheckout').hide();
                        cart.deleteAll();
                        setTimeout(function () {
                            $('#cartContent').html('Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                        }, 2000);
                    }

                }
                else {
                    $('#divMessage').show();
                    $('#divMessage').text(response.message);
                }
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();

                }
            }
        });
    },

    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update ok');
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.Price,
                            PriceF: numeral(item.Product.Price).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Quantity * item.Product.Price).format('0,0')
                        });
                    });

                    $('#cartBody').html(html);

                    if (html == '') {
                        $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng.');
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();