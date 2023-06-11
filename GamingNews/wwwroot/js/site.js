// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

function getEmpty(URL) {
    return jQuery.ajax({
        url: URL,
        type: "GET",
        cache: false,
        statusCode: {
            401: function () {
                redirectToLogin(urlLogin);
            }
        },
        complete: function () {
            $("#preloader").fadeOut('fast');
        },
        error: function () {
            $("#preloader").fadeOut('fast');
        }
    });
}

$(document).ready(function () {
    var down = false;

    $('#bell').click(function (e) {
        var color = $(this).text();
        if (down) {
            $('#box').css('height', '0px');
            $('#box').css('opacity', '0');
            $('#box').css('display', 'none');
            down = false;
        } else {
            $('#box').css('height', 'auto');
            $('#box').css('opacity', '1');
            $('#box').css('display', 'block');
            down = true;
        }
    });

    $("#markAsReadForm").submit(function (e) {
        e.preventDefault();

        var form = $(this);
        $.ajax({
            type: "POST",
            url: "Notification/MarkAllAsRead",
            data: form.serialize(), // serializes the form's elements.
            success: function (result) {
                if (result == true) {
                    //$("#notification-panel").load(location.href + " #notification-panel>*", "");
                    location.reload();
                }
                else {
                }
            }
        });
    });

    $("#markAsRead").click(function () {
        $("#markAsReadForm").submit();
    })

});