document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid'],
        defaultView: 'dayGridMonth',
        header: {
            center: 'addEventButton'
        },
        customButtons: {
            addEventButton: {
                text: 'add event...',
                click: function() {
                    var dateStr = prompt('Enter a date in YYYY-MM-DD format');
                    var date = new Date(dateStr + 'T00:00:00'); // will be in local time

                    if (!isNaN(date.valueOf())) { // valid?
                        calendar.addEvent({
                            title: 'dynamic event',
                            start: date,
                            allDay: true
                        });
                        alert('Great. Now, update your database...');
                    } else {
                        alert('Invalid date.');
                    }
                }
            }
        },
        defaultDate: '2020-05-12',
        events: [{
                title: 'All Day Event',
                start: '2020-05-01'
            },
            {
                title: 'Long Event',
                start: '2020-05-07',
                end: '2020-05-10'
            },
            {
                groupId: '999',
                title: 'Repeating Event',
                start: '2020-05-09T16:00:00'
            },
            {
                groupId: '999',
                title: 'Repeating Event',
                start: '2020-05-16T16:00:00'
            },
            {
                title: 'Conference',
                start: '2020-05-11',
                end: '2020-05-13'
            },
            {
                title: 'Meeting',
                start: '2020-05-12T10:30:00',
                end: '2020-05-12T12:30:00'
            },
            {
                title: 'Lunch',
                start: '2020-05-12T12:00:00'
            },
            {
                title: 'Birthday Party',
                start: '2020-05-13T07:00:00'
            },
            {
                title: 'Click for Google',
                url: 'http://google.com/',
                start: '2020-05-28'
            }
        ]
    });

    calendar.render();
});