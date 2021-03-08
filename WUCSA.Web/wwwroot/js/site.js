// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//====== Clients Logo Slider
tns({
    container: '.client-logo-carousel',
    slideBy: 'page',
    autoplay: true,
    autoplayButtonOutput: false,
    mouseDrag: true,
    gutter: 15,
    nav: false,
    controls: false,
    responsive: {
        0: {
            items: 1,
        },
        540: {
            items: 2,
        },
        768: {
            items: 3,
        },
        992: {
            items: 4,
        },
        1170: {
            items: 6,
        }
    }
});

