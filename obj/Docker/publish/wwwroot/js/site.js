document.addEventListener('DOMContentLoaded', function() {
    // Khởi tạo tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Khởi tạo popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function(popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Hiệu ứng fade-in cho các phần tử khi trang tải
    const fadeElements = document.querySelectorAll('.card, .detail-card, .form-group, .table');
    fadeElements.forEach((element, index) => {
        element.style.opacity = '0';
        element.style.animation = `fadeIn 0.5s ease-out ${index * 0.1}s forwards`;
    });

    // Xử lý thông báo tự động biến mất
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.opacity = '0';
            alert.style.transform = 'translateY(-20px)';
            alert.style.transition = 'opacity 0.5s, transform 0.5s';
            
            setTimeout(() => {
                if (alert.parentNode) {
                    alert.parentNode.removeChild(alert);
                }
            }, 500);
        }, 5000);
    });

    // Xử lý thumbnail gallery trong trang chi tiết
    const thumbnails = document.querySelectorAll('.thumbnail');
    const mainImage = document.querySelector('.detail-img');
    
    if (thumbnails.length > 0 && mainImage) {
        thumbnails.forEach(thumb => {
            thumb.addEventListener('click', function() {
                // Cập nhật hình ảnh chính
                mainImage.src = this.src;
                
                // Cập nhật trạng thái active
                thumbnails.forEach(t => t.classList.remove('active'));
                this.classList.add('active');
            });
        });
    }

    // Hiệu ứng hover cho các hàng trong bảng
    const tableRows = document.querySelectorAll('table tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.transition = 'background-color 0.3s';
            this.style.backgroundColor = 'rgba(58, 134, 255, 0.05)';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
        });
    });

    // Hiệu ứng cho nút
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('mousedown', function() {
            this.style.transform = 'scale(0.98)';
        });
        
        button.addEventListener('mouseup', function() {
            this.style.transform = '';
        });
        
        button.addEventListener('mouseleave', function() {
            this.style.transform = '';
        });
    });

    // Hiệu ứng sticky header cho bảng khi cuộn
    const tables = document.querySelectorAll('.table-responsive');
    tables.forEach(table => {
        const header = table.querySelector('thead');
        if (header) {
            window.addEventListener('scroll', function() {
                const tableRect = table.getBoundingClientRect();
                if (tableRect.top < 0 && tableRect.bottom > 0) {
                    header.style.position = 'sticky';
                    header.style.top = '0';
                    header.style.zIndex = '10';
                    header.style.backgroundColor = 'white';
                    header.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)';
                } else {
                    header.style.position = '';
                    header.style.top = '';
                    header.style.zIndex = '';
                    header.style.boxShadow = '';
                }
            });
        }
    });

    // Hiệu ứng đếm số cho các giá trị số
    // Hiệu ứng đếm số cho các giá trị số
const priceElements = document.querySelectorAll('.laptop-price, .price');
priceElements.forEach(element => {
    const originalText = element.textContent.trim();
    // Trích xuất số từ chuỗi (chỉ lấy các chữ số)
    const numericValue = originalText.replace(/[^\d]/g, '');
    
    if (numericValue && !isNaN(parseInt(numericValue))) {
        const finalPrice = parseInt(numericValue);
        let startPrice = 0;
        const duration = 1000;
        const frameRate = 20;
        const frames = duration / frameRate;
        const increment = finalPrice / frames;
        
        // Lấy phần tiền tệ (ví dụ: "₫")
        const currencySymbol = originalText.includes('₫') ? '₫' : '';
        
        let currentFrame = 0;
        const counter = setInterval(() => {
            currentFrame++;
            startPrice += increment;
            
            if (currentFrame === frames) {
                startPrice = finalPrice;
                clearInterval(counter);
            }
            
            // Hiển thị giá trị đã định dạng với dấu phân cách hàng nghìn
            element.textContent = Math.floor(startPrice).toLocaleString() + ' ' + currencySymbol;
        }, frameRate);
    }
});


    // Hiệu ứng smooth scroll cho các anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;
            
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                e.preventDefault();
                window.scrollTo({
                    top: targetElement.offsetTop - 100,
                    behavior: 'smooth'
                });
            }
        });
    });

    // Thêm hiệu ứng loading khi submit form
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function() {
            const submitButton = this.querySelector('button[type="submit"]');
            if (submitButton && !submitButton.classList.contains('no-loading')) {
                const originalText = submitButton.innerHTML;
                submitButton.disabled = true;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Đang xử lý...';
                
                // Lưu nội dung gốc để khôi phục nếu có lỗi
                submitButton.setAttribute('data-original-text', originalText);
                
                // Khôi phục nút sau 10 giây nếu form không được gửi thành công
                setTimeout(() => {
                    if (submitButton.disabled) {
                        submitButton.disabled = false;
                        submitButton.innerHTML = originalText;
                    }
                }, 10000);
            }
        });
    });

    // Xử lý lazy loading cho hình ảnh
    if ('loading' in HTMLImageElement.prototype) {
        // Trình duyệt hỗ trợ lazy loading
        document.querySelectorAll('img[data-src]').forEach(img => {
            img.src = img.dataset.src;
        });
    } else {
        // Trình duyệt không hỗ trợ, sử dụng Intersection Observer
        let lazyImageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    let lazyImage = entry.target;
                    lazyImage.src = lazyImage.dataset.src;
                    lazyImageObserver.unobserve(lazyImage);
                }
            });
        });

        document.querySelectorAll('img[data-src]').forEach(img => {
            lazyImageObserver.observe(img);
        });
    }

    // Xử lý dark mode (nếu cần)
    const darkModeToggle = document.getElementById('darkModeToggle');
    if (darkModeToggle) {
        // Kiểm tra trạng thái dark mode từ localStorage
        const isDarkMode = localStorage.getItem('darkMode') === 'true';
        
        // Áp dụng dark mode nếu đã được bật trước đó
        if (isDarkMode) {
            document.body.classList.add('dark-mode');
            darkModeToggle.checked = true;
        }
        
        // Xử lý sự kiện thay đổi
        darkModeToggle.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('dark-mode');
                localStorage.setItem('darkMode', 'true');
            } else {
                document.body.classList.remove('dark-mode');
                localStorage.setItem('darkMode', 'false');
            }
        });
        // Đảm bảo dropdown hoạt động
document.addEventListener('DOMContentLoaded', function() {
    // Khởi tạo tất cả các dropdown của Bootstrap
    var dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'))
    var dropdownList = dropdownElementList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl)
    });
    
    // Xử lý riêng cho userDropdown nếu cần
    var userDropdown = document.getElementById('userDropdown');
    if (userDropdown) {
        userDropdown.addEventListener('click', function(e) {
            e.stopPropagation();
            var dropdown = bootstrap.Dropdown.getInstance(userDropdown);
            if (dropdown) {
                dropdown.toggle();
            } else {
                new bootstrap.Dropdown(userDropdown).toggle();
            }
        });
    }
});

    }
});