// wwwroot/js/chartInterop.js

window.chartInterop = {
    drawBarChart: function (canvasId, data, options) {
        var ctx = document.getElementById(canvasId).getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: data,
            options: options
        });
    },
    showElement: function (elementId) {
        var element = document.getElementById(elementId);
        if (element) {
            element.style.display = 'block';
        }
    }
};

