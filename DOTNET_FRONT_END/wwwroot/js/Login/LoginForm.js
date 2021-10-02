'use strict'

$(function () {
    $('input, select').on('focus', function () {
        $(this).parent().find('.input-group-text').css('border-color', '#80bdff');
    });
    $('input, select').on('blur', function () {
        $(this).parent().find('.input-group-text').css('border-color', '#ced4da');
    });
});

$(function () {
    let $formData;
    let $url;
    const $handlerForm = $('#formID');
    $handlerForm.on('submit', function (e) {
        e.preventDefault();

    }).validate({
       
        rules: {
            username: {
                required: true
            },
            password: {
                required: true
            } 
        },
        messages: {
            username: 'Required Username',
            password: 'Required Password'
        },
        submitHandler: function () {
            var $username = $('#username').val();
            var $password = $('#password').val();
            $url = $handlerForm.attr('action');
            $.ajax({
                type: 'POST',
                url: $url,
                data: { user: $username, pass: $password},
                success: function (response, textStatus, xhr) {
                    if (xhr.status === 201) {
                        if (response.status === 30) {
                            Swal.fire({
                                text: response.message,
                                title: response.title,
                                icon: response.type,
                                allowOutsideClick: false
                            });
                        }    
                    } else {

                    }
                }, error: function (response) {

                }
            });
        }

    });
});