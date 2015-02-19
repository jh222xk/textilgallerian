window.addEventListener('load', function () {
    var dropdown = document.querySelector('.type');
    var specificFields = document.querySelectorAll('.specific-field');

    function showCorrectFields() {
        var value = dropdown.options[dropdown.selectedIndex].value;
        var type = value.replace('Domain.Entities.', '');

        Array.prototype.forEach.call(specificFields, function (field) {
            if (!field.classList.contains(type)) {
                field.hidden = true;
            }
        });

        var currentFields = document.getElementsByClassName(type);

        Array.prototype.forEach.call(currentFields, function (field) {
            field.hidden = false;
        });
    }

    dropdown.addEventListener('change', showCorrectFields);
    showCorrectFields();
});