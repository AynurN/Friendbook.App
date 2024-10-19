var options = {
    series: [44, 55, 13],
    chart: {
        width: 320,
        type: 'pie',
    },
    legend: {
        position: 'bottom'
    },
    options: {
        legend: {
            position: 'bottom'
        }
    },
    colors: ['#0389c9', '#385cb4', '#fe4653'],
    labels: ['Positive', 'Nagative', 'Natural'],
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            },
            legend: {
                position: 'left'
            }
        },
        breakpoint: 1199,
        options: {
            legend: {
                position: 'right'
            }
        },
        breakpoint: 1461,
        options: {
            chart: {
                width: 280
            }
        }
    }]
};

var chart = new ApexCharts(document.querySelector("#chart-container"), options);
chart.render();