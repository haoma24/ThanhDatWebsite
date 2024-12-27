

window.addEventListener('DOMContentLoaded', event => {
    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
         if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
             document.body.classList.toggle('sb-sidenav-toggled');
         }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }
    //Khôi phục trạng thái của thẻ a
    const anchorStates = JSON.parse(localStorage.getItem('anchorStates')) || {};
    console.log(anchorStates);
    for (const [id, isCollapse] of Object.entries(anchorStates)) {
        const anchorElement = document.getElementById(id);
        if (anchorElement) {
            anchorElement.classList.toggle('collapsed', isCollapse)
        }
    }
    const anchorElement = document.querySelectorAll('.nav-link');
    anchorElement.forEach(element => {
        element.addEventListener('click', function () {
            if ($(element).hasClass("collapsed")) {
                const id = element.getAttribute('id');
                anchorStates[id] = true;
                localStorage.setItem('anchorStates', JSON.stringify(anchorStates));
            }
            else {
                const id = element.getAttribute('id');
                anchorStates[id] = false;
                localStorage.setItem('anchorStates', JSON.stringify(anchorStates));
            }
        })
    })

    // Khôi phục trạng thái từ localStorage
    const accordionStates = JSON.parse(localStorage.getItem('accordionStates')) || {};

    for (const [id, isOpen] of Object.entries(accordionStates)) {
        const collapseElement = document.getElementById(id);
        if (collapseElement) {
            collapseElement.classList.toggle('show', isOpen);
        }
    }
    // Chọn tất cả các thẻ <a> có class "collapsed"
    const anchorElements = document.querySelectorAll('a.nav-link');

    // Duyệt qua từng thẻ <a>
    anchorElements.forEach(anchor => {
        // Lấy giá trị của thuộc tính data-bs-target
        const targetId = anchor.getAttribute('data-bs-target');

        // Kiểm tra nếu data-bs-target trỏ đến một <div> có class "show"
        const targetElement = document.querySelector(targetId);
        if (targetElement && targetElement.classList.contains('show')) {
            // Xóa class "collapsed" ở thẻ <a>
            anchor.classList.remove('collapsed');
        }
    });
    const accordionElements = document.querySelectorAll('.collapse');
    accordionElements.forEach(element => {
        element.addEventListener('shown.bs.collapse', function () {
            const id = element.getAttribute('id');
            accordionStates[id] = true;
            localStorage.setItem('accordionStates', JSON.stringify(accordionStates));
        });

        element.addEventListener('hidden.bs.collapse', function () {
            const id = element.getAttribute('id');
            const parentCollapse = element.closest('.collapse'); // Tìm accordion cha gần nhất

            // Kiểm tra nếu có accordion cha đang mở, thì giữ nguyên trạng thái của nó
            if (parentCollapse && parentCollapse.classList.contains('show')) {
                return; // Không cập nhật trạng thái nếu accordion cha đang mở
            }

            accordionStates[id] = false;
            localStorage.setItem('accordionStates', JSON.stringify(accordionStates));
        });
    });
});
