﻿window.addEventListener('load', function () {
    var json;
    var content = [];
   
    (function ($) {
        var url = 'http://eerie.se/textil/kategorier.json?callback=?';
        $.ajax({
            type: 'GET',
            url: url,
            async: false,
            jsonpCallback: 'jsonCallback',
            contentType: "application/json",
            dataType: 'jsonp',
            success: function (jsoncallback) {
                console.log(jsoncallback);
                json = jsoncallback;
                categoryDropDown();
                brandDropDown();
                productSearch();
           },
            error: function (e) {
                console.log(e.message);
            }
        });
    })(jQuery);
    function categoryDropDown() {
 
            var obj = json["KATEGORIER"];
            for (var prop in obj) {
                var div = document.getElementById("category-dropdown");
                var opt = document.createElement('option');
                opt.value = prop;
                opt.innerHTML = prop;
                div.appendChild(opt);
            }
        
    };
    function brandDropDown() {
 
            var obj = json["BRANDS"];
            for (var prop in obj) {
                var div = document.getElementById("brand-dropdown");
                var opt = document.createElement('option');
                opt.value = prop;
                opt.innerHTML = prop;
                div.appendChild(opt);
            }
        
    };
    function productSearch() {
        jQuery.each(json["BRANDS"], function (brandkey, brandvalue) {
            jQuery.each(brandvalue["PRODUCTS"], function (productKey, productValue) {
                content.push({
                    title: productKey,
                    description: productValue,
                    price: brandkey
                });
            }); searchUpdate();
        }); 
    };
    function searchUpdate() {
        $('#product-search')
         .search({
             onSelect: function (result, response) {
                 content = $('textarea#products').val();
                 var item = result["title"] + " : " +result["description"] + "\n";
                 content = content + item;
                 $('textarea#products').val(content);
             },
             source: content,
             searchFields: [
               'title', 'description', 'price'
             ],
             searchFullText: false
         });
        $('#free-product-search')
       .search({
           onSelect: function (result, response) {
               content = $('#free-product').val();
               var item = result["title"] + " : " + result["description"] + "\n";
               content = content + item;
               $('#free-product').val(content+", ");
           },
           source: content,
           searchFields: [
             'title', 'description', 'price'
           ],
           searchFullText: false
       });
    };

    $('#category-dropdown')
      .dropdown({
          onChange: function (value, text) {
              content = $('textarea#sub-categories').val();
              $.each(json["KATEGORIER"][text], function (i, l) {
                 
                  var item = l+"\n";
                  content = content + item;
              });
              $('textarea#sub-categories').val(content + "\n");
          }
      })
    ;
    $('#brand-dropdown')
     .dropdown({
         onChange: function (value, text) {
             content = $('textarea#brands').val();

                 var item = value + "\n";
                 content = content + value;
             $('textarea#brands').val(content + "\n");
             console.log($('textarea#brands').val());
         }
     });
});

