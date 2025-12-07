// Hide loader when page is fully loaded
window.addEventListener('load', function () {
    const loader = document.getElementById('pageLoader');
    if (loader) {
        // Add fade-out class
        loader.classList.add('fade-out');

        // Remove loader from DOM after animation completes
        setTimeout(() => {
            loader.style.display = 'none';
        }, 300);
    }
});

    // Alternative: Hide loader when DOM is ready and all scripts loaded
    // Use this if you want faster loading (before images)
    /*
    document.addEventListener('DOMContentLoaded', function() {
        // Wait a bit for initial scripts to execute
        setTimeout(() => {
            const loader = document.getElementById('pageLoader');
            if (loader) {
                loader.classList.add('fade-out');
                setTimeout(() => {
                    loader.style.display = 'none';
                }, 300);
            }
        }, 500);
    });
    */
