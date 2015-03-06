window.addEventListener('load', function () {
   
    var dropdown = document.querySelector('.type');
    var disposable = document.querySelector('.disposable');
    var specificFields = document.querySelectorAll('.specific-field');
    var campaignFields = document.querySelectorAll('.campaign-code');
    var disposableFields = document.querySelectorAll('.disposable-codes');

    function showCorrectFields() {
        var value = dropdown.options[dropdown.selectedIndex].value;
        var type = value.replace('Domain.Entities.', '');

        Array.prototype.forEach.call(specificFields, function (field) {
            if (!field.classList.contains(type)) {
                var input = field.querySelector('input');
                if (input.getAttribute('required')) {
                    input.setAttribute('should-be-required', 'required');
                    input.removeAttribute('required');
                }
                field.hidden = true;
            }
        });
    
        //Stop Form Submission of Enter Key Press
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;

        var currentFields = document.getElementsByClassName(type);

        Array.prototype.forEach.call(currentFields, function (field) {
            field.hidden = false;
            var input = field.querySelector('input');
            if (input.getAttribute('should-be-required')) {
                input.setAttribute('required', 'required');
                input.removeAttribute('should-be-required');
            }
        });

        Array.prototype.forEach.call(campaignFields, function (field) {
            field.hidden = disposable.checked;
            var input = field.querySelector('input');
            if (input.getAttribute('required')) {
                input.setAttribute('should-be-required', 'required');
                input.removeAttribute('required');
            }
        });
        Array.prototype.forEach.call(disposableFields, function (field) {
            field.hidden = !disposable.checked;
            var input = field.querySelector('input');
            if (input.getAttribute('should-be-required')) {
                input.setAttribute('required', 'required');
                input.removeAttribute('should-be-required');
            }
        });
    }

    dropdown.addEventListener('change', showCorrectFields);
    disposable.addEventListener('click', showCorrectFields);
    disposable.addEventListener('change', showCorrectFields);

    showCorrectFields();
});


var usercontent = [];
function userSearch() {
    $("#user-search").search({
        source: usercontent,
        searchFullText: false,
        cache: false
    });
}
$("#userinput").click(function () {
    usercontent = [];
    var users = jQuery("textarea#users").val();
    users = users.split("\n");
    usercontent = usercontent.concat(users);
    for (var i = 0; i < usercontent.length; i++) {
        usercontent[i] = { title: usercontent[i] };
    }
    userSearch();
});





