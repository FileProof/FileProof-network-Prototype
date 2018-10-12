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
            if (data.msg.length > 0) {
                $('#curUser').val(data.msg);
                $('#curUserNameLink').text(data.msg.substr(0,7) + '...');
                updatecontrols();
            }
        },
        error: function () {
            alert("signing in error");
        }
    });
}



function updatecontrols() {
    if ($('#curUser').val().length > 0) {

        $("#signInBtn").removeClass('show');
        $("#signInBtn").addClass('hide');
        $("#signInBtn").hide();

        $("#loginInput").hide();
        $("#loginInput").addClass('hide');
        $("#loginInput").removeClass('show');

        $("#curUserName").show();
        $("#curUserName").addClass('show');
        $("#curUserName").removeClass('hide');

        $("#docs").show();
        $("#docs").addClass('show');
        $("#docs").removeClass('hide');
        
        if ($("#curUser").val() == "0x0100000000000000000000000000000000000000000000000000000000000000") {
            $("#reset").show();
            $("#reset").addClass('show');
            $("#reset").removeClass('hide');
        }
        else {
            $("#reset").hide();
            $("#reset").addClass('hide');
            $("#reset").removeClass('show');
        }

        $("#signOutBtn").removeClass('hide');
        $("#signOutBtn").addClass('show');
        $("#signOutBtn").show();
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
        //$("#curUserNameLink").text('');

        $("#docs").hide();
        $("#docs").addClass('hide');
        $("#docs").removeClass('show');

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
        if (url.length > 0) {
            window.location.replace(url);
        }
        });        
}

function restart() {
    $.ajax({
        type: "POST",
        url: "/Contract/DeleteAll/",
        processData: false
    });

    logout("/Contract/Validate/");
}


//var ajaxFormSubmit = function (e) {
//    var $form = $(this);

//    var data = $form.serialize();
//    var options = {
//        url: $form.attr("action"),
//        type: $form.attr("method"),
//        data: data
//    };

//    $.ajax(options)
//        .done(function (data) {
//            var targerId = $form.attr("data-target");
//            if (targerId != undefined) {
//                var $target = $(targerId);
//                $target.replaceWith(data);
//            }
//            if (e.data) {
//                var o = e.data;
//                if (o.callback && (typeof o.callback == 'function')) {
//                    var func = o.callback;
//                    var input = o.input;
//                    func(input);
//                }
//            }
//        })
//        .fail(function (data) {
//            var message = '';
//            var response = JSON.parse(data.responseText);
//            if (response && response.errorMessage !== undefined) {
//                message = response.errorMessage;
//            }
//            new PNotify({
//                type: 'error',
//                title: 'Error on AJAX form submit',
//                text: message
//            });
//        });

//    return false;
//};
