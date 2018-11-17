﻿function searchByEnter() {
    $(document).ready(function () {
        $('#search-input-value').keyup(function (event) {
            if (event.keyCode === 13) {
                searchEmployees();
            }
        });
    });
}

function searchEmployees() {
    var employeeName = $('#search-input-value').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var oldSearchingText = document.getElementById('searching').innerText;

    document.getElementById('searching').innerText = 'Идет поиск...';

    $.ajax({
        url: "/Employee/FindEmployees",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "name": employeeName
        },
        success: function (html) {
            document.getElementById('searching').innerText = oldSearchingText;
            $("#found-items").empty();
            $("#found-items").append(html);
        },
        error: function (XmlHttpRequest) {
            document.getElementById('searching').innerText = 'Ошибка';
            console.log(XmlHttpRequest);
        }
    });
    return false;
}

function clearSearch() {
    $("#found-items").empty();
    $("#search-input-value").val('');
}

function attachEmployee(employeeId) {
    if (document.body.contains(document.getElementById('pinned-' + employeeId))) {
        alert('Сотрудник уже прикреплен.');
        return false;
    }

    var projectId = $("#projectId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Project/AttachEmployee',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            "projectId": projectId,
            "employeeId": employeeId
        },
        success: function (html) {
            $('#no-employees-message').remove();
            $('#attached-employees').append(html);
        },
        error: function (XmlHttpRequest) {
            console.log(XmlHttpRequest);
        }
    });
    return false;
}

function detachEmployee(employeeId) {
    var projectId = $("#projectId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Project/DetachEmployee',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            "projectId": projectId,
            "employeeId": employeeId
        },
        success: function (message) {
            if (message === "success") {
                $("#pinned-" + employeeId).remove();
            } else if (message === "fail") {
                alert("Ошибка. Попробуйте еще раз.");
            }
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Попробуйте еще раз.');
            console.log(XmlHttpRequest);
        }
    });
    return false;
}