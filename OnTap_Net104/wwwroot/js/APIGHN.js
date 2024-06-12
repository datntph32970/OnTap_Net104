getProvinces();
async function getProvinces() {

    const response = await fetch('https://online-gateway.ghn.vn/shiip/public-api/master-data/province', {
        headers: {
            'Token': 'c5a07264-26a0-11ef-ad6a-e6aec6d1ae72',
        },
    });
    const data = await response.json();

    console.log(data.data);

    var provinceDropdown = document.getElementById("province");

    data.data.forEach(function (provinceObject) {
        var provinceId = provinceObject.ProvinceID;
        var provinceName = provinceObject.ProvinceName;
        var option = $('<option></option>').val(provinceId).text(provinceName);
        $('#province').append(option);
    });


}
async function getDistrict(provinceID) {
    $('#district').empty();
   
    if(provinceID == ""){
        $('#district').empty();
        var option = $('<option></option>').val(0).text("Chọn Quận/Huyện");
        $('#district').append(option);
        return;
    };
    const response = await fetch('https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=' + provinceID, {
        headers: {
            'Token': 'c5a07264-26a0-11ef-ad6a-e6aec6d1ae72',
        },
    });
    const data = await response.json();

    console.log(data.data);

    var districtDropdown = document.getElementById("district");

    data.data.forEach(function (districtObject) {
        var DistrictID = districtObject.DistrictID;
        var DistrictName = districtObject.DistrictName;
        var option = $('<option></option>').val(DistrictID).text(DistrictName);
        $('#district').append(option);
    });


}

async function getWard(districtID) {
    GetAvailableServices(districtID);
    $('#ward').empty();

    if (districtID == "") {
        $('#ward').empty();
        var option = $('<option></option>').val(0).text("Chọn Phường/Xã");
        $('#ward').append(option);
        return;
    };
    const response = await fetch('https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=' + districtID, {
        headers: {
            'Token': 'c5a07264-26a0-11ef-ad6a-e6aec6d1ae72',
        },
    });
    const data = await response.json();

    console.log(data.data);

    var wardDropdown = document.getElementById("ward");

    data.data.forEach(function (wardObject) {
        var WardCode = wardObject.WardCode;
        var WardName = wardObject.WardName;
        var option = $('<option></option>').val(WardCode).text(WardName);
        $('#ward').append(option);
    });


}
async function GetAvailableServices(toDistrictID) {
    $('#service').empty();
    var option = $('<option></option>').val(0).text("Chọn Dịch Vụ");

    $('#service').append(option);
    if (toDistrictID == "") {
        $('#service').empty();
        var option = $('<option></option>').val(0).text("Chọn Dịch Vụ");
        $('#service').append(option);
        return;
    }


    const response = await fetch('https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services?from_district=3440&shop_id=5123377&to_district='+toDistrictID, {
        method: 'GET',
        headers: {
            'Token': 'c5a07264-26a0-11ef-ad6a-e6aec6d1ae72',
        },
    });
    const data = await response.json();

    console.log(data.data);


    data.data.forEach(function (serviceObject) {
        var service_id = serviceObject.service_id;
        var short_name = serviceObject.short_name;
        var option = $('<option></option>').val(service_id).text(short_name);
        $('#service').append(option);
    });
}

async function totalTransportFee(service_id, insurance_value , to_district_id, to_ward_code, from_district_id) {
    const response = await fetch('https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Token': 'c5a07264-26a0-11ef-ad6a-e6aec6d1ae72',
        },
        body: JSON.stringify({
            "service_id": service_id,
            "service_type_id": 2,
            "coupon": "",
            "from_district_id": 3440,
            "to_district_id": to_district_id,
            "to_ward_code": to_ward_code,
            "weight": 1000,
            "length": 10,
            "width": 10,
            "height": 10,
            "insurance_value": insurance_value,
        })
        
    });
    const data = await response.json();

    console.log(data.data);

    var totalTransportFee = document.getElementById("totalTransportFee");
    totalTransportFee.innerHTML = data.data.total;
   

}
function update(id, quantity, status) {

    var productQuantity = document.getElementById('productQuatity_' + id);
    var productQuantityValue = parseInt(productQuantity.textContent)

    var CartQuantity = document.getElementById('Quantity_' + id);
    var errorQuatity = document.getElementById('error_' + id);
    var buttonPay = document.getElementById('buttonPay');

    if (quantity < 0 || quantity > productQuantityValue) {
        errorQuatity.hidden = false;
        buttonPay.disabled = true;
        console.log(quantity + "< 0 || " + quantity + '>' + productQuantityValue)

        return;
    }
    else {
        errorQuatity.hidden = true;
        buttonPay.disabled = false;
        console.log(quantity + "> 0 && " + quantity + '<' + productQuantityValue)
    }

    $.ajax({
        url: '/CartDetails/Edit',
        type: 'POST',
        data: { id: id, Quantity: quantity, Status: status },
        success: function (data) {
            var totalPriceElement = document.getElementById('total_' + id);
            var priceElement = document.getElementById('price_' + id);
            var price = parseFloat(priceElement.textContent);
            totalPriceElement.textContent = (price * quantity).toFixed(2);

            CartQuantity.value = quantity;
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi
        }
    });
}

function updateTrasportFeeForAllProduct() {
    const provinceDropdown = document.getElementById("province");
    const selectedProvinceValue = provinceDropdown.value;

    const districtDropdown = document.getElementById("district");
    const selectedDistrictValue = districtDropdown.value;

    const wardDropdown = document.getElementById("ward");
    const selectedWardValue = wardDropdown.value;

    const serviceDropdown = document.getElementById("service");
    const selectedServiceValue = serviceDropdown.value;

    $.ajax({
        url: '/CartDetails/UpdateTransportFeeForAllCartDetails',
        type: 'POST',
        data: { serviceId: selectedServiceValue, toDistrictId: selectedDistrictValue, toWardCode: selectedWardValue },
        success: function (data) {
            toastr.success('Update thành công', 'Success');
            setTimeout(function () {
                window.location.reload();
            }, 1000);
        },
        error: function (xhr, status, error) {
            toastr.error("Lỗi khi update", 'Lỗi');           
        }
    });
}

