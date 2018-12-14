// Write your JavaScript code.
var ajaxFormSubmit = function (e) {
    var $form = $(this);

    var formData = new FormData(this);//$form.serialize();
    var options = {
        url: $form.attr("action"),
        type: $form.attr("method"),
        data: formData,
        cache: false,
        contentType: false,
        processData: false
    };

    $.ajax(options)
        .done(function (data) {           
            if (e.data) {
                var o = e.data;                
                if (o.callback && (typeof o.callback == 'function')) {
                    var func = o.callback;
                    var btn = o.input;                    
                    func(btn, data );
                }
            }
        })
        .fail(function (data) {
            var message = '';
            var response = JSON.parse(data.responseText);
            if (response && response.errorMessage !== undefined) {
                message = response.errorMessage;
            }
            new PNotify({
                type: 'error',
                title: 'Error on AJAX form submit',
                text: message
            });
        });

    return false;
};

function login() {
    var data = JSON.stringify({ hash: $('#Login').val() });
    $.ajax({
        type: "POST",
        url: "/Auth/Login/",
        contentType: "application/json",
        dataType: "json",
        processData: false,               
        data: data,
        success: function (data){
            if (data.id.length > 0) {
                $('#curUser').val(data.id);
                $('#curUserNameLink').text(data.id.substr(0, 7) + '...');
                $('#curRoles').val(data.roles.join(','));
                updatecontrols();
            }
        },
        error: function () {
            alert("signing in error");
        }
    });
}



function updatecontrols() {  
    var curUser = $('#curUser');

    if (curUser.val()) {

        $("#signInBtn").hide();
        $("#signInBtn").removeClass('show');
        $("#signInBtn").addClass('hide');

        $("#loginInput").hide();
        $("#loginInput").addClass('hide');
        $("#loginInput").removeClass('show');

        $("#curUserName").show();
        $("#curUserName").addClass('show');
        $("#curUserName").removeClass('hide');

        $("#signOutBtn").show();
        $("#signOutBtn").removeClass('hide');
        $("#signOutBtn").addClass('show');            

        $("#validate").show();
        $("#validate").addClass('show');
        $("#validate").removeClass('hide');

        $("#docs").show();
        $("#docs").addClass('show');
        $("#docs").removeClass('hide');

        var roles = $('#curRoles').val().split(',');

        if (roles.includes('Admin')) {
            $("#reset").show();
            $("#reset").addClass('show');
            $("#reset").removeClass('hide');
        }
        else {
            $("#reset").hide();
            $("#reset").addClass('hide');
            $("#reset").removeClass('show');
        }
    } else {
        $("#signInBtn").removeClass('hide');
        $("#signInBtn").addClass('show');
        $("#signInBtn").show();
        
        $("#loginInput").show();
        $("#loginInput").addClass('show');
        $("#loginInput").removeClass('hide');

        $("#curUserName").hide();
        $("#curUserName").addClass('hide');
        $("#curUserName").removeClass('show');

        $("#docs").hide();
        $("#docs").addClass('hide');
        $("#docs").removeClass('show');

        $("#validate").hide();
        $("#validate").addClass('hide');
        $("#validate").removeClass('show');

        $("#reset").hide();
        $("#reset").addClass('hide');
        $("#reset").removeClass('show');

        $("#signOutBtn").removeClass('show');
        $("#signOutBtn").addClass('hide');
        $("#signOutBtn").hide();
    }
}

function logout(url) {
    $.ajax({
        type: "POST",
        url: "/Auth/Logout/",
        processData: false,
        error: function () {
            alert("signing out error");
        }
    }).done(function (data) {
        $('#curUser').val('');
        updatecontrols();
        if (url) {
            window.location.replace(url);
        }
        });        
}
