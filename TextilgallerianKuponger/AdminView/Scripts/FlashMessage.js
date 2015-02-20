window.addEventListener('load', function () {
    var closeButton = document.querySelector('.close');

    if (closeButton) {
        closeButton.addEventListener('click', function () {
            closeButton.parentNode.parentNode.removeChild(closeButton.parentNode);
        });
    }
});