﻿$(document).ready(function () {

    $('.num').click(function () {
        var num = $(this);
        var text = $.trim(num.find('.txt').clone().children().remove().end().text());
        var telNumber = $('#telNumber');
        $(telNumber).val(telNumber.val() + text);
    });

    $('.btn-group .btn').click(function () {
        $(this).val();
        var amount = $(this).val();
        var CashInserted = $('#CashInserted');
        $(CashInserted).val(Math.round(((+CashInserted.val()) + (+amount)) * 100) / 100);
    });

});