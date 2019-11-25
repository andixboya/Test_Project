// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//js for CreateTicketInputModel
$('#Discount').on('input', function () {
    var newDiscount = $('#Discount').val();

    let childCount = Number($('#ChildCount').val());
    let adultCount = Number($('#AdultCount').val());
    let childPrice = Number($('#ChosenExcursion_PricePerChild').val());
    let adultPrice = Number($('#ChosenExcursion_PricePerAdult').val());
    let result = (childCount * childPrice + adultPrice * adultCount) * ((100.00 - newDiscount) / 100.00)
    $('#SumAfterDiscount').val(result.toFixed(2))
    //33
})
$('#ChildCount').on('input', function () {
    var newChildCount = $('#ChildCount').val();

    let discount = Number($('#Discount').val());
    let adultCount = Number($('#AdultCount').val());
    let childPrice = Number($('#ChosenExcursion_PricePerChild').val());
    let adultPrice = Number($('#ChosenExcursion_PricePerAdult').val());
    let result = (newChildCount * childPrice + adultPrice * adultCount) * ((100.00 - discount) / 100.00)
    let totalCount = Number(adultCount) + Number(newChildCount);
    let x = $('#TouristCount');
    $('#SumAfterDiscount').val(result.toFixed(2))
    $('#TouristCount').val(totalCount)

})
$('#AdultCount').on('input', function () {
    var newAdultCount = $('#AdultCount').val();

    let discount = Number($('#Discount').val());
    let childCount = Number($('#ChildCount').val());
    let childPrice = Number($('#ChosenExcursion_PricePerChild').val());
    let adultPrice = Number($('#ChosenExcursion_PricePerAdult').val());
    let result = (childCount * childPrice + adultPrice * newAdultCount) * ((100.00 - discount) / 100.00)
    let totalCount = Number(newAdultCount) + Number(childCount);
    $('#SumAfterDiscount').val(result.toFixed(2))
    $('#TouristCount').val(totalCount)
})

//js for CreateSaleInputModel
$('#CreditCard').on('input', function () {
    var newCreditSum = $('#CreditCard').val();
    var cashSum = $('#Cash').val();
    var newAcumulated = Number(newCreditSum) + Number(cashSum);

    $('#TotalAccumulated').val(newAcumulated.toFixed(2));
})

$('#Cash').on('input', function () {
    let newCashSum = $('#Cash').val();
    let creditSum = $('#CreditCard').val();
    let newAcumulated = Number(newCashSum) + Number(creditSum);

    $('#TotalAccumulated').val(newAcumulated.toFixed(2));
})

//js for TicketRefundCreate (model)
$('#ChildrenToRefund').on('input', function () {

    let childCountToRefund = $('#ChildrenToRefund').val();
    let adultCountToRefund = $('#AdultToRefund').val();

    let pricePerChild = $('#PricePerAdult').val();
    let pricePerAdult = $('#PricePerAdult').val();

    let newTotalSumToRefund = Number(childCountToRefund) * Number(pricePerChild) + Number(adultCountToRefund) * Number(pricePerAdult);
    $('#SumToRefund').val(newTotalSumToRefund.toFixed(2));

})
$('#AdultToRefund').on('input', function () {

    let childCountToRefund = $('#ChildrenToRefund').val();
    let adultCountToRefund = $('#AdultToRefund').val();

    let pricePerChild = $('#PricePerAdult').val();
    let pricePerAdult = $('#PricePerAdult').val();

    let newTotalSumToRefund = Number(childCountToRefund) * Number(pricePerChild) + Number(adultCountToRefund) * Number(pricePerAdult);
    $('#SumToRefund').val(newTotalSumToRefund.toFixed(2));

})  