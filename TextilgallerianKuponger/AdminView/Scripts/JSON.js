window.addEventListener('load', function () {
    var categories;
    var content="";
    (function ($) {
        var url = 'http://eerie.se/textil/kategorier.json?callback=?';

        $.ajax({
            type: 'GET',
            url: url,
            async: false,
            jsonpCallback: 'jsonCallback',
            contentType: "application/json",
            dataType: 'jsonp',
            success: function (json) {
                categories = json;
                categoryDropDown();
           },
            error: function (e) {
                console.log(e.message);
            }
        });
    })(jQuery);
    function categoryDropDown() {
        for (var key in categories) {
            var obj = categories["KATEGORIER"];
            for (var prop in obj) {
                var div = document.getElementById("category-dropdown")
                var opt = document.createElement('option');
                opt.value = prop;
                opt.innerHTML = prop;
                div.appendChild(opt);
            }
        }
    }
    $('.dropdown')
      .dropdown({
          onChange: function (value, text) {
              content = $('textarea#sub-categories').val();
              $.each(categories["KATEGORIER"][text], function (i, l) {
                  if ($('textarea#sub-categories:contains('+l+')').length > 0) {
                      alert('1');
                      return;
                  }
                  var item = l+"\n";
                  content = content + item;
             
              });
             
        
              $('textarea#sub-categories').val(content + "\n")
              console.log($('textarea#sub-categories').val());
            
          }
      })
    ;
});

