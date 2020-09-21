
const mySwiper = new Swiper('.gallery-main', {
    speed: 300,
    loop: true,
    lazy: true,
    spaceBetween: 10,
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
    pagination: {
        el: '.swiper-pagination',
        type: 'progressbar',
    },
});



const postMessageLister = (arg_messge) => {
    const el_swiper_wrapper = $(".swiper-wrapper");
    el_swiper_wrapper.empty();

    const file_list = JSON.parse(arg_messge)

    for (let i = 0; i < file_list.length; i++) {

        let add_slide_div = $('<div>', { class: 'swiper-slide' });
        let add_slide_img = $('<img>', { class: 'swiper-slide-image', src: 'file:///' + file_list[i] });

        add_slide_div.append(add_slide_img);
        el_swiper_wrapper.append(add_slide_div);
    }

    mySwiper.update();
    mySwiper.slideTo(1);
    
}

//.netから渡される値へのリスナー
window.chrome.webview.addEventListener('message', event => postMessageLister(event.data) );

