function searchByEnter() {
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

function sortAndFilterProjectList(property) {
    resetActiveSortButton(property);
    filterAndSortProjectListLogic();
}

function resetActiveSortButton(property) {
    dropCurrentActiveSortButton();
    setActiveSortButton($("#" + property + "-sort-btn"));
}

function dropCurrentActiveSortButton() {
    var activeSortButtons = $(".active");
    for (var i = 0; i < activeSortButtons.length; i++) {
        activeSortButtons[i].classList.add('btn-sm');
        activeSortButtons[i].classList.remove('active');
    }
}

function setActiveSortButton(button) {
    button.removeClass('btn-sm');
    button.addClass('active');
}

function filterAndSortProjectList() {
    $(document).ready(function () {
        $(".filter").change(function () {
            filterAndSortProjectListLogic();
        });
    });
}

function filterAndSortProjectListLogic() {
    var sortProperty = $(".active")[0].dataset.property;
    var dateStartFromFilter = $("#date-start-from").val();
    var dateStartToFilter = $("#date-start-to").val();
    var priorityFilter = $("#priority-filter").val();
    var managerFilter = $("#manager-filter").val();

    $.ajax({
        url: '/Project/GetFilteredAndSortedProjectList',
        type: 'Post',
        data: {
            sortProperty,
            dateStartFromFilter,
            dateStartToFilter,
            priorityFilter,
            managerFilter
        },
        success: function (html) {
            $("#target-div").empty();
            $("#target-div").append(html);
        },
        error: function (XmlHttpRequest) {
            alert('Произошла ошибка');
            console.log(XmlHttpRequest);
        }
    });
    return false;
}