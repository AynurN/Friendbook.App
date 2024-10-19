function initMap() {
    var london = {
        lat: 51.495403,
        lng: -0.150377
    };
    var map = new google.maps.Map(
        document.getElementById('map-dance'), {
            zoom: 4,
            center: london,
            styles: [{
                    "featureType": "administrative.country",
                    "elementType": "geometry.fill",
                    "stylers": [{
                            "color": "#ffffff"
                        },
                        {
                            "lightness": -5
                        }
                    ]
                },
                {
                    "featureType": "administrative.country",
                    "elementType": "geometry.stroke",
                    "stylers": [{
                        "color": "#ffffff"
                    }]
                },
                {
                    "featureType": "water",
                    "elementType": "geometry.fill",
                    "stylers": [{
                        "color": "#7ec5ff"
                    }]
                }
            ]
        });
    var marker = new google.maps.Marker({
        position: london,
        map: map,
        icon: '../assets/images/icon/marker-dance.png'
    });
}