window.onbeforeunload = function() {
    var isRememberMe = localStorage.getItem('isRememberMe');
    if (isRememberMe === 'false') {
        var theme = localStorage.getItem('theme');
        localStorage.clear();
        if (theme) {
            localStorage.setItem('theme', theme);
        }
    }
}