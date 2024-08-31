function OpenEntryForm(categoryId) {
    $(".floating-form-cont").addClass("active");
    window.disableSiteScrolling();
    $.ajax({
        type: 'GET',
        url: '@Url.Action("AddProfileEntry")',
        data: { category: categoryId },
        success: function (result) {
            $(".floating-form-cont").append(result);
            $(".floating-form-cont").find("#cancel-btn").click(function () {
                HideForm();
            });
        },
        error: function () {
            alert("Error trying to retrieve AddProfileEntry");
        }
    })
}

$(document).ajaxSuccess(function (event, xhr, settings) {
    if (settings.url === '@Url.Action("AddProfileEntry")') {
        var response = xhr.responseJSON;
        LoadProfileEntries(response.category);
        HideForm();
    }
});

function HideForm() {
    $(".floating-form-cont").empty();
    $(".floating-form-cont").removeClass("active");
    window.enableSiteScrolling();
}

function LoadProfileEntries(categoryId) {
    $.ajax({
        type: 'GET',
        url: '@Url.Action("GetViewRecordByCategory")',
        data: { category: categoryId },
        success: function (result) {
            let container;
            if (categoryId == @((int)(EntryCategory.Education)))
                container = "#education-list";
            else container = "#experience-list";
            $(container).empty();
            $(container).append(result);
        },
        error: function () {
            alert("Error trying to retrieve Entries");
        }
    })
}