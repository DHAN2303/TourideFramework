$(() => {
    const span = document.getElementById('kt_quick_user_togglee');
    const firstLetter = document.getElementById('name_first_letter');

    $.ajax({
        cache: false,
        url: "/Home/GetUser",
        type: 'GET',
        success: function (response) {

            span.textContent = response;
            firstLetter.textContent = response.charAt(0);// first letter of name

        },
        error: function (er) {
            span.textContent = '';
            firstLetter.textContent = '';

            alert('Error! Kullanıcı bilgisi getirilemedi.' + er);
        }
    });

    $("#loadPanel").dxLoadPanel({
        shadingColor: 'rgba(0,0,0,0.4)',
        visible: false,
        showIndicator: true,
        showPane: true,
        shading: true,
        hideOnOutsideClick: false,
    }).dxLoadPanel('instance');
});