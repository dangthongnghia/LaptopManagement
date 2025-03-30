// Tính năng gallery hình ảnh
class ImageGallery {
    constructor(container) {
        this.container = container;
        this.mainImage = container.querySelector('.main-image');
        this.thumbnails = container.querySelectorAll('.thumbnail');
        this.init();
    }

    init() {
        this.thumbnails.forEach(thumb => {
            thumb.addEventListener('click', () => this.changeImage(thumb));
        });
    }

    changeImage(thumbnail) {
        // Cập nhật hình ảnh chính
        this.mainImage.src = thumbnail.dataset.fullsize || thumbnail.src;
        
        // Cập nhật trạng thái active
        this.thumbnails.forEach(t => t.classList.remove('active'));
        thumbnail.classList.add('active');
    }
}

// Khởi tạo tất cả gallery trên trang
document.addEventListener('DOMContentLoaded', function() {
    const galleries = document.querySelectorAll('.image-gallery');
    galleries.forEach(gallery => new ImageGallery(gallery));
});
