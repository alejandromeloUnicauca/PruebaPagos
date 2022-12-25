// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showLoader() {
    $("<div />").css({
        'position': 'fixed',
        'left': 0,
        'right': 0,
        'bottom': 0,
        'top': 0,
        'background': '#0020ff36',
        'z-index': '99',
        'text-align': 'center'
    }).appendTo($("body"))
        .append(
            $("<img />").attr("src", "https://mir-s3-cdn-cf.behance.net/project_modules/disp/35771931234507.564a1d2403b3a.gif")
        );
}