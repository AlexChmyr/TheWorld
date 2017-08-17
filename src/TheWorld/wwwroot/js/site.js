(function () {
   
    $("#sidebatToggle").on("click", function () {
        var sidebarIcon = $("#sidebatToggle i.fa");
        var sidebarAndWrapper = $("#sidebar,#wrapper");
        sidebarAndWrapper.toggleClass("hide-sidebar");

        if (sidebarAndWrapper.hasClass("hide-sidebar")) {
            sidebarIcon.removeClass("fa-angle-left");
            sidebarIcon.addClass("fa-angle-right");
        }
        else {
            sidebarIcon.removeClass("fa-angle-right");
            sidebarIcon.addClass("fa-angle-left");
        }
    })
    
})();
