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
