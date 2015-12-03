function showAllTabs() {
    $('#allchanges').addClass('active');
    $('.tab-pane').each(function (i, t) {
        $('#tabs li').removeClass('active');
        $(this).addClass('active');
    });
}

$(document).ready(function() {
    $('#allchanges').click(showAllTabs);
})